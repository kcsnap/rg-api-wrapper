using rg_wellbeing.Auth;

namespace rg_wellbeing_tests
{
    [TestClass]
    public sealed class Authentication_Tests
    {
        [TestMethod]
        public async Task GetAccessTokenAsync_HappyTest()
        {
            var auth = new Authentication(new rg_wellbeing.Auth.Models.RgPartnerCredentials
            {
                ClientId = "",
                ClientSecret = "",
                PartnerId = ""
            });

            var response = await auth.GetAccessTokenAsync();

            Assert.IsNotNull(response);
        }
    }
}
