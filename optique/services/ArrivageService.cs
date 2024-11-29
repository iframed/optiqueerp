using AutoMapper;
using MyAspNetApp.Repositories;
using optique.Dtos;
using optique.Models;
using optique.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using optique.Data;
using Microsoft.EntityFrameworkCore;

namespace optique.Services
{
    public class ArrivageService : IArrivageService
    {
        private readonly IRepository<Arrivage> _repository;
        private readonly IMapper _mapper;
         private readonly ApplicationDbContext _context;
         private readonly ILogger<ArrivageService> _logger;

        public ArrivageService(IRepository<Arrivage> repository, IMapper mapper, ApplicationDbContext context,ILogger<ArrivageService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

       


        public async Task<IEnumerable<ArrivageDTO>> GetAllAsync()
{
    var arrivages = await _context.Arrivages
    .Include(a => a.Fournisseur)
    .Include(a => a.Societe)
    .Include(a => a.RefStatutDistribution)
    .Select(a => new ArrivageDTO
    {
        Id = a.Id,
        DateArrivage = a.DateArrivage,
        FournisseurId = a.FournisseurId,
        FournisseurNom = a.Fournisseur.NomFournisseur,
        NumBL = a.NumBL,
        NumFacture = a.NumFacture,
        MontantFacture = a.MontantFacture,
        SocieteNom = a.Societe.NomSociete,
         SocieteId = a.SocieteId,
        StatutId = a.StatutId.HasValue ? a.StatutId.Value : 0,  // Vérification de null
        StatutDistributionLibelle = a.RefStatutDistribution != null 
            ? a.RefStatutDistribution.Libelle 
            : "Aucun Statut"  // Gestion des statuts nuls
    }).ToListAsync();

// Ajoutez un point d'arrêt ici ou déboguez pour vérifier les valeurs chargées.
foreach (var arrivage in arrivages)
{
    Console.WriteLine($"ID: {arrivage.Id}, Statut: {arrivage.StatutDistributionLibelle}");
}


    return arrivages;
}


     

        public async Task<ArrivageDTO?> GetByIdAsync(int id)
{
    var arrivage = await _context.Arrivages
        .Include(a => a.DetailsPaiements) // Inclure les détails de paiement
   
        .ThenInclude(dp => dp.TypeDePaiement) // Inclure le type de paiement
        //.Include(a => a.Societe) // Inclure la société
        .Include(a => a.Fournisseur) 
        .Include(a => a.Societe)
        .FirstOrDefaultAsync(a => a.Id == id);

    if (arrivage == null)
    {
        return null;
    }

    var arrivageDTO = _mapper.Map<ArrivageDTO>(arrivage);
    arrivageDTO.SocieteNom = arrivage.Societe.NomSociete; // Remplir le nom de la société
    arrivageDTO.FournisseurNom = arrivage.Fournisseur.NomFournisseur; // Remplir le nom du fournisseur


    return arrivageDTO;
}



        

        public async Task<int> AddAsync(ArrivageDTO dto)
        {
            var newArrivage = new Arrivage
            {
                DateArrivage = DateTime.Now,
                SocieteId = dto.SocieteId,
                FournisseurId = dto.FournisseurId,
                NumBL = dto.NumBL,
                NumFacture = dto.NumFacture,
                MontantFacture = dto.MontantFacture,
                StatutId = dto.StatutId 
                
            };

            await _repository.AddAsync(newArrivage);

            // Sauvegarder les modifications et obtenir l'ID
            await _repository.SaveChangesAsync(); // Assurez-vous d'avoir une méthode SaveChangesAsync dans votre repository

            return newArrivage.Id; // Retourner l'ID de l'arrivage nouvellement créé
        }

        public async Task UpdateAsync(ArrivageDTO dto)
        {
            var arrivage = await _repository.GetByIdAsync(dto.Id);
            if (arrivage == null)
            {
                throw new Exception("Arrivage not found");
            }

            _mapper.Map(dto, arrivage);
            await _repository.UpdateAsync(arrivage);
        }

        public async Task DeleteAsync(int id)
        {
            var arrivage = await _repository.GetByIdAsync(id);
            if (arrivage == null)
            {
                throw new Exception("Arrivage not found");
            }

            await _repository.DeleteAsync(arrivage);
        }

      public async Task AjouterPaiementPourArrivage(int arrivageId, DetailsPaiementDTO paiementDto)
{
    var arrivage = await _repository.GetByIdAsync(arrivageId);
    if (arrivage == null)
    {
        throw new Exception("Arrivage non trouvé");
    }

    var paiement = _mapper.Map<DetailsPaiement>(paiementDto);
    paiement.ArrivageId = arrivageId;

    _context.DetailsPaiements.Add(paiement);
    await _context.SaveChangesAsync();
}

        public async Task<List<DetailsPaiementDTO>> GetDetailsPaiementsByArrivageId(int arrivageId)
{
    return await _context.DetailsPaiements
        .Where(dp => dp.ArrivageId == arrivageId) // Assurez-vous que vous avez cette relation
        .Select(dp => new DetailsPaiementDTO
        {
            TypeDePaiementId = dp.TypeDePaiementId,
            NCheque = dp.NCheque,
            NLCN = dp.NLCN,
            Montant = dp.Montant,
            DateEcheance = dp.DateEcheance
        })
        .ToListAsync();
}

       public async Task<bool> ValiderArrivage(int arrivageId)
{
    var arrivage = await _context.Arrivages.FindAsync(arrivageId);
    if (arrivage == null)
    {
        _logger.LogWarning($"Arrivage with ID {arrivageId} not found.");
        return false;
    }

    // Log avant de changer le statut
    _logger.LogInformation($"Validating arrivage with ID {arrivageId}.");

    arrivage.StatutId = 1;
    
    await _context.SaveChangesAsync();
     
    return true;
}


    }
}
