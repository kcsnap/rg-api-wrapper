﻿using Newtonsoft.Json;
using rg_wellbeing.Auth.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace rg_wellbeing.Content
{
    public class RgContentManager
    {
        private readonly HttpClient _httpClient;
        private readonly RgPartnerAuthResponse _authResponse;

        public RgContentManager(RgPartnerAuthResponse authResponse, bool useProxy = false)
        {
            _authResponse = authResponse;

            if (useProxy)
            {
                // used with HttpToolkit to track api calls
                var httpClientHandler = new HttpClientHandler
                {
                    Proxy = new WebProxy("http://localhost:8000"),
                    UseProxy = true
                };
                _httpClient = new HttpClient(httpClientHandler);
            }
            else
            {
                _httpClient = new HttpClient();
            }

            // auth headers
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authResponse.AccessToken);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.rewardgateway+json;version=3.0");

        }

        #region topics and tags
        public async Task<RGWellbeingTopicsResponse> GetTopics()
        {
            var topics = await WellbeingApiGet<RGWellbeingTopicsResponse>(RgUrls.WellbeingTopics);
            return topics;
        }
        public async Task<RGWellbeingTagsResponse> GetTags(string? tagSearch = "")
        {
            var tags = await WellbeingApiGet<RGWellbeingTagsResponse>(RgUrls.WellbeingTags + tagSearch);
            return tags;            
        }
        #endregion
        #region content
        public async Task<RgWellbeingContentCreateResponse> DeleteContent(RgWellbeingContentCreateRequest wellbeingContent)
        {            
            string json = JsonConvert.SerializeObject(wellbeingContent);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(RgUrls.WellbeingContent, httpContent);

            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var result = await response.Content.ReadAsStringAsync();
                var wellbeingResponse = JsonConvert.DeserializeObject<RgWellbeingContentCreateResponse>(result);
                return wellbeingResponse;
            }
            else
            {
                // Handle the error response
                throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }
        public async Task<RgWellbeingContentCreateResponse> CreateContent(RgWellbeingContentCreateRequest wellbeingContent)
        {
            string json = JsonConvert.SerializeObject(wellbeingContent);
            return await WellbeingApiPost<RgWellbeingContentCreateResponse>(RgUrls.WellbeingContent, json);

            //string json = JsonConvert.SerializeObject(wellbeingContent);
            //HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            //var response = await _httpClient.PostAsync(RgUrls.WellbeingContent, httpContent);

            //if (response.IsSuccessStatusCode)
            //{
            //    // Read the response content as a string
            //    var result = await response.Content.ReadAsStringAsync();
            //    var wellbeingResponse = JsonConvert.DeserializeObject<RgWellbeingContentCreateResponse>(result);
            //    return wellbeingResponse;
            //}
            //else
            //{
            //    // Handle the error response
            //    throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            //}
        }

        public async Task<RgWellbeingContentUpdateResponse> ChangeContent(string uuid, RgWellbeingContentUpdateRequest wellbeingContent)
        {
            string json = JsonConvert.SerializeObject(wellbeingContent);
            var url = string.Format(RgUrls.WellbeingContentPatch, uuid);

            var response = await WellbeingApiPatch<RgWellbeingContentUpdateResponse[]>(url, json);
            return response[0];
        }

        public async Task<string> GetContent(string uuid, string contentType)
        {
            string url = string.Empty;

            switch (contentType)
            {
                // what's going on with the urls?
                case "article":
                case "recipe":
                    url = string.Format(RgUrls.WellbeingContentArticleRecipe, contentType, uuid);
                    break;
                case "video":
                case "audio":
                    url = string.Format(RgUrls.WellbeingContentMedia, uuid, contentType);
                    break;
                default:
                    throw new Exception("Invalid ContentType");
                    break;
            }

            return await WellbeingApiGet<string>(url);
        }

        #endregion
        #region providers
        public async Task<RgProviderCreateResponse> CreateProvider(RgWellbeingProviderCreateRequest provider)
        {
            string json = JsonConvert.SerializeObject(provider);
            return await WellbeingApiPost<RgProviderCreateResponse>(RgUrls.WellbeingProviders, json);

        }

        public async Task<RGWellbeingProviderResponse> GetProviders()
        {
            var providers = await WellbeingApiGet<RGWellbeingProviderResponse>(RgUrls.WellbeingProviders);
            return providers;
        }

        public async Task<RgWellbeingContentCreateResponse> DeleteProvider(string providerUuid)
        {            
            var response = await _httpClient.DeleteAsync(String.Format(RgUrls.WellbeingProvidersUuid, providerUuid));

            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var result = await response.Content.ReadAsStringAsync();
                var wellbeingResponse = JsonConvert.DeserializeObject<RgWellbeingContentCreateResponse>(result);
                return wellbeingResponse;
            }
            else
            {
                // Handle the error response
                var errorMsg = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error: {response.StatusCode}, {errorMsg}");
            }
        }
        #endregion
        #region article
        public async Task<string> GetArticle(string uuid)
        {
            var url = string.Format(RgUrls.WellbeingContentArticleRecipe, "article", uuid);

            return await WellbeingApiGet<string>(url);
        }
        public async Task<string> GetContentAsync(string contentType)
        {
            var url = string.Format(RgUrls.WellbeingContentGetAll, contentType); // + // "?offset=0&limit=100&topicId=10";

            return await WellbeingApiGet<string>(url);
        }
        public async Task<RgWellbeingArticleUploadResponse> UploadArticle(string uuid, string articleHtmlAsString)
        {
            var request = "{" +
                "\"body\": {" +
                    "\"article\": \"" + articleHtmlAsString + "\"" +
                "}" +
            "}";
            //string json = JsonConvert.SerializeObject(request);
            var url = string.Format(RgUrls.WellbeingContentArticleRecipe, "article", uuid);

            return await WellbeingApiPost<RgWellbeingArticleUploadResponse>(url, request);
        }
        public async Task<RgWellbeingArticlePatchResponse> ChangeArticle(string uuid, string articleHtmlAsString)
        {
            var request = "{" +
                "\"body\": {" +
                    "\"article\": \"" + articleHtmlAsString + "\"" +
                "}" +
            "}";
            var url = string.Format(RgUrls.WellbeingContentArticleRecipe, "article", uuid);

            return await WellbeingApiPatch<RgWellbeingArticlePatchResponse>(url, request);
        }
        public async Task<RgWellbeingArticleDeleteResponse> DeleteArticle(string uuid, string contentType)
        {
            var url = string.Format(RgUrls.WellbeingContentTypeUuid, contentType, uuid);

            return await WellbeingApiDelete<RgWellbeingArticleDeleteResponse>(url);

        }
        #endregion
        #region recipe
        public async Task<string> GetRecipe(string uuid)
        {
            var url = string.Format(RgUrls.WellbeingContentArticleRecipe, "recipe", uuid);

            return await WellbeingApiGet<string>(url);
        }
        public async Task<RgWellbeingRecipeResponse> GetRecipesAsync()
        {
            var url = string.Format(RgUrls.WellbeingContentGet) + "?offset=0&limit=100&topicId=10";

            return await WellbeingApiGet<RgWellbeingRecipeResponse>(url);
        }
        public async Task<RgWellbeingRecipeUploadResponse> UploadRecipe(string uuid, string recipeJsonAsString)
        {
            var request = recipeJsonAsString;
            
            //string json = JsonConvert.SerializeObject(request);
            var url = string.Format(RgUrls.WellbeingContentArticleRecipe, "recipe", uuid);

            return await WellbeingApiPost<RgWellbeingRecipeUploadResponse>(url, request);
        }
        public async Task<RgWellbeingRecipePatchResponse> ChangeRecipe(string uuid, string recipeJsonAsString)
        {
            var request = recipeJsonAsString;
            var url = string.Format(RgUrls.WellbeingContentArticleRecipe, "recipe", uuid);

            return await WellbeingApiPatch<RgWellbeingRecipePatchResponse>(url, request);
        }
        public async Task<RgWellbeingRecipeDeleteResponse> DeleteRecipe(string uuid, string contentType)
        {
            var url = string.Format(RgUrls.WellbeingContentTypeUuid, contentType, uuid);

            return await WellbeingApiDelete<RgWellbeingRecipeDeleteResponse>(url);

        }
        #endregion

        #region audio
        public async Task<RgWellbeingAudioUploadResponse> UploadAudio(string uuid, string audioUrl)
        {
            var request = "{" +
                "\"media\": [" +
                    "\"" + audioUrl + "\"" +
                "]" +
            "}";

            var url = string.Format(RgUrls.WellbeingContentMedia, uuid, "audio");
            return await WellbeingApiPost<RgWellbeingAudioUploadResponse>(url, request);
        }
        public async Task<RgWellbeingAudioPatchResponse> ChangeAudio(string uuid, string audioUrl)
        {
            var request = "{" +
                "\"media\": [" +
                    "\"" + audioUrl + "\"" +
                "]" +
            "}";

            var url = string.Format(RgUrls.WellbeingContentMedia, uuid, "audio");
            return await WellbeingApiPatch<RgWellbeingAudioPatchResponse>(url, request);
        }
        public async Task<RgWellbeingAudioDeleteResponse> DeleteAudio(string uuid, string contentType)
        {
            var url = string.Format(RgUrls.WellbeingContentTypeUuid, contentType, uuid);

            return await WellbeingApiDelete<RgWellbeingAudioDeleteResponse>(url);

        }
        #endregion
        #region video
        public async Task<RgWellbeingVideoUploadResponse> UploadVideo(string uuid, string videoUrl)
        {
            var request = "{" +
                "\"media\": [" +
                    "\"" + videoUrl + "\"" +
                "]" +
            "}";

            var url = string.Format(RgUrls.WellbeingContentMedia, uuid, "video");
            return await WellbeingApiPost<RgWellbeingVideoUploadResponse>(url, request);
        }
        public async Task<RgWellbeingVideoPatchResponse> ChangeVideo(string uuid, string videoUrl)
        {
            var request = "{" +
                "\"media\": [" +
                    "\"" + videoUrl + "\"" +
                "]" +
            "}";

            var url = string.Format(RgUrls.WellbeingContentMedia, uuid, "video");
            return await WellbeingApiPatch<RgWellbeingVideoPatchResponse>(url, request);
        }
        public async Task<RgWellbeingVideoDeleteResponse> DeleteVideo(string uuid, string contentType)
        {
            var url = string.Format(RgUrls.WellbeingContentTypeUuid, contentType, uuid);

            return await WellbeingApiDelete<RgWellbeingVideoDeleteResponse>(url);

        }
        #endregion
        #region generic http methods
        private async Task<T> WellbeingApiGet<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var result = await response.Content.ReadAsStringAsync();
                T wellbeingResponse;

                if (typeof(T) == typeof(String)) // shouldn't do this in generics! But only a test app!
                {
                    wellbeingResponse = (T)(object)result; // Cast to string if T is string
                }
                else
                {
                    wellbeingResponse = JsonConvert.DeserializeObject<T>(result);
                }

                return wellbeingResponse;
            }
            else
            {
                // Handle the error response
                throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }
        private async Task<T> WellbeingApiPost<T>(string url, string body)
        {            
            //string json = JsonConvert.SerializeObject(body);
            HttpContent httpContent = new StringContent(body, Encoding.UTF8, "application/json");

            // make the call
            var response = await _httpClient.PostAsync(url, httpContent);

            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var result = await response.Content.ReadAsStringAsync();
                var deserialisedResult = JsonConvert.DeserializeObject<T>(result);
                return deserialisedResult;
            }
            else
            {
                // Handle the error response
                var errorDescription = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error: {response.StatusCode}, {errorDescription}");
            }
        }
        private async Task<T> WellbeingApiPatch<T>(string url, string body)
        {
            //string json = JsonConvert.SerializeObject(body);
            HttpContent httpContent = new StringContent(body, Encoding.UTF8, "application/json");

            // make the call
            var response = await _httpClient.PatchAsync(url, httpContent);

            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var result = await response.Content.ReadAsStringAsync();
                var deserialisedResult = JsonConvert.DeserializeObject<T>(result);
                return deserialisedResult;
            }
            else
            {
                // Handle the error response
                throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }
        private async Task<T> WellbeingApiDelete<T>(string url)
        {
            // make the call
            var response = await _httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var result = await response.Content.ReadAsStringAsync();
                var deserialisedResult = JsonConvert.DeserializeObject<T>(result);
                return deserialisedResult;
            }
            else
            {
                // Handle the error response
                throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }
        #endregion
    }
}
