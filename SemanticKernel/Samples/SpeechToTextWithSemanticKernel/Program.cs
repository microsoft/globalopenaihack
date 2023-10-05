using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .Build();

var deviceId = AudioSelector.SelectDevice();

var transcriber = new Transcriber(config["SubscriptionId"], config["Location"], deviceId);

// interaction look is say something and hit enter, then wait until the process completes, then say something again

Console.WriteLine("You're now chatting with your copilot. Press escape at anytime to exit the program...");
ConsoleKeyInfo keyinfo;
var receivingSpeech = true;
// considerations... maybe move away from StartContinuousRecognitionAsync and just do a one off with verification from user?
// in a scenario where we're just listening in on a patient chat with doc, we need to listen, accumulate text, chunk it, run it through open ai for planning
// probably need to perform that work inside transcribeasync to play nice with that loop (or pull that out) with another class that handles chunking and SK interaction
do
{
    keyinfo = Console.ReadKey();

    if (receivingSpeech)
    {
        await transcriber.TranscribeAsync();

        Console.WriteLine("I heard the following from you:");
        foreach (var text in transcriber.RecognizedText)
        {
            Console.WriteLine(text);
        }

        receivingSpeech = false;
    }
    else
    {
        // execute on received text...?
    }
}
while (keyinfo.Key != ConsoleKey.Escape);


Console.WriteLine("Press any key to exit...");

Console.ReadKey();