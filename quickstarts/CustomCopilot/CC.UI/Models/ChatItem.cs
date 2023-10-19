using Microsoft.Maui.Graphics.Converters;
using System.Globalization;

namespace CC.UI.Models;

public class ChatAction
{
    public ChatAction() { }
    public ChatAction(string description) { Description = description; }
    public string Description { get; set; } = string.Empty;
    public delegate void ActionDelegate();
}

public class ChatItem
{
    public ChatItem() { }
    // cheating :)
    public ChatItem(string author, string description, params string[] actionDescriptions)
    {
        Author = author;
        Text = description;
        Timestamp = DateTime.Now;
        Actions.AddRange(actionDescriptions.Select(a => new ChatAction(a)));

        BackgroundColor = author switch
        {
            "Copilot" => "Red",//"#FFC0C0C0";
            "Patient" => "Yellow",//"#FFC0C0FF";
            "You" => "Blue",//"#FFC0FFC0";
            _ => "White",//"#FFFFFFFF";
        };
        HorizontalAlignment = author == "You" ? "Start" : "End";
    }

    public string Text { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; }

    public List<ChatAction> Actions { get; } = new();

    public string BackgroundColor { get; set; }

    public string HorizontalAlignment { get; set; }
}
