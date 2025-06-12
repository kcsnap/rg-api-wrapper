using rg_wellbeing.Auth;
using rg_wellbeing.Auth.Models;

namespace RgWebapp.Services
{
    public class AuthService
    {
        private Authentication authentication;
        public event EventHandler AuthChanged;

        public async Task<RgPartnerAuthResponse> Authenticate(string clientId, string clientSecret, string partnerId)
        {
            authentication = new Authentication(new RgPartnerCredentials()
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                PartnerId = partnerId
            });

            var token = await authentication.GetAccessTokenAsync();

            // assume no errors thrown 
            AuthChanged?.Invoke(this, EventArgs.Empty);

            return token;
        }
    }
}
