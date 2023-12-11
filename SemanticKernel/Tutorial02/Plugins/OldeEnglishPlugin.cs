using Microsoft.SemanticKernel;
using System.ComponentModel;

public class OldeEnglishPlugin
{
    /// <summary>
    /// Given a word or phrase, translate it into Olde English
    /// </summary>
    /// <param name="input">The word or phrase to translate</param>
    /// <returns>The translated word or phrase</returns>
    [KernelFunction(), Description("Given a word or phrase, translate it into olde English")]
    public string Translate(string input)
    {
        return $"Translate the following into olde English: {input}";
    }
}
