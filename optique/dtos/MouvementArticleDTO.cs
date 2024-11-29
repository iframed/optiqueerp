using System;

namespace optique.Dtos
{
    public class MouvementArticleDTO
    {
        public int Id { get; set; }
        public int VenteId { get; set; }
       
       public string VenteCreePar { get; set; } = string.Empty;
        public DateTime DateMouvement { get; set; }
        public int Quantite { get; set; }
    }
}
