namespace optique.Models
{
public class RefStatutDistribution
{
    public int Id { get; set; }
    public required string Libelle { get; set; }
    public  required string Code { get; set; }
    public required ICollection<Distribution> Distributions { get; set; }
}
}