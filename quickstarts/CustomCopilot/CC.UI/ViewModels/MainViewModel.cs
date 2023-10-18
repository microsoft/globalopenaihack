using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Firely = Hl7.Fhir.Model;

namespace CC.UI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public ICommand SetLocationCommand { private set; get; }

        public string CurrentLocation { get; set; } = "Baxter Building";

        public ObservableCollection<Firely.Patient> Patients { get; } = new();

        public MainViewModel()
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

//class ClockViewModel : INotifyPropertyChanged
//{
//    public event PropertyChangedEventHandler PropertyChanged;

//    private DateTime _dateTime;
//    private Timer _timer;

//    public DateTime DateTime
//    {
//        get => _dateTime;
//        set
//        {
//            if (_dateTime != value)
//            {
//                _dateTime = value;
//                OnPropertyChanged(); // reports this property
//            }
//        }
//    }

//    public ClockViewModel()
//    {
//        this.DateTime = DateTime.Now;

//        // Update the DateTime property every second.
//        _timer = new Timer(new TimerCallback((s) => this.DateTime = DateTime.Now),
//                           null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
//    }

//    ~ClockViewModel() =>
//        _timer.Dispose();

//    public void OnPropertyChanged([CallerMemberName] string name = "") =>
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
//}