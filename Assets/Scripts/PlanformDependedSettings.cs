using OnePF;

namespace Assets.Scripts
{
    public static class PlanformDependedSettings
    {

        #if UNITY_ANDROID

        public static string StoreName = OpenIAB_Android.STORE_GOOGLE;
        public const string StorePublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAmbpWl+DluEA8A/EptHdKLcYAvxAohkyLzZTYd/CXLxnXpmWq6ycFYRbeyj8H93gh/00pw1E6ijOkNSDKY67d5AGHMkkSajRRUiklVPNT8FCHM/MevuMCUV805mF6Dpk7cxoEV76xcABolD//hj/KCIudtKvVdRSmeEiImmeFx1m6IRqDMY0m8WxnezyUVyOQuAT0StyGSV+daVjHErcYdE9oh8G8Yi+Z09RR3IYXliQHGsAbBAPm0omVwf9EOC3F4/9iAmkKQWT3TDj3gwAR4NbMMtqTDxCauYWbMeMDexdtg2P/RiWuc4rJJ+so1TB9NBSjFbjqPRttTng4COirtQIDAQAB";
        public const string StoreLink = "https://play.google.com/store/apps/details?id=com.BlackRainbowApps.KACKO";
        
        #elif UNITY_IPHONE

        public static string StoreName = OpenIAB_iOS.STORE;
        public const string StorePublicKey = "41f95fc5fdbe455a963a21b411f9024f";
        public const string StoreLink = "itms-apps://itunes.apple.com/app/id936492645";

        #endif
    }
}