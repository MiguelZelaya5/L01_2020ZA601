using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
namespace L01_2020ZA601.Models
{
    public class blogDbContext : DbContext
    {
        public blogDbContext(DbContextOptions<blogDbContext> options) : base(options)
        { }

        public DbSet<usuarios> usuarios { get; set; }
        public DbSet<calificaciones> calificaciones { get;set; }
        public DbSet<comentarios> comentarios { get; set; } 
    }
}
