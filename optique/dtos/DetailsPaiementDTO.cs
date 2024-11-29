namespace optique.Dtos
{
public class DetailsPaiementDTO
{
    public int Id { get; set; }
    public int? VenteId { get; set; }

      public int? ArrivageId { get; set; }
   public  string? NCheque { get; set; } = string.Empty; // Initialisation par défaut
        public  string? NLCN { get; set; } = string.Empty; 
    public DateTime? DateEcheance { get; set; }
    public decimal Montant { get; set; }
    public int TypeDePaiementId { get; set; }

    public string? TypeDePaiement { get; set; }
     public bool IsDeleted { get; set; } 
    
   
}
}