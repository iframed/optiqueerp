
namespace optique.Models
{
public class RefTypeClient
{
    public int Id { get; set; }
    public  required string Libelle { get; set; }
    public required string Code { get; set; }
    public required ICollection<Client> Clients { get; set; }
}
}