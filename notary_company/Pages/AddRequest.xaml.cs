using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using notary_company.shared.Dtos;

namespace notary_company.Pages
{
    public partial class AddRequest : Window
    {
        private List<ServiceDto> _availableServices;
        private List<string> _selectedServices;

        public AddRequest()
        {
            InitializeComponent();
            _selectedServices = new List<string>();
            Loaded += AddRequest_Loaded;
            ClientPhoneTextBox.Text = "8";
        }

        private async void AddRequest_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _availableServices = await App.Api.GetServicesAsync();
                ServicesComboBox.ItemsSource = _availableServices;
                ServicesComboBox.DisplayMemberPath = "Service_name";
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Не удалось загрузить список услуг: {ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ServicesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddServiceButton.IsEnabled = ServicesComboBox.SelectedItem != null;
        }

        private void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedService = ServicesComboBox.SelectedItem as ServiceDto;

            if (selectedService == null)
                return;

            if (!_selectedServices.Contains(selectedService.Service_name))
            {
                _selectedServices.Add(selectedService.Service_name);
                UpdateAddedServicesList();
                ServicesTxtStatus.Text = "";
            }
            else
            {
                MessageBox.Show("Эта услуга уже добавлена!", "Внимание",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UpdateAddedServicesList()
        {
            if (_selectedServices.Count == 0)
            {
                AddedServicesText.Text = "Нет добавленных услуг";
            }
            else
            {
                var servicesInfo = _availableServices
                    .Where(s => _selectedServices.Contains(s.Service_name))
                    .ToList();

                string servicesText = "";
                for (int i = 0; i < servicesInfo.Count; i++)
                {
                    servicesText += $"• {servicesInfo[i].Service_name} - {servicesInfo[i].Service_price:C}";
                    if (i < servicesInfo.Count - 1)
                        servicesText += "\n";
                }
                AddedServicesText.Text = servicesText;
            }
        }

        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            string name = ClientNameTextBox.Text;
            string phone = ClientPhoneTextBox.Text;
            string services = AddedServicesText.Text;

            phone = new string(phone.Where(char.IsDigit).ToArray());

            bool isValid = true;

            if (phone.Length != 11)
            {
                PhoneTxtStatus.Text = "Введите 11 цифр номера телефона";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone) || services == "Нет добавленных услуг")
            {
                if (string.IsNullOrEmpty(name))
                    NameTxtStatus.Text = "Обязательное поле";
                if (string.IsNullOrEmpty(phone))
                    PhoneTxtStatus.Text = "Обязательное поле";
                if (services == "Нет добавленных услуг")
                    ServicesTxtStatus.Text = "Выберите хотя бы одну услугу";
                isValid = false;
            }

            if (!isValid)
                return;

            try
            {
                string descr = AdditionalInfoTextBox.Text;

                await App.Api.CreateRequestAsync(new CreateRequestDto
                {
                    Client_phone = phone,
                    Client_name = name,
                    Additional_information = descr,
                    Services = _selectedServices
                });

                MessageBox.Show("Ваша заявка подана.\nC вами скоро свяжутся.", "Успех",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Нет связи с сервером: {ex.Message}", "Ошибка сети",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при создании заявки: {ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClientPhoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char ch in e.Text)
            {
                if (!char.IsDigit(ch))
                {
                    e.Handled = true;
                    return;
                }
            }

            string currentText = ClientPhoneTextBox.Text;
            string digits = new string(currentText.Where(char.IsDigit).ToArray());

            if (digits.Length >= 11)
            {
                e.Handled = true;
            }
        }

        private void ClientPhoneTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int cursorPosition = ClientPhoneTextBox.CaretIndex;

            string digits = new string(ClientPhoneTextBox.Text.Where(char.IsDigit).ToArray());

            if (digits.Length > 11)
                digits = digits.Substring(0, 11);

            if (digits.Length > 0 && digits[0] != '8')
            {
                digits = "8" + (digits.Length > 1 ? digits.Substring(1) : "");
            }

            string formattedPhone = FormatPhoneWithSpaces(digits);

            if (ClientPhoneTextBox.Text != formattedPhone)
            {
                ClientPhoneTextBox.Text = formattedPhone;

                if (cursorPosition <= ClientPhoneTextBox.Text.Length)
                    ClientPhoneTextBox.CaretIndex = cursorPosition;
                else
                    ClientPhoneTextBox.CaretIndex = ClientPhoneTextBox.Text.Length;
            }

            int newPosition = formattedPhone.Length;

            if (newPosition == 0)
                newPosition = 1;

            if (newPosition < formattedPhone.Length && formattedPhone[newPosition] == ' ')
                newPosition++;

            ClientPhoneTextBox.CaretIndex = newPosition;
        }

        private string FormatPhoneWithSpaces(string digits)
        {
            if (string.IsNullOrEmpty(digits))
                return "";

            if (digits.Length == 1)
                return digits;
            else if (digits.Length <= 4)
                return $"{digits.Substring(0, 1)} {digits.Substring(1)}";
            else if (digits.Length <= 7)
                return $"{digits.Substring(0, 1)} {digits.Substring(1, 3)} {digits.Substring(4)}";
            else if (digits.Length <= 9)
                return $"{digits.Substring(0, 1)} {digits.Substring(1, 3)} {digits.Substring(4, 3)} {digits.Substring(7)}";
            else
                return $"{digits.Substring(0, 1)} {digits.Substring(1, 3)} {digits.Substring(4, 3)} {digits.Substring(7, 2)} {digits.Substring(9)}";
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}