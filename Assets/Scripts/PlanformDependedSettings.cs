using OnePF;

namespace Assets.Scripts
{
    #if UNITY_ANDROID

    public static class PlanformDependedSettings
    {
        public static string StoreName = OpenIAB_Android.STORE_GOOGLE;
        public const string StorePublicKey = "";
    }

    #endif

    #if UNITY_IPHONE

    public static class PlanformDependedSettings
    {
        public static string StoreName = OpenIAB_iOS.STORE;
        public const string StorePublicKey = "";
    }

    #endif
}