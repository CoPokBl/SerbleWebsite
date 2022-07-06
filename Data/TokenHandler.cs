using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GeneralPurposeLib;
using Microsoft.IdentityModel.Tokens;
using SerbleWebsite.Data.Schemas;

namespace SerbleWebsite.Data; 

public class TokenHandler {
    private readonly Dictionary<string, string> _config;

    public TokenHandler(Dictionary<string, string> config) {
        _config = config;
    }
    
    public string GenerateToken(Dictionary<string, string> claims, OAuthApp? app = null) {
        
        foreach (string claim in TokenClaims.Claims) {
            if (!claims.ContainsKey(claim)) Logger.Warn("The following required claim was missing from a generated token: " + claim);
        }

        if (app != null) {
            // Add client secret claim
            claims.Add("client_secret", app.ClientSecret);
        }
        
        string mySecret = _config["token_secret"];
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(claims.Select(c => new Claim(c.Key, c.Value)).ToArray()),
            Expires = DateTime.Now.AddYears(1),
            Issuer = _config["token_issuer"],
            Audience = _config["token_audience"],
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature),
        };
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
        
    public bool ValidateCurrentToken(string? token, out Dictionary<string, string>? claims, OAuthApp? app = null) {
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

        if (app != null) {
            if (claims["client_secret"] != app.ClientSecret) {
                return false;
            }
        }
        else {
            if (claims.ContainsKey("client_secret")) {
                // It's an app token but it's being checked as a user token
                return false;
            }
        }
        
        // If any of the values from TokenClaims are not present in the claims dictionary, return false
        foreach (string claim in TokenClaims.Claims) {
            if (!claims.ContainsKey(claim)) return false;
        }

        return true;
    }
        
}