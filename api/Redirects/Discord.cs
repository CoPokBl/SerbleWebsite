using Microsoft.AspNetCore.Mvc;

namespace SerbleWebsite.api.Redirects; 

[ApiController]
[Route("discord")]
public class Discord : Controller {
    
    [HttpGet]
    public IActionResult Get() {
        return Redirect("https://discord.gg/fzvcNhW");
    }
    
}