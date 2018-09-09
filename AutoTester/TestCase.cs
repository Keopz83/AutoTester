using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTester {

    public interface ITestCase {

        object[] Inputs { get; set; }

        object Output { get; set; }
    }


    public class TestCase: ITestCase {

        public object[] Inputs { get; set; }

        public object Output { get; set; }
    }
}
