using notary_company.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;

namespace notary_company.Pages
{
    public partial class AddRequest : Window
    {
        Facade _facade;
        private List<Service> _availableServices;
        private List<string> _selectedServices;

        public AddRequest()
        {
            InitializeComponent();
            _facade = App.Facade;

            _selectedServices = new List<string>();

            LoadServices();

            ClientPhoneTextBox.Text = "8";
        }

        private void LoadServices()
        {
            try
            {
                _availableServices = _facade.getAllServices();
                ServicesComboBox.ItemsSource = _availableServices;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось загрузить список услуг: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ServicesComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            AddServiceButton.IsEnabled = ServicesComboBox.SelectedItem != null;
        }

        private void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedService = ServicesComboBox.SelectedItem as Service;

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
                MessageBox.Show("Эта услуга уже добавлена!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void CreateButton_Click(object sender, RoutedEventArgs e)
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

                _facade.createRequest(phone, name, descr, _selectedServices);

                MessageBox.Show("Ваша заявка подана.\nC вами скоро свяжутся.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при создании заявки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void ClientPhoneTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
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

        private void ClientPhoneTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
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

            int newPosition = 0; if (newPosition == 0)
                newPosition = formattedPhone.Length;

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