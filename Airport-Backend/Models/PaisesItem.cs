using Microsoft.EntityFrameworkCore;

namespace ProyectoServiciosWeb.Models
{
    public class PaisesItem : DbContext
    {
        public PaisesItem(DbContextOptions<PaisesItem> options)
           : base(options)
        {
        }

        public DbSet<Paises> Paises { get; set; } = null!;


    }
}
