using SOSPets.Controllers;
using System;
using System.ComponentModel.DataAnnotations;
using SOSPets.Controllers;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Web;
using SOSPets.Controllers;
using SOSPets.Models;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace SOSPets.Models
{

    [Table("Adocao")]
    public class AdocaoModel
    {

        [Column("Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Column("Nome")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Column("Peso")]
        [Display(Name = "Peso")]
        public double Peso { get; set; }

        [Column("Porte")]
        [Display(Name = "Porte")]
        public string Porte { get; set; }

        [Column("Sexo")]
        [Display(Name = "Sexo")]
        public string Sexo { get; set; }

        [Column("Raca")]
        [Display(Name = "Raca")]
        public string Raca { get; set;}

        [Column("Idade")]
        [Display(Name = "Idade")]
        public string Idade { get; set; }

        [Column("Cor")]
        [Display(Name = "Cor")]
        public string Cor { get; set; }

        [Column("Cidade")]
        [Display(Name = "Cidade")]
        public string Cidade { get; set; }

        [Column("Data")]
        [Display(Name = "Data")]
        public DateTime Data { get; set; } = DateTime.Now;

        [Column("Imagem")]
        [Display(Name = "Imagem")]
        public string Imagem { get; set; }

        [Column("UsuarioId")]
        [Display(Name = "UsuarioId")]
        public int UsuarioId { get; set; }
        public UsuarioModel Usuario { get; set; }
    }
}
