using System;

namespace notary_company.Dtos
{
    public class Client
    {
        public string Client_phone { get; set; }
        public string Client_name { get; set; }
    }

    public class Request
    {
        public int Request_id { get; set; }
        public string Client_phone { get; set; }
        public string Additional_information { get; set; }
        public string Request_status { get; set; }
        public DateTime Request_date { get; set; }
        public DateTime Date_of_completion { get; set; }
    }

    public class Service
    {
        public int Service_id { get; set; }
        public string Service_name { get; set; }
        public string Service_description { get; set; }
        public decimal Service_price { get; set; }
    }

    public class RequestService
    {
        public int Request_detail_id { get; set; }
        public int Request_id { get; set; }
        public int Service_id { get; set; }
    }

    public class User
    {
        public int User_id { get; set; }
        public string Login { get; set; } // он же email в форме
        public string Password_hash { get; set; }
    }

    public class Notary
    {
        public int Notary_id { get; set; }
        public int User_id { get; set; }
        public string Notary_name { get; set; }
        public string Notary_description { get; set; }
        public string Notary_phone { get; set; }
        public bool Is_notary_helper { get; set; } = true;
    }
}