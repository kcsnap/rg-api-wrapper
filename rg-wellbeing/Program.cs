using rg_wellbeing.Auth;
using rg_wellbeing.Auth.Models;
using rg_wellbeing.Content;

var clientId = string.Empty;
var clientSecret = string.Empty;
var partnerId = string.Empty;

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
    // create content
    var responseContent = await contentManager.CreateContent(new RgWellbeingContentCreateRequest()
    {
        Thumbnail = "https://ugc.rewardgateway.dev/image/get/1/wellbeingcenter/1/123.jpg",
        Description = "Join Helen Faliveno in this 15-minute video",
        Title = "Neck tension release",
        ContentType = "video",
        Provider = "8a98e6b3-d589-4782-9522-da619acc7916",
        TopicUuid = "8a98e6b3-d589-4782-9522-da619acc7917",
        LanguageCode = "en"
    });

    // upload article
    var responseArticle = await contentManager.UploadArticle(responseContent.Uuid, "<p>Lorem Ipsum</p>");

    // upload audio
    // upload video
    // upload recipe
    // get detailed content
    // delete content

    // get tags
    // get topics

    // create provider
    // get provider by uuid
    // get providers

}