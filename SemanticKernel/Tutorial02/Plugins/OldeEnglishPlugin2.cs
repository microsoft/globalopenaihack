using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.SkillDefinition;
using System.ComponentModel;

public class OldeEnglishPlugin2
{
    [SKFunction()]
    [Description("Given a word or phrase, translate it into Olde English")]
    public async Task<string> Translate([SKName("input")][Description("input is the built-in parameter")] string input)
    {
        // load settings (you can pass in the kernel to this plugin if you prefer, but this examples assumes you can't)
        MySettings settings = Settings.LoadFromFile();

        // configure AI backend used by the kernel
        KernelBuilder builder = new();
        if (settings.Type == "azure")
            builder.WithAzureTextCompletionService(settings.AzureOpenAI.CompletionsDeployment, settings.AzureOpenAI.Endpoint, settings.AzureOpenAI.ApiKey);
        else
            builder.WithOpenAITextCompletionService(settings.OpenAI.Model, settings.OpenAI.ApiKey, settings.OpenAI.OrgId);
        IKernel kernel = builder.Build();

        // define function
        string functionDefinition = "Translate the following into Olde English: {{$input}}";

        // add function to the kernel
        var functionInstance = kernel.CreateSemanticFunction(functionDefinition);
        
        // call OpenAI API using the function and provided input
        var completion = await functionInstance.InvokeAsync(input);

        return completion.Result;
    }
}
