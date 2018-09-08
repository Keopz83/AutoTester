using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTester {


    public static class RoslynExtensions {


        public static MethodDeclarationSyntax GetMethod(this ClassDeclarationSyntax classDecl, string methodName) {
            var methods = classDecl.Members.OfType<MethodDeclarationSyntax>();
            Assert.IsNotNull(methods, "Expects non null MethodDeclarationSyntax list.");
            return methods.Where(x => x.Identifier.ValueText.Equals(methodName)).FirstOrDefault();
        }


        public static IEnumerable<MethodDeclarationSyntax> GetMethods(this ClassDeclarationSyntax classDecl) {
            return classDecl.Members.OfType<MethodDeclarationSyntax>();
        }


        public static IEnumerable<string> GetMethodExternalVars(this MethodDeclarationSyntax method) {

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
