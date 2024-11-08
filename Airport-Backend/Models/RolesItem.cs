using Microsoft.EntityFrameworkCore;
namespace ProyectoServiciosWeb.Models
{
    public class RolesItem :DbContext
    {
        public RolesItem(DbContextOptions<RolesItem> options) 
            : base(options)
        { 
        }
        public DbSet<Roles> Roles { get; set; } = null!;
    }
}
