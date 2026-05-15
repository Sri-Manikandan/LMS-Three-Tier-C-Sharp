using Npgsql;

namespace LMSDALLibrary
{
    public static class DBHelper
    {
        private static string ConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=Poornima290178@;Database=library-three-tier";

        private static NpgsqlDataSource? _dataSource;

        public static NpgsqlDataSource DataSource
        {
            get
            {
                if (_dataSource == null)
                {
                    var builder = new NpgsqlDataSourceBuilder(ConnectionString);
                    _dataSource = builder.Build();
                }
                return _dataSource;
            }
        }
    }
}
