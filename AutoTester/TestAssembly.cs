using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SampleAssembly;
using System.ComponentModel;
using static AutoTester.Annotations;

namespace AutoTester
{

    public class TestAssembly
    {

        public static void TestAll(string assemblyName) {

            Assembly assembly = Assembly.ReflectionOnlyLoad("SampleAssembly");

            foreach (Type type in assembly.GetTypes()) {

                var runType = Type.GetType(type.AssemblyQualifiedName);
                var typeInstance = Activator.CreateInstance(runType);

                Console.WriteLine(type.Name);

                foreach(MethodInfo methodInfo in (runType as TypeInfo).DeclaredMethods) {

                    Console.WriteLine();
                    PrintMethoSignature(methodInfo);

                    Console.WriteLine();

                    var passed = true;
                    for(var testCase = 0; testCase < methodInfo.GetCustomAttributes().Count(); testCase++) {

                        Console.WriteLine($"\t\tTest case {testCase}:");

                        var testCaseValues = (methodInfo.GetCustomAttributes().ElementAt(testCase) as TestCaseValues);
                        var testInputValues = testCaseValues.Inputs.ToArray();
                        var expectedOutput = testCaseValues.Output;

                        Console.WriteLine($"\t\t\tInput values: {PrintValues(testInputValues)}");
                        Console.WriteLine($"\t\t\tOutput values: {PrintValues(expectedOutput)}");

                        var result = methodInfo.Invoke(typeInstance, testInputValues);
                        try {
                            Assert.AreEqual(expectedOutput, result, "Unexpected output.");
                            Console.WriteLine("\t\t\tTest passed.");
                        }
                        catch (Exception e) {
                            Console.WriteLine(e.Message);
                            passed = false;
                        }
                    }

                    if (!passed) {
                        Assert.Fail();
                    }
                }

                Console.WriteLine();
            }

        }

        private static void PrintMethoSignature(MethodInfo methodInfo) {
            var argumentTokensStr = methodInfo.GetParameters().Select(x => x.ParameterType.Name + ": " + x.Name);
            var argumentsStr = string.Join(", ", argumentTokensStr);
            Console.WriteLine($"\t{methodInfo.Name}({argumentsStr})");
        }

        private static string PrintValues(params object[] values) {
            return string.Join(", ", values.Select(x => x.ToString()));
        }
    }
}
