using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

internal class Transcriber
{
    private readonly SpeechConfig _config;
    private readonly string? _microphoneDeviceId;

    public Transcriber(string? subscriptionId, string? location, string? microphoneDeviceId = null)
    {
        if(string.IsNullOrEmpty(subscriptionId)) throw new ArgumentException("SubscriptionId is required. Check your appsettings.json or user secrets.", nameof(subscriptionId));
        if(string.IsNullOrEmpty(location)) throw new ArgumentException("Location is required.  Check your appsettings.json.", nameof(location));

        _config = SpeechConfig.FromSubscription(subscriptionId, location);

        _microphoneDeviceId = microphoneDeviceId;
    }

    internal List<string> RecognizedText { get; private set; } = new List<string>();

    internal async Task TranscribeAsync()
    {
        RecognizedText.Clear();

        using var audioConfig = string.IsNullOrEmpty(_microphoneDeviceId) ? AudioConfig.FromDefaultMicrophoneInput() : AudioConfig.FromMicrophoneInput(_microphoneDeviceId);
        using var recognizer = new SpeechRecognizer(_config, audioConfig);

        // Subscribes to events.
        //recognizer.Recognizing += (s, e) => Console.WriteLine($"RECOGNIZING: Text={e.Result.Text}");
        //recognizer.Canceled += (s, e) => Console.WriteLine($"Canceled. Reason: {e.Reason}, CanceledReason: {e.Reason}");
        //recognizer.SessionStarted += (s, e) => Console.WriteLine("Session started event.");
        //recognizer.SessionStopped += (s, e) => Console.WriteLine("Session stopped event.");

        recognizer.Recognized += (s, e) =>
        {
            var result = e.Result;
            //Console.WriteLine($"Reason: {result.Reason}");
            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                RecognizedText.Add(result.Text);
                //Console.WriteLine($"Final result: Text: {result.Text}.");
            }
        };

        // Starts continuous recognition. 
        // Uses StopContinuousRecognitionAsync() to stop recognition.
        await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

        do
        {
            Console.WriteLine("Press Enter to stop transcribing.");
        } while (Console.ReadKey().Key != ConsoleKey.Enter);

        // Stops recognition.
        await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
    }
}
