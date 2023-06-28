using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.SkillDefinition;
using System.ComponentModel;

public class ChaosPlugin
{
    private IKernel _kernel;

    public ChaosPlugin(IKernel kernel)
    {
        _kernel = kernel;
    }
    
    [SKFunction()]
    public async Task<string> GenerateRandomNumbers(
        [SKName("count")][DefaultValue(3)] int count,
        [SKName("lowerBound")][DefaultValue(0)] int lowerBound,
        [SKName("upperBound")][DefaultValue(5)] int upperBound, 
        SKContext context)
    {
        List<int> numbers = new();
        for (int i = 0; i < count; i++)
        {
            numbers.Add(new Random().Next(lowerBound, upperBound));
        }

        context["randomNumbers"] = string.Join(",", numbers);
        
        Console.WriteLine($"randomNumbers: {context["randomNumbers"]}");

        return context["randomNumbers"];
    }
    
    [SKFunction()]
    public async Task<string> GenerateRandomWords(
        [SKName("randomNumbers")] string randomNumbers, 
        SKContext context)
    {
        List<int> numbers = randomNumbers.Split(',').Select(int.Parse).ToList();

        string functionDefinition = 
            """
            Generate random words of different types.
            
            EXAMPLE:
            User: Generate 3 nouns, 2 verbs, and 3 adjectives.
            Assistant: nouns: cat, town, house; verbs: run, jump; adjectives: red, funny, heavy
            """ + 
            $"Generate {numbers[0]} nouns, {numbers[1]} verbs, and {numbers[2]} adjectives.\nAssistant:";

        var functionInstance = _kernel.CreateSemanticFunction(functionDefinition);

        var completion = await functionInstance.InvokeAsync(context);

        context["randomWords"] = completion.Result;

        Console.WriteLine($"randomWords: {context["randomWords"]}");

        return context["randomWords"];
    }
}