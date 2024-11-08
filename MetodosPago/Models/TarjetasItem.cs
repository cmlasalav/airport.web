using Microsoft.EntityFrameworkCore;

namespace MetodosPago.Models
{
    public class TarjetasItem : DbContext
    {
        public TarjetasItem(DbContextOptions<TarjetasItem> options)
            : base(options)
        {
        }
        public DbSet<Tarjeta> Tarjeta { get; set; } = null!;
    }
}
