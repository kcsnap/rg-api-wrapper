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
        public static string WellbeingProvidersUuid => baseUrl + "/content/providers/{0}";
        public static string WellbeingContentGet => baseUrl + "/content/wellbeing";
        public static string WellbeingContent => baseUrl + "/content/ingest/wellbeing";
        public static string WellbeingContentPatch => baseUrl + "/content/ingest/wellbeing/{0}";
        public static string WellbeingContentTypeUuid => baseUrl + "/content/ingest/wellbeing/{0}/{1}"; // i.e. 0=>article,recipe,etc 1=>uuid
        public static string WellbeingContentArticle => baseUrl + "/content/ingest/wellbeing/article/{0}/content";
        public static string WellbeingContentAudio  => baseUrl + "/content/ingest/wellbeing/{0}/audio";
        public static string WellbeingContentRecipe => baseUrl + "/content/ingest/wellbeing/recipe/{0}/content";
    }
}
