namespace optique.Models
{
public class RefMarque
{
    public int Id { get; set; }
    public required string Libelle { get; set; }
    public required string Code { get; set; }
    public bool AvecNumSerie { get; set; }
    public required ICollection<Article> Articles { get; set; }
}
}