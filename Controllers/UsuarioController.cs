using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoServiciosWeb.Models;

namespace ProyectoServiciosWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioItem _usuarioContext;

        public UsuarioController(UsuarioItem usuarioContext)
        {
            _usuarioContext = usuarioContext;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Usuarios>>> GetUsuarios()
        {
            // Consulta SQL para obtener datos 
            var usuarios = await _usuarioContext.Usuarios.FromSqlRaw(@"SELECT 
	        Id_Usuario,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Primer_Apellido) as varchar(max)) as Primer_Apellido,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Segundo_Apellido) as varchar(max)) as Segundo_Apellido,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Nombre) as varchar(max)) as Nombre,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Usuario) as varchar(max)) as Usuario,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Correo_Usuario) as varchar(max)) as Correo_Usuario,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Password_Usuario) as varchar(max)) as Password_Usuario,
	        Id_Pregunta,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Respuesta_Seguridad) as varchar(max)) as Respuesta_Seguridad,
	        Id_Rol
            From Usuarios;").ToListAsync();

            return usuarios;
        }

        [HttpPost]
        public async Task<ActionResult<Usuarios>> PostUsuario(Usuarios usuario)
        {
            try
            {
                string ID_Usuario = (usuario.Id_Usuario);
                string Primer_Apellido = usuario.Primer_Apellido;
                string Segundo_Apellido = usuario.Segundo_Apellido;
                string Nombre_Usuario = usuario.Nombre;
                string Usuario = usuario.Usuario;
                string Correo_Usuario = usuario.Correo_Usuario;
                string Password_Usuario = usuario.Password_Usuario;
                int Id_Pregunta = usuario.Id_Pregunta;
                string Respuesta_Seguridad = usuario.Respuesta_Seguridad;
                int Id_Rol = usuario.Id_Rol;

                if (await _usuarioContext.Usuarios.AnyAsync(a => a.Id_Usuario == ID_Usuario))
                {
                    return BadRequest("Ya existe un usuario con este ID.");
                }

                // Query SQL
                await _usuarioContext.Database.ExecuteSqlRawAsync($@"
                INSERT INTO Usuarios (Id_Usuario, Primer_Apellido, Segundo_Apellido, Nombre, Usuario, Correo_Usuario, Password_Usuario, Id_Pregunta, Respuesta_Seguridad, Id_Rol) 
                VALUES (
	                   '{ID_Usuario}', 
	                    EncryptByPassPhrase('ServiciosWeb2023.','{Primer_Apellido}'), 
	                    EncryptByPassPhrase('ServiciosWeb2023.','{Segundo_Apellido}'), 
	                    EncryptByPassPhrase('ServiciosWeb2023.','{Nombre_Usuario}'), 
	                    EncryptByPassPhrase('ServiciosWeb2023.','{Usuario}'), 
	                    EncryptByPassPhrase('ServiciosWeb2023.','{Correo_Usuario}'),
	                    EncryptByPassPhrase('ServiciosWeb2023.', '{Password_Usuario}'),
	                    {Id_Pregunta}, 
	                    EncryptByPassPhrase('ServiciosWeb2023.','{Respuesta_Seguridad}'),
	                    {Id_Rol}
                        );
                ");

                var usuarios = await _usuarioContext.Usuarios.ToListAsync();
                return Ok(usuarios);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 2627)
                {
                    return BadRequest("Ya existe un usuario con este ID.");
                }
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }

        }

        [HttpGet("{Id_Usuario}")]

        public async Task<ActionResult<Usuarios>> GetUsuarioID(string Id_Usuario)
        {
            // Consulta SQL para obtener datos 
            var usuarios = await _usuarioContext.Usuarios.FromSqlRaw(@"SELECT 
	        Id_Usuario,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Primer_Apellido) as varchar(max)) as Primer_Apellido,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Segundo_Apellido) as varchar(max)) as Segundo_Apellido,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Nombre) as varchar(max)) as Nombre,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Usuario) as varchar(max)) as Usuario,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Correo_Usuario) as varchar(max)) as Correo_Usuario,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Password_Usuario) as varchar(max)) as Password_Usuario,
	        Id_Pregunta,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Respuesta_Seguridad) as varchar(max)) as Respuesta_Seguridad,
	        Id_Rol
            From Usuarios 
            WHERE Id_Usuario = @Id_Usuario",
        new SqlParameter("@Id_Usuario", Id_Usuario)).FirstOrDefaultAsync();

            return Ok(usuarios);

        }

        [HttpPut("{Id_Usuario}")]
        public async Task<IActionResult> PutUsuario(string Id_Usuario, Usuarios usuario)
        {
            if (Id_Usuario != usuario.Id_Usuario)
            {
                return BadRequest();
            }

            try
            {
                // Desencriptar la tabla
                var usuarioExistente = await _usuarioContext.Usuarios.FromSqlRaw(
                 @"SELECT 
	                Id_Usuario,
	                CAST(DecryptByPassPhrase('ServiciosWeb2023.', Primer_Apellido) as varchar(max)) as Primer_Apellido,
	                CAST(DecryptByPassPhrase('ServiciosWeb2023.', Segundo_Apellido) as varchar(max)) as Segundo_Apellido,
	                CAST(DecryptByPassPhrase('ServiciosWeb2023.', Nombre) as varchar(max)) as Nombre,
	                CAST(DecryptByPassPhrase('ServiciosWeb2023.', Usuario) as varchar(max)) as Usuario,
	                CAST(DecryptByPassPhrase('ServiciosWeb2023.', Correo_Usuario) as varchar(max)) as Correo_Usuario,
	                CAST(DecryptByPassPhrase('ServiciosWeb2023.', Password_Usuario) as varchar(max)) as Password_Usuario,
	                Id_Pregunta,
	                CAST(DecryptByPassPhrase('ServiciosWeb2023.', Respuesta_Seguridad) as varchar(max)) as Respuesta_Seguridad,
	                Id_Rol
                From Usuarios 
                WHERE Id_Usuario = @Id_Usuario",
                new SqlParameter("@Id_Usuario", Id_Usuario)).FirstOrDefaultAsync();

                if (usuarioExistente == null)
                {
                    return NotFound();
                }

                // Actualizar valores 
                string Primer_Apellido = usuario.Primer_Apellido;
                string Segundo_Apellido = usuario.Segundo_Apellido;
                string Nombre_Usuario = usuario.Nombre;
                string Usuario = usuario.Usuario;
                string Correo_Usuario = usuario.Correo_Usuario;
                string Password_Usuario = usuario.Password_Usuario;
                int Id_Pregunta = usuario.Id_Pregunta;
                string Respuesta_Seguridad = usuario.Respuesta_Seguridad;
                int Id_Rol = usuario.Id_Rol;

                // Encriptar tabla
                await _usuarioContext.Database.ExecuteSqlRawAsync(
            @"UPDATE Usuarios
                SET 
                    Primer_Apellido = EncryptByPassPhrase('ServiciosWeb2023.', @Primer_Apellido),
                    Segundo_Apellido = EncryptByPassPhrase('ServiciosWeb2023.', @Segundo_Apellido),
                    Nombre = EncryptByPassPhrase('ServiciosWeb2023.', @Nombre_Usuario),
                    Usuario = EncryptByPassPhrase('ServiciosWeb2023.', @Usuario),
                    Correo_Usuario = EncryptByPassPhrase('ServiciosWeb2023.', @Correo_Usuario),
                    Password_Usuario = EncryptByPassPhrase('ServiciosWeb2023.', @Password_Usuario),
                    Id_Pregunta = @Id_Pregunta,
                    Respuesta_Seguridad = EncryptByPassPhrase('ServiciosWeb2023.', @Respuesta_Seguridad),
                    Id_Rol = @Id_Rol
                    WHERE Id_Usuario = @Id_Usuario",
                //Parametros para almacenar
                new SqlParameter("@Primer_Apellido", Primer_Apellido),
                new SqlParameter("@Segundo_Apellido", Segundo_Apellido),
                new SqlParameter("@Nombre_Usuario", Nombre_Usuario),
                new SqlParameter("@Usuario", Usuario),
                new SqlParameter("@Correo_Usuario", Correo_Usuario),
                new SqlParameter("@Password_Usuario", Password_Usuario),
                new SqlParameter("@Id_Pregunta", Id_Pregunta),
                new SqlParameter("@Respuesta_Seguridad", Respuesta_Seguridad),
                new SqlParameter("@Id_Rol", Id_Rol),

                new SqlParameter("@Id_Usuario", Id_Usuario));

                await _usuarioContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExist(Id_Usuario))
                {
                    return NotFound();
                }
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{Id_Usuario}")]
        public async Task<IActionResult> DeleteUsuario(string Id_Usuario)
        {
            if (_usuarioContext.Usuarios == null)
            {
                return NotFound();
            }
            var Usuarios = await _usuarioContext.Usuarios.FindAsync(Id_Usuario);
            if (Usuarios == null)
            {
                return NotFound();
            }
            _usuarioContext.Usuarios.Remove(Usuarios);
            await _usuarioContext.SaveChangesAsync();
            return NoContent();
        }


        private bool UsuarioExist(string Id_Usuario)
        {
            return (_usuarioContext.Usuarios?.Any(e => e.Id_Usuario == Id_Usuario)).GetValueOrDefault();
        }
    }

   
}
