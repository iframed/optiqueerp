using System.Collections.Generic;
using System.Threading.Tasks;
using optique.Dtos;
using optique.IServices;
using optique.Models;
using MyAspNetApp.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace optique.Services
{
    public class RefTypeDePaiementService : IRefTypeDePaiementService
    {
        private readonly IRepository<RefTypeDePaiement> _repository;
        private readonly IMapper _mapper;

        public RefTypeDePaiementService(IRepository<RefTypeDePaiement> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RefTypeDePaiementDTO>> GetAllAsync()
        {
            var refTypes = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<RefTypeDePaiementDTO>>(refTypes);
        }

        public async Task<RefTypeDePaiementDTO?> GetByIdAsync(int id)
        {
            var refType = await _repository.GetByIdAsync(id);
            return _mapper.Map<RefTypeDePaiementDTO?>(refType);
        }

        public async Task AddAsync(RefTypeDePaiementDTO refTypeDePaiementDTO)
        {
            var refType = _mapper.Map<RefTypeDePaiement>(refTypeDePaiementDTO);
            await _repository.AddAsync(refType);
        }

        public async Task UpdateAsync(RefTypeDePaiementDTO refTypeDePaiementDTO)
        {
            var refType = await _repository.GetByIdAsync(refTypeDePaiementDTO.Id);
            if (refType == null)
            {
                throw new KeyNotFoundException("Le type de paiement spécifié n'existe pas.");
            }
            _mapper.Map(refTypeDePaiementDTO, refType);
            await _repository.UpdateAsync(refType);
        }

        public async Task DeleteAsync(int id)
        {
            var refType = await _repository.GetByIdAsync(id);
            if (refType == null)
            {
                throw new KeyNotFoundException("Le type de paiement spécifié n'existe pas.");
            }
            await _repository.DeleteAsync(refType);
        }
    }
}
