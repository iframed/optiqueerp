using optique.Dtos;
using optique.IServices;
using optique.Models;
using MyAspNetApp.Repositories;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using optique.Data;
using System;
using optique.ViewModels;

namespace optique.Services
{
    public class VenteService : IVenteService
    {
        private readonly IRepository<Vente> _repository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VenteService> _logger;

        public VenteService(IRepository<Vente> repository, IMapper mapper, ApplicationDbContext context, ILogger<VenteService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

       

     public async Task<IEnumerable<VenteDetailsDTO>> GetAllAsync()
{
    var ventes = await _context.Ventes
        .Include(v => v.Client)
        .Include(v => v.ClientAcheteur) 
        .Include(v => v.Article)
            .ThenInclude(a => a.Marque)
        .Include(v => v.Article)
            .ThenInclude(a => a.Type)
        .Include(v => v.DetailsPaiements)
            .ThenInclude(dp => dp.TypeDePaiement)
        .ToListAsync();

    // Utilisation de SelectMany pour obtenir chaque paiement sur une ligne séparée
    var venteDetailsDTOs = ventes.SelectMany(vente => vente.DetailsPaiements.Select(dp => new VenteDetailsDTO
    {
        DateDeVente = vente.DateDeVente,
        Client = vente.Client.NomClient,
        ClientAcheteur = vente.ClientAcheteur?.NomClient,
        Marque = vente.Article.Marque.Libelle,
        Reference = vente.Article.Reference,
        PrixDeVente = vente.PrixDeVente,
  
        TypeArticle = vente.Article.Type?.Libelle,
        Quantite = vente.QuantiteVendu,
        TypeDePaiement = dp.TypeDePaiement?.Libelle,
        DetailsPaiement = dp.NCheque
    })).ToList();

    return venteDetailsDTOs;
}



        public async Task<VenteDetailsDTO?> GetByIdAsync(int id)
        {
            var vente = await _context.Ventes
                .Include(v => v.Client)
                .Include(v => v.Article)
                    .ThenInclude(a => a.Marque)
                .Include(v => v.Article)
                    .ThenInclude(a => a.Type)
                .Include(v => v.DetailsPaiements)
                    .ThenInclude(dp => dp.TypeDePaiement)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vente == null)
                return null;

            var venteDetailsDTO = new VenteDetailsDTO
            {
                DateDeVente = vente.DateDeVente,
                Client = vente.Client.NomClient,
                Marque = vente.Article.Marque.Libelle,
                Reference = vente.Article.Reference,
                PrixDeVente = vente.PrixDeVente,
                TypeArticle = vente.Article.Type?.Libelle,
                Quantite = vente.QuantiteVendu,
              
               DetailsPaiements = vente.DetailsPaiements.Select(dp => new DetailsPaiementDTO
        {
            TypeDePaiement = dp.TypeDePaiement?.Libelle,
            NCheque = dp.NCheque
        }).ToList()
            };

            return venteDetailsDTO;
        }

      

    











// Méthode supplémentaire pour mettre à jour les détails de paiement
public async Task UpdateDetailsPaiementsAsync(List<DetailsPaiementDTO> detailsPaiements)
{
    foreach (var detailsPaiementDTO in detailsPaiements)
    {
        var detailsPaiement = await _context.DetailsPaiements.FindAsync(detailsPaiementDTO.Id);
        if (detailsPaiement != null)
        {
            detailsPaiement.VenteId = detailsPaiementDTO.VenteId;
            _context.Entry(detailsPaiement).State = EntityState.Modified;
        }
    }
    await _context.SaveChangesAsync();
}

