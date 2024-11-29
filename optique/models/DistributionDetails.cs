
namespace optique.Models
{
public class DistributionDetails
{
    public int Id { get; set; }
    public int DistributionId { get; set; }
   
    public required Distribution Distribution { get; set; }
    public int ArrivageDetailsId { get; set; }
    public required ArrivageDetails ArrivageDetails { get; set; }
    public int Quantite { get; set; }
    public decimal PrixDeVente { get; set; }
    public required string NumFacture { get; set; }
    public required ICollection<MouvementArticle> MouvementArticles { get; set; }
     public bool IsDeleted { get; set; }
   
 
   
}
}