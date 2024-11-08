using Microsoft.EntityFrameworkCore;

namespace ProyectoServiciosWeb.Models
{
    public class TarjetaUsuarioItem : DbContext
    {
        public TarjetaUsuarioItem(DbContextOptions<TarjetaUsuarioItem> options)
            : base(options)
        {
        }
        public DbSet<TarjetaUsuario> TarjetaUsuario { get; set; } = null!;
    }
}