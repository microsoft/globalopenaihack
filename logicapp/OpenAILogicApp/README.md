# Logic App Sentiment Analysis

This example demonstrates how to call an Azure Logic App using OpenAI's Completion API or the Azure OpenAI Rest Service to classify the sentiment in a given text.  
[OpenAI API Reference](https://platform.openai.com/docs/api-reference/completions)  
[Azure OpenAI API Reference](https://learn.microsoft.com/en-us/azure/cognitive-services/openai/reference#completions)

## Getting Started

### Prerequisites

* An [Azure subscription](https://azure.microsoft.com/free/)  

To use this example, you will need either an OpenAI account or an Azure OpenAI account  
**OpenAI**
* An [OpenAI account](https://openai.com/)  
[Video on how to open an account](https://www.youtube.com/)
* API key from the [OpenAI portal](https://platform.openai.com/)  

**Azure OpenAI**
* Azure OpenAI subscription.  Access to the Azure OpenAI portal is by application only.  
Apply for access with this [form](https://aka.ms/oai/access?azure-portal=true)  
* API key from the [Azure OpenAI portal](https://learn.microsoft.com/en-us/azure/cognitive-services/openai/how-to/create-resource?pivots=web-portal#create-a-resource)
* Deployed model to reference in [Azure OpenAI Studio](https://learn.microsoft.com/en-us/azure/cognitive-services/openai/how-to/create-resource?pivots=web-portal#deploy-a-model)

## Usage  
> TODO change link to reference main  

Deploy the Logic App to your Azure subscription.  
[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmicrosoft%2Fglobalopenaihack%2Fsentiment%2Flogicapp%2FOpenAILogicApp%2Fazuredeploy.json)  


1. Set the subscription, resource group, region and name the Logic App.  
![Step on Logic App](https://github.com/microsoft/globalopenaihack/blob/sentiment/assets/logicapps/step1.jpg)
