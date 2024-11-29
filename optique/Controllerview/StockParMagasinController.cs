using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using optique.Data;
using optique.Dtos;
using optique.IServices;
using optique.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace optique.Controllers
{
    [Route("StockParMagasin")]
    public class StockParMagasinController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IFournisseurService _fournisseurService;
        private readonly IRefMarqueService _refMarqueService;
        private readonly IRefTypeDePaiementService _refTypeDePaiementService;
        private readonly IArticleService _articleService;
        private readonly IVenteService _venteService;
        private readonly IUserService _userService;
        private readonly IArrivageDetailsService _arrivageDetailsService; 
        private readonly ILogger<StockParMagasinController> _logger; 
        private readonly IRefDeviseService _refDeviseService;
        private readonly ApplicationDbContext _context;

        public StockParMagasinController(
            IClientService clientService, 
            IFournisseurService fournisseurService, 
            IRefMarqueService refMarqueService,
            IArticleService articleService,
            IRefTypeDePaiementService refTypeDePaiementService,
            IVenteService venteService,
            IUserService userService,
            ILogger<StockParMagasinController> logger, 
            IArrivageDetailsService arrivageDetailsService,
            IRefDeviseService refDeviseService,
            ApplicationDbContext context) 
        {
            _arrivageDetailsService = arrivageDetailsService; 
            _clientService = clientService;
            _fournisseurService = fournisseurService;
            _refMarqueService = refMarqueService;
            _articleService = articleService;
            _refTypeDePaiementService = refTypeDePaiementService; 
            _venteService = venteService;
            _userService = userService;
            _refDeviseService = refDeviseService;
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(string? clientName = null, int? fournisseurId = null, int? marqueId = null, string search = "")
        {
            var clients = await _clientService.GetMagazinInterneClientsAsync();
            var fournisseurs = await _fournisseurService.GetAllAsync();
            var marques = await _refMarqueService.GetAllAsync();
            var articles = await _clientService.GetArticlesWithQuantitiesAsync();

            if (!string.IsNullOrEmpty(clientName))
            {
                articles = articles.Where(a => a.ClientNom != null && a.ClientNom.Equals(clientName, System.StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (fournisseurId.HasValue)
            {
                articles = articles.Where(a => a.FournisseurId == fournisseurId.Value).ToList();
            }

            if (marqueId.HasValue)
            {
                articles = articles.Where(a => a.MarqueId == marqueId.Value).ToList();
            }

            if (!string.IsNullOrEmpty(search))
            {
                articles = articles.Where(a => a.Reference != null && a.Reference.Equals(search, System.StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var model = new StockParMagasinViewModel
            {
                Clients = clients,
                Fournisseurs = fournisseurs,
                Marques = marques,
                Articles = articles
            };

            return View(model);
        }

     

[HttpGet("NouvelleVente")]
public async Task<IActionResult> NouvelleVente(int? clientId, int? articleId)
{
    if (clientId.HasValue && articleId.HasValue)
    {
        HttpContext.Session.SetInt32("ClientId", clientId.Value);
        HttpContext.Session.SetInt32("ArticleId", articleId.Value);
    }
    else
    {
        clientId = HttpContext.Session.GetInt32("ClientId");
        articleId = HttpContext.Session.GetInt32("ArticleId");
    }

    if (!clientId.HasValue || !articleId.HasValue)
    {
        _logger.LogWarning("ClientId ou ArticleId manquant.");
        return RedirectToAction(nameof(Index));
    }

    var paymentDetails = HttpContext.Session.GetObjectFromJson<List<DetailsPaiementDTO>>("PaymentDetails") ?? new List<DetailsPaiementDTO>();


   

    // Charger les valeurs des champs de vente depuis la session
    var dateDeVenteSession = HttpContext.Session.GetString("DateDeVente");
    var prixDeVenteSession = HttpContext.Session.GetString("PrixDeVente");
    var quantiteVenduSession = HttpContext.Session.GetString("QuantiteVendu");
    var clientIdAcheteurSession = HttpContext.Session.GetInt32("ClientIdAcheteur");


    // Mettre à jour le modèle avec les valeurs de la session si elles existent
    if (!string.IsNullOrEmpty(dateDeVenteSession))
    {
        ViewBag.DateDeVente = dateDeVenteSession;
    }
    if (!string.IsNullOrEmpty(prixDeVenteSession))
    {
        ViewBag.PrixDeVente = prixDeVenteSession;
    }
    if (!string.IsNullOrEmpty(quantiteVenduSession))
    {
        ViewBag.QuantiteVendu = quantiteVenduSession;
    }

    var clientsParticuliers = await _clientService.GetByTypeLibelleAsync("Particulier");
    ViewBag.ClientsParticuliers = clientsParticuliers;


    var client = await _clientService.GetByIdAsync(clientId.Value);
    var articlesWithQuantities = await _clientService.GetArticlesWithQuantitiesAsync();
    var article = articlesWithQuantities.FirstOrDefault(a => a.Id == articleId.Value);

    if (client == null || article == null)
    {
        return NotFound();
    }

    var typesDePaiement = await _refTypeDePaiementService.GetAllAsync();


    /*
    var ventes = await _context.Ventes
    .Where(v => v.ArticleId == articleId.Value && v.ClientId == clientId.Value)
    .ToListAsync();

int quantiteVendu = ventes.Sum(v => v.QuantiteVendu);
*/
    var ventes = await _context.Ventes
        .Where(v => v.ArticleId == articleId.Value && v.ClientId == clientId.Value)
        .ToListAsync();
    int quantiteVendu = ventes.Sum(v => v.QuantiteVendu);




    var distributionDetails = await _context.DistributionDetails
        .Include(dd => dd.ArrivageDetails)
        .Where(dd => dd.ArrivageDetails.ArticleId == articleId.Value && dd.Distribution.ClientId == clientId.Value)
        .ToListAsync();
    int quantiteDistribuee = distributionDetails.Sum(dd => dd.Quantite);

    int quantiteDisponible = quantiteDistribuee - quantiteVendu;

   

  


    var venteVm = new NouvelleVenteViewModel
    {
        ClientId = clientId.Value,
        ArticleId = articleId.Value,
        CreePar = _userService.GetUserName(),
        DetailsPaiements = paymentDetails,
        Marque = article.MarqueLibelle ?? "",
        Reference = article.Reference ?? "",
        Couleur = article.Couleur ?? "",
        TypeArticle = article.TypeLibelle ?? "",
        QuantiteDisponible = quantiteDisponible,
        DateDeVente = !string.IsNullOrEmpty(dateDeVenteSession) ? DateTime.Parse(dateDeVenteSession) : DateTime.Now,
        PrixDeVente = !string.IsNullOrEmpty(prixDeVenteSession) ? decimal.Parse(prixDeVenteSession) : 0,
        QuantiteVendu = !string.IsNullOrEmpty(quantiteVenduSession) ? int.Parse(quantiteVenduSession) : 0,
        ClientIdAcheteur = clientIdAcheteurSession
    };

    ViewBag.TypesDePaiement = typesDePaiement.Select(t => new SelectListItem
    {
        Value = t.Id.ToString(),
        Text = t.Libelle
    });

    return View(venteVm);
}


    


[HttpPost("SavePaymentDetail")]
[ValidateAntiForgeryToken]
public IActionResult SavePaymentDetail(NouvelleVenteViewModel venteVm)
{
    var paymentDetails = HttpContext.Session.GetObjectFromJson<List<DetailsPaiementDTO>>("PaymentDetails") ?? new List<DetailsPaiementDTO>();

    if (venteVm.DetailsPaiements.Any())
    {
        // Ajouter seulement les nouveaux détails de paiement
        foreach (var detail in venteVm.DetailsPaiements)
        {
            if (!paymentDetails.Any(p => p.TypeDePaiementId == detail.TypeDePaiementId && p.Montant == detail.Montant))
            {
                paymentDetails.Add(detail);
            }
        }
    }

    // Sauvegarder les détails de paiement mis à jour dans la session
    HttpContext.Session.SetObjectAsJson("PaymentDetails", paymentDetails);

    // Stocker les valeurs de DateDeVente, PrixDeVente, et QuantiteVendu dans la session
    HttpContext.Session.SetString("DateDeVente", venteVm.DateDeVente.ToString());
    HttpContext.Session.SetString("PrixDeVente", venteVm.PrixDeVente.ToString());
    HttpContext.Session.SetString("QuantiteVendu", venteVm.QuantiteVendu.ToString());
    // Stocker ClientIdAcheteur dans la session
    if (venteVm.ClientIdAcheteur.HasValue)
    {
        HttpContext.Session.SetInt32("ClientIdAcheteur", venteVm.ClientIdAcheteur.Value);
    }


    // Redirection vers la même vue avec les données mises à jour
    return RedirectToAction("NouvelleVente", new { clientId = venteVm.ClientId, articleId = venteVm.ArticleId });
}


        [HttpPost("NouvelleVente")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NouvelleVente(NouvelleVenteViewModel venteVm)
        {


                _logger.LogWarning("ClientIdAcheteur soumis : {ClientIdAcheteur}", venteVm.ClientIdAcheteur);

            if (!venteVm.ClientIdAcheteur.HasValue)
    {
        var clientIdAcheteurSession = HttpContext.Session.GetInt32("ClientIdAcheteur");
        venteVm.ClientIdAcheteur = clientIdAcheteurSession;
    }
    

            var paymentDetails = HttpContext.Session.GetObjectFromJson<List<DetailsPaiementDTO>>("PaymentDetails") ?? new List<DetailsPaiementDTO>();

            venteVm.DetailsPaiements = paymentDetails;

            ModelState.Remove("Marque");
            ModelState.Remove("Reference");
            ModelState.Remove("Couleur");
            ModelState.Remove("TypeArticle");
            ModelState.Remove("Article");

            venteVm.CreePar = _userService.GetUserName();
            _logger.LogInformation("Nom de l'utilisateur défini sur CreePar : {CreePar}", venteVm.CreePar);

            if (string.IsNullOrEmpty(venteVm.CreePar))
            {
                ModelState.AddModelError("CreePar", "Le champ CreePar est requis.");
                _logger.LogWarning("CreePar n'est pas défini ou est vide.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("ModelState is invalid.");
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        _logger.LogError($"ModelState error in {modelStateKey}: {error.ErrorMessage}");
                    }
                }

                var typesDePaiement = await _refTypeDePaiementService.GetAllAsync();
                ViewBag.TypesDePaiement = typesDePaiement.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Libelle
                });

                return View(venteVm);
            }
             venteVm.ClientIdAcheteur = venteVm.ClientIdAcheteur;  // Vérifiez que le champ est correctement récupéré
             _logger.LogInformation("ClientIdAcheteur reçu dans POST: {ClientIdAcheteur}", venteVm.ClientIdAcheteur);



            var created = await _venteService.CreateVenteAsync(venteVm);
            if (!created)
            {
                ModelState.AddModelError(string.Empty, "Une erreur s'est produite lors de la création de la vente.");
                var typesDePaiement = await _refTypeDePaiementService.GetAllAsync();
                ViewBag.TypesDePaiement = typesDePaiement.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Libelle
                });

                return View(venteVm);
            }

            HttpContext.Session.Remove("PaymentDetails");
            HttpContext.Session.Remove("ClientIdAcheteur");

            return RedirectToAction(nameof(Index));
        }


        

    }
}