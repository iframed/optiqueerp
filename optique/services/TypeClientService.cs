using AutoMapper;
using MyAspNetApp.Repositories;
using optique.Dtos;
using optique.Models;
using optique.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.Services
{
    public class RefTypeClientService : ITypeClientService
    {
        private readonly IRepository<RefTypeClient> _repository;
        private readonly IMapper _mapper;

        public RefTypeClientService(IRepository<RefTypeClient> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RefTypeClientDTO>> GetAllAsync()
        {
            var types = await _repository.ListAsync();
            return _mapper.Map<IEnumerable<RefTypeClientDTO>>(types);
        }

        public async Task<RefTypeClientDTO?> GetByIdAsync(int id)
        {
            var type = await _repository.GetByIdAsync(id);
            return _mapper.Map<RefTypeClientDTO?>(type);
        }

        public async Task AddAsync(RefTypeClientDTO refTypeClientDTO)
        {
            var refTypeClient = _mapper.Map<RefTypeClient>(refTypeClientDTO);
            await _repository.AddAsync(refTypeClient);
        }

        public async Task UpdateAsync(RefTypeClientDTO refTypeClientDTO)
        {
            var refTypeClient = await _repository.GetByIdAsync(refTypeClientDTO.Id);
            if (refTypeClient == null)
            {
                throw new Exception("Type de client non trouvé");
            }

            _mapper.Map(refTypeClientDTO, refTypeClient);
            await _repository.UpdateAsync(refTypeClient);
        }

        public async Task DeleteAsync(int id)
        {
            var refTypeClient = await _repository.GetByIdAsync(id);
            if (refTypeClient == null)
            {
                throw new Exception("Type de client non trouvé");
            }

            await _repository.DeleteAsync(refTypeClient);
        }
    }
}
