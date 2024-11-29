namespace optique.Models
{
public class RefTypeRetour
{
    public int Id { get; set; }
    public required string Libelle { get; set; }
    public  required string Code { get; set; }
    public required ICollection<RetourFournisseur> RetourFournisseurs { get; set; }
}
}