

namespace optique.Models
{
    public class ArrivageDetails
    {
        public int Id { get; set; }
        public int ArrivageId { get; set; }
        public required Arrivage Arrivage { get; set; }
        public int ArticleId { get; set; }
        public required Article Article { get; set; }
        public required string NumSerie { get; set; }
        public int QuantiteRecuParArticle { get; set; }
        public decimal PrixDachatDevise { get; set; }
        public decimal PrixDachatMAD { get; set; }
        public decimal TauxRemise { get; set; }
        public decimal PrixAchatNetDevise { get; set; }
        public decimal PrixAchatNetMAD { get; set; }
        public required ICollection<DistributionDetails> DistributionDetails { get; set; }

        public required ICollection<MouvementArticle> MouvementArticles { get; set; } 

        public bool IsDeleted { get; set; }

        public DateTime DateArrivage { get; set; }  // Date d'arrivée

        public required string CreePar { get; set; }
       // public int RetourFournisseurId { get; set; }
        public RetourFournisseur? RetourFournisseur { get; set; }

        public ICollection<RetourFournisseur>? RetourFournisseurs { get; set; } 

         public int FichierId { get; set; }


    }
}
