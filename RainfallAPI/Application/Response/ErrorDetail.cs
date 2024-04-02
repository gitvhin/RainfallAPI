// Represents details of an error
namespace RainfallAPI.Application.Response
{
    public class ErrorDetail
    {
        // Name of the property causing the error
        public string PropertyName { get; set; }
        // Error message
        public string Message { get; set; }
    }
}
