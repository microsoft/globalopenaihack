
using System.Text;
using Newtonsoft.Json;

string apiUrl = "https://api.openai.com/v1/completions";
string apiKey = "Your-OpenAI-API-Key";

var client = new HttpClient();
var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
request.Headers.Add("Authorization", $"Bearer {apiKey}");

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


string text = $@"This is an agent used to detect intent and provide a Department value for the following categories: “Books”, “Home”, “Fashion”, “Electronics”, “Grocery”, “Others”
---
Sample Output
Department: Electronics
Order Intent: Defect

Department: Grocery
Order Intent: Rotten food
---
Text: {book}";


var body = new {
    model = "text-davinci-003", // Specify the OpenAI model you wish to use
    prompt = text,
    temperature = 0,
    max_tokens = 100,
    top_p = 1,
    frequency_penalty = 0,
    presence_penalty = 0
};
var objAsJson = JsonConvert.SerializeObject(body);
request.Content = new StringContent(objAsJson, Encoding.UTF8, "application/json");

var response = await client.SendAsync(request);

string jsonResponse = await response.Content.ReadAsStringAsync();
dynamic? result = JsonConvert.DeserializeObject(jsonResponse);
string completion = result!.choices[0].text;

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
//Now parsed into department and orderIntent, you can use these to route the request to the appropriate department
Console.WriteLine(department);
Console.WriteLine(orderIntent);