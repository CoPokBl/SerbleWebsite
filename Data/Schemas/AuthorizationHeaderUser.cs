using Microsoft.AspNetCore.Mvc;

namespace SerbleWebsite.Data.Schemas; 

public class AuthorizationHeaderUser {
    
    [FromHeader]
    // Format: "APPID SECRET"
    public string SerbleAuth { get; set; }

    public bool Check(string userId, out string[]? scopes, out User? user, out string? msg) {
        scopes = null;
        user = null;
        msg = null;
        
        if (string.IsNullOrEmpty(SerbleAuth)) {
            msg = "Authorization header is missing";
            return false;
        }

        string[] parts = SerbleAuth.Split(' ');
        if (parts.Length != 3) {
            msg = "Header is not in the correct format";
            return false;
        }

        string appId = parts[0];
        string secret = parts[1];

        // Find app
        Program.StorageService!.GetOAuthApp(appId, out OAuthApp? app);
        if (app == null) {
            msg = "App null";
            return false;
        }

        if (app.ClientSecret != secret) {
            msg = "Client secret is not correct";
            return false;
        }
        
        // Check if app is authorized for user
        Program.StorageService.GetUser(userId, out user);
        if (user == null) {
            msg = "User not found";
            return false;
        }

        if (user.PermLevel < (int) AccountAccessLevel.Normal) {
            msg = "User account is disabled";
            return false;
        }

        if (!user.AuthorizedAppIds.Contains(appId)) {
            msg = "App unauthorized for user";
            return false;
        }
        
        scopes = ScopeHandler.StringToListOfScopeIds(
            user.AuthorizedApps
                .Where(appObj => appObj.AppId == appId)
                .Select(appObj2 => appObj2.Scopes)
                .First());

        msg = "Check success";
        return true;
    }
    
}