using notary_company.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;

namespace notary_company.Pages
{
    public partial class ChangeStatusWindow : Window
    {
        private readonly Request _request;
        private readonly Facade _facade;

        public ChangeStatusWindow(Request request, Facade facade)
        {
            InitializeComponent();
            _request = request;
            _facade = facade;

            try
            {
                foreach (ComboBoxItem item in StatusComboBox.Items)
                {
                    if (item.Content.ToString() == _request.Request_status)
                    {
                        StatusComboBox.SelectedItem = item;
                        break;
                    }
                }

                if (_request.Date_of_completion > DateTime.MinValue)
                {
                    DateTime localDate = ConvertUtcToLocal(_request.Date_of_completion);
                    DatePicker.SelectedDate = localDate.Date;
                    TimeTextBox.Text = localDate.ToString("HH:mm");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке статуса: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private DateTime ConvertUtcToLocal(DateTime utcDate)
        {
            if (utcDate == DateTime.MinValue)
                return DateTime.MinValue;

            return DateTime.SpecifyKind(utcDate, DateTimeKind.Utc).ToLocalTime();
        }

        private DateTime ConvertLocalToUtc(DateTime localDate)
        {
            if (localDate == DateTime.MinValue)
                return DateTime.MinValue;

            return DateTime.SpecifyKind(localDate, DateTimeKind.Local).ToUniversalTime();
        }

        private void StatusComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedStatus = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (selectedStatus == "назначена дата")
            {
                DatePanel.Visibility = Visibility.Visible;
            }
            else
            {
                DatePanel.Visibility = Visibility.Collapsed;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedStatus = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                DateTime? assignedDateUtc = null;

                if (selectedStatus == "назначена дата")
                {
                    if (DatePicker.SelectedDate.HasValue)
                    {
                        var date = DatePicker.SelectedDate.Value;
                        var time = TimeTextBox.Text;
                        var timeParts = time.Split(':');

                        if (timeParts.Length == 2 && int.TryParse(timeParts[0], out int hour) && int.TryParse(timeParts[1], out int minute))
                        {
                            DateTime localDateTime = new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
                            assignedDateUtc = ConvertLocalToUtc(localDateTime);
                        }
                        else
                        {
                            assignedDateUtc = ConvertLocalToUtc(date);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста, выберите дату", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                _facade.updateRequestStatus(_request.Request_id, selectedStatus, assignedDateUtc);
                MessageBox.Show("Статус успешно изменен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (ValidationException ex)
            {
                MessageBox.Show($"Ошибка валидации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении статуса: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}