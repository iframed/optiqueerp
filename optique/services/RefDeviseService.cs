using AutoMapper;
using MyAspNetApp.Repositories;
using optique.Dtos;
using optique.IServices;
using optique.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.Services
{
    public class RefDeviseService : IRefDeviseService
    {
        private readonly IRepository<RefDevise> _repository;
        private readonly IMapper _mapper;

        public RefDeviseService(IRepository<RefDevise> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RefDeviseDTO>> GetAllAsync()
        {
            var refDevises = await _repository.ListAsync();
            return _mapper.Map<IEnumerable<RefDeviseDTO>>(refDevises);
        }

        public async Task<RefDeviseDTO?> GetByIdAsync(int id)
        {
            var refDevise = await _repository.GetByIdAsync(id);
            return _mapper.Map<RefDeviseDTO?>(refDevise);
        }

        public async Task AddAsync(RefDeviseDTO refDeviseDTO)
        {
            var refDevise = _mapper.Map<RefDevise>(refDeviseDTO);
            await _repository.AddAsync(refDevise);
        }

        public async Task UpdateAsync(RefDeviseDTO refDeviseDTO)
        {
            var refDevise = await _repository.GetByIdAsync(refDeviseDTO.Id);
            if (refDevise == null)
            {
                throw new Exception("La devise spécifiée n'existe pas.");
            }

            _mapper.Map(refDeviseDTO, refDevise);
            await _repository.UpdateAsync(refDevise);
        }

        public async Task DeleteAsync(int id)
        {
            var refDevise = await _repository.GetByIdAsync(id);
            if (refDevise == null)
            {
                throw new Exception("La devise spécifiée n'existe pas.");
            }

            await _repository.DeleteAsync(refDevise);
        }
    }
}
