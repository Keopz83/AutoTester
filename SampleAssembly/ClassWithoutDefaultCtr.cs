namespace SampleAssembly {

    public class ClassWithoutDefaultCtr {

        public string StrProp { get; set; }

        public ClassWithoutDefaultCtr(string name) {
            StrProp = name;
        }
    }
}
