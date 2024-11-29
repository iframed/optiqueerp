using Microsoft.AspNetCore.Mvc;

namespace optique.Controllers
{
    public class BaseDeDonneesController : Controller
    {
        // GET: /BaseDeDonnees/
        public IActionResult Index()
        {
            return View();
        }
    }
}
