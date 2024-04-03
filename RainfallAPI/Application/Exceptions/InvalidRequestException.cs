using RainfallAPI.Application.Response;
using System;

namespace RainfallAPI.Application.Exceptions
{
    /// <summary>
    /// Represents an exception thrown when a request is invalid.
    /// </summary>
    public class InvalidRequestException : Exception
    {
        /// <summary>
        /// Gets the name of the property causing the error.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRequestException"/> class with the specified property name and error message.
        /// </summary>
        /// <param name="propertyName">The name of the property causing the error.</param>
        /// <param name="message">The error message.</param>
        public InvalidRequestException(string propertyName, string message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}
