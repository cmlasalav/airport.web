using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoServiciosWeb.Models
{
    [Table ("Tipo_Vuelo")]
    public class Tipo_Vuelo
    {
        [Key]
        public int Id_Tipo { get; set; }

        public string Descripcion_Tipo { get; set; }    


    }
}
