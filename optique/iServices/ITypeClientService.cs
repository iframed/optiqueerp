using optique.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.IServices
{
    public interface ITypeClientService
    {
        Task<IEnumerable<RefTypeClientDTO>> GetAllAsync();
        Task<RefTypeClientDTO?> GetByIdAsync(int id);
        Task AddAsync(RefTypeClientDTO dto);
        Task UpdateAsync(RefTypeClientDTO dto);
        Task DeleteAsync(int id);
    }
}
