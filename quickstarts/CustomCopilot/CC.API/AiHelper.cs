using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;

namespace CC.API
{
    public class AiHelper
    {
        private readonly string _languageEndpoint = "https://ai-cc.openai.azure.com/";

        private readonly string _imageEndpoint = "https://ai-cc.openai.azure.com/";

        private readonly string _speechEndpoint = "https://ai-cc.openai.azure.com/";

        private readonly string _model = "gpt-4";

        private readonly IKernel _kernel;

        public AiHelper(string apiKey)
        {
            _kernel = Kernel.Builder
                .WithAzureChatCompletionService(_model, _languageEndpoint, apiKey)
                .Build();

            var pluginsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");
            _kernel.ImportSemanticFunctionsFromDirectory(pluginsDirectory, "ComicBookPlugin");
        }

        public async Task<List<string>> GeneratePatients()
        {
            ISKFunction getPopularCharacters = _kernel.Functions.GetFunction("ComicBookPlugin", "GetPopularCharacters");
            KernelResult result1 = await _kernel.RunAsync("20", getPopularCharacters);
            string? result1String = result1.GetValue<string>();
            List<string> characterNames = new();
            if (result1String != null)
            {
                characterNames = result1String.Split('|').ToList<string>();
            }

            return characterNames;
        }

        public async Task<string> GenerateFhirOfPatient(string characterName)
        {
            ISKFunction generateFhir = _kernel.Functions.GetFunction("ComicBookPlugin", "GenerateFhir");
            KernelResult result2 = await _kernel.RunAsync(characterName, generateFhir);
            string? value = result2.GetValue<string>();
            if (value is null)
                throw new ApplicationException("No value returned from kernel.");

            int firstJsonCharacter = value.IndexOf("```json\n") + 8;
            int lastJsonCharacter = value.IndexOf("\n```\n");
            int jsonLength = lastJsonCharacter - firstJsonCharacter;
            string justJson = value.Substring(firstJsonCharacter, jsonLength);

            string filename = $"\\Data\\Patients\\{characterName}.json";
            string filePath = $"C:\\src\\CC\\CC.API{filename}";
            using (StreamWriter writetext = new(filePath))
            {
                writetext.WriteLine(justJson);
            }

            return justJson;
        }

        //public async Task<string> GenerateFhirEncounters(string characterName)
        //{
        //    ISKFunction generateEncounters = _kernel.Functions.GetFunction("ComicBookPlugin", "GenerateFhirEncounters");
        //}
    }
}
