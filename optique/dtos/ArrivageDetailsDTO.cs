
namespace optique.Dtos
{
public class ArrivageDetailsDTO
{
    public int Id { get; set; }
    public int ArrivageId { get; set; }
    public int ArticleId { get; set; }
   
  
     public  string NumSerie { get; set; } = string.Empty;

      public decimal PrixDachatDevise { get; set; }
        public decimal PrixDachatMAD { get; set; }
        public decimal TauxRemise { get; set; }
        public decimal PrixAchatNetDevise { get; set; }

         public int QuantiteRecuParArticle { get; set; }

         public decimal PrixAchatNetMAD { get; set; }

         public string? UserName { get; set; }

             public decimal PrixDachatTotal { get; set; }
    public decimal PrixDachatRemise { get; set; }

    public string Description { get; set; }
   


   // Ajout des nouvelles propriétés
        public string Societe { get; set; } = string.Empty;  // Nom de la société
        public string Fournisseur { get; set; } = string.Empty;  // Nom du fournisseur
        public DateTime DateArrivage { get; set; }  // Date d'arrivée
        public string ReferenceProduit { get; set; } = string.Empty;  // Référence du produit
        public int QuantiteLivree { get; set; }  // Quantité livrée

       
}
}