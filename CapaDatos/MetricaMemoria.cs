namespace CapaEntidades
{
    public class MetricaMemoria
    {
        public string? Id { get; set; }
        public double ProcesoMemoriaMB { get; set; }
        public double GcMemoriaMB { get; set; }
        public double UsoCpu { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public string Aplicacion { get; set; } = "ApparelAPI";
    }
}