using Microsoft.AspNetCore.Mvc.Rendering;
using optique.Dtos;
using System.Collections.Generic;






namespace optique.ViewModels
{
    public class ArticleViewModel
    {
        public IEnumerable<ArticleDTO> Articles { get; set; } = new List<ArticleDTO>();
        public IEnumerable<FournisseurDTO> Fournisseurs { get; set; } = new List<FournisseurDTO>();
        public IEnumerable<RefMarqueDTO> Marques { get; set; } = new List<RefMarqueDTO>();
        public IEnumerable<RefTypeDTO> Types { get; set; } = new List<RefTypeDTO>();

        public SelectList FournisseurList { get; set; }
        public SelectList MarqueList { get; set; }
        public SelectList TypeList { get; set; }

        public string SelectedFournisseur { get; set; } = string.Empty;
        public string SelectedMarque { get; set; } = string.Empty;
        public string SelectedType { get; set; } = string.Empty;
        public string SearchReference { get; set; } = string.Empty;

        public ArticleDTO NewArticle { get; set; } = new ArticleDTO("Default description", "Default color", "Default reference");
    }
}
