namespace optique.Dtos
{
    public class ArrivageDTO
    {
        public int Id { get; set; }
        public int SocieteId { get; set; } 
        public int FournisseurId { get; set; }
       public DateTime DateArrivage { get; set; }
        public string NumBL { get; set; } = string.Empty;
        public string NumFacture { get; set; } = string.Empty; 
        public decimal MontantFacture { get; set; } 
         public string FournisseurNom { get; set; } = string.Empty;
         public string SocieteNom { get; set; } = string.Empty;

           public List<DetailsPaiementDTO> DetailsPaiements { get; set; } = new List<DetailsPaiementDTO>();

        
        public int StatutId { get; set; }
        public string StatutDistributionLibelle { get; set; } = string.Empty;



         // Informations de paiement
    public int? TypeDePaiementId { get; set; }
    public string? NCheque { get; set; }
    public string? NLCN { get; set; }
    public DateTime? DateEcheance { get; set; }
    public decimal? Montant { get; set; }
     
    }
}