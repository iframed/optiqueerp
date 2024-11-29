namespace optique.Models
{
    public class MouvementArticle
    {
        public int Id { get; set; }
        public int? ArrivageDetailsId { get; set; }
        public ArrivageDetails? ArrivageDetails { get; set; }

        public int? RetourFournisseurId { get; set; }
        public RetourFournisseur? RetourFournisseur { get; set; }

        public int? DistributionDetailsId { get; set; }
        public DistributionDetails? DistributionDetails { get; set; }

        public int? VenteId { get; set; }
        public Vente? Vente { get; set; }

        public int Quantite { get; set; }
        public DateTime DateDeCreation { get; set; }
         public bool IsDeleted { get; set; }

        public required string CreePar { get; set; } // Username of the user who performed the action
    }
}
