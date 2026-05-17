using Microsoft.VisualStudio.TestTools.UnitTesting;
using notary_company.Repositories;
using notary_company.Models;

namespace RepositoryTests
{
    [TestClass]
    public class UserRepositoryTests : RepositoryTestBase
    {
        private UserRepository _repo;

        [TestInitialize]
        public void Setup()
        {
            _repo = new UserRepository(Connection);
        }

        [TestMethod]
        public void Create_ShouldAddUser()
        {
            var user = new User
            {
                Login = $"{Guid.NewGuid()}@test.com",
                Password_hash = "hash123"
            };

            var id = _repo.Create(user);

            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public void ReadById_ShouldReturnUser()
        {
            var user = new User
            {
                Login = $"{Guid.NewGuid()}@test.com",
                Password_hash = "hash123"
            };
            var id = _repo.Create(user);

            var result = _repo.ReadById(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(user.Login, result.Login);
        }

        [TestMethod]
        public void ReadAll_ShouldReturnAllUsers()
        {
            var login1 = $"{Guid.NewGuid()}@test.com";
            var login2 = $"{Guid.NewGuid()}@test.com";

            _repo.Create(new User { Login = login1, Password_hash = "hash1" });
            _repo.Create(new User { Login = login2, Password_hash = "hash2" });

            var result = _repo.ReadAll();
            var ourUsers = result.Where(u => u.Login == login1 || u.Login == login2).ToList();

            Assert.AreEqual(2, ourUsers.Count);
        }

        [TestMethod]
        public void GetByLogin_ShouldReturnUser()
        {
            var login = $"{Guid.NewGuid()}@test.com";
            var user = new User
            {
                Login = login,
                Password_hash = "hash123"
            };
            _repo.Create(user);

            var result = _repo.GetByLogin(login);

            Assert.IsNotNull(result);
            Assert.AreEqual(login, result.Login);
        }

        [TestMethod]
        public void Delete_ShouldRemoveUser()
        {
            var user = new User
            {
                Login = $"{Guid.NewGuid()}@test.com",
                Password_hash = "hash123"
            };
            var id = _repo.Create(user);

            _repo.Delete(id);
            var result = _repo.ReadById(id);

            Assert.IsNull(result);
        }
    }
}