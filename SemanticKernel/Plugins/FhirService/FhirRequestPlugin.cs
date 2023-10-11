namespace FhirService;

using Microsoft.SemanticKernel;
using System.ComponentModel;
using Hl7.Fhir.Model;

public class FhirRequestPlugin
{    
    
    [SKFunction, Description("Given a patient identifier, find a patient record.")]
    public async Task<string> GetPatientAsync([Description("The patient identifier.")] string identifier)
    {
        var p = new Patient();
        p.Id = identifier;
        p.Name.Add(new HumanName().WithGiven("John").AndFamily("Doe"));
        return p.ToString();
    }

    [SKFunction, Description("Prescribes the given medication to the given patient.")]
    public async Task<string> CreateMedicationRequestAsync(
        [Description("The patient identifier.")] string patientId,
        [Description("The name of the medication to prescribe.")] string medicationName
        )
    {
        var mr = new MedicationRequest();
        var med = new Medication();
        med.Id = "med0320";
        med.Code = new CodeableConcept("http://snomed.info/sct", "324252006", medicationName);
        mr.Subject = new ResourceReference($"Patient/{patientId}", "John Doe");
        mr.Contained.Add(med);

        return mr.ToString();
    }
        
}
