namespace SOSPets.Models
{
    public class HomeViewModel
    {
        public ONGsModel ongs { get; set; }
        public List<ONGsModel> listOngs { get; set; }

        public AdocaoModel adocao { get; set; }
        public List<AdocaoModel> listAdocao { get; set; } 

        public EncontradosModel encontrados { get; set; }
        public List<EncontradosModel> listEncontrados { get; set; }

        public DesaparecidosModel desaparecidos { get; set; }
        public List<DesaparecidosModel> listDesaparecidos { get; set; }

        public UsuarioModel usuario { get; set; }
        public List<UsuarioModel> listUser { get; set; }
    }
}
