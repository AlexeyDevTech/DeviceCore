using ANG24.Core.External;
using ANG24.Infrastructure.Middleware.Base;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TerminalLab.Controls.ViewModels
{
    public class ControlledMethodButtonsViewModel : BindableBase
    {
        public ObservableCollection<MethodButton> MethodButtons { get; set; }


        private SimpleDeviceBase _device;
        public SimpleDeviceBase Device
        {
            get => _device;
            set
            {
                FindPublicMethods(value);
                SetProperty(ref _device, value);
            }
        }
        public ObservableCollection<SimpleDeviceBase> Controllers { get; set; }

        public ControlledMethodButtonsViewModel()
        {
            Controllers = new ObservableCollection<SimpleDeviceBase>()
              {
                 SimpleDeviceFabric.Create(ANG24.Core.External.Types.ControllerNames.Main),
                 SimpleDeviceFabric.Create(ANG24.Core.External.Types.ControllerNames.Compensation),
                 SimpleDeviceFabric.Create(ANG24.Core.External.Types.ControllerNames.MNK)
              };

            MethodButtons = new ObservableCollection<MethodButton>();
        }
        private void FindPublicMethods(SimpleDeviceBase device)
        {
            Type targetType = device.GetType();
            MethodInfo[] methods = targetType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                             .Where(e => e.DeclaringType == targetType &&
                                                         e.IsSpecialName == false).ToArray();
            MethodButtons.Clear();
            foreach (MethodInfo method in methods)
            {
                var parameters = method.GetParameters();
                // Создаем команду для каждого метода
                ICommand command = new DelegateCommand<object>(parameter =>
                {
                    var parameters = method.GetParameters();

                    if (parameters.Length == 0)
                        method.Invoke(device, null);
                    else
                    {
                        if (parameter == null)
                        {
                            return;
                        }
                        try
                        {
                            method.Invoke(device, new object[] { parameter });
                        }
                        catch
                        {
                            double par = 0;
                            try
                            {
                                par = Double.Parse((parameter as string).Replace('.', ',').Trim());
                                method.Invoke(device, new object[] { par });
                            }
                            catch
                            {
                                try
                                {

                                    method.Invoke(device, new object[] { (int)par });
                                }
                                catch
                                {

                                }
                            }
                        }



                    }
                });

                MethodButton button = new MethodButton();
                button.Name = method.Name;
                button.Command = command;

                if (parameters.Length > 0)
                {
                    try
                    {
                        button.Values = parameters[0].ParameterType.GetEnumValues();
                        button.isEnumParam = true;
                    }
                    catch
                    {
                        button.Param = parameters[0];
                        button.isStringParam = true;
                    }
                }
                MethodButtons.Add(button);
            }
        }
    }

    public class MethodButton
    {
        public string Name { get; set; }
        public ICommand Command { get; set; }

        public bool isStringParam { get; set; }
        public bool isEnumParam { get; set; }
        public object? Values { get; set; }
        public object? Param { get; set; }
    }

}
