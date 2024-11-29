using optique.Dtos;

namespace optique.IServices
{
    public interface IRefDeviseService
    {
        Task<IEnumerable<RefDeviseDTO>> GetAllAsync();
        Task<RefDeviseDTO?> GetByIdAsync(int id);
        Task AddAsync(RefDeviseDTO refDeviseDTO);
        Task UpdateAsync(RefDeviseDTO refDeviseDTO);
        Task DeleteAsync(int id);
    }
}
