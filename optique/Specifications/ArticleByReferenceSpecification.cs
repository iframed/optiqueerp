using Ardalis.Specification;
using optique.Models;

namespace optique.Specifications
{
    public class ArticleByReferenceSpecification : Specification<Article>
    {
        public ArticleByReferenceSpecification(string reference)
        {
            Query.Where(article => article.Reference == reference);
        }
       

        
    }
}
