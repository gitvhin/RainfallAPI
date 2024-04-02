using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RainfallAPI.Application.Contracts;
using RainfallAPI.Application.Response;

namespace RainfallAPI.Application.Services
{
    public class RainfallService : IRainfallService
    {
        private readonly IHttpClientFactory _clientFactory;

        public RainfallService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<RainfallReadingResponse> GetRainfallReadingsAsync(string stationId, int count = 10)
        {
            // Retrieve base URL from configuration
            var apiBaseUrl = "https://environment.data.gov.uk";

            // Create HTTP client
            var client = _clientFactory.CreateClient();

            // Construct API endpoint URL
            var apiUrl = $"{apiBaseUrl}/flood-monitoring/id/stations/{stationId}/readings?_limit={count}";

            // Send GET request to API endpoint
            var response = await client.GetAsync(apiUrl);
            return null;
        }
    }
}
