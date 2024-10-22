using Prism.Commands;
using Prism.Mvvm;
using PubSub;
using System.Windows.Input;
using TerminalLab.PubSubTypes;

namespace TerminalLab.Controls.ViewModels
{
    public class ControllerSettingsViewModel : BindableBase
    {
        Hub hub = Hub.Default;

        private int _baudRate;

        public ICommand ApplyCommand { get; set; }
        public ICommand CancelCommand { get; set; }


        public int BaudRate
        {
            get => _baudRate;
            set => SetProperty(ref _baudRate, value);
        }



        public ControllerSettingsViewModel()
        {
            ApplyCommand = new DelegateCommand(Apply);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Cancel()
        {
            hub.Publish(new SettingsEvent { Apply = false, BaudRate = BaudRate });
        }

        private void Apply()
        {
            hub.Publish(new SettingsEvent { Apply = true, BaudRate = BaudRate });

        }


    }
}
