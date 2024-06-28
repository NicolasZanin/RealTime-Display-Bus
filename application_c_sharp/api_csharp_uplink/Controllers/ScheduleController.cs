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
        /// Get all the schedule of the ride CHUNG CƯ HÒA HIỆP NAM – CÔNG VIÊN BIỂN ĐÔNG of a bus station for the line 5
        /// </summary>
        /// <param name="stationName">The name of the bus station</param>
        /// <returns>A response with all the schedule of the day or NotFound response</returns>
        [HttpGet("aller/stationName/{stationName}")]
        [ProducesResponseType(typeof(Schedule), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetAllScheduleOfStationAller(string stationName)
        {
            try
            {
                return Ok(_scheduleService.StationSchedulesAller(stationName));
            }
            catch (Exception e)
            {
                return ErrorManager.HandleError(e);
            }
        }
        /// <summary>
        /// Get all the schedule of the ride CÔNG VIÊN BIỂN ĐÔNG – CHUNG CƯ HÒA HIỆP NAM of a bus station for the line 5
        /// </summary>
        /// <param name="stationName">The name of the bus station</param>
        /// <returns>A response with all the schedule of the day or NotFound response</returns>
        [HttpGet("retour/stationName/{stationName}")]
        [ProducesResponseType(typeof(Schedule), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetAllScheduleOfStationRetour(string stationName)
        {
            try
            {
                return Ok(_scheduleService.StationSchedulesRetour(stationName));
            }
            catch (Exception e)
            {
                return ErrorManager.HandleError(e);
            }
        }
            
        /// <summary>
        /// Get all the schedule of the ride CHUNG CƯ HÒA HIỆP NAM – CÔNG VIÊN BIỂN ĐÔNG of all bus station for the line 5
        /// </summary>
        /// <returns>A response with all the schedule of the day or NotFound response</returns>
        [HttpGet("aller")]
        [ProducesResponseType(typeof(List<Schedule>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetAllSchedulesAller()
        {
            try
            {
                return Ok(_scheduleService.SchedulesAller());
            }
            catch (Exception e)
            {
                return ErrorManager.HandleError(e);
            }
        }
        /// <summary>
        /// Get all the schedule of the ride CÔNG VIÊN BIỂN ĐÔNG – CHUNG CƯ HÒA HIỆP NAM of all bus station for the line 5
        /// </summary>
        /// <returns>A response with all the schedule of the day or NotFound response</returns>
        [HttpGet("retour")]
        [ProducesResponseType(typeof(List<Schedule>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetAllSchedulesRetour()
        {
            try
            {
                return Ok(_scheduleService.SchedulesRetour());
            }
            catch (Exception e)
            {
                return ErrorManager.HandleError(e);
            }
        }

    }
}