using notary_company.shared.Dtos;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace notary_company.Pages
{
    public partial class AddHelperWindow : Window
    {
        public AddHelperWindow()
        {
            InitializeComponent();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = HelperNameTextBox.Text;
            string login = HelperLoginTextBox.Text;
            string descr = HelperDescriptionTextBox.Text;

            NameTxtStatus.Text = "";
            LoginTxtStatus.Text = "";

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(login))
            {
                if (string.IsNullOrEmpty(name)) NameTxtStatus.Text = "Обязательное поле";
                if (string.IsNullOrEmpty(login)) LoginTxtStatus.Text = "Обязательное поле";
                return;
            }
            else
            {
                try
                {
                    string password = GeneratePassword();

                    await App.Api.AddHelperAsync(new AddHelperDto
                    {
                        Name = name,
                        Login = login,
                        Password = password,
                        Description = descr
                    });

                    MessageBoxResult result = MessageBox.Show(
                        $"Пароль: {password}\n\nСкопировать пароль в буфер обмена?",
                        "Пароль для нового помощника",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        Clipboard.SetText(password);
                        MessageBox.Show("Пароль скопирован!", "Успех",
                                        MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    this.DialogResult = true;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении помощника: {ex.Message}",
                                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string GeneratePassword()
        {
            Random random = new Random();
            string parametrz = "qwertyuiopasdfghjklzxcvbnm1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
            int lng = parametrz.Length;
            int len = random.Next(10, 15);

            string password = "";

            for (int i = 0; i < len; i++)
            {
                password += parametrz[random.Next(lng)];
            }

            return password;
        }
    }
}