// Responsible for defining the contract for interacting with an external API
using RainfallAPI.Application.Response;

namespace RainfallAPI.Application.Contracts
{
    public interface IExternalAPIService
    {
        // Retrieves rainfall readings from an external API asynchronously
        Task<ExternalAPIResponse> GetRainfallReadingsFromExternalApiAsync(string stationId, int count);
    }
}
