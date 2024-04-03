using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RainfallAPI.Application.Contracts;
using RainfallAPI.Application.Response;
using Newtonsoft.Json;

namespace RainfallAPI.Infrastracture.ExternalAPI
{
    /// <summary>
    /// Responsible for making HTTP requests to an external API.
    /// </summary>
    public class RestClient : IExternalAPIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client instance.</param>
        /// <param name="configuration">The configuration.</param>
        public RestClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Retrieves rainfall readings from an external API asynchronously.
        /// </summary>
        /// <param name="stationId">The ID of the weather station.</param>
        /// <param name="count">The number of readings to retrieve.</param>
        /// <returns>An asynchronous task that represents the operation and holds the response.</returns>
        public async Task<ExternalAPIResponse> GetRainfallReadingsFromExternalApiAsync(string stationId, int count)
        {
            // Make HTTP request to external API
            var apiBaseUrl = _configuration.GetValue<string>("ApiSettings:BaseUrl");
            var apiUrl = $"{apiBaseUrl}/flood-monitoring/id/stations/{stationId}/readings?_limit={count}";
            var response = await _httpClient.GetAsync(apiUrl);

            // Read response body
            var responseBody = await response.Content.ReadAsStringAsync();

            // Deserialize JSON response into ExternalAPIResponse object
            var externalAPIResponse = JsonConvert.DeserializeObject<ExternalAPIResponse>(responseBody);

            return externalAPIResponse;
        }
    }
}
