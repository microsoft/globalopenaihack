using CC.Shared;
using CC.UI.Pages;
using CC.UI.Services;
using System.Collections.ObjectModel;

namespace CC.UI.Controls;

public partial class PatientList : ContentView
{
    public event EventHandler PatientSelected;

    public ObservableCollection<PatientDTO> Patients { get; } = new();

    public PatientList()
	{
		InitializeComponent();

        BindingContext = this;
    }

    private async void OnGetPatientsClicked(object sender, EventArgs e)
    {
        Patients.Clear();

        RestService service = new();
        List<PatientDTO> patients = await service.RetrievePatients();
        foreach (PatientDTO patient in patients)
        {
            Patients.Add(patient);
        }
    }

    private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        PatientSelected?.Invoke(this, e);
    }

    private async void OnNewEncounterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(EncounterPage));
    }
}