using Microsoft.EntityFrameworkCore;

namespace ProyectoServiciosWeb.Models
{
    public class UsuarioItem : DbContext
    {
        public UsuarioItem(DbContextOptions<UsuarioItem> options) 
            : base(options) 
        { 
        }

        public DbSet<Usuarios> Usuarios { get; set; } = null!;

    }
}
