using System.Collections.Generic;
using System.Threading.Tasks;
using optique.Dtos;

namespace optique.IServices
{
    public interface IMouvementArticleService
    {
        Task<IEnumerable<MouvementArticleDTO>> GetAllAsync();
  
        Task AddAsync(MouvementArticleDTO mouvementArticleDTO);
        Task<MouvementArticleDTO?> GetByIdAsync(int id);
    }
}
