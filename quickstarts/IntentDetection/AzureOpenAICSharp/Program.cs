using System.Net.Http;
using System.Text;
using System.Text.Json;

using Azure;
using Azure.AI.OpenAI;

string endpoint = "https://your.azure.openai.endpoint/";
string key = "api-key-found-in-azure-openai";
string deploymentName = "AzureOpenAI Studio deployment model name";

OpenAIClient client = new(new Uri(endpoint), new AzureKeyCredential(key));

//Sample text to be used for intent detection
string dress = @"Dear Contoso,
I recently purchased a dress from your store and was disappointed to find that it was much shorter than expected. The dress I received was not the same length as the one pictured on your website.
I am writing to request an exchange for a dress of the correct length. I understand that the product I received was not what I expected, and I would like to receive a dress that is the same length as the one pictured on your website.
I look forward to hearing from you soon.
Sincerely,
Liea Organa";


string book = @"
Dear Contoso,
I am writing to request a refund for the books I purchased from your store last week. Unfortunately, the books did not meet my expectations, and I would like to return them for a full refund.
I have attached a copy of the purchase receipt to this email as proof of purchase. The books are in their original packaging and have not been used, so I hope that the refund process will be straightforward.
Please let me know what steps I need to take to return the books and receive a refund. I look forward to hearing back from you soon.
Thank you for your attention to this matter.
Sincerely,
Jabba D Hutt";


var completionsOptions = new CompletionsOptions()
    {
		Prompts = { $@"This is an agent used to detect intent and provide a Department value for the following categories: “Books”, “Home”, “Fashion”, “Electronics”, “Grocery”, “Others”
---
Sample Output
Department: Electronics
Order Intent: Defect

Department: Grocery
Order Intent: Rotten food
---
Text: {dress}
" },
		Temperature = (float)0,
		MaxTokens = 100,
		NucleusSamplingFactor = (float)1,
		FrequencyPenalty = (float)0,
		PresencePenalty = (float)0,
		GenerationSampleCount = 1,
	};


Response<Completions> completionsResponse = await client.GetCompletionsAsync(deploymentName, completionsOptions);

string completion = completionsResponse.Value.Choices[0].Text;

//split completion into department and orderIntent
string[] completionParts = completion.Split(new string[] { "\r\n" }, StringSplitOptions.None);
string department  = "";
string orderIntent = "";
foreach (string part in completionParts)
{		
    if(part.Contains("Department:"))
	{
		department = part.Replace("Department:", "").Trim();
	}else if(part.Contains("Order Intent:"))
	{
		orderIntent = part.Replace("Order Intent:", "").Trim();
	}    
}
//Now parsed into department and orderIntent, actions like sending an email, calling a function or raising an event can occurr.  
Console.WriteLine(department);
Console.WriteLine(orderIntent);
