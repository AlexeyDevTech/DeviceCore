using Prism.Mvvm;
using System.Collections.ObjectModel;
using TerminalLab.Controls.ViewModels;
using PubSub;
using System.Diagnostics;
using TerminalLab.PubSubTypes;
using Prism.Commands;

namespace TerminalLab.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        Hub hub = Hub.Default;
        private string _rawText;

        int _maxStringLenght = 1024;
        private ControllerItemUserControlViewModel _selController;
        private string _inputText;

        public string RawText 
        {
            get => _rawText;
            set
            {
                var NS = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    var l = value;
                    var localFaultOperationCounter = 0;
                    if (l.Length > _maxStringLenght)
                    {
                        while (l.Length > _maxStringLenght && localFaultOperationCounter < 5)
                        {
                            var _indexNL = value.IndexOf('\n');
                            _indexNL++;
                            NS = value.Substring(_indexNL, (value.Length - _indexNL));
                            l = NS;
                            localFaultOperationCounter++;
                        }
                    } else NS = value;
                }
                else NS = value;
                SetProperty(ref _rawText, NS);
            }
        
        }

        public ObservableCollection<ControllerItemUserControlViewModel> Controllers { get; set; }
        public ControllerItemUserControlViewModel SelController
        {
            get => _selController;
            set => SetProperty(ref _selController, value);
        }

        public string InputText
        {
            get => _inputText;
            set => SetProperty(ref _inputText, value);
        }
        public DelegateCommand EnterKeyCommand { get; set; }

        public MainWindowViewModel()
        {
            EnterKeyCommand = new DelegateCommand(() =>
            {
                if (SelController == null)
                {
                    RawText += $"!<<ERROR>[[Select controller first!!!]]\n";
                    return;
                }
                if (string.IsNullOrEmpty(InputText))
                {
                    RawText += $"!<<ERROR>[[Command empty!!!]]\n";
                    return;
                }
                SelController.Send(InputText);
            });


            Controllers = new ObservableCollection<ControllerItemUserControlViewModel>()
              {
                  new ControllerItemUserControlViewModel() { ControllerName = "MainController", PortName = "COM1" },
                  new ControllerItemUserControlViewModel() { ControllerName = "AOT", PortName = "COM2" },
                  new ControllerItemUserControlViewModel() { ControllerName = "Sync", PortName = "COM3" },
                  new ControllerItemUserControlViewModel() { ControllerName = "Ref", PortName = "COM4" },
                  

              };
            hub.Subscribe<OpenPortPubSubObject>(x =>
            {
                //something...
                RawText += $"[[port {x.Controller} is open]]\n";
            });
            hub.Subscribe<ClosePortPubSubObject>(x =>
            {
                RawText += $"[[port {x.Controller} is close]]\n";
            });
            hub.Subscribe<SelectControllerSendCommandPubSubObject>(x =>
            {
                RawText += $"{x.Controller} <- {x.Command}\n";
            });
        }
    }
}
