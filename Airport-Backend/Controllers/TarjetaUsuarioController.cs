using ProyectoServiciosWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ProyectoServiciosWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarjetaUsuarioController : ControllerBase
    {
        private readonly TarjetaUsuarioItem _tarjetausuarioContext;

        public TarjetaUsuarioController (TarjetaUsuarioItem tarjetausuarioContext)
        {
            _tarjetausuarioContext = tarjetausuarioContext;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<TarjetaUsuario>>> GetTarjetaUsuario()
        {
            var tarjetasusuario = await _tarjetausuarioContext.TarjetaUsuario.FromSqlRaw(@"SELECT
	            Id_RegistroTarjeta,
	            Id_Usuario,
	            CAST(DecryptByPassPhrase('ServiciosWeb2023.', Numero_Tarjeta) as varchar(max)) as Numero_Tarjeta,
	            CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Mes_Exp) as varchar(50)) as int) as Mes_Exp,
	            CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Year_Exp) as varchar(50)) as int) as Year_Exp,
	            CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', CVV) as varchar(50)) as int) as CVV,
                CAST(DecryptByPassPhrase('ServiciosWeb2023.', Tipo_Tarjeta) as varchar(max)) as Tipo_Tarjeta
            From Tarjeta;").ToListAsync();
            return tarjetasusuario;
        }

        [HttpGet("{Id_Usuario}")]
        public async Task<ActionResult<IEnumerable<TarjetaUsuario>>> GetTarjetasID(string Id_Usuario)
        {
            var tarjetas = await _tarjetausuarioContext.TarjetaUsuario.FromSqlRaw(@"SELECT
	            Id_RegistroTarjeta,
	            Id_Usuario,
	            CAST(DecryptByPassPhrase('ServiciosWeb2023.', Numero_Tarjeta) as varchar(max)) as Numero_Tarjeta,
	            CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Mes_Exp) as varchar(50)) as int) as Mes_Exp,
	            CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Year_Exp) as varchar(50)) as int) as Year_Exp,
	            CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', CVV) as varchar(50)) as int) as CVV,
                CAST(DecryptByPassPhrase('ServiciosWeb2023.', Tipo_Tarjeta) as varchar(max)) as Tipo_Tarjeta
            From Tarjeta
            WHERE Id_Usuario = @Id_Usuario",
            new SqlParameter("@Id_Usuario", Id_Usuario)).FirstOrDefaultAsync();
            return Ok(tarjetas);
        }

        [HttpPost]
        public async Task<ActionResult<TarjetaUsuario>> PostTarjetaUsuario(TarjetaUsuario tarjeta)
        {
            try
            {
                string Id_RegistroTarjeta = (tarjeta.Id_RegistroTarjeta);
                string Id_Usuario = (tarjeta.Id_Usuario);
                string Numero_Tarjeta = (tarjeta.Numero_Tarjeta);
                int Mes_Exp = (tarjeta.Mes_Exp);
                int Year_Exp = (tarjeta.Year_Exp);
                int CVV = (tarjeta.CVV);
                string Tipo_Tarjeta = (tarjeta.Tipo_Tarjeta);

                if (await _tarjetausuarioContext.TarjetaUsuario.AnyAsync(a => a.Id_RegistroTarjeta == Id_RegistroTarjeta))
                {
                    return BadRequest("Ya existe una tarjeta con ese ID");
                }

                await _tarjetausuarioContext.Database.ExecuteSqlRawAsync($@"INSERT INTO Tarjeta (Id_RegistroTarjeta, Id_Usuario, Numero_Tarjeta, Mes_Exp, Year_Exp, CVV, Tipo_Tarjeta)
                    VALUES (
	                    '{Id_RegistroTarjeta}', 
	                    '{Id_Usuario}', 
	                    EncryptByPassPhrase('ServiciosWeb2023.','{Numero_Tarjeta}'), 
	                    EncryptByPassPhrase('ServiciosWeb2023.','{Mes_Exp}'), 
	                    EncryptByPassPhrase('ServiciosWeb2023.','{Year_Exp}'), 
	                    EncryptByPassPhrase('ServiciosWeb2023.','{CVV}'),
	                    EncryptByPassPhrase('ServiciosWeb2023.','{Tipo_Tarjeta}')
                   );");

                var tarjetas = await _tarjetausuarioContext.TarjetaUsuario.ToListAsync();
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

        [HttpPut("{Id_RegistroTarjeta}")]

        public async Task<IActionResult> PutTarjeta(string Id_RegistroTarjeta, TarjetaUsuario tarjeta)
        {
            if (Id_RegistroTarjeta != tarjeta.Id_RegistroTarjeta)
            {
                return BadRequest();
            }
            try
            {
                var tarjetaExistente = await _tarjetausuarioContext.TarjetaUsuario.FromSqlRaw(@"SELECT
	            Id_RegistroTarjeta,
	            Id_Usuario,
	            CAST(DecryptByPassPhrase('ServiciosWeb2023.', Numero_Tarjeta) as varchar(max)) as Numero_Tarjeta,
	            CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Mes_Exp) as varchar(50)) as int) as Mes_Exp,
	            CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Year_Exp) as varchar(50)) as int) as Year_Exp,
	            CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', CVV) as varchar(50)) as int) as CVV,
                CAST(DecryptByPassPhrase('ServiciosWeb2023.', Tipo_Tarjeta) as varchar(max)) as Tipo_Tarjeta
            From Tarjeta
            WHERE Id_RegistroTarjeta = @Id_RegistroTarjeta",
            new SqlParameter("@Id_RegistroTarjeta", Id_RegistroTarjeta)).FirstOrDefaultAsync();

                if (tarjetaExistente == null)
                {
                    return NotFound();
                }
                string Numero_Tarjeta = tarjeta.Numero_Tarjeta;
                int Mes_Exp = tarjeta.Mes_Exp;
                int Year_Exp = tarjeta.Year_Exp;
                int CVV = tarjeta.CVV;
                string Tipo_Tarjeta = tarjeta.Tipo_Tarjeta;

                await _tarjetausuarioContext.Database.ExecuteSqlRawAsync(
                @"UPDATE Tarjeta
                    SET
                       Numero_Tarjeta = EncryptByPassPhrase('ServiciosWeb2023.', @Numero_Tarjeta),
                       Mes_Exp = EncryptByPassPhrase('ServiciosWeb2023.', CAST(CAST(@Mes_Exp AS nvarchar(max)) AS varchar(max))),
                       Year_Exp = EncryptByPassPhrase('ServiciosWeb2023.', CAST(CAST(@Year_Exp AS nvarchar(max)) AS varchar(max))),
                       CVV = EncryptByPassPhrase('ServiciosWeb2023.', CAST(CAST(@CVV AS nvarchar(max)) AS varchar(max))),
                       Tipo_Tarjeta = EncryptByPassPhrase('ServiciosWeb2023.', @Tipo_Tarjeta)
                    WHERE Id_RegistroTarjeta = @Id_RegistroTarjeta",
                new SqlParameter("@Numero_Tarjeta", Numero_Tarjeta),
                new SqlParameter("@Mes_Exp", Mes_Exp),
                new SqlParameter("@Year_Exp", Year_Exp),
                new SqlParameter("@CVV", CVV),
                new SqlParameter("@Tipo_Tarjeta", Tipo_Tarjeta),
                new SqlParameter("@Id_RegistroTarjeta", Id_RegistroTarjeta));

                await _tarjetausuarioContext.SaveChangesAsync();

            }
            catch (DbUpdateException ex)
            {
                if (!TarjetaExist(Id_RegistroTarjeta))
                {
                    return NotFound();
                }
                return BadRequest();
            }
            return NoContent();
        }

        [HttpDelete("{Id_RegistroTarjeta}")]
        public async Task<IActionResult> DeleteCuenta(string Id_RegistroTarjeta)
        {
            var query = $"DELETE FROM Tarjeta WHERE Id_RegistroTarjeta = '{Id_RegistroTarjeta}'";

            try
            {
                await _tarjetausuarioContext.Database.ExecuteSqlRawAsync(query);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la tarjeta: {ex.Message}");
            }
        }

        private bool TarjetaExist(string Id_RegistroTarjeta)
        {
            return (_tarjetausuarioContext.TarjetaUsuario?.Any(e => e.Id_RegistroTarjeta == Id_RegistroTarjeta)).GetValueOrDefault();
        }
    }
}
