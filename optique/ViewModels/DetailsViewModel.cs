using optique.Dtos;
using System.Collections.Generic;


namespace optique.ViewModels
{
    public class DetailsViewModel
    {
        public ArrivageDTO Arrivage { get; set; }
        public string SocieteNom { get; set; } = string.Empty; 
    public string FournisseurNom { get; set; } = string.Empty; 

    public string StatutDistributionLibelle { get; set; }
        public List<DetailsPaiementViewModel> DetailsPaiements { get; set; } = new List<DetailsPaiementViewModel>();

        public bool IsValiderVisible {get ;set;}
    }
}
