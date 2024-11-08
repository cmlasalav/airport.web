using MetodosPago.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MetodosPago.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarjetaController : ControllerBase
    {
        private readonly TarjetasItem _tarjetaContext;

        public TarjetaController(TarjetasItem tarjetaContext)
        {
            _tarjetaContext = tarjetaContext;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Tarjeta>>> GetTarjetas()
        {
            var tarjetas = await _tarjetaContext.Tarjeta.FromSqlRaw(@"SELECT
	        Id_Registro,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Numero_Tarjeta) as varchar(max)) as Numero_Tarjeta,
	        CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Mes_Exp) as varchar(50)) as int) as Mes_Exp,
	        CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Year_Exp) as varchar(50)) as int) as Year_Exp,
	        CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', CVV) as varchar(50)) as int) as CVV,
	        CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Total) as varchar(50)) as int) as Total,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Estado_Pago) as varchar(max)) as Estado_Pago,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Tipo_Tarjeta) as varchar(max)) as Tipo_Tarjeta
            From Tarjeta;").ToListAsync();
            return tarjetas;
        }

        [HttpGet("{Id_Registro}")]
        public async Task<ActionResult<IEnumerable<Tarjeta>>> GetTarjetasID(string Id_Registro)
        {
            var tarjetas = await _tarjetaContext.Tarjeta.FromSqlRaw(@"SELECT
	        Id_Registro,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Numero_Tarjeta) as varchar(max)) as Numero_Tarjeta,
	        CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Mes_Exp) as varchar(50)) as int) as Mes_Exp,
	        CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Year_Exp) as varchar(50)) as int) as Year_Exp,
	        CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', CVV) as varchar(50)) as int) as CVV,
	        CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Total) as varchar(50)) as int) as Total,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Estado_Pago) as varchar(max)) as Estado_Pago,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Tipo_Tarjeta) as varchar(max)) as Tipo_Tarjeta
            From Tarjeta
            WHERE Id_Registro = @Id_Registro",
            new SqlParameter("@Id_Registro", Id_Registro)).FirstOrDefaultAsync();
            return Ok(tarjetas);
        }

        [HttpPost]
        public async Task<ActionResult<Tarjeta>> PostTarjeta(Tarjeta tarjeta)
        {
            try
            {
                string Id_Registro = (tarjeta.Id_Registro);
                string Numero_Tarjeta = (tarjeta.Numero_Tarjeta);
                int Mes_Exp = (tarjeta.Mes_Exp);
                int Year_Exp = (tarjeta.Year_Exp);
                int CVV = (tarjeta.CVV);
                int Total = (tarjeta.Total);
                string Estado_Pago = (tarjeta.Estado_Pago);
                string Tipo_Tarjeta = (tarjeta.Tipo_Tarjeta);

                if (await _tarjetaContext.Tarjeta.AnyAsync(a => a.Id_Registro == Id_Registro))
                {
                    return BadRequest("Ya existe una tarjeta con ese ID");
                }

                await _tarjetaContext.Database.ExecuteSqlRawAsync($@"INSERT INTO Tarjeta (Id_Registro, Numero_Tarjeta, Mes_Exp, Year_Exp, CVV, Total, Estado_Pago, Tipo_Tarjeta)
                VALUES (
	                '{Id_Registro}',  
	                EncryptByPassPhrase('ServiciosWeb2023.','{Numero_Tarjeta}'), 
	                EncryptByPassPhrase('ServiciosWeb2023.','{Mes_Exp}'), 
	                EncryptByPassPhrase('ServiciosWeb2023.','{Year_Exp}'), 
	                EncryptByPassPhrase('ServiciosWeb2023.','{CVV}'),
	                EncryptByPassPhrase('ServiciosWeb2023.','{Total}'),
	                EncryptByPassPhrase('ServiciosWeb2023.','{Estado_Pago}'),
	                EncryptByPassPhrase('ServiciosWeb2023.','{Tipo_Tarjeta}')
                    );");

                var tarjetas = await _tarjetaContext.Tarjeta.ToListAsync();
                return Ok(tarjetas);

            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 2627)
                {
                    return BadRequest("Ya existe una cuenta con este ID.");
                }
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        [HttpPut("{Id_Registro}")]

        public async Task<IActionResult> PutCuenta(string Id_Registro, Tarjeta tarjeta)
        {
            if (Id_Registro != tarjeta.Id_Registro)
            {
                return BadRequest();
            }
            try
            {
                var tarjetaExistente = await _tarjetaContext.Tarjeta.FromSqlRaw(@"SELECT
                Id_Registro,
                    CAST(DecryptByPassPhrase('ServiciosWeb2023.', Numero_Tarjeta) as varchar(max)) as Numero_Tarjeta,
                    CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Mes_Exp) as varchar(50)) as int) as Mes_Exp,
                    CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Year_Exp) as varchar(50)) as int) as Year_Exp,
                    CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', CVV) as varchar(50)) as int) as CVV,
                    CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Total) as varchar(50)) as int) as Total,
                    CAST(DecryptByPassPhrase('ServiciosWeb2023.', Estado_Pago) as varchar(max)) as Estado_Pago,
                    CAST(DecryptByPassPhrase('ServiciosWeb2023.', Tipo_Tarjeta) as varchar(max)) as Tipo_Tarjeta
                From Tarjeta
                WHERE Id_Registro = @Id_Registro",
            new SqlParameter("@Id_Registro", Id_Registro)).FirstOrDefaultAsync();

                if (tarjetaExistente == null)
                {
                    return NotFound();
                }
                string Numero_Tarjeta = tarjeta.Numero_Tarjeta;
                int Mes_Exp = tarjeta.Mes_Exp;
                int Year_Exp = tarjeta.Year_Exp;
                int CVV = tarjeta.CVV;
                int Total = tarjeta.Total;
                string Estado_Pago = tarjeta.Estado_Pago;
                string Tipo_Tarjeta = tarjeta.Tipo_Tarjeta;

                await _tarjetaContext.Database.ExecuteSqlRawAsync(
                @"UPDATE Tarjeta
                    SET
                       Numero_Tarjeta = EncryptByPassPhrase('ServiciosWeb2023.', @Numero_Tarjeta),
                       Mes_Exp = EncryptByPassPhrase('ServiciosWeb2023.', CAST(CAST(@Mes_Exp AS nvarchar(max)) AS varchar(max))),
                       Year_Exp = EncryptByPassPhrase('ServiciosWeb2023.', CAST(CAST(@Year_Exp AS nvarchar(max)) AS varchar(max))),
                       CVV = EncryptByPassPhrase('ServiciosWeb2023.', CAST(CAST(@CVV AS nvarchar(max)) AS varchar(max))),
                       Total = EncryptByPassPhrase('ServiciosWeb2023.', CAST(CAST(@Total AS nvarchar(max)) AS varchar(max))),
                       Estado_Pago = EncryptByPassPhrase('ServiciosWeb2023.', @Estado_Pago),
                       Tipo_Tarjeta = EncryptByPassPhrase('ServiciosWeb2023.', @Tipo_Tarjeta)
                    WHERE Id_Registro = @Id_Registro",
                new SqlParameter("@Numero_Tarjeta", Numero_Tarjeta),
                new SqlParameter("@Mes_Exp", Mes_Exp),
                new SqlParameter("@Year_Exp", Year_Exp),
                new SqlParameter("@CVV", CVV),
                new SqlParameter("@Total", Total),
                new SqlParameter("@Estado_Pago", Estado_Pago),
                new SqlParameter("@Tipo_Tarjeta", Tipo_Tarjeta),
                new SqlParameter("@Id_Registro", Id_Registro));

                await _tarjetaContext.SaveChangesAsync();

            }
            catch (DbUpdateException ex)
            {
                if (!TarjetaExist(Id_Registro))
                {
                    return NotFound();
                }
                return BadRequest();
            }
            return NoContent();
        }

        [HttpDelete("{Id_Registro}")]
        public async Task<IActionResult> DeleteCuenta(string Id_Registro)
        {
            var query = $"DELETE FROM Tarjeta WHERE Id_Registro = '{Id_Registro}'";

            try
            {
                await _tarjetaContext.Database.ExecuteSqlRawAsync(query);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la tarjeta: {ex.Message}");
            }
        }

        private bool TarjetaExist(string Id_Registro)
        {
            return (_tarjetaContext.Tarjeta?.Any(e => e.Id_Registro == Id_Registro)).GetValueOrDefault();
        }

    }
}
