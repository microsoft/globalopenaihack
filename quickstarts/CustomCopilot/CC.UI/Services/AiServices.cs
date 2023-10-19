using CC.UI.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.Planners;
using Microsoft.SemanticKernel.Planning;
using Microsoft.SemanticKernel.TemplateEngine;

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
            string apiKey = "5f9112856f554010b2cca1d19ac71730";
            _kernel = Kernel.Builder
                .WithAzureChatCompletionService(_model, _languageEndpoint, apiKey)
                .Build();

            //string pluginsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
            //string pluginsDirectory = FileSystem.Current.AppDataDirectory;
            //_kernel.ImportSemanticFunctionsFromDirectory(pluginsDirectory, "MedicalKnowlegePlugin");
            //_kernel.ImportSemanticFunctionsFromDirectory(pluginsDirectory, "ApplicationHelpPlugin");

            //_kernel.RegisterSemanticFunction(
            //    "MedicalKnowledgePlugin", "SummarizeEncounter", 
            //    new PromptTemplateConfig(), new PromptTemplate());
        }

        internal static async Task<Plan> CreateActionPlan(ChatItem chatItem)
        {
            ActionPlanner planner = new(_kernel);
            Plan plan = await planner.CreatePlanAsync(chatItem.Text);

            return plan;
        }

        internal static async Task<string> ExecutePlan(Plan plan)
        {
            FunctionResult result = await plan.InvokeAsync(_kernel);
            string resultString = result.GetValue<string>();

            if (resultString is null)
                throw new ApplicationException("No value returned from kernel.");

            return resultString;
        }

        internal static async Task<string> SummarizeEncounter(string transcription)
        {
            ISKFunction summarize = _kernel.Functions.GetFunction("ComicBookPlugin", "SummarizeEncounter");
            KernelResult result1 = await _kernel.RunAsync(transcription, summarize);
            string result1String = result1.GetValue<string>();
            
            if (result1String is null)
                throw new ApplicationException("No value returned from kernel.");

            return result1String;
        }
    }
}
