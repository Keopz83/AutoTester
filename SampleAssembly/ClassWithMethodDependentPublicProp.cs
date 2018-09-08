using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleAssembly {

    
    public class ClassWithMethodDependentPublicProp {

        public bool InfluencerPublicProp { get; set; }

        //TODO: test MethodInfluencedByPublicProp
        public bool MethodInfluencedByPublicProp(bool outProp) {

            bool internalVar = false;

            if(outProp && InfluencerPublicProp && internalVar) {
                return true;
            }

            return false;
        }
    }
}
