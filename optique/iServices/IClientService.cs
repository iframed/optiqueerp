using optique.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.IServices
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDTO>> GetAllAsync();
        Task<ClientDTO?> GetByIdAsync(int id);
        Task AddAsync(ClientDTO clientDTO);
        Task UpdateAsync(ClientDTO clientDTO);
        Task DeleteAsync(int id);

       public  Task<IEnumerable<ClientDTO>> GetByTypeLibelleAsync(string typeLibelle);

       public  Task<IEnumerable<ClientDTO>> GetByAdresseAsync(string adresse);

     Task<IEnumerable<ClientDTO>> GetMagazinInterneClientsAsync();
      Task<IEnumerable<ArticleDTO>> GetArticlesWithQuantitiesAsync();
    }
}
