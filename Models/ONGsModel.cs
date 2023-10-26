using System.ComponentModel.DataAnnotations.Schema;

namespace SOSPets.Models
{
    [Table("ONGs")]
    public class ONGsModel
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("Nome")]
        public string Nome { get; set; }

        [Column("Descricao")]
        public string Descricao { get; set; }

        [Column("Endereco")]
        public string Endereco { get; set; }

        [Column("Cidade")]
        public string Cidade { get; set; }

        [Column("Tel")]
        public string Tel { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Data")]
        public DateTime Data { get; set; } = DateTime.Now;

        [Column("Imagem")]
        public string Imagem { get; set; }
    }
}
