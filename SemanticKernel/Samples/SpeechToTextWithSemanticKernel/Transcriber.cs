using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Diagnostics;
using Microsoft.SemanticKernel.Planners;
using Microsoft.SemanticKernel.Planning;

internal class Transcriber : IDisposable
{
    private readonly AudioConfig _audioConfig;
    private readonly SpeechConfig _config;
    private readonly IKernel _kernel;
    private readonly ISequentialPlanner _planner;
    private readonly SpeechRecognizer _recognizer;

    // todo: pass plugins in with kernel but determine planner from summary - is it actions, is it a goal, is it sequential, is there a mix?
    public Transcriber(TranscriberSettings settings, IKernel kernel, ISequentialPlanner planner)
    {
        if (string.IsNullOrEmpty(settings.SubscriptionId)) throw new ArgumentException("SubscriptionId is required. Check your appsettings.json or user secrets.", nameof(settings.SubscriptionId));
        if (string.IsNullOrEmpty(settings.Location)) throw new ArgumentException("Location is required. Check your appsettings.json.", nameof(settings.Location));

        _config = SpeechConfig.FromSubscription(settings.SubscriptionId, settings.Location);

        _audioConfig = string.IsNullOrEmpty(settings.MicrophoneDeviceId) ? AudioConfig.FromDefaultMicrophoneInput() : AudioConfig.FromMicrophoneInput(settings.MicrophoneDeviceId);
        _recognizer = new SpeechRecognizer(_config, _audioConfig);
        _recognizer.Recognized += RecognizedAudioHandler;
        // Subscribes to events.
        //recognizer.Recognizing += (s, e) => Console.WriteLine($"RECOGNIZING: Text={e.Result.Text}");
        //recognizer.Canceled += (s, e) => Console.WriteLine($"Canceled. Reason: {e.Reason}, CanceledReason: {e.Reason}");
        //recognizer.SessionStarted += (s, e) => Console.WriteLine("Session started event.");
        //recognizer.SessionStopped += (s, e) => Console.WriteLine("Session stopped event.");

        // TODO: setup kernel

        _kernel = kernel;
        _planner = planner;
    }

    private void RecognizedAudioHandler(object? sender, SpeechRecognitionEventArgs e)
    {
        var result = e.Result;

        if (result.Reason == ResultReason.RecognizedSpeech)
            RecognizedText.Add(result.Text);


    }


    // as timer ticks, summarize text using sk
    internal async Task StartAsync()
    {
        // start timer


        // Starts continuous recognition. 
        // Uses StopContinuousRecognitionAsync() to stop recognition.
        await _recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
    }

    // considerations... maybe move away from StartContinuousRecognitionAsync and just do a one off with verification from user?
    // in a scenario where we're just listening in on a patient chat with doc, we need to listen, accumulate text, chunk it, run it through open ai for planning
    // probably need to perform that work inside transcribeasync to play nice with that loop (or pull that out) with another class that handles chunking and SK interaction


    // loop: chunk => summarize (persist + append) => send to planner for actions (keep track of existing action items) => 
    // --note: planner comes back with plan.. then you have another statement to execute; can persist plans
    // phrase exit => present summary of actions 

    // consider: changes to action items during discussion e.g. "I'm going to prescribe x medication" => "Actually I'm going to prescribe y amount of x"


    // merge branches
    // maui leverage whisper
    // how to show the chat... in maui



    // kill the timer, get the plan from the planner and present
    internal async Task<Plan> StopAsync()
    {
        await _recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);

        var prompt = string.Join(' ', RecognizedText.ToArray()) ?? "";

        Plan plan;
        try
        {
            plan = await _planner.CreatePlanAsync(prompt);
        }
        catch (SKException ex)
        {
            Console.WriteLine(ex.Message);

            return null;
        }

        Console.WriteLine("Writing Plan to Console");
        Console.WriteLine("------------------------");
        for (var i = 0; i < plan.Steps.Count; i++)
            Console.WriteLine($"{i + 1}. {plan.Steps[i].PluginName} {plan.Steps[i].Description}");
        Console.WriteLine("------------------------");

        return plan;
    }

    internal List<string> RecognizedText { get; private set; } = new List<string>();

    void IDisposable.Dispose()
    {
        _recognizer.Dispose();
        _audioConfig.Dispose();
    }
}
