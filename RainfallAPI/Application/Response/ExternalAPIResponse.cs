// Represents the response from an external API
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RainfallAPI.Application.Response
{
    public class ExternalAPIResponse
    {
        // Context of the response
        [JsonProperty("@context")]
        public string Context { get; set; }
        // Metadata of the response
        public Meta Meta { get; set; }
        // Items in the response
        public List<Item> Items { get; set; }
    }

    public class Meta
    {
        // Publisher of the response
        public string Publisher { get; set; }
        // License of the response
        public string Licence { get; set; }
        // Documentation of the response
        public string Documentation { get; set; }
        // Version of the response
        public string Version { get; set; }
        // Comment about the response
        public string Comment { get; set; }
        // Formats of the response
        public List<string> HasFormat { get; set; }
        // Limit of the response
        public int Limit { get; set; }
    }

    public class Item
    {
        // Identifier of the item
        [JsonProperty("@id")]
        public string Id { get; set; }
        // Date and time of measurement
        public DateTime DateTime { get; set; }
        // Measurement unit
        public string Measure { get; set; }
        // Value of the measurement
        public double Value { get; set; }
    }
}
