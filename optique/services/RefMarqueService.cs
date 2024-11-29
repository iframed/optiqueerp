using AutoMapper;
using MyAspNetApp.Repositories;
using optique.Dtos;
using optique.IServices;
using optique.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.Services
{
    public class RefMarqueService : IRefMarqueService
    {
        private readonly IRepository<RefMarque> _repository;
        private readonly IMapper _mapper;

        public RefMarqueService(IRepository<RefMarque> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RefMarqueDTO>> GetAllAsync()
        {
            var refMarques = await _repository.ListAsync();
            return _mapper.Map<IEnumerable<RefMarqueDTO>>(refMarques);
        }

        public async Task<RefMarqueDTO?> GetByIdAsync(int id)
        {
            var refMarque = await _repository.GetByIdAsync(id);
            return _mapper.Map<RefMarqueDTO?>(refMarque);
        }

        public async Task AddAsync(RefMarqueDTO refMarqueDTO)
        {
            var refMarque = _mapper.Map<RefMarque>(refMarqueDTO);
            await _repository.AddAsync(refMarque);
        }

        public async Task UpdateAsync(RefMarqueDTO refMarqueDTO)
        {
            var refMarque = await _repository.GetByIdAsync(refMarqueDTO.Id);
            if (refMarque == null)
            {
                throw new Exception("La marque spécifiée n'existe pas.");
            }

            _mapper.Map(refMarqueDTO, refMarque);
            await _repository.UpdateAsync(refMarque);
        }

        public async Task DeleteAsync(int id)
        {
            var refMarque = await _repository.GetByIdAsync(id);
            if (refMarque == null)
            {
                throw new Exception("La marque spécifiée n'existe pas.");
            }

            await _repository.DeleteAsync(refMarque);
        }
    }
}
