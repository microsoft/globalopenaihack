
using Azure;
using Azure.AI.OpenAI;
string endpoint = "https://your.azure.openai.endpoint/";
string key = "api-key-found-in-azure-openai";

//Curie is a fast and less expensive model that is good for sentiment analysis, summarization, and text generation.
string deploymentName = "AzureOpenAI Studio deployment model name";

OpenAIClient client = new(new Uri(endpoint), new AzureKeyCredential(key));

var completionsOptions = new CompletionsOptions()
{   Prompts = { @$"Classify the sentiment in this text.
     ----
     Text
     I don't like Pizza.
    ----
   Is the text Positive, Neutral or Negative?"},
 };



Response<Completions> completionsResponse = client.GetCompletions(deploymentName, completionsOptions);

string completion = completionsResponse.Value.Choices[0].Text;
Console.WriteLine(completion);