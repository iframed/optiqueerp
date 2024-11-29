using optique.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using optique.Data;
using optique.ViewModels;

namespace optique.IServices
{
    public interface IVenteService
    {
        Task<IEnumerable<VenteDetailsDTO>> GetAllAsync();
        Task<VenteDetailsDTO?> GetByIdAsync(int id);
        Task AddAsync(VenteDTO venteDTO , string userName);
        Task UpdateAsync(VenteDTO venteDTO);
        Task DeleteAsync(int id);

  Task<IEnumerable<ChequeDueDateDTO>> GetChequesByDateAsync(DateTime date);
     

      Task<IEnumerable<VenteDetailsDTO>> SearchAsync(string? client, string? marque, string? typeArticle, string? reference);
     // Task<IEnumerable<ChequeDueDateDTO>> GetChequeDueDatesAsync(int month, int year);

      Task<IEnumerable<ChequeDueDateDTO>> GetChequeDueDatesAsync(int month, int year, string view);

     

      Task UpdateDetailsPaiementsAsync(List<DetailsPaiementDTO> detailsPaiements);
      Task<bool> CreateVenteAsync(NouvelleVenteViewModel venteVm);

      Task<IEnumerable<ChequeDueDateDTO>> GetFournisseurChequeDueDatesAsync(int month, int year, string view);

      Task<IEnumerable<ChequeDueDateDTO>> GetFournisseurChequesByDateAsync(DateTime date);

      Task<IEnumerable<ChequeDueDateDTO>> GetLCNDueDatesAsync(int month, int year, string view, int? day = null);
      Task<IEnumerable<ChequeDueDateDTO>> GetFournisseurLCNDueDatesAsync(int month, int year, string view);
      Task<IEnumerable<ChequeDueDateDTO>> GetLCNDueDatesByDayAsync(DateTime date);
      Task<IEnumerable<ChequeDueDateDTO>> GetFournisseurLCNDueDatesByExactDateAsync(DateTime date);
       
    }
}
