
using System.Text;
using Newtonsoft.Json;

string apiUrl = "https://api.openai.com/v1/completions";
string apiKey = "Your-OpenAI-API-Key";

var client = new HttpClient();
var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
request.Headers.Add("Authorization", $"Bearer {apiKey}");

string statement = "I don't like Pizza.";
var body = new {
    model = "text-curie-001", // Specify the OpenAI model you wish to use
    prompt = $"Classify the sentiment in this text: {statement}",
    temperature = 0,
    max_tokens = 60,
    top_p = 1,
    frequency_penalty = 0,
    presence_penalty = 0
};
var objAsJson = JsonConvert.SerializeObject(body);
request.Content = new StringContent(objAsJson, Encoding.UTF8, "application/json");

var response = await client.SendAsync(request);

string jsonResponse = await response.Content.ReadAsStringAsync();
dynamic? result = JsonConvert.DeserializeObject(jsonResponse);
Console.WriteLine(result!.choices[0].text);