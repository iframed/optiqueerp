using optique.Dtos;

namespace optique.IServices
{
    public interface IRefMarqueService
    {
        Task<IEnumerable<RefMarqueDTO>> GetAllAsync();
        Task<RefMarqueDTO?> GetByIdAsync(int id);
        Task AddAsync(RefMarqueDTO refMarqueDTO);
        Task UpdateAsync(RefMarqueDTO refMarqueDTO);
        Task DeleteAsync(int id);
    }
}
