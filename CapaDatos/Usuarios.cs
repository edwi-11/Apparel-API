using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class Usuarios
    {
        public int CodUsuario { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string UsuarioLogin { get; set; } = null!;
        public int CodRol { get; set; }
        public string Cedula { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
        public string RolNombre { get; set; } = null!;
        public int Estado { get; set; }
    }
}
