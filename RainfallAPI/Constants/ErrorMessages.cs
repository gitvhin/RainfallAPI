namespace RainfallAPI.Constants
{
    /// <summary>
    /// Contains constant strings for error messages used throughout the application.
    /// </summary>
    public static class ErrorMessages
    {
        /// <summary>
        /// Error message for an invalid request.
        /// </summary>
        public const string InvalidRequest = "Invalid request";

        /// <summary>
        /// Error message for not found.
        /// </summary>
        public const string NotFound = "No readings found for the specified stationId";

        /// <summary>
        /// Error message for internal server error.
        /// </summary>
        public const string InternalServerError = "Internal server error";
    }
}
