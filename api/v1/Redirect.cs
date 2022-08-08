using Microsoft.AspNetCore.Mvc;

namespace SerbleWebsite.api.v1; 

[ApiController]
[Route("/api/v1/redirect")]
public class Redirect : Controller {
    
    [HttpGet]
    public IActionResult Get([FromQuery] string to) {
        return Redirect(to);
    }
    
    [HttpPost]
    public IActionResult Post([FromBody] string to) {
        return Redirect(to);
    }
    
}