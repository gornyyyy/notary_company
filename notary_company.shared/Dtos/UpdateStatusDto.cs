using System;

namespace notary_company.shared.Dtos
{
    public class UpdateStatusDto
    {
        public string New_status { get; set; }
        public DateTime? Date_of_completion { get; set; }
    }
}