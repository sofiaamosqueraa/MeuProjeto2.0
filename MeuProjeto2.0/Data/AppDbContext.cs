using Microsoft.EntityFrameworkCore;
using MeuProjeto2._0.Models;

namespace MeuProjeto2._0.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<RespostaIA> RespostasIA { get; set; }
    }
}
