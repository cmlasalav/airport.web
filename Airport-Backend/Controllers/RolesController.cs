using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoServiciosWeb.Models;

namespace ProyectoServiciosWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RolesItem _rolesContext;

        public RolesController(RolesItem RolesContext) 
        {
            _rolesContext = RolesContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Roles>>> GetRoles()
        {
            // Consulta SQL para obtener datos 
            var roles = await _rolesContext.Roles.FromSqlRaw(@"SELECT 
	            Id_Rol,
	            CAST(DecryptByPassPhrase('ServiciosWeb2023.', Nombre_Rol) as varchar(max)) as Nombre_Rol,
	            CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion_Rol) as varchar(max)) as Descripcion_Rol
            FROM Roles;").ToListAsync();

            return roles;
        }

        [HttpGet("{Id_Rol}")]
        public async Task<ActionResult<IEnumerable<Roles>>> GetRolesId(int Id_Rol)
        {
            // Consulta SQL para obtener datos 
            var roles = await _rolesContext.Roles.FromSqlRaw(@"SELECT 
	            Id_Rol,
	            CAST(DecryptByPassPhrase('ServiciosWeb2023.', Nombre_Rol) as varchar(max)) as Nombre_Rol,
	            CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion_Rol) as varchar(max)) as Descripcion_Rol
            FROM Roles
            WHERE Id_Rol = @Id_Rol",
            new SqlParameter("Id_Rol", Id_Rol)).ToListAsync();

            return roles;
        }
    }
}
