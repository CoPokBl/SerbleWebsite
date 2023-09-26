using System.Text.Json.Serialization;

namespace SerbleWebsite.Data.Schemas; 

public class AuthResponseBody {
    [JsonPropertyName("token")]
    public string Token { get; set; } = null!;
    
    [JsonPropertyName("mfa_token")]
    public string MfaToken { get; set; } = null!;
    
    [JsonPropertyName("mfa_required")]
    public bool MfaRequired { get; set; }
}