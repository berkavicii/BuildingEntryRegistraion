using BuildingEntryRegistration.Api.Models;
using BuildingEntryRegistration.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuildingEntryRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ICheckInService _checkInService;

        public TeamsController(ICheckInService checkInService)
        {
            _checkInService = checkInService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
        {
            var teams = await _checkInService.GetTeamsAsync();
            return Ok(teams);
        }
        
    }
}