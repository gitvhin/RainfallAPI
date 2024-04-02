// Represents an exception thrown when a requested resource is not found
using RainfallAPI.Application.Response;

namespace RainfallAPI.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public string PropertyName { get; }

        // Constructor with property name and error message
        public NotFoundException(string propertyName, string message) : base(message)
        {
            PropertyName = propertyName;
        }

        // Creates an error response for this exception
        public ErrorResponse CreateErrorResponse()
        {
            return new ErrorResponse
            {
                Message = "Not found",
                Detail = new List<ErrorDetail> { new ErrorDetail { PropertyName = PropertyName, Message = Message } }
            };
        }
    }
}
