using Capa_Datos.Mongo;
using CapaNegocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApparelAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class LogsController : ControllerBase
    {
        private readonly LogsNegocio _logsNegocio;

        public LogsController(MongoMetricasContext contexto)
        {
            _logsNegocio = new LogsNegocio(contexto);
        }

        [HttpGet]
        public IActionResult ObtenerLogs(
            [FromQuery] string? texto,
            [FromQuery] string? modulo,
            [FromQuery] string? metodo,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanoPagina = 20)
        {
            var (items, total) = _logsNegocio.ObtenerLogs(texto, modulo, metodo, pagina, tamanoPagina);

            var data = items.Select(x => new
            {
                id = x.Id,
                fecha = x.Fecha,
                usuario = x.Usuario ?? "Anónimo",
                metodo = x.Metodo,
                ruta = x.Ruta,
                modulo = x.Modulo,
                status = x.StatusCode,
                tiempoRespuestaMs = x.TiempoRespuestaMs
            });

            return Ok(new { success = true, data, pagina, tamanoPagina, total });
        }
    }
}