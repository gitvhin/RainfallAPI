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
    public class RainfallController : ControllerBase
    {
        private readonly IRainfallService _rainfallService;

        public RainfallController(IRainfallService rainfallService)
        {
            _rainfallService = rainfallService ?? throw new ArgumentNullException(nameof(rainfallService));
        }

        /// <summary>
        /// Get rainfall readings by station Id.
        /// </summary>
        /// <remarks>
        /// Retrieve the latest readings for the specified stationId
        /// </remarks>
        /// <param name="stationId">The ID of the reading station.</param>
        /// <param name="count">The number of readings to retrieve. <br> Minimum: 1 </br>Maximum: 100</param>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        /// <response code="200">A list of rainfall readings successfully retrieved.</response>
        /// <response code="400">Invalid request.</response>
        /// <response code="404">No readings found for the specified stationId.</response>
        /// <response code="500">Internal server error.</response>
        [ProducesResponseType(typeof(RainfallReadingResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [HttpGet("rainfall/id/{stationId}/readings")]
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
