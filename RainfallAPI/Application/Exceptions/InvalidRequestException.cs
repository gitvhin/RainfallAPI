using RainfallAPI.Application.Response;

namespace RainfallAPI.Application.Exceptions
{
    public class InvalidRequestException : Exception
    {
        public string PropertyName { get; } 

        public InvalidRequestException(string propertyName, string message) : base(message)
        {
            PropertyName = propertyName;
        }

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
