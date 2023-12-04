// Copyright (c) Microsoft. All rights reserved.
using System;
using System.IO;
using System.Text.Json;
using Microsoft.SemanticKernel;

public class MySettings {
    public string Type { get; set; } = "azure";
    public AzureOpenAI AzureOpenAI { get; set; } = new();
    public OpenAI OpenAI { get; set; } = new();
}

public class AzureOpenAI {
    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string ChatDeployment { get; set; } = string.Empty;
    public string CompletionsDeployment { get; set; } = string.Empty;
}

public class OpenAI {
    public string ApiKey { get; set; } = string.Empty;
    public string OrgId { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
}

public static class Settings
{
    private const string DefaultConfigFile = "../config/settings.json";
    
    /// <summary>
    /// Load settings from file
    /// </summary>
    public static MySettings LoadFromFile(string configFile = DefaultConfigFile)
    {
        if (!File.Exists(DefaultConfigFile))
        {
            Console.WriteLine($"Configuration not found: {DefaultConfigFile}");
            Console.WriteLine("Please create a settings.json in the config folder. See Tutorial00.");
            throw new ApplicationException("Configuration not found, please setup the notebooks first.");
        }

        try
        {
            MySettings settings = JsonSerializer.Deserialize<MySettings>(File.ReadAllText(DefaultConfigFile));

            if (string.IsNullOrWhiteSpace(settings.Type)) throw new ApplicationException("Type is not set in settings.json");
            if (settings.Type == "azure")
            {
                if (string.IsNullOrWhiteSpace(settings.AzureOpenAI.Endpoint)) 
                    Console.WriteLine("AzureOpenAI.Endpoint is not set in settings.json");
                if (string.IsNullOrWhiteSpace(settings.AzureOpenAI.ApiKey)) 
                    Console.WriteLine("AzureOpenAI.ApiKey is not set in settings.json");
                if (string.IsNullOrWhiteSpace(settings.AzureOpenAI.ChatDeployment) && string.IsNullOrWhiteSpace("AzureOpenAI.CompletionsDeployment")) 
                    Console.WriteLine("Either AzureOpenAI.ChatDeployment and/or AzureOpenAI.CompletionsDeployment must be set in settings.json");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(settings.OpenAI.ApiKey))
                    Console.WriteLine("OpenAI.ApiKey is not set in settings.json");
                if (string.IsNullOrWhiteSpace(settings.OpenAI.Model))
                    Console.WriteLine("OpenAI.Model is not set in settings.json");
            }

            return settings;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Something went wrong: {e.Message}");
            return null;
        }
    }
}