namespace optique.Dtos
{
    public class VenteDetailsDTO
    {
        public DateTime DateDeVente { get; set; }
        public string Client { get; set; }
        public string Marque { get; set; }
        public string Reference { get; set; }
        public decimal PrixDeVente { get; set; }
        public string NumBon { get; set; }
        public string? TypeArticle { get; set; }
        public int Quantite { get; set; }
        public string? TypeDePaiement { get; set; }
        public string? DetailsPaiement { get; set; }
        public List<string>? TypesDePaiement { get; set; } // Liste des types de paiements
    public List<string>? NCheques { get; set; } 

     public List<DetailsPaiementDTO>? DetailsPaiements { get; set; } 
     public string? ClientAcheteur { get; set; }
    }
}
