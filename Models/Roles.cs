using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoServiciosWeb.Models
{
    [Table("Roles")]
    public class Roles
    {
        [Key]
        public int Id_Rol { get; set; }

        public string Nombre_Rol { get; set; }

        public string Descripcion_Rol { get; set; }

    }
}