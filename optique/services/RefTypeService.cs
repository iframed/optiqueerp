using AutoMapper;
using MyAspNetApp.Repositories;
using optique.Dtos;
using optique.IServices;
using optique.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.Services
{
    public class RefTypeService : IRefTypeService
    {
        private readonly IRepository<RefType> _repository;
        private readonly IMapper _mapper;

        public RefTypeService(IRepository<RefType> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RefTypeDTO>> GetAllAsync()
        {
            var refTypes = await _repository.ListAsync();
            return _mapper.Map<IEnumerable<RefTypeDTO>>(refTypes);
        }

        public async Task<RefTypeDTO?> GetByIdAsync(int id)
        {
            var refType = await _repository.GetByIdAsync(id);
            return _mapper.Map<RefTypeDTO?>(refType);
        }

        public async Task AddAsync(RefTypeDTO refTypeDTO)
        {
            var refType = _mapper.Map<RefType>(refTypeDTO);
            await _repository.AddAsync(refType);
        }

        public async Task UpdateAsync(RefTypeDTO refTypeDTO)
        {
            var refType = await _repository.GetByIdAsync(refTypeDTO.Id);
            if (refType == null)
            {
                throw new Exception("Le type spécifié n'existe pas.");
            }

            _mapper.Map(refTypeDTO, refType);
            await _repository.UpdateAsync(refType);
        }

        public async Task DeleteAsync(int id)
        {
            var refType = await _repository.GetByIdAsync(id);
            if (refType == null)
            {
                throw new Exception("Le type spécifié n'existe pas.");
            }

            await _repository.DeleteAsync(refType);
        }
    }
}
