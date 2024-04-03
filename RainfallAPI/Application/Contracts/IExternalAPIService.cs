using RainfallAPI.Application.Response;
using System.Threading.Tasks;

namespace RainfallAPI.Application.Contracts
{
    /// <summary>
    /// Responsible for defining the contract for interacting with an external API.
    /// </summary>
    public interface IExternalAPIService
    {
        /// <summary>
        /// Retrieves rainfall readings from an external API asynchronously.
        /// </summary>
        /// <param name="stationId">The ID of the reading station.</param>
        /// <param name="count">The number of readings to retrieve.</param>
        /// <returns>An asynchronous task that returns an ExternalAPIResponse containing the rainfall readings.</returns>
        Task<ExternalAPIResponse> GetRainfallReadingsFromExternalApiAsync(string stationId, int count);
    }
}
