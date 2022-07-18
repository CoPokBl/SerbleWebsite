using Microsoft.AspNetCore.Mvc;

namespace SerbleWebsite.Data.Schemas; 

public class AuthorizationHeader {
    
    [FromHeader]
    public string Authorization { get; set; }
    
    [FromHeader]
    public string X_App_Secret { get; set; } 
    
    public bool Check(string appId, out string[]? scopes, out User? user, out string? msg) {
        scopes = null;
        user = null;
        msg = null;
        
        if (string.IsNullOrEmpty(Authorization)) {
            msg = "Authorization header is missing";
            return false;
        }

        if (string.IsNullOrEmpty(X_App_Secret)) {
            msg = "X_App_Secret header is missing";
            return false;
        }
        
        string[] parts = Authorization.Split(' ');
        if (parts.Length != 2) {
            msg = "Header is not in the correct format";
            return false;
        }
        
        if (parts[0] != "Bearer") {
            msg = "Header is not Bearer";
            return false;
        }
        
        // Find app
        Program.StorageService!.GetOAuthApp(appId, out OAuthApp? app);
        if (app == null) {
            msg = "App null";
            return false;
        }
        
        TokenHandler tokenHandler = new TokenHandler(Program.Config!);
        if (!tokenHandler.ValidateCurrentToken(parts[1], out Dictionary<string, string>? claims, out string failMsg, app)) {
            msg = "Token validation failed: " + failMsg;
            return false;
        }

        if (app.ClientSecret != X_App_Secret) {
            msg = "X_App_Secret is not correct";
            return false;
        }
        
        scopes = ScopeHandler.StringToListOfScopeIds(claims!["scope"]);
        Program.StorageService.GetUser(claims["id"], out user);

        msg = "Check success";
        return true;
    }
    
}