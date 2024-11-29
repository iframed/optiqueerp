


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using optique.Dtos;
using optique.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using optique.ViewModels;
using optique.Services;


namespace optique.Controllers
{
[Route("ArrivageMvc")]
public class ArrivageMvcController : Controller
{
    private readonly IArrivageService _arrivageService;
    private readonly IFournisseurService _fournisseurService;
    private readonly ISocieteService _societeService;
    private readonly IRefTypeDePaiementService _refTypeDePaiementService;

    private readonly IRefStatutDistributionService _RefStatutDistributionService;
    private readonly ILogger<ArrivageMvcController> _logger;


    public ArrivageMvcController(IArrivageService arrivageService, IFournisseurService fournisseurService, ISocieteService societeService,IRefTypeDePaiementService refTypeDePaiementService, IRefStatutDistributionService RefStatutDistributionService,ILogger<ArrivageMvcController> logger)
    {
        _arrivageService = arrivageService;
        _fournisseurService = fournisseurService;
        _societeService = societeService;
        _refTypeDePaiementService = refTypeDePaiementService; 
        _RefStatutDistributionService = RefStatutDistributionService; 
        _logger = logger;
    }



[HttpGet]
[Route("")]
public async Task<IActionResult> Index()
{
    var fournisseurs = await _fournisseurService.GetAllAsync();
    var arrivages = await _arrivageService.GetAllAsync();

    // Map FournisseurId to NomFournisseur for display
    var arrivagesWithFournisseurNames = arrivages
    .OrderByDescending(a => a.DateArrivage)
    .Select(a => new ArrivageDTO
    {
        Id = a.Id,
        DateArrivage = a.DateArrivage,
        NumBL = a.NumBL,
        NumFacture = a.NumFacture,
        MontantFacture = a.MontantFacture,
        FournisseurId = a.FournisseurId,
        FournisseurNom = fournisseurs.FirstOrDefault(f => f.Id == a.FournisseurId)?.NomFournisseur, // Utilisez la bonne propriété ici
        StatutId = a.StatutId,  // Ajout du statut
        StatutDistributionLibelle = a.StatutDistributionLibelle 
    });

    var model = new ArrivageViewModel
    {
        Arrivages = arrivagesWithFournisseurNames,
        FournisseurList = new SelectList(fournisseurs, "Id", "NomFournisseur") // Utilisez la bonne propriété ici
    };
    

    return View(model);
}
[HttpGet]
[Route("Filter")]
public async Task<IActionResult> Filter(string? selectedFournisseur, string? searchNumFacture)
{
    var arrivages = await _arrivageService.GetAllAsync();
    var fournisseurs = await _fournisseurService.GetAllAsync();
    

    // Convertir selectedFournisseur en int si non nul et filtrer par FournisseurId
    if (!string.IsNullOrEmpty(selectedFournisseur) && int.TryParse(selectedFournisseur, out int fournisseurId))
    {
        arrivages = arrivages.Where(a => a.FournisseurId == fournisseurId).ToList();
    }

    // Filtrer par numéro de facture
    if (!string.IsNullOrEmpty(searchNumFacture))
    {
        arrivages = arrivages.Where(a => a.NumFacture.Contains(searchNumFacture)).ToList();
    }

    var arrivagesWithFournisseurNames = arrivages.Select(a => new ArrivageDTO
    {
        Id = a.Id,
        DateArrivage = a.DateArrivage,
        NumBL = a.NumBL,
        NumFacture = a.NumFacture,
        MontantFacture = a.MontantFacture,
        FournisseurId = a.FournisseurId,
        FournisseurNom = fournisseurs.FirstOrDefault(f => f.Id == a.FournisseurId)?.NomFournisseur
    });

    var model = new ArrivageViewModel
    {
        Arrivages = arrivagesWithFournisseurNames,
        FournisseurList = new SelectList(fournisseurs, "Id", "NomFournisseur"),
        SelectedFournisseur = selectedFournisseur,
        SearchNumFacture = searchNumFacture
    };

    return View("Index", model);
}

    
  
[HttpGet]
[Route("Create")]
public async Task<IActionResult> Create()
{
    // Récupération des fournisseurs, sociétés et types de paiement
    var fournisseurs = await _fournisseurService.GetAllAsync();
    var societes = await _societeService.GetAllAsync();
    var typesDePaiement = await _refTypeDePaiementService.GetAllAsync();
    var statuts = await _RefStatutDistributionService.GetAllAsync();

    // Initialiser ViewBag pour la vue
    ViewBag.Fournisseurs = fournisseurs;
    ViewBag.Societes = societes;

    // Ajouter les types de paiement dans le ViewBag
    ViewBag.TypesDePaiement = typesDePaiement.Select(t => new SelectListItem
    {
        Value = t.Id.ToString(),
        Text = t.Libelle
    });

    ViewBag.Statuts = (await _RefStatutDistributionService.GetAllAsync()).Select(s => new SelectListItem
    {
        Value = s.Id.ToString(),
        Text = s.Libelle
    });


    // Initialiser le modèle avec une liste vide de détails de paiement
    var model = new ArrivageDTO
    {
        DetailsPaiements = new List<DetailsPaiementDTO>() // Toujours initialiser la liste
    };

    return View(model);
}





    


[HttpPost]
[Route("Create")]
public async Task<IActionResult> Create(ArrivageDTO dto)
{
    if (ModelState.IsValid)
    {
        try
        {
            // Création de l'arrivage
            var arrivageId = await _arrivageService.AddAsync(dto);

            // Si l'arrivage est créé, ajouter les paiements multiples
            if (arrivageId > 0)
            {
                if (dto.DetailsPaiements != null && dto.DetailsPaiements.Count > 0)
                {
                    foreach (var paiement in dto.DetailsPaiements)
                    {
                        await _arrivageService.AjouterPaiementPourArrivage(arrivageId, paiement);
                    }
                }

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Erreur lors de la création de l'arrivage.");
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Erreur : " + ex.Message);
        }
    }

    // Recharger les listes de fournisseurs, sociétés et types de paiement en cas d'erreur
    ViewBag.Fournisseurs = await _fournisseurService.GetAllAsync();
    ViewBag.Societes = await _societeService.GetAllAsync();
    ViewBag.TypesDePaiement = (await _refTypeDePaiementService.GetAllAsync()).Select(t => new SelectListItem
    {
        Value = t.Id.ToString(),
        Text = t.Libelle
    });

     // Récupérer les statuts à nouveau en cas d'erreur de validation
    var statuts = await _RefStatutDistributionService.GetAllAsync();
    ViewBag.Statuts = statuts.Select(s => new SelectListItem
    {
        Value = s.Id.ToString(),
        Text = s.Libelle
    });


    return View(dto);
}



[HttpPost]
[Route("AjouterPaiement")]
public async Task<IActionResult> AjouterPaiement(int arrivageId, DetailsPaiementDTO paiementDto)
{

    _logger.LogInformation("Démarrage de la méthode AjouterPaiement pour l'arrivage ID : {ArrivageId}", arrivageId);

    // Log du DTO reçu
    _logger.LogInformation("Détails du paiement reçu : TypeDePaiementId={TypeDePaiementId}, NCheque={NCheque}, NLCN={NLCN}, Montant={Montant}, DateEcheance={DateEcheance}",
        paiementDto.TypeDePaiementId, paiementDto.NCheque, paiementDto.NLCN, paiementDto.Montant, paiementDto.DateEcheance);

    if (ModelState.IsValid)
    {
        try
        {
            await _arrivageService.AjouterPaiementPourArrivage(arrivageId, paiementDto);
            return RedirectToAction("Details", new { id = arrivageId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Erreur lors de l'ajout du paiement : " + ex.Message);
        }
    }

    // Si quelque chose échoue, revenir à la vue actuelle avec les erreurs
    return View(paiementDto);
}

[HttpGet]
[Route("Details/{arrivageId}")]
public async Task<IActionResult> Details(int arrivageId)
{
    // Récupérer l'arrivage par son Id
    var arrivage = await _arrivageService.GetByIdAsync(arrivageId);
    if (arrivage == null)
    {
        return NotFound();
    }

    // Récupérer les détails des paiements associés
    var detailsPaiements = await _arrivageService.GetDetailsPaiementsByArrivageId(arrivageId);

  
    var totalPaiements = detailsPaiements.Sum(p => p.Montant); // Somme des paiements

    // Charger tous les types de paiement
   
    var typesDePaiement = await _refTypeDePaiementService.GetAllAsync();
ViewBag.TypesDePaiement = typesDePaiement.Select(t => new SelectListItem
{
    Value = t.Id.ToString(), // L'ID du type de paiement
    Text = t.Libelle // Le libellé affiché
});


    var viewModel = new DetailsViewModel
    {
        Arrivage = arrivage,
        SocieteNom = await _societeService.GetNomByIdAsync(arrivage.SocieteId),
        FournisseurNom = await _fournisseurService.GetNomByIdAsync(arrivage.FournisseurId),
        StatutDistributionLibelle = await _RefStatutDistributionService.GetLibelleByIdAsync(arrivage.StatutId),
        IsValiderVisible = totalPaiements == arrivage.MontantFacture, // Condition pour afficher le bouton
        DetailsPaiements = new List<DetailsPaiementViewModel>()
        
    };
    // Remplir les détails des paiements
    foreach (var paiement in detailsPaiements)
    {
        if (paiement != null)
        {
            viewModel.DetailsPaiements.Add(new DetailsPaiementViewModel
            {
                TypeDePaiementId = paiement.TypeDePaiementId,
                NCheque = paiement.NCheque,
                NLCN = paiement.NLCN,
                Montant = paiement.Montant,
                DateEcheance = paiement.DateEcheance,
                TypeDePaiementLibelle = await GetTypeDePaiementLibelle(paiement.TypeDePaiementId)
            });
        }
    }

    // Initialiser ViewBag avec les types de paiement pour le formulaire
    ViewBag.TypesDePaiement = typesDePaiement.Select(t => new SelectListItem
    {
        Value = t.Id.ToString(),
        Text = t.Libelle
    });

    return View(viewModel);
}

private async Task<string> GetTypeDePaiementLibelle(int typeDePaiementId)
{
    var typeDePaiement = await _refTypeDePaiementService.GetByIdAsync(typeDePaiementId);
    return typeDePaiement?.Libelle ?? string.Empty;
}


[HttpPost]
[Route("Valider")]
public async Task<IActionResult> Valider(int arrivageId)
{
    _logger.LogWarning("Tentative de validation de l'arrivage avec ID: {ArrivageId}", arrivageId);
    
    var isUpdated = await _arrivageService.ValiderArrivage(arrivageId);

    if (isUpdated)
    {
        _logger.LogInformation("L'arrivage avec ID {ArrivageId} a été validé avec succès.", arrivageId);
        return RedirectToAction("Index");
    }

    _logger.LogWarning("La validation de l'arrivage avec ID {ArrivageId} a échoué.", arrivageId);
    ModelState.AddModelError("", "Erreur lors de la validation de l'arrivage.");
    return RedirectToAction("Details", new { arrivageId });
}


    
}}