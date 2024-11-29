using AutoMapper;
using MyAspNetApp.Repositories;
using optique.Dtos;
using optique.IServices;
using optique.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.Services
{
    public class RefStatutDistributionService : IRefStatutDistributionService
    {
        private readonly IRepository<RefStatutDistribution> _repository;
        private readonly IMapper _mapper;

        public RefStatutDistributionService(IRepository<RefStatutDistribution> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RefStatutDistributionDTO>> GetAllAsync()
        {
            var refStatutDistributions = await _repository.ListAsync();
            return _mapper.Map<IEnumerable<RefStatutDistributionDTO>>(refStatutDistributions);
        }

        public async Task<RefStatutDistributionDTO?> GetByIdAsync(int id)
        {
            var refStatutDistribution = await _repository.GetByIdAsync(id);
            return _mapper.Map<RefStatutDistributionDTO?>(refStatutDistribution);
        }

        public async Task AddAsync(RefStatutDistributionDTO refStatutDistributionDTO)
        {
            var refStatutDistribution = _mapper.Map<RefStatutDistribution>(refStatutDistributionDTO);
            await _repository.AddAsync(refStatutDistribution);
        }

        public async Task UpdateAsync(RefStatutDistributionDTO refStatutDistributionDTO)
        {
            var refStatutDistribution = await _repository.GetByIdAsync(refStatutDistributionDTO.Id);
            if (refStatutDistribution == null)
            {
                throw new Exception("The specified status distribution does not exist.");
            }

            _mapper.Map(refStatutDistributionDTO, refStatutDistribution);
            await _repository.UpdateAsync(refStatutDistribution);
        }

        public async Task DeleteAsync(int id)
        {
            var refStatutDistribution = await _repository.GetByIdAsync(id);
            if (refStatutDistribution == null)
            {
                throw new Exception("The specified status distribution does not exist.");
            }

            await _repository.DeleteAsync(refStatutDistribution);
        }

        public async Task<string?> GetLibelleByIdAsync(int id)
{
    var statut = await _repository.GetByIdAsync(id);
    return statut?.Libelle; // Retourne le libellé du statut ou null si pas trouvé
}

    }
}
