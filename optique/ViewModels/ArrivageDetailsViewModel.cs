using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace optique.ViewModels
{
    public class ArrivageDetailsIndexViewModel
    {
        // Propriété pour les détails d'arrivage
        public IEnumerable<ArrivageDetailsViewModel> ArrivageDetails { get; set; } = new List<ArrivageDetailsViewModel>();

        // Listes pour les filtres
        public SelectList SocieteList { get; set; }
        public SelectList FournisseurList { get; set; }
        public SelectList MarqueList { get; set; }

        public string SelectedSociete { get; set; }
        public string SelectedFournisseur { get; set; }
        public string SelectedMarque { get; set; }
        public string SearchReference { get; set; }
    }

    public class ArrivageDetailsViewModel
    {
        public int Id { get; set; }
        public int ArrivageId { get; set; }
        public int ArticleId { get; set; }
        public string Societe { get; set; } = string.Empty;
        public string Fournisseur { get; set; } = string.Empty;
        public DateTime DateArrivage { get; set; }
        public string Marque { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Livre { get; set; }
        public int Retourne { get; set; }
        public int Vendu { get; set; }
        public int Distribue { get; set; }
        public int Reste { get; set; }
        public decimal PrixAchatNetDevise { get; set; }
        public decimal PrixAchatNetMAD { get; set; }

        // Propriétés ajoutées pour correspondre à l'interface utilisateur
        public string NumBL { get; set; } = string.Empty;  // Numéro de Bon de Livraison
        public string NumFacture { get; set; } = string.Empty;  // Numéro de Facture
        public decimal MontantFacture { get; set; }  // Montant de la Facture

        // Propriétés pour les détails d'article
        public string NumSerie { get; set; } = string.Empty;
        public int QuantiteRecu { get; set; }
        public decimal PrixAchat { get; set; }
        public decimal TauxRemise { get; set; }
    }
}
