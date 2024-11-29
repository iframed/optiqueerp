namespace optique.Models
{
public class Client
{
    public int Id { get; set; }
    public required string NomClient { get; set; }
    public required string Adresse { get; set; }
    public required string NumTelephone { get; set; }
    public int TypeClientId { get; set; }
    public required RefTypeClient TypeClient { get; set; }
    public required ICollection<Distribution> Distributions { get; set; }
    public required ICollection<Vente> Ventes { get; set; }
    public bool IsDeleted { get; set; }
}
}