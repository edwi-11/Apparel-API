using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class LoginRequest
    {
        public string UsuarioLogin { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
    }
}
