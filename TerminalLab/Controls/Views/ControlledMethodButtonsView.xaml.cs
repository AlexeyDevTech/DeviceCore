using ANG24.Infrastructure.Middleware.Base;
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
using TerminalLab.Controls.ViewModels;
using TerminalLab.Helpers;

namespace TerminalLab.Controls.Views
{
    /// <summary>
    /// Логика взаимодействия для ControlledMethodButtonsView.xaml
    /// </summary>
    public partial class ControlledMethodButtonsView : UserControl
    {
        ControlledMethodButtonsViewModel viewModel;
        public ControlledMethodButtonsView()
        {
            InitializeComponent();
            
        }
       
        private void GenerateMethods()
        {
            this.Panel.Children.Clear();
            viewModel = (ControlledMethodButtonsViewModel)this.DataContext;
            foreach (MethodButton item in viewModel.MethodButtons)
            {
                Border border = new Border();
                border.BorderThickness = new Thickness(1);
                border.BorderBrush = Brushes.Black;


                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Vertical;


                Button button = new();
                button.MinWidth = 100;
                button.Height = 25;
                button.Margin = new Thickness(5);
                button.Padding = new Thickness(5);
                button.Command = item.Command;
                button.Content = item.Name;



                if (item.Params.Count > 0)
                {
                    panel.Children.Add(button);
                    MultiBinding multiBinding = new MultiBinding();
                    for (int i = 0; i < item.Params.Count; i++)
                    {
                        if (item.Params[i].IsEnumParam)
                        {
                            ComboBox comboBox = new();
                            comboBox.Height = 25;
                            comboBox.ItemsSource = item.Params[i].Param as Array;
                            comboBox.Style = this.Resources["ComboBoxUserControl"] as Style;
                            Binding binding = new Binding();
                            binding.Source = item.Params[i];
                            binding.Path = new PropertyPath("Value");

                            comboBox.SetBinding(ComboBox.SelectedItemProperty, binding);

                            multiBinding.Bindings.Add(binding);

                            panel.Children.Add(comboBox);
                        }
                        else
                        {
                            TextBox textBox = new TextBox();
                            textBox.Height = 25;

                            Binding binding = new Binding();
                            binding.Source = item.Params[i];
                            binding.Path = new PropertyPath("Value");

                            textBox.SetBinding(TextBox.TextProperty, binding);
                            textBox.Text = "";
                            multiBinding.Bindings.Add(binding);


                            panel.Children.Add(textBox);
                        }
                    }
                    multiBinding.Converter = new MultiBindConverter();
                    button.SetBinding(Button.CommandParameterProperty, multiBinding);
                }
                else
                {
                    panel.Children.Add(button);
                }


                border.Child = panel;
                this.Panel.Children.Add(border);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GenerateMethods();
        }


    }
}
