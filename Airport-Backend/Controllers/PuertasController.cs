using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoServiciosWeb.Models;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace ProyectoServiciosWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PuertasController : ControllerBase
    {
        private readonly PuertasItem _puertasContext;

        public PuertasController(PuertasItem PuertasContext)
        {
            _puertasContext = PuertasContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Puertas_Aeropuerto>>> GetPuertas()
        {
            // Consulta SQL para obtener datos 
            var puertas = await _puertasContext.Puertas_Aeropuerto.FromSqlRaw(@"SELECT ID_Puerta,
        CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',Numero_Puerta) as varchar(50)) as int) as Numero_Puerta,
	    CAST(DecryptByPassPhrase('ServiciosWeb2023.', Estado_Puerta) as varchar(max)) as Estado_Puerta
        FROM Puertas_Aeropuerto;").ToListAsync();

            return puertas;
        }

        //POST

        [HttpPost]
        public async Task<ActionResult<Puertas_Aeropuerto>> PostPuertas(Puertas_Aeropuerto puerta)
        {
            try
            {
                string ID_Puerta = (puerta.ID_Puerta);
                int Numero_Puerta = (puerta.Numero_Puerta);
                string Estado_Puerta = puerta.Estado_Puerta;

                if (await _puertasContext.Puertas_Aeropuerto.AnyAsync(a => a.ID_Puerta == ID_Puerta))
                {
                    return BadRequest("Ya existe una puerta con este ID.");
                }

                // Query SQL
                await _puertasContext.Database.ExecuteSqlRawAsync($@"
                INSERT INTO Puertas_Aeropuerto (ID_Puerta, Numero_Puerta, Estado_Puerta)
                VALUES (
                '{ID_Puerta}', 
                EncryptByPassPhrase('ServiciosWeb2023.', '{Numero_Puerta}'),
                EncryptByPassPhrase('ServiciosWeb2023.','{Estado_Puerta}'));
                ");
                 

                var puertas = await _puertasContext.Puertas_Aeropuerto.ToListAsync();
                return Ok(puertas);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 2627)
                {
                    return BadRequest("Ya existe una puerta con este ID.");
                }
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        //GET (ID)
        [HttpGet("{ID_Puerta}")]

        public async Task<ActionResult<Puertas_Aeropuerto>> GetPuertaID(string ID_Puerta)
        {
            // Consulta SQL para obtener datos 
            var puertas = await _puertasContext.Puertas_Aeropuerto.FromSqlRaw(@"SELECT ID_Puerta, 
	    CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Numero_Puerta) as varchar(50)) as int) as Numero_Puerta,
	    CAST(DecryptByPassPhrase('ServiciosWeb2023.', Estado_Puerta) as varchar(max)) as Estado_Puerta
        FROM Puertas_Aeropuerto
        WHERE ID_Puerta = @ID_Puerta",
        new SqlParameter("@ID_Puerta", ID_Puerta)).FirstOrDefaultAsync();

            return Ok(puertas);

        }

        //PUT
        [HttpPut("{ID_Puerta}")]
        public async Task<IActionResult> PutPuerta(string ID_Puerta, Puertas_Aeropuerto puertas)
        {
            if (ID_Puerta != puertas.ID_Puerta)
            {
                return BadRequest();
            }

            try
            {
                // Desencriptar la tabla
                var puertaExistente = await _puertasContext.Puertas_Aeropuerto.FromSqlRaw(
                 @"SELECT ID_Puerta, 
                 CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Numero_Puerta) as varchar(50)) as int) as Numero_Puerta,
                 CAST(DecryptByPassPhrase('ServiciosWeb2023.', Estado_Puerta) as varchar(max)) as Estado_Puerta
                 FROM Puertas_Aeropuerto
                 WHERE ID_Puerta = @ID_Puerta",
                new SqlParameter("@ID_Puerta", ID_Puerta)).FirstOrDefaultAsync();

                if (puertaExistente == null)
                {
                    return NotFound();
                }

                // Actualizar valores 
                int Numero_Puerta = puertas.Numero_Puerta;
                string Estado_Puerta = puertas.Estado_Puerta;

                // Encriptar tabla
                await _puertasContext.Database.ExecuteSqlRawAsync(
            @"UPDATE Puertas_Aeropuerto
                SET 
                    Numero_Puerta = EncryptByPassPhrase('ServiciosWeb2023.',CAST(CAST(@Numero_Puerta AS nvarchar(max)) AS varchar(max))),
                    Estado_Puerta = EncryptByPassPhrase('ServiciosWeb2023.', @Estado_Puerta)
                    WHERE ID_Puerta = @ID_Puerta",
                //Parametros para almacenar
                new SqlParameter("@Numero_Puerta", Numero_Puerta),
                new SqlParameter("@Estado_Puerta", Estado_Puerta),
                new SqlParameter("@ID_Puerta", ID_Puerta));

                await _puertasContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PuertaExist(ID_Puerta))
                {
                    return NotFound();
                }
                return BadRequest();
            }

            return NoContent();
        }
        //DELETE
        [HttpDelete("{ID_Puerta}")]
        public async Task<IActionResult> DeletePuertas(string ID_Puerta)
        {
            var query = $"DELETE FROM Puertas_Aeropuerto WHERE ID_Puerta = '{ID_Puerta}'";

            try
            {
                await _puertasContext.Database.ExecuteSqlRawAsync(query);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la puerta: {ex.Message}");
            }
        }

        private bool PuertaExist(string ID_Puerta)
        {
            return (_puertasContext.Puertas_Aeropuerto?.Any(e => e.ID_Puerta == ID_Puerta)).GetValueOrDefault();
        }


    }
}
