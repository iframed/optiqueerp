using Microsoft.AspNetCore.Mvc;
using optique.IServices;
using optique.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace optique.Controllers
{
    [Route("ClientMvc")]
    public class ClientMvcController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ITypeClientService _typeClientService;
         private readonly ILogger<ClientMvcController> _logger;


        public ClientMvcController(IClientService clientService, ITypeClientService typeClientService,ILogger<ClientMvcController> logger)
        {
            _clientService = clientService;
             _typeClientService = typeClientService;
             _logger = logger;
            
        }

        // Utilisation d'une seule méthode d'action pour afficher et filtrer les clients
        [HttpGet("")]
        public async Task<IActionResult> Index(string? adresse, string? nomClient, string? typeLibelle)
        {
            // Récupérer tous les clients
            var clients = await _clientService.GetAllAsync();

            // Fetch all client types from the service and store them in ViewBag
    var typesClient = await _typeClientService.GetAllAsync();
    ViewBag.TypesClient = typesClient.ToList(); 
            // Appliquer les filtres si les valeurs sont fournies
            if (!string.IsNullOrWhiteSpace(adresse))
            {
                clients = clients.Where(c => c.Adresse.Contains(adresse)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(nomClient))
            {
                clients = clients.Where(c => c.NomClient.Contains(nomClient)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(typeLibelle))
            {
                clients = clients.Where(c => c.TypeClientLibelle.Contains(typeLibelle)).ToList();
            }

            // Transmettre les valeurs de filtre à la vue pour les afficher dans les champs
            ViewBag.Adresse = adresse;
            ViewBag.NomClient = nomClient;
            ViewBag.TypeLibelle = typeLibelle;

            return View("Index", clients);
        }

        // Autres méthodes CRUD ici...
    
        // Autres méthodes CRUD ici...

        [HttpGet("Create")]
public async Task<IActionResult> Create()
{
    // Récupération de tous les types de clients
    var typesClient = await _typeClientService.GetAllAsync();
    // Passage des types de clients à la vue via ViewBag
    ViewBag.TypesClient = typesClient.ToList();

    // Retourner la vue pour la création de client
    return View();
}






    
   
 [HttpPost("Create")]
public async Task<IActionResult> Create(ClientDTO clientDTO)
{
    // Log les informations du client
    Console.WriteLine($"Nom du client: {clientDTO.NomClient}, Type de client ID: {clientDTO.TypeClientId}, Libelle: {clientDTO.TypeClientLibelle}");

    var typeClient = await _typeClientService.GetByIdAsync(clientDTO.TypeClientId);
    if (typeClient == null)
    {
        ModelState.AddModelError("TypeClientId", "Le type de client spécifié n'existe pas.");
    }
    else
    {
        Console.WriteLine($"TypeClient Libelle Retrieved: {typeClient.Libelle}");
        clientDTO.TypeClientLibelle = typeClient.Libelle;
    }

    // Log les erreurs de validation si le modèle est invalide
    if (!ModelState.IsValid)
    {
        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
            Console.WriteLine($"Validation Error: {error.ErrorMessage}");
        }

        // Recharger les types de client en cas d'erreur pour remplir la liste déroulante
        var typesClient = await _typeClientService.GetAllAsync();
        ViewBag.TypesClient = typesClient.ToList();
        return View(clientDTO);
    }

    // Tentative d'ajout du client
    try
    {
        Console.WriteLine($"Saving client: {clientDTO.NomClient}");
        await _clientService.AddAsync(clientDTO);
        Console.WriteLine("Client successfully created.");
        return RedirectToAction(nameof(Index));
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error creating client: {ex.Message}");
        var typesClient = await _typeClientService.GetAllAsync();
        ViewBag.TypesClient = typesClient.ToList();
        return View(clientDTO);
    }
}




        // Méthodes pour Edit, Delete, etc...



        

    }
}
