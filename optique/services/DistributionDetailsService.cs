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
    public class DistributionDetailsService : IDistributionDetailsService
    {
        private readonly IRepository<DistributionDetails> _repository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        private readonly ILogger<DistributionDetailsService> _logger;

        public DistributionDetailsService(IRepository<DistributionDetails> repository, IMapper mapper, ApplicationDbContext context,ILogger<DistributionDetailsService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
            _logger = logger;
            
        }

        public async Task<IEnumerable<DistributionDetailsDTO>> GetAllAsync()
        {
            var distributionDetails = await _repository.ListAsync();
            return _mapper.Map<IEnumerable<DistributionDetailsDTO>>(distributionDetails);
        }

        public async Task<DistributionDetailsDTO?> GetByIdAsync(int id)
        {
            var distributionDetails = await _repository.GetByIdAsync(id);
            return _mapper.Map<DistributionDetailsDTO?>(distributionDetails);
        }

        


     
public async Task AddAsync(DistributionDetailsDTO distributionDetailsDTO, string userName)
{
    // Récupérez les détails d'arrivage
    var arrivageDetails = await _context.ArrivageDetails
        .Include(ad => ad.Article)
        .FirstOrDefaultAsync(ad => ad.Id == distributionDetailsDTO.ArrivageDetailsId);

    if (arrivageDetails == null)
    {
        _logger.LogError($"ArrivageDetails with ID {distributionDetailsDTO.ArrivageDetailsId} not found.");
        throw new Exception("Le ArrivageDetails spécifié n'existe pas.");
    }

    // Calculez les quantités distribuées, retournées et vendues
    var quantiteDistribuee = await _context.DistributionDetails
        .Where(dd => dd.ArrivageDetailsId == distributionDetailsDTO.ArrivageDetailsId)
        .SumAsync(dd => dd.Quantite);

    var quantiteRetournee = await _context.RetourFournisseurs
        .Where(rf => rf.ArrivageDetailsId == distributionDetailsDTO.ArrivageDetailsId)
        .SumAsync(rf => rf.QuantiteRetournee);

    var quantiteVendue = await _context.Ventes
        .Where(v => v.ArticleId == arrivageDetails.ArticleId)
        .SumAsync(v => v.QuantiteVendu);

    var quantiteRestante = arrivageDetails.QuantiteRecuParArticle - (quantiteDistribuee + quantiteRetournee );

    // Log the calculated quantities
    _logger.LogInformation($"Quantité reçue: {arrivageDetails.QuantiteRecuParArticle}");
    _logger.LogInformation($"Quantité distribuée: {quantiteDistribuee}");
    _logger.LogInformation($"Quantité retournée: {quantiteRetournee}");
    _logger.LogInformation($"Quantité vendue: {quantiteVendue}");
    _logger.LogInformation($"Quantité restante: {quantiteRestante}");

    // Vérifiez la quantité restante
    if (distributionDetailsDTO.Quantite > quantiteRestante)
    {
        _logger.LogError("La quantité demandée dépasse la quantité restante disponible.");
        throw new Exception("La quantité demandée dépasse la quantité restante disponible.");
    }

    // Créez l'entité DistributionDetails
    var distributionDetails = _mapper.Map<DistributionDetails>(distributionDetailsDTO);
    distributionDetails.ArrivageDetails = arrivageDetails;

    // Ajoutez DistributionDetails au repository
    await _repository.AddAsync(distributionDetails);

    // Créez et ajoutez MouvementArticle
    var mouvementArticle = new MouvementArticle
    {
        DistributionDetailsId = distributionDetails.Id,
        Quantite = distributionDetails.Quantite,
        DateDeCreation = DateTime.UtcNow,
        CreePar = userName
    };
    
    _context.MouvementArticles.Add(mouvementArticle);

    // Enregistrez les modifications dans le contexte
    await _context.SaveChangesAsync();
}







        public async Task UpdateAsync(DistributionDetailsDTO distributionDetailsDTO)
        {
            var distributionDetails = await _repository.GetByIdAsync(distributionDetailsDTO.Id);
            if (distributionDetails == null)
            {
                throw new Exception("The specified DistributionDetails does not exist.");
            }

            _mapper.Map(distributionDetailsDTO, distributionDetails);
            await _repository.UpdateAsync(distributionDetails);
        }

        public async Task DeleteAsync(int id)
        {
            var distributionDetails = await _repository.GetByIdAsync(id);
            if (distributionDetails == null)
            {
                throw new Exception("The specified DistributionDetails does not exist.");
            }

            await _repository.DeleteAsync(distributionDetails);
        }
    }
}
