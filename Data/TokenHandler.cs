using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SerbleWebsite.Data; 

public class TokenHandler {
    private readonly Dictionary<string, string> _config;

    public TokenHandler(Dictionary<string, string> config) {
        _config = config;
    }
    
    public string GenerateToken(long maxUpload, bool isAdmin = false) {
        string mySecret = _config["token_secret"];
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[] {
                new Claim("maxUpload", maxUpload.ToString()),
                new Claim("isAdmin", isAdmin.ToString())
            }),
            Expires = DateTime.Now.AddYears(1),
            Issuer = _config["token_issuer"],
            Audience = _config["token_audience"],
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature),
        };
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
        
    public bool ValidateCurrentToken(string? token, out Dictionary<string, string>? claims) {
        claims = null;
        string mySecret = _config["token_secret"];
        SymmetricSecurityKey mySecurityKey = new(Encoding.ASCII.GetBytes(mySecret));
        JwtSecurityTokenHandler tokenHandler = new();
        try {
            tokenHandler.ValidateToken(token, new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _config["token_issuer"],
                ValidAudience = _config["token_audience"],
                IssuerSigningKey = mySecurityKey
            }, out SecurityToken _);
        }
        catch (Exception) {
            return false;
        }
        JwtSecurityTokenHandler tokenHandler2 = new();
        if (tokenHandler2.ReadToken(token) is not JwtSecurityToken securityToken) return false;

        // Put all claims in a dictionary
        if (securityToken.Claims == null) return false;
        claims = securityToken.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
        
        // If any of the values from TokenClaims are not present in the claims dictionary, return false
        foreach (string claim in TokenClaims.Claims) {
            if (!claims.ContainsKey(claim)) return false;
        }

        return true;
    }
        
}