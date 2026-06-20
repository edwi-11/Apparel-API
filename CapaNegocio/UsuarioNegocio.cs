using Capa_Datos;
using CapaEntidades;
using CapaNegocio;
using Konscious.Security.Cryptography;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CapaNegocio
{
    public class UsuarioNegocio
    {
        private readonly UsuariosDatos _usuarioDatos;

        public UsuarioNegocio(string connectionString)
        {
            _usuarioDatos = new UsuariosDatos(connectionString);
        }

        public bool CrearUsuario(Usuarios usuario)
        {
            // Hashear contraseña antes de guardar
            byte[] salt = GenerarSalt();
            usuario.Contraseña = HashearClave(usuario.Contraseña, salt);

            return _usuarioDatos.CrearUsuario(usuario);
        }

        public bool ActualizarUsuario(Usuarios usuario)
        {
            var usuarioExistente = _usuarioDatos.Login(usuario.UsuarioLogin);
            if (usuarioExistente == null) return false;
            if (usuario.Contraseña != usuarioExistente.Contraseña)
            {
                byte[] salt = GenerarSalt();
                usuario.Contraseña = HashearClave(usuario.Contraseña, salt);
            }
            return _usuarioDatos.ActualizarUsuario(usuario);
        }
        public bool EliminarUsuario(int codUsuario)
        {
            return _usuarioDatos.EliminarUsuario(codUsuario);
        }
        public List<Usuarios> ObtenerUsuario(int codUsuario)
        {
            return _usuarioDatos.ObtenerUsuarios(codUsuario);
        }
        public List<Usuarios> ObtenerTodosUsuarios()
        {
            return _usuarioDatos.ObtenerTodosUsuarios();
        }
        public Usuarios? Login(string usuarioLogin, string contraseña)
        {
            var usuario = _usuarioDatos.Login(usuarioLogin);
            if (usuario == null) return null;

            // Verificar contraseña
            if (VerificarClave(contraseña, usuario.Contraseña))
                return usuario;

            return null;
        }

        private byte[] GenerarSalt(int tamaño = 16)
        {
            byte[] salt = new byte[tamaño];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private string HashearClave(string clave, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(clave))
            {
                Salt = salt,
                DegreeOfParallelism = 2,
                Iterations = 4,
                MemorySize = 1024
            };

            var hashBytes = argon2.GetBytes(16);
            return Convert.ToBase64String(salt) + "." + Convert.ToBase64String(hashBytes);
        }

        private bool VerificarClave(string clave, string hash)
        {
            var partes = hash.Split('.');
            if (partes.Length != 2) return false;

            var salt = Convert.FromBase64String(partes[0]);
            var hashOriginal = partes[1];
            var hashNuevo = HashearClave(clave, salt).Split('.')[1];

            return hashNuevo == hashOriginal;
        }
    }
}
