namespace RainfallAPI.Domain.Entities
{
    /// <summary>
    ///  Details of a rainfall reading
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