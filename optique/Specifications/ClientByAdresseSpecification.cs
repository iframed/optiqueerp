using Ardalis.Specification;
using optique.Models;

public class ClientByAdresseSpecification : Specification<Client>
{
    public ClientByAdresseSpecification(string adresse)
    {
        Query.Where(client => client.Adresse.Contains(adresse));
    }
}
