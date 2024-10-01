using Prism.Commands;
using Prism.Mvvm;
using PubSub;
using System.Windows.Input;
using TerminalLab.PubSubTypes;

namespace TerminalLab.Controls.ViewModels
{
    public partial class ControllerItemUserControlViewModel : BindableBase
    {
        public bool Online
        {
            get => _online;
            set => SetProperty(ref _online, value);
        }

        public string PortName
        {
            get => _portName;
            set => SetProperty(ref _portName, value);
        }
        public string ControllerName
        {
            get => _controllerName;
            set => SetProperty(ref _controllerName, value);
        }


        public ControllerItemUserControlViewModel()
        {
            OpenCommand = new DelegateCommand(OpenCmd);
            CloseCommand = new DelegateCommand(CloseCmd);
            SettingsCommand = new DelegateCommand(SettingsCmd);
        }

        private void SettingsCmd()
        {

        }

        private void CloseCmd()
        {
            Online = false;
            hub.Publish(new ClosePortPubSubObject { Controller = ControllerName, PortName = PortName });
        }

        private void OpenCmd()
        {
            Online = true;
            hub.Publish(new OpenPortPubSubObject { Controller = ControllerName, PortName = PortName});
        }

        public void Send(string value)
        {
            hub.Publish(new SelectControllerSendCommandPubSubObject { Controller = ControllerName, PortName = PortName, Command = value});
        }
    }

    public partial class ControllerItemUserControlViewModel 
    {
        Hub hub = Hub.Default;
        private bool _online;
        string _portName;
        string _controllerName;

        public ICommand OpenCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand SettingsCommand { get; set; }

    }

}
