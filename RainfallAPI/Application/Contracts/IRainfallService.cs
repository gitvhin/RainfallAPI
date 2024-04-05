using RainfallAPI.Application.Models;
using System.Threading.Tasks;

namespace RainfallAPI.Application.Contracts
{
    /// <summary>
    /// Responsible for defining the contract for the core business logic of the application.
    /// </summary>
    public interface IRainfallService
    {
        /// <summary>
        /// Retrieves rainfall readings asynchronously.
        /// </summary>
        /// <param name="stationId">The ID of the reading station.</param>
        /// <param name="count">The number of readings to retrieve. Default is 10.</param>
        /// <returns>An asynchronous task that returns a RainfallReadingResponse containing the rainfall readings.</returns>
        Task<RainfallReadingResponse> GetRainfallReadingsAsync(string stationId, int count = 10);
    }
}
