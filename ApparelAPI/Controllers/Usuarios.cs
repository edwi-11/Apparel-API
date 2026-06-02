using CapaNegocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CapaEntidades;
using Capa_Datos;

namespace ApparelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Usuarios : ControllerBase
    {
        private readonly UsuarioNegocio _usuarioNegocio;
        public Usuarios(IConfiguration config)
        {
            _usuarioNegocio = new UsuarioNegocio(config.GetConnectionString("DefaultConnection"));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Agregar Usuario")]
        public IActionResult Post([FromBody] CapaEntidades.Usuarios usuario)
        {
            if (string.Equals(usuario.Nombre, "string", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(usuario.Apellidos, "string", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(usuario.UsuarioLogin, "string", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(usuario.Contraseña, "string", StringComparison.OrdinalIgnoreCase) ||
               usuario.CodRol <= 0 ||
               string.Equals(usuario.Cedula, "string", StringComparison.OrdinalIgnoreCase) ||
               usuario.Estado <= 0)
                return BadRequest("Por favor llenar todos los campos");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _usuarioNegocio.CrearUsuario(usuario);
                return Ok("Usuario creado exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ActualizarUsuario/{CodUsuario}")]
        public IActionResult Put(int CodUsuario, [FromBody] CapaEntidades.Usuarios usuario)
        {
            if (string.Equals(usuario.Nombre, "string", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(usuario.Apellidos, "string", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(usuario.UsuarioLogin, "string", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(usuario.Contraseña, "string", StringComparison.OrdinalIgnoreCase) ||
               usuario.CodRol <= 0 ||
               string.Equals(usuario.Cedula, "string", StringComparison.OrdinalIgnoreCase) ||
               usuario.Estado <= 0)
                return BadRequest("Por favor llenar todos los campos");

            if (usuario == null) return BadRequest("Error, datos invalidos");
            bool result = _usuarioNegocio.ActualizarUsuario(usuario);
            return result ? Ok("Usuario actualizado correctamente") : StatusCode(500, "Ocurrio un error al actualizar");
        }

        [AllowAnonymous]
        [HttpGet("VerUsuario/{CodUsuario}")]
        public IActionResult Get(int CodUsuario)
        {
            if (CodUsuario <= 0) return BadRequest("Codigo invalido");

            if (CodUsuario <= 0) return BadRequest("Codigo invalido");
            var usuarios = _usuarioNegocio.ObtenerUsuario(CodUsuario);
            return usuarios != null ? Ok(usuarios) : StatusCode(500, "Error al obtener usuarios");
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DarBajaUsuario/{CodUsuario}")]
        public IActionResult Delete(int CodUsuario)
        {
            if (CodUsuario <= 0) return BadRequest("Codigo invalido");
            bool result = _usuarioNegocio.EliminarUsuario(CodUsuario);
            return result ? Ok("Usuario eliminado correctamente") : StatusCode(500, "Error al eliminar");
        }

        
    }
}
