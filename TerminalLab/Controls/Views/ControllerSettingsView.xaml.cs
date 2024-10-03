using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
