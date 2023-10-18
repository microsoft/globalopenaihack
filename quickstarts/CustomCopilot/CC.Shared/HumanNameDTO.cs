using System.Text.Json.Serialization;

namespace CC.Shared
{
    public class HumanNameDTO
    {
        [JsonPropertyName("use")]
        public string Use { get; set; } = string.Empty;

        [JsonPropertyName("family")]
        public string Family { get; set; } = string.Empty;

        [JsonPropertyName("given")]
        public List<string> Given { get; set; } = new();
    }
}
