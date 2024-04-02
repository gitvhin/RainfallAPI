using RainfallAPI.Application.Contracts;
using Newtonsoft.Json;

namespace RainfallAPI.Infrastracture.ExternalAPI
{
    public class RestClient : IExternalAPIService
    {
        private readonly HttpClient _httpClient;

        public RestClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<ExternalAPIResponse> GetRainfallReadingsFromExternalApiAsync(string stationId, int count)
        {
            // Make HTTP request to external API
            var apiBaseUrl = "https://environment.data.gov.uk";
            var apiUrl = $"{apiBaseUrl}/flood-monitoring/id/stations/{stationId}/readings?_limit={count}";
            var response = await _httpClient.GetAsync(apiUrl);

            // Read response body
            var responseBody = await response.Content.ReadAsStringAsync();

            // Deserialize JSON response into RainfallReadingResponse object
            var externalAPIResponse = JsonConvert.DeserializeObject<ExternalAPIResponse>(responseBody);

            return externalAPIResponse;
        }
    }
}
