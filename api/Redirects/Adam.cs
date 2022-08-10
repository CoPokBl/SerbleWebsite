using Microsoft.AspNetCore.Mvc;

namespace SerbleWebsite.api.Redirects; 

[ApiController]
[Route("adam")]
public class Adam : Controller {
    
    [HttpGet]
    public IActionResult Get() {
        return Redirect("https://adamflore.com/");
    }
    
}