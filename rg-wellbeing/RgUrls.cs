using static System.Net.WebRequestMethods;

namespace rg_wellbeing
{
    public static class RgUrls
    {
        // Auth
        public static string AuthUrl => "https://api.testing.aws.rewardgateway.net/auth/access_token";
        // PROD "https://identity.rewardgateway.net/access_token";

        // Core features
        private static string baseUrl = "https://api.testing.aws.rewardgateway.net"; // TEST
        //private static string baseUrl = "https://api.rewardgateway.net"; // PROD

        public static string WellbeingTags => baseUrl + "/content/wellbeing/tags?limit=500";
        public static string WellbeingTopics => baseUrl + "/content/wellbeing/topics";
        public static string WellbeingProviders => baseUrl + "/content/providers";
        public static string WellbeingContent => baseUrl + "/content/wellbeing";
        public static string WellbeingContentPatch => baseUrl + "/content/wellbeing/{0}";
        public static string WellbeingContentArticle => baseUrl + "/content/wellbeing/article/{0}/content";        
    }
}
