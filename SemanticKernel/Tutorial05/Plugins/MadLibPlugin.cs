using Microsoft.SemanticKernel;

public class MadLibPlugin
{
    [KernelFunction()]
    public async Task<string> GenerateMadLib(Kernel kernel, KernelArguments arguments)
    {
        // split the string representation of the random numbers into a list of integers
        ArgumentNullException.ThrowIfNull(arguments);
        string numbersString = arguments["randomNumbers"] != null ? Convert.ToString(arguments["randomNumbers"])! : string.Empty;
        List<int> numbers = numbersString.Split(',').Select(int.Parse).ToList();

        // define a few-shot function used to generate the mad lib with blanks for the number of word types
        string promptTemplate = """
            Use '{{$madLibTheme}}' as the theme to generate an amusing mad lib.
            
            EXAMPLES:
            User: Generate a mad lib with blanks for 3 adjectives, 3 nouns, and 2 verbs. Use 'vampires' as the theme for the mad lib.
            MadLib: The ___(adjective)___ vampire was ___(verb)___ by a ___(adjective)___ ___(noun)___. It was very ___(adjective)___ in it's ___(noun)___ so it returned to ___(noun)___.

            User: Generate a mad lib with blanks for 1 adjectives, 2 nouns, and 0 verbs. Use 'carnival' as the theme for the mad lib.
            MadLib: The ___(noun)___ Carnival was ___(adjective)___ again this year so they changed the name to the ___(noun)___ Carnival!
            """ +
            $"\n\nUser: Generate a mad lib with blanks for {numbers[0]} adjectives, {numbers[1]} nouns, and {numbers[2]} verbs.\nMadLib: ";

        KernelFunction function = kernel.CreateFunctionFromPrompt(promptTemplate);

        // call OpenAI to generate the specified number of adjectives, nouns, and verbs
        FunctionResult result = await function.InvokeAsync(kernel, arguments);

        // set the madLib context variable to the generated mad lib
        arguments["madLib"] = result.GetValue<string>()!;

        return arguments["madLib"] != null ? Convert.ToString(arguments["madLib"])! : string.Empty;
    }
}
