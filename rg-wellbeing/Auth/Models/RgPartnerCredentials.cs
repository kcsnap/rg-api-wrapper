
using Newtonsoft.Json;

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
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }

    public class RgWellbeingProviderCreateRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("disclaimer")]
        public string Disclaimer { get; set; }
    }

    public class RgWellbeingContentCreateRequest
    {
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("provider")]
        public string Provider { get; set; }

        [JsonProperty("topicUuid")]
        public string TopicUuid { get; set; }

        [JsonProperty("languageCode")]
        public string LanguageCode { get; set; }

        [JsonProperty("tagIds")]
        public List<string> TagIds { get; set; }
    }
    public class RgWellbeingContentUpdateRequest : RgWellbeingContentCreateRequest
    {

    }
    

    #region content
    public class RgWellbeingContentCreateResponse
    {
        public string Uuid { get; set; }
    }
    public class RgWellbeingContentUpdateResponse
    {
        public string Uuid { get; set; }
    }
    public class RgProviderCreateResponse
    {
        public string Uuid { get; set; }
        public string Name { get; set; }
        public string Disclaimer { get; set; }
    }
    #endregion
    #region recipe
    public class RgWellbeingRecipeUploadResponse : RgWellbeingArticleUploadResponse { }
    public class RgWellbeingRecipePatchResponse : RgWellbeingArticlePatchResponse { }
    public class RgWellbeingRecipeDeleteResponse : RgWellbeingArticleDeleteResponse { }
    public class RgWellbeingRecipeResponse
    {
    }
    #endregion
    #region video
    public class RgWellbeingVideoUploadResponse : RgWellbeingArticleUploadResponse { }
    public class RgWellbeingVideoPatchResponse : RgWellbeingArticlePatchResponse { }
    public class RgWellbeingVideoDeleteResponse : RgWellbeingArticleDeleteResponse { }
    #endregion
    #region audio
    public class RgWellbeingAudioUploadResponse : RgWellbeingArticleUploadResponse { }
    public class RgWellbeingAudioPatchResponse : RgWellbeingArticlePatchResponse { }
    public class RgWellbeingAudioDeleteResponse : RgWellbeingArticleDeleteResponse { }
    #endregion   
    #region articles
    public class RgWellbeingArticleUploadResponse
    {
        public string[] Uuids { get; set; }
    }
    public class RgWellbeingArticlePatchResponse
    {
        // THIS IS RETURNING A FULL OBJECT AND NOT UUID LIKE THE ABOVE
        public PatchContent content { get; set; }
    }
    public class RgWellbeingArticleDeleteResponse
    {

    }
    public class RgWellbeingArticlesResponse
    {
    }
    #endregion

    public class PatchContent
    {
        public string uuid { get; set; }
    }

    

    public class Tag
    {
        public string TagUuid { get; set; }
        public string Title { get; set; }
    }

    public class ArticleData
    {
        public string Content { get; set; }
    }

    public class DetailedContent
    {
        public ArticleData ArticleData { get; set; }
    }

    public class BaseGetArticleContent
    {
        public GetArticleContent Content { get; set; }
    }

    public class GetArticleContent
    {
        public string Uuid { get; set; }
        public string TopicUuid { get; set; }
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string ContentType { get; set; }
        public string Description { get; set; }
        public List<Tag> Tags { get; set; }
        public DetailedContent DetailedContent { get; set; }
    }


    public class RGWellbeingTag
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public class RGWellbeingTagsResponse
    {
        [JsonProperty("tags")]
        public List<RGWellbeingTag> Tags { get; set; }

        [JsonProperty("totalItems")]
        public int TotalItems { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }
    }

    public class RGWellbeingTopic
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("iconUrl")]
        public string IconUrl { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }
    }

    public class RGWellbeingLinks
    {
        [JsonProperty("self")]
        public RGWellbeingSelfLink Self { get; set; }
    }

    public class RGWellbeingSelfLink
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }

    public class RGWellbeingTopicsResponse
    {
        [JsonProperty("topics")]
        public List<RGWellbeingTopic> Topics { get; set; }

        [JsonProperty("_links")]
        public RGWellbeingLinks Links { get; set; }
    }

    public class RGWellbeingProvider
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("disclaimer")]
        public string Disclaimer { get; set; }
    }

    public class RGWellbeingProviderResponse
    {
        [JsonProperty("providers")]
        public List<RGWellbeingProvider> Providers { get; set; }
    }
}
