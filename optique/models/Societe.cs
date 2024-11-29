namespace optique.Models
{
public class Societe
{
    public int Id { get; set; }
    public required string NomSociete { get; set; }
    public required string Adresse { get; set; }
    public required string NumTelephone { get; set; }
    public required ICollection<Arrivage> Arrivages { get; set; }
    public required ICollection<Distribution> Distributions { get; set; }
     public bool IsDeleted { get; set; }
}
}