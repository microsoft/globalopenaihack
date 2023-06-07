// Copyright (c) Microsoft. All rights reserved.
using System;
using System.IO;
using System.Text.Json;

public class MySettings {
    public string Type { get; set; } = string.Empty;
    public AzureOpenAI AzureOpenAI { get; set; } = new();
    public OpenAI OpenAI { get; set; } = new();
}

public class AzureOpenAI {
    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
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

            return settings;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Something went wrong: {e.Message}");
            return null;
        }
    }
}