using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.SkillDefinition;

public class OldeEnglishSkill2
{
    [SKFunction("Given a word or phrase, translate it into Olde English")]
    [SKFunctionInput(Description = "The word or phrase to translate")]
    public async Task<string> Translate(string input)
    {
        // load settings
        MySettings settings = Settings.LoadFromFile();

        // configure AI backend used by the kernel
        var builder = new KernelBuilder();
        if (settings.Type == "azure")
            builder.WithAzureTextCompletionService(settings.AzureOpenAI.CompletionsDeployment, settings.AzureOpenAI.Endpoint, settings.AzureOpenAI.ApiKey);
        else
            builder.WithOpenAITextCompletionService(settings.OpenAI.Model, settings.OpenAI.ApiKey, settings.OpenAI.OrgId);
        IKernel kernel = builder.Build();

        // define function
        string functionDefinition = """
        Translate the following into Olde English: {{$input}}
        """;

        // add function to the kernel
        var functionInstance = kernel.CreateSemanticFunction(functionDefinition);
        
        // call OpenAI API using the function and provided input
        var completion = await functionInstance.InvokeAsync(input);

        return completion.Result;
    }
}
