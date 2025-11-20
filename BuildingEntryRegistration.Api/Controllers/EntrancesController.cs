using System.Threading.Tasks;
using BuildingEntryRegistration.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildingEntryRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntrancesController : ControllerBase
    {
        private readonly ICheckInService _checkInService;

        public EntrancesController(ICheckInService checkInService)
        {
            _checkInService = checkInService;
        }

        public class EntranceValidationRequest
        {
            public string EntranceId { get; set; } = default!;
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateEntrance([FromBody] EntranceValidationRequest request)
        {
            bool isValid = await _checkInService.IsEntranceValidAsync(request.EntranceId);

            return Ok(new
            {
                entranceId = request.EntranceId,
                isValid = isValid
            });
        }
    }
}