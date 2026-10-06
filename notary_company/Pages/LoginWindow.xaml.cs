using System.Text;
using System.Windows;
using System.ComponentModel.DataAnnotations;
using notary_company.shared.Dtos;

namespace notary_company.Pages
{
    public partial class LoginWindow : Window
    {
        private MainWindow _mainWindow;
        public LoginWindow(MainWindow mainWindow)
        {
            InitializeComponent();

            _mainWindow = mainWindow;
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = txtLogin.Text;
                string password = txtPassword.Password;

                var notary = await App.Api.LoginAsync(login, password);

                if (notary != null)
                {
                    _mainWindow.NotaryName = notary.Notary_name;
                    _mainWindow.SwitchToNotaryPage(notary.Notary_name);

                    DialogResult = true;
                    Close();
                }
                else
                {
                    txtStatus.Text = "Неверный логин или пароль!";
                    txtPassword.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}