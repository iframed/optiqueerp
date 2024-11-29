using Ardalis.Specification;
using optique.Models;

public class SocieteByNameSpecification : Specification<Societe>
{
    public SocieteByNameSpecification(string nomSociete)
    {
         Query.Where(s => s.NomSociete == nomSociete);
    }
}

public class SocieteByAdresseSpecification : Specification<Societe>
{
    public SocieteByAdresseSpecification(string adresse)
    {
        Query.Where(s => s.Adresse == adresse);
    }
}
