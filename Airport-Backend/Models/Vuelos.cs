using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoServiciosWeb.Models
{
    [Table("Vuelos")]
    public class Vuelos
    {
        [Key]
        public string Id_Vuelo { get; set; }

        [ForeignKey ("Aerolinea")]
        public string Aerolinea { get; set; }

        [ForeignKey ("Id_Pais")]
        public string Id_Pais { get; set;}

        public DateTime Fecha_Vuelo { get; set; }

        public TimeSpan Hora_vuelo { get; set; }

        public string Estado_Vuelo { get; set; }

        public decimal Precio_Vuelo { get; set; }

        [ForeignKey ("Puerta_Vuelo")]
        public string Puerta_Vuelo { get; set; }    

        public int Tipo_Vuelo { get; set; }


    }
}
