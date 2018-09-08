using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AutoTester {


    public class CSharpDocument {

        public CompilationUnitSyntax CompilationUnit { get; }

        public CSharpDocument(string csFilePath) {

            //CS document tree
            var srcText = File.ReadAllText(csFilePath);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(srcText);
            Assert.IsNotNull(tree, "Expects non null SyntaxTree.");

            CompilationUnit = tree.GetRoot() as CompilationUnitSyntax;
            Assert.IsNotNull(CompilationUnit, "Expects non null CompilationUnitSyntax.");
        }


        public IEnumerable<string> GetExternalVars(string className, string methodName) {

            //Namespace
            var nameSpaceSyntax = CompilationUnit.Members.OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
            Assert.IsNotNull(nameSpaceSyntax, "Expects non null NamespaceDeclarationSyntax.");
            Console.WriteLine($"Namespace '{nameSpaceSyntax.Name}':");


            //Classes
            var classes = nameSpaceSyntax.Members.OfType<ClassDeclarationSyntax>();
            Assert.IsNotNull(classes, "Expects non null ClassDeclarationSyntax list.");
            var chosenClass = classes.Where(x => x.Identifier.ValueText.Equals(className)).FirstOrDefault();
            Assert.IsNotNull(chosenClass, $"Could not find class with name '{className}'.");
            Console.WriteLine($"Class '{chosenClass.Identifier.Value}':");


            //Methods
            var methods = chosenClass.Members.OfType<MethodDeclarationSyntax>();
            Assert.IsNotNull(methods, "Expects non null MethodDeclarationSyntax list.");
            var chosenMethod = methods.Where(x => x.Identifier.ValueText.Equals(methodName)).FirstOrDefault();

            return GetMethodExternalVars(chosenMethod);
        }


        public static IEnumerable<string> GetMethodExternalVars(MethodDeclarationSyntax method) {

            //Method arguments
            var arguments = method.ParameterList.Parameters.Select(x => x.Identifier.Value);


            //Method statements
            Console.WriteLine($"Method '{method.Identifier.Value}':");
            var methodBody = method.Body;
            foreach (var statement in methodBody.Statements) {

                Console.WriteLine("\t--------body statement-----------");
                //Console.WriteLine(statement.ToString());

                var variables = statement.DescendantNodes().OfType<IdentifierNameSyntax>().Distinct();
                foreach (IdentifierNameSyntax variable in variables) {

                    var externalVar = !arguments.Contains(variable.Identifier.Value);

                    Console.WriteLine($"\t\tVar: '{variable}'. External: '{externalVar}'");

                    if (externalVar) {
                        yield return variable.ToString();
                    }

                }

                Console.WriteLine("\t---------------------------------");
            }
        }
    }
}
