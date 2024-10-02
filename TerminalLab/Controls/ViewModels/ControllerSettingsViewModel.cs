using Prism.Mvvm;

namespace TerminalLab.Controls.ViewModels
{
    public class ControllerSettingsViewModel : BindableBase
    {
        private int _baudRate;

        public int BaudRate
        {
            get => _baudRate;
            set => SetProperty(ref _baudRate, value);
        }
        public ControllerSettingsViewModel()
        {
        }
    }
}
