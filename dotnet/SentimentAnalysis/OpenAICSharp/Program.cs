
using System.Text;
using Newtonsoft.Json;


string apiUrl = "https://api.openai.com/v1/completions";
string apiKey = "Your-OpenAI-API-Key";

string statement = "I can't stand homework";

var client = new HttpClient();
var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
request.Headers.Add("Authorization", $"Bearer {apiKey}");
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
var content = new StringContent(objAsJson, Encoding.UTF8, "application/json");

request.Content = content;
var response = await client.SendAsync(request);
response.EnsureSuccessStatusCode();

string jsonResponse = await response.Content.ReadAsStringAsync();
dynamic? result = JsonConvert.DeserializeObject(jsonResponse);
Console.WriteLine(result!.choices[0].text);