using optique.Models;

public class RefDevise
{
    public int Id { get; set; }
    public string Libelle { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    
    
    public ICollection<Fournisseur> Fournisseurs { get; set; }
   
   
    //public ICollection<DetailsPaiement> DetailsPaiements { get; set; }

    // Constructeur qui initialise les collections
    public RefDevise()
    {
        Fournisseurs = new List<Fournisseur>();
      
       // DetailsPaiements = new List<DetailsPaiement>();
    }
}
