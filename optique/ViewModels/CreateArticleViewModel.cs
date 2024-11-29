using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace optique.ViewModels
{
    public class CreateArticleViewModel
    {
        [Required(ErrorMessage = "La description est requise.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "La référence est requise.")]
        public string Reference { get; set; }

        public string Couleur { get; set; }

        [Required(ErrorMessage = "Veuillez sélectionner une marque.")]
        public int MarqueId { get; set; }

        [BindNever]  // Empêche la validation de la liste
        public SelectList MarqueList { get; set; }

        [Required(ErrorMessage = "Veuillez sélectionner un type.")]
        public int TypeId { get; set; }

        [BindNever]  // Empêche la validation de la liste
        public SelectList TypeList { get; set; }

        [Required(ErrorMessage = "Veuillez sélectionner un fournisseur.")]
        public int FournisseurId { get; set; }

        [BindNever]  // Empêche la validation de la liste
        public SelectList FournisseurList { get; set; }

        [Required(ErrorMessage = "Le prix de vente intermédiaire est requis.")]
        public decimal PrixDeVenteInter { get; set; }

        [Required(ErrorMessage = "Le prix de vente revendeur est requis.")]
        public decimal PrixDeVenteRevendeur { get; set; }

        [Required(ErrorMessage = "Le prix de vente client final est requis.")]
        public decimal PrixDeVenteClientFinal { get; set; }
    }
}
