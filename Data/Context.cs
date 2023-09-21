using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SOSPets.Models;

namespace SOSPets.Data
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {

        }

        public DbSet<UsuarioModel> UsuarioModels { get; set; }
        public DbSet<AdocaoModel> AdocaoModel { get; set; }
        public DbSet<EncontradosModel> EncontradosModels { get; set; }
        public DbSet<DesaparecidosModel> DesaparecidosModel { get; set; }
    }
}
