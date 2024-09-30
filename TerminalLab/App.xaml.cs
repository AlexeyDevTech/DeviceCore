using ANG24.Core.Devices.Creators;
using ANG24.Core.Devices.Interfaces;
using Autofac;
using System.Configuration;
using System.Data;
using System.Windows;

namespace TerminalLab
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 
    
    public partial class App : Application
    {
        private static IContainer Container { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var builder = new ContainerBuilder();
            builder.RegisterType<DeviceCreator>().As<IDeviceCreator>().SingleInstance();
        }
    }

}
