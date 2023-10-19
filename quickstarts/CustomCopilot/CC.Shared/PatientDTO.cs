using System.Text.Json.Serialization;

namespace CC.Shared
{
    public class PatientDTO
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; } = string.Empty;

        [JsonPropertyName("birthDate")]
        public string BirthDate { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public List<HumanNameDTO> Name { get; set; } = new();

        [JsonPropertyName("allergyTolerance")]
        public List<AllergyIntoleranceDTO> AllergyIntolerance { get; set; } = new();
    }
}