        public async Task UpdateAsync(VenteDTO venteDTO)
        {
            var vente = await _repository.GetByIdAsync(venteDTO.Id);
            if (vente == null)
            {
                throw new Exception("La vente spécifiée n'existe pas.");
            }

            _mapper.Map(venteDTO, vente);

            foreach (var detailsPaiementDTO in venteDTO.DetailsPaiements)
            {
                var detailsPaiement = _mapper.Map<DetailsPaiement>(detailsPaiementDTO);
                vente.DetailsPaiements.Add(detailsPaiement);
            }

            await _repository.UpdateAsync(vente);
        }

        public async Task DeleteAsync(int id)
        {
            var vente = await _repository.GetByIdAsync(id);
            if (vente == null)
            {
                throw new Exception("La vente spécifiée n'existe pas.");
            }

            await _repository.DeleteAsync(vente);
        }

       

        public async Task<IEnumerable<VenteDetailsDTO>> SearchAsync(string? client, string? marque, string? typeArticle, string? reference)
{
    var query = _context.Ventes
        .Include(v => v.Client)
        .Include(v => v.Article)
            .ThenInclude(a => a.Marque)
        .Include(v => v.Article)
            .ThenInclude(a => a.Type)
        .Include(v => v.DetailsPaiements)
            .ThenInclude(dp => dp.TypeDePaiement)
        .AsQueryable();

    if (!string.IsNullOrEmpty(client))
    {
        query = query.Where(v => v.Client.NomClient == client);
    }

    if (!string.IsNullOrEmpty(marque))
    {
        query = query.Where(v => v.Article.Marque.Libelle == marque);
    }

    if (!string.IsNullOrEmpty(typeArticle))
    {
        query = query.Where(v => v.Article.Type.Libelle == typeArticle);
    }

    if (!string.IsNullOrEmpty(reference))
    {
        query = query.Where(v => v.Article.Reference == reference);
    }

    var ventes = await query.ToListAsync();

    var venteDetailsDTOs = ventes.Select(vente => new VenteDetailsDTO
    {
        DateDeVente = vente.DateDeVente,
        Client = vente.Client.NomClient,
        Marque = vente.Article.Marque.Libelle,
        Reference = vente.Article.Reference,
        PrixDeVente = vente.PrixDeVente,
        TypeArticle = vente.Article.Type?.Libelle,
        Quantite = vente.QuantiteVendu,
        TypeDePaiement = vente.DetailsPaiements.FirstOrDefault()?.TypeDePaiement?.Libelle,
        DetailsPaiement = vente.DetailsPaiements.FirstOrDefault()?.NCheque
    }).ToList();

    return venteDetailsDTOs;
}

public async Task<IEnumerable<ChequeDueDateDTO>> GetChequesByDateAsync(DateTime date)
{
    var cheques = await _context.DetailsPaiements
        .Include(dp => dp.Vente)
        .ThenInclude(v => v.Client)
        .Where(dp => dp.DateEcheance.HasValue && dp.DateEcheance.Value.Date == date.Date && dp.TypeDePaiement.Libelle == "Chèque")
        .Select(dp => new ChequeDueDateDTO
        {
            ClientName = dp.Vente.Client.NomClient,
            ClientAcheteurName = dp.Vente.ClientAcheteur != null ? dp.Vente.ClientAcheteur.NomClient : "N/A", // Ensure this part correctly fetches the acheteur
            DateEcheance = dp.DateEcheance,
            NCheque = dp.NCheque,
            Amount = dp.Montant
        })
        .ToListAsync();

    return cheques;
}

   




public async Task<IEnumerable<ChequeDueDateDTO>> GetChequeDueDatesAsync(int month, int year, string view)
{
    _logger.LogDebug("Fetching cheques due dates from the database.");

 _logger.LogDebug("Fetching client cheques for month: {Month}, year: {Year}, view: {View}", month, year, view);


        IQueryable<DetailsPaiement> query = _context.DetailsPaiements
        .Include(dp => dp.Vente)
        .ThenInclude(v => v.Client)
        .Include(dp => dp.Vente)
        .ThenInclude(v => v.ClientAcheteur) // Inclure le client acheteur
        .Where(dp => dp.TypeDePaiement.Libelle == "Chèque")
        .Where(dp => dp.Vente.Client != null && dp.Arrivage == null); // S'assurer que ce sont des ventes et non des arrivages de fournisseurs

    if (view == "week")
    {
        // Calcul de la date actuelle
        DateTime currentDate = DateTime.Now;

        // Trouver le premier jour de la semaine (dimanche) et le dernier jour (samedi)
        DateTime startOfWeek = currentDate.AddDays(-(int)currentDate.DayOfWeek);
        DateTime endOfWeek = startOfWeek.AddDays(6);

        // Debug: log start and end of the week
        _logger.LogDebug("Week view: StartOfWeek = {StartOfWeek}, EndOfWeek = {EndOfWeek}", startOfWeek, endOfWeek);

        // Filtrer les chèques dont la date d'échéance est dans la semaine actuelle
        query = query.Where(dp => dp.DateEcheance.HasValue && 
                                  dp.DateEcheance.Value >= startOfWeek && 
                                  dp.DateEcheance.Value <= endOfWeek);
    }
    else
    {
        // Filtrer les chèques par mois
        query = query.Where(dp => dp.DateEcheance.HasValue && 
                                  dp.DateEcheance.Value.Month == month && 
                                  dp.DateEcheance.Value.Year == year);
    }

    // Exécution de la requête pour récupérer les chèques
    var cheques = await query
        .Select(dp => new ChequeDueDateDTO
        {
            ClientName = dp.Vente.Client.NomClient,
            ClientAcheteurName = dp.Vente.ClientAcheteur != null ? dp.Vente.ClientAcheteur.NomClient : "N/A", // Nom de l'acheteur si existant
            DateEcheance = dp.DateEcheance,
            NCheque = dp.NCheque,
            Amount = dp.Montant,
            IsFromFournisseur = false
        })
        .ToListAsync();


    _logger.LogDebug("Fetched {Count} cheques.", cheques.Count);

    return cheques;
}


public async Task<IEnumerable<ChequeDueDateDTO>> GetFournisseurChequeDueDatesAsync(int month, int year, string view)
{
    IQueryable<DetailsPaiement> query = _context.DetailsPaiements
        .Include(dp => dp.Arrivage)
        .ThenInclude(a => a.Fournisseur)
        .Where(dp => dp.TypeDePaiement.Libelle == "Chèque")
         .Where(dp => dp.Arrivage.Fournisseur != null);  // Assurez-vous que le fournisseur est correctement filtré

    // Filtrer par mois et année
    query = query.Where(dp => dp.DateEcheance.HasValue && dp.DateEcheance.Value.Month == month && dp.DateEcheance.Value.Year == year);

    var cheques = await query
        .Select(dp => new ChequeDueDateDTO
        {
            FournisseurName = dp.Arrivage.Fournisseur.NomFournisseur,
            DateEcheance = dp.DateEcheance,
            NCheque = dp.NCheque,
            Amount = dp.Montant,  // Le montant est correct ici pour les fournisseurs
            IsFromFournisseur = true
        })
        .ToListAsync();
        _logger.LogDebug("Fournisseur cheques fetched: {Count}", cheques.Count);



    return cheques;
}




