namespace optique.Dtos
{
    public class ChequeDueDateDTO
    {
        public string ClientName  { get; set; } = string.Empty; 
         public string? ClientAcheteurName { get; set; }
        public DateTime? DateEcheance { get; set; }

        public string NCheque  { get; set; } = string.Empty; 
         public string NLCN  { get; set; } = string.Empty; 

         public decimal Amount { get; set; }

           public string FournisseurName { get; set; }

            public bool IsFromFournisseur { get; set; }
    }
}
