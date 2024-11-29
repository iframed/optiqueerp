using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyAspNetApp.Repositories;
using optique.Dtos;
using optique.IServices;
using optique.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.Services
{
    public class RefTypeRetourService : IRefTypeRetourService
    {
        private readonly IRepository<RefTypeRetour> _repository;
        private readonly IMapper _mapper;

        public RefTypeRetourService(IRepository<RefTypeRetour> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RefTypeRetourDTO>> GetAllAsync()
        {
            var refTypeRetours = await _repository.ListAsync();
            return _mapper.Map<IEnumerable<RefTypeRetourDTO>>(refTypeRetours);
        }

        public async Task<RefTypeRetourDTO?> GetByIdAsync(int id)
        {
            var refTypeRetour = await _repository.GetByIdAsync(id);
            return _mapper.Map<RefTypeRetourDTO?>(refTypeRetour);
        }

        public async Task AddAsync(RefTypeRetourDTO refTypeRetourDTO)
        {
            var refTypeRetour = _mapper.Map<RefTypeRetour>(refTypeRetourDTO);
            await _repository.AddAsync(refTypeRetour);
        }

        public async Task UpdateAsync(RefTypeRetourDTO refTypeRetourDTO)
        {
            var refTypeRetour = await _repository.GetByIdAsync(refTypeRetourDTO.Id);
            if (refTypeRetour == null)
            {
                throw new Exception("Le type de retour spécifié n'existe pas.");
            }

            _mapper.Map(refTypeRetourDTO, refTypeRetour);
            await _repository.UpdateAsync(refTypeRetour);
        }

        public async Task DeleteAsync(int id)
        {
            var refTypeRetour = await _repository.GetByIdAsync(id);
            if (refTypeRetour == null)
            {
                throw new Exception("Le type de retour spécifié n'existe pas.");
            }

            await _repository.DeleteAsync(refTypeRetour);
        }
    }
}
