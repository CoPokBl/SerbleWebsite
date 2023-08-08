using System.Text.Json.Serialization;

namespace SerbleWebsite.Data.Schemas; 

public class ProductCheckoutEntry {
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    
    [JsonPropertyName("priceid")]
    public string PriceId { get; set; } = null!;
}