namespace BuildingEntryRegistration.Api.Models
{
    public class VisitorCheckIn
    {
        // DTO-style: provide non-null defaults and use DateTimeOffset for timestamps
        public Guid Id { get; set; }
        public string EntranceId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public int TeamId { get; set; }
        public bool AcceptedPolicies { get; set; }
        public DateTimeOffset CheckInDate { get; set; }
    }
}