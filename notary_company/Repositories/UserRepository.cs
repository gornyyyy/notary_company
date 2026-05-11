using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using notary_company.Models;

namespace notary_company.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly IDbConnection _connection;

        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public int Create(User entity)
        {
            const string sql = @"
                INSERT INTO users (login, password_hash)
                VALUES (@Login, @PasswordHash)
                RETURNING user_id";

            return _connection.QuerySingle<int>(sql, entity);
        }

        public List<User> ReadAll()
        {
            const string sql = "SELECT * FROM users ORDER BY login";
            return _connection.Query<User>(sql).ToList();
        }

        public User ReadById(int id)
        {
            const string sql = "SELECT * FROM users WHERE user_id = @Id";
            return _connection.QueryFirstOrDefault<User>(sql, new { Id = id });
        }

        public User GetByLoginAndPassword(string login, string passwordHash)
        {
            const string sql = "SELECT * FROM users WHERE login = @Login AND password_hash = @PasswordHash";
            return _connection.QueryFirstOrDefault<User>(sql, new { Login = login, PasswordHash = passwordHash });
        }

        public User GetByLogin(string login)
        {
            const string sql = "SELECT * FROM users WHERE login = @Login";
            return _connection.QueryFirstOrDefault<User>(sql, new { Login = login });
        }

        public void UpdatePassword(int userId, string newPasswordHash)
        {
            const string sql = "UPDATE users SET password_hash = @PasswordHash WHERE user_id = @UserId";
            _connection.Execute(sql, new { UserId = userId, PasswordHash = newPasswordHash });
        }

        public void Delete(int userId)
        {
            const string sql = "DELETE FROM users WHERE user_id = @UserId";
            _connection.Execute(sql, new { UserId = userId });
        }
    }
}