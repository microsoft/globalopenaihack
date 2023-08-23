using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;
using System.ComponentModel;

public class CelebratePlugin
{
    private readonly IKernel _kernel;

    public CelebratePlugin(IKernel kernel)
    {
        _kernel = kernel;
    }

    [SKFunction]
    [Description("Generates text to celebrate a given special occasion")]
    public async Task<string> GenerateOccasionCard(string input)
    {
        string functionDefinition = """
            generate text to celebrate a given special occasion: {{$input}}
            """;
        ISKFunction function = _kernel.CreateSemanticFunction(functionDefinition);
        SKContext context = await _kernel.RunAsync(input, function);

        return context.Result;
    }

    [SKFunction]
    [Description("Generate five gift ideas to celebrate a given special occasion")]
    public async Task<string> GenerateGiftIdeas(string input)
    {
        string functionDefinition = """
            create a bulleted list of gift ideas to celebrate a given special occasion.
            the list should always include five items.

            EXAMPLES:
            USER: what should I get my wife for Valentine's Day?
            BOT: Here are five gift ideas for Valentine's Day:
            - A bouquet of roses
            - A box of chocolates
            - A bottle of wine
            - A romantic dinner
            - A weekend getaway

            USER: {{$input}}
            BOT: 
            """;
        ISKFunction function = _kernel.CreateSemanticFunction(functionDefinition, maxTokens: 1_000);
                
        SKContext context = await _kernel.RunAsync(input, function);

        return context.Result;
    }
}
