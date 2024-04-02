using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RainfallAPI.Application.Contracts;
using RainfallAPI.Application.Response;

namespace RainfallAPI.Application.Services
{
    public class RainfallService : IRainfallService
    {
        private readonly IExternalAPIService _externalApiService;

        public RainfallService(IExternalAPIService externalApiService)
        {
            _externalApiService = externalApiService ?? throw new ArgumentNullException(nameof(externalApiService));
        }

        public async Task<RainfallReadingResponse> GetRainfallReadingsAsync(string stationId, int count = 10)
        {
            var externalApiResponse = await _externalApiService.GetRainfallReadingsFromExternalApiAsync(stationId, count);
            
            var rainfallReadings = MapToRainfallReadings(externalApiResponse);
            return new RainfallReadingResponse { Readings = rainfallReadings };
        }

        private List<RainfallReading> MapToRainfallReadings(ExternalAPIResponse externalApiResponse)
        {
            //TODO ARVIN: Implement mapping logic from external API response to RainfallReading entities
            return new List<RainfallReading>();
        }
    }
}
