using CapaNegocio;
using CapaEntidades;
using Microsoft.AspNetCore.Mvc;

namespace ApparelAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteNegocio _clienteNegocio;

        public ClienteController(IConfiguration config)
        {
            _clienteNegocio = new ClienteNegocio(config.GetConnectionString("DefaultConnection")!);
        }

        // GET: api/Cliente
        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            var clientes = _clienteNegocio.ObtenerTodosClientes();
            return Ok(clientes);
        }

        // GET: api/Cliente/5
        [HttpGet("{id}")]
        public IActionResult ObtenerPorId(int id)
        {
            var clientes = _clienteNegocio.ObtenerClientePorId(id);
            if (clientes == null || clientes.Count == 0)
                return NotFound(new { mensaje = "Cliente no encontrado" });
            return Ok(clientes[0]);
        }

        // POST: api/Cliente
        [HttpPost]
        public IActionResult Crear([FromBody] Cliente cliente)
        {
            bool resultado = _clienteNegocio.CrearCliente(cliente);
            if (!resultado)
                return BadRequest(new { mensaje = "No se pudo crear el cliente" });
            return Ok(new { mensaje = "Cliente creado exitosamente" });
        }

        // PUT: api/Cliente/5
        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, [FromBody] Cliente cliente)
        {
            cliente.CodCliente = id;
            bool resultado = _clienteNegocio.ActualizarCliente(cliente);
            if (!resultado)
                return NotFound(new { mensaje = "Cliente no encontrado o no actualizado" });
            return Ok(new { mensaje = "Cliente actualizado exitosamente" });
        }

        // DELETE: api/Cliente/5
        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            bool resultado = _clienteNegocio.EliminarCliente(id);
            if (!resultado)
                return NotFound(new { mensaje = "Cliente no encontrado" });
            return Ok(new { mensaje = "Cliente eliminado exitosamente" });
        }
    }
}