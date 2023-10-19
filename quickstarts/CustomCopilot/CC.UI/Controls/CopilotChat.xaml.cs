using CC.UI.Models;
using System.Collections.ObjectModel;

namespace CC.UI.Controls;

public partial class CopilotChat : ContentView
{
    public ObservableCollection<ChatItem> ChatHistory
    {
        get => (ObservableCollection<ChatItem>)GetValue(ChatHistoryProperty);
        set => SetValue(ChatHistoryProperty, value);
    }

    public static readonly BindableProperty ChatHistoryProperty = 
        BindableProperty.Create(nameof(ChatHistory), typeof(ObservableCollection<ChatItem>), typeof(CopilotChat), new ObservableCollection<ChatItem>());

    public event EventHandler<NewChatEventArgs> SubmitChat;

    public CopilotChat()
	{
		InitializeComponent();
    }

    //TODO: just for testing UI; remove once we get real data
    private void OnGenerateChatHistoryClicked(object sender, EventArgs e)
    {
        ChatItem chat09 = new()
        {
            Author = "You",
            Text = "Your strep test came back positive.",
            Timestamp = new DateTime(2023, 10, 18, 8, 51, 0)
        };
        ChatHistory.Add(chat09);

        ChatItem chat10 = new()
        {
            Author = "You",
            Text = "I'd like to prescribe you an antibiotic to fight the infection.",
            Timestamp = new DateTime(2023, 10, 18, 8, 51, 30)
        };
        ChatHistory.Add(chat10);

        ChatItem chat20 = new()
        {
            Author = "Copilot",
            Text = "Reminder: This patient is allergic to penicillin.",
            Timestamp = new DateTime(2023, 10, 18, 8, 51, 45)
        };
        chat20.Actions.Add("List alternatives.");
        chat20.Actions.Add("View allergies.");
        ChatHistory.Add(chat20);

        ChatItem chat30 = new()
        {
            Author = "You",
            Text = "Since you are allergic to penicillin, I'm going to write you a prescription for Keflex.",
            Timestamp = new DateTime(2023, 10, 18, 8, 52, 30)
        };
        ChatHistory.Add(chat30);

        ChatItem chat40 = new()
        {
            Author = "Copilot",
            Text = "Do you want me to send the prescription to the patient's preferred pharmacy?",
            Timestamp = new DateTime(2023, 10, 18, 8, 53, 30)
        };
        chat40.Actions.Add("Submit medication request");
        ChatHistory.Add(chat40);

        ChatItem chat50 = new()
        {
            Author = "Patient",
            Text = "Is Keflex my cheapest option?",
            Timestamp = new DateTime(2023, 10, 18, 8, 54, 0)
        };
        ChatHistory.Add(chat50);

        ChatItem chat60 = new()
        {
            Author = "You",
            Text = "Let me see.",
            Timestamp = new DateTime(2023, 10, 18, 8, 54, 30)
        };
        ChatHistory.Add(chat60);

        ChatItem chat70 = new()
        {
            Author = "Copilot",
            Text = "Yes, based on the Patient's insurance, Keflex is their cheapest option.",
            Timestamp = new DateTime(2023, 10, 18, 8, 54, 0)
        };
        chat70.Actions.Add("View insurance");
        chat70.Actions.Add("View alternative medications");
        ChatHistory.Add(chat70);
    }

    private void SubmitChatBtn_Clicked(object sender, EventArgs e)
    {
        ChatItem chatItem = new()
        {
            Author = "You",
            Text = ChatEntry.Text,
            Timestamp = DateTime.Now
        };
        SubmitChat?.Invoke(this, new NewChatEventArgs() { ChatItem = chatItem });
    }
}