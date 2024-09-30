using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerminalLab.Controls.ViewModels;

namespace TerminalLab.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public ObservableCollection<ControllerItemUserControlViewModel> Controllers { get; set; }


        public MainWindowViewModel()
        {
            Controllers = new ObservableCollection<ControllerItemUserControlViewModel>()
              {
                  new ControllerItemUserControlViewModel() { ControllerName = "MainController", PortName = "COM1" },
                  new ControllerItemUserControlViewModel() { ControllerName = "AOT", PortName = "COM2" },
                  new ControllerItemUserControlViewModel() { ControllerName = "Sync", PortName = "COM3" },
                  new ControllerItemUserControlViewModel() { ControllerName = "Ref", PortName = "COM4" }
              };
        }
    }
}
