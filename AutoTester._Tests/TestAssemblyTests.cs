using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTester._Tests
{
    [TestClass]
    public class TestAssemblyTests
    {


        [TestMethod]
        public void CanInstanstiateClassWithoutDefaultCtr() {

            var expectedType = typeof(SampleAssembly.ClassWithoutDefaultCtr);
            var obj = Instantiator.InstanstiateClass(typeof(SampleAssembly.ClassWithoutDefaultCtr));
            Assert.AreEqual(expectedType, obj.GetType());
        }



        [TestMethod]
        public void TestAll_suceeds() {

            var testResults = AssemblyTester.TestAll("SampleAssembly");
            Assert.IsTrue(testResults.Succeeded, "Expects test to succeed.");
        }
    }
}
