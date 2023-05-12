
using Azure;
using Azure.AI.OpenAI;
string endpoint = "https://your.azure.openai.endpoint/";
string key = "api-key-found-in-azure-openai";


OpenAIClient client = new(new Uri(endpoint), new AzureKeyCredential(key));

var completionsOptions = new CompletionsOptions()
{   Prompts = { @$"Classify the sentiment in this text.
     ----
     Text
     I don't like Pizza.
    ----
   Is the text Positive, Neutral or Negative?"},
 };



Response<Completions> completionsResponse = client.GetCompletions("AzureOpenAI Studio deployment model name", completionsOptions);

string completion = completionsResponse.Value.Choices[0].Text;
Console.WriteLine(completion);