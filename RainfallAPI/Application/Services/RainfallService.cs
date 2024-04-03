using AutoMapper;
using RainfallAPI.Application.Contracts;
using RainfallAPI.Application.Exceptions;
using RainfallAPI.Application.Response;
using RainfallAPI.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RainfallAPI.Application.Services
{
    /// <summary>
    /// Implements the core business logic of the application related to rainfall.
    /// </summary>
    public class RainfallService : IRainfallService
    {
        private readonly IExternalAPIService _externalApiService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="RainfallService"/> class.
        /// </summary>
        /// <param name="externalApiService">The service responsible for interacting with the external API.</param>
        /// <param name="mapper">The AutoMapper instance for mapping objects.</param>
        public RainfallService(IExternalAPIService externalApiService, IMapper mapper)
        {
            _externalApiService = externalApiService ?? throw new ArgumentNullException(nameof(externalApiService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Retrieves rainfall readings asynchronously.
        /// </summary>
        /// <param name="stationId">The ID of the rainfall station.</param>
        /// <param name="count">The number of readings to retrieve.</param>
        /// <returns>A task representing the asynchronous operation that returns the rainfall reading response.</returns>
        /// <exception cref="InvalidRequestException">Thrown when the request is invalid.</exception>
        /// <exception cref="NotFoundException">Thrown when the requested resource is not found.</exception>
        public async Task<RainfallReadingResponse> GetRainfallReadingsAsync(string stationId, int count = 10)
        {
            ValidateRequest(stationId, count);

            // Retrieve rainfall readings from external API
            var externalApiResponse = await _externalApiService.GetRainfallReadingsFromExternalApiAsync(stationId, count);

            // Check if the response contains data
            if (externalApiResponse.Items == null || externalApiResponse.Items.Count == 0)
                throw new NotFoundException("stationId", Constants.ErrorMessages.NotFound);

            // Implement mapping logic from external API response to RainfallReading entities
            var rainfallReadings = _mapper.Map<List<Item>, List<RainfallReading>>(externalApiResponse.Items);

            return new RainfallReadingResponse { Readings = rainfallReadings };
        }

        // Validates the request parameters
        private void ValidateRequest(string stationId, int count)
        {
            if (string.IsNullOrEmpty(stationId))
                throw new InvalidRequestException("stationId", Constants.ErrorMessages.InvalidRequest);

            if (count <= 0 || count > 100)
                throw new InvalidRequestException("count", Constants.ErrorMessages.InvalidRequest);
        }
    }
}
