﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using static AutoTester.Annotations;
using System.Runtime.Serialization;

namespace AutoTester
{

    public class AssemblyTester
    {


        public static TestResults TestAll(string assemblyName) {

            Assembly assembly = Assembly.ReflectionOnlyLoad(assemblyName);

            var passed = true;
            
            foreach (Type type in assembly.GetTypes()) {

                var runType = Type.GetType(type.AssemblyQualifiedName);
                object typeInstance;
                try {
                    typeInstance = Instantiator.InstanstiateClass(runType); // Activator.CreateInstance(runType);
                } catch(Exception e) {
                    Console.WriteLine($"Cannot create instance of type '{runType.FullName}'.");
                    Console.WriteLine(e.ToString());
                    passed = false;
                    continue;
                }

                Console.WriteLine(type.Name);

                foreach (MethodInfo methodInfo in (runType as TypeInfo).DeclaredMethods) {

                    Console.WriteLine();
                    PrintMethoSignature(methodInfo);

                    Console.WriteLine();

                    
                    for(var testCase = 0; testCase < methodInfo.GetCustomAttributes().Count(); testCase++) {

                        if(methodInfo.GetParameters().Count() == 0) {
                            Console.WriteLine("\t\t-- Method receives no inputs. --");
                            continue;
                        }

                        if (methodInfo.GetCustomAttributes().Count() == 0) {
                            Console.WriteLine("\t\t-- No test cases defined. --");
                            continue;
                        }

                        var testCaseValues = (methodInfo.GetCustomAttributes().ElementAt(testCase) as TestCaseValues);
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
                            Console.WriteLine("\t\t\tTest passed.");
                        }
                        catch (Exception e) {
                            Console.WriteLine(e.Message);
                            passed = false;
                        }
                    }

                }

                Console.WriteLine();
            }

            return new TestResults() { Succeeded = passed };
        }

        private static void PrintMethoSignature(MethodInfo methodInfo) {
            var argumentTokensStr = methodInfo.GetParameters().Select(x => x.ParameterType.Name + ": " + x.Name);
            var argumentsStr = string.Join(", ", argumentTokensStr);
            Console.WriteLine($"\t{methodInfo.Name}({argumentsStr})");
        }

        private static string PrintValues(params object[] values) {
            return string.Join(", ", values.Select(x => x.ToString()));
        }

        //public Dictionary<string, Tuple<object[], object>> TestValues = new Dictionary<string, Tuple<object[], object>>() {
        //    {nameof(Class1.NameLengthMatch), new Tuple<object[], object>(new object[] { new Client("Jose"), 4 }, true) }
        //};

        //public object[] GetInputs(string methodName) {
        //    switch (methodName) {
        //        case nameof(NameLengthMatch):
        //            return new object[] { new Client("Jose"), 3 };
        //        default:
        //            return null;
        //    }
        //}
    }
}