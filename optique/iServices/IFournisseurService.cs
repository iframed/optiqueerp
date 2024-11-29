using optique.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.IServices
{
    public interface IFournisseurService
    {
        Task<IEnumerable<FournisseurDTO>> GetAllAsync();
        Task<FournisseurDTO?> GetByIdAsync(int id);
       // Task AddAsync(FournisseurDTO fournisseurDTO);
        Task UpdateAsync(FournisseurDTO fournisseurDTO);
        Task DeleteAsync(int id);

        Task<IEnumerable<FournisseurDTO>> GetByDeviseLibelleAsync(string deviseLibelle);

         Task<IEnumerable<FournisseurDTO>> GetByICEAsync(string ice);

        Task AddAsync(FournisseurDTO fournisseurDTO);
         Task<IEnumerable<FournisseurDetailsDTO>> GetFournisseurDetailsAsync();

         Task<IEnumerable<FournisseurDTO>> GetByICEAndDeviseLibelleAsync(string ice, string deviseLibelle);

         Task<string?> GetNomByIdAsync(int id);
    }
}
