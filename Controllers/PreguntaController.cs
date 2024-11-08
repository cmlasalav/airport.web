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
    public class PreguntaController : ControllerBase
    {
        private readonly PreguntaItem _preguntaContext;

        public PreguntaController(PreguntaItem PreguntaContext)
        {
            _preguntaContext = PreguntaContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pregunta_Seguridad>>> GetPregunta()
        {
            // Consulta SQL para obtener datos 
            var pregunta = await _preguntaContext.Pregunta_Seguridad.FromSqlRaw(@"SELECT 
	Id_Pregunta,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion_Pregunta) as varchar(max)) as Descripcion_Pregunta
    FROM Pregunta_Seguridad;").ToListAsync();

            return pregunta;
        }

        //POST
        [HttpPost]
        public async Task<ActionResult<Pregunta_Seguridad>> Postpregunta(Pregunta_Seguridad pregunta)
        {
            try
            {
                int Id_Pregunta = (pregunta.Id_Pregunta);

                string Descripcion_Pregunta = pregunta.Descripcion_Pregunta;


                if (await _preguntaContext.Pregunta_Seguridad.AnyAsync(a => a.Id_Pregunta == Id_Pregunta))
                {
                    return BadRequest("Ya existe una pregunta con este ID.");
                }

                // Query SQL
                await _preguntaContext.Database.ExecuteSqlRawAsync($@"
                INSERT INTO Pregunta_Seguridad (Id_Pregunta, Descripcion_Pregunta)
            VALUES (
                '{Id_Pregunta}', 
                EncryptByPassPhrase('ServiciosWeb2023.','{Descripcion_Pregunta}')

                    );
                ");

                var preguntas = await _preguntaContext.Pregunta_Seguridad.ToListAsync();
                return Ok(preguntas);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 2627)
                {
                    return BadRequest("Ya existe una pregunta con este ID.");
                }
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        //GET ID
        [HttpGet("{Id_Pregunta}")]

        public async Task<ActionResult<Pregunta_Seguridad>> GetPreguntaID(int Id_Pregunta)
        {
            // Consulta SQL para obtener datos 
            var preguntas = await _preguntaContext.Pregunta_Seguridad.FromSqlRaw(@"SELECT Id_Pregunta, 
	    CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion_Pregunta) as varchar(max)) as Descripcion_Pregunta
        FROM Pregunta_Seguridad
        WHERE Id_Pregunta = @Id_Pregunta",
        new SqlParameter("@Id_Pregunta", Id_Pregunta)).FirstOrDefaultAsync();

            return Ok(preguntas);

        }

        //PUT
        [HttpPut("{Id_Pregunta}")]
        public async Task<IActionResult> PutPregunta(int Id_Pregunta, Pregunta_Seguridad pregunta)
        {
            if (Id_Pregunta != pregunta.Id_Pregunta)
            {
                return BadRequest();
            }

            try
            {
                // Desencriptar la tabla
                var preguntaExistente = await _preguntaContext.Pregunta_Seguridad.FromSqlRaw(
                 @"SELECT Id_Pregunta, 
                 CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion_Pregunta) as varchar(max)) as Descripcion_Pregunta              
                 FROM Pregunta_Seguridad
                 WHERE Id_Pregunta = @Id_Pregunta",
                new SqlParameter("@Id_Pregunta", Id_Pregunta)).FirstOrDefaultAsync();

                if (preguntaExistente == null)
                {
                    return NotFound();
                }

                // Actualizar valores 

                string Descripcion_Pregunta = pregunta.Descripcion_Pregunta;


                // Encriptar tabla
                await _preguntaContext.Database.ExecuteSqlRawAsync(
            @"UPDATE Pregunta_Seguridad
                SET 
                    Descripcion_Pregunta = EncryptByPassPhrase('ServiciosWeb2023.', @Descripcion_Pregunta)
                    WHERE Id_Pregunta = @Id_Pregunta",
                //Parametros para almacenar
                new SqlParameter("@Descripcion_Pregunta", Descripcion_Pregunta),
                new SqlParameter("@Id_Pregunta", Id_Pregunta));

                await _preguntaContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PreguntaExist(Id_Pregunta))
                {
                    return NotFound();
                }
                return BadRequest();
            }

            return NoContent();
        }

        //DELETE
        [HttpDelete("{Id_Pregunta}")]
        public async Task<IActionResult> DeletePregunta(int Id_Pregunta)
        {
            if (_preguntaContext.Pregunta_Seguridad == null)
            {
                return NotFound();
            }
            var Pregunta_Seguridad = await _preguntaContext.Pregunta_Seguridad.FindAsync(Id_Pregunta);
            if (Pregunta_Seguridad == null)
            {
                return NotFound();
            }
            _preguntaContext.Pregunta_Seguridad.Remove(Pregunta_Seguridad);
            await _preguntaContext.SaveChangesAsync();
            return NoContent();
        }

        private bool PreguntaExist(int Id_Pregunta)
        {
            return (_preguntaContext.Pregunta_Seguridad?.Any(e => e.Id_Pregunta == Id_Pregunta)).GetValueOrDefault();
        }


    }
}
