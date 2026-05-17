using notary_company.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace notary_company
{
    public interface IClient
    {
        public void createRequest(string client_phone, string descr, List<string> services);
        public List<Service> getAllServices();

    }

    public interface INotary
    {
        public void addHelper(string name, string login, string passwordhash, string descr);

        public void updateRequestStatus(int request_id, string new_status, DateTime date_of_completion);
        public List<Request> getAllRequests();
        public List<Service> getServicesForRequest();

        public Notary GetNotaryByLogin(string login);
        public string GetNotaryPasswordByLogin(string login);

        public List<Notary> getAllNotary();

    }
}
