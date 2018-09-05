using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleAssembly {

    public class ClassWithMethodCustomType {

        public bool MethodWithCustomType(ClassWithDefaultCtr customType, int count) {
            return customType.StrProp.Length == count;
        }
    }
}
