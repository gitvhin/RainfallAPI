// Responsible for defining the contract for the core business logic of the application
using RainfallAPI.Application.Response;

namespace RainfallAPI.Application.Contracts
{
    public interface IRainfallService
    {
        // Retrieves rainfall readings asynchronously
        Task<RainfallReadingResponse> GetRainfallReadingsAsync(string stationId, int count = 10);
    }
}
