using CC.Shared;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using System.Diagnostics;
using System.Text.Json;

namespace CC.UI.Services
{
    internal class RestService
    {
        private HttpClient _httpClient = new();

        private JsonSerializerOptions _serializerOptions;

        internal RestService()
        {
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true, 
                WriteIndented = true
            };
        }

        internal async Task<List<PatientDTO>> RetrievePatients()
        {
            List<PatientDTO> patients = new();
            Uri uri = new("https://localhost:7120/patients");
            HttpResponseMessage response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                List<string> strings = JsonSerializer.Deserialize<List<string>>(content);

                foreach (string item in strings)
                {
                    //var options = new JsonSerializerOptions().ForFhir(ModelInfo.ModelInspector);
                    try
                    {
                        //PatientDTO deserializedPatient = JsonSerializer.Deserialize<PatientDTO>(item, options);
                        PatientDTO deserializedPatient = JsonSerializer.Deserialize<PatientDTO>(item);

                        if (deserializedPatient != null)
                        {
                            patients.Add(deserializedPatient);
                        }
                    }
                    catch (DeserializationFailedException e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
            }
            return patients;
        }
    }
}
