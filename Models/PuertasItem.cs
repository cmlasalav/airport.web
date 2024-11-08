using Microsoft.EntityFrameworkCore;

namespace ProyectoServiciosWeb.Models
{
    public class PuertasItem : DbContext
    {
        public PuertasItem(DbContextOptions<PuertasItem> options)
           : base(options)
        {
        }

        public DbSet<Puertas_Aeropuerto> Puertas_Aeropuerto { get; set; } = null!;


    }
}
