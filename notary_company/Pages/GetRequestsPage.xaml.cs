using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using notary_company.Models;

namespace notary_company.Pages
{
    public partial class GetRequestsPage : Page
    {
        private readonly Facade _facade;
        private List<Request> _allRequests;
        private Dictionary<int, List<Service>> _requestServices;
        private Dictionary<string, Client> _clients;

        public GetRequestsPage()
        {
            InitializeComponent();
            _facade = App.Facade;
            LoadRequests();
        }

        private void LoadRequests()
        {
            _allRequests = _facade.getAllRequests();
            _requestServices = new Dictionary<int, List<Service>>();
            _clients = new Dictionary<string, Client>();

            foreach (var request in _allRequests)
            {
                var services = _facade.getServicesForRequest(request.Request_id);
                _requestServices[request.Request_id] = services;
            }

            DisplayRequests(_allRequests);
        }

        private void DisplayRequests(List<Request> requests)
        {
            RequestsStackPanel.Children.Clear();

            foreach (var request in requests)
            {
                var card = CreateRequestCard(request);
                RequestsStackPanel.Children.Add(card);
            }
        }

        private Border CreateRequestCard(Request request)
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

            Grid mainGrid = new Grid();
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Верхняя строка с ID, статусом и кнопкой изменения статуса
            Grid headerGrid = new Grid();
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Информация о заявке
            string statusText = GetStatusText(request);
            TextBlock requestInfoText = new TextBlock
            {
                Text = $"Заявка {request.Request_id} - {statusText}",
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51)),
                TextWrapping = TextWrapping.Wrap
            };

            // Добавляем дату если назначена
            if (request.Request_status == "назначена дата" && request.Date_of_completion > DateTime.MinValue)
            {
                requestInfoText.Text += $" - {request.Date_of_completion:dd.MM.yyyy HH:mm}";
            }

            Grid.SetColumn(requestInfoText, 0);
            headerGrid.Children.Add(requestInfoText);

            // Кнопка изменения статуса
            TextBlock changeStatusText = new TextBlock
            {
                Text = "✎ Изменить статус",
                FontSize = 14,
                Foreground = new SolidColorBrush(Color.FromRgb(186, 106, 106)),
                Cursor = Cursors.Hand,
                TextDecorations = TextDecorations.Underline,
                VerticalAlignment = VerticalAlignment.Center
            };

            changeStatusText.MouseLeftButtonUp += (s, e) => OpenChangeStatusWindow(request);
            Grid.SetColumn(changeStatusText, 1);
            headerGrid.Children.Add(changeStatusText);

            Grid.SetRow(headerGrid, 0);
            mainGrid.Children.Add(headerGrid);

            // Разделитель
            Border separator1 = new Border
            {
                Height = 1,
                Background = new SolidColorBrush(Color.FromRgb(224, 224, 224)),
                Margin = new Thickness(0, 10, 0, 10)
            };
            Grid.SetRow(separator1, 1);
            mainGrid.Children.Add(separator1);

            // Информация о клиенте
            Client client = _clients.ContainsKey(request.Client_phone) ? _clients[request.Client_phone] : null;
            string clientInfo = client != null
                ? $"{request.Client_phone} - {client.Client_name}"
                : request.Client_phone;

            TextBlock clientText = new TextBlock
            {
                Text = clientInfo,
                FontSize = 15,
                Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102)),
                Margin = new Thickness(0, 0, 0, 10)
            };
            Grid.SetRow(clientText, 2);
            mainGrid.Children.Add(clientText);

            // Услуги
            StackPanel servicesPanel = new StackPanel();
            var services = _requestServices.ContainsKey(request.Request_id)
                ? _requestServices[request.Request_id]
                : new List<Service>();

            for (int i = 0; i < services.Count; i++)
            {
                var service = services[i];
                TextBlock serviceText = new TextBlock
                {
                    Text = $"• {service.Service_name} - {service.Service_price:C}",
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Color.FromRgb(85, 85, 85)),
                    Margin = new Thickness(0, 0, 0, i == services.Count - 1 ? 0 : 5)
                };
                servicesPanel.Children.Add(serviceText);
            }

            if (services.Count == 0)
            {
                TextBlock noServicesText = new TextBlock
                {
                    Text = "Нет услуг",
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Color.FromRgb(150, 150, 150)),
                    FontStyle = FontStyles.Italic
                };
                servicesPanel.Children.Add(noServicesText);
            }

            Grid.SetRow(servicesPanel, 3);
            mainGrid.Children.Add(servicesPanel);

            card.Child = mainGrid;
            return card;
        }

        private string GetStatusText(Request request)
        {
            switch (request.Request_status)
            {
                case "ожидание":
                    return "⏳ Ожидание";
                case "назначена дата":
                    return "📅 Назначена дата";
                case "выполнено":
                    return "✅ Выполнено";
                case "отказано":
                    return "❌ Отказано";
                default:
                    return request.Request_status;
            }
        }

        private void OpenChangeStatusWindow(Request request)
        {
            var window = new ChangeStatusWindow(request, _facade);
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();

            LoadRequests();
            ApplyFilters();
        }

        private void ApplyFilters()
        {
           
        }

        private void StatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void PhoneSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ClearFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            StatusFilterComboBox.SelectedIndex = 0;
            ApplyFilters();
        }
    }
}