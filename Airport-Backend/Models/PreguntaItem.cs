using Microsoft.EntityFrameworkCore;

namespace ProyectoServiciosWeb.Models
{
    public class PreguntaItem : DbContext
    {
        public PreguntaItem(DbContextOptions<PreguntaItem> options)
           : base(options)
        {
        }

        public DbSet<Pregunta_Seguridad> Pregunta_Seguridad { get; set; } = null!;


    }
}