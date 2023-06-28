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
        // NOTE: this function doesn't make any calls to OpenAI LLM. It just uses .NET's random number generator.
        List<int> numbers = new();
        // loop a number of times equal to the count parameter
        for (int i = 0; i < count; i++)
        {
            // generate a random number between the lower and upper bound parameters
            numbers.Add(new Random().Next(lowerBound, upperBound));
        }

        // set the randomNumbers context variable to a comma-separated string of the random numbers
        context["randomNumbers"] = string.Join(",", numbers);
        
        Console.WriteLine($"randomNumbers: {context["randomNumbers"]}");

        return context["randomNumbers"];
    }
    
    [SKFunction()]
    public async Task<string> GenerateRandomWords(
        [SKName("randomNumbers")] string randomNumbers, 
        SKContext context)
    {
        // split the string representation of the random numbers into a list of integers
        List<int> numbers = randomNumbers.Split(',').Select(int.Parse).ToList();

        // define a one-shot function used to generate the random words
        string functionDefinition = 
            """
            Generate random words of different types.

            EXAMPLE:
            User: Generate 3 adjectives, 3 nouns, and 2 verbs.
            Assistant: adjectives: red, funny, heavy; nouns: cat, town, house; verbs: run, jump
            """ + 
            $"User: Generate {numbers[0]} adjectives, {numbers[1]} nouns, and {numbers[2]} verbs.\nAssistant:";

        var functionInstance = _kernel.CreateSemanticFunction(functionDefinition);

        // call OpenAI to generate the specified number of adjectives, nouns, and verbs
        var completion = await functionInstance.InvokeAsync(context);

        // set the randomWords context variable to a comma-separated string of the random words
        context["randomWords"] = completion.Result;

        Console.WriteLine($"randomWords: {context["randomWords"]}");

        return context["randomWords"];
    }
}
