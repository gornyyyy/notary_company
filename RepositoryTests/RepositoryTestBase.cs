using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Data;
using System.IO;

namespace RepositoryTests
{
    [TestClass]
    public abstract class RepositoryTestBase
    {
        protected IDbConnection Connection { get; private set; }
        protected static string ConnectionString { get; private set; }

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("secrets.json", optional: false, reloadOnChange: false)
                .Build();

            ConnectionString = configuration.GetConnectionString("Postgres");
        }


        [TestInitialize]
        public void TestInitialize()
        {
            Connection = new NpgsqlConnection(ConnectionString);
            Connection.Open();
            ClearAllTables();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Connection?.Close();
            Connection?.Dispose();
        }

        protected void ClearAllTables()
        {
            Connection.Execute("TRUNCATE TABLE request_services CASCADE");
            Connection.Execute("TRUNCATE TABLE requests CASCADE");
            Connection.Execute("TRUNCATE TABLE notaries CASCADE");
            Connection.Execute("TRUNCATE TABLE users CASCADE");
            Connection.Execute("TRUNCATE TABLE services CASCADE");
            Connection.Execute("TRUNCATE TABLE clients CASCADE");
        }
    }
}