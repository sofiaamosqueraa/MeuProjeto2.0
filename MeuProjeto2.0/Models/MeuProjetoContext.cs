using Microsoft.EntityFrameworkCore;
using MeuProjeto2._0.Models;

namespace MeuProjeto2._0
{
    public class MeuProjetoContext : DbContext
    {
        public MeuProjetoContext(DbContextOptions<MeuProjetoContext> options) : base(options) { }

        public DbSet<FormDataModel> Formularios { get; set; }
        public DbSet<RespostaIaModel> RespostasIA { get; set; }  // NOVO
    }
}
