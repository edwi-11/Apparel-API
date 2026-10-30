using System;
using System.Collections.Generic;

namespace CapaEntidades
{
    public class Ventas
    {
        public int CodCliente { get; set; }
        public int CodUsuario { get; set; }
        public List<DetalleVenta> Detalles { get; set; } = new();
    }

    public class VentaListado
    {
        public int CodVenta { get; set; }
        public int? CodCliente { get; set; }
        public int? CodUsuario { get; set; }
        public DateTime? FechaVenta { get; set; }
        public decimal Total { get; set; }
        public List<DetalleVentaListado> Detalles { get; set; } = new();
    }

    public class DetalleVentaListado
    {
        public int CodDetalleVenta { get; set; }
        public int CodVenta { get; set; }
        public int? CodigoProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Descuento { get; set; }
        public decimal Subtotal { get; set; }
    }
}