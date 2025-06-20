using Microsoft.Extensions.Configuration;
using rg_wellbeing.Auth;
using rg_wellbeing.Auth.Models;
using rg_wellbeing.Content;

/* SUGGESTIONS - Documentation updates or other enhancements to simplify the onboarding of BUs
 * > Accept headers clearly defined - 'json' in some places
 * > GET request for generated content to be documented
 * > Steps to view simple article uploaded on the wellbeing platform
 * > Make postman files publically available https://edenred-my.sharepoint.com/:u:/p/ventsi_boyadzhiev/EY6eRZwPY9VHmhEelT1nq2MBbivcG1ezS1KvuaftlwRNCw?e=RrcV0f
 */

// Example URL of wellbeing for this BU (login via okta)
// https://site25398.testing.aws.rewardgateway.net/WellbeingCentre/topic/7
// ClientName = "SmartHub Demo Site [Staging]"

// Use this instead due to issue with above:
// https://site40651.testing.aws.rewardgateway.net

// manage secrets
IConfigurationRoot config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Welcome to RG API Console!");

// https://stackoverflow.com/questions/42268265/how-to-get-manage-user-secrets-in-a-net-core-console-application
var clientId = config["ClientId"];
var clientSecret = config["ClientSecret"];
var partnerId = config["PartnerId"];

// if any are blank, fail with message
if(string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret) || string.IsNullOrEmpty(partnerId))
{
    Console.WriteLine("Please update your secrets in the User Secrets file.");
    Console.WriteLine("See https://stackoverflow.com/questions/42268265/how-to-get-manage-user-secrets-in-a-net-core-console-application for more information.");
    Console.WriteLine("Press any key to exit.");
    Console.ReadKey();
    return;
}

Console.WriteLine(">> Press enter to create content and upload article.");
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

Console.WriteLine();
Console.WriteLine("Access Token: " + authResponse.AccessToken);

var contentManager = new RgContentManager(authResponse, true);

// actions
{
    // get available providers
    var providersResponse = await contentManager.GetProviders();

    // get available providers
    var topicsResponse = await contentManager.GetTopics();

    // get available tags
    var tagResponse = await contentManager.GetTags();

    // filter relevant tags
    var relevantTags = tagResponse.Tags.Where(t => t.Title.Contains("5 min") || t.Title.Contains("10 min")).ToList();

    var contentCreation = new RgWellbeingContentCreateRequest()
    {
        Thumbnail = "https://images.unsplash.com/photo-1746457002269-106424d702e4?w=900&auto=format&fit=crop&q=60&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxmZWF0dXJlZC1waG90b3MtZmVlZHwxN3x8fGVufDB8fHx8fA%3D%3D",
        Description = DateTime.Now.ToString("ddd MM yyyy HH:mm:ss") + "Join Coxy in this 15-minute video",
        Title = DateTime.Now.ToString("ddd MM yyyy HH:mm:ss") + " Ear tension release",
        ContentType = "article",
        Provider = providersResponse.Providers[0].Uuid,
        TopicUuid = topicsResponse.Topics[1].Uuid, // 0 is 'Featured' - don't reuse this, 1 is workout
        LanguageCode = "en",
        TagIds = new List<string> // default to first 2 tags for now...
        {
            "f417fca0-52a9-4701-837a-2a7c4509f17a",
            "45be55a9-b733-41b4-91a3-ce111e34c3c5",
            "50c38c1b-3476-4eab-bb71-c0dd2c94a29d"
            //relevantTags[0].Uuid,
            //relevantTags[1].Uuid
        }
    };

    // create content
    var responseContent = await contentManager.CreateContent(contentCreation);

    Console.WriteLine();
    Console.WriteLine("Content Created with Uuid: " + responseContent.Uuid);
    Console.WriteLine("...to Provider: " + providersResponse.Providers[0].Name);
    Console.WriteLine("...and Topic: " + topicsResponse.Topics[1].Name);
    Console.WriteLine("...and Tags: " + tagResponse.Tags[0].Title + " and " + tagResponse.Tags[1].Title);

    //var responseContentGet = await contentManager.GetContent(responseContent.Uuid);

    // update content
    Console.WriteLine();
    Console.WriteLine("Update Content...");

    contentCreation.Title = "UPDATED " + DateTime.Now.ToString("ddd MM yyyy HH:mm:ss") + " Ear tension release";
    contentCreation.Description = "UPDATED " + DateTime.Now.ToString("ddd MM yyyy HH:mm:ss") + " Join Coxy in this 15-minute video";
    //var updatedContent = await contentManager.ChangeContent(responseContent.Uuid, contentCreation);
    //Console.WriteLine("Content udpated with Uuid: " + updatedContent[0].Uuid);

    //// get updated content
    //var updatedContentGet = await contentManager.GetContent(responseContent.Uuid);
    //Console.WriteLine();

    // upload article
    Console.WriteLine();
    Console.WriteLine("Adding Article...");
    var responseArticle = await contentManager.UploadArticle(responseContent.Uuid, "<p>Today is " + DateTime.Now.ToString("ddd MM yyyy HH:mm:ss") + "</p>");
    Console.WriteLine("Article created with Uuid: " + responseArticle.Uuids[0]);

    //// get article
    //var responseArticleGet = await contentManager.GetArticle(responseContent.Uuid);

    // update article
    Console.WriteLine();
    Console.WriteLine("Update Article...");
    var updatedArticle = await contentManager.ChangeArticle(responseContent.Uuid, "<p>UPDATED ARTICLE Today is " + DateTime.Now.ToString("ddd MM yyyy HH:mm:ss") + "</p>");
    Console.WriteLine("Article udpated with Uuid: " + responseArticle.Uuids[0]);

    // get updated article
    var updatedArticleGet = await contentManager.GetArticle(responseContent.Uuid);
    Console.WriteLine();

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