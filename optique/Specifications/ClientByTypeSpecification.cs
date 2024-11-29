using Ardalis.Specification;
using optique.Models;

public class ClientByTypeLibelleSpecification : Specification<Client>
{
    public ClientByTypeLibelleSpecification(string typeLibelle)
    {
        Query.Where(client => client.TypeClient.Libelle == typeLibelle);
    }
}
