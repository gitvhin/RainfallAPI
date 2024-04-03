using RainfallAPI.Domain;
using System.Collections.Generic;

namespace RainfallAPI.Application.Response
{
    /// <summary>
    /// Get rainfall readings response
    /// </summary>
    public class RainfallReadingResponse
    {
        /// <summary>
        /// Gets or sets the list of rainfall readings.
        /// </summary>
        public List<RainfallReading> Readings { get; set; }

    }
}
