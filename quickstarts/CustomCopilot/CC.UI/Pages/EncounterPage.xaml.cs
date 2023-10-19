namespace CC.UI.Pages;

public partial class EncounterPage : ContentPage
{
	public static string Description { get; } = "This page allows user to capture notes and other details about a specific clinical encounter with a patient.";

	public EncounterPage()
	{
		InitializeComponent();
	}

    private void OnSummarizeTranscriptClicked(object sender, EventArgs e)
    {
		//AiServices.SummarizeEncounter();
    }
}