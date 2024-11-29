using Ardalis.Specification;
using optique.Models;

namespace optique.Specifications
{
    public class ArticleByMarqueSpecification : Specification<Article>
    {
       public ArticleByMarqueSpecification(string libelle)
        {
            Query.Include(article => article.Marque)
                 .Where(article => article.Marque.Libelle == libelle);
        }
    }
}
