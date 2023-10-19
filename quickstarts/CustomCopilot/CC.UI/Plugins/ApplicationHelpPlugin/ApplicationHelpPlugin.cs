using CC.UI.Pages;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.TemplateEngine;
using System.ComponentModel;
using static Microsoft.SemanticKernel.TemplateEngine.PromptTemplateConfig;

namespace CC.UI.Plugins.ApplicationHelpPlugin
{
    public class ApplicationHelpPlugin
    {
        private readonly IKernel _kernel;

        public ApplicationHelpPlugin(IKernel kernel)
        {
            _kernel = kernel;
        }

        [SKFunction, Description("Gets the likeliest name of the page the user is looking for.")]
        public async Task<string> NavigateApplicationPages(string input)
        {
            List<string> pageNames = new()
            {
                $"{nameof(MainPage)}: {MainPage.Description}", 
                $"{nameof(EncounterPage)}: {EncounterPage.Description}", 
                $"{nameof(PatientDetailPage)}: {PatientDetailPage.Description}", 
                $"{nameof(SchedulingPage)}: {SchedulingPage.Description}"
            };
            string pageList = string.Join("; ", pageNames);

            string prompt = $"""
                User: {input}
                Based on the input above, which of these pages is the user most likely looking for: {pageList}?
                Return only one page name
                """;

            if (!_kernel.Functions.TryGetFunction("WhichPage", out ISKFunction whichPageFunction))
            {
                var promptConfig = new PromptTemplateConfig
                {
                    Description = "returns the most likely page the user is looking for",
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
                whichPageFunction = _kernel.RegisterSemanticFunction("WhichPage", functionConfig, promptTemplate);
            }

            KernelResult result = await _kernel.RunAsync(prompt, whichPageFunction);

            if (result is null)
                throw new ApplicationException("No value returned from kernel.");

            return result.GetValue<string>();
        }
    }
}
