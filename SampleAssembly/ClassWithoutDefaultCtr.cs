namespace SampleAssembly {

    public class ClassWithoutDefaultCtr {

        public string StrProp { get; set; }

        //TODO: decide if should test constructors.
        public ClassWithoutDefaultCtr(string name) {
            StrProp = name;
        }
    }
}
