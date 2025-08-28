using System.Text.Json.Serialization;

namespace IntegrationApi.Core.DTOs
{
    public class QueryVariables
    {
        [JsonPropertyName("search")]
        public string? Search { get; set; }
        
        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}