using System;
using System.Threading.Tasks;
using BuildingEntryRegistration.Api.Services;
using Xunit;

namespace BuildingEntryRegistration.Api.Tests
{
    public class CheckInServiceTests
    {
        [Fact]
        public async Task CreateCheckInAsync_EmptyEntranceId_ThrowsInvalidOperationException()
        {
            // Arrange
            var store = new InMemoryStorageService();
            var service = new CheckInService(store);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => service.CreateCheckInAsync(
                    string.Empty,
                    "John Doe",
                    "john@example.com",
                    "Acme",
                    1,
                    true));
        }
    }
}
