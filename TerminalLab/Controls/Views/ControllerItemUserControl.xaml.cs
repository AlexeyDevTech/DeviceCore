using System.Windows;
using System.Windows.Controls;

namespace TerminalLab.Controls.Views
{
    /// <summary>
    /// Логика взаимодействия для ControllerItemUserControl.xaml
    /// </summary>
    public partial class ControllerItemUserControl : UserControl
    {
        public ControllerItemUserControl()
        {
            InitializeComponent();
        }



        public string ControllerName
        {
            get { return (string)GetValue(ControllerNameProperty); }
            set { SetValue(ControllerNameProperty, value); }
        }



        public string COM
        {
            get { return (string)GetValue(COMProperty); }
            set { SetValue(COMProperty, value); }
        }

        // Using a DependencyProperty as the backing store for COM.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty COMProperty =
            DependencyProperty.Register("COM", typeof(string), typeof(ControllerItemUserControl), new PropertyMetadata("COM1"));



        // Using a DependencyProperty as the backing store for ControllerName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ControllerNameProperty =
            DependencyProperty.Register("ControllerName", typeof(string), typeof(ControllerItemUserControl), new PropertyMetadata("MainController"));


    }
}
