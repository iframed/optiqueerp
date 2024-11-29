using optique.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.IServices
{
    public interface IArrivageDetailsService
    {
        Task<IEnumerable<ArrivageDetailsDTO>> GetAllAsync();
        Task<ArrivageDetailsDTO?> GetByIdAsync(int id);
       Task AddAsync(ArrivageDetailsDTO dto, string userName);
        Task UpdateAsync(ArrivageDetailsDTO dto);
        Task DeleteAsync(int id);
        // Ajoutez cette ligne
        Task<IEnumerable<ArrivageDetailsDTO>> GetByArrivageIdAsync(int arrivageId);
    }
}
