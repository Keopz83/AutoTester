using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTester {

    public class TestSuite {

        public Dictionary<string, List<TestCase>> TestCases { get; set; } = new Dictionary<string, List<TestCase>>();

        public void Add(string methodName, params TestCase[] testCases) {

            if (!TestCases.ContainsKey(methodName)) {
                TestCases[methodName] = new List<TestCase>();
            }

            TestCases[methodName].AddRange(testCases);
        }
    }
}
