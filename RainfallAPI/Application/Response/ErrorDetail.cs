namespace RainfallAPI.Application.Response
{
    /// <summary>
    /// Represents details of an error.
    /// </summary>
    public class ErrorDetail
    {
        /// <summary>
        /// Gets or sets the name of the property causing the error.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; }
    }
}
