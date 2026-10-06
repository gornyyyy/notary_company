using notary_company.shared.Dtos;
using System;
using System.Collections.Generic;

namespace notary_company.shared.Dtos
{
    public class RequestDto
    {
        public int Request_id { get; set; }
        public string Client_phone { get; set; }
        public string Client_name { get; set; }
        public string Additional_information { get; set; }
        public string Request_status { get; set; }
        public DateTime Request_date { get; set; }
        public DateTime? Date_of_completion { get; set; }
        public List<ServiceDto> Services { get; set; } = new();
    }
}