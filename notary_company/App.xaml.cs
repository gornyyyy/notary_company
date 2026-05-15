using System.Configuration;
using System.Data;
using Npgsql;
using System.Windows;

namespace notary_company
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Facade Facade { get; private set; }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            string connectionString = "Server=localhost;Port=5432;Database=notary_company;User Id=postgres;Password=1239Exkrim;";

            IDbConnection connection = new NpgsqlConnection(connectionString);

            Facade = new Facade(connection);
        }

        private void App_Exit(object sender, EventArgs e)
        {
            Facade?.Dispose();  
        }
    }
}
