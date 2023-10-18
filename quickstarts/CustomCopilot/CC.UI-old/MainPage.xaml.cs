namespace CC.UI
{
    public partial class MainPage : ContentPage
    {
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