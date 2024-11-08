using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoServiciosWeb.Models
{
    [Table("Tarjeta")]
    public class TarjetaUsuario
    {
        [Key]
        public string Id_RegistroTarjeta { get; set; }
        [ForeignKey("Id_Usuario")]
        public string Id_Usuario { get; set; }
        public string Numero_Tarjeta { get; set; }
        public int Mes_Exp { get; set; }
        public int Year_Exp { get; set; }
        public int CVV { get; set; }
        public string Tipo_Tarjeta { get; set; }

    }
}
