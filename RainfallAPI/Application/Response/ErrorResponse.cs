// Represents a generic error response
namespace RainfallAPI.Application.Response
{
    public class ErrorResponse
    {
        // Error message
        public string Message { get; set; }
        // Details of the error
        public List<ErrorDetail> Detail { get; set; }
    }
}
