using System.Text.Json.Serialization;

namespace CC.Shared
{
    public class CodingDTO
    {
        [JsonPropertyName("system")]
        public string System { get; set; } = string.Empty;

        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("display")]
        public string Display { get; set; } = string.Empty;
    }
}
