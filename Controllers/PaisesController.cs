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
    public class PaisesController : ControllerBase
    {
        private readonly PaisesItem _paisesContext;

        public PaisesController(PaisesItem PaisesContext)
        {
            _paisesContext = PaisesContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paises>>> GetPaises()
        {
            // Consulta SQL para obtener datos 
            var paises = await _paisesContext.Paises.FromSqlRaw(@" SELECT Id_Pais, 
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Siglas_Pais) as varchar(max)) as Siglas_Pais,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion_Pais) as varchar(max)) as Descripcion_Pais,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Imagen_Pais) as varchar(max)) as Imagen_Pais
    FROM Paises;").ToListAsync();

            return paises;
        }

        //POST

        [HttpPost]
        public async Task<ActionResult<Paises>> PostPaises(Paises paises)
        {
            try
            {
                string Id_Pais = (paises.Id_Pais);
                string Siglas_Pais = paises.Siglas_Pais;
                string Descripcion_Pais = paises.Descripcion_Pais;
                string Imagen_Pais = paises.Imagen_Pais;

                if (await _paisesContext.Paises.AnyAsync(p => p.Id_Pais == Id_Pais))
                {
                    return BadRequest("Ya existe un pais con este ID.");
                }

                // Query SQL
                await _paisesContext.Database.ExecuteSqlRawAsync($@"
                INSERT INTO paises (Id_Pais, Siglas_Pais, Descripcion_Pais, Imagen_Pais)
            VALUES (
                '{Id_Pais}', 
                EncryptByPassPhrase('ServiciosWeb2023.','{Siglas_Pais}'), 
                EncryptByPassPhrase('ServiciosWeb2023.','{Descripcion_Pais}'), 
                EncryptByPassPhrase('ServiciosWeb2023.','{Imagen_Pais}')
                    );
                ");

                var paisess = await _paisesContext.Paises.ToListAsync();
                return Ok(paisess);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 2627)
                {
                    return BadRequest("Ya existe una pais con este ID.");
                }
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        //Get ID
        [HttpGet("{Id_Pais}")]

        public async Task<ActionResult<Paises>> GetPaisesID(string Id_Pais)
        {
            // Consulta SQL para obtener datos 
            var paises = await _paisesContext.Paises.FromSqlRaw(@" SELECT Id_Pais, 
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Siglas_Pais) as varchar(max)) as Siglas_Pais,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion_Pais) as varchar(max)) as Descripcion_Pais,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Imagen_Pais) as varchar(max)) as Imagen_Pais
    FROM Paises
        WHERE Id_Pais = @Id_Pais",
        new SqlParameter("@Id_Pais", Id_Pais)).FirstOrDefaultAsync();

            return Ok(paises);

        }

        //PUT
        [HttpPut("{Id_Pais}")]
        public async Task<IActionResult> PutPaises(string Id_Pais, Paises paises)
        {
            if (Id_Pais != paises.Id_Pais)
            {
                return BadRequest();
            }

            try
            {
                // Desencriptar la tabla
                var paisesExistente = await _paisesContext.Paises.FromSqlRaw(
                 @" SELECT Id_Pais, 
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Siglas_Pais) as varchar(max)) as Siglas_Pais,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion_Pais) as varchar(max)) as Descripcion_Pais,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Imagen_Pais) as varchar(max)) as Imagen_Pais
    FROM Paises
                 WHERE Id_Pais = @Id_Pais",
                new SqlParameter("@Id_Pais", Id_Pais)).FirstOrDefaultAsync();

                if (paisesExistente == null)
                {
                    return NotFound();
                }

                // Actualizar valores 
                string Siglas_Pais = paises.Siglas_Pais;
                string Descripcion_Pais = paises.Descripcion_Pais;
                string Imagen_Pais = paises.Imagen_Pais;

                // Encriptar tabla
                await _paisesContext.Database.ExecuteSqlRawAsync(
            @"UPDATE Paises
                SET 
                    Siglas_Pais = EncryptByPassPhrase('ServiciosWeb2023.', @Siglas_Pais ),
                    Descripcion_Pais = EncryptByPassPhrase('ServiciosWeb2023.', @Descripcion_Pais),
                    Imagen_Pais = EncryptByPassPhrase('ServiciosWeb2023.', @Imagen_Pais)
                    WHERE Id_Pais = @Id_Pais",
                //Parametros para almacenar
                new SqlParameter("@Siglas_Pais", Siglas_Pais),
                new SqlParameter("@Descripcion_Pais", Descripcion_Pais),
                new SqlParameter("@Imagen_Pais", Imagen_Pais),
                new SqlParameter("@Id_Pais", Id_Pais));

                await _paisesContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaisesExist(Id_Pais))
                {
                    return NotFound();
                }
                return BadRequest();
            }

            return NoContent();
        }

        //DELETE
        [HttpDelete("{Id_Pais}")]
        public async Task<IActionResult> DeletePaises(string Id_Pais)
        {
            if (_paisesContext.Paises == null)
            {
                return NotFound();
            }
            var Paises = await _paisesContext.Paises.FindAsync(Id_Pais);
            if (Paises == null)
            {
                return NotFound();
            }
            _paisesContext.Paises.Remove(Paises);
            await _paisesContext.SaveChangesAsync();
            return NoContent();
        }

        private bool PaisesExist(string Id_Pais)
        {
            return (_paisesContext.Paises?.Any(e => e.Id_Pais == Id_Pais)).GetValueOrDefault();
        }


    }
}
