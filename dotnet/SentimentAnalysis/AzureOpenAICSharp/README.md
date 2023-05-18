# Azure OpenAI Sentiment Analysis

This code is an example of how to use the Azure.AI.OpenAI ckient library to classify the sentiment in a given text using the Azure OpenAI service.

 [Azure.AI.OpenAI Source code](https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/openai/Azure.AI.OpenAI/src) | [Package (NuGet)](https://www.nuget.org/packages/Azure.AI.OpenAI)

## Getting Started

### Prerequisites

* An [Azure subscription](https://azure.microsoft.com/free/)
* Azure OpenAI subscription.  Access to the Azure OpenAI portal is by application only.  Apply for access with this [form](https://aka.ms/oai/access?azure-portal=true)  
* API key from the [Azure OpenAI portal](https://learn.microsoft.com/en-us/azure/cognitive-services/openai/how-to/create-resource?pivots=web-portal#create-a-resource)
* Deployed model to reference in [Azure OpenAI Studio](https://learn.microsoft.com/en-us/azure/cognitive-services/openai/how-to/create-resource?pivots=web-portal#deploy-a-model)


## Usage

Clone this repository to your local machine.

Open the solution in Visual Studio or Visual Studio Code.

Replace the endpoint and key variables in Program.cs with your own values from the Azure Open AI portal.  

![azure portal open ai key](https://github.com/microsoft/globalopenaihack/blob/sentiment/assets/openaikeys.jpg)  

```C#
Program.cs

 string endpoint = "https://your.azure.openai.endpoint/";
 string key = "api-key-found-in-azure-openai";
```

Replace the AzureOpenAI Studio deployment model name with the name of your own deployment model.

![azure portal open ai key](https://github.com/microsoft/globalopenaihack/blob/sentiment/assets/deployments.jpg)  

```C#
Program.cs

 string deploymentName = "AzureOpenAI Studio deployment model name";
```

Run the code.  
```dotnetcli
dotnet run
```
![azure portal open ai key](https://github.com/microsoft/globalopenaihack/blob/sentiment/assets/openaicsharpoutput.jpg)  
  

Code Explanation

The code first sets the endpoint and key for the Azure OpenAI service. It then creates an instance of the OpenAIClient using the endpoint and key.


Next, it creates an instance of the CompletionsOptions class and sets the prompt to classify the sentiment in a given text.


Finally, it calls the GetCompletions method of the OpenAIClient instance, passing in the name of the AzureOpenAI Studio deployment model and the CompletionsOptions instance. The response is then printed to the console.


This code can be used as a starting point for building an application that uses the Azure OpenAI service to classify sentiment in text.


License

This code is licensed under the MIT License. See the LICENSE file for details.

