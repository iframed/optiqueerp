using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace optique.ViewModels
{
public class RetourFournisseurViewModel
{
    public int Id { get; set; }
    [Required]
    public int ArrivageDetailsId { get; set; }
    [Display(Name = "Date retour")]
    [Required(ErrorMessage = "La date de retour est obligatoire.")]
    public DateTime DateRetour { get; set; }
    [Display(Name = "Quantité retournée")]
    [Required(ErrorMessage = "La quantité retournée est obligatoire.")]
    [Range(1, int.MaxValue, ErrorMessage = "La quantité retournée doit être supérieure à 0.")]
    public int QuantiteRetourne { get; set; }
    [Display(Name = "Motif de retour")]
    [StringLength(500, ErrorMessage = "Le motif de retour ne peut pas dépasser 500 caractères.")]
    public string? MotifRetour { get; set; }
    [Display(Name = "Type de retour")]
    [Required(ErrorMessage = "Le type de retour est obligatoire.")]
    public int TypeRetourId { get; set; }
    public string? UserName { get; set; }
    public string Societe { get; set; } = "Non spécifié";
    public string Fournisseur { get; set; } = "Non spécifié";
    public DateTime? DateArrivage { get; set; }  // Changer à DateTime?
    public decimal MontantFacture { get; set; }
    public string ReferenceProduit { get; set; } = "Non spécifié";
    public int? QuantiteLivree { get; set; }  // Utiliser un type nullable ici aussi
    public List<SelectListItem> TypeRetours { get; set; } = new List<SelectListItem>();
}
}