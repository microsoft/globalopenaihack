using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using System.ComponentModel;

namespace CC.UI.Plugins.MedicalKnowledgePlugin
{
    public class MedicalKnowledgePlugin
    {
        private readonly IKernel _kernel;

        public MedicalKnowledgePlugin(IKernel kernel)
        {
            _kernel = kernel;
        }

        [SKFunction, Description("Summarize a medical encounter and capture specific actions.")]
        public async Task<string> SummarizeEncounter(string transcription)
        {
            string prompt = $"""
                User: {transcription}
                Summarize the above medical encounter in less than 200 words.
                At the end, include a bulleted list of tasks that need to be completed and who is owning each task.
                """;

            KernelResult result = await _kernel.RunAsync(prompt);

            if (result is null)
                throw new ApplicationException("No value returned from kernel.");

            return result.GetValue<string>();
        }
    }
}
