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

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone) || services == "Нет добавленных услуг")
            {
                if (string.IsNullOrEmpty(name))
                    NameTxtStatus.Text = "Обязательное поле";
                if (string.IsNullOrEmpty(phone))
                    PhoneTxtStatus.Text = "Обязательное поле";
                if (services == "Нет добавленных услуг")
                    ServicesTxtStatus.Text = "Выберите хотя бы одну услугу";
            }
            else
            {
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
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}