        public async Task AddAsync(VenteDTO venteDTO, string userName)
{
    _logger.LogDebug("Début de AddAsync pour Vente avec ArticleId: {ArticleId}, ClientId: {ClientId}, VenteDTO: {@VenteDTO}", venteDTO.ArticleId, venteDTO.ClientId, venteDTO);

    using var transaction = await _context.Database.BeginTransactionAsync();

    try
    {
        // Vérification de la validité de la collection de détails de paiement
        if (venteDTO.DetailsPaiements == null || !venteDTO.DetailsPaiements.Any())
        {
            _logger.LogWarning("La collection DetailsPaiements est null ou vide pour Vente avec ArticleId: {ArticleId}, ClientId: {ClientId}", venteDTO.ArticleId, venteDTO.ClientId);
            throw new InvalidOperationException("Aucun détail de paiement fourni.");
        }

        var vente = _mapper.Map<Vente>(venteDTO);
        vente.CreePar = userName;

        _context.Ventes.Add(vente);
        await _context.SaveChangesAsync(); // Sauvegarder pour obtenir le VenteId

        _logger.LogDebug("Vente ajoutée avec succès avec ID {VenteId}", vente.Id);

        // Vérifier les quantités disponibles en stock
        var totalQuantiteEnStock = await _context.DistributionDetails
            .Include(dd => dd.Distribution)
            .ThenInclude(d => d.Client)
            .Where(dd => dd.ArrivageDetails.ArticleId == vente.ArticleId && dd.Distribution.Client.TypeClient.Libelle == "MagazinInterne")
            .SumAsync(dd => dd.Quantite);

        _logger.LogDebug("Quantité totale en stock pour l'article ID {ArticleId}: {TotalQuantiteEnStock}", vente.ArticleId, totalQuantiteEnStock);

        if (totalQuantiteEnStock < vente.QuantiteVendu)
        {
            _logger.LogWarning("Quantité insuffisante en stock pour l'article ID {ArticleId}. Disponible: {Disponible}, Requis: {Requis}", vente.ArticleId, totalQuantiteEnStock, vente.QuantiteVendu);
            throw new InvalidOperationException("La quantité disponible dans le stock global est insuffisante pour la vente.");
        }

        // Diminuer la quantité en stock par la quantité vendue
        var distributionDetails = await _context.DistributionDetails
            .Include(dd => dd.Distribution)
            .ThenInclude(d => d.Client)
            .Where(dd => dd.ArrivageDetails.ArticleId == vente.ArticleId && dd.Distribution.Client.TypeClient.Libelle == "MagazinInterne")
            .ToListAsync();

        int quantiteRestante = vente.QuantiteVendu;

        foreach (var detail in distributionDetails)
        {
            if (quantiteRestante <= 0)
                break;

            if (detail.Quantite >= quantiteRestante)
            {
                _logger.LogDebug("Réduction de la quantité pour DistributionDetails ID {DistributionDetailsId}. Quantité avant: {Avant}, Réduite de: {Réduction}", detail.Id, detail.Quantite, quantiteRestante);
                detail.Quantite -= quantiteRestante;
                quantiteRestante = 0;
            }
            else
            {
                _logger.LogDebug("Quantité insuffisante dans DistributionDetails ID {DistributionDetailsId}. Quantité avant: {Avant}, Mise à zéro", detail.Id, detail.Quantite);
                quantiteRestante -= detail.Quantite;
                detail.Quantite = 0;
            }

            _context.Entry(detail).State = EntityState.Modified;
        }

        // Associer le VenteId à chaque DétailsPaiement et ajouter à la base de données
        foreach (var detailsPaiementDTO in venteDTO.DetailsPaiements)
        {
            var detailsPaiement = _mapper.Map<DetailsPaiement>(detailsPaiementDTO);
            if (detailsPaiement != null)
            {
                detailsPaiement.VenteId = vente.Id; // Associer le VenteId
                _context.DetailsPaiements.Add(detailsPaiement);
            }
        }

        await _context.SaveChangesAsync(); // Sauvegarder les détails de paiement

        // Création et ajout du mouvement d'article
        var mouvementArticle = new MouvementArticle
        {
            VenteId = vente.Id,
            Quantite = vente.QuantiteVendu,
            DateDeCreation = DateTime.UtcNow,
            CreePar = userName
        };

        _context.MouvementArticles.Add(mouvementArticle);
        await _context.SaveChangesAsync(); // Sauvegarder le MouvementArticle après avoir ajouté la vente

        await transaction.CommitAsync();

        _logger.LogInformation("Vente et détails de paiement ajoutés avec succès pour VenteId {VenteId}", vente.Id);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Erreur lors de l'ajout de la vente");
        await transaction.RollbackAsync();
        throw;
    }
}










public async Task<bool> CreateVenteAsync(NouvelleVenteViewModel venteVm)
{
    // Vérifiez si CreePar est null ou vide
    if (string.IsNullOrEmpty(venteVm.CreePar))
    {
        _logger.LogWarning("Le champ CreePar est vide ou nul.");
        return false; // Retournez false si CreePar est manquant
    }

    // Récupérer l'article, le client, et le client acheteur
    var article = await _context.Articles.FindAsync(venteVm.ArticleId);
    var client = await _context.Clients.FindAsync(venteVm.ClientId);
    var clientAcheteur = venteVm.ClientIdAcheteur.HasValue ? await _context.Clients.FindAsync(venteVm.ClientIdAcheteur.Value) : null;

    if (article == null || client == null)
    {
        _logger.LogWarning("Article ou client introuvable.");
        return false;
    }

    // Si un client acheteur (particulier) est spécifié, vérifiez qu'il existe
    if (venteVm.ClientIdAcheteur.HasValue && clientAcheteur == null)
    {
        _logger.LogWarning("Client acheteur introuvable avec l'ID {ClientIdAcheteur}.", venteVm.ClientIdAcheteur);
        return false;
    }

    // Calculer la quantité disponible pour ce client à partir des distributions et des ventes
    var distributions = await _context.DistributionDetails
        .Include(dd => dd.ArrivageDetails)
        .Include(dd => dd.Distribution)
        .Where(dd => dd.ArrivageDetails.ArticleId == venteVm.ArticleId && dd.Distribution.ClientId == venteVm.ClientId)
        .ToListAsync();

    var totalDistribue = distributions.Sum(dd => dd.Quantite);
    
    // Calculer les ventes déjà effectuées pour cet article et ce client
    var ventes = await _context.Ventes
        .Where(v => v.ArticleId == venteVm.ArticleId && v.ClientId == venteVm.ClientId)
        .ToListAsync();

    var totalVendu = ventes.Sum(v => v.QuantiteVendu);

    // Calculer la quantité disponible
    var quantiteDisponible = totalDistribue - totalVendu;

    // Vérification si la quantité vendue est égale ou inférieure à la quantité disponible
    if (venteVm.QuantiteVendu > quantiteDisponible)
    {
        _logger.LogWarning("La quantité vendue dépasse la quantité disponible en stock.");
        return false;
    }

    // Créer la vente
    var vente = new Vente
    {
        DateDeVente = venteVm.DateDeVente,
        PrixDeVente = venteVm.PrixDeVente,
        QuantiteVendu = venteVm.QuantiteVendu,
        ArticleId = venteVm.ArticleId,
         NDeBon = venteVm.NDeBon,
        ClientId = venteVm.ClientId,
        ClientIdAcheteur = venteVm.ClientIdAcheteur, // Client acheteur (particulier)
        CreePar = venteVm.CreePar,
        MouvementArticles = new List<MouvementArticle>(),
        Article = article,
        Client = client,
        ClientAcheteur = clientAcheteur, // Lier le client acheteur si défini
        DetailsPaiements = new List<DetailsPaiement>()
    };

    // Ajouter les paiements
    foreach (var dpVm in venteVm.DetailsPaiements)
    {
        var typeDePaiement = await _context.RefTypeDePaiements.FindAsync(dpVm.TypeDePaiementId);
        if (typeDePaiement == null)
        {
            _logger.LogWarning("Type de paiement introuvable pour l'ID {TypeDePaiementId}.", dpVm.TypeDePaiementId);
            return false;
        }

        if (typeDePaiement.Libelle == "Chèque" && string.IsNullOrEmpty(dpVm.NCheque))
        {
            _logger.LogWarning("Le champ NCheque est requis pour les paiements par chèque.");
            return false;
        }

        var detailsPaiement = new DetailsPaiement
        {
            NCheque = dpVm.NCheque,
            NLCN = dpVm.NLCN,
            DateEcheance = dpVm.DateEcheance ?? DateTime.MinValue,
            Montant = dpVm.Montant,
            IsDeleted = dpVm.IsDeleted,
            TypeDePaiementId = dpVm.TypeDePaiementId,
            TypeDePaiement = typeDePaiement,
            Vente = vente
        };

        vente.DetailsPaiements.Add(detailsPaiement);
    }

    _context.Ventes.Add(vente);

    try
    {
        // Sauvegarder la vente
        var saveResult = await _context.SaveChangesAsync();
        if (saveResult > 0)
        {
            // Ajouter le mouvement après la vente
            var mouvementArticle = new MouvementArticle
            {
                VenteId = vente.Id,
                Quantite = venteVm.QuantiteVendu,
                DateDeCreation = DateTime.UtcNow,
                CreePar = venteVm.CreePar
            };

            _context.MouvementArticles.Add(mouvementArticle);

            // Sauvegarder le mouvement
            return await _context.SaveChangesAsync() > 0;
        }
        return false;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Erreur lors de l'ajout de la vente");
        return false;
    }
}

public async Task<IEnumerable<ChequeDueDateDTO>> GetFournisseurChequesByDateAsync(DateTime date)
{
    var cheques = await _context.DetailsPaiements
        .Include(dp => dp.Arrivage)
        .ThenInclude(a => a.Fournisseur)
        .Where(dp => dp.DateEcheance.HasValue && dp.DateEcheance.Value.Date == date.Date && dp.TypeDePaiement.Libelle == "Chèque")
        .Select(dp => new ChequeDueDateDTO
        {
            FournisseurName = dp.Arrivage.Fournisseur.NomFournisseur,
            DateEcheance = dp.DateEcheance,
            NCheque = dp.NCheque,
            Amount = dp.Montant,
            IsFromFournisseur = true
        })
        .ToListAsync();

    return cheques;
}

   



