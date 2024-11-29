namespace optique.Dtos
{
    public class DistributionSummaryDTO
    {
        public string Fournisseur { get; set; } = string.Empty;
        public string Client { get; set; } = string.Empty;
        public string Marque { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public DateTime DateDistribution { get; set; }
        public int Quantite { get; set; }
        public decimal PrixDeVente { get; set; }
        public string NumFacture { get; set; } = string.Empty;
        public string Statut { get; set; } = string.Empty;
    }
}
