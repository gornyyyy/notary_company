using Microsoft.VisualStudio.TestTools.UnitTesting;
using notary_company;
using notary_company.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TestProj;

namespace FacadeTests
{
    [TestClass]
    public class FacadeTests : RepositoryTestBase
    {
        private Facade _facade;

        [TestInitialize]
        public new void TestInitialize()
        {
            base.TestInitialize();
            _facade = new Facade(Connection);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _facade?.Dispose();
        }

        [TestMethod]
        public void addHelper_ValidData_ShouldNotThrow()
        {
            string login = $"{Guid.NewGuid():N}@helper.com";

            _facade.addHelper("Петров Иван Сидорович", login, "secret_hash", "Опытный помощник");

            var notary = _facade.GetNotaryByLogin(login);
            Assert.IsNotNull(notary);
            Assert.AreEqual("Петров Иван Сидорович", notary.Notary_name);
        }

        [TestMethod]
        public void addHelper_EmptyName_ShouldThrowValidationException()
        {
            // В MSTest 4.0 используется Assert.Throws<T> вместо ThrowsException<T>
            Assert.Throws<ValidationException>(() =>
            {
                _facade.addHelper("", "login@test.com", "hash", "descr");
            });
        }

        [TestMethod]
        public void addHelper_EmptyLogin_ShouldThrowValidationException()
        {
            Assert.Throws<ValidationException>(() =>
            {
                _facade.addHelper("Имя", " ", "hash", "descr");
            });
        }

        [TestMethod]
        public void createRequest_ValidData_ShouldCreateClientAndRequestWithServices()
        {
            string serviceName = $"Услуга_{Guid.NewGuid():N}";
            _facade.Services.Create(new Service
            {
                Service_name = serviceName,
                Service_price = 500.00m
            });

            string phone = Guid.NewGuid().ToString("N")[..11];
            var servicesList = new List<string> { serviceName };

            _facade.createRequest(phone, "Алексей", "Нужна копия паспорта", servicesList);

            var clientName = _facade.getClientName(phone);
            Assert.AreEqual("Алексей", clientName);

            var requests = _facade.getAllRequests();
            var clientRequest = requests.FirstOrDefault(r => r.Client_phone == phone);

            Assert.IsNotNull(clientRequest);
            Assert.AreEqual("Нужна копия паспорта", clientRequest.Additional_information);
        }

        [TestMethod]
        public void createRequest_EmptyServices_ShouldThrowValidationException()
        {
            string phone = Guid.NewGuid().ToString("N")[..11];

            Assert.Throws<ValidationException>(() =>
            {
                _facade.createRequest(phone, "Клиент", "Описание", new List<string>());
            });
        }

        [TestMethod]
        public void updateRequestStatus_ExistingRequest_ShouldUpdateStatus()
        {
            string phone = Guid.NewGuid().ToString("N")[..11];
            _facade.Clients.Create(new Client { Client_phone = phone, Client_name = "Тест" });
            int requestId = _facade.Requests.Create(new Request { Client_phone = phone });

            DateTime completionDate = DateTime.Now.AddDays(1);
            _facade.updateRequestStatus(requestId, "назначена дата", completionDate);

            var allRequests = _facade.getAllRequests();
            var updatedRequest = allRequests.FirstOrDefault(r => r.Request_id == requestId);

            Assert.IsNotNull(updatedRequest);
            Assert.AreEqual("назначена дата", updatedRequest.Request_status);
        }

        [TestMethod]
        public void updateRequestStatus_NonExistentRequest_ShouldThrowValidationException()
        {
            Assert.Throws<ValidationException>(() =>
            {
                _facade.updateRequestStatus(99999, "выполнено", null);
            });
        }

        [TestMethod]
        public void GetNotaryPasswordByLogin_ExistingUser_ShouldReturnCorrectHash()
        {
            string login = $"{Guid.NewGuid():N}@notary.com";
            string passwordHash = "super_secure_hash_123";

            _facade.Users.Create(new User { Login = login, Password_hash = passwordHash });

            string actualHash = _facade.GetNotaryPasswordByLogin(login);

            Assert.AreEqual(passwordHash, actualHash);
        }
    }
}