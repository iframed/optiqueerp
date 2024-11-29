using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using optique.IServices;
using optique.ViewModels; 
using System.Linq; 
using System.Threading.Tasks;

namespace optique.Controllers
{
   [Authorize(Policy = "UserOnly")]

    public class AcceuilController : Controller
    {
        private readonly IArrivageDetailsService _arrivageDetailsService;
        private readonly IRetourFournisseurService _retourFournisseurService;
        private readonly IDistributionDetailsService _distributionDetailsService;
        private readonly IVenteService _venteService;

        public AcceuilController(
            IArrivageDetailsService arrivageDetailsService,
            IRetourFournisseurService retourFournisseurService,
            IDistributionDetailsService distributionDetailsService,
            IVenteService venteService)
        {
            _arrivageDetailsService = arrivageDetailsService;
            _retourFournisseurService = retourFournisseurService;
            _distributionDetailsService = distributionDetailsService;
            _venteService = venteService;
        }

        // GET: /Acceuil/
        
          public async Task<IActionResult> Index()
        {
            var arrivages = await _arrivageDetailsService.GetAllAsync();
            var retours = await _retourFournisseurService.GetAllAsync();
            var distributions = await _distributionDetailsService.GetAllAsync();
            var ventes = await _venteService.GetAllAsync();

            var model = new StatistiquesViewModel
            {
                NombreArrivages = arrivages.Count(),
                NombreRetours = retours.Count(),
                NombreDistributions = distributions.Count(),
                NombreVentes = ventes.Count(),
                NombreArticlesRecus = arrivages.Sum(a => a.QuantiteRecuParArticle),
                NombreArticlesRetournes = retours.Sum(r => r.QuantiteRetourne),
                NombreArticlesDistribues = distributions.Sum(d => d.Quantite),
                NombreArticlesVendus = ventes.Sum(v => v.Quantite)
            };

            return View("Acceuil", model); 
        }
    }
}
