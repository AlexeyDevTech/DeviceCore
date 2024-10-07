using ANG24.Core.Devices.Base.DataSources;
using ANG24.Core.Devices.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.Base.Abstract
{
    public abstract class DeviceBase
    {
        protected DataSourceBase source;

        public DeviceBase()
        {

        }

        private void Source_OnData(object obj)
        {
            Console.WriteLine($"device received data: {obj}");
            OnData(obj);
        }
        private void DeviceBase_OnDisconnect()
        {
            Console.WriteLine("device offline");
        }
        private void DeviceBase_OnConnect()
        {
            Console.WriteLine("device online");
        }

        public void SetDataSource(IDataSource source)
        {
            if (this.source != null)
            {
                this.source.OnData -= Source_OnData;
                ((IConnectable)this.source).OnConnect -= DeviceBase_OnConnect;
                ((IConnectable)this.source).OnDisconnect -= DeviceBase_OnDisconnect;
            }

            this.source = (DataSourceBase)source;
            this.source.OnData += Source_OnData;
            ((IConnectable)this.source).OnConnect += DeviceBase_OnConnect;
            ((IConnectable)this.source).OnDisconnect += DeviceBase_OnDisconnect;
        }

        public void Connect()
        {
            ((IConnectable)source)?.Connect();
        }
        public void Disconnect()
        {
            ((IConnectable)source)?.Disconnect();
        }
        public bool Online => source.Online;

        public void Write<T>(T msg) => source.Write(msg);
        public void Write(Type type, object msg) => source.Write(type, msg);
        protected abstract void OnData(object data);
    }
}
