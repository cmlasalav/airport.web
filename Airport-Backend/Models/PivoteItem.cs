using Microsoft.EntityFrameworkCore;

namespace ProyectoServiciosWeb.Models
{
    public class PivoteItem : DbContext
    {
        public PivoteItem(DbContextOptions<PivoteItem> options)
           : base(options)
        {
        }

        public DbSet<Pivote> Pivote { get; set; } = null!;


    }
}
