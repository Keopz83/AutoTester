using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTester {

    public static class Annotations {

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
        public class TestCaseValues : Attribute {

            public object[] Inputs { get; set; }

            public object Output { get; set; }
        }


    }
}
