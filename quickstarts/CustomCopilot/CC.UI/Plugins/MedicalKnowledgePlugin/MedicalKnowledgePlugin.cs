using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.TemplateEngine;
using System.ComponentModel;
using static Microsoft.SemanticKernel.TemplateEngine.PromptTemplateConfig;

namespace CC.UI.Plugins.MedicalKnowledgePlugin
{
    public class MedicalKnowledgePlugin
    {
        private readonly IKernel _kernel;

        public MedicalKnowledgePlugin(IKernel kernel)
        {
            _kernel = kernel;
        }

        [SKFunction, Description("Summarize a clinical encounter and capture specific actions.")]
        public async Task<string> SummarizeEncounter(string transcription)
        {
            string prompt = $"""
                User: {transcription}
                Summarize the above medical encounter in less than 200 words.
                At the end, include a bulleted list of tasks that need to be completed and who is owning each task.
                """;

            if (!_kernel.Functions.TryGetFunction("WhichPage", out ISKFunction summarizeEncounterFunction))
            {
                var promptConfig = new PromptTemplateConfig
                {
                    Description = "summarizes a clinical encounter with a patient",
                    Input =
                     {
                        Parameters = new List<InputParameter>
                        {
                            new InputParameter
                            {
                                Name = "input",
                                Description = "The user's request.",
                                DefaultValue = ""
                            }
                        }
                    }
                };

                var promptTemplate = new PromptTemplate(
                    prompt,
                    promptConfig,
                    _kernel
                );
                PromptTemplateConfig functionConfig = new();
                summarizeEncounterFunction = _kernel.RegisterSemanticFunction("SummarizeEncounter", functionConfig, promptTemplate);
            }

            KernelResult result = await _kernel.RunAsync(prompt, summarizeEncounterFunction);

            if (result is null)
                throw new ApplicationException("No value returned from kernel.");

            return result.GetValue<string>();
        }
    }
}
