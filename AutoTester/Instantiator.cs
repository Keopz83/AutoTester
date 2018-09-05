using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AutoTester {

    public class Instantiator {

        public static object InstanstiateClass(Type type) {
            var obj = FormatterServices.GetUninitializedObject(type); //does not call ctor
            return obj;
        }
    }
}
