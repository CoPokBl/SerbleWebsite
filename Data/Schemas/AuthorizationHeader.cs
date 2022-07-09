using Microsoft.AspNetCore.Mvc;

namespace SerbleWebsite.Data.Schemas; 

public class AuthorizationHeader {
    
    [FromHeader]
    public string Authorization { get; set; }
    
    public bool Check(string appId, out string[]? scopes, out User? user) {
        scopes = null;
        user = null;
        
        if (string.IsNullOrEmpty(Authorization)) {
            return false;
        }
        
        string[] parts = Authorization.Split(' ');
        if (parts.Length != 2) {
            return false;
        }
        
        if (parts[0] != "Bearer") {
            return false;
        }
        
        // Find app
        Program.StorageService!.GetOAuthApp(appId, out OAuthApp? app);
        if (app == null) {
            return false;
        }
        
        TokenHandler tokenHandler = new TokenHandler(Program.Config!);
        if (!tokenHandler.ValidateCurrentToken(parts[1], out Dictionary<string, string>? claims, app)) {
            return false;
        }
        
        scopes = ScopeHandler.StringToListOfScopeIds(claims!["scope"]);
        
        return true;
    }
    
}