using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Configuration;
using System.Data;
using System.Text.Json;
using System.Windows;

namespace notary_company
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Facade Facade { get; private set; }
        public static IConfiguration Configuration { get; private set; }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                Configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("secrets.json", optional: false, reloadOnChange: false)
                    .Build();

                string connectionString = Configuration.GetConnectionString("Postgres");

                IDbConnection connection = new NpgsqlConnection(connectionString);

                Facade = new Facade(connection);
            }
            catch (Exception ex) { }
        }

        private void App_Exit(object sender, EventArgs e)
        {
            Facade?.Dispose();  
        }
    }
}
