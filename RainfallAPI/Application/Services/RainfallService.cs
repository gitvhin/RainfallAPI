using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RainfallAPI.Application.Contracts;
using RainfallAPI.Application.Models;
using RainfallAPI.Constants;
using RainfallAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RainfallAPI.Application.Services
{
    /// <summary>
    /// Implements the core business logic of the application related to rainfall.
    /// </summary>
    public class RainfallService : IRainfallService
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<RainfallService> _logger;


        /// <summary>
        /// Initializes a new instance of the <see cref="RainfallService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client instance.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="mapper">The AutoMapper instance for mapping objects.</param>
        public RainfallService(IHttpClientWrapper httpClient, IConfiguration configuration, IMapper mapper, ILogger<RainfallService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }

        /// <summary>
        /// Retrieves rainfall readings asynchronously.
        /// </summary>
        /// <param name="stationId">The ID of the rainfall station.</param>
        /// <param name="count">The number of readings to retrieve.</param>
        /// <returns>A task representing the asynchronous operation that returns the rainfall reading response.</returns>
        /// <exception cref="ArgumentException">Thrown when the request is invalid.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the requested resource is not found or API base URL is not configured.</exception>
        public async Task<RainfallReadingResponse> GetRainfallReadingsAsync(string stationId, int count = 10)
        {
            ValidateRequest(stationId, count);

            var apiBaseUrl = _configuration.GetValue<string>("ApiSettings:BaseUrl");

            if (string.IsNullOrEmpty(apiBaseUrl))
            {
                _logger.LogError("API base URL is not configured."); // Log error message
                throw new InvalidOperationException("API base URL is not configured.");
            }

            var apiUrl = $"{apiBaseUrl}/flood-monitoring/id/stations/{stationId}/readings?_limit={count}";

            var response = await _httpClient.GetAsync(apiUrl);

            // Check if response is successful
            response.EnsureSuccessStatusCode();

            // Read response body
            var responseBody = await response.Content.ReadAsStringAsync();

            // Deserialize JSON response into ExternalAPIResponse object
            var externalAPIResponse = JsonConvert.DeserializeObject<ExternalAPIResponse>(responseBody);

            if (externalAPIResponse.Items == null || externalAPIResponse.Items.Count == 0)
            {
                _logger.LogWarning("No readings found for the specified stationId: {StationId}", stationId); 
                throw new InvalidOperationException(ErrorMessages.NotFound);
            }

            // Implement mapping logic from external API response to RainfallReading entities
            var rainfallReadings = _mapper.Map<List<Item>, List<RainfallReading>>(externalAPIResponse.Items);

            _logger.LogInformation("Rainfall readings retrieved successfully for stationId: {StationId}", stationId);

            return new RainfallReadingResponse { Readings = rainfallReadings };
        }

        // Validates the request parameters
        private void ValidateRequest(string stationId, int count)
        {
            if (string.IsNullOrWhiteSpace(stationId))
            {
                _logger.LogError("Invalid stationId: {StationId}", stationId); // Log error message
                throw new ArgumentException("StationId cannot be null or empty.", nameof(stationId));
            }

            if (count <= 0 || count > 100)
            {
                _logger.LogError("Invalid count: {Count}", count); // Log error message
                throw new ArgumentException("Count must be between 1 and 100.", nameof(count));
            }
        }
    }
}