using Microsoft.SemanticKernel;
using System.ComponentModel;

public class OldeEnglishPlugin2
{
    [KernelFunction()]
    [Description("Given a word or phrase, translate it into Olde English")]
    public async Task<string> TranslateAsync(Kernel kernel, string input)
{
    // define function
    string functionDefinition = """
        Translate the following into Olde English: {{$input}}
        """;

    // add function to the kernel
    KernelFunction toOldeEnglishFunction = kernel.CreateFunctionFromPrompt(functionDefinition);

    // call OpenAI API using the function and provided input
    FunctionResult result = await toOldeEnglishFunction.InvokeAsync(kernel, new(){{"input", input}});

    return result.GetValue<string>()!;
}
}
