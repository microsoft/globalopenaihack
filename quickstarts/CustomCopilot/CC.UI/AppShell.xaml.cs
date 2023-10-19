using CC.UI.Pages;

namespace CC.UI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(EncounterPage), typeof(EncounterPage));
            Routing.RegisterRoute(nameof(PatientDetailPage), typeof(PatientDetailPage));
            Routing.RegisterRoute(nameof(SchedulingPage), typeof(SchedulingPage));
        }
    }
}