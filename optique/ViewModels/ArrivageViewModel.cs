using Microsoft.AspNetCore.Mvc.Rendering;
using optique.Dtos;

namespace optique.ViewModels
{
public class ArrivageViewModel
{
    public IEnumerable<ArrivageDTO> Arrivages { get; set; } = new List<ArrivageDTO>();
    public SelectList FournisseurList { get; set; } = new SelectList(new List<SelectListItem>());
    public string SelectedFournisseur { get; set; }
    public string? SearchNumFacture { get; set; }
}
}