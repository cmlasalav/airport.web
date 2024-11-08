using Microsoft.EntityFrameworkCore;

namespace ProyectoServiciosWeb.Models
{
    public class ReservacionesItem: DbContext
    {
        public ReservacionesItem(DbContextOptions<ReservacionesItem> options)
            : base(options)
        {
        }

        public DbSet<Reservaciones> Reservaciones { get; set; } = null!;

    }
}
