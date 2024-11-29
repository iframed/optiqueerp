namespace optique.Models
{
public class Fournisseur
{
    public int Id { get; set; }
    public required string NomFournisseur { get; set; }
    public required string Adresse { get; set; }
    public required string NumTelephone { get; set; }
    public required string ICE { get; set; }
    public required string Pays { get; set; }
    public int DeviseId { get; set; }
    public required RefDevise Devise { get; set; }
    public required ICollection<Article> Articles { get; set; }
    public required ICollection<Arrivage> Arrivages { get; set; }

     public bool IsDeleted { get; set; }
}
}