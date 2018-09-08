using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;

namespace AutoTester._Tests
{
    [TestClass]
    public class TestAssemblyTests
    {

        [TestMethod]
        public void CanFindVariableExternalFromMethod() {

            //Arrange
            var sourceFolder = @"C:\Users\user1\source\repos\AutoTester\SampleAssembly";
            var csFileName = "ClassWithMethodDependentPublicProp.cs";
            var className = Path.GetFileNameWithoutExtension(csFileName);
            var methodName = "MethodInfluencedByPublicProp";
            var csFilePath = Path.Combine(sourceFolder, csFileName);
            var compUnit = new CSharpDocument(csFilePath);

            //Act
            var externalVars = compUnit.GetExternalVars(className, methodName);

            //Assert
            Assert.AreEqual(1, externalVars.Count(), "Unexpected number of external variables.");
            Assert.AreEqual("InfluencerPublicProp", externalVars.First(), "Unexpected external variable name.");
        }


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
