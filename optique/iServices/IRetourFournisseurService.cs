using optique.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.IServices
{
    public interface IRetourFournisseurService
    {
        Task<IEnumerable<RetourFournisseurDTO>> GetAllAsync();
        Task<RetourFournisseurDTO?> GetByIdAsync(int id);
        Task AddAsync(RetourFournisseurDTO retourFournisseurDTO, string userName);
        Task UpdateAsync(RetourFournisseurDTO retourFournisseurDTO);
        Task DeleteAsync(int id);

        Task<IEnumerable<FournisseurDetailsDTO>> GetFournisseurDetailsAsync();
       Task<IEnumerable<FournisseurDetailsDTO>> GetFournisseurDetailsByCriteriaAsync(string? societe, string? marque, string? reference, string? typeRetour);

       
    }
}
