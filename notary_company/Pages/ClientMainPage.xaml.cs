using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace notary_company.Pages
{
    public partial class ClientMainPage : Page
    {
        private MainWindow _mainWindow;
        public ClientMainPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            ContentFrame.Navigate(new MainInfoPage());
        }

        private void NotaryLoginText_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_mainWindow.IsNotaryLogged)
                return;

            LoginWindow loginWindow = new LoginWindow(_mainWindow);
            loginWindow.Owner = Window.GetWindow(this);
            bool? result = loginWindow.ShowDialog();
        }

        private void BtnCatalog_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new CatalogPage());
        }

        private void BtnCreateRequest_Click(object sender, RoutedEventArgs e)
        {
            AddRequest addRequest = new AddRequest();
            addRequest.Owner = Window.GetWindow(this);
            bool? result = addRequest.ShowDialog();
        }

        private void BtnMyRequests_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Logo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContentFrame.Navigate(new MainInfoPage());
        }
    }
}