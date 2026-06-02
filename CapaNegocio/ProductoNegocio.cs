using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Datos;
using CapaEntidades;
using Microsoft.Identity.Client;
namespace CapaNegocio
{
    public class ProductoNegocio
    {
        
        private readonly ProductoDatos _datos;

     

        public ProductoNegocio(string connectionString)
        {
            _datos = new ProductoDatos(connectionString);
        }

        public int ObtenerStockProducto(Producto producto)
        {
            return _datos.Stock(producto.CodigoProducto);
        }
        public bool InsertarProducto(Producto producto)
        {
            return _datos.CrearProducto(producto);

        }
        public bool ActualizarProducto(Producto producto)
        {
            return _datos.ActualizarProducto(producto);
        }

        public bool EliminarProducto(int codigoProducto)
        {
            return _datos.EliminarProducto(codigoProducto);
        }

      public  List<Producto> VerProductos(int? CodigoProducto = null)
        {
            return _datos.ObtenerProductos(CodigoProducto.Value);
        }
    }
}
