using System.Collections.Generic;

namespace RainfallAPI.Application.Response
{
    /// <summary>
    /// An error object returned for failed requests
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the details of the error.
        /// </summary>
        public List<ErrorDetail> Detail { get; set; }
    }
}
