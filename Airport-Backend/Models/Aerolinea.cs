using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace ProyectoServiciosWeb.Models
{
    [Table("Aerolinea")]
    public class Aerolinea
    {
        [Key]
        [Required]
        public string ID_Aerolinea { get; set; }
        [Required]
        public string Siglas_Aerolinea { get; set; }

        public string Descripcion_Aerolinea { get; set; }   

        public string Imagen_Aerolinea { get; set; }    

    }
}
