public class TranscriberSettings
{
    public TranscriberSettings() { }
    public TranscriberSettings(string? subscriptionId, string? location, string? microphoneDeviceId)
    {
        SubscriptionId = subscriptionId;
        Location = location;
        MicrophoneDeviceId = microphoneDeviceId;
    }
    public string? SubscriptionId { get; set; } = null;
    public string? Location { get; set; } = null;
    public string? MicrophoneDeviceId { get; set; } = null;
}
