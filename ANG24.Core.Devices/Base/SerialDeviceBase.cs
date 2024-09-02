﻿using ANG24.Core.Devices.DeviceBehaviors.Interfaces;
using ANG24.Core.Devices.Helpers;
using ANG24.Core.Devices.Interfaces;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.Base
{

    //public delegate void ProcessDataEventHandler

    //public abstract class BlankDeviceBase
    //{

    //    public string DeviceStatus { get; set; } = string.Empty;

    //    public event EventHandler? Connected;
    //    public event EventHandler? Disconnected;
    //    public event DREventHandler DeviceDataReceived;
    //    public Action<object> ProcessData;
    //    protected BlankDeviceBase() { }
    //    public virtual bool Online { get; set; }
    //    public string Name { get; set; }

    //    protected virtual void Connect()
    //    {
    //        Online = true;
    //        Connected?.Invoke(this, new EventArgs());

    //    }
    //    protected void Disconnect()
    //    {
    //        Online = false;
    //        Disconnected?.Invoke(this, new EventArgs());
    //    }
    //    protected abstract void SetCommand(string command); //пока много вопросов по этому методу в данном классе, но оставлю

    //}

    //public abstract class SerialDeviceBase : BlankDeviceBase
    //{
    //    public override bool Online => _port?.IsOpen ?? false;

    //    protected SerialPort _port;
    //    private bool _autoDR;



    //    //для обобщения свойство вынесено в базу
    //    public Action<SerialPort> ReadProcessAction { get; set; }

    //    protected bool AutoDR
    //    {
    //        get => _autoDR;
    //        set
    //        {
    //            if (_autoDR != value)
    //            {
    //                if (value) OnAutoDR();
    //                else OffAutoDR();
    //            }
    //            _autoDR = value;
    //        }
    //    }

    //    protected SerialDeviceBase()
    //    {
    //        _port = new SerialPort();
    //    }

    //    private void OnAutoDR() => _port.DataReceived += port_DR;
    //    private void OffAutoDR() => _port.DataReceived -= port_DR;


    //    //реализация будет зависеть от способа чтения данных
    //    private void port_DR(object sender, SerialDataReceivedEventArgs e)
    //    {
    //        if (Online) DRProcess(); //не допускает чтение неподключенного устройства
    //    }
    //    protected abstract void DRProcess();
    //    /*
    //     * подключение -- примитивное;
    //     * если порт смог открыться -- считай подключились
    //     * только стоит учесть, что есть порты, которые могут открыться, но дальше пусто
    //     * поэтому метод виртуальный, предполагающий расширение функционала
    //     * 
    //     * 
    //     * by Prozorov
    //     */
    //    protected new virtual void Connect()
    //    {
    //        try
    //        {
    //            if (_port != null && !Online)
    //            {
    //                _port.Open();
    //                base.Connect();
    //            }
    //        }
    //        catch { }
    //    }
    //    protected new virtual void Disconnect()
    //    {
    //        try
    //        {
    //            if (_port != null && Online)
    //            {
    //                _port.Close();
    //                base.Disconnect();
    //            }
    //        }
    //        catch { }
    //    }
    //    protected virtual void Reconnect()
    //    {
    //        Connect();
    //    }
    //    protected virtual async Task<bool> Find(string exceptedRequest, string exceptedResponce, int baudRate = 9600)
    //    {
    //        var name = await SerialPortFinder.FindDeviceAsync(exceptedRequest, exceptedResponce, baudRate);
    //        if (name == null) return false;
    //        if (name.Contains("device not found")) return false;
    //        _port.PortName = name;
    //        //_portName = name;
    //        return true;
    //    }

    //    protected override void SetCommand(string command)
    //    {

    //        try
    //        {
    //            if (Online)
    //            {
    //                _port.Write(command);
    //            }
    //        }
    //        catch (InvalidOperationException ioex)
    //        {
    //            //когда порт закрыт (да, такое бывает)
    //            base.Disconnect();
    //        }
    //        catch (TimeoutException tex)
    //        {
    //            //когда время отправки команды истекло
    //            //(или когда входной буфер занят)
    //            Console.WriteLine("Timeout write command");
    //            //ничего не делаем
    //            //(технически, можно считать количество неудачных попыток)
    //        }
    //        catch (Exception ex)
    //        {
    //            //во всех остальных случаях 
    //            //считаем, что устройство сломалось
    //            base.Disconnect();
    //        }
    //    }
    //    protected virtual void SetCommand(byte[] command, int offset = 0, int count = 1)
    //    {

    //        try
    //        {
    //            if (Online)
    //            {
    //                _port.Write(command, 0, count);
    //            }
    //        }
    //        catch (InvalidOperationException ioex)
    //        {
    //            //когда порт закрыт (да, такое бывает)
    //            base.Disconnect();
    //        }
    //        catch (TimeoutException tex)
    //        {
    //            //когда время отправки команды истекло
    //            //(или когда входной буфер занят)
    //            Console.WriteLine("Timeout write command");
    //            //ничего не делаем
    //            //(технически, можно считать количество неудачных попыток)
    //        }
    //        catch (Exception ex)
    //        {
    //            //во всех остальных случаях 
    //            //считаем, что устройство сломалось
    //            base.Disconnect();
    //        }

    //    }
    //    protected virtual void SetCommand(char[] command, int offset = 0, int count = 1)
    //    {
    //        try
    //        {
    //            if (Online)
    //            {
    //                _port.Write(command, 0, count);
    //            }
    //        }
    //        catch (InvalidOperationException ioex)
    //        {
    //            //когда порт закрыт (да, такое бывает)
    //            base.Disconnect();
    //        }
    //        catch (TimeoutException tex)
    //        {
    //            //когда время отправки команды истекло
    //            //(или когда входной буфер занят)
    //            Console.WriteLine("Timeout write command");
    //            //ничего не делаем
    //            //(технически, можно считать количество неудачных попыток)
    //        }
    //        catch (Exception ex)
    //        {
    //            //во всех остальных случаях 
    //            //считаем, что устройство сломалось
    //            base.Disconnect();
    //        }
    //    }
    //    public void SetReadProcess(Action<SerialPort> action) => ReadProcessAction = action;

    //}
    //public class StringSerialDeviceBase : SerialDeviceBase
    //{
    //    public StringSerialReadMode ReadMode { get; set; } = StringSerialReadMode.Line;
    //    public string TerminatorString { get; set; } = "\n";
    //    public int ReadCount { get; set; } = 1;

    //    protected StringSerialDeviceBase()
    //    {

    //    }
    //    protected override void DRProcess()
    //    {
    //        try
    //        {
    //            if (_port.BytesToRead > 0)
    //            {
    //                string dat = string.Empty;
    //                switch (ReadMode)
    //                {
    //                    case StringSerialReadMode.Line:
    //                        dat = _port.ReadLine().Replace('\r', ' ').Replace('\n', ' ').Trim();
    //                        break;
    //                    case StringSerialReadMode.Existing:
    //                        dat = _port.ReadExisting();
    //                        break;
    //                    case StringSerialReadMode.OptionalToString:
    //                        dat = _port.ReadTo(TerminatorString);
    //                        break;
    //                    case StringSerialReadMode.OptionalToBytes:
    //                        var buf = new byte[ReadCount];
    //                        _port.Read(buf, 0, ReadCount);
    //                        dat = Encoding.UTF8.GetString(buf);
    //                        break;
    //                    case StringSerialReadMode.Action:
    //                        ReadProcessAction?.Invoke(_port);
    //                        break;
    //                }
    //                ProcessData?.Invoke(dat);
    //            }
    //        }
    //        catch (InvalidOperationException ioex)
    //        {

    //        }
    //    }
    //    //protected abstract void ProcessData(string data);
    //    public enum StringSerialReadMode : int
    //    {
    //        Line,
    //        Existing,
    //        OptionalToString,
    //        OptionalToBytes,
    //        Action,
    //    }
    //}
    //public class ByteSerialDeviceBase : SerialDeviceBase
    //{
    //    public ByteSerialReadMode ReadMode { get; set; } = ByteSerialReadMode.Signal;
    //    public int ReadCount { get; set; } = 1;
    //    protected ByteSerialDeviceBase() { }
    //    protected override void DRProcess()
    //    {
    //        try
    //        {
    //            switch (ReadMode)
    //            {
    //                case ByteSerialReadMode.Signal:
    //                    ProcessData?.Invoke(new byte[1]);
    //                    break;
    //                case ByteSerialReadMode.Packet:
    //                    var buf = new byte[ReadCount];
    //                    _port.Read(buf, 0, ReadCount);
    //                    ProcessData?.Invoke(buf);
    //                    break;
    //                case ByteSerialReadMode.Action:
    //                    ReadProcessAction?.Invoke(_port);
    //                    ProcessData?.Invoke(new byte[1]);
    //                    break;
    //            }
    //        }
    //        catch (InvalidOperationException ioex) { }
    //    }
    //    public enum ByteSerialReadMode : int
    //    {
    //        Signal,
    //        Packet,
    //        Action
    //    }
    //}

    //public class ManagedDevice
    //{
    //    public BlankDeviceBase Device { get; }
    //    private readonly IDeviceBehavior _behavior;                             //хендл поведения устройства
    //    private readonly ICommandBehavior _commandBehavior;                     //хендл поведения отправки команд устройством


    //    protected ManagedDevice(IDeviceBehavior manageBehavior, ICommandBehavior commandBehavior)
    //    {
    //        Device.ProcessData = Process;
    //        _behavior = manageBehavior;
    //        _commandBehavior = commandBehavior;
    //    }

    //    private void Process(object obj)
    //    {
    //        _behavior.HandleData()
    //    }
    //}

    public interface IPort
    {
        
        void Connect();
        void Disconnect();
    }
    public interface IDeviceReader<T> : IDeviceOperational
    {
        public event Action<T> OnDataReceived;
    }
    public interface IDeviceWriter<T> : IDeviceOperational
    {
        void Write(T message);
    }
    public interface IDeviceOperational
    {
        IPort port { get; }
        void SetPort(IPort port);
    }




    public abstract class BlankDeviceBase
    {
        IPort port;
        IDeviceOperational deviceReader;
        IDeviceOperational deviceWriter;

        public void SetPort(IPort port)
        {
            this.port = port;
            if(deviceWriter != null) deviceWriter.SetPort(port);
            if(deviceReader != null) deviceReader.SetPort(port);
        }
        public void SetReader<T>(IDeviceReader<T> deviceReader)
        {
            this.deviceReader = deviceReader;
            this.deviceReader.SetPort(port);
        }
        public void SetWriter<T>(IDeviceWriter<T> deviceWriter)
        {
            this.deviceWriter = deviceWriter;
            this.deviceWriter.SetPort(port);
        }
    }

    public class StringDeviceReader : IDeviceReader<string>
    {
        public IPort port { get; private set; }

        public event Action<string> OnDataReceived;


        public void SetPort(IPort port)
        {
            this.port = port;
        }
    }






}