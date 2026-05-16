using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using notary_company.Models;

namespace notary_company.Repositories
{
    public class NotaryRepository : IRepository<Notary>
    {
        private readonly IDbConnection _connection;

        public NotaryRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public int Create(Notary entity)
        {
            const string sql = @"
                INSERT INTO notaries (user_id, notary_name)
                VALUES (@UserId, @NotaryName, @NotaryDescription, @NotaryPhone)
                RETURNING notary_id";

            return _connection.QuerySingle<int>(sql, entity);
        }

        public List<Notary> ReadAll()
        {
            const string sql = "SELECT * FROM notaries ORDER BY notary_name";
            return _connection.Query<Notary>(sql).ToList();
        }

        public Notary ReadById(int id)
        {
            const string sql = "SELECT * FROM notaries WHERE notary_id = @Id";
            return _connection.QueryFirstOrDefault<Notary>(sql, new { Id = id });
        }
        public Notary ReadByUserId(int id)
        {
            const string sql = "SELECT * FROM notaries WHERE user_id = @Id";
            return _connection.QueryFirstOrDefault<Notary>(sql, new { Id = id }); ;
        }
        public void Update(Notary entity)
        {
            const string sql = @"
                UPDATE notaries 
                SET notary_name = @NotaryName,
                    notary_description = @NotaryDescription,
                    notary_phone = @NotaryPhone,
                WHERE notary_id = @NotaryId";

            _connection.Execute(sql, entity);
        }


        public int CreateWithUser(Notary notary, string login, string passwordHash)
        {
            using var transaction = _connection.BeginTransaction();

            try
            {
                const string userSql = @"
                    INSERT INTO users (login, password_hash)
                    VALUES (@Login, @PasswordHash)
                    RETURNING user_id";

                var userId = _connection.QuerySingle<int>(userSql, new
                {
                    Login = login,
                    PasswordHash = passwordHash
                }, transaction);

                const string notarySql = @"
                    INSERT INTO notaries (user_id, notary_name, notary_description, notary_phone)
                    VALUES (@UserId, @Notary_name, @Notary_description, @Notary_phone)
                    RETURNING notary_id";

                var notaryId = _connection.QuerySingle<int>(notarySql, new
                {
                    UserId = userId,
                    notary.Notary_name,
                    notary.Notary_description,
                    notary.Notary_phone,
                }, transaction);

                transaction.Commit();
                return notaryId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}