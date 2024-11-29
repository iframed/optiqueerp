using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using optique.Dtos;

namespace optique.ViewModels
{
    public class ArrivageDetailsFullViewModel
    {
        // Propriétés existantes
        public int ArrivageId { get; set; }
        public DateTime DateArrivage { get; set; }
        public string NumBL { get; set; } = string.Empty;
        public string NumFacture { get; set; } = string.Empty;
        public decimal MontantFacture { get; set; }


     // Propriétés pour les noms du fournisseur et de la société
    public string SocieteNom { get; set; } = string.Empty; 
    public string NomFournisseur { get; set; } = string.Empty;

        public string? Marque { get; set; }

        [BindNever]
        public SelectList ArticlesList { get; set; } = new SelectList(new List<SelectListItem>());
        
        // Propriétés manquantes à ajouter
        public int SocieteId { get; set; } // Ajoutez cette propriété pour Societe
        public int FournisseurId { get; set; } // Ajoutez cette propriété pour Fournisseur
       

        // Ajoutez également toutes les autres propriétés nécessaires pour le formulaire
        public int ArticleId { get; set; }
        public string NumSerie { get; set; } = string.Empty;
        public decimal PrixDachatDevise { get; set; }
        public decimal PrixDachatMAD { get; set; }
        public decimal TauxRemise { get; set; }
        public decimal PrixAchatNetDevise { get; set; }

            public string Description { get; set; } = string.Empty;

        public int QuantiteRecuParArticle { get; set; }
        public decimal PrixAchatNetMAD { get; set; }
        public string? UserName { get; set; }
        
        // Nouvelle propriété pour la liste des articles
      

         public List<ArrivageDetailsDTO> ArrivageDetails { get; set; } = new List<ArrivageDetailsDTO>();
    }
}
