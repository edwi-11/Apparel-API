using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;
using Capa_Datos;
using System.Xml;

namespace CapaNegocio
{
    public class VentaBLL
    {
        private readonly VentaDAL _dal;

        public VentaBLL(VentaDAL dal)
        {
            _dal = dal;
        }

        public int RegistrarVenta(Ventas venta)
        {
            if (venta == null)
                throw new Exception("Venta inválida");

            if (venta.Detalles == null || venta.Detalles.Count == 0)
                throw new Exception("Debe agregar productos a la venta");

            return _dal.RegistrarVenta(venta);
        }
    }
}
