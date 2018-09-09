using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Serialization;
using static AutoTester.Annotations.Annotations;
using Microsoft.Build.Evaluation;
using System.IO;
using AutoTester.CSharpAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoTester
{

    public class AssemblyTester
    {
        public static object Run() {
            throw new NotImplementedException();
        }

        public static TestResults TestAll(string assemblyName) {

            Assembly assembly = Assembly.ReflectionOnlyLoad(assemblyName);

            var passed = true;
            
            foreach (Type type in assembly.GetTypes()) {

                var runType = Type.GetType(type.AssemblyQualifiedName);

                Console.WriteLine(type.Name);

                foreach (MethodInfo methodInfo in (runType as TypeInfo).DeclaredMethods) {

                    var testCases = methodInfo.GetCustomAttributes().Cast<ITestCase>();
                    var result = TestMethod(runType, methodInfo, testCases);
                    if (!result)
                        passed = false;

                }

                Console.WriteLine();
            }

            return new TestResults() { Succeeded = passed };
        }


        public static bool TestMethod(Type runType, MethodInfo methodInfo, IEnumerable<ITestCase> testCases) {


            var testsPassed = true;


            //Create type instance
            object typeInstance;
            try {
                // Following does not run constructor.
                // Otherwise use instead Activator.CreateInstance(runType);
                typeInstance = Instantiator.InstanstiateClass(runType); 
            }
            catch (Exception e) {
                Console.WriteLine($"Cannot create instance of type '{runType.FullName}'.");
                Console.WriteLine(e.ToString());
                return false;
            }


            Console.WriteLine();
            PrintMethoSignature(methodInfo);

            Console.WriteLine();

            if (testCases.Count() == 0) {
                Console.WriteLine("\t\t-- No test cases defined. --");
                return false;
            }

            //Execute test cases
            for (var testCase = 0; testCase < testCases.Count(); testCase++) {

                if (methodInfo.GetParameters().Count() == 0) {
                    Console.WriteLine("\t\t-- Method receives no inputs. --");
                    continue;
                }
                

                var testCaseValues = testCases.ElementAt(testCase);
                if (testCaseValues == null) {
                    Console.WriteLine("\t\t-- No test cases defined. --");
                    continue;
                }
                Console.WriteLine($"\t\tTest case {testCase}:");

                var testInputValues = testCaseValues.Inputs.ToArray();
                var expectedOutput = testCaseValues.Output;

                Console.WriteLine($"\t\t\tInput values: {PrintValues(testInputValues)}");
                Console.WriteLine($"\t\t\tOutput values: {PrintValues(expectedOutput)}");

                var result = methodInfo.Invoke(typeInstance, testInputValues);
                try {
                    Assert.AreEqual(expectedOutput, result, "Unexpected output.");
                    PrintSuccess("\t\t\tTest passed.");
                }
                catch (Exception e) {
                    PrintFail("\t\t\tTest failed.");
                    Console.WriteLine(e.Message);
                    testsPassed = false;
                }

            }

            return testsPassed;

        }


        public static void PrintSuccess(string message) {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(message);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }


        public static void PrintFail(string message) {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(message);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }


        public static object Run(string projectFilePath, TestSuite testSuite) {

            Console.WriteLine("\n##########################");
            Console.WriteLine("####### AutoTester #######");
            Console.WriteLine("##########################\n");

            Console.WriteLine($"Running test suite for project '{projectFilePath}'...");

            
            var project = new Project(projectFilePath);
            var assemblyName = project.Properties.Where(x => x.Name.Equals("MSBuildProjectName")).First().EvaluatedValue;
            Assembly assembly = Assembly.ReflectionOnlyLoad(assemblyName);
            var assemblyTypes = assembly.GetTypes();
            

            var csFiles = project.AllEvaluatedItems.Where(x => x.ItemType.Equals("Compile")).Select(x => x.EvaluatedInclude);



            var rootFolder = Path.GetDirectoryName(projectFilePath);

            bool allTestsPassed = true;

            foreach(var csFileName in csFiles) {

                var csFilePath = Path.Combine(rootFolder, csFileName);

                Console.WriteLine($"\nParsing CS document '{csFilePath}'...");
                var csDoc = new CSharpDocument(csFilePath);
                var csDocClasses = csDoc.Classes;
                if(csDocClasses == null || !csDocClasses.Any()) {
                    PrintWarning($"Warning: file '{csFilePath}' contains no classes.");
                    continue;
                }

                foreach(var classDecl in csDocClasses) {

                    var reflectionType = assemblyTypes.Where(x => x.Name.Equals(classDecl.Identifier.ValueText)).FirstOrDefault();
                    var runType = Type.GetType(reflectionType.AssemblyQualifiedName);

                    Console.WriteLine($"\nClass: '{classDecl.Identifier.ValueText}':");

                    var methods = classDecl.GetMethods();
                    if (!methods.Any()) {
                        Console.WriteLine("\tClass contains no methods.");
                        continue;
                    }

                    foreach(var method in methods) {
                        Console.WriteLine($"\tMethod: '{method.Identifier.ValueText}':");

                        if (!testSuite.TestCases.ContainsKey(method.Identifier.ValueText)) {
                            PrintWarning($"\t\tWarning: no test case defined for {classDecl.Identifier.ValueText}.{method.Identifier.ValueText}");
                        }
                        else {
                            //apply test case
                            Console.WriteLine($"\t\tApplying test case for {classDecl.Identifier.ValueText}.{method.Identifier.ValueText}...");

                            var methodsInfo = (runType as TypeInfo).DeclaredMethods;
                            var methodInfo = methodsInfo.Where(x => x.Name.Equals(method.Identifier.ValueText)).FirstOrDefault();

                            var result = TestMethod(runType, methodInfo, testSuite.TestCases[method.Identifier.ValueText].Cast<ITestCase>());

                            if (!result)
                                allTestsPassed = false;
                        }
                        
                    }
                    
                }
                
            }

            return allTestsPassed;
        }


        private static void PrintMethoSignature(MethodInfo methodInfo) {
            var argumentTokensStr = methodInfo.GetParameters().Select(x => x.ParameterType.Name + ": " + x.Name);
            var argumentsStr = string.Join(", ", argumentTokensStr);
            Console.WriteLine($"\t{methodInfo.Name}({argumentsStr})");
        }


        private static string PrintValues(params object[] values) {
            return string.Join(", ", values.Select(x => x.ToString()));
        }


        public static void PrintWarning(string message) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
