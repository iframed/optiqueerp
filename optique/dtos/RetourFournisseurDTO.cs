
namespace optique.Dtos
{

public class RetourFournisseurDTO
{
    public int Id { get; set; }
    public int ArrivageDetailsId { get; set; }
     public int QuantiteRetourne { get; set; }
   
    public  string? MotifRetour { get; set; }
    public int TypeRetourId { get; set; }

    public string? UserName { get; set; } 
    
    
public DateTime DateRetour { get; set; }




      
        
      
}

}