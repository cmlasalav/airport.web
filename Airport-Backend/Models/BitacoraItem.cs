using Microsoft.EntityFrameworkCore;

namespace ProyectoServiciosWeb.Models
{
    public class BitacoraItem : DbContext
    {
        public BitacoraItem(DbContextOptions<BitacoraItem> options)
           : base(options)
        {
        }
        public DbSet<Bitacora> Bitacora { get; set; } = null!;
    }
}
