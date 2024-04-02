// Responsible for making HTTP requests to an external API
using RainfallAPI.Application.Contracts;
using RainfallAPI.Application.Response;
using Newtonsoft.Json;

namespace RainfallAPI.Infrastracture.ExternalAPI
{
    public class RestClient : IExternalAPIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public RestClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration;
        }

        // Retrieves rainfall readings from an external API asynchronously
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
