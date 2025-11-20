using System;
using System.Linq;
using System.Threading.Tasks;
using BuildingEntryRegistration.Api.Services;
using BuildingEntryRegistration.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace BuildingEntryRegistration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckInsController : ControllerBase
    {
        private readonly ICheckInService _checkInService;

        public CheckInsController(ICheckInService checkInService)
        {
            _checkInService = checkInService;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCheckInRequest request)
        {
            try
            {
                var checkIn = await _checkInService.CreateCheckInAsync(
                    request.EntranceId,
                    request.FullName,
                    request.Email,
                    request.CompanyName,
                    request.TeamId,
                    request.AcceptedPolicies);

                var teams = await _checkInService.GetTeamsAsync();
                var team = teams.FirstOrDefault(t => t.Id == checkIn.TeamId);

                var response = new
                {
                    checkIn.Id,
                    checkIn.FullName,
                    checkIn.Email,
                    checkIn.CompanyName,
                    checkIn.TeamId,
                    TeamName = team?.Name,
                    checkIn.EntranceId,
                    checkIn.CheckInDate
                };

                return StatusCode(200, response);
            }
            catch (InvalidOperationException ex)
            {
                // Business rule hataları için 400
                return StatusCode(400, new { error = ex.Message });
            }
            catch (Exception)
            {
                //500 Internal Server Error
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }
    }
}