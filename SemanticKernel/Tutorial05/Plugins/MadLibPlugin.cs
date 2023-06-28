using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.SkillDefinition;

public class MadLibPlugin
{
    private IKernel _kernel;

    public MadLibPlugin(IKernel kernel)
    {
        _kernel = kernel;
    }

    [SKFunction()]
    public async Task<string> GenerateMadLib(
        [SKName("randomNumbers")] string randomNumbers,
        [SKName("madLibTheme")] string madLibTheme, 
        SKContext context)
    {
        // split the string representation of the random numbers into a list of integers
        List<int> numbers = context["randomNumbers"].Split(',').Select(int.Parse).ToList();

        // define a few-shot function used to generate the mad lib with blanks for the number of word types
        string functionDefinition =
            """
            Use '{{$madLibTheme}}' as the theme to generate an amusing mad lib.
            
            EXAMPLES:
            User: Generate a mad lib with blanks for 3 adjectives, 3 nouns, and 2 verbs. Use 'vampires' as the theme for the mad lib.
            MadLib: The ___(adjective)___ vampire was ___(verb)___ by a ___(adjective)___ ___(noun)___. It was very ___(adjective)___ in it's ___(noun)___ so it returned to ___(noun)___.

            User: Generate a mad lib with blanks for 1 adjectives, 2 nouns, and 0 verbs. Use 'carnival' as the theme for the mad lib.
            MadLib: The ___(noun)___ Carnival was ___(adjective)___ again this year so they changed the name to the ___(noun)___ Carnival!
            """ + 
            $"\n\nUser: Generate a mad lib with blanks for {numbers[0]} adjectives, {numbers[1]} nouns, and {numbers[2]} verbs.\nMadLib: ";

        var functionInstance = _kernel.CreateSemanticFunction(functionDefinition);

        // call OpenAI to generate the specified number of adjectives, nouns, and verbs
        var completion = await functionInstance.InvokeAsync(context);

        // set the madLib context variable to the generated mad lib
        context["madLib"] = completion.Result;

        Console.WriteLine($"madLib: {context["madLib"]}");

        return context["madLib"];
    }
}
