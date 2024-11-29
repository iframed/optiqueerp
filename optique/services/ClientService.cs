using AutoMapper;
using MyAspNetApp.Repositories;
using optique.Dtos;
using optique.Models;
using optique.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace optique.Services
{
    public class ClientService : IClientService
    {
        private readonly IRepository<Client> _repository;
        private readonly IRepository<DistributionDetails> _distributionDetailsRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<RefTypeClient> _typeClientRepository;
        private readonly ILogger<Client> _logger;

        public ClientService(
            IRepository<Client> repository,
            IRepository<DistributionDetails> distributionDetailsRepository,
            IMapper mapper,
            IRepository<RefTypeClient> typeClientRepository,
             ILogger<Client> logger)
           
        {
            _repository = repository;
            _distributionDetailsRepository = distributionDetailsRepository;
            _mapper = mapper;
            _typeClientRepository = typeClientRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ClientDTO>> GetAllAsync()
        {
           var clients = await _repository.ListAsync();
           // Utilisation d'Include pour charger les types de clients avec les clients
   //var clients = await _repository.ListAsync(query => query.Include(c => c.TypeClient));
    
            var clientDTOs = _mapper.Map<IEnumerable<ClientDTO>>(clients);

            foreach (var clientDTO in clientDTOs)
            {
                var typeClient = await _typeClientRepository.GetByIdAsync(clientDTO.TypeClientId);
                clientDTO.TypeClientLibelle = typeClient?.Libelle ?? "Type inconnu";
            }

            return clientDTOs;
        }

        public async Task<ClientDTO?> GetByIdAsync(int id)
        {
            var client = await _repository.GetByIdAsync(id);
            return _mapper.Map<ClientDTO?>(client);
        }

        /*public async Task AddAsync(ClientDTO clientDTO)
        {
            var typeClient = await _typeClientRepository.GetByIdAsync(clientDTO.TypeClientId);
            if (typeClient == null)
            {
                throw new Exception("Le type de client spécifié n'existe pas.");
            }

            var client = _mapper.Map<Client>(clientDTO);
            client.TypeClient = typeClient;
            await _repository.AddAsync(client);
        }*/

       public async Task AddAsync(ClientDTO clientDTO)
        {
            _logger.LogInformation($"Début de l'ajout d'un nouveau client: {clientDTO.NomClient}");

            try
            {
                var typeClient = await _typeClientRepository.GetByIdAsync(clientDTO.TypeClientId);

                if (typeClient == null)
                {
                    _logger.LogError($"Le type de client avec l'ID {clientDTO.TypeClientId} n'a pas été trouvé.");
                    throw new Exception("Le type de client spécifié n'existe pas.");
                }

                _logger.LogInformation($"TypeClient trouvé: {typeClient.Libelle}");

                clientDTO.TypeClientLibelle = typeClient.Libelle;
                var client = _mapper.Map<Client>(clientDTO);
                client.TypeClient = typeClient;

                await _repository.AddAsync(client);
                await _repository.SaveChangesAsync();

                _logger.LogInformation($"Client {clientDTO.NomClient} ajouté avec succès.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de l'ajout du client: {clientDTO.NomClient}");
                throw;
            }
        }

        public async Task UpdateAsync(ClientDTO clientDTO)
        {
            var client = await _repository.GetByIdAsync(clientDTO.Id);
            if (client == null)
            {
                throw new Exception("Le client spécifié n'existe pas.");
            }

            _mapper.Map(clientDTO, client);
            await _repository.UpdateAsync(client);
        }

        public async Task DeleteAsync(int id)
        {
            var client = await _repository.GetByIdAsync(id);
            if (client == null)
            {
                throw new Exception("Le client spécifié n'existe pas.");
            }

            await _repository.DeleteAsync(client);
        }

        public async Task<IEnumerable<ClientDTO>> GetByTypeLibelleAsync(string typeLibelle)
        {
            var spec = new ClientByTypeLibelleSpecification(typeLibelle);
            var clients = await _repository.ListAsync(spec);
            return _mapper.Map<IEnumerable<ClientDTO>>(clients);
        }

        public async Task<IEnumerable<ClientDTO>> GetByAdresseAsync(string adresse)
        {
            var spec = new ClientByAdresseSpecification(adresse);
            var clients = await _repository.ListAsync(spec);
            return _mapper.Map<IEnumerable<ClientDTO>>(clients);
        }

        public async Task<IEnumerable<ClientDTO>> GetMagazinInterneClientsAsync()
        {
            var clients = await _repository.ListAsync(query => query
                .Include(c => c.TypeClient)
                .Include(c => c.Distributions)
                    .ThenInclude(d => d.DistributionDetails)
                        .ThenInclude(dd => dd.ArrivageDetails)
                            .ThenInclude(ad => ad.Article)
                .Where(c => c.TypeClient.Libelle == "MagazinInterne"));
                _logger.LogInformation("Nombre de clients de type MagazinInterne récupérés: {Count}", clients.Count());

            var clientDTOs = _mapper.Map<IEnumerable<ClientDTO>>(clients);

            return clientDTOs;
        }




public async Task<IEnumerable<ArticleDTO>> GetArticlesWithQuantitiesAsync()
{
    // Récupérer les détails de distribution pour les clients de type "MagazinInterne"
    var distributionDetails = await _distributionDetailsRepository.ListAsync(query => query
        .Include(dd => dd.ArrivageDetails)
            .ThenInclude(ad => ad.Article)
            .ThenInclude(a => a.Marque)
        .Include(dd => dd.ArrivageDetails.Article.Type)
        .Include(dd => dd.ArrivageDetails.Article.Fournisseur)
        .Include(dd => dd.ArrivageDetails.Article.Ventes)
        .Include(dd => dd.Distribution.Client)
        .Where(dd => dd.Distribution.Client.TypeClient.Libelle == "MagazinInterne"));

    // Grouper les articles par MarqueId, TypeId, et ClientId
    var groupedArticles = distributionDetails
        .Where(dd => dd.ArrivageDetails != null && dd.ArrivageDetails.Article != null)
        .GroupBy(dd => new { 
            
            dd.ArrivageDetails.Article.MarqueId, 
            dd.ArrivageDetails.Article.TypeId, 
            dd.Distribution.Client.Id 
        })
        .Select(group => new ArticleDTO
        {
            Id = group.FirstOrDefault()?.ArrivageDetails?.Article?.Id ?? 0,
            Description = group.FirstOrDefault()?.ArrivageDetails?.Article?.Description ?? "Description inconnue",
            PrixDeVenteInter = group.FirstOrDefault()?.ArrivageDetails?.Article?.PrixDeVenteInter ?? 0m,
            PrixDeVenteRevendeur = group.FirstOrDefault()?.ArrivageDetails?.Article?.PrixDeVenteRevendeur ?? 0m,
            PrixDeVenteClientFinal = group.FirstOrDefault()?.ArrivageDetails?.Article?.PrixDeVenteClientFinal ?? 0m,
            MarqueId = group.Key.MarqueId,
            MarqueLibelle = group.FirstOrDefault()?.ArrivageDetails?.Article?.Marque?.Libelle ?? "Marque inconnue",
            TypeId = group.Key.TypeId,
            TypeLibelle = group.FirstOrDefault()?.ArrivageDetails?.Article?.Type?.Libelle ?? "Type inconnu",
            Couleur = group.FirstOrDefault()?.ArrivageDetails?.Article?.Couleur,
            FournisseurId = group.FirstOrDefault()?.ArrivageDetails?.Article?.FournisseurId ?? 0,
            FournisseurNom = group.FirstOrDefault()?.ArrivageDetails?.Article?.Fournisseur?.NomFournisseur ?? "Fournisseur inconnu",
            Reference = group.FirstOrDefault()?.ArrivageDetails?.Article?.Reference,
            QuantiteDistribuee = group.Sum(dd => dd.Quantite), // Somme des quantités distribuées pour le groupe
            ClientId = group.Key.Id,
            ClientNom = group.FirstOrDefault()?.Distribution?.Client?.NomClient ?? "Client inconnu",
            TypeClientLibelle = group.FirstOrDefault()?.Distribution?.Client?.TypeClient?.Libelle ?? "Type inconnu",
           /* QuantiteRestante = group.Sum(dd => dd.Quantite) 
    - group.Sum(dd => dd.ArrivageDetails.Article.Ventes
        .Where(v => v.ClientId == group.Key.Id) // Filtrer uniquement les ventes pour ce client
        .Sum(v => v.QuantiteVendu)),*/ // Calcul de la somme des quantités vendues

        QuantiteRestante = group.Sum(dd => dd.Quantite) 
    - (group.FirstOrDefault()?.ArrivageDetails?.Article?.Ventes
        .Where(v => v.ClientId == group.Key.Id) // Filtrer uniquement les ventes pour ce client
        .Sum(v => v.QuantiteVendu) ?? 0) // Assurez-vous que la somme est calculée correctement ou renvoie 0


            

        }).ToList();

    return groupedArticles;
}







    }
}
