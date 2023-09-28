using System.Text.Json.Serialization;

namespace SerbleWebsite.Data.Schemas; 

public class TotpCheckResponse {
    [JsonPropertyName("valid")]
    public bool Valid { get; set; }
}