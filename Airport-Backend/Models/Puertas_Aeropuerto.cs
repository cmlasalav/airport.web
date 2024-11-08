using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoServiciosWeb.Models
{
    [Table("Puertas_Aeropuerto")]
    public class Puertas_Aeropuerto
    {
        [Key]
        public string ID_Puerta{ get; set; }
        public int Numero_Puerta { get; set; }
        public string Estado_Puerta { get; set; }

    }
}