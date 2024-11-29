namespace optique.Dtos
{
    public class ArticleDetailsDTO
    {
       


        public int Id { get; set; }
        public int ArrivageId { get; set; }
        public int ArticleId { get; set; }
        public string Societe { get; set; } = string.Empty;
        public string Fournisseur { get; set; } = string.Empty;
        public DateTime DateArrivage { get; set; }
        public string Marque { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Livre { get; set; }
        public int Retourne { get; set; }
        
         public int Vendu { get; set; }
        public int Distribue { get; set; }
        public int Reste { get; set; }
        public decimal PrixAchatNetDevise { get; set; }
        public decimal PrixAchatNetMAD { get; set; }

        
    }
}
