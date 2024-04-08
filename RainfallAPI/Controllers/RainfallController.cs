using Microsoft.AspNetCore.Mvc;
using RainfallAPI.Application.Contracts;
using RainfallAPI.Application.Models;
using RainfallAPI.Constants;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace RainfallAPI.Controllers
{
    /// <summary>
    /// Responsible for handling HTTP requests related to rainfall readings.
    /// </summary>
    [ApiController]
    [Produces("application/json")]
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
        /// <param name="count">The number of readings to retrieve.</param>
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
        public async Task<IActionResult> GetRainfallReadings(string stationId, [Range(1, 100)] int count = 10)
        {
            try
            {
                var rainfallResponse = await _rainfallService.GetRainfallReadingsAsync(stationId, count);

                return Ok(rainfallResponse);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(CreateErrorResponse("Invalid request", "stationId", ex.Message));
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(CreateErrorResponse("Not found", "stationId", ex.Message));
            }
            catch (Exception ex)
            {
                // Any any other exceptions and return 500 Internal Server Error
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
