using RainfallAPI.Application.Response;

namespace RainfallAPI.Application.Contracts
{
    public interface IExternalAPIService
    {
        Task<ExternalAPIResponse> GetRainfallReadingsFromExternalApiAsync(string stationId, int count);
    }
}
