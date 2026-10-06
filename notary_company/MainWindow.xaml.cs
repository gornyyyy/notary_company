using notary_company.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace notary_company
{
    public partial class MainWindow : Window
    {
        public string NotaryName { get; set; }
        public bool IsNotaryLogged { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Navigate(new ClientMainPage(this));
        }

        public void SwitchToNotaryPage(string notaryName)
        {
            MainFrame.Navigate(new NotaryMainPage(this, notaryName));
        }

    }
}