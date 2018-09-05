using System;
using System.Collections.Generic;
using static AutoTester.Annotations;

namespace SampleAssembly
{
    public class Class1
    {

        public Class1() { }

        [TestCaseValues(Inputs = new object[] { 1, 2 }, Output = 3)]
        [TestCaseValues(Inputs = new object[] { 3, 4 }, Output = 7)]
        public int Sum(int a, int b) {
            return a + b;
        }


        [TestCaseValues(Inputs = new object[] { 1, 2 }, Output = 2)]
        [TestCaseValues(Inputs = new object[] { 3, 4 }, Output = 12)]
        public int Multiply(int a, int b) {
            return a * b;
        }


        [TestCaseValues(Inputs = new object[] { "a", 1 }, Output = true)]
        [TestCaseValues(Inputs = new object[] { "aa", 3 }, Output = false)]
        public bool StringLengthMatch(string str, int count) {
            return str.Length == count;
        }


        public bool NameLengthMatch(Client str, int count) {
            return str.Name.Length == count;
        }

    }


    public class Client {

        public string Name { get; set; }

        public Client(string name) {
            Name = name;
        }
    }
}
