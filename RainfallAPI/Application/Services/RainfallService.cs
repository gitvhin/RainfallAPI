// Implements the core business logic of the application
using AutoMapper;
using RainfallAPI.Application.Contracts;
using RainfallAPI.Application.Exceptions;
using RainfallAPI.Application.Response;

namespace RainfallAPI.Application.Services
{
    public class RainfallService : IRainfallService
    {
        private readonly IExternalAPIService _externalApiService;
        private readonly IMapper _mapper;

        public RainfallService(IExternalAPIService externalApiService, IMapper mapper)
        {
            _externalApiService = externalApiService ?? throw new ArgumentNullException(nameof(externalApiService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // Retrieves rainfall readings asynchronously
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
