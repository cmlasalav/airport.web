using Microsoft.EntityFrameworkCore;

namespace MetodosPago.Models
{
    public class Cuenta_BancoItem : DbContext
    {
        public Cuenta_BancoItem(DbContextOptions<Cuenta_BancoItem> options)
            : base(options) 
        { 
        }
        public DbSet<Cuenta_Banco> Cuenta_Banco { get; set; } = null!; 
    }
}
