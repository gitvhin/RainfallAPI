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
            // Retrieve rainfall readings from external API
            var externalApiResponse = await _externalApiService.GetRainfallReadingsFromExternalApiAsync(stationId, count);

            // Implement mapping logic from external API response to RainfallReading entities
            var rainfallReadings = _mapper.Map<List<Item>, List<RainfallReading>>(externalApiResponse.Items);
            
            if (rainfallReadings is null || rainfallReadings.Count == 0)
                throw new NotFoundException("stationId", Constants.ErrorMessages.NotFound);
            else if (externalApiResponse is null)
                throw new InvalidRequestException("stationId", Constants.ErrorMessages.InvalidRequest);

            return new RainfallReadingResponse { Readings = rainfallReadings };
        }
    }
}
