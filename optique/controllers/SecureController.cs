using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class SecureController : ControllerBase
{
   /* [HttpGet("admin")]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult AdminEndpoint()
    {
        return Ok("This is an admin endpoint.");
    }*/

    [HttpGet("user")]
    [Authorize(Policy = "UserOnly")]
    public IActionResult UserEndpoint()
    {
        return Ok("This is a user endpoint.");
    }
}
