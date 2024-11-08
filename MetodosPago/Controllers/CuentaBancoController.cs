using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MetodosPago.Models;

namespace MetodosPago.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaBancoController : ControllerBase
    {
        private readonly Cuenta_BancoItem _cuentaContext;

        public CuentaBancoController(Cuenta_BancoItem cuentaContext)
        {
            _cuentaContext = cuentaContext;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Cuenta_Banco>>> GetCuentas()
        {
            var cuentas = await _cuentaContext.Cuenta_Banco.FromSqlRaw(@"SELECT 
	        Id_RegistroCuenta,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Num_Cuenta) as varchar(max)) as Num_Cuenta,
	        CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Codigo) as varchar(50)) as int) as Codigo,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Password_Cuenta) as varchar(max)) as Password_Cuenta
            FROM Cuenta_Banco;").ToListAsync();

            return cuentas;
        }

        [HttpGet("{Id_RegistroCuenta}")]

        public async Task<ActionResult<Cuenta_Banco>> GetCuentasID(string Id_RegistroCuenta)
        {
            var cuentas = await _cuentaContext.Cuenta_Banco.FromSqlRaw(@"SELECT 
	        Id_RegistroCuenta,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Num_Cuenta) as varchar(max)) as Num_Cuenta,
	        CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Codigo) as varchar(50)) as int) as Codigo,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Password_Cuenta) as varchar(max)) as Password_Cuenta
            FROM Cuenta_Banco
            WHERE Id_RegistroCuenta = @Id_RegistroCuenta",
        new SqlParameter("@Id_RegistroCuenta", Id_RegistroCuenta)).FirstOrDefaultAsync();

            return Ok(cuentas);
        }

        [HttpPost]
        public async Task<ActionResult<Cuenta_Banco>> PostCuentas(Cuenta_Banco cuenta)
        {
            try
            {
                string Id_RegistroCuenta = (cuenta.Id_RegistroCuenta);
                string Num_Cuenta = (cuenta.Num_Cuenta);
                int Codigo = (cuenta.Codigo);
                string Password_Cuenta = (cuenta.Password_Cuenta);

                if(await _cuentaContext.Cuenta_Banco.AnyAsync(a => a.Id_RegistroCuenta == Id_RegistroCuenta))
                {
                    return BadRequest("Ya existe una cuenta con ese ID");
                }

                await _cuentaContext.Database.ExecuteSqlRawAsync($@"INSERT INTO Cuenta_Banco (Id_RegistroCuenta, Num_Cuenta, Codigo, Password_Cuenta) 
                VALUES(
	            '{Id_RegistroCuenta}',
	            EncryptByPassPhrase('ServiciosWeb2023.','{Num_Cuenta}'),
	            EncryptByPassPhrase('ServiciosWeb2023.','{Codigo}'),
	            EncryptByPassPhrase('ServiciosWeb2023.','{Password_Cuenta}')
                );");

                var cuentas = await _cuentaContext.Cuenta_Banco.ToListAsync();
                return Ok(cuentas);

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

        [HttpPut("{Id_RegistroCuenta}")]

        public async Task<IActionResult> PutCuenta(string Id_RegistroCuenta, Cuenta_Banco cuenta)
        {
            if(Id_RegistroCuenta != cuenta.Id_RegistroCuenta)
            {
                return BadRequest();
            }
            try
            {
                var cuentaExistente = await _cuentaContext.Cuenta_Banco.FromSqlRaw(@"SELECT 
	        Id_RegistroCuenta,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Num_Cuenta) as varchar(max)) as Num_Cuenta,
	        CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Codigo) as varchar(50)) as int) as Codigo,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Password_Cuenta) as varchar(max)) as Password_Cuenta
            FROM Cuenta_Banco
            WHERE Id_RegistroCuenta = @Id_RegistroCuenta",
        new SqlParameter("@Id_RegistroCuenta", Id_RegistroCuenta)).FirstOrDefaultAsync();

                if(cuentaExistente == null)
                {
                    return NotFound();
                }
                string Num_Cuenta = cuenta.Num_Cuenta;
                int Codigo = cuenta.Codigo;
                string Password_Cuenta = cuenta.Password_Cuenta;

                await _cuentaContext.Database.ExecuteSqlRawAsync(
                @"UPDATE Cuenta_Banco
                    SET
                       Num_Cuenta = EncryptByPassPhrase('ServiciosWeb2023.', @Num_Cuenta),
                       Codigo = EncryptByPassPhrase('ServiciosWeb2023.', CAST(CAST(@Codigo AS nvarchar(max)) AS varchar(max))),
                       Password_Cuenta = EncryptByPassPhrase('ServiciosWeb2023.', @Password_Cuenta)
                    WHERE Id_RegistroCuenta = @Id_RegistroCuenta",
                new SqlParameter("@Num_Cuenta", Num_Cuenta),
                new SqlParameter("@Codigo", Codigo),
                new SqlParameter("@Password_Cuenta", Password_Cuenta),
                new SqlParameter("@Id_RegistroCuenta", Id_RegistroCuenta));

                await _cuentaContext.SaveChangesAsync();

            }
            catch (DbUpdateException ex)
            {
                if (!CuentaExist(Id_RegistroCuenta))
                {
                    return NotFound();
                }
                return BadRequest();
            }
            return NoContent();
        }

        [HttpDelete("{Id_RegistroCuenta}")]
        public async Task<IActionResult> DeleteCuenta(string Id_RegistroCuenta)
        {
            var query = $"DELETE FROM Cuenta_Banco WHERE Id_RegistroCuenta = '{Id_RegistroCuenta}'";

            try
            {
                await _cuentaContext.Database.ExecuteSqlRawAsync(query);
                return NoContent();
            }
            catch (Exception ex)
            {
  
                return StatusCode(500, $"Error al eliminar la cuenta: {ex.Message}");
            }
        }

        private bool CuentaExist(string Id_RegistroCuenta)
        {
            return (_cuentaContext.Cuenta_Banco?.Any(e => e.Id_RegistroCuenta == Id_RegistroCuenta)).GetValueOrDefault();
        }
    }
}
