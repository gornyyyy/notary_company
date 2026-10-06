using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Windows;
using notary_company.Services;

namespace notary_company
{
    public partial class App : Application
    {
        public static ApiService Api { get; private set; }
        public static IConfiguration Configuration { get; private set; }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                Configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                string baseUrl = Configuration["ApiSettings:BaseUrl"];

                if (string.IsNullOrWhiteSpace(baseUrl))
                {
                    MessageBox.Show("Не настроен адрес API в appsettings.json (ApiSettings:BaseUrl).",
                                    "Ошибка конфигурации", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Shutdown(1);
                    return;
                }

                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(baseUrl),
                    Timeout = TimeSpan.FromSeconds(30)
                };

                Api = new ApiService(httpClient);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка запуска: {ex.Message}", "Критическая ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown(1);
            }
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            Api?.Dispose();
        }
    }
}