using AutoTester;
using System;

namespace SampleAssembly._AutoTests {

    public class Program {

        static void Main(string[] args) {

            var testSuite = new TestSuite();

            testSuite.Add(
                methodName: nameof(SampleAssembly.ClassWithMethodsDefaultTypes.SumTwoInts), 
                testCases: new TestCase() {
                    Inputs = new object[] { 2, 3 },
                    Output = 5
                }
            );

            testSuite.Add(
                methodName: nameof(SampleAssembly.ClassWithMethodsDefaultTypes.MultiplyTwoInts),
                testCases: new TestCase() {
                    Inputs = new object[] { 2, 3 },
                    Output = 6
                }
            );

            testSuite.Add(
                methodName: nameof(SampleAssembly.ClassWithMethodsDefaultTypes.StringLengthMatch),
                testCases: new TestCase() {
                    Inputs = new object[] { "abc", 3 },
                    Output = true
                }
            );


            var customType = new ClassWithDefaultCtr() { StrProp = "abc" };
            testSuite.Add(
                methodName: nameof(SampleAssembly.ClassWithMethodCustomType.MethodWithCustomType),
                testCases: new TestCase() {
                    Inputs = new object[] { customType, 3 },
                    Output = true
                }
            );

            var projectFilePath = @"C:\Users\user1\source\repos\AutoTester\SampleAssembly\SampleAssembly.csproj";

            var results = AssemblyTester.Run(projectFilePath, testSuite);

            Console.WriteLine($"Test run global result: '{results}'");

            Console.ReadLine();
        }
    }

}
