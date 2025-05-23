﻿using Microsoft.Extensions.Configuration;
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

    var contentCreation = new RgWellbeingContentCreateRequest()
    {
        Thumbnail = "https://www.snapwire.co.uk/media/2yefydvg/square-logo.png",
        Description = "Join Coxy in this 15-minute video",
        Title = "Ear tension release",
        ContentType = "article",
        Provider = "f609e4ae-977d-11ef-8b28-023f2f4bd5f5", // providersResponse.Providers[0].Uuid,
        TopicUuid = topicsResponse.Topics[1].Uuid, // 0 is 'Featured' - don't reuse this
        LanguageCode = "en",
        TagIds = new List<string> // default to first 2 tags for now...
        {
            tagResponse.Tags[0].Uuid,
            tagResponse.Tags[1].Uuid
        }
    };

    // create content
    var responseContent = await contentManager.CreateContent(contentCreation);

    Console.WriteLine();
    Console.WriteLine("Content Created with Uuid: " + responseContent.Uuid);
    Console.WriteLine("...to Provider: " + providersResponse.Providers[0].Name);
    Console.WriteLine("...and Topic: " + topicsResponse.Topics[0].Name);
    Console.WriteLine("...and Tags: " + tagResponse.Tags[0].Title + " and " + tagResponse.Tags[1].Title);

    var responseContentGet = await contentManager.GetContent(responseContent.Uuid);

    // upload article
    Console.WriteLine();
    Console.WriteLine("Adding Article...");
    var responseArticle = await contentManager.UploadArticle(responseContent.Uuid, "<p>Today is " + DateTime.Now.ToString("ddd MM yyyy HH:mm:ss") + "</p>");
    Console.WriteLine("Article created with Uuid: " + responseArticle.Uuids[0]);

    var responseArticleGet = await contentManager.GetArticle(responseContent.Uuid);

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