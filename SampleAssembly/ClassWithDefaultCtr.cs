namespace SampleAssembly {

    public class ClassWithDefaultCtr {

        //TODO: decide what to do with setter and getter methods (ignore or allow?)
        public string StrProp { get; set; }

        public ClassWithDefaultCtr() {}

        public ClassWithDefaultCtr(string name) {
            StrProp = name;
        }
    }
}
