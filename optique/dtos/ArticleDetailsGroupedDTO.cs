namespace optique.Dtos
{
    public class ArticleDetailsGroupedDTO
    {
public string Description { get; set; }
        public string TypeLibelle { get; set; }
        public string MarqueLibelle { get; set; }
        public List<ArticleDTO> Articles { get; set; }
        public int TotalQuantiteLivree { get; set; }
        public int TotalQuantiteRestante { get; set; }
        public string FournisseurNom { get; set; }
        public string Couleur { get; set; }
        public string Reference { get; set; }
        public string CodeBarre { get; set; }
        public decimal PrixDeVenteRevendeur { get; set; }
        public decimal PrixDeVenteClientFinal { get; set; }

        public ArticleDetailsGroupedDTO()
        {
            Articles = new List<ArticleDTO>();
        }
    }
}
