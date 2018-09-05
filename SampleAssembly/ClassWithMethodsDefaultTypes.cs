using System;
using System.Collections.Generic;
using static AutoTester.Annotations;

namespace SampleAssembly {

    /// <summary>
    /// Class with a default Ctr. and only default types.
    /// </summary>
    public class ClassWithMethodsDefaultTypes
    {

        public ClassWithMethodsDefaultTypes() { }

        [TestCaseValues(Inputs = new object[] { 1, 2 }, Output = 3)]
        [TestCaseValues(Inputs = new object[] { 3, 4 }, Output = 7)]
        public int SumTwoInts(int a, int b) {
            return a + b;
        }


        [TestCaseValues(Inputs = new object[] { 1, 2 }, Output = 2)]
        [TestCaseValues(Inputs = new object[] { 3, 4 }, Output = 12)]
        public int MultiplyTwoInts(int a, int b) {
            return a * b;
        }


        [TestCaseValues(Inputs = new object[] { "a", 1 }, Output = true)]
        [TestCaseValues(Inputs = new object[] { "aa", 3 }, Output = false)]
        public bool StringLengthMatch(string str, int count) {
            return str.Length == count;
        }


    }
}
