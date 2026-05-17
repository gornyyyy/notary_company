using Microsoft.VisualStudio.TestTools.UnitTesting;
using notary_company.Repositories;
using notary_company.Models;

namespace RepositoryTests
{
    [TestClass]
    public class ServiceRepositoryTests : RepositoryTestBase
    {
        private ServiceRepository _repo;

        [TestInitialize]
        public void Setup()
        {
            _repo = new ServiceRepository(Connection);
        }

        [TestMethod]
        public void Create_ShouldAddService()
        {
            var service = new Service
            {
                Service_name = $"Услуга_{Guid.NewGuid()}",
                Service_description = "Описание услуги",
                Service_price = 1000.00m
            };

            var id = _repo.Create(service);

            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public void ReadById_ShouldReturnService()
        {
            var service = new Service
            {
                Service_name = $"Услуга_{Guid.NewGuid()}",
                Service_description = "Описание услуги",
                Service_price = 1000.00m
            };
            var id = _repo.Create(service);

            var result = _repo.ReadById(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(service.Service_name, result.Service_name);
        }

        [TestMethod]
        public void ReadAll_ShouldReturnAllServices()
        {
            var name1 = $"Услуга_{Guid.NewGuid()}";
            var name2 = $"Услуга_{Guid.NewGuid()}";

            _repo.Create(new Service { Service_name = name1, Service_price = 100 });
            _repo.Create(new Service { Service_name = name2, Service_price = 200 });

            var result = _repo.ReadAll();
            var ourServices = result.Where(s => s.Service_name == name1 || s.Service_name == name2).ToList();

            Assert.AreEqual(2, ourServices.Count);
        }

        [TestMethod]
        public void Update_ShouldUpdateService()
        {
            var service = new Service
            {
                Service_name = $"Старое_{Guid.NewGuid()}",
                Service_description = "Старое описание",
                Service_price = 1000.00m
            };
            var id = _repo.Create(service);
            service.Service_id = id;
            service.Service_name = $"Новое_{Guid.NewGuid()}";

            _repo.Update(service);
            var updated = _repo.ReadById(id);

            Assert.AreEqual(service.Service_name, updated.Service_name);
        }

        [TestMethod]
        public void Delete_ShouldRemoveService()
        {
            var service = new Service
            {
                Service_name = $"Услуга_{Guid.NewGuid()}",
                Service_price = 1000.00m
            };
            var id = _repo.Create(service);

            _repo.Delete(id);
            var result = _repo.ReadById(id);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ReadByServiceName_ShouldReturnService()
        {
            var name = $"Уникальное_имя_{Guid.NewGuid()}";
            var service = new Service
            {
                Service_name = name,
                Service_price = 1000.00m
            };
            _repo.Create(service);

            var result = _repo.ReadByServiceName(name);

            Assert.IsNotNull(result);
            Assert.AreEqual(name, result.Service_name);
        }
    }
}