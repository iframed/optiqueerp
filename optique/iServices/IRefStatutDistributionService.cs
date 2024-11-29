using optique.Dtos;

namespace optique.IServices
{
    public interface IRefStatutDistributionService
    {
        Task<IEnumerable<RefStatutDistributionDTO>> GetAllAsync();
        Task<RefStatutDistributionDTO?> GetByIdAsync(int id);
        Task AddAsync(RefStatutDistributionDTO refStatutDistributionDTO);
        Task UpdateAsync(RefStatutDistributionDTO refStatutDistributionDTO);
        Task DeleteAsync(int id);
        Task<string?> GetLibelleByIdAsync(int id);
    }
}
