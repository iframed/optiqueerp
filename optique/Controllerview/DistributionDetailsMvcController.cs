using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using optique.Dtos;
using optique.IServices;
using optique.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace optique.Controllerview
{
    [Route("DistributionMvc")]
    public class DistributionMvcController : Controller
    {
        private readonly IDistributionService _distributionService;
        private readonly IDistributionDetailsService _distributionDetailsService;
        private readonly IRefMarqueService _refMarqueService;
        private readonly IRefStatutDistributionService _refStatutDistributionService;
        private readonly IClientService _clientService;
        private readonly IFournisseurService _fournisseurService;
        private readonly ISocieteService _societeService; 
       private readonly IArticleService _articleService;

        public DistributionMvcController(
            IDistributionService distributionService,
            IDistributionDetailsService distributionDetailsService,
            IRefMarqueService refMarqueService,
            IRefStatutDistributionService refStatutDistributionService,
            IClientService clientService,
            IFournisseurService fournisseurService,
            ISocieteService societeService,
            IArticleService articleService)  // Ajoutez ce paramètre
        {
            _distributionService = distributionService;
            _distributionDetailsService = distributionDetailsService;
            _refMarqueService = refMarqueService;
            _refStatutDistributionService = refStatutDistributionService;
            _clientService = clientService;
            _fournisseurService = fournisseurService;
            _societeService = societeService;  
            _articleService = articleService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(string? client, string? fournisseur, string? marque, string? statut, string? reference)
        {
            // Récupérer toutes les données de distribution
            var distributionSummaries = await _distributionService.SearchByCriteriaAsync(client, fournisseur, marque, statut, reference);

            // Récupérer toutes les marques, statuts, clients, fournisseurs et sociétés
            var marques = await _refMarqueService.GetAllAsync();
            var statuts = await _refStatutDistributionService.GetAllAsync();
            var clients = await _clientService.GetAllAsync();
            var fournisseurs = await _fournisseurService.GetAllAsync();
            var societes = await _societeService.GetAllAsync();  // Utilisez _societeService ici

            // Extraire les noms distincts de clients, fournisseurs, marques, statuts et sociétés
            var clientNames = clients.Select(c => c.NomClient).Distinct().ToList();
            var fournisseurNames = fournisseurs.Select(f => f.NomFournisseur).Distinct().ToList();
            var marqueNames = marques.Select(m => m.Libelle).Distinct().ToList();
            var statutNames = statuts.Select(s => s.Libelle).Distinct().ToList();
            var societeSelectList = societes.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.NomSociete }).ToList();  // Utilisez societeSelectList ici

            // Passer les données à la vue via ViewBag
            ViewBag.Clients = clientNames;
            ViewBag.Fournisseurs = fournisseurNames;
            ViewBag.Marques = marqueNames;
            ViewBag.Statuts = statutNames;
            ViewBag.Societes = societeSelectList;  // Ajoutez Societes au ViewBag
            ViewBag.Client = client;
            ViewBag.Fournisseur = fournisseur;
            ViewBag.Marque = marque;
            ViewBag.Statut = statut;
            ViewBag.Reference = reference;

            return View("Index", distributionSummaries);
        }

       

        [HttpGet("Create")]
public async Task<IActionResult> Create(int? arrivageDetailsId)
{
   // var clients = await _clientService.GetAllAsync();
    var clients = await _clientService.GetByTypeLibelleAsync("MagazinInterne");
    var statuts = await _refStatutDistributionService.GetAllAsync();
    var societes = await _societeService.GetAllAsync();

    // Utiliser le service ArticleService pour récupérer les détails de l'arrivage
    ArticleDetailsDTO? articleDetails = null;
    if (arrivageDetailsId.HasValue)
    {
        articleDetails = await _articleService.GetArticleDetailsByArrivageDetailsIdAsync(arrivageDetailsId.Value);
    }

    var viewModel = new DistributionCreateViewModel
    {
        ArrivageDetailsId = arrivageDetailsId,
        DateDistribution = DateTime.Now,
        Clients = clients.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.NomClient }).ToList(),
        StatutsDistribution = statuts.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Libelle }).ToList(),
        Societes = societes.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.NomSociete }).ToList(),
        ArticleDetails = articleDetails // Ajouter les détails de l'article au ViewModel
    };

    return View(viewModel);
}




       

        [HttpPost("Create")]
public async Task<IActionResult> Create(DistributionCreateViewModel viewModel)
{
    if (!ModelState.IsValid)
    {
        // Recharger les données en cas d'erreur de validation
        var clients = await _clientService.GetAllAsync();
        var statuts = await _refStatutDistributionService.GetAllAsync();
        var societes = await _societeService.GetAllAsync();

        viewModel.Clients = clients.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.NomClient }).ToList();
        viewModel.StatutsDistribution = statuts.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Libelle }).ToList();
        viewModel.Societes = societes.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.NomSociete }).ToList();

        return View(viewModel);
    }

    // Récupérer les détails de l'article
    var articleDetails = await _articleService.GetArticleDetailsByArrivageDetailsIdAsync(viewModel.ArrivageDetailsId.Value);
    if (articleDetails == null)
    {
        ModelState.AddModelError("", "Détails de l'article non trouvés.");
        return View(viewModel);
    }

    // Calculer la quantité restante
    var quantiteRestante = articleDetails.Livre - (articleDetails.Retourne + articleDetails.Distribue);

    // Vérifier si la quantité demandée pour distribution est inférieure ou égale à la quantité restante
    if (viewModel.Quantite > quantiteRestante)
    {
        ModelState.AddModelError("Quantite", "La quantité demandée dépasse la quantité restante.");
        
        // Recharger les données pour réafficher les listes déroulantes en cas d'erreur
        var clients = await _clientService.GetAllAsync();
        var statuts = await _refStatutDistributionService.GetAllAsync();
        var societes = await _societeService.GetAllAsync();

        viewModel.Clients = clients.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.NomClient }).ToList();
        viewModel.StatutsDistribution = statuts.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Libelle }).ToList();
        viewModel.Societes = societes.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.NomSociete }).ToList();

        return View(viewModel); // Retourner la vue avec l'erreur
    }

    // Création du DTO de distribution
    var distributionDto = new DistributionDTO
    {
        DateDistribution = viewModel.DateDistribution,
        ClientId = viewModel.ClientId,
        StatutDistributionId = viewModel.StatutDistributionId,
        SocieteId = viewModel.SocieteId
    };

    int distributionId = await _distributionService.AddAsync(distributionDto);

    // Création du DTO de détail de distribution
    var distributionDetailsDto = new DistributionDetailsDTO
    {
        DistributionId = distributionId,
        ArrivageDetailsId = viewModel.ArrivageDetailsId.Value,
        Quantite = viewModel.Quantite,
        PrixDeVente = viewModel.PrixDeVente,
        NumFacture = viewModel.NumFacture,
        Statut = "Active"
    };

    // Récupérer le nom de l'utilisateur connecté
    var userName = User.Identity?.Name ?? "Utilisateur inconnu";

    await _distributionDetailsService.AddAsync(distributionDetailsDto, userName);

    return RedirectToAction("Index");
}

    }
}
