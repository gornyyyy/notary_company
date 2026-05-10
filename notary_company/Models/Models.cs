using System;

namespace notary_company.Models
{
    public class Client
    {
        public string? ClientPhone { get; set; }
        public string? ClientName { get; set; }
    }

    public class Request
    {
        public int? RequestId { get; set; }
        public string? ClientPhone { get; set; }
        public decimal? TotalApproximatePrice { get; set; }
        public string? AdditionalInformation { get; set; }
        public string? RequestStatus { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? DateOfCompletion { get; set; }
    }

    public class Service
    {
        public int? ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public string? ServiceDescription { get; set; }
        public decimal? ServicePrice { get; set; }
    }

    public class RequestService
    {
        public int? RequestDetailId { get; set; }
        public int? RequestId { get; set; }
        public int? ServiceId { get; set; }
    }

    public class User
    {
        public int? UserId { get; set; }
        public string? Login { get; set; } // он же email в форме
        public string? PasswordHash { get; set; }
    }

    public class Notary
    {
        public int? NotaryId { get; set; }
        public int? UserId { get; set; }
        public string? NotaryName { get; set; }
        public string? NotaryDescription { get; set; }
        public string? NotaryPhone { get; set; }
        public bool? IsNotaryHelper { get; set; }
    }
}