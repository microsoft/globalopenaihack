using CC.UI.Controls;
using CC.UI.Models;
using CC.UI.Pages;
using CC.UI.Services;
using CC.UI.ViewModels;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.Planning;
using System.ComponentModel.DataAnnotations;

namespace CC.UI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(EncounterPage), typeof(EncounterPage));
            Routing.RegisterRoute(nameof(PatientDetailPage), typeof(PatientDetailPage));
            Routing.RegisterRoute(nameof(SchedulingPage), typeof(SchedulingPage));
        }

        private async void CopilotChat_SubmitChat(object sender, NewChatEventArgs e)
        {
            Plan plan = await AiServices.CreateActionPlan(e.ChatItem);

            // based on plan, provide parameters to the action
            if (plan.Steps != null && plan.Steps.Count > 0)
            {
                SKContext context = AiServices._kernel.CreateNewContext();
                switch (plan.Steps[0].Name)
                {
                    case "NavigateApplicationPages":
                        // no additional parameters needed
                        FunctionResult navigateResult = await plan.InvokeAsync(context);
                        await Shell.Current.GoToAsync(navigateResult.GetValue<string>());
                        break;
                    case "SummarizeEncounter":
                        // no additional parameters needed
                        FunctionResult summarizeResult = await plan.InvokeAsync(context);
                        CopilotChatViewModel vm = copilotChat.BindingContext as CopilotChatViewModel;
                        vm.ChatHistory.Add(new ChatItem() { 
                            Text = summarizeResult.GetValue<string>(), 
                            Author = "Copilot", Timestamp = DateTime.Now 
                        });
                        break;
                    default:
                        break;
                };
            }
        }
    }
}