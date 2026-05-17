using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using notary_company.Models;

namespace notary_company.Repositories
{
    public class ClientRepository : IRepository<Client>
    {
        private readonly IDbConnection _connection;

        public ClientRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public int Create(Client entity)
        {
            const string sql = @"
                INSERT INTO clients (client_phone, client_name)
                VALUES (@Client_phone, @Client_name)
                ON CONFLICT (client_phone) DO NOTHING";

            return _connection.Execute(sql, entity);
        }

        public List<Client> ReadAll()
        {
            const string sql = "SELECT * FROM clients ORDER BY client_name";
            return _connection.Query<Client>(sql).ToList();
        }

        public Client ReadById(int id)
        {
            throw new NotImplementedException("Используйте ReadByPhone(string phone) для поиска клиента по номеру телефона");
        }

        public Client ReadByPhone(string phone)
        {
            const string sql = "SELECT * FROM clients WHERE client_phone = @Phone";
            return _connection.QueryFirstOrDefault<Client>(sql, new { Phone = phone });
        }

        public void Update(Client entity)
        {
            const string sql = @"
                UPDATE clients 
                SET client_name = @Client_name
                WHERE client_phone = @Client_phone";

            _connection.Execute(sql, entity);
        }
    }
}