namespace optique.Models
{
    public class Arrivage
    {
        public int Id { get; set; }
        public DateTime DateArrivage { get; set; }
        public int SocieteId { get; set; }
        public Societe Societe { get; set; } = default!; 
        public int FournisseurId { get; set; }
        public Fournisseur Fournisseur { get; set; } = default!; 
        public string NumBL { get; set; } = string.Empty; 
        public string NumFacture { get; set; } = string.Empty; 
        public decimal MontantFacture { get; set; }
        public ICollection<ArrivageDetails> ArrivageDetails { get; set; } = new List<ArrivageDetails>();

          public ICollection<DetailsPaiement> DetailsPaiements { get; set; } = new List<DetailsPaiement>();

          public int? StatutId { get; set; }
        public RefStatutDistribution? RefStatutDistribution { get; set; }
        public bool IsDeleted { get; set; }

       
}
}