using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CC.UI.ViewModels
{
    public class AppViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public ICommand SetLocationCommand { private set; get; }

        public string CurrentLocation { get; set; } = "Baxter Building";

        public AppViewModel()
        {
            SetLocationCommand = new Command<string>(
            execute: (string arg) =>
            {
                CurrentLocation = arg;
            },
            canExecute: (string arg) =>
            {
                return true;
            });
        }
    }
}
