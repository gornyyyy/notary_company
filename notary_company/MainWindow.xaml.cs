using notary_company.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace notary_company
{
    public partial class MainWindow : Window
    {
        private readonly Facade _facade;

        public MainWindow()
        {
            InitializeComponent();
            _facade = App.Facade;

        }


        private void NotaryLoginText_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsNotaryLoggedIn())
                return;

            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Owner = Window.GetWindow(this);
            bool? result = loginWindow.ShowDialog();

            if (result == true)
            {
                string notaryName = loginWindow.NotaryName;

                NotaryLoginText.Text = notaryName;
            }
        }
        private bool IsNotaryLoggedIn()
        {
            return NotaryLoginText.Text != "Войти как нотариус";
        }

    }
}