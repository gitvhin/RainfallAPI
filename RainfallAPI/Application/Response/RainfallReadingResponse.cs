namespace RainfallAPI.Application.Response
{
    public class RainfallReadingResponse
    {
        public List<RainfallReading> Readings { get; set; }
    }
}

public class RainfallReading
{
    public DateTime DateMeasured { get; set; }
    public decimal AmountMeasured { get; set; }
}