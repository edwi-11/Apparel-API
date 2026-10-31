using Capa_Datos;
using Capa_Datos.Mongo;
using CapaEntidades;

namespace CapaNegocio
{
    public class LogsNegocio
    {
        private const int TamanoMaximo = 100;

        private readonly LogsDatos _logsDatos;

        public LogsNegocio(MongoMetricasContext contexto)
        {
            _logsDatos = new LogsDatos(contexto);
        }

        public bool RegistrarLog(LogApi log) => _logsDatos.InsertarLog(log);

        public (List<LogApi> Items, long Total) ObtenerLogs(
            string? texto, string? modulo, string? metodo, int pagina, int tamanoPagina)
        {
            pagina = Math.Max(pagina, 1);
            tamanoPagina = Math.Clamp(tamanoPagina, 1, TamanoMaximo);

            return _logsDatos.ObtenerLogs(texto, modulo, metodo, pagina, tamanoPagina);
        }
    }
}