namespace CC.UI
{
    public partial class MainPage : ContentPage
    {
        public static string Description { get; } = "This page allows user view a list of patients.";

        public MainPage()
        {
            InitializeComponent();
        }

        private void PatientList_PatientSelected(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("PatientDetailPage");
        }
    }
}