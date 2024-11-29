namespace optique.Dtos
{
public class FournisseurDTO
{
    public int Id { get; set; }
    public required string NomFournisseur { get; set; }
    public required string Adresse { get; set; }
    public required string Telephone { get; set; }
   
    public int DeviseId { get; set; }
    public required string ICE { get; set; }
    public required string Pays { get; set; }

    public required string DeviseLibelle { get; set; } 
        public required string DeviseCode { get; set; } 
}}