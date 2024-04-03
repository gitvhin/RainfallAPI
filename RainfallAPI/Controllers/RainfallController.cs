using Microsoft.AspNetCore.Mvc;
using RainfallAPI.Application.Contracts;
using RainfallAPI.Application.Response;
using RainfallAPI.Application.Exceptions;
using RainfallAPI.Constants;

namespace RainfallAPI.Controllers
{
    /// <summary>
    /// Responsible for handling HTTP requests related to rainfall readings.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RainfallController : ControllerBase
    {
        private readonly IRainfallService _rainfallService;

        public RainfallController(IRainfallService rainfallService)
        {
            _rainfallService = rainfallService ?? throw new ArgumentNullException(nameof(rainfallService));
        }

        /// <summary>
        /// Retrieves rainfall readings for a specific station.
        /// </summary>
        /// <param name="stationId">The ID of the station.</param>
        /// <param name="count">The number of readings to retrieve.</param>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        [HttpGet("id/{stationId}/readings")]
        public async Task<IActionResult> GetRainfallReadings(string stationId, int count = 1)
        {
            try
            {
                var rainfallResponse = await _rainfallService.GetRainfallReadingsAsync(stationId, count);

                return Ok(rainfallResponse);
            }
            catch (InvalidRequestException ex)
            {
                return BadRequest(CreateErrorResponse("Invalid request", ex.PropertyName, ex.Message));
            }
            catch (NotFoundException ex)
            {
                return NotFound(CreateErrorResponse("Not found", ex.PropertyName, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, CreateErrorResponse("Server error", "server", ex.Message));
            }
        }

        // Creates a custom error response
        private ErrorResponse CreateErrorResponse(string message, string propertyName, string errorMessage)
        {
            return new ErrorResponse
            {
                Message = message,
                Detail = new List<ErrorDetail> { new ErrorDetail { PropertyName = propertyName, Message = errorMessage } }
            };
        }
    }
}
