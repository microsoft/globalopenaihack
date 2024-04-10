using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.ComponentModel;

public class CelebratePlugin
{
    [KernelFunction]
    [Description("Generates text to celebrate a given special occasion")]
    public async Task<string> GenerateOccasionCard(Kernel kernel, string input)
    {
        string promptTemplate = """
            generate text to celebrate a given special occasion: {{$input}}
            """;

        KernelFunction function = kernel.CreateFunctionFromPrompt(promptTemplate);
        FunctionResult result = await kernel.InvokeAsync(function, new(){{"input", input}});

        return result.GetValue<string>()!;
    }

    [KernelFunction]
    [Description("Generate five gift ideas to celebrate a given special occasion")]
    public async Task<string> GenerateGiftIdeas(Kernel kernel, string input)
    {
        string promptTemplate = """
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
        
        OpenAIPromptExecutionSettings executionSettings = new()
        {
            MaxTokens = 1_000,
            Temperature = 0.0
        };

        KernelFunction function = kernel.CreateFunctionFromPrompt(promptTemplate, executionSettings);

        FunctionResult result = await kernel.InvokeAsync(function, new(){{"input", input}});

        return result.GetValue<string>()!;
    }
}
