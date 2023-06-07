using Microsoft.SemanticKernel.SkillDefinition;

public class OldeEnglishSkill
{
    [SKFunction("Given a word or phrase, translate it into Olde English")]
    [SKFunctionInput(Description = "The word or phrase to translate")]
    public string Translate(string input)
    {
        return $"Translate the following into olde English: {{$input}}";
    }
}
