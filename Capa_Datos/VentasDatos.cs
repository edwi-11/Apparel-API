using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CapaEntidades;
using System.Threading.Tasks;

namespace Capa_Datos
{
    internal class VentasDatos
    {
        private readonly string _connectionString;
        public VentasDatos(string connectionString)
        {
            _connectionString = connectionString;
        }


        /* public void InsertarVenta(Ventas Venta)

              {
                  try
                  {
                      using (SqlConnection con = new SqlConnection(_connectionString)) 
                      {
                          con.Open();

                          SqlTransaction tran = con.BeginTransaction();
                          SqlCommand cmd = new SqlCommand("Sp_InsertarVenta", con);
                          {
                              cmd.CommandType = System.Data.CommandType.StoredProcedure;
                              cmd.Parameters.AddWithValue("");
                              cmd.Parameters.AddWithValue("");
                              cmd.Parameters.AddWithValue("");
                              cmd.Parameters.AddWithValue("");


                              int IdVenta = Convert.ToInt32(cmd.ExecuteScalar());

                              //Insertamos los detalles de la venta

                              foreach (var detalle in Venta.DetalleVenta


                                      //trans.Comit() Significa que la transaccion esta normal y que retorne todo

                          }

                      }

                  }
                  catch (Exception)
                  {

                  }
        */


       /* public Ventas seleccionarVentaConDetalle(int IdVenta)

        {
            Ventas venta = new Ventas();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_selccionarventa", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                     
                }
            }


        }
        public interface IVentaDatos
        {
            Ventas SeleccionarVentaConDetalle(int ventaId);
            int InsertarVenta(Ventas ventas);
        }
       */
    }
}


        
    
