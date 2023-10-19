using CC.UI.Models;
using CC.UI.Plugins.ApplicationHelpPlugin;
using CC.UI.Plugins.MedicalKnowledgePlugin;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.Planners;
using Microsoft.SemanticKernel.Planning;

namespace CC.UI.Services
{
    internal static class AiServices
    {
        private static readonly string _languageEndpoint = "https://ai-cc.openai.azure.com/";

        private static readonly string _imageEndpoint = "https://ai-cc.openai.azure.com/";

        private static readonly string _speechEndpoint = "https://ai-cc.openai.azure.com/";

        private static readonly string _model = "gpt-4";

        internal static readonly IKernel _kernel;

        static AiServices()
        {
            string apiKey = "88eb9b24ebb8415faf674b699b3a490f";
            _kernel = Kernel.Builder
                .WithAzureChatCompletionService(_model, _languageEndpoint, apiKey)
                .Build();

            _kernel.ImportFunctions(new ApplicationHelpPlugin(_kernel), "ApplicationHelpPlugin");
            _kernel.ImportFunctions(new MedicalKnowledgePlugin(_kernel), "MedicalKnowledgePlugin");
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
            ISKFunction summarize = _kernel.Functions.GetFunction("MedicalKnowledgePlugin", "SummarizeEncounter");
            KernelResult result1 = await _kernel.RunAsync(transcription, summarize);
            string result1String = result1.GetValue<string>();
            
            if (result1String is null)
                throw new ApplicationException("No value returned from kernel.");

            return result1String;
        }
    }
}
