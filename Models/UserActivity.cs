using System;
using System.Text.Json.Serialization;

namespace G3MWL.Models
{
    public class UserActivity
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("userEmail")]
        public string UserEmail { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}


