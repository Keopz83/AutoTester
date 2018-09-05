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
        public void TestAll_suceeds() {

            var testResults = TestAssembly.TestAll("SampleAssembly");
            Assert.IsTrue(testResults.Succeeded, "Expects test to succeed.");
        }
    }
}