       public async Task<IEnumerable<ChequeDueDateDTO>> GetFournisseurLCNDueDatesAsync(int month, int year, string view)
{
    IQueryable<DetailsPaiement> query = _context.DetailsPaiements
        .Include(dp => dp.Arrivage)
        .ThenInclude(a => a.Fournisseur)
        .Where(dp => dp.TypeDePaiement.Libelle == "LCN" ) // Filtrer pour les LCN
        .Where(dp => dp.Arrivage.Fournisseur != null);  // Filtrer les LCN des fournisseurs

    if (view == "week")
    {
        DateTime currentDate = DateTime.Now;
        DateTime startOfWeek = currentDate.AddDays(-(int)currentDate.DayOfWeek);
        DateTime endOfWeek = startOfWeek.AddDays(6);

        query = query.Where(dp => dp.DateEcheance.HasValue && 
                                  dp.DateEcheance.Value >= startOfWeek && 
                                  dp.DateEcheance.Value <= endOfWeek);
    }
    else
    {
        query = query.Where(dp => dp.DateEcheance.HasValue && 
                                  dp.DateEcheance.Value.Month == month && 
                                  dp.DateEcheance.Value.Year == year);
    }

    var fournisseurLCNDueDates = await query
        .Select(dp => new ChequeDueDateDTO
        {
            FournisseurName = dp.Arrivage.Fournisseur.NomFournisseur,
            DateEcheance = dp.DateEcheance,
            NLCN = dp.NLCN, // Utiliser NLCN pour les LCN
            Amount = dp.Montant,
            IsFromFournisseur = true
        })
        .ToListAsync();

    return fournisseurLCNDueDates;
}

public async Task<IEnumerable<ChequeDueDateDTO>> GetLCNDueDatesByDayAsync(DateTime date)
{
    // Filtrer les LCN pour la date exacte
    var lcnDueDates = await _context.DetailsPaiements
        .Include(dp => dp.Vente)
        .ThenInclude(v => v.Client)
        .Include(dp => dp.Vente)
        .ThenInclude(v => v.ClientAcheteur)
        
        .Where(dp => dp.TypeDePaiement.Libelle == "LCN" && dp.DateEcheance.HasValue && dp.DateEcheance.Value.Date == date.Date)
        .Select(dp => new ChequeDueDateDTO
        {
            ClientName = dp.Vente.Client.NomClient,
            ClientAcheteurName = dp.Vente.ClientAcheteur != null ? dp.Vente.ClientAcheteur.NomClient : "NULL",
            DateEcheance = dp.DateEcheance,
            NLCN = dp.NLCN, 
            Amount = dp.Montant
        })
        .ToListAsync();

    return lcnDueDates;
}





