using Microsoft.AspNetCore.Mvc;

namespace SerbleWebsite.api.v1; 

[ApiController]
[Route("api/v1/adam/")]
public class AdamController : Controller {
    
    [HttpGet]
    public IActionResult Get() {
        return Ok("Did you know that cats are cute?");
    }
    
    [HttpGet("{cat:int}")]
    public IActionResult Get(int cat) {
        return Ok($"<img src=\"https://http.cat/{cat}\">");
    }
    
    [HttpPost]
    public IActionResult Post() {
        return Ok("You posted me");
    }
    
    [HttpPut]
    public IActionResult Put() {
        return Ok("I have been put");
    }
    
    [HttpDelete]
    public IActionResult Delete() {
        return Ok($"NOOOOOOOO DON'T DELETE ME");
    }
    
    [HttpPatch]
    public IActionResult Patch() {
        return Ok("I have been patched");
    }
    
}