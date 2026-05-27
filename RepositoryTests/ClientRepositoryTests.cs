using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using notary_company.Models;
using notary_company.Repositories;

namespace TestProj
{
    [TestClass]
    public class ClientRepositoryTests : RepositoryTestBase
    {
        private ClientRepository _repo;

        [TestInitialize]
        public new void TestInitialize()
        {
            base.TestInitialize();
            _repo = new ClientRepository(Connection);
        }

        [TestMethod]
        public void Create_ShouldAddClient()
        {
            var client = new Client
            {
                Client_phone = UniquePhone(),
                Client_name = "Иван Петров"
            };

            var result = _repo.Create(client);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Create_DuplicatePhone_ShouldNotThrowAndReturnZero()
        {
            var client = new Client { Client_phone = UniquePhone(), Client_name = "Иван" };
            _repo.Create(client);

            var result = _repo.Create(client);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void ReadByPhone_ShouldReturnClient()
        {
            var phone = UniquePhone();
            var client = new Client { Client_phone = phone, Client_name = "Иван Петров" };
            _repo.Create(client);

            var result = _repo.ReadByPhone(phone);

            Assert.IsNotNull(result);
            Assert.AreEqual(phone, result.Client_phone);
            Assert.AreEqual("Иван Петров", result.Client_name);
        }

        [TestMethod]
        public void ReadByPhone_NonExistent_ShouldReturnNull()
        {
            var result = _repo.ReadByPhone("000-not-exists");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ReadAll_ShouldReturnAllClients()
        {
            var phone1 = UniquePhone();
            var phone2 = UniquePhone();

            _repo.Create(new Client { Client_phone = phone1, Client_name = "Иван" });
            _repo.Create(new Client { Client_phone = phone2, Client_name = "Петр" });

            var result = _repo.ReadAll();
            var ourClients = result
                .Where(c => c.Client_phone == phone1 || c.Client_phone == phone2)
                .ToList();

            Assert.AreEqual(2, ourClients.Count);
        }

        [TestMethod]
        public void ReadAll_EmptyTable_ShouldReturnEmptyList()
        {
            var result = _repo.ReadAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Update_ShouldUpdateClientName()
        {
            var phone = UniquePhone();
            var client = new Client { Client_phone = phone, Client_name = "Старое имя" };
            _repo.Create(client);

            client.Client_name = "Новое имя";
            _repo.Update(client);

            var updated = _repo.ReadByPhone(phone);
            Assert.AreEqual("Новое имя", updated.Client_name);
        }

        [TestMethod]
        public void Update_NonExistentClient_ShouldNotThrow()
        {
            var ghost = new Client { Client_phone = "000-ghost", Client_name = "Призрак" };

            _repo.Update(ghost);

            Assert.IsNull(_repo.ReadByPhone(ghost.Client_phone));
        }

        private static string UniquePhone() =>
            Guid.NewGuid().ToString("N")[..11];
    }
}