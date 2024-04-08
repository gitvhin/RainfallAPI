using AutoMapper;
using RainfallAPI.Application.Contracts;
using RainfallAPI.Application.Models;
using RainfallAPI.Constants;
using RainfallAPI.Domain.Entities;
using Newtonsoft.Json;
using System.Net;

namespace RainfallAPI.Application.Services
{
    /// <summary>
    /// Implements the core business logic of the application related to rainfall.
    /// </summary>
    public class RainfallService : IRainfallService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly ILogger<RainfallService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RainfallService"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        /// <param name="mapper">The AutoMapper instance for mapping objects.</param>
        /// <param name="logger">The logger instance.</param>
        public RainfallService(IHttpClientFactory httpClientFactory, IMapper mapper, ILogger<RainfallService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("RainfallAPI");
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<RainfallReadingResponse> GetRainfallReadingsAsync(string stationId, int count = 10)
        {
            try
            {
                ValidateRequest(stationId, count);

                var apiUrl = $"/flood-monitoring/id/stations/{stationId}/readings?_limit={count}";

                var response = await _httpClient.GetAsync(apiUrl);

                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                var externalAPIResponse = JsonConvert.DeserializeObject<ExternalAPIResponse>(responseBody);

                if (externalAPIResponse.Items == null || externalAPIResponse.Items.Count == 0)
                {
                    _logger.LogWarning("No readings found for the specified stationId: {StationId}", stationId);
                    throw new HttpRequestException(ErrorMessages.NotFound, null, HttpStatusCode.NotFound);
                }

                var rainfallReadings = _mapper.Map<List<Item>, List<RainfallReading>>(externalAPIResponse.Items);

                _logger.LogInformation("Rainfall readings retrieved successfully for stationId: {StationId}", stationId);

                return new RainfallReadingResponse { Readings = rainfallReadings };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.InternalServerError, stationId);
                throw;
            }
        }

        private void ValidateRequest(string stationId, int count)
        {
            if (string.IsNullOrWhiteSpace(stationId))
            {
                _logger.LogError("Invalid stationId: {StationId}", stationId);
                throw new HttpRequestException(ErrorMessages.InvalidRequest, null, HttpStatusCode.BadRequest);
            }

            if (count <= 0 || count > 100)
            {
                _logger.LogError("Invalid count: {Count}", count);
                throw new HttpRequestException(ErrorMessages.InvalidRequest, null, HttpStatusCode.BadRequest);
            }
        }
    }
}
