using notary_company.Models;
using notary_company.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace notary_company
{
    public partial class MainWindow : Window
    {
        public Facade facade;
        public Notary Notary;
        public bool IsNotaryLogged = false;

        public MainWindow()
        {
            InitializeComponent();
            facade = App.Facade;

            MainFrame.Navigate(new ClientMainPage(this));
        }

        public void SwitchToNotaryPage(string notaryName)
        {
            MainFrame.Navigate(new NotaryMainPage(this, notaryName));
        }

    }
}