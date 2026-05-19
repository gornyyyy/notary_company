using BCrypt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace notary_company.Pages
{
    public partial class AddHelperWindow : Window
    {
        Facade _facade;
        public AddHelperWindow()
        {
            InitializeComponent();
            _facade = App.Facade;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = HelperNameTextBox.Text;
            string login = HelperLoginTextBox.Text;
            string descr = HelperDescriptionTextBox.Text;

            NameTxtStatus.Text = "";
            LoginTxtStatus.Text = "";

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(login))
            {
                if (string.IsNullOrEmpty(name))
                    NameTxtStatus.Text = "Обязательное поле";
                if (string.IsNullOrEmpty(login))
                    LoginTxtStatus.Text = "Обязательное поле";
            }
            else
            {
                try
                {
                    string password = GeneratePassword();
                    string password_hash = BCrypt.Net.BCrypt.HashPassword(password);
                    _facade.addHelper(name, login, password_hash, descr);

                    MessageBoxResult result = MessageBox.Show(
                        $"Пароль: {password}\n\nСкопировать пароль в буфер обмена?",
                        "Пароль для нового помощника",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        System.Windows.Clipboard.SetText(password);
                        MessageBox.Show("Пароль скопирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    this.DialogResult = true;
                    this.Close();
                }
                catch (ValidationException ex)
                {
                    if (ex.Message.Contains("ФИО"))
                        NameTxtStatus.Text = ex.Message;
                    else if (ex.Message.Contains("Логин"))
                        LoginTxtStatus.Text = ex.Message;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при добавлении помощника: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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