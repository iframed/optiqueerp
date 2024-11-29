using Microsoft.AspNetCore.Mvc;
using optique.IServices;
using optique.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.Controllers
{
    [Route("FournisseurMvc")]
    public class FournisseurMvcController : Controller
    {
        private readonly IFournisseurService _fournisseurService;

         private readonly IRefDeviseService _refDeviseService; 

        public FournisseurMvcController(IFournisseurService fournisseurService, IRefDeviseService refDeviseService)
        {
            _fournisseurService = fournisseurService;
            _refDeviseService = refDeviseService;
        }

       [HttpGet("")]
public async Task<IActionResult> Index(string? deviseLibelle, string? ice)
{
    // Récupération de tous les fournisseurs ou application des filtres si spécifiés
    var fournisseurs = await _fournisseurService.GetAllAsync();
    
    if (!string.IsNullOrEmpty(deviseLibelle))
    {
        fournisseurs = fournisseurs.Where(f => f.DeviseLibelle == deviseLibelle).ToList();
    }

    if (!string.IsNullOrEmpty(ice))
    {
        fournisseurs = fournisseurs.Where(f => f.ICE == ice).ToList();
    }

    // Remplissage du ViewBag pour les filtres
    ViewBag.Devises = (await _fournisseurService.GetAllAsync()).Select(f => f.DeviseLibelle).Distinct().ToList();
    ViewBag.DeviseLibelle = deviseLibelle;
    ViewBag.ICE = ice;

    return View("Index", fournisseurs);
}

// Updated Create method to include the list of devises
        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            // Fetch available devises using RefDeviseService
            var devises = await _refDeviseService.GetAllAsync();
            ViewBag.Devises = devises.Select(d => new { d.Libelle, d.Code }).ToList();

            return View();
        }

        // Handle the POST for creating a new fournisseur
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FournisseurDTO fournisseurDto)
        {
            if (ModelState.IsValid)
            {
                await _fournisseurService.AddAsync(fournisseurDto);
                return RedirectToAction("Index");
            }

            // If there are errors, re-fetch the devises for the dropdown
            var devises = await _refDeviseService.GetAllAsync();
            ViewBag.Devises = devises.Select(d => new { d.Libelle, d.Code }).ToList();

            return View(fournisseurDto);
        }
    }
        

       
    }

