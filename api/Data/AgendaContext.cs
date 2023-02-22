using api.Models.Dbase;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class AgendaContext : IdentityDbContext<Usuarios>
    {
        public DbSet<Agendas> Agendas { get; set; }
        public DbSet<AgendasCompartilhadas> AgendasCompartilhadas { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public AgendaContext(DbContextOptions<AgendaContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AI");

            modelBuilder.Entity<AgendasCompartilhadas>(entity =>
            {
                entity.HasKey(k => new { k.IdAgenda, k.IdUsuario });
            });
        }
    }
}
