﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOSPets.Models
{
    [Table("Desaparecido")]
    public class DesaparecidosModel
    {
        [Column("Id")]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Column("Nome")]
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Campo requerido!")]
        public string Nome { get; set; }

        [Column("Descricao")]
        [Display(Name = "Descricao")]
        [Required(ErrorMessage = "Campo requerido!")]
        public string Descricao { get; set; }

        [Column("Cidade")]
        [Display(Name = "Cidade")]
        [Required(ErrorMessage = "Campo requerido!")]
        public string Cidade { get; set; }

        [Column("Data")]
        public DateTime Data { get; set; } = DateTime.Now;

        [Column("Imagem")]
        [Display(Name = "Imagem")]
        [Required(ErrorMessage = "Campo requerido!")]
        public string Imagem { get; set; }

        [Column("UsuarioId")]
        [Display(Name = "UsuarioId")]
        public int UsuarioId { get; set; }
        public UsuarioModel Usuario { get; set; }
    }
}
