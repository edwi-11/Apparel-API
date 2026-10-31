namespace CapaEntidades
{
    public class LogApi
    {
        public string? Id { get; set; }
        public int? UsuarioId { get; set; }
        public string? Usuario { get; set; }
        public string Modulo { get; set; } = string.Empty;
        public string Ruta { get; set; } = string.Empty;
        public string Metodo { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public double TiempoRespuestaMs { get; set; }
        public string Ip { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
}