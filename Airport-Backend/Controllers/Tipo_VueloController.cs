using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProyectoServiciosWeb.Models;

namespace ProyectoServiciosWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Tipo_VueloController : ControllerBase
    {
        private readonly Tipo_VueloItem _tipovueloContext;

        public Tipo_VueloController(Tipo_VueloItem tipovueloContext)
        {
            _tipovueloContext = tipovueloContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tipo_Vuelo>>> GetTipoVuelo()
        {
            var tipoVuelo = await _tipovueloContext.Tipo_Vuelo.FromSqlRaw(@"SELECT 
	        Id_Tipo,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion_Tipo) as varchar(max)) as Descripcion_Tipo
            FROM Tipo_Vuelo;").ToListAsync();
            return tipoVuelo;
        }

        [HttpGet("{Id_Tipo}")]
        public async Task<ActionResult<IEnumerable<Tipo_Vuelo>>> GetTipoVueloID(int Id_Tipo)
        {
            var tipoVuelo = await _tipovueloContext.Tipo_Vuelo.FromSqlRaw(@"SELECT 
	        Id_Tipo,
	        CAST(DecryptByPassPhrase('ServiciosWeb2023.', Descripcion_Tipo) as varchar(max)) as Descripcion_Tipo
            FROM Tipo_Vuelo
            WHERE Id_Tipo = @Id_Tipo",
            new SqlParameter("@Id_Tipo", Id_Tipo)).FirstOrDefaultAsync();
            return Ok(tipoVuelo);
        }
    }
}
