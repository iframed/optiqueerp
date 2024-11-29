using AutoMapper;
using MyAspNetApp.Repositories;
using optique.Data;
using optique.Dtos;
using optique.IServices;
using optique.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace optique.Services
{
    public class DistributionService : IDistributionService
    {
        private readonly IRepository<Distribution> _repository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public DistributionService(IRepository<Distribution> repository, IMapper mapper, ApplicationDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<DistributionDTO>> GetAllAsync()
        {
            var distributions = await _repository.ListAsync();
            return _mapper.Map<IEnumerable<DistributionDTO>>(distributions);
        }

        public async Task<DistributionDTO?> GetByIdAsync(int id)
        {
            var distribution = await _repository.GetByIdAsync(id);
            return _mapper.Map<DistributionDTO?>(distribution);
        }

       

        public async Task<int> AddAsync(DistributionDTO distributionDTO)
{
    var distribution = _mapper.Map<Distribution>(distributionDTO);
    await _repository.AddAsync(distribution);
    await _context.SaveChangesAsync(); // Assurez-vous de sauvegarder les modifications pour obtenir l'ID
    return distribution.Id; // Retournez l'ID de la distribution nouvellement créée
}


        public async Task UpdateAsync(DistributionDTO distributionDTO)
        {
            var distribution = await _repository.GetByIdAsync(distributionDTO.Id);
            if (distribution == null)
            {
                throw new Exception("The specified distribution does not exist.");
            }

            _mapper.Map(distributionDTO, distribution);
            await _repository.UpdateAsync(distribution);
        }

        public async Task DeleteAsync(int id)
        {
            var distribution = await _repository.GetByIdAsync(id);
            if (distribution == null)
            {
                throw new Exception("The specified distribution does not exist.");
            }

            await _repository.DeleteAsync(distribution);
        }


        public async Task<IEnumerable<DistributionSummaryDTO>> GetDistributionSummaryAsync()
{
    var distributions = await _context.DistributionDetails
        .Include(d => d.Distribution)
            .ThenInclude(dist => dist.Client)
        .Include(d => d.Distribution)
            .ThenInclude(dist => dist.StatutDistribution)
        .Include(d => d.ArrivageDetails)
            .ThenInclude(ad => ad.Article)
                .ThenInclude(a => a.Fournisseur)
        .Include(d => d.ArrivageDetails)
            .ThenInclude(ad => ad.Article)
                .ThenInclude(a => a.Marque)
        .ToListAsync();

    return _mapper.Map<IEnumerable<DistributionSummaryDTO>>(distributions);
}


public async Task<IEnumerable<DistributionSummaryDTO>> GetDistributionSummaryByClientAsync(int clientId)
    {
        var distributions = await _context.DistributionDetails
            .Include(d => d.Distribution)
                .ThenInclude(dist => dist.Client)
            .Include(d => d.Distribution)
                .ThenInclude(dist => dist.StatutDistribution)
            .Include(d => d.ArrivageDetails)
                .ThenInclude(ad => ad.Article)
                    .ThenInclude(a => a.Fournisseur)
            .Include(d => d.ArrivageDetails)
                .ThenInclude(ad => ad.Article)
                    .ThenInclude(a => a.Marque)
            .Where(d => d.Distribution.ClientId == clientId) // Filtrer par client
            .ToListAsync();

        return _mapper.Map<IEnumerable<DistributionSummaryDTO>>(distributions);
    }



    public async Task<IEnumerable<DistributionSummaryDTO>> GetDistributionSummaryByFournisseurAsync(int fournisseurId)
    {
        var distributions = await _context.DistributionDetails
            .Include(d => d.Distribution)
                .ThenInclude(dist => dist.Client)
            .Include(d => d.Distribution)
                .ThenInclude(dist => dist.StatutDistribution)
            .Include(d => d.ArrivageDetails)
                .ThenInclude(ad => ad.Article)
                    .ThenInclude(a => a.Fournisseur)
            .Include(d => d.ArrivageDetails)
                .ThenInclude(ad => ad.Article)
                    .ThenInclude(a => a.Marque)
            .Where(d => d.ArrivageDetails.Article.FournisseurId == fournisseurId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<DistributionSummaryDTO>>(distributions);
    }


    public async Task<IEnumerable<DistributionSummaryDTO>> GetDistributionSummaryByMarqueLibelleAsync(string marqueLibelle)
    {
        var distributions = await _context.DistributionDetails
            .Include(d => d.Distribution)
                .ThenInclude(dist => dist.Client)
            .Include(d => d.Distribution)
                .ThenInclude(dist => dist.StatutDistribution)
            .Include(d => d.ArrivageDetails)
                .ThenInclude(ad => ad.Article)
                    .ThenInclude(a => a.Fournisseur)
            .Include(d => d.ArrivageDetails)
                .ThenInclude(ad => ad.Article)
                    .ThenInclude(a => a.Marque)
            .Where(d => d.ArrivageDetails.Article.Marque.Libelle == marqueLibelle)
            .ToListAsync();

        return _mapper.Map<IEnumerable<DistributionSummaryDTO>>(distributions);
    }



    public async Task<IEnumerable<DistributionSummaryDTO>> GetDistributionSummaryByStatutLibelleAsync(string statutLibelle)
    {
        var distributions = await _context.DistributionDetails
            .Include(d => d.Distribution)
                .ThenInclude(dist => dist.Client)
            .Include(d => d.Distribution)
                .ThenInclude(dist => dist.StatutDistribution)
            .Include(d => d.ArrivageDetails)
                .ThenInclude(ad => ad.Article)
                    .ThenInclude(a => a.Fournisseur)
            .Include(d => d.ArrivageDetails)
                .ThenInclude(ad => ad.Article)
                    .ThenInclude(a => a.Marque)
            .Where(d => d.Distribution.StatutDistribution.Libelle == statutLibelle)
            .ToListAsync();

        return _mapper.Map<IEnumerable<DistributionSummaryDTO>>(distributions);
    }



     public async Task<IEnumerable<DistributionSummaryDTO>> GetDistributionSummaryByReferenceAsync(string reference)
    {
        var distributions = await _context.DistributionDetails
            .Include(d => d.Distribution)
                .ThenInclude(dist => dist.Client)
            .Include(d => d.Distribution)
                .ThenInclude(dist => dist.StatutDistribution)
            .Include(d => d.ArrivageDetails)
                .ThenInclude(ad => ad.Article)
                    .ThenInclude(a => a.Fournisseur)
            .Include(d => d.ArrivageDetails)
                .ThenInclude(ad => ad.Article)
                    .ThenInclude(a => a.Marque)
            .Where(d => d.ArrivageDetails.Article.Reference == reference)
            .ToListAsync();

        return _mapper.Map<IEnumerable<DistributionSummaryDTO>>(distributions);
    }




   public async Task<IEnumerable<DistributionSummaryDTO>> SearchByCriteriaAsync(string? client, string? fournisseur, string? marque, string? statut, string? reference)
{
    var query = _context.DistributionDetails
        .Include(d => d.Distribution)
            .ThenInclude(dist => dist.Client)
        .Include(d => d.Distribution)
            .ThenInclude(dist => dist.StatutDistribution)
        .Include(d => d.ArrivageDetails)
            .ThenInclude(ad => ad.Article)
                .ThenInclude(a => a.Fournisseur)
        .Include(d => d.ArrivageDetails)
            .ThenInclude(ad => ad.Article)
                .ThenInclude(a => a.Marque)
        .AsQueryable();

    if (!string.IsNullOrEmpty(client))
    {
        query = query.Where(d => d.Distribution.Client.NomClient.Trim() == client.Trim());
    }

    if (!string.IsNullOrEmpty(fournisseur))
    {
        query = query.Where(d => d.ArrivageDetails.Article.Fournisseur.NomFournisseur.Trim() == fournisseur.Trim());
    }

    if (!string.IsNullOrEmpty(marque))
    {
        query = query.Where(d => d.ArrivageDetails.Article.Marque.Libelle.Trim() == marque.Trim());
    }

    if (!string.IsNullOrEmpty(statut))
    {
        query = query.Where(d => d.Distribution.StatutDistribution.Libelle.Trim() == statut.Trim());
    }

    if (!string.IsNullOrEmpty(reference))
    {
        query = query.Where(d => d.ArrivageDetails.Article.Reference.Trim() == reference.Trim());
    }

    var distributions = await query.ToListAsync();
    return _mapper.Map<IEnumerable<DistributionSummaryDTO>>(distributions);
}

        public Task<int> CreateDistributionAsync(DistributionDTO distributionDTO)
        {
            throw new NotImplementedException();
        }
    }
}
