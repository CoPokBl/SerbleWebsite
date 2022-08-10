using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace SerbleWebsite.api.v1; 

[ApiController]
[Route("api/v1/checkuser")]
public class CheckUser : Controller {
    
    [HttpGet]
    public IActionResult Get([FromQuery] string redirect, [FromQuery] bool redirectOnFail = false) {
        
        // On Fail Check
        if (false) {
            if (redirectOnFail) {
                return Redirect(QueryHelpers.AddQueryString(redirect, "success", "false"));
            }
            return Redirect("/accessdenied");
        }
        
        return Redirect(QueryHelpers.AddQueryString(redirect, "success", "true"));
    }
    
}