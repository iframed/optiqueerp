

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using optique.Models;
using optique.ViewModels;

namespace optique.Controllerview
{
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AuthController> logger, IAuthService authService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _authService = authService;
            _roleManager = roleManager;
        }

        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        


       /* [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Login(LoginPageModel model)
{
    if (ModelState.IsValid)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
            {
                var token = _authService.GenerateToken(user.UserName, "User");
                Response.Cookies.Append("AuthToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });

                return RedirectToAction("Index", "Acceuil");

            }
            ModelState.AddModelError("", "Invalid login attempt.");
        }
        else
        {
            ModelState.AddModelError("", "User not found.");
        }
    }
    return View(model);
}*/



[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginPageModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        var token = _authService.GenerateToken(user.UserName, "user");
                        Response.Cookies.Append("AuthToken", token, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTimeOffset.UtcNow.AddHours(1)
                        });

                        return RedirectToAction("Index", "Acceuil");
                    }
                    else
                    {
                        // Définir le message d'erreur pour une tentative de connexion invalide
                        ViewBag.ErrorMessage = "Tentative de connexion invalide. Veuillez vérifier votre mot de passe.";
                    }
                }
                else
                {
                    // Définir le message d'erreur pour un utilisateur non trouvé
                    ViewBag.ErrorMessage = "Utilisateur non trouvé. Veuillez vérifier votre nom d'utilisateur.";
                }
            }

            // Si le modèle n'est pas valide ou si une erreur est survenue, retournez la vue avec le modèle et les messages d'erreur
            return View(model);
        }


        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Logout()
{
    // Déconnexion de l'utilisateur via SignInManager
    await _signInManager.SignOutAsync();

    // Supprimer le cookie contenant le token JWT
    if (Request.Cookies["AuthToken"] != null)
    {
        Response.Cookies.Delete("AuthToken");
    }

    // Journaliser l'action de déconnexion
    _logger.LogInformation("User logged out.");

    // Rediriger l'utilisateur vers la page de login ou d'accueil
  return RedirectToAction("Login", "Auth");


}

    }

    }
