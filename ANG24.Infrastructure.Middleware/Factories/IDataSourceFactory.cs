using ANG24.Core.Interfaces;
using ANG24.Infrastructure.Physical.DataSource;

namespace ANG24.Infrastructure.Middleware.Factories
{
    public abstract class DataSourceFactory
    {
    }
    public class SerialDataSourceFactory : DataSourceFactory
    {
        public IDataSource Create(string com)
        {
            return new SerialDataSource(com);
        }
    }
}
