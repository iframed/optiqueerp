using Ardalis.Specification;
using optique.Models;

namespace optique.Specifications
{
    public class ArticleByTypeLibelleSpecification : Specification<Article>
    {
        public ArticleByTypeLibelleSpecification(string libelle)
        {
            Query.Include(article => article.Type)
                 .Where(article => article.Type.Libelle == libelle);
        }
    }
}
