using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MetodosPago.Models
{
    [Table("Cuenta_Banco")]
    public class Cuenta_Banco
    {
        [Key]
        public string Id_RegistroCuenta { get; set; }

        public string Num_Cuenta { get; set; }

        public int Codigo { get; set; }

        public string Password_Cuenta { get; set; }

    }
}