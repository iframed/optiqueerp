using System;
using System.Collections.Generic;

namespace optique.Models
{
    public class RetourFournisseur
    {
        public int Id { get; set; }
        public int QuantiteRetournee { get; set; }
        public required string MotifRetour { get; set; }
        public DateTime DateRetour { get; set; }
        public int TypeRetourId { get; set; }
        public required RefTypeRetour TypeRetour { get; set; }

        public int ArrivageDetailsId { get; set; }
        public ArrivageDetails? ArrivageDetails { get; set; } 
        public ICollection<MouvementArticle> MouvementArticles { get; set; } = new List<MouvementArticle>();

         public required string CreePar { get; set; }
          public bool IsDeleted { get; set; }
    }
}
