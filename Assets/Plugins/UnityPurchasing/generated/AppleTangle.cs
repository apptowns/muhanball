#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class AppleTangle
    {
        private static byte[] data = System.Convert.FromBase64String("2eKD0PfMCt0VWOj+l4wf3RuvFh2wvP/57uj1+vX//ej5vOzz8PX/5eW8/e/v6fH577z9///57Oj98v/5iqyImp/JmJ+Pkd3s7PD5vM7z8+g3P+0O28/JXTOz3S9kZ3/sUXo/0FWF7mnBkknjwwduuZ8myRPRwZFtImjvB3JO+JNX5dOoRD6iZeRj91QtrMRwxpiuEPQvE4FC+e9j+8L5IJTCrB6djZqfyYG8mB6dlKwenZisE+8d/FqHx5WzDi5k2NRs/KQCiWm2GtQaa5GdnZmZnKz+rZeslZqfyZMBoW+31bSGVGJSKSWSRcKASlehHIi3TPXbCOqVYmj3EbLcOmvb0ePVROoDr4j5PesIVbGen52cnT8enSmmMWiTkpwOly29irLoSaCRR/6Kzvnw9f3y//m88/K86PT177z/+e66rLian8mYl4+B3ezs8Pm83/nu6CuHIQ/euI62W5OBKtEAwv9U1xyLrB6YJ6wenz88n56dnp6dnqyRmpXs8Pm8zvPz6Lzf3ayCi5GsqqyorvD5vNXy/7Ktuqy4mp/JmJePgd3s7v3/6PX/+bzv6P3o+fH58ujvsqxc/6/ra6absMp3RpO9kkYm74XTKamuraisr6rGi5GvqayurKWuraisNEDivqlWuUlFk0r3SD64v41rPTDr67L97Ozw+bL/8/Gz/ezs8Pn//bOsHV+alLeanZmZm56erB0qhh0v8vi8//Py+PXo9fPy77zz+rzp7/n1+vX//ej18/K83eno9PPu9ejlrR6dnJqVthrUGmv/+JmdrB1urLaa+Km/ideJxYEvCGtqAAJTzCZdxMyYmo+eyc+tj6yNmp/JmJaPlt3s7K+qxqz+rZeslZqfyZiaj57Jz62P+xOUKLxrVzCwvPPsKqOdrBAr31O88/q86PT5vOj0+fK8/ezs8PX//UWq410byUU7BSWu3mdESe0C4j3Omp/JgZKYipiIt0z12wjqlWJo9xGhuvu8Fq/2a5EeU0J3P7Nlz/bH+Lh+d00r7EOT2X27Vm3x5HF7KYuLm3DhpR8Xz7xPpFgtIwbTlvdjt2C8392sHp2+rJGalbYa1BprkZ2dnbz98vi8//nu6PX69f/96PXz8rzs4900BGVNVvoAuPeNTD8neIe2X4Po9fr1//3o+bz+5bz98uW87P3u6MU7mZXgi9zKjYLoTysXv6fbP0nzgxkfGYcFodurbjUH3BKwSC0MjkSqBdCx5CtxEAdAb+sHbupO66zTXbLcOmvb0eOUwqyDmp/Jgb+YhKyKlLeanZmZm56dioL06Ojs76azs+usjZqfyZiWj5bd7Ozw+bzV8v+yrej08+716OWtiqyImp/JmJ+Pkd3s5qweneqskpqfyYGTnZ1jmJifnp0XhRVCZdfwaZs3vqyedISiZMyVTwkC5pA42xfHSIqrr1dYk9FSiPVNmqyTmp/JgY+dnWOYmayfnZ1jrIGDDUeC28x3mXHC5Rixd6o+y9DJcP7w+bzv6P3y+P3u+Lzo+e7x77z97PD5vN/57uj1+vX//ej18/K83emRmpW2GtQaa5GdnZmZnJ8enZ2cwJmcnx6dk5ysHp2Wnh6dnZx4DTWVzDYWSUZ4YEyVm6ss6em9");
        private static int[] order = new int[] { 25,44,40,12,24,19,50,26,33,55,44,22,32,27,23,38,44,59,25,50,32,46,57,26,54,36,37,43,57,34,32,40,33,46,41,58,46,56,53,51,43,45,59,51,52,49,48,51,58,55,50,57,52,58,55,58,59,58,58,59,60 };
        private static int key = 156;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
