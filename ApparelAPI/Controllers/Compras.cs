using ApparelAPI.Controllers_Productos;
using Capa_Datos;
using CapaEntidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApparelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Compras : ControllerBase
    {
        public readonly CompraDatos _compras;
        public Compras(IConfiguration config)
        {
            _compras = new CompraDatos(config.GetConnectionString("DefaultConnection"));
        }

        [HttpPost("InsertarCompra")]
        public IActionResult InsertarCompra([FromBody] CapaEntidades.Compras compra)
        {
            if (compra == null) return BadRequest("Compra invalida");

            int idCompra = _compras.CrearCompra(compra); //se obtiene el id de la compra insertada
            if (idCompra <= 0)
            {
                return StatusCode(500, "Error al insertar la compra");
            }

            return Ok(new { IdCompra = idCompra, Mensaje = "Compra insertada correctamente" });
        }

        [HttpPost("InsertarDetalleCompra")]
        public IActionResult InsertarDetalleCompra([FromBody] detalleCompra detalledecompra, [FromQuery] int idCompra)
        {
            if (detalledecompra == null) return BadRequest("Detalle de compra invalido");

            
            bool result = _compras.insertardetalledecompra(idCompra, detalledecompra);
            return result ? Ok("Detalle de compra insertado correctamente") : StatusCode(500, "Error al insertar");
        }

        [HttpGet("ListarCompras")]
        public IActionResult ListarCompras()
        {
            var listado = _compras.ListarCompras();
            return Ok(listado);
        }
    }
}

