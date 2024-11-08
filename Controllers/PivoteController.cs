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
    public class PivoteController : ControllerBase
    {
        private readonly PivoteItem _pivoteContext;

        public PivoteController(PivoteItem PivoteContext)
        {
            _pivoteContext = PivoteContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pivote>>> GetPivote()
        {
            // Consulta SQL para obtener datos 
            var pivote = await _pivoteContext.Pivote.FromSqlRaw(@"SELECT 
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',ID) as varchar(50)) as int) as ID, 
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion) as varchar(50)) as Descripcion,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Prefijo) as varchar(50)) as Prefijo,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',Consecutivo) as varchar(50)) as int) as Consecutivo,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',RangoInicial) as varchar(50)) as int) as RangoInicial,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',RangoFinal) as varchar(50)) as int) as RangoFinal
    FROM Pivote
    ORDER BY ID;").ToListAsync();

            return pivote;
        }


        [HttpGet("{ID}")]
        public async Task<ActionResult<Pivote>> GetPivoteID(int ID)
        {
            // Consulta SQL para obtener datos 
            var consecutivos = await _pivoteContext.Pivote.FromSqlRaw(@"SELECT 
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',ID) as varchar(50)) as int) as ID, 
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion) as varchar(50)) as Descripcion,
	CAST(DecryptByPassPhrase('ServiciosWeb2023.', Prefijo) as varchar(50)) as Prefijo,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',Consecutivo) as varchar(50)) as int) as Consecutivo,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',RangoInicial) as varchar(50)) as int) as RangoInicial,
	CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',RangoFinal) as varchar(50)) as int) as RangoFinal
    FROM Pivote
    WHERE CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',ID) as varchar(50)) as int) = @ID_Pivote",
        new SqlParameter("@ID_Pivote", ID)).FirstOrDefaultAsync();

            return Ok(consecutivos);

        }


        [HttpPut("{ID}")]
        public async Task<IActionResult> PutPivote(int ID, Pivote pivote)
        {
            if (ID != pivote.ID)
            {
                return BadRequest();
            }

            try
            {
                // Desencriptar la tabla
                var pivoteExistente = await _pivoteContext.Pivote.FromSqlRaw(
                 @"SELECT 
	                CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',ID) as varchar(50)) as int) as ID, 
	                CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion) as varchar(50)) as Descripcion,
	                CAST(DecryptByPassPhrase('ServiciosWeb2023.', Prefijo) as varchar(50)) as Prefijo,
	                CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',Consecutivo) as varchar(50)) as int) as Consecutivo,
	                CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',RangoInicial) as varchar(50)) as int) as RangoInicial,
	                CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',RangoFinal) as varchar(50)) as int) as RangoFinal
                FROM Pivote
                    WHERE CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',ID) as varchar(50)) as int) = @ID_Pivote",
        new SqlParameter("@ID_Pivote", ID)).FirstOrDefaultAsync();

                if (pivoteExistente == null)
                {
                    return NotFound();
                }

                // Actualizar valores 
                string Descripcion = pivote.Descripcion;
                int Consecutivo = pivote.Consecutivo;
                string Prefijo = pivote.Prefijo;
                int RangoInicial = pivote.RangoInicial;
                int RangoFinal = pivote.RangoFinal;

                // Encriptar tabla
                await _pivoteContext.Database.ExecuteSqlRawAsync(
           @"UPDATE Pivote 
                SET 
                    Descripcion = EncryptByPassPhrase('ServiciosWeb2023.', @Descripcion),
                    Consecutivo = EncryptByPassPhrase('ServiciosWeb2023.', CAST(CAST(@Consecutivo AS nvarchar(max)) AS varchar(max))),
                    Prefijo = EncryptByPassPhrase('ServiciosWeb2023.', @Prefijo),
                    RangoInicial = EncryptByPassPhrase('ServiciosWeb2023.', CAST(CAST(@RangoInicial AS nvarchar(max)) AS varchar(max))),
                    RangoFinal = EncryptByPassPhrase('ServiciosWeb2023.', CAST(CAST(@RangoFinal AS nvarchar(max)) AS varchar(max)))
                WHERE CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.',ID) as varchar(50)) as int) = @ID",
                //Parametros para almacenar
                new SqlParameter("@Descripcion", Descripcion),
                new SqlParameter("@Consecutivo", Consecutivo),
                new SqlParameter("@Prefijo", Prefijo),
                new SqlParameter("@RangoInicial", RangoInicial),
                new SqlParameter("@RangoFinal", RangoFinal),
                new SqlParameter("@ID", ID));

                await _pivoteContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PivoteExist(ID))
                {
                    return NotFound();
                }
                return BadRequest();
            }

            return NoContent();
        }


        private bool PivoteExist(int ID)
        {
            return (_pivoteContext.Pivote?.Any(e => e.ID == ID)).GetValueOrDefault();
        }

    }
}
