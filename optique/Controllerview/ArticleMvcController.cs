using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using optique.Dtos;
using optique.IServices;
using optique.ViewModels;
using System.Linq;

using System.Threading.Tasks;
using Newtonsoft.Json;

namespace optique.Controllers
{
   // [JwtAuthorize("your_super_secret_key_that_is_at_least_16_characters_long222")]
   // [Route("ArticleMvc")]
  
    public class ArticleMvcController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IFournisseurService _fournisseurService;
        private readonly IRefMarqueService _marqueService;
        private readonly IRefTypeService _typeService;

        private readonly ILogger<ArticleMvcController> _logger;

        public ArticleMvcController(
            IArticleService articleService, 
            IFournisseurService fournisseurService, 
            IRefMarqueService marqueService, 
            IRefTypeService typeService,
             ILogger<ArticleMvcController> logger)
        
        {
            _articleService = articleService;
            _fournisseurService = fournisseurService;
            _marqueService = marqueService;
            _typeService = typeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var fournisseurs = await _fournisseurService.GetAllAsync();
            var marques = await _marqueService.GetAllAsync();
            var types = await _typeService.GetAllAsync();

         

            var model = new ArticleViewModel
{    Fournisseurs = fournisseurs,
                Marques = marques,
                Types = types,
    Articles = await _articleService.GetAllAsync(),
    FournisseurList = new SelectList(fournisseurs, "NomFournisseur", "NomFournisseur"),  // Utilise NomFournisseur
    MarqueList = new SelectList(marques, "Libelle", "Libelle"),  // Utilise Libelle
    TypeList = new SelectList(types, "Libelle", "Libelle")  // Utilise Libelle
};


            return View(model);
        }

       


       [HttpGet]
public async Task<IActionResult> Filter(string? selectedFournisseur, string? selectedMarque, string? selectedType, string? searchReference)
{
    // Ajout de journaux pour la débogage
    Console.WriteLine($"Contrôleur - Fournisseur: {selectedFournisseur}, Marque: {selectedMarque}, Type: {selectedType}, Référence: {searchReference}");

    // Correction de la requête de filtre
    var articles = await _articleService.SearchArticlesByCriteriaAsync(
        selectedFournisseur, 
        selectedMarque, 
        selectedType, 
        searchReference?.ToLower());

    Console.WriteLine($"Nombre d'articles trouvés après filtrage: {articles.Count()}");

    var model = new ArticleViewModel
    {
        Articles = articles,
        FournisseurList = new SelectList(await _fournisseurService.GetAllAsync(), "Id", "NomFournisseur"),
        MarqueList = new SelectList(await _marqueService.GetAllAsync(), "Id", "Libelle"),
        TypeList = new SelectList(await _typeService.GetAllAsync(), "Id", "Libelle"),
        SelectedFournisseur = selectedFournisseur,
        SelectedMarque = selectedMarque,
        SelectedType = selectedType,
        SearchReference = searchReference
    };

    return View("Index", model);
}




[HttpGet("Create")]
public async Task<IActionResult> Create()
{
    var articleDTO = new ArticleDTO("default description", "default couleur", "default reference");

    // Fetch the necessary data for dropdowns
    ViewBag.MarqueList = new SelectList(await _marqueService.GetAllAsync(), "Id", "Libelle");
    ViewBag.TypeList = new SelectList(await _typeService.GetAllAsync(), "Id", "Libelle");
    ViewBag.FournisseurList = new SelectList(await _fournisseurService.GetAllAsync(), "Id", "NomFournisseur");

    return View(articleDTO);
}


[HttpPost("Create")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(ArticleDTO articleDTO)
{
    if (!ModelState.IsValid)
    {
        ViewBag.MarqueList = new SelectList(await _marqueService.GetAllAsync(), "Id", "Libelle");
        ViewBag.TypeList = new SelectList(await _typeService.GetAllAsync(), "Id", "Libelle");
        ViewBag.FournisseurList = new SelectList(await _fournisseurService.GetAllAsync(), "Id", "NomFournisseur");
        
        return View(articleDTO);
    }

    await _articleService.AddAsync(articleDTO);
    return RedirectToAction("Index");
}


[HttpGet]
public async Task<IActionResult> Delete(int id)
{
    await _articleService.DeleteAsync(id);
    return RedirectToAction("Index");
}



    }}