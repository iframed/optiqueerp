using System.Collections.Generic;
using System.Threading.Tasks;
using optique.Dtos;

namespace optique.IServices
{
    public interface IRefTypeDePaiementService
    {
        Task<IEnumerable<RefTypeDePaiementDTO>> GetAllAsync();
        Task<RefTypeDePaiementDTO?> GetByIdAsync(int id);
       
        Task AddAsync(RefTypeDePaiementDTO refTypeDePaiementDTO);
        Task UpdateAsync(RefTypeDePaiementDTO refTypeDePaiementDTO);
        Task DeleteAsync(int id);
    }
}
