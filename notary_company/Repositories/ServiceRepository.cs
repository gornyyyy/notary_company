using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using notary_company.Models;

namespace notary_company.Repositories
{
    public class ServiceRepository : IRepository<Service>
    {
        private readonly IDbConnection _connection;

        public ServiceRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public int Create(Service entity)
        {
            const string sql = @"
                INSERT INTO services (service_name, service_description, service_price)
                VALUES (@Service_name, @Service_description, @Service_price)
                RETURNING service_id";

            return _connection.QuerySingle<int>(sql, entity);
        }

        public List<Service> ReadAll()
        {
            const string sql = "SELECT * FROM services ORDER BY service_name";
            return _connection.Query<Service>(sql).ToList();
        }

        public Service ReadById(int id)
        {
            const string sql = "SELECT * FROM services WHERE service_id = @Id";
            return _connection.QueryFirstOrDefault<Service>(sql, new { Id = id });
        }

        public Service ReadByServiceName(string service_name)
        {
            const string sql = "SELECT * FROM services WHERE service_name = @service_name";
            return _connection.QueryFirstOrDefault<Service>(sql, new { service_name });
        }
        public void Update(Service entity)
        {
            const string sql = @"
                UPDATE services 
                SET service_name = @Service_name,
                    service_description = @Service_description,
                    service_price = @Service_price
                WHERE service_id = @Service_id";

            _connection.Execute(sql, entity);
        }

        public void Delete(int id)
        {
            const string sql = "DELETE FROM services WHERE service_id = @Id";
            _connection.Execute(sql, new { Id = id });
        }
    }
}