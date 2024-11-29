using Microsoft.AspNetCore.Mvc;

namespace optique.Controllers
{
    public class StockController : Controller
    {
        public IActionResult StockIndex()
        {
            return View();
        }

       
    }
}
