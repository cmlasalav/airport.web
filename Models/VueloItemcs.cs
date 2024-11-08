using Microsoft.EntityFrameworkCore; 

namespace ProyectoServiciosWeb.Models
{
    public class VueloItemcs : DbContext
    {
        public VueloItemcs (DbContextOptions<VueloItemcs> options)
            : base(options)
        {
        }

        public DbSet<Vuelos> Vuelos { get; set; } = null!;


    }
}
