# Logic App Intent Detection

This example demonstrates how to call an Azure Logic App using OpenAI's Completion API or the Azure OpenAI Rest Service to determine the intent of a statement and then act on the response.  
[OpenAI API Reference](https://platform.openai.com/docs/api-reference/completions)  
[Azure OpenAI API Reference](https://learn.microsoft.com/en-us/azure/cognitive-services/openai/reference#completions)

## Getting Started

### Prerequisites

* An [Azure subscription](https://azure.microsoft.com/free/)  
* A http querying tool like Postman or curl

To use this example, you will need either an OpenAI account or an Azure OpenAI account  
**OpenAI**
* An [OpenAI account](https://openai.com/)  
[Video on how to open an account](https://www.youtube.com/watch?v=zJSYMWlCcPY)
* API key from the [OpenAI portal](https://platform.openai.com/)  

**Azure OpenAI**
* Azure OpenAI subscription.  Access to the Azure OpenAI portal is by application only.  
Apply for access with this [form](https://aka.ms/oai/access?azure-portal=true)  
* API key from the [Azure OpenAI portal](https://learn.microsoft.com/en-us/azure/cognitive-services/openai/how-to/create-resource?pivots=web-portal#create-a-resource)
* Deployed model to reference in [Azure OpenAI Studio](https://learn.microsoft.com/en-us/azure/cognitive-services/openai/how-to/create-resource?pivots=web-portal#deploy-a-model)

## Usage  

Deploy the Logic App to your Azure subscription.  
[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmicrosoft%2Fglobalopenaihack%2Ftest%2Fquickstarts%2Fintentdetection%2FOpenAILogicApp%2Fazuredeploy.json)   

1. Set the subscription, resource group, region and name the Logic App.  
![Step on Logic App](../../../images/intentdetection/step1.jpg)  

### OpenAI  
2. Add the OpenAI Key to the Initialize OpenAIKey step.  
![Step on Logic App](../../../images/intentdetection/step2oai.jpg)  
To find the API Key sign into the portal, and click the user profile in the top right corner  
![open ai dropdown](../../../images/sentimentanalysis/openaidropdown.jpg)  
Create and store the new key, you will not be able to retieve the value once the create dialog is closed. 
![open ai key](../../../images/sentimentanalysis/openaiSecret.jpg)  

3. Using a tool like Postman, post a sample email to the url found in the http trigger   
    The body of the request contains a prompt and a type.  The type is set to empty, because this a flow control value for the sample application.   
    Depending on the tool used, the email may need to be html encoded.    
    ```JSON
    {
        "prompt" : "Dear Contoso,
                    I recently purchased a dress from your store and was disappointed to find that it was much shorter than expected. The dress I received was not the same length as the one pictured on your website.
                    I am writing to request an exchange for a dress of the correct length. I understand that the product I received was not what I expected, and I would like to receive a dress that is the same length as the one pictured on your website.
                    I look forward to hearing from you soon.
                    Sincerely,
                    Liea Organa",
        "type" : ""
    }
    ```
    ![Http trigger](../../../images/intentdetection/step3oai.jpg)  
    ![OpenAI Postman](../../../images/intentdetection/step3oaipostman.jpg)  
    
    The response sentiment and source value of OpenAI identifies the sample flow condition selected.  
    ```JSON
    {
    "department": "Fashion",
    "orderIntent": "Exchange",
    "source": "OpenAI"
    }
    ```  
### Azure OpenAI
2. Add the Azure OpenAI endpoint, Azure OpenAI Key and The Azure OpenAI Studio deployment model name to the initialize steps  
![Step on Logic App](../../../images/intentdetection/step2azoai.jpg)  
![azure portal open ai key](../../../images/sentimentanalysis/openaikeys.jpg)  
![azure portal open ai deployment](../../../images/sentimentanalysis/deployments.jpg)  

3.  Using a tool like Postman, post a sample email to the url found in the http trigger   
    The body of the request contains a prompt and a type.  The type is set to empty, because this a flow control value for the sample application.   
    Depending on the tool used, the email may need to be html encoded.    
    ```JSON
    {
        "prompt" : "Dear Contoso,
                    I recently purchased a dress from your store and was disappointed to find that it was much shorter than expected. The dress I received was not the same length as the one pictured on your website.
                    I am writing to request an exchange for a dress of the correct length. I understand that the product I received was not what I expected, and I would like to receive a dress that is the same length as the one pictured on your website.
                    I look forward to hearing from you soon.
                    Sincerely,
                    Liea Organa",
        "type" : "azure"
    }
    ```
    ![Http trigger](../../../images/intentdetection/step3oai.jpg)  
    ![OpenAI Postman](../../../images/intentdetection/step3azureoaipostman.jpg)
    
