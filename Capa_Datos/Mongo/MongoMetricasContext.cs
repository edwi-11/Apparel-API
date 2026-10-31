using MongoDB.Driver;

namespace Capa_Datos.Mongo
{
    public class MongoMetricasContext
    {
        private static readonly TimeSpan TiempoEsperaServidor = TimeSpan.FromSeconds(5);

        private readonly Lazy<bool> _indices;

        internal IMongoDatabase? Database { get; }

        public bool EstaConfigurado { get; }

        public string Estado { get; }

        public MongoMetricasContext(string connectionString, string databaseName)
        {
            bool datosUtilizables =
                !string.IsNullOrWhiteSpace(connectionString)
                && !string.IsNullOrWhiteSpace(databaseName)
                && !connectionString.Contains('<');

            MongoClientSettings? settings = null;
            Estado = "MongoDB no está configurado.";

            if (datosUtilizables)
            {
                try
                {
                    settings = MongoClientSettings.FromConnectionString(connectionString);
                }
                catch (Exception)
                {
                    Estado = "La cadena de conexión de MongoDB no es válida.";
                }
            }

            if (settings != null)
            {
                settings.ServerSelectionTimeout = TiempoEsperaServidor;
                Database = new MongoClient(settings).GetDatabase(databaseName);
                EstaConfigurado = true;
                Estado = "MongoDB configurado.";
            }

            _indices = new Lazy<bool>(CrearIndices);
        }

        internal IMongoCollection<LogApiDocumento> Logs =>
            Database!.GetCollection<LogApiDocumento>("apiUsageLogs");

        public void AsegurarIndices()
        {
            _ = _indices.Value;
        }

        private bool CrearIndices()
        {
            if (!EstaConfigurado)
            {
                return false;
            }

            try
            {
                var porFecha = Builders<LogApiDocumento>.IndexKeys.Descending(x => x.Fecha);
                Logs.Indexes.CreateOne(new CreateIndexModel<LogApiDocumento>(porFecha));

                var porRutaYFecha = Builders<LogApiDocumento>.IndexKeys
                    .Ascending(x => x.Ruta)
                    .Descending(x => x.Fecha);
                Logs.Indexes.CreateOne(new CreateIndexModel<LogApiDocumento>(porRutaYFecha));

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}