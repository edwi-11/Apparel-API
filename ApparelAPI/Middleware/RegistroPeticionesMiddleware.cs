using System.Diagnostics;
using Capa_Datos.Mongo;
using CapaEntidades;
using CapaNegocio;

namespace ApparelAPI.Middleware
{
    public class RegistroPeticionesMiddleware
    {
        private static readonly string[] RutasIgnoradas = { "/swagger", "/api/Logs", "/favicon" };

        private readonly RequestDelegate _next;
        private readonly LogsNegocio _logsNegocio;
        private readonly ILogger<RegistroPeticionesMiddleware> _logger;

        public RegistroPeticionesMiddleware(
            RequestDelegate next,
            MongoMetricasContext contexto,
            ILogger<RegistroPeticionesMiddleware> logger)
        {
            _next = next;
            _logsNegocio = new LogsNegocio(contexto);
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string ruta = context.Request.Path.Value ?? string.Empty;

            if (RutasIgnoradas.Any(r => ruta.StartsWith(r, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }

            var reloj = Stopwatch.StartNew();
            try
            {
                await _next(context);
            }
            finally
            {
                reloj.Stop();

                int? usuarioId = int.TryParse(context.User.FindFirst("codUsuario")?.Value, out int id)
                    ? id
                    : null;

                var log = new LogApi
                {
                    UsuarioId = usuarioId,
                    Usuario = context.User.Identity?.Name,
                    Modulo = ObtenerModulo(ruta),
                    Ruta = ruta,
                    Metodo = context.Request.Method,
                    StatusCode = context.Response.StatusCode,
                    TiempoRespuestaMs = reloj.Elapsed.TotalMilliseconds,
                    Ip = context.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
                    Fecha = DateTime.UtcNow
                };

                _ = Task.Run(() => Guardar(log));
            }
        }

        private void Guardar(LogApi log)
        {
            try
            {
                _logsNegocio.RegistrarLog(log);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "No se pudo guardar el log de la petición.");
            }
        }

        // /api/Cliente/telefono/8888 -> "Cliente"
        private static string ObtenerModulo(string ruta)
        {
            string[] partes = ruta.Split('/', StringSplitOptions.RemoveEmptyEntries);
            return partes.Length >= 2 ? partes[1] : "General";
        }
    }
}