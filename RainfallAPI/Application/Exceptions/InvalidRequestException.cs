// Represents an exception thrown when a request is invalid
using RainfallAPI.Application.Response;

namespace RainfallAPI.Application.Exceptions
{
    public class InvalidRequestException : Exception
    {
        public string PropertyName { get; }

        // Constructor with property name and error message
        public InvalidRequestException(string propertyName, string message) : base(message)
        {
            PropertyName = propertyName;
        }

        // Creates an error response for this exception
        public ErrorResponse CreateErrorResponse()
        {
            return new ErrorResponse
            {
                Message = "Invalid request",
                Detail = new List<ErrorDetail> { new ErrorDetail { PropertyName = PropertyName, Message = Message } }
            };
        }
    }
}
