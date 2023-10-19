using CC.UI.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Globalization;

namespace CC.UI.ViewModels
{
    public partial class CopilotChatViewModel : BaseViewModel
    {
        private const string _defaultLanguage = "en-US";

        // Maui Speech capability fields
        private readonly ISpeechToText _speechToText;

        public ObservableCollection<ChatItem> ChatHistory { get; } = new();

        [ObservableProperty]
        private string recognitionText = "Hello!";

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(ListenCommand))]
        private bool canListenExecute = true;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(StartListenCommand))]
        private bool canStartListenExecute = true;

        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(StopListenCommand))]
        private bool canStopListenExecute = false;


        [ObservableProperty, NotifyCanExecuteChangedFor(nameof(GenerateChatHistoryCommand))]
        private bool canGenerateChatHistoryExecute = true;

        public CopilotChatViewModel()
        {
            _speechToText = SpeechToText.Default;
            _speechToText.StateChanged += _speechToText_StateChanged;
            _speechToText.RecognitionResultCompleted += _speechToText_RecognitionResultCompleted;
        }

        [RelayCommand(IncludeCancelCommand = true, CanExecute = nameof(CanListenExecute))]
        private async Task Listen(CancellationToken cancellationToken)
        {
            CanStartListenExecute = false;

            try
            {
                var isGranted = await _speechToText.RequestPermissions(cancellationToken);
                if (!isGranted)
                {
                    await Toast.Make("Permissions not granted").Show(CancellationToken.None);
                    return;
                }

                const string beginSpeakingPrompt = "Begin speaking...";
                
                RecognitionText = beginSpeakingPrompt;

                ChatHistory.Add(new ChatItem("Copilot", RecognitionText));

                SpeechToTextResult recognitionResult = await _speechToText.ListenAsync(
                    CultureInfo.GetCultureInfo(_defaultLanguage),
                    new Progress<string>(partialText =>
                    {
                        if (RecognitionText is beginSpeakingPrompt)
                        {
                            RecognitionText = string.Empty;
                        }
                        RecognitionText += partialText + " ";
                    }), cancellationToken);

                if (recognitionResult.IsSuccessful)
                {
                    //ChatItem justHeard = new()
                    //{
                    //    Author = "Copilot",
                    //    Text = recognitionResult.Text,
                    //    Timestamp = DateTime.Now
                    //};
                    //ChatHistory.Add(justHeard);

                    RecognitionText = recognitionResult.Text;
                }
                else
                    await Toast.Make(recognitionResult.Exception?.Message ?? "Unable to recognize speech").Show(CancellationToken.None);

                if (RecognitionText is beginSpeakingPrompt)
                    RecognitionText = string.Empty;
            }
            finally
            {
                CanStartListenExecute = true;
            }
        }

        [RelayCommand(IncludeCancelCommand = true, CanExecute = nameof(CanGenerateChatHistoryExecute))]
        private async Task GenerateChatHistory(CancellationToken cancellationToken)
        {
            ChatHistory.Add(new("You", "Your strep test came back positive."));
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            ChatHistory.Add(new("You", "I'd like to prescribe you an antibiotic to fight the infection."));
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            ChatHistory.Add(new("Copilot", "Reminder: This patient is allergic to penicillin.", "List alternatives.", "View allergies."));
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            ChatHistory.Add(new("You", "Since you are allergic to penicillin, I'm going to write you a prescription for Keflex."));
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            ChatHistory.Add(new("Copilot", "Do you want me to send the prescription to the patient's preferred pharmacy?", "Submit medication request"));
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            ChatHistory.Add(new("Patient", "Is Keflex my cheapest option?"));
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            ChatHistory.Add(new("You", "Let me see."));
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            ChatHistory.Add(new("Copilot", "Yes, based on the Patient's insurance, Keflex is their cheapest option.", "View insurance", "View alternative medications"));
        }


        [RelayCommand(CanExecute = nameof(CanStartListenExecute))]
        private async Task StartListen(CancellationToken cancellationToken)
        {
            CanListenExecute = false;
            CanStartListenExecute = false;
            CanStopListenExecute = true;

            var isGranted = await SpeechToText.RequestPermissions(cancellationToken);
            if (!isGranted)
            {
                await Toast.Make("Permission not granted").Show(CancellationToken.None);
                return;
            }

            const string beginSpeakingPrompt = "Begin speaking";
            RecognitionText = beginSpeakingPrompt;
            //ChatItem beginSpeaking = new()
            //{
            //    Author = "Copilot",
            //    Text = RecognitionText,
            //    Timestamp = DateTime.Now
            //};
            //ChatHistory.Add(beginSpeaking);

            await _speechToText.StartListenAsync(CultureInfo.GetCultureInfo(_defaultLanguage), cancellationToken);

            _speechToText.RecognitionResultUpdated += _speechToText_RecognitionResultUpdated;

            if (RecognitionText is beginSpeakingPrompt)
            {
                RecognitionText = string.Empty;
            }
        }

        [RelayCommand(CanExecute = nameof(CanStopListenExecute))]
        private Task StopListen(CancellationToken cancellationToken)
        {
            CanListenExecute = true;
            CanStartListenExecute = true;
            CanStopListenExecute = false;

            _speechToText.RecognitionResultUpdated -= _speechToText_RecognitionResultUpdated;

            return _speechToText.StopListenAsync(cancellationToken);
        }

        private void _speechToText_RecognitionResultUpdated(object sender, SpeechToTextRecognitionResultUpdatedEventArgs e)
        {
            RecognitionText += e.RecognitionResult;
        }

        private void _speechToText_RecognitionResultCompleted(object sender, SpeechToTextRecognitionResultCompletedEventArgs e)
        {
            RecognitionText = e.RecognitionResult;
        }

        private async void _speechToText_StateChanged(object sender, SpeechToTextStateChangedEventArgs e)
        {
            await Toast.Make($"State changed: {e.State}").Show(CancellationToken.None);
        }
    }
}
