# OpenAI Sentiment Analysis

This code is an example of how to use OpenAI's APIs to classify the sentiment in a given text using a simple HTTP request.

## Getting Started

### Prerequisites

* An [OpenAI account](https://openai.com/)  
[Video on how to open an account](https://www.youtube.com/)
* API key from the [OpenAI portal](https://platform.openai.com/)  
To find the API Key sign into the portal, and click the user profile in the top right corner  
![open ai dropdown](https://github.com/microsoft/globalopenaihack/blob/sentiment/assets/sentimentanalysis/openaidropdown.jpg)  

Create and store the newly created key for later.  
![open ai dropdown](https://github.com/microsoft/globalopenaihack/blob/sentiment/assets/sentimentanalysis/openaiSecret.jpg) 


## Usage

Clone this repository to your local machine.

Open the solution in your developer environment ([Visual Studio Code](https://code.visualstudio.com/), Visual Studio, etc.).

Replace the endpoint and key variables in Program.cs with your own values from the Azure Open AI portal.  

![azure portal open ai key](https://github.com/microsoft/globalopenaihack/blob/sentiment/assets/sentimentanalysis/openaikeys.jpg)  

```C#
Program.cs

 string endpoint = "https://your.azure.openai.endpoint/";
 string key = "api-key-found-in-azure-openai";
```

Replace the AzureOpenAI Studio deployment model name with the name of your own deployment model.

![azure portal open ai key](https://github.com/microsoft/globalopenaihack/blob/sentiment/assets/sentimentanalysis/deployments.jpg)  

```C#
Program.cs

 string deploymentName = "AzureOpenAI Studio deployment model name";
```

Run the code.  
```dotnetcli
dotnet run
```
![azure portal open ai key](https://github.com/microsoft/globalopenaihack/blob/sentiment/assets/sentimentanalysis/openaicsharpoutput.jpg)  
  

## Code Explanation

First, set the endpoint and key for the Azure OpenAI service and the Azure OpenAI Studio model name.  
```C#
using Azure;
using Azure.AI.OpenAI;
string endpoint = "https://your.azure.openai.endpoint/";
string key = "api-key-found-in-azure-openai";
string deploymentName = "AzureOpenAI Studio deployment model name";
```  
Next, create an instance of the OpenAIClient using the endpoint and key.  
```C#
OpenAIClient client = new(new Uri(endpoint), new AzureKeyCredential(key))
```

Then, create a variable of the CompletionsOptions class and set the prompt to classify the sentiment in a given text.
```C#
var completionsOptions = new CompletionsOptions()
{   Prompts = { @$"Classify the sentiment in this text.
     ----
     Text
     I don't like Pizza.
    ----
   Is the text Positive, Neutral or Negative?"},
 };
```

Finally, call the GetCompletions method of the OpenAIClient instance, passing in the name of the AzureOpenAI Studio deployment model and the completionsOptions variable.  
```C#
Response<Completions> completionsResponse = client.GetCompletions(deploymentName, completionsOptions);
```  
The response is then printed to the console.  
```C#
string completion = completionsResponse.Value.Choices[0].Text;
Console.WriteLine(completion);
```  
![azure portal open ai key](https://github.com/microsoft/globalopenaihack/blob/sentiment/assets/sentimentanalysis/openaicsharpoutput.jpg)  
  
  


