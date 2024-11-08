using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoServiciosWeb.Models;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using ProyectoServiciosWeb.ProteccionDatos;
using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Any;


namespace ProyectoServiciosWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitacoraController : Controller
    {
        private readonly BitacoraItem _bitacoraContext;
        private readonly ClaveDatos _claveDatos;

        public BitacoraController(BitacoraItem BitacoraContext)
        {
            _bitacoraContext = BitacoraContext;
            _claveDatos = new ClaveDatos("ServiciosWeb123.");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bitacora>>> GetBitacora()
        {
            // Consulta SQL para obtener datos 
            var bitacora =
                await _bitacoraContext.Bitacora.FromSqlRaw
                (@"SELECT Id_Bitacora, 
	             Id_Usuario,
	    CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Fecha) as varchar(max)) as date) as Fecha,
	    CAST(DecryptByPassPhrase('ServiciosWeb2023.',Tipo_Movimiento) as varchar(max)) as Tipo_Movimiento,
        CAST(DecryptByPassPhrase('ServiciosWeb2023.',Descripcion_Movimiento) as varchar(max)) as Descripcion_Movimiento
        FROM Bitacora
        ORDER BY Fecha ASC;").ToListAsync();

            return bitacora;
        }
        [HttpGet("{Id_Usuario}")]
        public async Task<ActionResult<IEnumerable<Bitacora>>> GetBitacoraID(string Id_Usuario)
        {
            // Consulta SQL para obtener datos 
            var bitacora =
                await _bitacoraContext.Bitacora.FromSqlRaw
                (@"SELECT Id_Bitacora, 
	             Id_Usuario,
	    CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Fecha) as varchar(max)) as date) as Fecha,
	    CAST(DecryptByPassPhrase('ServiciosWeb2023.',Tipo_Movimiento) as varchar(max)) as Tipo_Movimiento,
        CAST(DecryptByPassPhrase('ServiciosWeb2023.',Descripcion_Movimiento) as varchar(max)) as Descripcion_Movimiento
        FROM Bitacora
        WHERE Id_Usuario = @Id_Usuario
        ORDER BY Fecha ASC",
        new SqlParameter("Id_Usuario",Id_Usuario)).ToListAsync();

            return bitacora;
        }



    }
}
