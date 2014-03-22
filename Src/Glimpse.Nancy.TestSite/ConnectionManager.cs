using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using Nancy;

namespace Glimpse.Nancy.TestSite
{
    public interface IConnectionManager
    {
        IDbConnection GetConnection();
    }

    public class ConnectionManager : IConnectionManager
    {
        private readonly DbProviderFactory factory;
        private readonly string databaseName;

        public ConnectionManager(IRootPathProvider rootPathProvider)
        {
            this.factory = DbProviderFactories.GetFactory("System.Data.SQLite");
            this.databaseName = Path.Combine(rootPathProvider.GetRootPath(), "data.db");

            CreateDatabase();
        }

        private void CreateDatabase()
        {
            if (!File.Exists(this.databaseName))
            {
                SQLiteConnection.CreateFile(this.databaseName);
            }
        }

        public IDbConnection GetConnection()
        {
            var conn = this.factory.CreateConnection();
            conn.ConnectionString = @"Data Source=" + this.databaseName;
            conn.Open();
            return conn;
        }
    }
}