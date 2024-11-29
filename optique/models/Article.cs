namespace optique.Models
{

public class Article
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public  string Reference { get; set; }
    public int MarqueId { get; set; }
    public  RefMarque? Marque { get; set; }
    public int TypeId { get; set; }
    public   RefType? Type { get; set; }
    public  string Couleur { get; set; }
    public int FournisseurId { get; set; }
    public  Fournisseur? Fournisseur { get; set; }
    public decimal PrixDeVenteInter { get; set; }
    public decimal PrixDeVenteRevendeur { get; set; }
    public decimal PrixDeVenteClientFinal { get; set; }

    // public  ICollection<DistributionDetails> DistributionDetails { get; set; }
    public  ICollection<ArrivageDetails> ArrivageDetails { get; set; }
    public  ICollection<Vente> Ventes { get; set; }

    public bool IsDeleted { get; set; }
    public int CodeBarre { get; set; }

// Constructeur vide
    public Article() { }

public Article(RefMarque marque, RefType type, Fournisseur fournisseur)
    {
        /*Marque = marque;
        Type = type;
        Fournisseur = fournisseur;*/

        Marque = marque ?? throw new ArgumentNullException(nameof(marque));
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Fournisseur = fournisseur ?? throw new ArgumentNullException(nameof(fournisseur));
    }

     
    
}

}