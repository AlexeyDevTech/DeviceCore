using ANG24.Core.Devices.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.Base.DataAdapters
{
    public abstract class MockAdapter<T> : IDataSourceAdapter<T>
    {
        public abstract T Read();

        public abstract void Write(T data);
    }

    public class StringMockAdapter : MockAdapter<string>, IDisposable
    {
        StreamWriter StringWriter;
        FileStream fs;
        public StringMockAdapter()
        {
            fs = new FileStream("mock_out.txt", FileMode.Append);
            StringWriter = new StreamWriter(fs);
            StringWriter.AutoFlush = true;
        }

        ~StringMockAdapter()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            StringWriter.Close();
            StringWriter.Dispose();
            fs.Dispose();
        }

        public override string Read()
        {
            return "string response";
        }

        public override void Write(string data)
        {
            StringWriter.WriteLine(data);
        }
    }
}
