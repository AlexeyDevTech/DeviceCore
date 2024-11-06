using ANG24.Core.External;
using ANG24.Infrastructure.Middleware.Base;
using Autofac.Core;
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
        public ObservableCollection<MethodButton> MethodButtons { get; set; } = new ObservableCollection<MethodButton>();
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

        public ControlledMethodButtonsViewModel(ObservableCollection<ControllerItemUserControlViewModel> controllers)
        {
            FillControllers(controllers);

            MethodButtons = new ObservableCollection<MethodButton>();
        }
        private void FillControllers(ObservableCollection<ControllerItemUserControlViewModel> controllers)
        {
            Controllers = new();
            for (var i = 0; i < controllers.Count; i++)
            {
                Controllers.Add(SimpleDeviceFabric.Create(controllers[i].ControllerName));
            }
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


                MethodButton button = new MethodButton();
                button.Name = method.Name;
                button.Command = CreateCommand(method, device);

                if (parameters.Length > 0)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {

                        Parameter param = new Parameter();
                        try
                        {
                            param.Param = parameters[i].ParameterType.GetEnumValues();
                            param.IsEnumParam = true;
                        }
                        catch
                        {
                            param.Param = parameters[i];
                            param.IsEnumParam = false;
                        }
                        button.Params.Add(param);
                    }
                }
                MethodButtons.Add(button);
            }
        }

        private ICommand CreateCommand(MethodInfo method, SimpleDeviceBase device)
        {
            ICommand command = new DelegateCommand<object>(parameter =>
            {
                var parameters = method.GetParameters();
                if (parameters.Length == 0)
                    method.Invoke(device, null);
                else
                {
                    try
                    {
                        object[] invokeParams = new object[parameters.Length];
                        for (int i = 0; i < parameters.Length; i++)
                        {
                            Type type = parameters[i].ParameterType;
                            invokeParams[i] = GetParameterInType(type, (parameter as object[])[i]);
                        }
                        method.Invoke(device, invokeParams);
                    }
                    catch
                    {


                    }
                }
            });

            return command;
        }
        private object GetParameterInType(Type type, object param)
        {
            switch (type.Name)
            {
                case "Int32":
                    return int.Parse(param.ToString());
                case "Double":
                    return double.Parse((param as string).Replace('.', ',').Trim());
                case "String":
                    return param.ToString();
                default:
                    return param;
            }
        }
    }

    public class MethodButton
    {
        public string Name { get; set; }
        public ICommand Command { get; set; }
        public ObservableCollection<Parameter> Params { get; set; } = new ObservableCollection<Parameter>();
    }

    public class Parameter
    {
        public object? Param { get; set; }
        public object? Value { get; set; } = new object();
        public bool IsEnumParam { get; set; }
    }

}
