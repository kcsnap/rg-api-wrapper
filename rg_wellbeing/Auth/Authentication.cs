using rg_wellbeing.Auth.Models;
using System.Text.Json;

namespace rg_wellbeing.Auth
{
    /// <summary>
    /// See https://docs.rewardgateway.net/docs/authentication#obtaining-a-partner-scoped-access-token
    /// </summary>
    public class Authentication
    {
        private readonly HttpClient _httpClient;
        private readonly RgPartnerCredentials _credentials;

        public Authentication(RgPartnerCredentials credentials)
        {
            _httpClient = new HttpClient();
            _credentials = credentials;
        }

        public async Task<RgPartnerAuthResponse> GetAccessTokenAsync()
        {
            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", _credentials.GrantType),
                new KeyValuePair<string, string>("client_id", _credentials.ClientId),
                new KeyValuePair<string, string>("client_secret", _credentials.ClientSecret),
                new KeyValuePair<string, string>("partner_id", _credentials.PartnerId)
            });

            var response = await _httpClient.PostAsync(RgUrls.AuthUrl, requestContent);

            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var result = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<RgPartnerAuthResponse>(result);
                return authResponse;
            }
            else
            {
                // Handle the error response
                throw new Exception($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }
    }
}