       public async Task<IEnumerable<ChequeDueDateDTO>> GetLCNDueDatesAsync(int month, int year, string view, int? day = null)
{
    IQueryable<DetailsPaiement> query = _context.DetailsPaiements
        .Include(dp => dp.Vente)
        .ThenInclude(v => v.Client)
        .Include(dp => dp.Vente)
        .ThenInclude(v => v.ClientAcheteur)
        .Where(dp => dp.TypeDePaiement.Libelle == "LCN")
        .Where(dp => dp.Vente.Client != null && dp.Arrivage == null);

    if (view == "day" && day.HasValue)
    {
        // Filtrer les LCN pour le jour spécifique
        query = query.Where(dp => dp.DateEcheance.HasValue 
                                   && dp.DateEcheance.Value.Month == month 
                                   && dp.DateEcheance.Value.Year == year
                                   && dp.DateEcheance.Value.Day == day.Value);
    }
    else if (view == "week")
    {
        DateTime currentDate = DateTime.Now;
        DateTime startOfWeek = currentDate.AddDays(-(int)currentDate.DayOfWeek);
        DateTime endOfWeek = startOfWeek.AddDays(6);

        query = query.Where(dp => dp.DateEcheance.HasValue &&
                                  dp.DateEcheance.Value >= startOfWeek &&
                                  dp.DateEcheance.Value <= endOfWeek);
    }
    else
    {
        query = query.Where(dp => dp.DateEcheance.HasValue && dp.DateEcheance.Value.Month == month && dp.DateEcheance.Value.Year == year);
    }

    var lcnDueDates = await query
        .Select(dp => new ChequeDueDateDTO
        {
            ClientName = dp.Vente.Client.NomClient,
            ClientAcheteurName = dp.Vente.ClientAcheteur != null ? dp.Vente.ClientAcheteur.NomClient : "N/A",
            DateEcheance = dp.DateEcheance,
            NLCN = dp.NLCN,
            Amount = dp.Montant,
            IsFromFournisseur = false
        })
        .ToListAsync();

    return lcnDueDates;
}

        public async Task<IEnumerable<ChequeDueDateDTO>> GetFournisseurLCNDueDatesByExactDateAsync(DateTime date)
{
    // Filtrer les LCN pour la date d'échéance exacte
    var fournisseurLCNDueDates = await _context.DetailsPaiements
        .Include(dp => dp.Arrivage)
        .ThenInclude(a => a.Fournisseur)
        .Where(dp => dp.TypeDePaiement.Libelle == "LCN" && dp.DateEcheance.HasValue && dp.DateEcheance.Value.Date == date.Date)
        .Select(dp => new ChequeDueDateDTO
        {
            FournisseurName = dp.Arrivage.Fournisseur.NomFournisseur,
            DateEcheance = dp.DateEcheance,
            NLCN = dp.NLCN, // Numéro LCN
            Amount = dp.Montant,
            IsFromFournisseur = true 
        })
        .ToListAsync();

    return fournisseurLCNDueDates;
}

        
    }
}