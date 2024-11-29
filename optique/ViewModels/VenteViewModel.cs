using optique.Dtos;

namespace optique.ViewModels
{
    public class VenteViewModel


    {


        public VenteDTO Vente { get; set; } = new VenteDTO
        {
            DetailsPaiements = new List<DetailsPaiementDTO>() // Initialisation ici
        };
        public IEnumerable<VenteDetailsDTO> VenteDetails { get; set; } = new List<VenteDetailsDTO>();
        public List<string> Clients { get; set; } = new List<string>();
        public List<string> Marques { get; set; } = new List<string>();
        public List<string> TypesArticle { get; set; } = new List<string>();
        public string? SelectedClient { get; set; }
        public string? SelectedMarque { get; set; }
        public string? SelectedTypeArticle { get; set; }
        public string? Reference { get; set; }
    }
}
