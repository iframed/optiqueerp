namespace optique.Dtos
{
    public class FournisseurDetailsDTO
    {
        public string Societe { get; set; } = string.Empty;
        public string Fournisseur { get; set; } = string.Empty;
        public string NumFacture { get; set; } = string.Empty;
        public string NumBL { get; set; } = string.Empty;
        public DateTime DateArrivage { get; set; }
        public DateTime DateRetour { get; set; }
        public string Reference { get; set; } = string.Empty;
        public int QuantiteRetournee { get; set; }
        public string Marque { get; set; } = string.Empty;
        public decimal PrixDachatDeviseTTC { get; set; }
        public decimal PrixDachatMADTTC { get; set; }
        public string MotifRetour { get; set; } = string.Empty;
        public string TypeRetour { get; set; } = string.Empty;
    }
}
