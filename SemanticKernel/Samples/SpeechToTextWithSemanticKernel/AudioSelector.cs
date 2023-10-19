using NAudio.CoreAudioApi;

internal static class AudioSelector
{
    private readonly static List<Option> options = new();

    internal static string SelectDevice()
    {
        var deviceEnum = new MMDeviceEnumerator();

        MMDevice? selectedDevice = null;

        foreach (var device in deviceEnum.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active))
        {
            options.Add(new Option(device.FriendlyName, () => selectedDevice = device));
        }

        options.Add(new Option("Exit", () => Environment.Exit(0)));

        int index = 0;

        WriteMenu(options, options[index]);

        ConsoleKeyInfo keyinfo;
        do
        {
            keyinfo = Console.ReadKey();

            switch (keyinfo.Key)
            {
                case ConsoleKey.DownArrow:
                    if (index + 1 < options.Count)
                    {
                        index++;
                        WriteMenu(options, options[index]);
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (index - 1 >= 0)
                    {
                        index--;
                        WriteMenu(options, options[index]);
                    }
                    break;
                case ConsoleKey.Enter:
                    options[index].Selected.Invoke();
                    index = 0;
                    if (selectedDevice != null)
                    {
                        Console.Clear();
                        Console.WriteLine($"Using {selectedDevice.DeviceFriendlyName}");
                        return selectedDevice.ID;
                    }
                    break;
                default:
                    break;
            }
        }
        while (keyinfo.Key != ConsoleKey.X);

        return string.Empty;
    }
    private static void WriteMenu(List<Option> options, Option selectedOption)
    {
        Console.Clear();

        foreach (Option option in options)
        {
            if (option == selectedOption)
            {
                Console.Write("> ");
            }
            else
            {
                Console.Write(" ");
            }

            Console.WriteLine(option.Name);
        }
    }

    internal class Option
    {
        internal string Name { get; }
        internal Action Selected { get; }

        internal Option(string name, Action selected)
        {
            Name = name;
            Selected = selected;
        }
    }
}
