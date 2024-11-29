/*using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using optique.Models;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthController(UserManager<ApplicationUser> userManager, IAuthService authService, ILogger<AuthController> logger, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _authService = authService;
        _logger = logger;
        _roleManager = roleManager;
    }

    [HttpPost("register")]
public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
{
    if (ModelState.IsValid)
    {
        var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            // Assigner le rôle après la création de l'utilisateur
            var roleName = "user";  // Utiliser un rôle
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Role {roleName} does not exist.");
            }

            await _userManager.AddToRoleAsync(user, roleName);

            //  message de succès
            _logger.LogInformation("User created a new account with password.");
            return Ok("User registered successfully.");
        }
        AddErrors(result);
    }

    // retourner un résultat BadRequest avec les erreurs
    return BadRequest(ModelState);
}


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        var user = await _userManager.FindByNameAsync(loginModel.Username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginModel.Password))
        {
            return Unauthorized();
        }

        // Validation que UserName n'est pas null
        if (string.IsNullOrEmpty(user.UserName))
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: UserName is null or empty.");
        }

        var token = _authService.GenerateToken(user.UserName, "User");
        return Ok(new { Token = token });
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}*/
