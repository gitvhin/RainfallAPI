namespace RainfallAPI.Application.Response
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public List<ErrorDetail> Detail { get; set; }
    }
}
