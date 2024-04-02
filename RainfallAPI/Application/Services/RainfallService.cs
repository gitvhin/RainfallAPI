using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using RainfallAPI.Application.Contracts;
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

        public async Task<RainfallReadingResponse> GetRainfallReadingsAsync(string stationId, int count = 10)
        {
            var externalApiResponse = await _externalApiService.GetRainfallReadingsFromExternalApiAsync(stationId, count);

            //Implement mapping logic from external API response to RainfallReading entities
            var rainfallReadings = _mapper.Map<List<Item>, List<RainfallReading>>(externalApiResponse.Items);
            return new RainfallReadingResponse { Readings = rainfallReadings };
        }
    }
}
