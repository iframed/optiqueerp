using Microsoft.AspNetCore.Mvc;
using optique.IServices;
using System.Threading.Tasks;

namespace optique.Controllers
{
    public class StockParArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IRefTypeService _refTypeService;
        private readonly IFournisseurService _fournisseurService;
        private readonly IRefMarqueService _refMarqueService;

        public StockParArticleController(IArticleService articleService, IRefTypeService refTypeService, IFournisseurService fournisseurService, IRefMarqueService refMarqueService)
        {
            _articleService = articleService;
            _refTypeService = refTypeService;
            _fournisseurService = fournisseurService;
            _refMarqueService = refMarqueService;
        }

        public async Task<IActionResult> StockParArticle(int? typeId = null, int? fournisseurId = null, int? marqueId = null, string reference = null)
        {
            // Charger les articles avec ou sans filtres
            var articlesGrouped = await _articleService.GetArticlesGroupedByDescriptionTypeMarqueAsync(typeId, fournisseurId, marqueId, reference);
            
            // Charger les données pour les filtres
            ViewBag.Types = await _refTypeService.GetAllAsync();
            ViewBag.Fournisseurs = await _fournisseurService.GetAllAsync();
            ViewBag.Marques = await _refMarqueService.GetAllAsync();
            
            return View(articlesGrouped);
        }
    }
}
