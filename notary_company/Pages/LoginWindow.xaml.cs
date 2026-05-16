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
using notary_company.Models;

namespace notary_company.Pages
{
    public partial class LoginWindow : Window
    {
        private MainWindow _mainWindow;
        private Facade _facade;
        public LoginWindow(MainWindow mainWindow)
        {
            InitializeComponent();

            _mainWindow = mainWindow;

            _facade = _mainWindow.facade;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPassword.Password;

            string passwordhash = _facade.GetNotaryPasswordByLogin(login);

            if (passwordhash != null && BCrypt.Net.BCrypt.Verify(password, passwordhash))
            {
                
                _mainWindow.Notary = _facade.GetNotaryByLogin(login);
                _mainWindow.SwitchToNotaryPage(_mainWindow.Notary.Notary_name);

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