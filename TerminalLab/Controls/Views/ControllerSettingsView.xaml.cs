using System.Windows;

namespace TerminalLab.Controls.Views
{
    /// <summary>
    /// Логика взаимодействия для ControllerSettingsView.xaml
    /// </summary>
    public partial class ControllerSettingsView : Window
    {
        public ControllerSettingsView()
        {
            InitializeComponent();

        }

        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
