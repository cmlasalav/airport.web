using Microsoft.EntityFrameworkCore;

namespace ProyectoServiciosWeb.Models
{
    public class Tipo_VueloItem : DbContext
    {
        public Tipo_VueloItem(DbContextOptions<Tipo_VueloItem> options) 
            : base(options) 
        { 
        }

        public DbSet<Tipo_Vuelo> Tipo_Vuelo { get; set; } = null!;

    }
}
