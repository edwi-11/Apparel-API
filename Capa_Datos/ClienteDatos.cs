using System.Data;
using CapaEntidades;
using Microsoft.Data.SqlClient;

namespace Capa_Datos
{
    public class ClienteDatos
    {
        private readonly string _connectionString;

        public ClienteDatos(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool CrearCliente(Cliente cliente)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertarCliente", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NumeroCompra", (object?)cliente.NumeroCompra ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Nombre", (object?)cliente.Nombre ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Apellido", (object?)cliente.Apellido ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NumeroTelefono", (object?)cliente.NumeroTelefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Cedula", (object?)cliente.Cedula ?? DBNull.Value);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public bool ActualizarCliente(Cliente cliente)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarCliente", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodCliente", cliente.CodCliente);
                    cmd.Parameters.AddWithValue("@NumeroCompra", (object?)cliente.NumeroCompra ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Nombre", (object?)cliente.Nombre ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Apellido", (object?)cliente.Apellido ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NumeroTelefono", (object?)cliente.NumeroTelefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Cedula", (object?)cliente.Cedula ?? DBNull.Value);
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        public bool EliminarCliente(int codCliente)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_EliminarCliente", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodCliente", codCliente);
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        public List<Cliente> ObtenerClientePorId(int codCliente)
        {
            List<Cliente> clientes = new List<Cliente>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_VerClientePorID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CodCliente", codCliente);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clientes.Add(new Cliente
                            {
                                CodCliente = Convert.ToInt32(reader["CodCliente"]),
                                NumeroCompra = reader["NumeroCompra"] == DBNull.Value ? null : Convert.ToInt32(reader["NumeroCompra"]),
                                Nombre = reader["Nombre"]?.ToString(),
                                Apellido = reader["Apellido"]?.ToString(),
                                NumeroTelefono = reader["NumeroTelefono"] == DBNull.Value ? null : Convert.ToInt32(reader["NumeroTelefono"]),
                                Cedula = reader["Cedula"]?.ToString()
                            });
                        }
                    }
                }
            }
            return clientes;
        }

        public List<Cliente> ObtenerTodosClientes()
        {
            List<Cliente> clientes = new List<Cliente>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_VerTodosClientes", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clientes.Add(new Cliente
                            {
                                CodCliente = Convert.ToInt32(reader["CodCliente"]),
                                NumeroCompra = reader["NumeroCompra"] == DBNull.Value ? null : Convert.ToInt32(reader["NumeroCompra"]),
                                Nombre = reader["Nombre"]?.ToString(),
                                Apellido = reader["Apellido"]?.ToString(),
                                NumeroTelefono = reader["NumeroTelefono"] == DBNull.Value ? null : Convert.ToInt32(reader["NumeroTelefono"]),
                                Cedula = reader["Cedula"]?.ToString()
                            });
                        }
                    }
                }
            }
            return clientes;
        }
    }
}