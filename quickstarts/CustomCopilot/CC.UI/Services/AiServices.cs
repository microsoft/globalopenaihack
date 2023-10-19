using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;

namespace CC.UI.Services
{
    internal static class AiServices
    {
        private static readonly string _languageEndpoint = "https://ai-cc.openai.azure.com/";

        private static readonly string _imageEndpoint = "https://ai-cc.openai.azure.com/";

        private static readonly string _speechEndpoint = "https://ai-cc.openai.azure.com/";

        private static readonly string _model = "gpt-4";

        private static readonly IKernel _kernel;

        static AiServices()
        {
            string? apiKey = "5f9112856f554010b2cca1d19ac71730";
            _kernel = Kernel.Builder
                .WithAzureChatCompletionService(_model, _languageEndpoint, apiKey)
                .Build();

            var pluginsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
            _kernel.ImportSemanticFunctionsFromDirectory(pluginsDirectory, "MedicalKnowlegePlugin");
        }

        internal static async Task<string> SummarizeEncounter(string transcription)
        {
            ISKFunction summarize = _kernel.Functions.GetFunction("ComicBookPlugin", "SummarizeEncounter");
            KernelResult result1 = await _kernel.RunAsync(transcription, summarize);
            string? result1String = result1.GetValue<string>();
            if (result1String is null)
                throw new ApplicationException("No value returned from kernel.");

            return result1String;
        }
    }
}
