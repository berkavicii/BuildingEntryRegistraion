using System;

namespace BuildingEntryRegistration.Api.Models
{
    // DTO used by storage service and higher layers
    public class CheckIn
    {
        public Guid Id { get; set; }
        public string EntranceId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public int TeamId { get; set; }
        public bool AcceptedPolicies { get; set; }
        public DateTimeOffset TimestampUtc { get; set; }
    }
}
