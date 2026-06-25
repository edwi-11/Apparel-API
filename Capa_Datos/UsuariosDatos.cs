using CapaEntidades;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UsuariosDatos
{

    public readonly string _connectionString;

    public UsuariosDatos(string connectionString)
    {
        _connectionString = connectionString;
    }

    public bool CrearUsuario(Usuarios usuario)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("sp_AgregarUsuario", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@Apellidos", usuario.Apellidos);
                cmd.Parameters.AddWithValue("@UsuarioLogin", usuario.UsuarioLogin);
                cmd.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
                cmd.Parameters.AddWithValue("@CodRol", usuario.CodRol);
                cmd.Parameters.AddWithValue("@Cedula", usuario.Cedula);
                cmd.Parameters.AddWithValue("@Estado", usuario.Estado);
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows <= 0)
                {
                    throw new Exception("No se pudo crear el usuario.");
                }
            }
        }
        return true;
    }

    public bool ActualizarUsuario(Usuarios usuario)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("sp_ActualizarUsuario", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodUsuario", usuario.CodUsuario);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@Apellidos", usuario.Apellidos);
                cmd.Parameters.AddWithValue("@UsuarioLogin", usuario.UsuarioLogin);
                cmd.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
                cmd.Parameters.AddWithValue("@CodRol", usuario.CodRol);
                cmd.Parameters.AddWithValue("@Cedula", usuario.Cedula);
                cmd.Parameters.AddWithValue("@Estado", usuario.Estado);
                conn.Open();
                int rows = cmd.ExecuteNonQuery();


                return true;
            }
        }
    }

    public List<Usuarios> ObtenerUsuarios(int CodUsuario)
    {
        List<Usuarios> lista = new List<Usuarios>();
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("sp_VerUsuarioPorID", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodUsuario", CodUsuario);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Usuarios
                        {
                            CodUsuario = Convert.ToInt32(reader["CodUsuario"]),
                            Nombre = reader["Nombre"].ToString()!,
                            Apellidos = reader["Apellidos"].ToString()!,
                            UsuarioLogin = reader["UsuarioLogin"].ToString()!,
                            Contraseña = reader["Contraseña"].ToString()!,
                            CodRol = Convert.ToInt32(reader["CodRol"]),
                            Cedula = reader["Cedula"].ToString()!,
                            Estado = Convert.ToInt32(reader["estado"])!
                        });
                    }
                }
            }
        }
        return lista;
    }

    public List<Usuarios> ObtenerTodosUsuarios()
    {
        List<Usuarios> lista = new List<Usuarios>();
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("sp_ObtenerTodosUsuarios", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Usuarios
                        {
                            CodUsuario = Convert.ToInt32(reader["CodUsuario"]),
                            Nombre = reader["Nombre"].ToString()!,
                            Apellidos = reader["Apellidos"].ToString()!,
                            UsuarioLogin = reader["UsuarioLogin"].ToString()!,
                            Contraseña = reader["Contraseña"].ToString()!,
                            CodRol = Convert.ToInt32(reader["CodRol"]),
                            Cedula = reader["Cedula"].ToString()!,
                            Estado = Convert.ToInt32(reader["estado"])
                        });
                    }
                }
            }
        }
        return lista;
    }

    public bool EliminarUsuario(int CodUsuario)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("sp_EliminarUsuario", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodUsuario", CodUsuario);
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return true;
            }
        }
    }
    public Usuarios? Login(string usuarioLogin)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("sp_LoginUsuario", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UsuarioLogin", usuarioLogin);
                    conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Usuarios
                        {
                            CodUsuario = Convert.ToInt32(reader["CodUsuario"]),
                            Nombre = reader["Nombre"].ToString()!,
                            Apellidos = reader["Apellidos"].ToString()!,
                            UsuarioLogin = reader["UsuarioLogin"].ToString()!,
                            Contraseña = reader["Contraseña"].ToString()!,
                            CodRol = Convert.ToInt32(reader["CodRol"]),
                            RolNombre = reader["RolNombre"].ToString()!
                        };
                    }
                }
            }
        }
        return null;
    }
}
