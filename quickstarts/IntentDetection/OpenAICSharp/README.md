# OpenAI Intent Detection

This code is an example of how to use OpenAI's Completion API to determine the intent of a statement and then act on the reponse.  
[OpenAI API Reference](https://platform.openai.com/docs/api-reference/completions)  

## Getting Started

### Prerequisites

* An [OpenAI account](https://openai.com/)  
[Video on how to open an account](https://www.youtube.com/watch?v=zJSYMWlCcPY)
* API key from the [OpenAI portal](https://platform.openai.com/)  

## Usage

Clone this repository to your local machine.

Open the solution in your developer environment ([Visual Studio Code](https://code.visualstudio.com/), Visual Studio, etc.).

Replace the key variable in Program.cs with your own value from the Open AI portal.  
```C#
Program.cs

string apiUrl = "https://api.openai.com/v1/completions";
string apiKey = "Your-OpenAI-API-Key";
```  

To find the API Key sign into the portal, and click the user profile in the top right corner  
![open ai dropdown](../../../images/sentimentanalysis/openaidropdown.jpg)  

Create and store the new key, you will not be able to retieve the value once the create dialog is closed. 
![open ai dropdown](../../../images/sentimentanalysis/openaiSecret.jpg)  


Run the code.  
```dotnetcli
dotnet run
```
![azure portal open ai key](../../../images/intentdetection/azureopenaisharpoutput.jpg)  
  

## Code Explanation

First, set the key for the OpenAI service, instantiate the HttpClient, HttpRequestMessage and add the Authorization header.  
```C#
using System.Text;
using Newtonsoft.Json;

string apiUrl = "https://api.openai.com/v1/completions";
string apiKey = "Your-OpenAI-API-Key"; // Replace with your API key

var client = new HttpClient();
var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
request.Headers.Add("Authorization", $"Bearer {apiKey}");
```  

Create a prompt that includes instructions for the model's behavior and an example response.    
```C#
string text = $@"This is an agent used to detect intent and provide a Department value for the following categories: “Books”, “Home”, “Fashion”, “Electronics”, “Grocery”, “Others”
---
Sample Output
Department: Electronics
Order Intent: Defect

Department: Grocery
Order Intent: Rotten food
---
Text: {book}";
```  

Serialize, encode as JSON and set the request content.  
```C#
var objAsJson = JsonConvert.SerializeObject(body);
request.Content = new StringContent(objAsJson, Encoding.UTF8, "application/json");
```

Finally, call the client endpoint, deserialize and parsed to a department and orderIntet variable.  
```C#
var response = await client.SendAsync(request);

string jsonResponse = await response.Content.ReadAsStringAsync();
dynamic? result = JsonConvert.DeserializeObject(jsonResponse);
string completion = result!.choices[0].text;

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
```  
After the information is parsed, actions like sending an email, calling a function or raising an event can occurr.
  
  


