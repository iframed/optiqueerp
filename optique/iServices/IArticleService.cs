
using System.Collections.Generic;
using System.Threading.Tasks;
using optique.Dtos;
using optique.Models;

namespace optique.IServices
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleDTO>> GetAllAsync();
        Task<ArticleDTO?> GetByIdAsync(int id);
        Task AddAsync(ArticleDTO articleDTO);
        Task UpdateAsync(ArticleDTO articleDTO);
        Task DeleteAsync(int id);

        Task<ArticleDTO?> GetByReferenceAsync(string reference);

       Task<IEnumerable<ArticleDTO>> GetByMarqueLibelleAsync(string libelle);

       Task<IEnumerable<ArticleDTO>> GetByTypeLibelleAsync(string libelle);

       Task<IEnumerable<ArticleDTO>> GetByFournisseurAdresseAsync(string adresse); 

        Task<IEnumerable<ArticleDetailsDTO>> GetArticleDetailsAsync();

        Task<ArticleDetailsDTO?> GetArticleDetailsByArrivageDetailsIdAsync(int arrivageDetailsId);

        Task<IEnumerable<ArticleDTO>> SearchArticlesByCriteriaAsync(string? fournisseur, string? marque, string? type, string? reference);
        Task<IEnumerable<ArticleDetailsDTO>> SearchArticleDetailsByCriteriaAsync(string? societe, string? fournisseur, string? marque, string? reference);
       //  Task<IEnumerable<ArticleDetailsGroupedDTO>> GetArticlesGroupedByDescriptionTypeMarqueAsync();
       Task<IEnumerable<ArticleDetailsGroupedDTO>> GetArticlesGroupedByDescriptionTypeMarqueAsync(int? typeId, int? fournisseurId, int? marqueId, string reference);

        Task<IEnumerable<ArticleDTO>> GetByFournisseurIdAsync(int fournisseurId);
    }
}
