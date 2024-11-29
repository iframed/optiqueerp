using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using optique.Dtos;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace optique.ViewModels
{
    public class NouvelleVenteViewModel
{
    public DateTime DateDeVente { get; set; }
        public decimal PrixDeVente { get; set; }
        public int QuantiteVendu { get; set; }
        public int ArticleId { get; set; }
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Le champ CreePar est requis.")]
        public string CreePar { get; set; }

        // Nouvel acheteur (client particulier)
    public int? ClientIdAcheteur { get; set; }

    public int NDeBon { get; set; } 

 public ArticleDTO Article { get; set; }

       
        public List<DetailsPaiementDTO> DetailsPaiements { get; set; } = new();


[BindNever]
        public string Marque { get; set; }
        [BindNever]
    public string Reference { get; set; }
    [BindNever]
    public string Couleur { get; set; }
    [BindNever]
    public string TypeArticle { get; set; }
    public int QuantiteDisponible { get; set; }
}

}
