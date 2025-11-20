using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using BuildingEntryRegistration.Api.Models;

namespace BuildingEntryRegistration.Api.Services
{
    public class InMemoryStorageService : IStorageService
    {
        private readonly List<Team> _teams = new();
        private readonly List<Entrance> _entrances = new();
        private readonly ConcurrentBag<CheckIn> _checkIns = new();

        public InMemoryStorageService()
        {
            SeedIfNeeded();
        }

        public void SeedIfNeeded()
        {
            if (_teams.Any()) return;
            _teams.AddRange(new[]
            {
                new Team { Id = 1, Name = "Engineering" },
                new Team { Id = 2, Name = "Sales" },
                new Team { Id = 3, Name = "HR" },
                new Team { Id = 4, Name = "Design" },
                new Team { Id = 5, Name = "Security" }
            });

            _entrances.Add(new Entrance { Id = "ENTRANCE-LOBBY-1" });
        }

        public IEnumerable<Team> GetTeams() => _teams;

        public Team? GetTeam(int id) => _teams.FirstOrDefault(t => t.Id == id);

        public IEnumerable<CheckIn> GetCheckIns() => _checkIns.OrderByDescending(c => c.TimestampUtc);

        public IEnumerable<Entrance> GetEntrances() => _entrances;

        public CheckIn AddCheckIn(CheckIn checkIn)
        {
            checkIn.TimestampUtc = DateTimeOffset.UtcNow;
            _checkIns.Add(checkIn);
            return checkIn;
        }
    }
}
