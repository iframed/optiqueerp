using Ardalis.Specification;
using optique.Models;

public class FournisseurByICESpecification : Specification<Fournisseur>
{
    public FournisseurByICESpecification(string ice)
    {
        Query.Where(f => f.ICE == ice);
    }
}
