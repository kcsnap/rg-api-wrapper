using static System.Net.WebRequestMethods;

namespace rg_wellbeing
{
    public static class RgUrls
    {
        // Auth
        public static string AuthUrl => "https://identity.rewardgateway.net/access_token";

        // Core features
        private static string baseUrl = "https://api.rewardgateway.net";
        public static string WellbeingContent => baseUrl + "/content/wellbeing";
        public static string WellbeingContentArticle => baseUrl + "/content/wellbeing/article/{uuid}/content";

    }
}
