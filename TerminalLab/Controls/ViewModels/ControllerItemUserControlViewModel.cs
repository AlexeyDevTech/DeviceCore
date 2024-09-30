using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TerminalLab.Controls.ViewModels
{
    public partial class ControllerItemUserControlViewModel : BindableBase
    {
        

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

        }

        private void OpenCmd()
        {

        }
    }

    public partial class ControllerItemUserControlViewModel 
    { 
        string _portName;
        string _controllerName;

        public ICommand OpenCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand SettingsCommand { get; set; }

    }

}
