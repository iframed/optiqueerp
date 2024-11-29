using optique.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.IServices
{
    public interface IArrivageService
    {
        Task<IEnumerable<ArrivageDTO>> GetAllAsync();
        Task<ArrivageDTO?> GetByIdAsync(int id);
       // Task AddAsync(ArrivageDTO dto);
        Task UpdateAsync(ArrivageDTO dto);
        Task DeleteAsync(int id);
        Task<int> AddAsync(ArrivageDTO dto);

        Task AjouterPaiementPourArrivage(int arrivageId, DetailsPaiementDTO paiementDto);
        Task<List<DetailsPaiementDTO>> GetDetailsPaiementsByArrivageId(int arrivageId);
        Task<bool> ValiderArrivage(int arrivageId);


    }
}
