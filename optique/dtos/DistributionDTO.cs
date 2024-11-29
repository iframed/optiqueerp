namespace optique.Dtos
{
public class DistributionDTO
{
    public int Id { get; set; }
    public DateTime DateDistribution { get; set; }

    public int ArticleId { get; set; }
    public int SocieteId { get; set; }
    public int ClientId { get; set; }
    public int StatutDistributionId { get; set; }
}
}