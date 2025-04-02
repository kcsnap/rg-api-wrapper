using rg_wellbeing.Auth.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace rg_wellbeing.Content
{
    public class RgContentManager
    {
        private readonly HttpClient _httpClient;
        private readonly RgPartnerAuthResponse _authResponse;

        public RgContentManager(RgPartnerAuthResponse authResponse)
        {
            _authResponse = authResponse;
            _httpClient = new HttpClient();
        }

        public async Task<RgWellbeingContentCreateResponse> CreateContent(RgWellbeingContentCreateRequest wellbeingContent)
        {
            // auth headers
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authResponse.AccessToken);

            string json = JsonSerializer.Serialize(wellbeingContent);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(RgUrls.WellbeingContent, httpContent);

            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var result = await response.Content.ReadAsStringAsync();
                var wellbeingResponse = JsonSerializer.Deserialize<RgWellbeingContentCreateResponse>(result);
                return wellbeingResponse;
            }
            else
            {
                // Handle the error response
                throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }

        public async Task<RgWellbeingArticleUploadResponse> UploadArticle(string uuid, string htmlAsString)
        {
            var request = "{" +
                "\"body\": {" +
                    "\"article\": \"" + htmlAsString + "\"" +
                "\"}" +
            "}\"";

            // auth headers
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authResponse.AccessToken);

            string json = JsonSerializer.Serialize(request);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(string.Format(RgUrls.WellbeingContentArticle, uuid), httpContent);

            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var result = await response.Content.ReadAsStringAsync();
                var wellbeingResponse = JsonSerializer.Deserialize<RgWellbeingArticleUploadResponse>(result);
                return wellbeingResponse;
            }
            else
            {
                // Handle the error response
                throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }
    }
}
