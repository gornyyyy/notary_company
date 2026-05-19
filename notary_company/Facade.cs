using notary_company.Models;
using notary_company.Repositories;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Text;

namespace notary_company
{
    public class Facade : INotary, IClient
    {
        private readonly IDbConnection _connection;

        public ClientRepository Clients { get; private set; }
        public RequestRepository Requests { get; private set; }
        public ServiceRepository Services { get; private set; }
        public UserRepository Users { get; private set; }
        public NotaryRepository Notaries { get; private set; }


        public Facade(IDbConnection connection)
        {
            _connection = connection;
            _connection.Open();

            Clients = new ClientRepository(_connection);
            Requests = new RequestRepository(_connection);
            Services = new ServiceRepository(_connection);
            Users = new UserRepository(_connection);
            Notaries = new NotaryRepository(_connection);
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }

        public void addHelper(string name, string login, string passwordhash, string descr)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ValidationException("ФИО не может быть пустым");
                if (string.IsNullOrWhiteSpace(login))
                    throw new ValidationException("Логин не может быть пустым");

                Notary notary = new Notary();
                notary.Notary_name = name;
                notary.Notary_description = descr;
                Notaries.CreateWithUser(notary, login, passwordhash);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при добавлении помощника: {ex.Message}", ex);
            }
        }

        public void updateRequestStatus(int request_id, string new_status, DateTime? date_of_completion)
        {
            try
            {
                var request = Requests.ReadById(request_id);

                if (request == null)
                {
                    throw new ValidationException($"Заявка с ID {request_id} не найдена");
                }

                Requests.UpdateStatus(request_id, new_status, date_of_completion);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при обновлении статуса заявки: {ex.Message}", ex);
            }
        }

        public List<Request> getAllRequests()
        {
            try
            {
                return Requests.ReadAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении списка заявок: {ex.Message}", ex);
            }
        }

        public List<Service> getServicesForRequest(int request_id)
        {
            try
            {
                return Requests.GetServicesForRequest(request_id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении услуг для заявки: {ex.Message}", ex);
            }
        }

        public string getClientName(string phone)
        {
            try
            {
                var client = Clients.ReadByPhone(phone);

                return client.Client_name;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении имени клиента: {ex.Message}", ex);
            }
        }

        public Notary GetNotaryByLogin(string login)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(login))
                    throw new ValidationException("Логин не может быть пустым");

                User user = Users.GetByLogin(login);

                Notary notary = Notaries.ReadByUserId(user.User_id);

                return notary;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении нотариуса по логину: {ex.Message}", ex);
            }
        }

        public string GetNotaryPasswordByLogin(string login)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(login))
                    throw new ValidationException("Логин не может быть пустым");

                User user = Users.GetByLogin(login);
            
                return user.Password_hash;

            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении пароля нотариуса: {ex.Message}", ex);
            }
        }

        public List<Notary> getAllNotary()
        {
            try
            {
                return Notaries.ReadAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении списка нотариусов: {ex.Message}", ex);
            }
        }

        // IClient
        public void createRequest(string client_phone, string client_name, string? descr, List<string> services)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(client_phone))
                    throw new ValidationException("Телефон клиента не может быть пустым");
                if (string.IsNullOrWhiteSpace(client_name))
                    throw new ValidationException("Имя клиента не может быть пустым");
                if (services == null || services.Count == 0)
                    throw new ValidationException("Необходимо выбрать хотя бы одну услугу");

                Client client = new Client();
                client.Client_phone = client_phone;
                client.Client_name = client_name;
                Clients.Create(client);

                Request request = new Request();
                request.Client_phone = client_phone;
                request.Additional_information = descr;
                request.Request_date = DateTime.Now;

                List<int> serviceIds = new List<int>();

                foreach (string service_name in services)
                {
                    Service service = Services.ReadByServiceName(service_name);
                    if (service == null)
                        throw new ValidationException($"Услуга '{service_name}' не найдена в базе данных");
                    serviceIds.Add(service.Service_id);
                }

                Requests.CreateWithServices(request, serviceIds);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при создании заявки: {ex.Message}", ex);
            }
        }

        public List<Service> getAllServices()
        {
            try
            {
                return Services.ReadAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении списка услуг: {ex.Message}", ex);
            }
        }
    }
}