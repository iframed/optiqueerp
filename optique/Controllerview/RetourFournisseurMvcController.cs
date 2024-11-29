using Microsoft.AspNetCore.Mvc;
using optique.IServices;
using optique.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using optique.Services;
using optique.ViewModels;

namespace optique.Controllers
{
    [Route("RetourFournisseurMvc")]
    public class RetourFournisseurMvcController : Controller
    {
        private readonly IRetourFournisseurService _retourFournisseurService;
        private readonly IRefTypeRetourService _refTypeRetourService;
        private readonly IUserService _userService;
        private readonly IArticleService _articleService; // Ajout du service ArticleService
        private readonly ILogger<RetourFournisseurMvcController> _logger;

        public RetourFournisseurMvcController(IRetourFournisseurService retourFournisseurService,
                                              IRefTypeRetourService refTypeRetourService,
                                              IUserService userService,
                                              IArticleService articleService, // Ajout du service ArticleService
                                              ILogger<RetourFournisseurMvcController> logger)
        {
            _retourFournisseurService = retourFournisseurService;
            _refTypeRetourService = refTypeRetourService;
            _userService = userService;
            _articleService = articleService; // Initialisation du service ArticleService
            _logger = logger;
        }

        // Méthode d'action pour afficher et filtrer les retours fournisseurs
        [HttpGet("")]
        public async Task<IActionResult> Index(string? societe, string? marque, string? reference, string? fournisseur)
        {
            var retourFournisseurDetails = await _retourFournisseurService.GetFournisseurDetailsAsync();

            if (!string.IsNullOrWhiteSpace(societe))
            {
                retourFournisseurDetails = retourFournisseurDetails.Where(r => r.Societe.Contains(societe)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(marque))
            {
                retourFournisseurDetails = retourFournisseurDetails.Where(r => r.Marque.Contains(marque)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(reference))
            {
                retourFournisseurDetails = retourFournisseurDetails.Where(r => r.Reference.Contains(reference)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(fournisseur))
            {
                retourFournisseurDetails = retourFournisseurDetails.Where(r => r.Fournisseur.Contains(fournisseur)).ToList();
            }

            ViewBag.Societes = retourFournisseurDetails.Select(r => r.Societe).Distinct().ToList();
            ViewBag.Fournisseurs = retourFournisseurDetails.Select(r => r.Fournisseur).Distinct().ToList();
            ViewBag.Marques = retourFournisseurDetails.Select(r => r.Marque).Distinct().ToList();

            ViewBag.Societe = societe;
            ViewBag.Marque = marque;
            ViewBag.Reference = reference;
            ViewBag.Fournisseur = fournisseur;

            return View("Index", retourFournisseurDetails);
        }

        // Méthode d'action pour afficher le formulaire de création
        [HttpGet("Create")]
        public async Task<IActionResult> Create(int arrivageDetailsId)
        {
            // Utiliser ArticleService pour récupérer les détails de l'article
            var articleDetails = await _articleService.GetArticleDetailsByArrivageDetailsIdAsync(arrivageDetailsId);

            // Vérification si les détails de l'article ont été trouvés
            if (articleDetails == null)
            {
                _logger.LogWarning("Détails de l'article non trouvés pour ArrivageDetailsId: {ArrivageDetailsId}", arrivageDetailsId);
                return NotFound("Détails de l'article non trouvés.");
            }

            // Log des informations récupérées
            _logger.LogInformation("Fournisseur: {Fournisseur}, DateArrivage: {DateArrivage}, MontantFacture: {MontantFacture}, ReferenceProduit: {ReferenceProduit}, QuantiteLivree: {QuantiteLivree}",
                articleDetails.Fournisseur,
                articleDetails.DateArrivage,
                articleDetails.PrixAchatNetDevise,
                articleDetails.Reference,
                articleDetails.Livre);

            // Création du ViewModel avec les détails récupérés
            var viewModel = new RetourFournisseurViewModel
{
    ArrivageDetailsId = arrivageDetailsId,
    Societe = articleDetails.Societe ?? "Non spécifié",
    Fournisseur = articleDetails.Fournisseur ?? "Non spécifié",
    DateArrivage = articleDetails.DateArrivage != DateTime.MinValue ? articleDetails.DateArrivage : (DateTime?)null,
    MontantFacture = articleDetails.PrixAchatNetDevise , // Utilisez '0m' pour decimal
    ReferenceProduit = articleDetails.Reference ?? "Non spécifié",
    QuantiteLivree = articleDetails.Livre , // Assurez-vous que 'Livre' est un 'int?' sinon convertissez-le
    TypeRetours = await GetTypeRetoursAsync()
};



            // Journalisation du ViewModel créé
            _logger.LogInformation("ViewModel créé: {@ViewModel}", viewModel);

            ViewBag.TypeRetours = viewModel.TypeRetours;

            return View(viewModel);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(RetourFournisseurViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity?.Name;

                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not authenticated.");
                }

             
             // Récupérer les détails de l'arrivage
        var articleDetails = await _articleService.GetArticleDetailsByArrivageDetailsIdAsync(viewModel.ArrivageDetailsId);

        if (articleDetails == null)
        {
            _logger.LogWarning("Détails de l'article non trouvés pour ArrivageDetailsId: {ArrivageDetailsId}", viewModel.ArrivageDetailsId);
            return NotFound("Détails de l'article non trouvés.");
        }

        // Calculer la quantité restante
        var quantiteRestante = articleDetails.Livre - (articleDetails.Retourne + articleDetails.Distribue);

        // Vérifier si la quantité restante est supérieure ou égale à la quantité retournée demandée
        if (viewModel.QuantiteRetourne > quantiteRestante)
        {
            ModelState.AddModelError("QuantiteRetourne", "La quantité retournée dépasse la quantité restante.");
            viewModel.TypeRetours = await GetTypeRetoursAsync();
            return View(viewModel); // Retourner à la vue avec un message d'erreur
        }


                 


                var retourFournisseurDTO = new RetourFournisseurDTO
                {
                    ArrivageDetailsId = viewModel.ArrivageDetailsId,
                    DateRetour = viewModel.DateRetour,
                    QuantiteRetourne = viewModel.QuantiteRetourne,
                    MotifRetour = viewModel.MotifRetour,
                    TypeRetourId = viewModel.TypeRetourId,
                    UserName = userName
                };

                _logger.LogInformation("Creating a new return with DTO data: {DTOData}", retourFournisseurDTO);
                await _retourFournisseurService.AddAsync(retourFournisseurDTO, userName);
                _logger.LogInformation("Return created successfully.");

                return RedirectToAction("Index");
            }

            viewModel.TypeRetours = await GetTypeRetoursAsync();
            ViewBag.TypeRetours = viewModel.TypeRetours;
            return View(viewModel);
        }

        private async Task<List<SelectListItem>> GetTypeRetoursAsync()
        {
            var types = await _refTypeRetourService.GetAllAsync();
            return types.Select(tr => new SelectListItem
            {
                Value = tr.Id.ToString(),
                Text = tr.Libelle
            }).ToList();
        }
    }
}
