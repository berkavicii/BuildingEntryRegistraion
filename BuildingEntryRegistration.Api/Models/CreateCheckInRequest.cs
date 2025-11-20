using System;

namespace BuildingEntryRegistration.Api.Models
{
    public class CreateCheckInRequest
    {
        public string EntranceId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public int TeamId { get; set; }
        public bool AcceptedPolicies { get; set; }
    }
}
