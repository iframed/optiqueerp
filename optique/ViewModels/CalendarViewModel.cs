using optique.Dtos;


namespace optique.ViewModels
{
    public class CalendarViewModel
    {
        public IEnumerable<ChequeDueDateDTO> ClientCheques { get; set; }
        public IEnumerable<ChequeDueDateDTO> FournisseurCheques { get; set; }


        public IEnumerable<ChequeDueDateDTO> ClientLCNs { get; set; } = new List<ChequeDueDateDTO>();
        public IEnumerable<ChequeDueDateDTO> FournisseurLCNs { get; set; } = new List<ChequeDueDateDTO>();
    }
}
