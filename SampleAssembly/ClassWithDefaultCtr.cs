namespace SampleAssembly {

    public class ClassWithDefaultCtr {

        public string StrProp { get; set; }

        public ClassWithDefaultCtr() {}

        public ClassWithDefaultCtr(string name) {
            StrProp = name;
        }
    }
}
