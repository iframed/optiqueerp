using optique.Dtos;

namespace optique.ViewModels
{
    public class StockParMagasinViewModel
    {
        public IEnumerable<ClientDTO> Clients { get; set; }
        public IEnumerable<FournisseurDTO> Fournisseurs { get; set; }
        public IEnumerable<RefMarqueDTO> Marques { get; set; }
        public IEnumerable<ArticleDTO> Articles { get; set; }
         public List<ArrivageDetailsDTO> ArrivageDetails { get; set; }


         public int QuantiteDisponible { get; set; }
}
    
}
