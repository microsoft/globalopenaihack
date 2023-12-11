using Microsoft.SemanticKernel;
using System.ComponentModel;

public class ChaosPlugin
{    
    [KernelFunction()]
    public async Task<string> GenerateRandomNumbers(Kernel kernel, KernelArguments arguments)
    {
        // NOTE: this function doesn't make any calls to OpenAI LLM. It just uses .NET's random number generator.
        int countValue = arguments["count"] != null ? Convert.ToInt32(arguments["count"]) : 3;
        int lowerBoundValue = arguments["lowerBound"] != null ? Convert.ToInt32(arguments["lowerBound"]) : 0;
        int upperBoundValue = arguments["upperBound"] != null ? Convert.ToInt32(arguments["upperBound"]) : 5;

        List<int> numbers = new();
        // loop a number of times equal to the count parameter
        for (int i = 0; i < countValue; i++)
        {
            // generate a random number between the lower and upper bound parameters
            numbers.Add(new Random().Next(lowerBoundValue, upperBoundValue));
        }

        // set the randomNumbers context variable to a comma-separated string of the random numbers
        arguments["randomNumbers"] = string.Join(",", numbers);

        return arguments["randomNumbers"] != null ? Convert.ToString(arguments["randomNumbers"])! : string.Empty;
    }
    
    [KernelFunction()]
    public async Task<string> GenerateRandomWords(Kernel kernel, KernelArguments arguments)
    {
        // split the string representation of the random numbers into a list of integers
        string randomNumbers = arguments["randomNumbers"] != null ? Convert.ToString(arguments["randomNumbers"])! : string.Empty;
        List<int> numbers = randomNumbers.Split(',').Select(int.Parse).ToList();

        // define a one-shot function used to generate the random words
        string promptTemplate =
            """
            Generate random words of different types.

            EXAMPLE:
            User: Generate 3 adjectives, 3 nouns, and 2 verbs.
            Assistant: adjectives: red, funny, heavy; nouns: cat, town, house; verbs: run, jump
            """ +
            $"User: Generate {numbers[0]} adjectives, {numbers[1]} nouns, and {numbers[2]} verbs.\nAssistant:";

        KernelFunction function = kernel.CreateFunctionFromPrompt(promptTemplate);

        // call OpenAI to generate the specified number of adjectives, nouns, and verbs
        FunctionResult result = await function.InvokeAsync(kernel, arguments);

        // set the randomWords context variable to a comma-separated string of the random words
        arguments["randomWords"] = result.GetValue<string>();
        
        return arguments["randomWords"] != null ? Convert.ToString(arguments["randomWords"])! : string.Empty;
    }
}
