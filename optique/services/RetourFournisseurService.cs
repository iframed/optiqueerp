using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyAspNetApp.Repositories;
using optique.Data;
using optique.Dtos;
using optique.Models;
using optique.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.Services
{
    public class RetourFournisseurService : IRetourFournisseurService
    {
        private readonly IRepository<RetourFournisseur> _repository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public RetourFournisseurService(IRepository<RetourFournisseur> repository, IMapper mapper, ApplicationDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<RetourFournisseurDTO>> GetAllAsync()
        {
            var retourFournisseurs = await _repository.ListAsync();
            return _mapper.Map<IEnumerable<RetourFournisseurDTO>>(retourFournisseurs);
        }

        public async Task<RetourFournisseurDTO?> GetByIdAsync(int id)
        {
            var retourFournisseur = await _repository.GetByIdAsync(id);
            return _mapper.Map<RetourFournisseurDTO?>(retourFournisseur);
        }

        
        public async Task AddAsync(RetourFournisseurDTO retourFournisseurDTO, string userName)
{
    var retourFournisseur = _mapper.Map<RetourFournisseur>(retourFournisseurDTO);
    retourFournisseur.CreePar = userName;

    // If DateRetour is not set in the DTO, set it to the current date
    if (retourFournisseur.DateRetour == default(DateTime))
    {
        retourFournisseur.DateRetour = DateTime.UtcNow;
    }

    await _repository.AddAsync(retourFournisseur);

    // Add MouvementArticle
    var mouvementArticle = new MouvementArticle
    {
        RetourFournisseurId = retourFournisseur.Id,
        Quantite = retourFournisseur.QuantiteRetournee,
        DateDeCreation = DateTime.UtcNow,
        CreePar = userName
    };
    _context.MouvementArticles.Add(mouvementArticle);
    await _context.SaveChangesAsync();
}


        public async Task UpdateAsync(RetourFournisseurDTO retourFournisseurDTO)
        {
            var retourFournisseur = await _repository.GetByIdAsync(retourFournisseurDTO.Id);
            if (retourFournisseur == null)
            {
                throw new Exception("Le retour fournisseur spécifié n'existe pas.");
            }

            _mapper.Map(retourFournisseurDTO, retourFournisseur);
            await _repository.UpdateAsync(retourFournisseur);
        }

        public async Task DeleteAsync(int id)
        {
            var retourFournisseur = await _repository.GetByIdAsync(id);
            if (retourFournisseur == null)
            {
                throw new Exception("Le retour fournisseur spécifié n'existe pas.");
            }

            await _repository.DeleteAsync(retourFournisseur);
        }

        public async Task<IEnumerable<FournisseurDetailsDTO>> GetFournisseurDetailsAsync()
        {
            var arrivages = await _context.Arrivages
                .Include(a => a.Societe)
                .Include(a => a.Fournisseur)
                .Include(a => a.ArrivageDetails)
                    .ThenInclude(ad => ad.Article)
                        .ThenInclude(ar => ar.Marque)
                .Include(a => a.ArrivageDetails)
                    .ThenInclude(ad => ad.RetourFournisseurs!)
                        .ThenInclude(rf => rf.TypeRetour)
                .ToListAsync();

            var result = arrivages.SelectMany(a => a.ArrivageDetails, (a, ad) => new { Arrivage = a, ArrivageDetails = ad })
               // .SelectMany(aad => aad.ArrivageDetails.RetourFournisseurs.Select(rf => new FournisseurDetailsDTO
               .SelectMany(aad => (aad.ArrivageDetails.RetourFournisseurs ?? Enumerable.Empty<RetourFournisseur>()).Select(rf => new FournisseurDetailsDTO
                {
                    Societe = aad.Arrivage.Societe.NomSociete,
                    Fournisseur = aad.Arrivage.Fournisseur.NomFournisseur,
                    NumFacture = aad.Arrivage.NumFacture,
                    NumBL = aad.Arrivage.NumBL,
                    DateArrivage = aad.Arrivage.DateArrivage,
                    DateRetour = rf.DateRetour,
                    Reference = aad.ArrivageDetails.Article.Reference,
                    QuantiteRetournee = rf.QuantiteRetournee,
                    Marque = aad.ArrivageDetails.Article.Marque.Libelle,
                    PrixDachatDeviseTTC = aad.ArrivageDetails.PrixDachatDevise,
                    PrixDachatMADTTC = aad.ArrivageDetails.PrixDachatMAD,
                    MotifRetour = rf.MotifRetour,
                    TypeRetour = rf.TypeRetour.Libelle
                }))
                .ToList();

            return result;
        }

       

         public async Task<IEnumerable<FournisseurDetailsDTO>> GetFournisseurDetailsByCriteriaAsync(string? societe, string? marque, string? reference, string? fournisseur)
{
    // Step 1: Start the query
    var query = _context.Arrivages
        .Include(a => a.Societe)
        .Include(a => a.Fournisseur)
        .Include(a => a.ArrivageDetails)
            .ThenInclude(ad => ad.Article)
                .ThenInclude(ar => ar.Marque)
        .Include(a => a.ArrivageDetails)
            .ThenInclude(ad => ad.RetourFournisseurs!)
            //.ThenInclude(ad => ad.RetourFournisseurs ?? Enumerable.Empty<RetourFournisseur>())

                .ThenInclude(rf => rf.TypeRetour)
        .AsQueryable();

    // Step 2: Apply filters
    if (!string.IsNullOrEmpty(societe))
    {
        query = query.Where(a => a.Societe != null && a.Societe.NomSociete.Trim() == societe.Trim());
    }

    if (!string.IsNullOrEmpty(fournisseur))
    {
        query = query.Where(a => a.Fournisseur != null && a.Fournisseur.NomFournisseur.Trim() == fournisseur.Trim());
    }

    if (!string.IsNullOrEmpty(marque))
    {
        query = query.Where(a => a.ArrivageDetails.Any(ad => ad.Article.Marque != null && ad.Article.Marque.Libelle.Trim() == marque.Trim()));
    }

    if (!string.IsNullOrEmpty(reference))
    {
        query = query.Where(a => a.ArrivageDetails.Any(ad => ad.Article.Reference.Trim() == reference.Trim()));
    }

    // Step 3: Execute the query
    var arrivages = await query.ToListAsync();

    // Step 4: Map to DTO
    var result = arrivages
        //.SelectMany(a => a.ArrivageDetails.SelectMany(ad => ad.RetourFournisseurs, (ad, rf) => new FournisseurDetailsDTO
                .SelectMany(a => a.ArrivageDetails.SelectMany(ad => ad.RetourFournisseurs ?? Enumerable.Empty<RetourFournisseur>(), (ad, rf) => new FournisseurDetailsDTO

        {
            Societe = a.Societe?.NomSociete  ?? "Unknown",
            Fournisseur = a.Fournisseur?.NomFournisseur ?? "Unknown",
            NumFacture = a.NumFacture,
            NumBL = a.NumBL,
            DateArrivage = a.DateArrivage,
            DateRetour = rf.DateRetour,
            Reference = ad.Article.Reference,
            QuantiteRetournee = rf.QuantiteRetournee,
            Marque = ad.Article.Marque.Libelle,
            PrixDachatDeviseTTC = ad.PrixDachatDevise,
            PrixDachatMADTTC = ad.PrixDachatMAD,
            MotifRetour = rf.MotifRetour,
            TypeRetour = rf.TypeRetour?.Libelle ?? "Unknown"
        }))
        .ToList();

    return result;
}


      
       


    }
}
