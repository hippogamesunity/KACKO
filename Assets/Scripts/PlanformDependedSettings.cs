namespace Assets.Scripts
{
    public static class PlanformDependedSettings
    {
        #if UNITY_ANDROID

        public const string StoreLink = "https://play.google.com/store/apps/details?id=com.KASKO";
        
        #elif UNITY_IPHONE

        public const string StoreLink = "itms-apps://itunes.apple.com/app/id936492645";

        #endif
    }
}