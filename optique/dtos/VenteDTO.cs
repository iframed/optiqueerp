using System.Collections.Generic;

namespace optique.Dtos
{
public class VenteDTO
{
    public int Id { get; set; }
    public DateTime DateDeVente { get; set; }
    public decimal PrixDeVente { get; set; }
    public int QuantiteVendu { get; set; }
    public int ArticleId { get; set; }
    public int ClientId { get; set; }
   // public int DeviseId { get; set; }
public string? UserName { get; set; }
 public decimal? Remise { get; set; } 

    // public required string CreePar { get; set; } 

   // Initialisation de la collection obligatoire dans le constructeur
        public required List<DetailsPaiementDTO> DetailsPaiements { get; set; }

        public VenteDTO()
        {
            DetailsPaiements = new List<DetailsPaiementDTO>();
        }
}
}