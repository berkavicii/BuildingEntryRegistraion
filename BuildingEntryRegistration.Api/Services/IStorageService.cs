using System.Collections.Generic;
using BuildingEntryRegistration.Api.Models;

namespace BuildingEntryRegistration.Api.Services
{
    public interface IStorageService
    {
        IEnumerable<Team> GetTeams();
        IEnumerable<Entrance> GetEntrances();
        Team? GetTeam(int id);
        IEnumerable<CheckIn> GetCheckIns();
        CheckIn AddCheckIn(CheckIn checkIn);
        void SeedIfNeeded();
    }
}
