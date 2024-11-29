using System.Collections.Generic;
using optique.Dtos;
namespace optique.ViewModels
{
public class DetailsPaiementViewModel
{
    public int TypeDePaiementId { get; set; }
    public string TypeDePaiementLibelle { get; set; } = string.Empty;  // Pour l'affichage
    public string NCheque { get; set; } = string.Empty;
    public string NLCN { get; set; } = string.Empty;
    public decimal Montant { get; set; }
    public DateTime? DateEcheance { get; set; }
}
}