using Ardalis.Specification;
using optique.Models;

namespace optique.Specifications
{
    public class ArticleByFournisseurAdresseSpecification : Specification<Article>
    {
        public ArticleByFournisseurAdresseSpecification(string adresse)
        {
            Query.Include(article => article.Fournisseur)
                 .Where(article => article.Fournisseur.Adresse == adresse);
        }
    }
}
