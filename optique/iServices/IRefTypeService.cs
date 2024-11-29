using optique.Dtos;

namespace optique.IServices
{
    public interface IRefTypeService
    {
        Task<IEnumerable<RefTypeDTO>> GetAllAsync();
        Task<RefTypeDTO?> GetByIdAsync(int id);
        Task AddAsync(RefTypeDTO refTypeDTO);
        Task UpdateAsync(RefTypeDTO refTypeDTO);
        Task DeleteAsync(int id);
    }
}
