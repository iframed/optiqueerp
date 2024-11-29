using optique.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.IServices
{
    public interface IRefTypeRetourService
    {
        Task<IEnumerable<RefTypeRetourDTO>> GetAllAsync();
        Task<RefTypeRetourDTO?> GetByIdAsync(int id);
        Task AddAsync(RefTypeRetourDTO refTypeRetourDTO);
        Task UpdateAsync(RefTypeRetourDTO refTypeRetourDTO);
        Task DeleteAsync(int id);
    }
}
