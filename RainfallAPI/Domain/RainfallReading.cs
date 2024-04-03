namespace RainfallAPI.Domain
{
    /// <summary>
    /// Represents rainfall readings containing information about the date and time
    /// when the measurement was taken and the amount of rainfall measured.
    /// </summary>
    public class RainfallReading
    {
        /// <summary>
        /// Gets or sets the date and time when the rainfall measurement was taken.
        /// </summary>
        public DateTime DateMeasured { get; set; }

        /// <summary>
        /// Gets or sets the amount of rainfall measured.
        /// </summary>
        public decimal AmountMeasured { get; set; }
    }
}
