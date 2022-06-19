#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("h5FUQTOcg/JV1qrUrIEKitbwTsMkmIbXHpuM9GOOvrnPpGWszvHEUR6dk5ysHp2Wnh6dnZwBCdJ1oZSNjQpvQLO7yYeC9qARCGfzBOfVW9btmYQk2HrHYxRBKqhYFrON9Pn4Sd4cypmlm53isv+8WM1lmWtIxqx4tLaNFrNZBVLdaEq2hw9/L1aQZFf2fApgufQ8rxJrhdmNMVVWAhI+uW+qxNfTEBchMVlVAxF9j+L15f3wlfQw0EHC0KmoKOag47npmGGxlpWsHp2+rJGalbYa1BprkZ2dnZmcn3wdEIaY19ujTSlUnwEFOHinteJdv6leD0+CfOfEFpMFJ7SYcHZbZCk2wwE9I20zSPfWKWi8akklh9bi3z0x2X5afpmj7Z6fnZyd");
        private static int[] order = new int[] { 6,10,10,4,7,7,12,9,10,13,12,11,12,13,14 };
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
