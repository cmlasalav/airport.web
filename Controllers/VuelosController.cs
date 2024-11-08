using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoServiciosWeb.Models;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace ProyectoServiciosWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VuelosController : ControllerBase
    {
        private readonly VueloItemcs _vuelosContext;


        public VuelosController(VueloItemcs VuelosContext)
        {
            _vuelosContext = VuelosContext;
        }

        [HttpGet("testdb")]
        public IActionResult TestDatabaseConnection()
        {
            try
            {
                // Intenta obtener un registro de alguna tabla
                var data = _vuelosContext.Vuelos.Take(1).ToList();

                // Verifica si se obtuvo algún dato
                if (data.Any())
                {
                    return Ok("Conexión exitosa a la base de datos");
                }
                else
                {
                    return StatusCode(500, "La tabla está vacía");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error de conexión a la base de datos: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vuelos>>> GetVuelos()
        {
            // Consulta SQL para obtener datos
            var vuelos= await _vuelosContext.Vuelos.FromSqlRaw(@"SELECT 
	        Id_Vuelo,
	        Aerolinea,
	        Id_Pais,
	        CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Fecha_Vuelo) as varchar(50)) as date) as Fecha_Vuelo,
	        CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Hora_Vuelo) as varchar(50)) as time) as Hora_Vuelo,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Estado_Vuelo) as varchar(max)) as Estado_Vuelo,
	        CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Precio_Vuelo) as varchar(50)) as decimal(6,2)) as Precio_Vuelo,
	        Puerta_Vuelo,
            Tipo_Vuelo
            FROM Vuelos;").ToListAsync();

            return vuelos;
        }



    }
        
    

 
}
