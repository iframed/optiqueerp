
namespace optique.Dtos
{
public class ClientDTO
{
    public int Id { get; set; }
    public required string NomClient { get; set; }
    public required string Adresse { get; set; }
    public required string Telephone { get; set; }
    public  string? TypeClientLibelle { get; set; }
    public int TypeClientId { get; set; }
    public List<DistributionDTO> Distributions { get; set; } = new List<DistributionDTO>();
}
}