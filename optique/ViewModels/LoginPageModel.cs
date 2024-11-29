using System.ComponentModel.DataAnnotations;

namespace optique.ViewModels
{
    public class LoginPageModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;  // Initialisation pour éviter NullReferenceException

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;  // Initialisation pour éviter NullReferenceException
    }
}
