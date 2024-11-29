using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using optique.Dtos;
using optique.IServices;

using optique.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace optique.Controllers
{
    [JwtAuthorize("your_super_secret_key_that_is_at_least_16_characters_long222")]
    [Route("ArrivageDetailsMvc")]
    public class ArrivageDetailsMvcController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IArrivageService _arrivageService;
        private readonly IArrivageDetailsService _arrivageDetailsService;
        private readonly ILogger<ArrivageDetailsMvcController> _logger;

        public ArrivageDetailsMvcController(
            IArticleService articleService,
            IArrivageService arrivageService,
            IArrivageDetailsService arrivageDetailsService,
            ILogger<ArrivageDetailsMvcController> logger)
        {
            _articleService = articleService;
            _arrivageService = arrivageService;
            _arrivageDetailsService = arrivageDetailsService;
            _logger = logger;
        }

      
      // [Authorize(Roles = "User")]
       public async Task<IActionResult> Index(string selectedSociete, string selectedFournisseur, string selectedMarque, string searchReference)
{
    _logger.LogInformation("Index method called with filters: Société = {selectedSociete}, Fournisseur = {selectedFournisseur}, Marque = {selectedMarque}, Reference = {searchReference}", selectedSociete, selectedFournisseur, selectedMarque, searchReference);

    // Utilisez le service pour filtrer les articles
    var articleDetails = await _articleService.SearchArticleDetailsByCriteriaAsync(selectedSociete, selectedFournisseur, selectedMarque, searchReference);

    // Convertir les DTO en ViewModel
    var arrivageDetails = articleDetails.Select(ad => new ArrivageDetailsViewModel
    {
        Id = ad.Id,
        ArrivageId = ad.ArrivageId,
        ArticleId = ad.ArticleId,
        Societe = ad.Societe,
        Fournisseur = ad.Fournisseur,
        DateArrivage = ad.DateArrivage,
        Marque = ad.Marque,
        Reference = ad.Reference,
        Description = ad.Description,
        Type = ad.Type,
        Livre = ad.Livre,
        Retourne = ad.Retourne,
        Distribue = ad.Distribue,
        Vendu = ad.Vendu,
        Reste = ad.Reste,
        PrixAchatNetDevise = ad.PrixAchatNetDevise,
        PrixAchatNetMAD = ad.PrixAchatNetMAD
    }).ToList();

    var viewModel = new ArrivageDetailsIndexViewModel
    {
        ArrivageDetails = arrivageDetails, // Utiliser le ViewModel converti
        SocieteList = new SelectList(arrivageDetails.Select(ad => ad.Societe).Distinct()),
        FournisseurList = new SelectList(arrivageDetails.Select(ad => ad.Fournisseur).Distinct()),
        MarqueList = new SelectList(arrivageDetails.Select(ad => ad.Marque).Distinct())
    };

    // Conserver les valeurs des filtres dans ViewData pour réutilisation dans la vue
    ViewData["SelectedSociete"] = selectedSociete;
    ViewData["SelectedFournisseur"] = selectedFournisseur;
    ViewData["SelectedMarque"] = selectedMarque;
    ViewData["SearchReference"] = searchReference;

    _logger.LogInformation("Returning Index view with filtered ViewModel");

    return View(viewModel);
}



       
[HttpGet("Filter")]
public async Task<IActionResult> Filter(string selectedSociete, string selectedFournisseur, string selectedMarque, string searchReference)
{
    _logger.LogInformation("Filter method called with filters: Société = {selectedSociete}, Fournisseur = {selectedFournisseur}, Marque = {selectedMarque}, Reference = {searchReference}", selectedSociete, selectedFournisseur, selectedMarque, searchReference);

    // Utilisez le service pour filtrer les articles
    var articleDetails = await _articleService.SearchArticleDetailsByCriteriaAsync(selectedSociete, selectedFournisseur, selectedMarque, searchReference);

    // Convertir les DTO en ViewModel
    var arrivageDetails = articleDetails.Select(ad => new ArrivageDetailsViewModel
    {
        Id = ad.Id,
        ArrivageId = ad.ArrivageId,
        ArticleId = ad.ArticleId,
        Societe = ad.Societe,
        Fournisseur = ad.Fournisseur,
        DateArrivage = ad.DateArrivage,
        Marque = ad.Marque,
        Reference = ad.Reference,
        Description = ad.Description,
        Type = ad.Type,
        Livre = ad.Livre,
        Retourne = ad.Retourne,
        Distribue = ad.Distribue,
        Vendu = ad.Vendu,
        Reste = ad.Reste,
        PrixAchatNetDevise = ad.PrixAchatNetDevise,
        PrixAchatNetMAD = ad.PrixAchatNetMAD
    }).ToList();

    var viewModel = new ArrivageDetailsIndexViewModel
    {
        ArrivageDetails = arrivageDetails,
        SocieteList = new SelectList(arrivageDetails.Select(ad => ad.Societe).Distinct()),
        FournisseurList = new SelectList(arrivageDetails.Select(ad => ad.Fournisseur).Distinct()),
        MarqueList = new SelectList(arrivageDetails.Select(ad => ad.Marque).Distinct())
    };

    // Conserver les valeurs des filtres dans ViewData pour réutilisation dans la vue
    ViewData["SelectedSociete"] = selectedSociete;
    ViewData["SelectedFournisseur"] = selectedFournisseur;
    ViewData["SelectedMarque"] = selectedMarque;
    ViewData["SearchReference"] = searchReference;

    _logger.LogInformation("Returning Filter view with filtered ViewModel");

    return View("Index", viewModel); // Utilisez la vue Index si elle affiche la liste filtrée
}

  




[HttpGet("Create")]
public async Task<IActionResult> Create(int arrivageId)
{
    _logger.LogInformation("Create method called with arrivageId: {ArrivageId}", arrivageId);

    // Get the arrivage from the database
    var arrivage = await _arrivageService.GetByIdAsync(arrivageId);
    if (arrivage == null)
    {
        _logger.LogWarning("Arrivage with id {ArrivageId} not found", arrivageId);
        return NotFound("Arrivage non trouvé.");
    }

   

    var articles = await _articleService.GetByFournisseurIdAsync(arrivage.FournisseurId);

if (articles == null || !articles.Any())
{
    _logger.LogWarning("No articles found for the selected fournisseur");
    TempData["ErrorMessage"] = "Aucun article disponible pour ce fournisseur.";
    return RedirectToAction("Index", "ArrivageMvc"); // Redirige vers la page Index ou une autre page appropriée
}


// Vérifiez les propriétés nulles des articles avant de les utiliser
var articleDtos = articles.Where(a => a != null && a.Description != null)
                          .Select(a => new ArticleDTO
                          {
                              Id = a.Id,
                              Description = a.Description ?? "Description inconnue",
                              // Assurez-vous que toutes les autres propriétés sont également vérifiées
                          }).ToList();

   // Ajout des logs pour chaque article dans la boucle

                 


    // Get the existing details from the database
    var existingDetails = await _arrivageDetailsService.GetByArrivageIdAsync(arrivageId);

    // Get details stored in the session
    var sessionDetails = HttpContext.Session.GetObjectFromJson<List<ArrivageDetailsDTO>>("ArrivageDetails") ?? new List<ArrivageDetailsDTO>();

    // Merge existing details with session details
    var allDetails = existingDetails.ToList(); // Start with details from the database
    allDetails.AddRange(sessionDetails); // Add details from the session

    var articlesList = new SelectList(articles, "Id", "Description");

    // Si aucun article n'est trouvé, définir un message d'erreur dans ViewBag
    if (articles == null || !articles.Any())
{
    _logger.LogWarning("No articles found for the selected fournisseur");
    TempData["ErrorMessage"] = "Aucun article disponible pour ce fournisseur.";
    return RedirectToAction("Index"); // Rediriger vers Index ou une autre vue
}

    // Create the view model
    var viewModel = new ArrivageDetailsFullViewModel
    {
        ArrivageId = arrivageId,
        NumBL = arrivage.NumBL,
        NumFacture = arrivage.NumFacture,
        MontantFacture = arrivage.MontantFacture,
        DateArrivage = arrivage.DateArrivage,
        SocieteId = arrivage.SocieteId,
        FournisseurId = arrivage.FournisseurId,
       SocieteNom = arrivage.SocieteNom,  // Nom de la société
        NomFournisseur = arrivage.FournisseurNom, 
       // ArticlesList = articlesList,
      ArticlesList = new SelectList(articles, "Id", "Description"),
        ArrivageDetails = allDetails 
    };

    _logger.LogInformation("Returning Create view with ViewModel");
    return View(viewModel);
}





        





[HttpPost("Create")]
public async Task<IActionResult> Create(ArrivageDetailsFullViewModel viewModel)
{
    _logger.LogInformation("Create POST method called");

    // Fetch articles again for the form dropdown
    var articles = await _articleService.GetAllAsync();
    viewModel.ArticlesList = new SelectList(articles, "Id", "Description");

 
    if (!ModelState.IsValid)
{
    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
    {
        _logger.LogWarning(error.ErrorMessage);
    }
    return View(viewModel);
}


    // Get existing details from the session (newly added ones) and from the database
    var sessionDetails = HttpContext.Session.GetObjectFromJson<List<ArrivageDetailsDTO>>("ArrivageDetails") ?? new List<ArrivageDetailsDTO>();
    var existingDetails = await _arrivageDetailsService.GetByArrivageIdAsync(viewModel.ArrivageId);

    // Add the new detail from the form to the session
    sessionDetails.Add(new ArrivageDetailsDTO
    {
        ArrivageId = viewModel.ArrivageId,
        ArticleId = viewModel.ArticleId,
        NumSerie = viewModel.NumSerie,
        PrixDachatDevise = viewModel.PrixDachatDevise,
        PrixDachatMAD = viewModel.PrixDachatMAD,
        TauxRemise = viewModel.TauxRemise,
        PrixAchatNetDevise = viewModel.PrixAchatNetDevise,
        QuantiteRecuParArticle = viewModel.QuantiteRecuParArticle,
        PrixAchatNetMAD = viewModel.PrixAchatNetMAD,
        UserName = User.Identity.Name
    });

    // Update the session with the newly added details
    HttpContext.Session.SetObjectAsJson("ArrivageDetails", sessionDetails);

    // Combine session details and existing database details
    var allDetails = existingDetails.ToList();
    allDetails.AddRange(sessionDetails);

    // Update the ViewModel with all details (existing + session) for display purposes
    viewModel.ArrivageDetails = allDetails;

    ModelState.Clear(); // Clear form inputs after submission
    return View(viewModel); // Stay on the same view to add more details
}







[HttpPost("SaveDetails")]
public async Task<IActionResult> SaveDetails(ArrivageDetailsFullViewModel viewModel)
{
    _logger.LogInformation("SaveDetails POST method called");

    // Récupérer les détails de l'arrivage à partir de la session
    var sessionDetails = HttpContext.Session.GetObjectFromJson<List<ArrivageDetailsDTO>>("ArrivageDetails");
    _logger.LogInformation("Session details retrieved: {DetailsCount}", sessionDetails?.Count);

    if (sessionDetails == null || !sessionDetails.Any())
    {
        _logger.LogWarning("No details to save");
        ModelState.AddModelError("", "Aucun détail à enregistrer.");
        return View("Create", viewModel);
    }

    // Mise à jour du ViewModel avec les détails de la session
    viewModel.ArrivageDetails = sessionDetails;

    
    

    if (ModelState.IsValid)
{
    // Sauvegarder chaque détail d'arrivage dans la base de données
    foreach (var detail in sessionDetails)
    {
        try
        {
            await _arrivageDetailsService.AddAsync(detail, User.Identity.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving ArrivageDetailsDTO for ArticleId: {ArticleId}", detail.ArticleId);
            ModelState.AddModelError("", "Une erreur s'est produite lors de l'enregistrement des détails.");
            return View("Create", viewModel);
        }
    }

    // Vider la session après un enregistrement réussi
    HttpContext.Session.Remove("ArrivageDetails");
    _logger.LogInformation("Session cleared after successful save");

    return RedirectToAction("Index");
}


    if (sessionDetails != null && sessionDetails.SequenceEqual(viewModel.ArrivageDetails))
{
    HttpContext.Session.Remove("ArrivageDetails");
    _logger.LogInformation("Session cleared after successful save");
}
else
{
    _logger.LogWarning("Session details and ViewModel details do not match, not clearing session");
}


    return RedirectToAction("Index");
}




[HttpPost("Cancel")]
public IActionResult Cancel()
{
    // Vider la session pour les détails d'arrivage
    HttpContext.Session.Remove("ArrivageDetails");

    // Rediriger vers l'action Index ou une autre page appropriée
    return RedirectToAction("Index");
}


[HttpGet("GetMarqueByArticleId")]
public async Task<IActionResult> GetMarqueByArticleId(int articleId)
{
    _logger.LogInformation("Attempting to retrieve article with ID: {ArticleId}", articleId);
var article = await _articleService.GetByIdAsync(articleId);

if (article == null)
{
    _logger.LogWarning("Article with ID {ArticleId} not found", articleId);
    return Json(new { marque = "" });
}

if (string.IsNullOrEmpty(article.MarqueLibelle))
{
    _logger.LogWarning("Article with ID {ArticleId} found but MarqueLibelle is null or empty", articleId);
}

var marque = article.MarqueLibelle;
_logger.LogInformation("Marque found: {Marque}", marque);
return Json(new { marque = marque });

}



[HttpPost("DeleteDetailFromSession")]
public IActionResult DeleteDetailFromSession(string numSerie)
{
    // Récupérer les détails de l'arrivage de la session
    var currentDetails = HttpContext.Session.GetObjectFromJson<List<ArrivageDetailsDTO>>("ArrivageDetails") ?? new List<ArrivageDetailsDTO>();

    // Trouver l'arrivageId avant de supprimer l'article
    var arrivageId = currentDetails.FirstOrDefault()?.ArrivageId ?? 0;

    // Filtrer les détails pour supprimer l'élément correspondant
    var updatedDetails = currentDetails.Where(detail => detail.NumSerie != numSerie).ToList();

    // Mettre à jour la session avec la liste modifiée
    HttpContext.Session.SetObjectAsJson("ArrivageDetails", updatedDetails);

    // Vérifier si l'arrivageId est valide après la suppression
    if (arrivageId == 0)
    {
        return RedirectToAction("Index"); // Rediriger vers Index si aucun arrivageId n'est disponible
    }

    // Rediriger vers la vue 'Create' avec le modèle de vue mis à jour
    return RedirectToAction("Create", new { arrivageId });
}









        private ArrivageDetailsViewModel ConvertToArrivageDetailsViewModel(ArticleDetailsDTO articleDetail)
        {
            return new ArrivageDetailsViewModel
            {
                Id = articleDetail.Id,
                ArrivageId = articleDetail.ArrivageId,
                ArticleId = articleDetail.ArticleId,
                Societe = articleDetail.Societe,
                Fournisseur = articleDetail.Fournisseur,
                DateArrivage = articleDetail.DateArrivage,
                Marque = articleDetail.Marque,
                Reference = articleDetail.Reference,
                Description = articleDetail.Description,
                Type = articleDetail.Type,
                Livre = articleDetail.Livre,
                Retourne = articleDetail.Retourne,
                Vendu = articleDetail.Vendu,
                Distribue = articleDetail.Distribue,
                Reste = articleDetail.Reste,
                PrixAchatNetDevise = articleDetail.PrixAchatNetDevise,
                PrixAchatNetMAD = articleDetail.PrixAchatNetMAD
            };
        }



[HttpPost("Importer")]
public async Task<IActionResult> Importer(IFormFile file, int arrivageId)
{
    _logger.LogInformation("Importer POST method called");

    if (arrivageId == 0)
    {
        _logger.LogWarning("ArrivageId is 0, this indicates a problem in passing the ID.");
        return RedirectToAction("Create", new { arrivageId });
    }

    if (file == null || file.Length == 0)
    {
        _logger.LogWarning("No file uploaded or the file is empty.");
        ModelState.AddModelError("", "Veuillez sélectionner un fichier Excel.");
        return RedirectToAction("Create", new { arrivageId });
    }

    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

    var arrivageDetails = new List<ArrivageDetailsDTO>();

    using (var stream = new MemoryStream())
    {
        await file.CopyToAsync(stream);

        using (var package = new ExcelPackage(stream))
        {
            var worksheet = package.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                try
                {
                    // Extraction des données du fichier Excel
                    string reference = worksheet.Cells[row, 1].Text?.ToString().Trim();
                    string numSerie = worksheet.Cells[row, 2].Text?.ToString().Trim(); // Numéro de série ajouté

                    // Use TryParse for numeric values to avoid exceptions
                    if (!int.TryParse(worksheet.Cells[row, 3].Text?.ToString().Trim(), out int quantiteRecu))
                    {
                        _logger.LogWarning($"Quantité reçue invalide à la ligne {row}. Valeur: {worksheet.Cells[row, 3].Text}");
                        continue; // Skip this row if the value is invalid
                    }

                    if (!decimal.TryParse(worksheet.Cells[row, 4].Text?.ToString().Trim(), out decimal prixDachatDevise))
                    {
                        _logger.LogWarning($"Prix d'achat en devise invalide à la ligne {row}. Valeur: {worksheet.Cells[row, 4].Text}");
                        continue; // Skip this row if the value is invalid
                    }

                    if (!decimal.TryParse(worksheet.Cells[row, 5].Text?.ToString().Trim(), out decimal prixDachatMAD))
                    {
                        _logger.LogWarning($"Prix d'achat en MAD invalide à la ligne {row}. Valeur: {worksheet.Cells[row, 5].Text}");
                        continue; // Skip this row if the value is invalid
                    }

                    if (!decimal.TryParse(worksheet.Cells[row, 6].Text?.ToString().Trim(), out decimal tauxRemise))
                    {
                        _logger.LogWarning($"Taux de remise invalide à la ligne {row}. Valeur: {worksheet.Cells[row, 6].Text}");
                        continue; // Skip this row if the value is invalid
                    }

                    // Recherche de l'article en base de données via la référence
                    var article = await _articleService.GetByReferenceAsync(reference);
                    if (article == null)
                    {
                        _logger.LogWarning($"Article with reference {reference} not found at row {row}.");
                        continue; // Passe à la ligne suivante
                    }

                    // Crée un nouveau ArrivageDetailsDTO à partir des données extraites
                    var detail = new ArrivageDetailsDTO
                    {
                        ArrivageId = arrivageId,
                        ArticleId = article.Id,
                        NumSerie = numSerie, // Ajouter le numéro de série ici
                        QuantiteRecuParArticle = quantiteRecu,
                        PrixDachatDevise = prixDachatDevise,
                        PrixDachatMAD = prixDachatMAD,
                        TauxRemise = tauxRemise,
                        PrixAchatNetDevise = prixDachatDevise * (1 - (tauxRemise / 100)),
                        PrixAchatNetMAD = prixDachatMAD * (1 - (tauxRemise / 100)),
                        UserName = User.Identity.Name
                    };

                    arrivageDetails.Add(detail);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Erreur à la ligne {row} du fichier Excel.");
                }
            }
        }
    }

    // Récupérer les détails actuels de la session
    var currentDetails = HttpContext.Session.GetObjectFromJson<List<ArrivageDetailsDTO>>("ArrivageDetails") ?? new List<ArrivageDetailsDTO>();

    // Ajouter les nouveaux détails importés à la liste actuelle
    currentDetails.AddRange(arrivageDetails);

    // Mettre à jour la session avec la nouvelle liste
    HttpContext.Session.SetObjectAsJson("ArrivageDetails", currentDetails);

    _logger.LogInformation($"Imported {arrivageDetails.Count} details from Excel.");

    return RedirectToAction("Create", new { arrivageId });
}



    }}