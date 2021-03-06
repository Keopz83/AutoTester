﻿using System;
using System.Collections.Generic;
using static AutoTester.Annotations.Annotations;

namespace SampleAssembly {

    /// <summary>
    /// Class with a default Ctr. and only default types.
    /// </summary>
    public class ClassWithMethodsDefaultTypes
    {

        public ClassWithMethodsDefaultTypes() { }

        //TODO: remove notifications from code (use a file with testcases instead, which template is generated from code?)
        [TestCaseAttribute(Inputs = new object[] { 1, 2 }, Output = 3)]
        [TestCaseAttribute(Inputs = new object[] { 3, 4 }, Output = 7)]
        public int SumTwoInts(int a, int b) {
            return a + b;
        }


        [TestCaseAttribute(Inputs = new object[] { 1, 2 }, Output = 2)]
        [TestCaseAttribute(Inputs = new object[] { 3, 4 }, Output = 12)]
        public int MultiplyTwoInts(int a, int b) {
            return a * b;
        }


        [TestCaseAttribute(Inputs = new object[] { "a", 1 }, Output = true)]
        [TestCaseAttribute(Inputs = new object[] { "aa", 3 }, Output = false)]
        public bool StringLengthMatch(string str, int count) {
            return str.Length == count;
        }


    }
}
