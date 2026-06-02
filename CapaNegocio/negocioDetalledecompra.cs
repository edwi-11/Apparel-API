using Capa_Datos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class negocioDetalledecompra
    {
        private readonly CompraDatos _datos;

        public negocioDetalledecompra(string connectionString)
        {
            _datos = new CompraDatos(connectionString);
        }
        public bool detalleCompraNegocio(int idCompra, detalleCompra detalle)
        {
            return _datos.insertardetalledecompra(idCompra, detalle);
        }

        public int CompraNegocio(CompraDatos compras)
        {
            return _datos.CrearCompra(new Compras() );

        }

        public List<detalleCompra> ListadoCompras()
        {
            return _datos.ListarCompras();
        }

    }
}
