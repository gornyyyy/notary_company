using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using notary_company.Models;

namespace notary_company.Pages
{
    public partial class MainInfoPage : Page
    {
        private readonly Facade _facade;

        public MainInfoPage()
        {
            InitializeComponent();
            _facade = App.Facade;
            LoadNotaries();
        }

        private void LoadNotaries()
        {
            List<Notary> notaries = _facade.getAllNotary();

            foreach (var notary in notaries)
            {
                Border card = CreateNotaryCard(notary);
                NotariesStackPanel.Children.Add(card);
            }
        }

        private Border CreateNotaryCard(Notary notary)
        {
            Border card = new Border
            {
                Margin = new Thickness(0, 0, 0, 15),
                Background = Brushes.White,
                CornerRadius = new CornerRadius(10),
                BorderBrush = new SolidColorBrush(Color.FromRgb(186, 106, 106)),
                BorderThickness = new Thickness(2),
                Padding = new Thickness(20, 15, 20, 15)
            };

            StackPanel stackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            TextBlock nameText = new TextBlock
            {
                Text = notary.Notary_name,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51)),
                TextAlignment = TextAlignment.Left,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 8)
            };
            stackPanel.Children.Add(nameText);

            TextBlock positionText = new TextBlock
            {
                Text = notary.Is_notary_helper ? "Помощник нотариуса" : "Нотариус",
                FontSize = 14,
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromRgb(186, 106, 106)),
                TextAlignment = TextAlignment.Left,
                Margin = new Thickness(0, 0, 0, 10)
            };
            stackPanel.Children.Add(positionText);

            Border separator = new Border
            {
                Height = 1,
                Background = new SolidColorBrush(Color.FromRgb(224, 224, 224)),
                Margin = new Thickness(0, 5, 0, 10)
            };
            stackPanel.Children.Add(separator);

            TextBlock descriptionText = new TextBlock
            {
                Text = notary.Notary_description ?? "Нет описания",
                FontSize = 15,
                Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102)),
                TextAlignment = TextAlignment.Left,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 5)
            };
            stackPanel.Children.Add(descriptionText);

            TextBlock contactsTitle = new TextBlock
            {
                Text = "Контакты:",
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51)),
                TextAlignment = TextAlignment.Left,
                Margin = new Thickness(0, 10, 0, 5)
            };
            stackPanel.Children.Add(contactsTitle);

            TextBlock phoneText = new TextBlock
            {
                Text = $"Телефон: {(notary.Notary_phone ?? "Не указан")}",
                FontSize = 14,
                Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102)),
                TextAlignment = TextAlignment.Left,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 3)
            };
            stackPanel.Children.Add(phoneText);

            card.Child = stackPanel;

            return card;
        }
    }
}