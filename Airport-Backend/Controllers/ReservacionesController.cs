using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoServiciosWeb.Models;

namespace ProyectoServiciosWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservacionesController : ControllerBase
    {
        private readonly ReservacionesItem _reservacionesContext;

        public ReservacionesController (ReservacionesItem reservacionesContext)
        {
            _reservacionesContext = reservacionesContext;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Reservaciones>>> GetReservaciones()
        {
            var reservaciones = await _reservacionesContext.Reservaciones.FromSqlRaw(@"SELECT
	            Id_Reservacion,
	            Id_Usuario,
	            Id_Vuelo,
	            CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Numero_Boletos) as varchar(50)) as int) as Numero_Boletos,
	            CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Total) as varchar(50)) as decimal(6,2)) as Total,
	            CAST(DecryptByPassPhrase('ServiciosWeb2023.', Estado_Reservacion) as varchar(max)) as Estado_Reservacion
            FROM Reservaciones;").ToListAsync();
            return reservaciones;
        }


        [HttpGet("{Id_Reservacion}")]

        public async Task<ActionResult<IEnumerable<Reservaciones>>> GetReservacionesID(string Id_Reservacion)
        {
            var reservaciones = await _reservacionesContext.Reservaciones.FromSqlRaw(@"SELECT
	            Id_Reservacion,
	            Id_Usuario,
	            Id_Vuelo,
	            CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Numero_Boletos) as varchar(50)) as int) as Numero_Boletos,
	            CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Total) as varchar(50)) as decimal(6,2)) as Total,
	            CAST(DecryptByPassPhrase('ServiciosWeb2023.', Estado_Reservacion) as varchar(max)) as Estado_Reservacion
            FROM Reservaciones
            WHERE Id_Reservacion = @Id_Reservacion",
            new SqlParameter("@Id_Reservacion", Id_Reservacion)).FirstOrDefaultAsync();
            return Ok(reservaciones);
        }

        [HttpPost]
        public async Task<ActionResult<Reservaciones>> PostReservaciones(Reservaciones reservaciones)
        {
            try
            {
                string Id_Reservacion = (reservaciones.Id_Reservacion);
                string Id_Usuario = (reservaciones.Id_Usuario);
                string Id_Vuelo = (reservaciones.Id_Vuelo);
                int Numero_Boletos = (reservaciones.Numero_Boletos);
                decimal Total = (reservaciones.Total);         
                string Estado_Reservacion = (reservaciones.Estado_Reservacion);

                if (await _reservacionesContext.Reservaciones.AnyAsync(a => a.Id_Reservacion == Id_Reservacion))
                {
                    return BadRequest("Ya existe una reservacion con ese ID");
                }

                await _reservacionesContext.Database.ExecuteSqlRawAsync($@"INSERT INTO Reservaciones (Id_Reservacion, Id_Usuario, Id_Vuelo, Numero_Boletos, Total, Estado_Reservacion)
                    VALUES (
	                    '{Id_Reservacion}', 
	                    '{Id_Usuario}', 
	                    '{Id_Vuelo}', 
	                    EncryptByPassPhrase('ServiciosWeb2023.','{Numero_Boletos}'), 
	                    EncryptByPassPhrase('ServiciosWeb2023.', '{Total}'), 
	                    EncryptByPassPhrase('ServiciosWeb2023.','{Estado_Reservacion}')
                    );");

                var reservacion = await _reservacionesContext.Reservaciones.ToListAsync();
                return Ok(reservacion);

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

        [HttpPut("{Id_Reservacion}")]

        public async Task<IActionResult> PutReservaciones(string Id_Reservacion, Reservaciones reservaciones)
        {
            if (Id_Reservacion != reservaciones.Id_Reservacion)
            {
                return BadRequest();
            }
            try
            {
                var reservacionExistente = await _reservacionesContext.Reservaciones.FromSqlRaw(@"SELECT
	            Id_Reservacion,
	            Id_Usuario,
	            Id_Vuelo,
	            CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Numero_Boletos) as varchar(50)) as int) as Numero_Boletos,
	            CAST(CAST(DecryptByPassPhrase('ServiciosWeb2023.', Total) as varchar(50)) as decimal(6,2)) as Total,
	            CAST(DecryptByPassPhrase('ServiciosWeb2023.', Estado_Reservacion) as varchar(max)) as Estado_Reservacion
                FROM Reservaciones
                WHERE Id_Reservacion = @Id_Reservacion",
                new SqlParameter("@Id_Reservacion", Id_Reservacion)).FirstOrDefaultAsync();

                if (reservacionExistente == null)
                {
                    return NotFound();
                }
                int Numero_Boletos = reservaciones.Numero_Boletos;
                decimal Total = reservaciones.Total;
                string Estado_Reservacion = reservaciones.Estado_Reservacion;

                await _reservacionesContext.Database.ExecuteSqlRawAsync(
                @"UPDATE Reservaciones
                    SET
                       Numero_Boletos = EncryptByPassPhrase('ServiciosWeb2023.', CAST(CAST(@Numero_Boletos AS nvarchar(max)) AS varchar(max))),
                       Total = EncryptByPassPhrase('ServiciosWeb2023.', CAST(CAST(@Total AS nvarchar(max)) AS varchar(max))),
                       Estado_Reservacion = EncryptByPassPhrase('ServiciosWeb2023.', @Estado_Reservacion)
                    WHERE Id_Reservacion = @Id_Reservacion",
                new SqlParameter("@Numero_Boletos", Numero_Boletos),
                new SqlParameter("@Total", Total),
                new SqlParameter("@Estado_Reservacion", Estado_Reservacion),
                new SqlParameter("@Id_Reservacion", Id_Reservacion));

                await _reservacionesContext.SaveChangesAsync();

            }
            catch (DbUpdateException ex)
            {
                if (!ReservacionExist(Id_Reservacion))
                {
                    return NotFound();
                }
                return BadRequest();
            }
            return NoContent();
        }

        [HttpDelete("{Id_Reservacion}")]
        public async Task<IActionResult> DeleteReservaciones(string Id_Reservacion)
        {
            var query = $"DELETE FROM Reservaciones WHERE Id_Reservacion = '{Id_Reservacion}'";

            try
            {
                await _reservacionesContext.Database.ExecuteSqlRawAsync(query);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la tarjeta: {ex.Message}");
            }
        }

        private bool ReservacionExist(string Id_Reservacion)
        {
            return (_reservacionesContext.Reservaciones?.Any(e => e.Id_Reservacion == Id_Reservacion)).GetValueOrDefault();
        }
    }
}
