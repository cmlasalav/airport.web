using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace ProyectoServiciosWeb.Models
{
    [Table("Bitacora")]
    public class Bitacora
    {
        [Key]

        public string Id_Bitacora { get; set; }
        public string Id_Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo_Movimiento { get; set; }
        public string Descripcion_Movimiento { get; set; }
    }
}
