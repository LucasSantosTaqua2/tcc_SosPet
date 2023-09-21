using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOSPets.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Informe seu e-mail!")]
        [EmailAddress(ErrorMessage = "O e-mail inserido é invalido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe sua senha!")]
        public string Password { get; set; }
    }
}
