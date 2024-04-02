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
    }
}
