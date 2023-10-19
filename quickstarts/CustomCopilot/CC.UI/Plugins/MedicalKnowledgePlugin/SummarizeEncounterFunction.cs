using Microsoft.SemanticKernel.TemplateEngine;

namespace CC.UI.Plugins.MedicalKnowledgePlugin
{
    internal static class SummarizeEncounterFunction
    {
        public static string Prompt { get; } = """
            User: {{$transcription}}
            Summarize the above medical encounter in less than 200 words.
            At the end, include a bulleted list of tasks that need to be completed and who is owning each task.
            """;

        public static string PluginName { get; } = "MedicalKnowledgePlugin";

        public static string Functionname { get; } = "SummarizeEncounter";

        public static PromptTemplateConfig PromptTemplateConfig { get; }

        public static PromptTemplate PromptTemplate { get; }

        static SummarizeEncounterFunction()
        {
            PromptTemplateConfig = new()
            {
                Description = "Summarize a medical encounter from a transcription and capture any assigned tasks."
            };

            // TODO: I don't understand how to use this :(
            //PromptTemplate = new(Prompt, PromptTemplateConfig, );
        }
    }
}
