using Microsoft.AspNetCore.Mvc;
using RainfallAPI.Application.Contracts;

namespace RainfallAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RainfallController : ControllerBase
    {
        private readonly IRainfallService _rainfallService;

        public RainfallController(IRainfallService rainfallService)
        {
            _rainfallService = rainfallService ?? throw new ArgumentNullException(nameof(rainfallService));
        }

        [HttpGet("id/{stationId}/readings")]
        public async Task<IActionResult> GetRainfallReadings(string stationId, int count = 10)
        {
            try
            {
                var rainfallResponse = await _rainfallService.GetRainfallReadingsAsync(stationId, count);
                return Ok(rainfallResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}
