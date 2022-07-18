using Microsoft.AspNetCore.Mvc;

namespace SerbleWebsite.Data.Schemas; 

public class AuthorizationHeader {
    
    [FromHeader]
    public string SerbleAuth { get; set; }

    public bool Check(string appId, out string[]? scopes, out User? user, out string? msg) {
        scopes = null;
        user = null;
        msg = null;
        
        if (string.IsNullOrEmpty(SerbleAuth)) {
            msg = "Authorization header is missing";
            return false;
        }

        string[] parts = SerbleAuth.Split(' ');
        if (parts.Length != 2) {
            msg = "Header is not in the correct format";
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

        if (app.ClientSecret != parts[0]) {
            msg = "X_App_Secret is not correct";
            return false;
        }
        
        scopes = ScopeHandler.StringToListOfScopeIds(claims!["scope"]);
        Program.StorageService.GetUser(claims["id"], out user);

        msg = "Check success";
        return true;
    }
    
}