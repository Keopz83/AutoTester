using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleAssembly {


    public class ClassWithMethodDependentPrivateProp {

        private bool _influencerPrivateVariable;

        public ClassWithMethodDependentPrivateProp(bool influencerPrivateVariable) {
            _influencerPrivateVariable = influencerPrivateVariable;
        }

        //TODO: check unit test coverage by method
        //TODO: find method "leaks" - variables that are from outside method scope
        //TODO: test MethodInfluencedByPrivateProp
        public bool MethodInfluencedByPrivateProp(bool outProp) {

            if(outProp && _influencerPrivateVariable) {
                return true;
            }

            return false;
        }
    }
}
