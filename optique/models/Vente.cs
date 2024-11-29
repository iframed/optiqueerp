using System.ComponentModel.DataAnnotations.Schema;

namespace optique.Models
{
public class Vente
{
    public int Id { get; set; }
    public DateTime DateDeVente { get; set; }
    public decimal PrixDeVente { get; set; }
    public int QuantiteVendu { get; set; }
    public int ArticleId { get; set; }
    public required Article Article { get; set; }
    public required int ClientId { get; set; }
    public required Client Client { get; set; }
    public  string CreePar { get; set; }
    public required ICollection<DetailsPaiement> DetailsPaiements { get; set; }

     public required ICollection<MouvementArticle> MouvementArticles { get; set; } 

  
// Client acheteur (particulier)
    public int? ClientIdAcheteur { get; set; }

    [ForeignKey("ClientIdAcheteur")]
    public  Client? ClientAcheteur { get; set; }

    public int NDeBon { get; set; }

    public bool IsDeleted { get; set; }




}
}