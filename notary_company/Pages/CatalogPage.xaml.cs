using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using notary_company.Models;

namespace notary_company.Pages
{
    public partial class CatalogPage : Page
    {
        private readonly Facade _facade;

        public CatalogPage()
        {
            InitializeComponent();
            _facade = App.Facade;
            LoadServices();
        }

        private void LoadServices()
        {
            List<Service> services = _facade.getAllServices();

            foreach (var service in services)
            {
                Border card = CreateServiceCard(service);
                ServicesWrapPanel.Children.Add(card);
            }
        }

        private Border CreateServiceCard(Service service)
        {
            // Основной прямоугольник (карточка)
            Border card = new Border
            {
                Width = 250,
                Height = 200,
                Margin = new Thickness(15),
                Background = Brushes.White,
                CornerRadius = new CornerRadius(25),
                BorderBrush = new SolidColorBrush(Color.FromRgb(186, 106, 106)),
                BorderThickness = new Thickness(2),
                Cursor = Cursors.Hand,
                Tag = service
            };

            // Анимация увеличения при наведении
            card.MouseEnter += (s, e) => AnimateCard(card, 1.02);
            card.MouseLeave += (s, e) => AnimateCard(card, 1.0);

            // Внутренний контейнер
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Название услуги
            TextBlock serviceName = new TextBlock
            {
                Text = service.Service_name,
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51)),
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(15),
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetRow(serviceName, 0);
            grid.Children.Add(serviceName);

            // Надпись "Подробнее" с подчеркиванием
            TextBlock detailsText = new TextBlock
            {
                Text = "Подробнее",
                FontSize = 14,
                FontWeight = FontWeights.Normal,
                Foreground = new SolidColorBrush(Color.FromRgb(186, 106, 106)),
                TextAlignment = TextAlignment.Center,
                TextDecorations = TextDecorations.Underline,
                Margin = new Thickness(0, 0, 0, 15),
                Cursor = Cursors.Hand
            };

            Grid.SetRow(detailsText, 1);
            grid.Children.Add(detailsText);

            card.Child = grid;

            // Клик по карточке для открытия подробностей
            card.MouseLeftButtonDown += (s, e) => Card_MouseLeftButtonDown(service);

            return card;
        }

        private void AnimateCard(Border card, double scale)
        {
            ScaleTransform transform = new ScaleTransform();
            card.RenderTransform = transform;
            card.RenderTransformOrigin = new Point(0.5, 0.5);

            DoubleAnimation animation = new DoubleAnimation
            {
                To = scale,
                Duration = TimeSpan.FromMilliseconds(200),
                EasingFunction = new QuadraticEase()
            };

            transform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
            transform.BeginAnimation(ScaleTransform.ScaleYProperty, animation);
        }

        private void Card_MouseLeftButtonDown(Service service)
        {
            ShowServiceDetails(service);
        }

        private void ShowServiceDetails(Service service)
        {
            MessageBox.Show($"Услуга: {service.Service_name}\n\nОписание: {service.Service_description}\n\nЦена: {service.Service_price:N2} ₽",
                            "Подробнее об услуге",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

            // Переход на страницу подачи заявки с выбранной услугой
            // var createRequestPage = new CreateRequestPage(service);
            // NavigationService.Navigate(createRequestPage);
        }
    }
}