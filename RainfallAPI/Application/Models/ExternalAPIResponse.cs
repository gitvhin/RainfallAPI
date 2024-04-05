using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RainfallAPI.Application.Models
{
    /// <summary>
    /// Represents the response from an external API.
    /// </summary>
    public class ExternalAPIResponse
    {
        /// <summary>
        /// Gets or sets the context of the response.
        /// </summary>
        [JsonProperty("@context")]
        public string Context { get; set; }

        /// <summary>
        /// Gets or sets the metadata of the response.
        /// </summary>
        public Meta Meta { get; set; }

        /// <summary>
        /// Gets or sets the items in the response.
        /// </summary>
        public List<Item> Items { get; set; }
    }

    /// <summary>
    /// Represents the metadata of the response.
    /// </summary>
    public class Meta
    {
        /// <summary>
        /// Gets or sets the publisher of the response.
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// Gets or sets the license of the response.
        /// </summary>
        public string Licence { get; set; }

        /// <summary>
        /// Gets or sets the documentation of the response.
        /// </summary>
        public string Documentation { get; set; }

        /// <summary>
        /// Gets or sets the version of the response.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the comment about the response.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the formats of the response.
        /// </summary>
        public List<string> HasFormat { get; set; }

        /// <summary>
        /// Gets or sets the limit of the response.
        /// </summary>
        public int Limit { get; set; }
    }

    /// <summary>
    /// Represents an item in the response.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Gets or sets the identifier of the item.
        /// </summary>
        [JsonProperty("@id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the date and time of measurement.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets the measurement unit.
        /// </summary>
        public string Measure { get; set; }

        /// <summary>
        /// Gets or sets the value of the measurement.
        /// </summary>
        public double Value { get; set; }
    }
}
