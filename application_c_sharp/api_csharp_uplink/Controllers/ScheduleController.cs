using Microsoft.AspNetCore.Mvc;
using api_csharp_uplink.Composant;
using api_csharp_uplink.Dto;
using api_csharp_uplink.DirException;
using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController(IScheduleService scheduleService) : ControllerBase
    {
        private readonly IScheduleService _scheduleService = scheduleService;
        
        /// <summary>
        /// Get all the schedule of a bus station for the line 5
        /// </summary>
        /// <param name="stationName">The name of the bus station</param>
        /// <returns>A response with all the schedule of the day or NotFound response</returns>
        [HttpGet("stationName/{stationName}")]
        [ProducesResponseType(typeof(Schedule), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetAllScheduleOfStation(string stationName)
        {
            try
            {
                return Ok(_scheduleService.StationSchedules(stationName));
            }
            catch (Exception e)
            {
                return ErrorManager.HandleError(e);
            }
        }

    }
}