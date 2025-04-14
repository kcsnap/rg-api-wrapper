
namespace rg_wellbeing.Auth.Models
{
    public class RgPartnerCredentials
    {
        public string GrantType { get; } = "partner";
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string PartnerId { get; set; }
    }

    public class RgPartnerAuthResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
    }

    public class RgWellbeingContentCreateRequest
    {
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string ContentType { get; set; }
        public string Provider { get; set; }
        public string TopicUuid { get; set; }
        public string LanguageCode { get; set; }
        public List<string> TagIds { get; set; }
    }

    public class RgWellbeingArticleUploadResponse
    {
        public string[] Uuids { get; set; }
    }
    public class RgWellbeingContentCreateResponse
    {
        public string Uuid { get; set; }
    }
}
