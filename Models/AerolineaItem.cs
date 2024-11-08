using Microsoft.EntityFrameworkCore;

namespace ProyectoServiciosWeb.Models
{
    public class AerolineaItem : DbContext
    {
        public AerolineaItem(DbContextOptions<AerolineaItem> options)
           : base(options)
        {
        }

        public DbSet<Aerolinea> Aerolinea { get; set; } = null!;
    }
}
