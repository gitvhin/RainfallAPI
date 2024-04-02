﻿// Responsible for handling HTTP requests related to rainfall readings
using Microsoft.AspNetCore.Mvc;
using RainfallAPI.Application.Contracts;
using RainfallAPI.Application.Response;
using RainfallAPI.Application.Exceptions;
using RainfallAPI.Constants;

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

        // Retrieves rainfall readings for a specific station
        [HttpGet("id/{stationId}/readings")]
        public async Task<IActionResult> GetRainfallReadings(string stationId, int count = 10)
        {
            try
            {
                var rainfallResponse = await _rainfallService.GetRainfallReadingsAsync(stationId, count);

                return Ok(rainfallResponse);
            }
            catch (InvalidRequestException ex)
            {
                return BadRequest(ex.CreateErrorResponse());
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.CreateErrorResponse());
            }
            catch (Exception ex)
            {
                return StatusCode(500, CreateError500("server", ErrorMessages.InternalServerError, ex.Message));
            }
        }

        // Creates a custom 500 error response
        private ErrorResponse CreateError500(string propertyName, string message, string errorMessage)
        {
            return new ErrorResponse
            {
                Message = message,
                Detail = new List<ErrorDetail> { new ErrorDetail { PropertyName = propertyName, Message = errorMessage } }
            };
        }
    }
}
