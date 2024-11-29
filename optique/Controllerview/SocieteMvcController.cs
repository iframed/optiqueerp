using Microsoft.AspNetCore.Mvc;
using optique.IServices;
using optique.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.Controllers
{
    [Route("SocieteMvc")]
    public class SocieteMvcController : Controller
    {
        private readonly ISocieteService _societeService;

        public SocieteMvcController(ISocieteService societeService)
        {
            _societeService = societeService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(string? adresse)
        {
            // Fetch all societies or filter by address
            var societes = string.IsNullOrEmpty(adresse)
                ? await _societeService.GetAllAsync()
                : await _societeService.GetByAdresseAsync(adresse);

            // Populate ViewBag for filters
            ViewBag.Adresse = adresse;

            return View("Index", societes);
        }

[HttpGet("Create")]
public IActionResult Create()
{
    // Return the view for creating a new society (Societe)
    return View();
}


        [HttpPost("Create")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(SocieteDTO societeDto)
{
    if (ModelState.IsValid)
    {
        await _societeService.AddAsync(societeDto);
        return RedirectToAction("Index");
    }
    return View(societeDto);
}

    }
}
