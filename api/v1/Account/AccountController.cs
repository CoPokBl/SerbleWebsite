using GeneralPurposeLib;
using Microsoft.AspNetCore.Mvc;
using SerbleWebsite.Data;
using SerbleWebsite.Data.Schemas;

namespace SerbleWebsite.api.v1.Account; 

[ApiController]
[Route("api/v1/account/{userid}")]
public class AccountController : Controller {
    
    [HttpGet]
    public ActionResult<SanitisedUser> Index(string userId, [FromHeader] AuthorizationHeaderUser? authorizationHeader) {
        // Check if the app has access to the user's account
        if (authorizationHeader == null) {
            Logger.Debug("No authorization header provided");
            return Unauthorized();
        }

        if (!authorizationHeader.Check(userId, out string[]? scopes, out User? user, out string? msg)) {
            Logger.Debug("Check failed: " + msg);
            return Unauthorized();
        }

        if (scopes == null || user == null) {
            Logger.Debug("NULL");
            return Unauthorized();
        }

        return new SanitisedUser(user, ScopeHandler.ListOfScopeIdsToString(scopes));
    }
    
    [HttpDelete]
    public ActionResult Delete(string userId, [FromHeader] AuthorizationHeaderUser? authorizationHeader) {
        // Check if the app has access to the user's account
        if (authorizationHeader == null) {
            Logger.Debug("No authorization header provided");
            return Unauthorized();
        }

        if (!authorizationHeader.Check(userId, out string[]? scopes, out User? user, out string? msg)) {
            Logger.Debug("Check failed: " + msg);
            return Unauthorized();
        }
        
        if (scopes == null || user == null) {
            Logger.Debug("NULL");
            return Unauthorized();
        }

        IEnumerable<ScopeHandler.ScopesEnum> scopesListEnum = ScopeHandler.ScopesIdsToEnumArray(scopes);
        if (!scopesListEnum.Contains(ScopeHandler.ScopesEnum.FullAccess)) {
            Logger.Debug("No full access");
            return Unauthorized();
        }
        
        // Delete the user's account
        Program.StorageService!.DeleteUser(user.Id);
        return Ok();
    }

}

public class Adam {
    public Adam() {
        Logger.Error("OH NO U UNLEASHED ADAM");
        throw new Exception("Adam");
    }
}