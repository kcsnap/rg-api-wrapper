using rg_wellbeing.Auth;
using rg_wellbeing.Auth.Models;

namespace RgWebapp.Services
{
    public class AuthService
    {
        Authentication authentication;

        public async Task<RgPartnerAuthResponse> Authenticate(string clientId, string clientSecret, string partnerId)
        {
            authentication = new Authentication(new RgPartnerCredentials()
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                PartnerId = partnerId
            });

            var token = await authentication.GetAccessTokenAsync();
            return token;
        }
    }
}
