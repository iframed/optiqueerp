

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace optique.Dtos
{
    public class ArticleDTO
    {
        public int Id { get; set; }
    
   
    public string Description { get; set; }

    public decimal PrixDeVenteInter { get; set; }
    public decimal PrixDeVenteRevendeur { get; set; }
    public decimal PrixDeVenteClientFinal { get; set; }

   
    public int MarqueId { get; set; }
    [BindNever]
    public string? MarqueLibelle { get; set; }

   
    public int TypeId { get; set; }
    [BindNever]
    public string? TypeLibelle { get; set; }

    public string Couleur { get; set; }

    
    public int FournisseurId { get; set; }
    [BindNever]
    public string? FournisseurNom { get; set; }

    public string Reference { get; set; }

    // Champs non requis
    [BindNever]
    public int QuantiteDistribuee { get; set; }

    [BindNever]
    public string? ClientNom { get; set; }

    [BindNever]
    public int ClientId { get; set; }

    [BindNever]
    public string? TypeClientLibelle { get; set; }

    [BindNever]
    public int QuantiteRestante { get; set; }

    // Lists for dropdowns, not to be bound by the form
    
   



 public ArticleDTO()
    {
    }

  
    

        public ArticleDTO(string description, string couleur, string reference)
        {
            Description = description;
            Couleur = couleur;
            Reference = reference;
        }

       
    }
}
