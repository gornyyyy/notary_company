using System.Windows;
using System.Windows.Controls;
using notary_company.Pages;

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



    }
}