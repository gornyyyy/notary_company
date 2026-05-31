using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace notary_company
{
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
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                string connectionString = Configuration.GetConnectionString("Postgres");

                if (string.IsNullOrWhiteSpace(connectionString) || connectionString.Contains("YOUR_PASSWORD_HERE"))
                {
                    MessageBox.Show(
                        "Не настроена строка подключения!\n\n" +
                        "Откройте файл appsettings.json и замените YOUR_PASSWORD_HERE на ваш пароль от PostgreSQL.",
                        "Ошибка конфигурации",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    Shutdown(1);
                    return;
                }

                InitializeDatabase(connectionString);

                IDbConnection connection = new NpgsqlConnection(connectionString);
                Facade = new Facade(connection);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка запуска: {ex.Message} (Возможно неверный пароль)", "Критическая ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown(1);
            }
        }

        private void InitializeDatabase(string mainConnectionString)
        {
            var builder = new NpgsqlConnectionStringBuilder(mainConnectionString);
            string dbName = builder.Database;
            string adminConnectionString = mainConnectionString.Replace($"Database={dbName}", "Database=postgres");

            using var adminConn = new NpgsqlConnection(adminConnectionString);
            adminConn.Open();

            bool dbExists = false;
            using (var cmd = new NpgsqlCommand("SELECT 1 FROM pg_database WHERE datname = @dbname", adminConn))
            {
                cmd.Parameters.AddWithValue("@dbname", dbName);
                dbExists = cmd.ExecuteScalar() != null;
            }

            if (dbExists)
                return;

            using (var cmd = new NpgsqlCommand($"CREATE DATABASE \"{dbName}\" WITH ENCODING='UTF8' TEMPLATE=template0", adminConn))
            {
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show($"База данных '{dbName}' создана.\nВыполняется восстановление из резервной копии...",
                            "Инициализация", MessageBoxButton.OK, MessageBoxImage.Information);

            bool success = RestoreDatabaseFromBackup(dbName, adminConnectionString);

            if (!success)
            {
                MessageBox.Show("База создана, но восстановление из бэкапа не удалось.",
                                "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool RestoreDatabaseFromBackup(string dbName, string mainConnectionString)
        {
            try
            {
                string backupFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "Database", "notary_backup.sql");

                if (!File.Exists(backupFilePath))
                {
                    MessageBox.Show($"Файл бэкапа не найден:\n{backupFilePath}",
                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                string psqlPath = FindPsqlPath();

                if (string.IsNullOrEmpty(psqlPath))
                {
                    MessageBox.Show("Не удалось найти psql.exe.",
                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                //пароль из строки подключения
                var connBuilder = new NpgsqlConnectionStringBuilder(mainConnectionString);

                var processInfo = new ProcessStartInfo
                {
                    FileName = psqlPath,
                    Arguments = $"-h localhost -p 5432 -U postgres -d \"{dbName}\" -f \"{backupFilePath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                processInfo.EnvironmentVariables["PGPASSWORD"] = connBuilder.Password;

                using var process = Process.Start(processInfo);
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode == 0)
                    return true;

                MessageBox.Show($"Ошибка при восстановлении:\n{error}",
                                "Ошибка psql", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при вызове psql:\n{ex.Message}",
                                "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private string FindPsqlPath()
        {
            string pathFromEnv = FindInEnvironmentPath("psql.exe");
            if (!string.IsNullOrEmpty(pathFromEnv))
                return pathFromEnv;

            var standardPaths = new[]
            {
                @"C:\Program Files\PostgreSQL\17\bin\psql.exe",
                @"C:\Program Files\PostgreSQL\16\bin\psql.exe",
                @"C:\Program Files\PostgreSQL\15\bin\psql.exe",
                @"C:\Program Files\PostgreSQL\14\bin\psql.exe",
                @"C:\Program Files\PostgreSQL\13\bin\psql.exe",
                @"C:\Program Files (x86)\PostgreSQL\17\bin\psql.exe",
                @"C:\Program Files (x86)\PostgreSQL\16\bin\psql.exe",
            };

            foreach (var path in standardPaths)
                if (File.Exists(path)) return path;

            return null;
        }

        private string FindInEnvironmentPath(string fileName)
        {
            try
            {
                string pathEnv = Environment.GetEnvironmentVariable("PATH") ?? "";

                foreach (var dir in pathEnv.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries))
                {
                    string fullPath = Path.Combine(dir.Trim(), fileName);
                    if (File.Exists(fullPath))
                        return fullPath;
                }
            }
            catch { }

            return null;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            Facade?.Dispose();
        }
    }
}