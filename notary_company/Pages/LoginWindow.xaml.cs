using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BCrypt.Net;

namespace notary_company.Pages
{
    public partial class LoginWindow : Window
    {
        private Facade _facade;
        public string NotaryName { get; private set; }
        public LoginWindow()
        {
            InitializeComponent();

            _facade = App.Facade;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPassword.Password;

            string passwordhash = _facade.GetNotaryPasswordByLogin(login);

            if (passwordhash != null && BCrypt.Net.BCrypt.Verify(password, passwordhash))
            {
                MessageBox.Show("Вход выполнен успешно!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                NotaryName = _facade.GetNotaryNameByLogin(login);

                this.DialogResult = true;
                this.Close();
            }
            else
            {
                txtStatus.Text = "Неверный логин или пароль!";
                txtPassword.Clear();
            }
        }
    }
}