using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoServiciosWeb.Models
{
    [Table("Pregunta_Seguridad")]
    public class Pregunta_Seguridad
    {
        [Key]
        public int Id_Pregunta { get; set; }
        public string Descripcion_Pregunta { get; set; }

    }
}