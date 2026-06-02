using System;
using CapaEntidades;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capa_Datos
{
    public class CompraDatos
    {
        private readonly string _connectionString;

        public CompraDatos(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para crear una compra y obtener el IdCompra generado
        public int CrearCompra(Compras compra)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_InsetarCompra", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Total", compra.total);
                cmd.Parameters.AddWithValue("@FormaPago", compra.FormaPago);
                cmd.Parameters.AddWithValue("@FechaCompra", compra.Fecha);

                conn.Open();
                object result = cmd.ExecuteScalar(); 
                return Convert.ToInt32(result); // devuelve el IdCompra generado
            }
        }

        public List<detalleCompra> ListarCompras()
        {
            List<detalleCompra> lista = new List<detalleCompra>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT CodCompra,CodigoProducto, PrecioUnitario, SubTotal, CodigoProvedor from DetalleCompra", conn))
            {
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new detalleCompra
                    {
                        CodigoCompra = dr["CodCompra"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CodCompra"]),
                        codigoProducto = dr["CodigoProducto"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CodigoProducto"]),
                        PrecioUnitario = Convert.ToDecimal(dr["PrecioUnitario"]),
                        Subtotal = Convert.ToDecimal(dr["SubTotal"]),
                        CodigoProveedor = Convert.ToInt32(dr["CodigoProvedor"]),
                    });
                }
            }

            return lista;
        }


        // Método para insertar un detalle de compra usando IdCompra
        public bool insertardetalledecompra(int idCompra, detalleCompra detalledecompraDatos)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_InsertartarDetalleCompra", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idCompra", idCompra);
                cmd.Parameters.AddWithValue("@codigoproducto", detalledecompraDatos.codigoProducto);
                cmd.Parameters.AddWithValue("@preciounitario", detalledecompraDatos.PrecioUnitario);
                cmd.Parameters.AddWithValue("@subtotal", detalledecompraDatos.Subtotal);
                cmd.Parameters.AddWithValue("@codigoproveedor", detalledecompraDatos.CodigoProveedor);
                cmd.Parameters.AddWithValue("@cantidad", detalledecompraDatos.Cantidad);

                conn.Open();
                int nuevoid = Convert.ToInt32(cmd.ExecuteScalar());
                return nuevoid > 0;
            }
        }
    }
}
