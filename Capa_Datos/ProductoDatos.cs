using CapaEntidades;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Datos
{
    public class ProductoDatos
    {
        private readonly string _connectionString;

        public ProductoDatos(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool CrearProducto(Producto producto)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertarProducto", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@Talla", producto.Talla);
                    cmd.Parameters.AddWithValue("@Marca", producto.Marca);
                    cmd.Parameters.AddWithValue("@Estado", producto.Estado);
                    cmd.Parameters.AddWithValue("@CodCategoria", producto.CodCategoria);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return true;

                }
            }
        }
        
        public bool ActualizarProducto(Producto producto)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarProducto", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoProducto", producto.CodigoProducto);
                    cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@Talla", producto.Talla);
                    cmd.Parameters.AddWithValue("@Marca", producto.Marca);
                    cmd.Parameters.AddWithValue("@Estado", producto.Estado);
                    cmd.Parameters.AddWithValue("@CodCategoria", producto.CodCategoria);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);


                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public bool EliminarProducto(int codigoProducto)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_EliminarProducto", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoProducto", codigoProducto);
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return true;
                }
            }


        }
        public int Stock(int codigoProducto)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("StockActual", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodProducto", codigoProducto);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }

                    return 0;
                }
            }
        }

        public List<Producto> ObtenerProductos(int CodigoProducto)
        {
            List<Producto> productos = new List<Producto>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_VerProductoPorID", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodigoProducto", CodigoProducto);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())

                    {
                        while (reader.Read())
                        {
                            Producto producto = new Producto
                            {
                                CodigoProducto = Convert.ToInt32(reader["CodigoProducto"]),
                                Nombre = reader["Nombre"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                                Talla = Convert.ToInt32(reader["Talla"]),
                                Marca = reader["Marca"].ToString(),
                                Estado = Convert.ToInt32(reader["Estado"]),
                                CodCategoria = Convert.ToInt32(reader["CodCategoria"]),
                                Precio = Convert.ToDecimal(reader["Precio"])
                            };
                            productos.Add(producto);
                        }
                    }
                }
            }
            return productos;
        }

     
    }
}

