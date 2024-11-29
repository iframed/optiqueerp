using AutoMapper;
using MyAspNetApp.Repositories;
using optique.Dtos;
using optique.Models;
using optique.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.Services
{
    public class SocieteService : ISocieteService
    {
        private readonly IRepository<Societe> _repository;
        private readonly IMapper _mapper;

        public SocieteService(IRepository<Societe> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SocieteDTO>> GetAllAsync()
        {
            var societes = await _repository.ListAsync();
            return _mapper.Map<IEnumerable<SocieteDTO>>(societes);
        }

        public async Task<SocieteDTO?> GetByIdAsync(int id)
        {
            var societe = await _repository.GetByIdAsync(id);
            return _mapper.Map<SocieteDTO?>(societe);
        }

        public async Task AddAsync(SocieteDTO societeDTO)
        {
            var societe = _mapper.Map<Societe>(societeDTO);
            await _repository.AddAsync(societe);
        }

        public async Task UpdateAsync(SocieteDTO societeDTO)
        {
            var societe = await _repository.GetByIdAsync(societeDTO.Id);
            if (societe == null)
            {
                throw new Exception("La société spécifiée n'existe pas.");
            }

            _mapper.Map(societeDTO, societe);
            await _repository.UpdateAsync(societe);
        }

        public async Task DeleteAsync(int id)
        {
            var societe = await _repository.GetByIdAsync(id);
            if (societe == null)
            {
                throw new Exception("La société spécifiée n'existe pas.");
            }

            await _repository.DeleteAsync(societe);
        }

        public async Task<SocieteDTO?> GetByNameAsync(string nomSociete)
        {
            var societe = await _repository.FirstOrDefaultAsync(new SocieteByNameSpecification(nomSociete));
            return _mapper.Map<SocieteDTO?>(societe);
        }

        public async Task<IEnumerable<SocieteDTO>> GetByAdresseAsync(string adresse)
        {
            var societes = await _repository.ListAsync(new SocieteByAdresseSpecification(adresse));
            return _mapper.Map<IEnumerable<SocieteDTO>>(societes);
        }

        public async Task<string?> GetNomByIdAsync(int id)
{
    var societe = await _repository.GetByIdAsync(id);
    return societe?.NomSociete; // Retourne le nom de la société ou null si pas trouvé
}

    }
}
