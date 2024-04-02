using RainfallAPI.Application.Response;

namespace RainfallAPI.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public string PropertyName { get; } 

        public NotFoundException(string propertyName, string message) : base(message)
        {
            PropertyName = propertyName;
        }

        public ErrorResponse CreateErrorResponse()
        {
            return new ErrorResponse
            {
                Message = "Not found",
                Detail = new List<ErrorDetail> { new ErrorDetail {  PropertyName = PropertyName, Message = Message } }
            };
        }
    }
}
