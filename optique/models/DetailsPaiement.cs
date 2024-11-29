
namespace optique.Models
{
public class DetailsPaiement
{
    public int Id { get; set; }
    public int? VenteId { get; set; }
    public Vente? Vente { get; set; }

   
    public int? ArrivageId { get; set; }
    public Arrivage? Arrivage { get; set; }

    public  string? NCheque { get; set; }
    public  string? NLCN { get; set; }
    public  DateTime? DateEcheance { get; set; }
    public required decimal Montant { get; set; }
    public bool IsDeleted { get; set; }
    public int TypeDePaiementId { get; set; }
    public required RefTypeDePaiement TypeDePaiement { get; set; }

     

   
    
}
}
