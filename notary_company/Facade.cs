using notary_company.Models;
using notary_company.Repositories;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace notary_company
{
    public class Facade
    {
        private readonly IDbConnection _connection;

        public ClientRepository Clients { get; private set; }
        public RequestRepository Requests { get; private set; }
        public ServiceRepository Services { get; private set; }
        public UserRepository Users { get; private set; }
        public NotaryRepository Notaries { get; private set; }


        public Facade(IDbConnection connection)
        {
            _connection = connection;
            _connection.Open(); 

            Clients = new ClientRepository(_connection);
            Requests = new RequestRepository(_connection);
            Services = new ServiceRepository(_connection);
            Users = new UserRepository(_connection);
            Notaries = new NotaryRepository(_connection);
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }


        public string GetNotaryNameByLogin(string login)
        {
            User user = Users.GetByLogin(login);

            Notary notary = Notaries.ReadByUserId(user.User_id);

            return notary.Notary_name;
        }

        public string GetNotaryPasswordByLogin(string login)
        {
            User user = Users.GetByLogin(login);

            if (user != null)
            {
                return user.Password_hash;
            }
            else return null;
        }
    }
}
