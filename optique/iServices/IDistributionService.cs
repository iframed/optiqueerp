using optique.Dtos;

namespace optique.IServices
{
    public interface IDistributionService
    {
        Task<IEnumerable<DistributionDTO>> GetAllAsync();
        Task<DistributionDTO?> GetByIdAsync(int id);
       // Task AddAsync(DistributionDTO distributionDTO);
       Task<int> AddAsync(DistributionDTO distributionDTO);
        Task UpdateAsync(DistributionDTO distributionDTO);
        Task DeleteAsync(int id);

        Task<IEnumerable<DistributionSummaryDTO>> GetDistributionSummaryAsync();

        Task<IEnumerable<DistributionSummaryDTO>> GetDistributionSummaryByClientAsync(int clientId); 
        
        Task<IEnumerable<DistributionSummaryDTO>> GetDistributionSummaryByFournisseurAsync(int fournisseurId);
        Task<IEnumerable<DistributionSummaryDTO>> GetDistributionSummaryByMarqueLibelleAsync(string marqueLibelle);
        Task<IEnumerable<DistributionSummaryDTO>> GetDistributionSummaryByStatutLibelleAsync(string statutLibelle); 

        Task<IEnumerable<DistributionSummaryDTO>> GetDistributionSummaryByReferenceAsync(string reference); 

        Task<IEnumerable<DistributionSummaryDTO>> SearchByCriteriaAsync(string? client, string? fournisseur, string? marque, string? statut, string? reference);


         Task<int> CreateDistributionAsync(DistributionDTO distributionDTO);
    }
}
