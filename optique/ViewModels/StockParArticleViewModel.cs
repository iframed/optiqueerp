using Microsoft.AspNetCore.Mvc.Rendering;
using optique.Dtos;

namespace optique.ViewModels
{
    public class StockParArticleViewModel
    {
        public IEnumerable<ArticleDTO> Articles { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public IEnumerable<SelectListItem> Fournisseurs { get; set; }
        public IEnumerable<SelectListItem> Marques { get; set; }
        public string SearchReference { get; set; }
        public string SelectedType { get; set; }
        public string SelectedFournisseur { get; set; }
        public string SelectedMarque { get; set; }

        public StockParArticleViewModel()
        {
            Articles = new List<ArticleDTO>();
            Types = new List<SelectListItem>();
            Fournisseurs = new List<SelectListItem>();
            Marques = new List<SelectListItem>();
        }
    }

    
}
