using Newtonsoft.Json;
using rg_wellbeing.Auth.Models;
using System;
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

        public async Task<RGWellbeingTopicsResponse> GetTopics()
        {
            var topics = await WellbeingApiGet<RGWellbeingTopicsResponse>(RgUrls.WellbeingTopics);
            return topics;
        }

        public async Task<RGWellbeingTagsResponse> GetTags()
        {
            var tags = await WellbeingApiGet<RGWellbeingTagsResponse>(RgUrls.WellbeingTags);
            return tags;            
        }

        public async Task<RGWellbeingProviderResponse> GetProviders()
        {
            var providers = await WellbeingApiGet<RGWellbeingProviderResponse>(RgUrls.WellbeingProviders);
            return providers;
        }

        private async Task<T> WellbeingApiGet<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var result = await response.Content.ReadAsStringAsync();
                var wellbeingResponse = JsonConvert.DeserializeObject<T>(result);
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

        public async Task<RgWellbeingContentPatchResponse[]> ChangeContent(string uuid, RgWellbeingContentCreateRequest wellbeingContent)
        {
            string json = JsonConvert.SerializeObject(wellbeingContent);
            var url = string.Format(RgUrls.WellbeingContentPatch, uuid);

            return await WellbeingApiPatch<RgWellbeingContentPatchResponse[]>(url, json);
        }

        public async Task<RgWellbeingContentCreateResponse> GetContent(string uuid)
        {
            var url = string.Format(RgUrls.WellbeingContentPatch, uuid);

            return await WellbeingApiGet<RgWellbeingContentCreateResponse>(url);
        }

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

        public async Task<RgWellbeingArticleUploadResponse> GetArticle(string uuid)
        {
            var url = string.Format(RgUrls.WellbeingContentArticle, uuid);

            return await WellbeingApiGet<RgWellbeingArticleUploadResponse>(url);
        }
        public async Task<RgWellbeingArticleUploadResponse> UploadArticle(string uuid, string articleHtmlAsString)
        {
            var request = "{" +
                "\"body\": {" +
                    "\"article\": \"" + articleHtmlAsString + "\"" +
                "}" +
            "}";
            //string json = JsonConvert.SerializeObject(request);
            var url = string.Format(RgUrls.WellbeingContentArticle, uuid);

            return await WellbeingApiPost<RgWellbeingArticleUploadResponse>(url, request);
        }


        public async Task<RgWellbeingArticlePatchResponse> ChangeArticle(string uuid, string htmlAsString)
        {
            var request = "{" +
                "\"body\": {" +
                    "\"article\": \"" + htmlAsString + "\"" +
                "}" +
            "}";
            var url = string.Format(RgUrls.WellbeingContentArticle, uuid);

            return await WellbeingApiPatch<RgWellbeingArticlePatchResponse>(url, request);

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
                throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }
    
        private async Task<T> WellbeingApiPatch<T>(string url, string body)
        {
            string json = JsonConvert.SerializeObject(body);
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
    }
}
