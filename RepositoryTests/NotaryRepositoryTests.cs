using Microsoft.VisualStudio.TestTools.UnitTesting;
using notary_company.Repositories;
using notary_company.Models;

namespace RepositoryTests
{
    [TestClass]
    public class NotaryRepositoryTests : RepositoryTestBase
    {
        private NotaryRepository _repo;
        private UserRepository _userRepo;

        [TestInitialize]
        public void Setup()
        {
            _repo = new NotaryRepository(Connection);
            _userRepo = new UserRepository(Connection);
        }

        private int CreateTestUser()
        {
            return _userRepo.Create(new User
            {
                Login = $"{Guid.NewGuid()}@test.com",
                Password_hash = "hash"
            });
        }

        [TestMethod]
        public void Create_ShouldAddNotary()
        {
            var userId = CreateTestUser();
            var notary = new Notary
            {
                User_id = userId,
                Notary_name = $"Нотариус_{Guid.NewGuid()}",
                Notary_phone = Guid.NewGuid().ToString().Substring(0, 11)
            };

            var id = _repo.Create(notary);

            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public void ReadById_ShouldReturnNotary()
        {
            var userId = CreateTestUser();
            var notaryName = $"Нотариус_{Guid.NewGuid()}";
            var notary = new Notary
            {
                User_id = userId,
                Notary_name = notaryName,
                Notary_phone = Guid.NewGuid().ToString().Substring(0, 11)
            };
            var id = _repo.Create(notary);

            var result = _repo.ReadById(id);

            Assert.IsNotNull(result);
            Assert.AreEqual(notaryName, result.Notary_name);
        }

        [TestMethod]
        public void ReadAll_ShouldReturnAllNotaries()
        {
            var userId1 = CreateTestUser();
            var userId2 = CreateTestUser();
            var name1 = $"Нотариус_{Guid.NewGuid()}";
            var name2 = $"Нотариус_{Guid.NewGuid()}";

            _repo.Create(new Notary { User_id = userId1, Notary_name = name1 });
            _repo.Create(new Notary { User_id = userId2, Notary_name = name2 });

            var result = _repo.ReadAll();
            var ourNotaries = result.Where(n => n.Notary_name == name1 || n.Notary_name == name2).ToList();

            Assert.AreEqual(2, ourNotaries.Count);
        }

        [TestMethod]
        public void ReadByUserId_ShouldReturnNotary()
        {
            var userId = CreateTestUser();
            var notaryName = $"Нотариус_{Guid.NewGuid()}";
            var notary = new Notary
            {
                User_id = userId,
                Notary_name = notaryName
            };
            _repo.Create(notary);

            var result = _repo.ReadByUserId(userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.User_id);
        }
    }
}