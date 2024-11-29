using AutoMapper;
using MyAspNetApp.Repositories;
using optique.Dtos;
using optique.Models;
using optique.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using optique.Data;

namespace optique.Services
{
    public class ArrivageDetailsService : IArrivageDetailsService
    {
        private readonly IRepository<ArrivageDetails> _repository;
        private readonly IMapper _mapper;

        private readonly ApplicationDbContext _context;

        public ArrivageDetailsService(IRepository<ArrivageDetails> repository, IMapper mapper, ApplicationDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<ArrivageDetailsDTO>> GetAllAsync()
        {
            var details = await _repository.ListAsync();
            return _mapper.Map<IEnumerable<ArrivageDetailsDTO>>(details);
        }

        public async Task<ArrivageDetailsDTO?> GetByIdAsync(int id)
        {
            var detail = await _repository.GetByIdAsync(id);
            return _mapper.Map<ArrivageDetailsDTO?>(detail);
        }

        

        public async Task AddAsync(ArrivageDetailsDTO dto, string userName)
        {
            var detail = _mapper.Map<ArrivageDetails>(dto);
            detail.CreePar = userName; // Set the CreePar value here

            await _repository.AddAsync(detail);

            // Add MouvementArticle
            var mouvementArticle = new MouvementArticle
            {
                ArrivageDetailsId = detail.Id,
                Quantite = detail.QuantiteRecuParArticle,
                DateDeCreation = DateTime.UtcNow,
                CreePar = userName // Utilisez le nom de l'utilisateur authentifié
            };
            _context.MouvementArticles.Add(mouvementArticle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ArrivageDetailsDTO dto)
        {
            var detail = await _repository.GetByIdAsync(dto.Id);
            if (detail == null)
            {
                throw new Exception("Detail d'arrivage non trouvé");
            }

            _mapper.Map(dto, detail);
            await _repository.UpdateAsync(detail);
        }

        public async Task DeleteAsync(int id)
        {
            var detail = await _repository.GetByIdAsync(id);
            if (detail == null)
            {
                throw new Exception("Detail d'arrivage non trouvé");
            }

            await _repository.DeleteAsync(detail);
        }

        public async Task<IEnumerable<ArrivageDetailsDTO>> GetByArrivageIdAsync(int arrivageId)
        {
            var details = await _context.ArrivageDetails
                                        .Include(ad => ad.Article)
                                        .Where(ad => ad.ArrivageId == arrivageId)
                                        .ToListAsync();

            return _mapper.Map<IEnumerable<ArrivageDetailsDTO>>(details);
        }

        

    }
}