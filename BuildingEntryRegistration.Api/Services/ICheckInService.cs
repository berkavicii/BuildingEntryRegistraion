using BuildingEntryRegistration.Api.Models;

namespace BuildingEntryRegistration.Api.Services
{
    public interface ICheckInService
    {
        Task<IReadOnlyCollection<Team>> GetTeamsAsync();
        Task<bool> IsEntranceValidAsync(string entranceId);
        Task<VisitorCheckIn> CreateCheckInAsync(string entranceId,string fullName,string email,string companyName,int teamId,bool acceptedPolicies);
    }
}