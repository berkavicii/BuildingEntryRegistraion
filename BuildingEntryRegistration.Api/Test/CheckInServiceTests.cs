using System;
using System.Linq;
using System.Threading.Tasks;
using BuildingEntryRegistration.Api.Services;
using Shouldly;
using Xunit;

namespace BuildingEntryRegistration.Api.Tests
{
    public class CheckInServiceTests
    {
        private readonly InMemoryStorageService store = new InMemoryStorageService();
        private readonly CheckInService service;
        public CheckInServiceTests(){
            store = new InMemoryStorageService();
            service = new CheckInService(store);
        }

        [Fact]
        public async Task CreateCheckInAsync_EmptyEntranceId_ThrowsInvalidOperationException()
        {


            await Should.ThrowAsync<InvalidOperationException>(async () =>
                await service.CreateCheckInAsync(
                    string.Empty,
                    "Berk Avcı",
                    "berkavci8@gmail.com",
                    "Nesine.com",
                    1,
                    true));
        }

        [Fact]
        public async Task CreateCheckInAsync_TeamId1_ShouldBeEngineering()
        {

            var result = await service.CreateCheckInAsync(
                "ENTRANCE-LOBBY-1",
                "Berk Avcı",
                "berkavci8@gmail.com",
                "Nesine.com",
                1, 
                true);

            var team = store.GetTeams().FirstOrDefault(t => t.Id == result.TeamId);
            team.ShouldNotBeNull();
            team.Name.ShouldBe("Engineering");
        }
    }
}
