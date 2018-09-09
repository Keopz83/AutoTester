using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTester.Annotations {

    public static class Annotations {

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
        public class TestCaseAttribute : Attribute, ITestCase {

            public object[] Inputs { get; set; }

            public object Output { get; set; }
        }


        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
        public class TargetClass : Attribute {

            public Type Type { get; set; }

        }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
        public class TargetMethod : Attribute {

            public string Name { get; set; }

        }

    }
}
