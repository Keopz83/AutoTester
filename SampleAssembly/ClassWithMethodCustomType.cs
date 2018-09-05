using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleAssembly {

    public class ClassWithMethodCustomType {

        //TODO: find a way to define custom type test inputs.
        //TODO: what about custom types, that contain custom types, that contain custom types...
        public bool MethodWithCustomType(ClassWithDefaultCtr customType, int count) {
            return customType.StrProp.Length == count;
        }
    }
}
