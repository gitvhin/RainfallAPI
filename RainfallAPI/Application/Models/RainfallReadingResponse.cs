using RainfallAPI.Domain.Entities;
using System.Collections.Generic;

namespace RainfallAPI.Application.Models
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
