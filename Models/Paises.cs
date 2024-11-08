using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoServiciosWeb.Models
{
    [Table("Paises")]
    public class Paises
    {
        [Key]
        public string Id_Pais { get; set; }
        public string Siglas_Pais { get; set; }
        public string Descripcion_Pais { get; set; }
        public string Imagen_Pais { get; set; }
        

    }
}
