using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;


// In this example all the keys and secrets have been stored in Azure App Configuration.
var builder = new ConfigurationBuilder();
builder.AddAzureAppConfiguration(Environment.GetEnvironmentVariable("ConnectionString"));

var config = builder.Build();

//To successfully make a call against Azure OpenAI, you'll need an endpoint and a key.
// EndPoint - This value can be found in the Keys & Endpoint section when examining your resource from the Azure portal.
//           Alternatively, you can find the value in the Azure OpenAI Studio > Playground > Code View.
//           An example endpoint is: https://docs-test-001.openai.azure.com/.

// API KEY - This value can be found in the Keys & Endpoint section when examining your resource from the Azure portal.
//           You can use either KEY1 or KEY2
OpenAIClient client = new OpenAIClient(new Uri(config["openaiuri"]),
                                                new AzureKeyCredential(config["openaikey"]));

string responseMessage = await CodeCompletion(config, client);

Console.WriteLine(responseMessage);
Console.ReadLine();


static async Task<string> CodeCompletion(IConfigurationRoot config, OpenAIClient client)
{
    Console.WriteLine("Please tell what kind of code you would like to generate");
    string? prompt = Console.ReadLine();

    // Enter the deployment name you chose when you deployed the model.
    // How to deploy a model - https://learn.microsoft.com/en-us/azure/cognitive-services/openai/how-to/create-resource?pivots=cli

    Response<Completions> completionsResponse = await client.GetCompletionsAsync(
        deploymentOrModelName: config["openai-code-deployment"],
            new CompletionsOptions()
            {
                Prompts = { prompt, },
                Temperature = (float)0,
                MaxTokens = 1000,
                NucleusSamplingFactor = (float)1,
                FrequencyPenalty = (float)0,
                PresencePenalty = (float)0,
                GenerationSampleCount = 1,
            });

    Completions completions = completionsResponse.Value;

    string responseMessage = string.IsNullOrEmpty(completions.Choices[0].Text)
        ? "Function to generate code has been executed successfully but no code has been generated, please try again!!!"
        : completions.Choices[0].Text;

    return responseMessage;
}
