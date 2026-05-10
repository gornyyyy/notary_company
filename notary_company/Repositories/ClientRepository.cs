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
                VALUES (@ClientPhone, @ClientName)
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
            const string sql = "SELECT * FROM clients WHERE client_phone = @Phone";
            return _connection.QueryFirstOrDefault<Client>(sql, new { Phone = id.ToString() });
        }

        public Client GetByPhone(string phone)
        {
            const string sql = "SELECT * FROM clients WHERE client_phone = @Phone";
            return _connection.QueryFirstOrDefault<Client>(sql, new { Phone = phone });
        }

        public void Update(Client entity)
        {
            const string sql = @"
                UPDATE clients 
                SET client_name = @ClientName
                WHERE client_phone = @ClientPhone";

            _connection.Execute(sql, entity);
        }

        public void Delete(string phone)
        {
            const string sql = "DELETE FROM clients WHERE client_phone = @Phone";
            _connection.Execute(sql, new { Phone = phone });
        }
    }
}