using GeneralPurposeLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SerbleWebsite.Data;
using SerbleWebsite.Data.Schemas;

namespace SerbleWebsite.api.v1; 

[ApiController]
[Route("api/v1/account/")]
public class AccountController : Controller {
    
    [HttpGet]
    public ActionResult<SanitisedUser> Index([FromQuery] string appId, [FromHeader] AuthorizationHeader? authorizationHeader) {
        // Check if the app has access to the user's account
        if (authorizationHeader == null) {
            Logger.Debug("No authorization header provided");
            return Unauthorized();
        }

        if (!authorizationHeader.Check(appId, out string[]? scopes, out User? user, out string? msg)) {
            Logger.Debug("Check failed: " + msg);
            
            // Log all headers
            Logger.Debug("Headers:");
            foreach (KeyValuePair<string, StringValues> header in Request.Headers) {
                Logger.Debug($"{header.Key}: {header.Value}");
            }
            
            return Unauthorized();
        }

        if (scopes == null || user == null) {
            Logger.Debug("NULL");
            return Unauthorized();
        }

        return new SanitisedUser(user, ScopeHandler.ListOfScopeIdsToString(scopes));
    }
    
}

public class Adam {
    public Adam() {
        Logger.Error("OH NO U UNLEASHED ADAM");
        throw new Exception("Adam");
    }
}