// Represents the response for rainfall readings
namespace RainfallAPI.Application.Response
{
    public class RainfallReadingResponse
    {
        // List of rainfall readings
        public List<RainfallReading> Readings { get; set; }
    }
}


public class RainfallReading
{
    public DateTime DateMeasured { get; set; }
    public decimal AmountMeasured { get; set; }
}