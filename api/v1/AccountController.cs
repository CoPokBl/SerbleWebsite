using Microsoft.AspNetCore.Mvc;

namespace SerbleWebsite.api.v1; 

[ApiController]
[Route("api/v1/account")]
public class AccountController : Controller {
    
    [HttpGet]
    public IActionResult Index([FromQuery] string appId) {
        return Ok();
    }
    
}