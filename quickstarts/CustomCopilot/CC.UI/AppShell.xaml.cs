using CC.UI.Controls;
using CC.UI.Pages;
using CC.UI.Services;
using Microsoft.SemanticKernel.Planning;

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

        private async void CopilotChat_SubmitChat(object sender, NewChatEventArgs e)
        {
            Plan plan = await AiServices.CreateActionPlan(e.ChatItem);

            // based on plan, provide parameters to the action
        }
    }
}