using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace notary_company.Pages
{
    /// <summary>
    /// Логика взаимодействия для NotaryMainPage.xaml
    /// </summary>
    public partial class NotaryMainPage : Page
    {
        MainWindow _mainWindow;
        public NotaryMainPage(MainWindow mainWindow, string notaryName)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            NotaryNameText.Text = notaryName;
            ContentFrame.Navigate(new MainInfoPage());
        }

        private void BtnCatalog_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new CatalogPage());
        }

        private void BtnViewRequests_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new GetRequestsPage());
        }

        private void BtnAddAssistant_Click(object sender, RoutedEventArgs e)
        {
            AddHelperWindow addHelperWindow = new AddHelperWindow();
            addHelperWindow.Owner = Window.GetWindow(this);
            bool? result = addHelperWindow.ShowDialog();
        }

        private void Logo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContentFrame.Navigate(new MainInfoPage());
        }
    }
}
