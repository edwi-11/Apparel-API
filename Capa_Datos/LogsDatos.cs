using System.Text.RegularExpressions;
using CapaEntidades;
using Capa_Datos.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Capa_Datos
{
    public class LogsDatos
    {
        private readonly MongoMetricasContext _contexto;

        public LogsDatos(MongoMetricasContext contexto)
        {
            _contexto = contexto;
        }

        public bool InsertarLog(LogApi log)
        {
            if (!_contexto.EstaConfigurado)
            {
                return false;
            }

            _contexto.AsegurarIndices();

            _contexto.Logs.InsertOne(new LogApiDocumento
            {
                UsuarioId = log.UsuarioId,
                Usuario = log.Usuario,
                Modulo = log.Modulo,
                Ruta = log.Ruta,
                Metodo = log.Metodo,
                StatusCode = log.StatusCode,
                TiempoRespuestaMs = log.TiempoRespuestaMs,
                Ip = log.Ip,
                Fecha = log.Fecha
            });

            return true;
        }

        public (List<LogApi> Items, long Total) ObtenerLogs(
            string? texto, string? modulo, string? metodo, int pagina, int tamanoPagina)
        {
            List<LogApi> logs = new List<LogApi>();

            if (!_contexto.EstaConfigurado)
            {
                return (logs, 0);
            }

            var f = Builders<LogApiDocumento>.Filter;
            var filtros = new List<FilterDefinition<LogApiDocumento>> { f.Empty };

            if (!string.IsNullOrWhiteSpace(modulo))
            {
                filtros.Add(f.Eq(x => x.Modulo, modulo));
            }

            if (!string.IsNullOrWhiteSpace(metodo))
            {
                filtros.Add(f.Eq(x => x.Metodo, metodo.ToUpperInvariant()));
            }

            if (!string.IsNullOrWhiteSpace(texto))
            {
                var regex = new BsonRegularExpression(Regex.Escape(texto.Trim()), "i");
                filtros.Add(f.Or(
                    f.Regex(x => x.Ruta, regex),
                    f.Regex(x => x.Usuario, regex)));
            }

            var filtro = f.And(filtros);

            long total = _contexto.Logs.CountDocuments(filtro);

            List<LogApiDocumento> documentos = _contexto.Logs
                .Find(filtro)
                .SortByDescending(x => x.Fecha)
                .Skip((pagina - 1) * tamanoPagina)
                .Limit(tamanoPagina)
                .ToList();

            foreach (LogApiDocumento d in documentos)
            {
                logs.Add(new LogApi
                {
                    Id = d.Id,
                    UsuarioId = d.UsuarioId,
                    Usuario = d.Usuario,
                    Modulo = d.Modulo,
                    Ruta = d.Ruta,
                    Metodo = d.Metodo,
                    StatusCode = d.StatusCode,
                    TiempoRespuestaMs = d.TiempoRespuestaMs,
                    Ip = d.Ip,
                    Fecha = d.Fecha
                });
            }

            return (logs, total);
        }
    }
}