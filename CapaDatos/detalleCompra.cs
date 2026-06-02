using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class detalleCompra
    {
        public int CodigoCompra { get; set; }
        public int codigoProducto { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public int CodigoProveedor { get; set; }

        public int Cantidad { get; set; }

    }




}
