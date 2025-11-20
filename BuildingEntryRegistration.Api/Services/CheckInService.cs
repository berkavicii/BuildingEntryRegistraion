using BuildingEntryRegistration.Api.Models;
using System.Collections.Generic;

namespace BuildingEntryRegistration.Api.Services{
    public class CheckInService : ICheckInService
    {
        private readonly IStorageService _store;

        public CheckInService(IStorageService store)
        {
            _store = store;
        }

        public async Task<IReadOnlyCollection<Team>> GetTeamsAsync()
        {
            var teams = _store.GetTeams().ToList().AsReadOnly();
            return await Task.FromResult(teams);
        }
        public async Task<bool> IsEntranceValidAsync(string entranceId)
        {
            var exists = _store.GetEntrances().Any(e => e.Id == entranceId);
            return await Task.FromResult(exists);
        }
        public async Task<VisitorCheckIn> CreateCheckInAsync(string entranceId,string fullName,string email,string companyName,int teamId,bool acceptedPolicies)
    {
        if (string.IsNullOrWhiteSpace(entranceId) ||
            !await IsEntranceValidAsync(entranceId))
        {
            throw new InvalidOperationException("Entrance Id should be valid");
        }

        // 2) FullName & Email boş olamaz
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new InvalidOperationException("Full name is required.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new InvalidOperationException("Email is required.");
        }

        // 3) Team kontrolü
        var team = _store.GetTeam(teamId);
        if (team is null)
        {
            throw new InvalidOperationException("Selected team does not exist.");
        }

        // 4) Policies zorunlu
        if (!acceptedPolicies)
        {
            throw new InvalidOperationException("Rules & policies must be accepted.");
        }

        // 5) Check-in kaydını oluştur
        var checkIn = new CheckIn
        {
            Id = Guid.NewGuid(),
            EntranceId = entranceId,
            FullName = fullName.Trim(),
            Email = email.Trim(),
            CompanyName = companyName?.Trim() ?? string.Empty,
            TeamId = teamId,
            AcceptedPolicies = true
        };

        var stored = _store.AddCheckIn(checkIn);

        // Map back to VisitorCheckIn (existing API contract)
        return new VisitorCheckIn
        {
            Id = stored.Id,
            EntranceId = stored.EntranceId,
            FullName = stored.FullName,
            Email = stored.Email,
            CompanyName = stored.CompanyName,
            TeamId = stored.TeamId,
            AcceptedPolicies = stored.AcceptedPolicies,
            CheckInDate = stored.TimestampUtc
        };
    }
    }
}