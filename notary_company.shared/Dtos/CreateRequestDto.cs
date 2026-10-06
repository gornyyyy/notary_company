using System.Collections.Generic;

namespace notary_company.shared.Dtos
{
    public class CreateRequestDto
    {
        public string Client_phone { get; set; }
        public string Client_name { get; set; }
        public string Additional_information { get; set; }
        public List<string> Services { get; set; } = new();
    }
}