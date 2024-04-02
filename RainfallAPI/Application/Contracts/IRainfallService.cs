using RainfallAPI.Application.Response;


namespace RainfallAPI.Application.Contracts
{
    public interface IRainfallService
    {
        Task<RainfallReadingResponse> GetRainfallReadingsAsync(string stationId, int count = 10);
    }
}
