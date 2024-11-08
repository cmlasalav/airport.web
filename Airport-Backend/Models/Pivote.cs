using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoServiciosWeb.Models
{
    [Table("Pivote")]
    public class Pivote
    {
        [Key]
        public int ID { get; set; }
        public string Descripcion { get; set; }
        public int Consecutivo { get; set; }
        public string Prefijo { get; set; }
        public int RangoInicial { get; set; }
        public int RangoFinal { get; set; }
                        
    }
}
