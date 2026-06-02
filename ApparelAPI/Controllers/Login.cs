// Controlador de autenticación con JWT

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CapaDatos;
using CapaEntidades;
using CapaNegocio;


[ApiController]
[Route("/api/login/IniciarSesion")]
public class Login : ControllerBase
{
    private readonly UsuarioNegocio _usuarioNegocio;
    private readonly IConfiguration _config;

    public Login(IConfiguration config)
    {
        _config = config;

        _usuarioNegocio = new UsuarioNegocio(config.GetConnectionString("DefaultConnection"));
    }


    [HttpPost("IniciarSesion")]
    public IActionResult IniciarSesion([FromBody] CapaEntidades.LoginRequest request)
    {
        if (string.Equals(request.UsuarioLogin, "string", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(request.Contraseña, "string", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Datos de Login no proporcionados, por favor llene todos los campos");

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (request == null || string.IsNullOrWhiteSpace(request.UsuarioLogin) || string.IsNullOrWhiteSpace(request.Contraseña))
            return BadRequest("UsuarioLogin y contraseña son requeridos.");

        var usuario = _usuarioNegocio.Login(request.UsuarioLogin, request.Contraseña);
        if (usuario == null) return Unauthorized(new { error = "Credenciales inválidas." });

        var token = GenerarToken(usuario);
        return Ok(new { token });
    }

    private string GenerarToken(Usuarios usuario)
    {
        if (usuario == null)
            throw new ArgumentNullException(nameof(usuario));

        // Validar que los campos no sean nulos
        string nombreUsuario = usuario.Nombre ?? "Usuario";
        string rolUsuario = usuario.RolNombre ?? "Usuario";

        // Crear claims
        var claims = new[]
        {
        new Claim(ClaimTypes.Name, nombreUsuario),
        new Claim(ClaimTypes.Role, rolUsuario)
    };

        // Obtener clave secreta desde configuración
        var keyValue = _config["Jwt:Key"];
        if (string.IsNullOrEmpty(keyValue))
            throw new InvalidOperationException("JWT Key no configurada");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Crear token
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"] ?? "Edwin",
            audience: _config["Jwt:Audience"] ?? "EdwinUsuarios",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),  // Usar UTC
            signingCredentials: creds
        );

        // Retornar token en formato string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
