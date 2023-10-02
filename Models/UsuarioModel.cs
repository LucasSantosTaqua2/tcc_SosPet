using SOSPets.Helper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOSPets.Models
{
    [Table("Usuario")]
    public class UsuarioModel
    {
        
        [Column("Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe seu nome!")]
        [Column("Nome")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe seu e-mail!")]
        [Column("Email")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "O e-mail inserido é invalido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe seu telefone!")]
        [Column("Tel")]
        [Display(Name = "Tel")]
        public string Tel { get; set; }

        [Required(ErrorMessage = "Informe sua senha!")]
        [Column("Password")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public void SetSenhaHash()
        {
            Password = Password.GerarHash();
        }


    }
}
