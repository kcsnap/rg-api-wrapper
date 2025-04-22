using Microsoft.Extensions.Configuration;
using rg_wellbeing.Auth;
using rg_wellbeing.Auth.Models;
using rg_wellbeing.Content;

/* SUGGESTIONS - Documentation updates or other enhancements to simplify the onboarding of BUs
 * > Accept headers clearly defined
 * > Steps to view simple article uploaded on the wellbeing platform
 * 
 */

// Example URL of wellbeing for this BU (login via okta)
// https://site25398.testing.aws.rewardgateway.net/WellbeingCentre/topic/7
// ClientName = "SmartHub Demo Site [Staging]"

// manage secrets
IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

// https://stackoverflow.com/questions/42268265/how-to-get-manage-user-secrets-in-a-net-core-console-application
var clientId = config["ClientId"];
var clientSecret = config["ClientSecret"];
var partnerId = config["PartnerId"];

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Welcome to RG API Console!");
Console.WriteLine("");
Console.WriteLine("Please ensure all credentials have been updated prior to continuing [press any key to continue].");
Console.ReadLine();

// setup credentials
var credentials = new RgPartnerCredentials
{
    ClientId = clientId,
    ClientSecret = clientSecret,
    PartnerId = partnerId
};

// authenticate
var authentication = new Authentication(credentials);
var authResponse = await authentication.GetAccessTokenAsync();
Console.WriteLine("Access Token: " + authResponse.AccessToken);

var contentManager = new RgContentManager(authResponse);

// actions
{
    // get available providers
    var providersResponse = await contentManager.GetProviders();

    // get available providers
    var topicsResponse = await contentManager.GetTopics();

    // get available tags
    var tagResponse = await contentManager.GetTags();

    // create content
    var responseContent = await contentManager.CreateContent(new RgWellbeingContentCreateRequest()
    {
        Thumbnail = "https://www.snapwire.co.uk/media/2yefydvg/square-logo.png",
        Description = "Join Helen Faliveno in this 15-minute video",
        Title = "Neck tension release",
        ContentType = "article",
        Provider = providersResponse.Providers[0].Uuid,
        TopicUuid = topicsResponse.Topics[0].Uuid,
        LanguageCode = "en",
        TagIds = new List<string> // default to first 2 tags for now...
        {
            tagResponse.Tags[0].Uuid,
            tagResponse.Tags[1].Uuid
        }
    });

    Console.WriteLine();
    Console.WriteLine("Content Created with Uuid: " + responseContent.Uuid);
    Console.WriteLine("...to Provider: " + providersResponse.Providers[0].Name);
    Console.WriteLine("...and Topic: " + topicsResponse.Topics[0].Name);
    Console.WriteLine("...and Tags: " + tagResponse.Tags[0].Title + " and " + tagResponse.Tags[1].Title);

    // upload article
    Console.WriteLine();
    Console.WriteLine("Adding Article...");
    var responseArticle = await contentManager.UploadArticle(responseContent.Uuid, "<p>Today is " + DateTime.Now.ToString("ddd MM yyyy HH:mm:ss") + "</p>");
    Console.WriteLine("Article created with Uuid: " + responseArticle.Uuids[0]);

    // upload audio
    Console.WriteLine();

    // upload video
    Console.WriteLine();

    // upload recipe
    Console.WriteLine();

    // get detailed content
    Console.WriteLine();

    // delete content
    Console.WriteLine();


    // get tags
    // get topics

    // create provider
    // get provider by uuid
    // get providers

}