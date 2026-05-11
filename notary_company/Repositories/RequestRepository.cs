using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using notary_company.Models;

namespace notary_company.Repositories
{
    public class RequestRepository : IRepository<Request>
    {
        private readonly IDbConnection _connection;

        public RequestRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public int Create(Request entity)
        {
            const string sql = @"
                INSERT INTO requests (client_phone, additional_information, request_date)
                VALUES (@ClientPhone, @AdditionalInformation, @RequestDate)
                RETURNING request_id";

            return _connection.QuerySingle<int>(sql, new
            {
                entity.Client_phone,
                entity.Additional_information,
                RequestDate = DateTime.Now
            });
        }

        public List<Request> ReadAll()
        {
            const string sql = "SELECT * FROM requests ORDER BY request_date DESC";
            return _connection.Query<Request>(sql).ToList();
        }

        public Request ReadById(int id)
        {
            const string sql = "SELECT * FROM requests WHERE request_id = @Id";
            return _connection.QueryFirstOrDefault<Request>(sql, new { Id = id });
        }

        public void UpdateStatus(int requestId, string status, DateTime? dateOfCompletion)
        {
            const string sql = @"
                UPDATE requests 
                SET request_status = @Status, 
                    date_of_completion = @DateOfCompletion
                WHERE request_id = @RequestId";

            _connection.Execute(sql, new
            {
                RequestId = requestId,
                Status = status,
                DateOfCompletion = dateOfCompletion
            });
        }

        private void AddServiceToRequest(int requestId, int serviceId)
        {
            const string sql = @"
                INSERT INTO request_services (request_id, service_id)
                VALUES (@RequestId, @ServiceId)
                ON CONFLICT (request_id, service_id) DO NOTHING";

            _connection.Execute(sql, new { RequestId = requestId, ServiceId = serviceId });
        }

        public List<Service> GetServicesForRequest(int requestId)
        {
            const string sql = @"
                SELECT s.* 
                FROM services s
                JOIN request_services rs ON s.service_id = rs.service_id
                WHERE rs.request_id = @RequestId";

            return _connection.Query<Service>(sql, new { RequestId = requestId }).ToList();
        }

        public List<Request> GetByClientPhone(string phone)
        {
            const string sql = "SELECT * FROM requests WHERE client_phone = @Phone ORDER BY request_date DESC";
            return _connection.Query<Request>(sql, new { Phone = phone }).ToList();
        }

        public List<Request> GetByStatus(string status)
        {
            const string sql = "SELECT * FROM requests WHERE request_status = @Status ORDER BY request_date DESC";
            return _connection.Query<Request>(sql, new { Status = status }).ToList();
        }

        public int CreateWithServices(Request request, List<int> serviceIds)
        {
            using var transaction = _connection.BeginTransaction();

            try
            {
                const string requestSql = @"
                    INSERT INTO requests (client_phone, additional_information, request_date, request_status)
                    VALUES (@ClientPhone, @AdditionalInformation, @RequestDate, 'ожидание')
                    RETURNING request_id";

                var requestId = _connection.QuerySingle<int>(requestSql, new
                {
                    request.Client_phone,
                    request.Additional_information,
                    RequestDate = DateTime.Now
                }, transaction);

                foreach (var serviceId in serviceIds)
                {
                    AddServiceToRequest(request.Request_id, serviceId);
                }

                transaction.Commit();
                return requestId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}