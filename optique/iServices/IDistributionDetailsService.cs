using optique.Dtos;

namespace optique.IServices
{
    public interface IDistributionDetailsService
    {
        Task<IEnumerable<DistributionDetailsDTO>> GetAllAsync();
        Task<DistributionDetailsDTO?> GetByIdAsync(int id);
         Task AddAsync(DistributionDetailsDTO distributionDetailsDTO, string userName);
        Task UpdateAsync(DistributionDetailsDTO distributionDetailsDTO);
        Task DeleteAsync(int id);
    }
}
