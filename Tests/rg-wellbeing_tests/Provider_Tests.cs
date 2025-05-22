using Azure.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using rg_wellbeing.Auth;
using rg_wellbeing.Auth.Models;
using rg_wellbeing.Content;

namespace rg_wellbeing_tests
{
    [TestClass]
    public sealed class Provider_Tests
    {
        RgPartnerAuthResponse _authResponse;

        [TestInitialize]
        public async Task Init()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddUserSecrets<Provider_Tests>()
                .Build();

            _authResponse = await new Authentication(new RgPartnerCredentials
            {
                ClientId = config["ClientId"],
                ClientSecret = config["ClientSecret"],
                PartnerId = config["PartnerId"]
            }).GetAccessTokenAsync();
        }

        [TestMethod]
        public async Task GetProviderAsync_HappyTest()
        {
            var contentManager = new RgContentManager(_authResponse); //, true);

            // get available providers
            var providersResponse = await contentManager.GetProviders();

            Assert.IsTrue(providersResponse.Providers.Count > 1, "Providers found: " + providersResponse.Providers.Count());
        }

        /// <summary>
        /// https://portal.azure.com/#@snapwire.co.uk/resource/subscriptions/66673944-b353-4256-b71c-0cd8751c81c0/resourcegroups/kenbot-rg/providers/Microsoft.DocumentDB/databaseAccounts/kenbot-cosmos-nosql/dataExplorer
        /// https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/query/working-with-dates
        /// https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/quickstart-portal
        /// https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/quickstart-dotnet
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task Cosmo_Test()
        {
            DefaultAzureCredential credential = new();

            CosmosClient client = new(
                accountEndpoint: "<azure-cosmos-db-nosql-account-endpoint>",
                tokenCredential: new DefaultAzureCredential()
            );
        }
    }
}
