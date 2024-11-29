namespace optique.Models
{
public class Distribution
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public required Client Client { get; set; }
    public int SocieteId { get; set; }
    public required Societe Societe { get; set; }
    public DateTime DateDistribution { get; set; }
    public int StatutDistributionId { get; set; }
    public required RefStatutDistribution StatutDistribution { get; set; }
    public required ICollection<DistributionDetails> DistributionDetails { get; set; }
     public bool IsDeleted { get; set; }
}
}