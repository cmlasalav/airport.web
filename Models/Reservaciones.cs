using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoServiciosWeb.Models
{
    [Table("Reservaciones")]
    public class Reservaciones
    {
        [Key]
        public string Id_Reservacion { get; set; }

        [ForeignKey ("Id_Usuario")]
        public string Id_Usuario { get; set; }

        [ForeignKey ("Id_Vuelo")]
        public string Id_Vuelo { get; set;}

        public int Numero_Boletos { get; set; }

        public decimal Total { get; set; }

        public string Estado_Reservacion { get; set; }


    }
}