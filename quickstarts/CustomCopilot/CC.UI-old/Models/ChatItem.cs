namespace CC.UI.Models
{
    public class ChatItem
    {
        public string Text { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }

        public List<string> Actions { get; } = new();

        public string BackgroundColor
        {
            get
            {
                switch (Author)
                {
                    case "Copilot":
                        return "#FFC0C0C0";
                    case "Patient":
                        return "#FFC0C0FF";
                    case "You":
                        return "#FFC0FFC0";
                    default:
                        return "#FFFFFFFF";
                }
            }
        }

        public string HorizontalAlignment
        {
            get
            {
                if (string.Compare(Author, "You") == 0)
                    return "Right";
                else
                    return "Left";
            }
        }
    }
}
