using Microsoft.AspNetCore.Mvc.Rendering;
using optique.Dtos;

namespace optique.ViewModels
{
    public class DistributionCreateViewModel
    {
        // Champs pour Distribution
        public int? ArrivageDetailsId { get; set; }
        public DateTime DateDistribution { get; set; }
        public int ClientId { get; set; }
        public int StatutDistributionId { get; set; }

        // Champs pour DistributionDetails
        public int Quantite { get; set; }
        public decimal PrixDeVente { get; set; }
        public string NumFacture { get; set; }

        // Nouvelles Propriétés à Ajouter
        public int ArticleId { get; set; }
        public int SocieteId { get; set; }

         public ArticleDetailsDTO? ArticleDetails { get; set; }


        // Listes de sélection pour les dropdowns
        public IEnumerable<SelectListItem> Clients { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> StatutsDistribution { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> ArrivageDetails { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Societes { get; set; } = new List<SelectListItem>(); // Ajoutez cette ligne
    }
}
