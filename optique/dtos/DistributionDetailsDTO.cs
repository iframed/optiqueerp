namespace optique.Dtos
{
public class DistributionDetailsDTO
{
    public int Id { get; set; }
    public int DistributionId { get; set; }
    public int ArrivageDetailsId { get; set; }
    public int Quantite { get; set; }
    public decimal PrixDeVente{ get; set; }
     public required string Statut { get; set; } 
    public required string NumFacture { get; set; }

}
}