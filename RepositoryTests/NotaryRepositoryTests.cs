using Microsoft.VisualStudio.TestTools.UnitTesting;
using notary_company.Repositories;
using notary_company.Models;

namespace TestProj
{
    [TestClass]
    public class NotaryRepositoryTests : RepositoryTestBase
    {
        private NotaryRepository _repo;
        private UserRepository _userRepo;

        [TestInitialize]
        public new void TestInitialize()
        {
            base.TestInitialize();
            _repo = new NotaryRepository(Connection);
            _userRepo = new UserRepository(Connection);
        }

        private int CreateTestUser()
        {
            var user = new User
            {
                Login = $"{Guid.NewGuid():N}@test.com",
                Password_hash = "hash"
            };
            return _userRepo.Create(user);
        }

        [TestMethod]
        public void Create_ShouldAddNotary()
        {
            var userId = CreateTestUser();
            var notary = new Notary
            {
                User_id = userId,
                Notary_name = $"Нотариус_{Guid.NewGuid():N}",
                Notary_phone = UniquePhone()
            };

            var id = _repo.Create(notary);

            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public void ReadById_ShouldReturnNotary()
        {
            var userId = CreateTestUser();
            var notaryName = $"Нотариус_{Guid.NewGuid():N}";
            var notary = new Notary
            {
                User_id = userId,
                Notary_name = notaryName,
                Notary_phone = UniquePhone()
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
            var name1 = $"Нотариус_{Guid.NewGuid():N}";
            var name2 = $"Нотариус_{Guid.NewGuid():N}";

            _repo.Create(new Notary { User_id = userId1, Notary_name = name1, Notary_phone = UniquePhone() });
            _repo.Create(new Notary { User_id = userId2, Notary_name = name2, Notary_phone = UniquePhone() });

            var result = _repo.ReadAll();
            var ourNotaries = result.Where(n => n.Notary_name == name1 || n.Notary_name == name2).ToList();

            Assert.AreEqual(2, ourNotaries.Count);
        }

        [TestMethod]
        public void ReadByUserId_ShouldReturnNotary()
        {
            var userId = CreateTestUser();
            var notaryName = $"Нотариус_{Guid.NewGuid():N}";
            var notary = new Notary
            {
                User_id = userId,
                Notary_name = notaryName,
                Notary_phone = UniquePhone()
            };
            _repo.Create(notary);

            var result = _repo.ReadByUserId(userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.User_id);
        }

        private static string UniquePhone() => Guid.NewGuid().ToString("N")[..11];
    }
}