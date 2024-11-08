using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MetodosPago.Models
{
    [Table("Tarjeta")]
    public class Tarjeta
    {
        [Key]
        public string Id_Registro { get; set; }
        public string Numero_Tarjeta { get; set; }
        public int Mes_Exp { get; set; }
        public int Year_Exp { get; set; }
        public int CVV { get; set; }
        public int Total { get; set; }
        public string Estado_Pago { get; set; }
        public string Tipo_Tarjeta { get; set; }





    }
}
