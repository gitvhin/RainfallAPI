// Represents the response for rainfall readings
using RainfallAPI.Domain;

namespace RainfallAPI.Application.Response
{
    public class RainfallReadingResponse
    {
        // List of rainfall readings
        public List<RainfallReading> Readings { get; set; }
    }
}


