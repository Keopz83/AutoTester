using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTester {


    public static class RoslynExtensions {


        public static IEnumerable<string> GetMethodExternalVars(this MethodDeclarationSyntax method) {

            var methodParameters = method.GetParameters().Select(x => x.Identifier.Value);
            var variablesInBody = method.GetMentionedVariables();
            var varsDefinedInBody = method.GetVariableDeclarations().Select(x => x.Identifier.ValueText);
            var externalVariables = variablesInBody.Where(x => {
                var isInParameters = methodParameters.Contains(x.Identifier.Value);
                var isDefinedLocally = varsDefinedInBody.Contains(x.Identifier.Value);
                return !isInParameters && !isDefinedLocally;
            });
            return externalVariables.Select(x => x.Identifier.ValueText);

        }

        public static MethodDeclarationSyntax GetMethod(this ClassDeclarationSyntax classDecl, string methodName) {
            var methods = classDecl.Members.OfType<MethodDeclarationSyntax>();
            Assert.IsNotNull(methods, "Expects non null MethodDeclarationSyntax list.");
            return methods.Where(x => x.Identifier.ValueText.Equals(methodName)).FirstOrDefault();
        }

        public static IEnumerable<MethodDeclarationSyntax> GetMethods(this ClassDeclarationSyntax classDecl) {
            return classDecl.Members.OfType<MethodDeclarationSyntax>();
        }


        public static IEnumerable<VariableDeclaratorSyntax> GetVariableDeclarations(this MethodDeclarationSyntax method) {
            return method.Body.DescendantNodes().OfType<VariableDeclaratorSyntax>();
        }

        public static IEnumerable<IdentifierNameSyntax> GetMentionedVariables(this MethodDeclarationSyntax method) {
            return method.Body.DescendantNodes().OfType<IdentifierNameSyntax>();
        }

        public static IEnumerable<ParameterSyntax> GetParameters(this MethodDeclarationSyntax method) {
            return method.ParameterList.Parameters;
        }

    }
}
