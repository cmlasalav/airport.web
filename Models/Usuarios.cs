using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ProyectoServiciosWeb.Models
{
    [Table("Usuarios")]
    public class Usuarios
    {
        [Key]
        public string Id_Usuario{ get; set; }
        public string Primer_Apellido { get; set; }
        public string Segundo_Apellido { get; set; }
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string Correo_Usuario { get; set; }
        public string Password_Usuario { get; set; }

        [ForeignKey ("Id_Pregunta")]
        public int Id_Pregunta { get; set; }
        public string Respuesta_Seguridad { get; set; }

        [ForeignKey ("Id_Rol")]
        public int Id_Rol { get; set;}

    }
}
