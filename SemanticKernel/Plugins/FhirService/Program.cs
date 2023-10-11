using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Planners;
using Microsoft.SemanticKernel.Functions.OpenAPI.Authentication;
using Microsoft.SemanticKernel.Functions.OpenAPI.Extensions;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI.ChatCompletion;
using Microsoft.SemanticKernel.Planning;
using Microsoft.SemanticKernel.Orchestration;

namespace FhirService
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to Semantic Kernel OpenAPI Plugins Example!");

            // Load configuration
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(path: "appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>()
                .Build();

            // Initialize logger
            using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
                builder.AddConfiguration(configuration.GetSection("Logging"))
                    .AddConsole()
                    .AddDebug());

            ILogger logger = loggerFactory.CreateLogger<Program>();

            // Initialize semantic kernel
            AIServiceOptions aiOptions = configuration.GetRequiredSection(AIServiceOptions.PropertyName).Get<AIServiceOptions>()
                ?? throw new InvalidOperationException($"Missing configuration for {AIServiceOptions.PropertyName}.");

            var kernel = Kernel.Builder
                .WithLoggerFactory(loggerFactory)
                .WithAzureChatCompletionService(aiOptions.Models.Completion, aiOptions.Endpoint, aiOptions.Key)
                .Build();

            // Import Native functions for FhirPlugin
            var fhirFunctions = kernel.ImportFunctions(new FhirRequestPlugin(), "FhirPlugin");

            // Now we can use planner to determine which functions to call


            foreach (var f in fhirFunctions)
            {
                Console.WriteLine(f.Key);
            }

            var getPatient = new Plan(fhirFunctions["GetPatient"]);
            getPatient.Parameters["identifier"] = "123";
            getPatient.Outputs.Add("patient");

            var t = kernel.ImportPlan(getPatient);

            var result = await t.InvokeAsync("Get a patient record with patient identifier 123.", kernel);


            Console.WriteLine(result.GetValue<string>());
        }
    }
}
