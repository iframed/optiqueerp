using optique.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.IServices
{
    public interface ISocieteService
    {
        Task<IEnumerable<SocieteDTO>> GetAllAsync();
        Task<SocieteDTO?> GetByIdAsync(int id);
        Task AddAsync(SocieteDTO societeDTO);
        Task UpdateAsync(SocieteDTO societeDTO);
        Task DeleteAsync(int id);
      Task<SocieteDTO?> GetByNameAsync(string nomSociete);

       Task<IEnumerable<SocieteDTO>> GetByAdresseAsync(string adresse);
        Task<string?> GetNomByIdAsync(int id);


    }
}
