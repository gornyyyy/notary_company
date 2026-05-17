using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using notary_company.Models;
using notary_company.Repositories;
using System;
using System.Linq;

namespace RepositoryTests
{
    [TestClass]
    public class RequestRepositoryTests : RepositoryTestBase
    {
        private RequestRepository _repo;
        private ClientRepository _clientRepo;

        [TestInitialize]
        public void Setup()
        {
            _repo = new RequestRepository(Connection);
            _clientRepo = new ClientRepository(Connection);
            // Очищаем таблицы перед каждым тестом
            Connection.Execute("DELETE FROM requests");
            Connection.Execute("DELETE FROM clients");
        }

        private string CreateTestClient()
        {
            var phone = Guid.NewGuid().ToString().Substring(0, 11);
            _clientRepo.Create(new Client { Client_phone = phone, Client_name = "Клиент" });
            return phone;
        }

        [TestMethod]
        public void Create_ShouldAddRequest()
        {
            var phone = CreateTestClient();
            var request = new Request
            {
                Client_phone = phone,
                Additional_information = "Срочно"
            };

            var id = _repo.Create(request);

            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public void ReadById_ShouldReturnRequest()
        {
            var phone = CreateTestClient();
            var request = new Request { Client_phone = phone };
            var id = _repo.Create(request);

            var result = _repo.ReadById(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(phone, result.Client_phone);
        }

        [TestMethod]
        public void ReadAll_ShouldReturnAllRequests()
        {
            var phone = CreateTestClient();

            _repo.Create(new Request { Client_phone = phone });
            _repo.Create(new Request { Client_phone = phone });

            var result = _repo.ReadAll();
            var ourRequests = result.Where(r => r.Client_phone == phone).ToList();

            Assert.AreEqual(2, ourRequests.Count);
        }

        [TestMethod]
        public void UpdateStatus_ShouldChangeToCompleted()
        {
            var phone = CreateTestClient();
            var request = new Request { Client_phone = phone };
            var id = _repo.Create(request);

            _repo.UpdateStatus(id, "выполнено", null);
            var updated = _repo.ReadById(id);

            Assert.AreEqual("выполнено", updated.Request_status);
        }

        [TestMethod]
        public void UpdateStatus_ShouldAssignDateAndChangeStatus()
        {
            var phone = CreateTestClient();
            var request = new Request { Client_phone = phone };
            var id = _repo.Create(request);
            var assignedDate = new DateTime(2025, 12, 25, 14, 30, 0);

            _repo.UpdateStatus(id, "назначена дата", assignedDate);
            var updated = _repo.ReadById(id);

            Assert.AreEqual("назначена дата", updated.Request_status);
            Assert.AreEqual(assignedDate.Date, updated.Date_of_completion.Date); //только дата
        }

        [TestMethod]
        public void GetByClientPhone_ShouldReturnClientRequests()
        {
            var phone = CreateTestClient();

            _repo.Create(new Request { Client_phone = phone });
            _repo.Create(new Request { Client_phone = phone });

            var result = _repo.GetByClientPhone(phone);

            Assert.AreEqual(2, result.Count);
        }

        
    }
}