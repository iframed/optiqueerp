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
    [Route("VenteMvc")]
    public class VenteMvcController : Controller
    {
        private readonly IVenteService _venteService;
        private readonly IClientService _clientService;
        private readonly IRefMarqueService _refMarqueService;
        private readonly IRefTypeService _refTypeService;
        private readonly IRefTypeDePaiementService _refTypeDePaiementService;
        private readonly IArticleService _articleService;

        public VenteMvcController(
            IVenteService venteService,
            IClientService clientService,
            IRefMarqueService refMarqueService,
            IRefTypeService refTypeService,
            IArticleService articleService,
            IRefTypeDePaiementService refTypeDePaiementService)
        {
            _venteService = venteService;
            _clientService = clientService;
            _refMarqueService = refMarqueService;
            _refTypeService = refTypeService;
            _articleService = articleService;
            _refTypeDePaiementService = refTypeDePaiementService; 
        }

        



        [HttpGet("")]
public async Task<IActionResult> Index(string? client, string? marque, string? typeArticle, string? reference)
{
    IEnumerable<VenteDetailsDTO> venteDetails;

    // Si aucun critère de recherche n'est fourni, on récupère toutes les ventes avec GetAllAsync
    if (string.IsNullOrEmpty(client) && string.IsNullOrEmpty(marque) && string.IsNullOrEmpty(typeArticle) && string.IsNullOrEmpty(reference))
    {
        venteDetails = await _venteService.GetAllAsync();
    }
    else
    {
        // Si des critères de recherche sont fournis, on effectue la recherche
        venteDetails = await _venteService.SearchAsync(client, marque, typeArticle, reference);
    }

    // Trier les résultats par la date de vente (du plus récent au plus ancien)
    var sortedVenteDetails = venteDetails.OrderByDescending(v => v.DateDeVente).ToList();

    // Récupérer les options pour les filtres
    var clients = await _clientService.GetAllAsync();
    var marques = await _refMarqueService.GetAllAsync();
    var typesArticle = await _refTypeService.GetAllAsync();

    // Créer le modèle de vue
    var model = new VenteViewModel
    {
        VenteDetails = sortedVenteDetails,
        Clients = clients.Select(c => c.NomClient).Distinct().ToList(),
        Marques = marques.Select(m => m.Libelle).Distinct().ToList(),
        TypesArticle = typesArticle.Select(t => t.Libelle).Distinct().ToList(),
        SelectedClient = client,
        SelectedMarque = marque,
        SelectedTypeArticle = typeArticle,
        Reference = reference
    };

    return View("Index", model);
}


 



 


[HttpGet("Calendar")]
public async Task<IActionResult> Calendar(int? month, int? year, string view = "month")
{
    var currentMonth = month ?? DateTime.Now.Month;
    var currentYear = year ?? DateTime.Now.Year;

    // Récupérer les chèques clients et fournisseurs
    var clientCheques = await _venteService.GetChequeDueDatesAsync(currentMonth, currentYear, view);
    var fournisseurCheques = await _venteService.GetFournisseurChequeDueDatesAsync(currentMonth, currentYear, view);

    var clientLCNs = await _venteService.GetLCNDueDatesAsync(currentMonth, currentYear, view);
    var fournisseurLCNs = await _venteService.GetFournisseurLCNDueDatesAsync(currentMonth, currentYear, view);


    // Log pour vérifier les données
    /*_logger.LogDebug("Nombre de chèques clients: {Count}", clientCheques.Count());
    _logger.LogDebug("Nombre de chèques fournisseurs: {Count}", fournisseurCheques.Count());*/

    // Créer le modèle de vue
    var model = new CalendarViewModel
    {
        ClientCheques = clientCheques,
        FournisseurCheques = fournisseurCheques,
        ClientLCNs = clientLCNs, // Ajoutez les LCN clients
        FournisseurLCNs = fournisseurLCNs // Ajoutez les LCN fournisseurs
    };

    ViewBag.CurrentMonth = currentMonth;
    ViewBag.CurrentYear = currentYear;
    ViewBag.CurrentView = view;

    return View(model);
}

[HttpGet("GetLCNsByDate")]
public async Task<IActionResult> GetLCNsByDate(DateTime date, string type)
{
    IEnumerable<ChequeDueDateDTO> lcns;

    if (type == "client")
    {
        // Utilisation de la date exacte pour filtrer
        lcns = await _venteService.GetLCNDueDatesByDayAsync(date); // Filtrer par date exacte
    }
    else if (type == "fournisseur")
    {
        lcns = await _venteService.GetFournisseurLCNDueDatesByExactDateAsync(date); // Filtrer par date exacte
    }
    else
    {
        return BadRequest("Type de LCN invalide.");
    }

    return PartialView("_ChequesDetails", lcns); // Assurez-vous que la vue partielle "_ChequesDetails" est correctement configurée pour afficher les résultats
}








[HttpGet("GetChequesByDate")]
public async Task<IActionResult> GetChequesByDate(DateTime date, string type)
{
    IEnumerable<ChequeDueDateDTO> cheques;
    
    if (type == "client")
    {
        cheques = await _venteService.GetChequesByDateAsync(date); // Récupère les chèques des clients
    }
    else if (type == "fournisseur")
    {
        cheques = await _venteService.GetFournisseurChequesByDateAsync(date); // Récupère les chèques des fournisseurs
    }
    else
    {
        return BadRequest("Type de chèque invalide.");
    }

    return PartialView("_ChequesDetails", cheques);
}







}



    }

