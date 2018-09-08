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

        public IEnumerable<NamespaceDeclarationSyntax> Namespaces { get; }

        public IEnumerable<ClassDeclarationSyntax> Classes { get; }

        public CSharpDocument(string csFilePath) {

            //CS document tree
            var srcText = File.ReadAllText(csFilePath);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(srcText);
            Assert.IsNotNull(tree, "Expects non null SyntaxTree.");

            CompilationUnit = tree.GetRoot() as CompilationUnitSyntax;
            Assert.IsNotNull(CompilationUnit, "Expects non null CompilationUnitSyntax.");

            //Namespace
            Namespaces = CompilationUnit.Members.OfType<NamespaceDeclarationSyntax>();
            Assert.IsTrue(Namespaces.Any(), "Expects at least 1 namespace.");
            var classes = new List<ClassDeclarationSyntax>();
            foreach (var nameSpace in Namespaces) {
                
                Console.WriteLine($"Namespace '{nameSpace.Name}':");

                //Classes
                var nameSpaceClasses = nameSpace.Members.OfType<ClassDeclarationSyntax>();
                Assert.IsNotNull(classes, "Expects non null ClassDeclarationSyntax list.");
                classes.AddRange(nameSpaceClasses);
            }
            Classes = classes;
        }


        public IEnumerable<string> GetExternalVars(string className, string methodName) {

            ClassDeclarationSyntax classDcl = GetClass(className);
            Console.WriteLine($"Class '{classDcl.Identifier.Value}':");

            MethodDeclarationSyntax methodDcl = classDcl.GetMethod(methodName);
            return methodDcl.GetMethodExternalVars();
        }


        public ClassDeclarationSyntax GetClass(string className) {
            var classDecl = Classes.Where(x => x.Identifier.ValueText.Equals(className)).FirstOrDefault();
            Assert.IsNotNull(classDecl, $"Could not find class with name '{className}'.");
            return classDecl;
        }


        public IEnumerable<MethodDeclarationSyntax> AllMethods => Classes.SelectMany(x => x.GetMethods());


        public IEnumerable<MethodDeclarationSyntax> GetMethodsWithExternalVars(string className) {

            return AllMethods.Where(x => x.GetMethodExternalVars().Any());
        }

    }
}
