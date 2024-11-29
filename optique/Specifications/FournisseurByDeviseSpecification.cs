using Ardalis.Specification;
using optique.Models;

public class FournisseurByDeviseSpecification : Specification<Fournisseur>
{
    public FournisseurByDeviseSpecification(string deviseLibelle)
    {
        Query.Where(f => f.Devise.Libelle == deviseLibelle);
    }
}
