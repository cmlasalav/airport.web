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
    public class AerolineaController : ControllerBase
    {
        private readonly AerolineaItem _aerolineaContext;
        private readonly ClaveDatos _claveDatos;

        public AerolineaController(AerolineaItem AerolinaContext)
        {
            _aerolineaContext = AerolinaContext;
            _claveDatos = new ClaveDatos("ServiciosWeb123.");


        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aerolinea>>> GetAerolineas()
        {
            // Consulta SQL para obtener datos 
            var aerolineas = await _aerolineaContext.Aerolinea.FromSqlRaw(@"SELECT ID_Aerolinea, 
	    CAST(DecryptByPassPhrase('ServiciosWeb2023.', Siglas_Aerolinea) as varchar(max)) as Siglas_Aerolinea,
	    CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion_Aerolinea) as varchar(max)) as Descripcion_Aerolinea,
	    CAST(DecryptByPassPhrase('ServiciosWeb2023.',Imagen_Aerolinea) as varchar(max)) as Imagen_Aerolinea
        FROM Aerolinea
        ORDER BY ID_Aerolinea;").ToListAsync();

            return aerolineas;
        }

        [HttpPost]
        public async Task<ActionResult<Aerolinea>> PostAerolinea(Aerolinea aerolinea)
        {
            try
            {
                string ID_Aerolinea = (aerolinea.ID_Aerolinea);
                string Siglas_Aerolinea = aerolinea.Siglas_Aerolinea;
                string Descripcion_Aerolinea = aerolinea.Descripcion_Aerolinea;
                string Imagen_Aerolinea = aerolinea.Imagen_Aerolinea;

                if (await _aerolineaContext.Aerolinea.AnyAsync(a => a.ID_Aerolinea == ID_Aerolinea))
                {
                    return BadRequest("Ya existe una aerolínea con este ID.");
                }

                // Query SQL
                await _aerolineaContext.Database.ExecuteSqlRawAsync($@"
                INSERT INTO Aerolinea (ID_Aerolinea, Siglas_Aerolinea, Descripcion_Aerolinea, Imagen_Aerolinea)
            VALUES (
                '{ID_Aerolinea}', 
                EncryptByPassPhrase('ServiciosWeb2023.','{Siglas_Aerolinea}'), 
                EncryptByPassPhrase('ServiciosWeb2023.','{Descripcion_Aerolinea}'), 
                EncryptByPassPhrase('ServiciosWeb2023.','{Imagen_Aerolinea}')
                    );
                ");

                var aerolineas = await _aerolineaContext.Aerolinea.ToListAsync();
                return Ok(aerolineas);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 2627)
                {
                    return BadRequest("Ya existe una aerolínea con este ID.");
                }
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("{ID_Aerolinea}")]

        public async Task<ActionResult<Aerolinea>> GetAerolineaID(string ID_Aerolinea)
        {
            // Consulta SQL para obtener datos 
            var aerolineas = await _aerolineaContext.Aerolinea.FromSqlRaw(@"SELECT ID_Aerolinea, 
	    CAST(DecryptByPassPhrase('ServiciosWeb2023.', Siglas_Aerolinea) as varchar(max)) as Siglas_Aerolinea,
	    CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion_Aerolinea) as varchar(max)) as Descripcion_Aerolinea,
	    CAST(DecryptByPassPhrase('ServiciosWeb2023.',Imagen_Aerolinea) as varchar(max)) as Imagen_Aerolinea
        FROM Aerolinea
        WHERE ID_Aerolinea = @ID_Aerolinea",
        new SqlParameter("@ID_Aerolinea", ID_Aerolinea)).FirstOrDefaultAsync();

            return Ok(aerolineas);

        }

        [HttpPut("{ID_Aerolinea}")]
        public async Task<IActionResult> PutAerolinea(string ID_Aerolinea, Aerolinea aerolinea)
        {
            if (ID_Aerolinea != aerolinea.ID_Aerolinea)
            {
                return BadRequest();
            }

            try
            {
                // Desencriptar la tabla
                var aerolineaExistente = await _aerolineaContext.Aerolinea.FromSqlRaw(
                 @"SELECT ID_Aerolinea, 
                 CAST(DecryptByPassPhrase('ServiciosWeb2023.', Siglas_Aerolinea) as varchar(max)) as Siglas_Aerolinea,
                 CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion_Aerolinea) as varchar(max)) as Descripcion_Aerolinea,
                 CAST(DecryptByPassPhrase('ServiciosWeb2023.', Imagen_Aerolinea) as varchar(max)) as Imagen_Aerolinea
                 FROM Aerolinea
                 WHERE ID_Aerolinea = @ID_Aerolinea",
                new SqlParameter("@ID_Aerolinea", ID_Aerolinea)).FirstOrDefaultAsync();

                if (aerolineaExistente == null)
                {
                    return NotFound();
                }

                // Actualizar valores 
                string Siglas_Aerolinea = aerolinea.Siglas_Aerolinea;
                string Descripcion_Aerolinea = aerolinea.Descripcion_Aerolinea;
                string Imagen_Aerolinea = aerolinea.Imagen_Aerolinea;

                // Encriptar tabla
                await _aerolineaContext.Database.ExecuteSqlRawAsync(
            @"UPDATE Aerolinea 
                SET 
                    Siglas_Aerolinea = EncryptByPassPhrase('ServiciosWeb2023.', @Siglas_Aerolinea),
                    Descripcion_Aerolinea = EncryptByPassPhrase('ServiciosWeb2023.', @Descripcion_Aerolinea),
                    Imagen_Aerolinea = EncryptByPassPhrase('ServiciosWeb2023.', @Imagen_Aerolinea)
                    WHERE ID_Aerolinea = @ID_Aerolinea",
            //Parametros para almacenar
                new SqlParameter("@Siglas_Aerolinea", Siglas_Aerolinea),
                new SqlParameter("@Descripcion_Aerolinea", Descripcion_Aerolinea),
                new SqlParameter("@Imagen_Aerolinea", Imagen_Aerolinea),
                new SqlParameter("@ID_Aerolinea", ID_Aerolinea));

                await _aerolineaContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AerolineaExist(ID_Aerolinea))
                {
                    return NotFound();
                }
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{ID_Aerolinea}")]
        public async Task<IActionResult> DeleteAerolinea(string ID_Aerolinea)
        {
            if (_aerolineaContext.Aerolinea == null)
            {
                return NotFound();
            }
            var Aerolinea = await _aerolineaContext.Aerolinea.FindAsync(ID_Aerolinea);
            if (Aerolinea == null)
            {
                return NotFound();
            }
            _aerolineaContext.Aerolinea.Remove(Aerolinea);
            await _aerolineaContext.SaveChangesAsync();
            return NoContent();
        }

        private bool AerolineaExist(string ID_Aerolinea)
        {
            return (_aerolineaContext.Aerolinea?.Any(e => e.ID_Aerolinea == ID_Aerolinea)).GetValueOrDefault();
        }
    }
}
