using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planners;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .Build();

var deviceId = AudioSelector.SelectDevice();

var transcriberSettings = new TranscriberSettings(config["SubscriptionId"], config["Location"], deviceId);

var completionsDeployment = config["CompletionsDeployment"];
var azureOpenAIEndpoint = config["AzureOpenAIEndpoint"];
var azureOpenAIApiKey = config["AzureOpenAIApiKey"];

if (string.IsNullOrEmpty(completionsDeployment)) throw new ArgumentException("CompletionsDeployment is required. Check your appsettings.json.", nameof(completionsDeployment));
if (string.IsNullOrEmpty(azureOpenAIEndpoint)) throw new ArgumentException("AzureOpenAIEndpoint is required. Check your appsettings.json.", nameof(azureOpenAIEndpoint));
if (string.IsNullOrEmpty(azureOpenAIApiKey)) throw new ArgumentException("AzureOpenAIApiKey is required. Check your appsettings.json.", nameof(azureOpenAIApiKey));

var kernelBuilder = new KernelBuilder();
var kernel = kernelBuilder.WithAzureChatCompletionService(completionsDeployment, azureOpenAIEndpoint, azureOpenAIApiKey).Build();

//var pluginDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
//var translatePlugin = kernel.ImportSemanticSkillFromDirectory(pluginDirectory, "TranslatePlugin");


// load the kernel up with multiple functions to choose from
//var celebratePlugin = kernel.ImportSkill(new CelebratePlugin(kernel));

// create a planner and ask it to create a plan for us based on the user's prompt
//ActionPlanner planner = new(kernel);
//Plan plan = await planner.CreatePlanAsync("I have no idea what to get my dad for father's day!");

//Console.WriteLine(plan.ToJson(true));

//// with the plan in hand, we can now execute it and get the result
//SKContext result = await plan.InvokeAsync();

//Console.WriteLine(result);

// register plugins for getting thematic travel destinations, tourist activities, drink recipes, and game ideas
string travelDestinations = "give me a comma-separated list of travel destinations given these criteria: {{$input}}";
kernel.CreateSemanticFunction(travelDestinations, description: "provides a comma-separated list of travel destinations given the user's criteria");

string touristActivities = "give me a list of popular tourist activities for these destinations: {{$input}}";
kernel.CreateSemanticFunction(touristActivities, description: "provides a list of popular tourist activities for the given destinations");

string drinkRecipes = "give me a drink recipe given these criteria: {{$input}}";
kernel.CreateSemanticFunction(drinkRecipes, description: "provides a drink recipe given the user's criteria");

string gameIdeas = "give me a bulleted list of three board games given these criteria: {{$input}}";
kernel.CreateSemanticFunction(gameIdeas, description: "provides a bulleted list of three board games given the user's criteria");


// create an instance of a planner and provide a connection to the kernel
SequentialPlanner planner = new(kernel);

using var transcriber = new Transcriber(transcriberSettings, kernel, planner);

// script: I want to book a two-week trip to somewhere warm and sunny and get some ideas for things to do while I'm there.
await transcriber.StartAsync();

do
{
    Console.WriteLine("You're now chatting with your copilot. Press escape at anytime to stop recording...");
} while (Console.ReadKey().Key != ConsoleKey.Escape);

var plan =  await transcriber.StopAsync();

var result = await plan.InvokeAsync(kernel);

Console.WriteLine(result);

Console.WriteLine("Press any key to exit...");

Console.ReadKey();