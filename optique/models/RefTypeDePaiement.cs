namespace optique.Models
{

public class RefTypeDePaiement
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public required string Libelle { get; set; }
    public required ICollection<DetailsPaiement> DetailsPaiements { get; set; }
}

}