using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Capa_Datos;
using CapaEntidades;
using Microsoft.AspNetCore.Authorization;

namespace ApparelAPI.Controllers_Productos
{
    [Route("api/[controller]")]
    [ApiController]
    public class Productos : ControllerBase
    {
        public readonly ProductoDatos _Producto;
        public Productos(IConfiguration config)
        {
            _Producto = new ProductoDatos(config.GetConnectionString("DefaultConnection"));
        }


        [HttpPost("InsertarProducto")]
        [Authorize (Roles = "Admin")]

        public IActionResult Post([FromBody] Producto producto)
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid("No estás autorizado para usar esta opción");
            }

           if (string.Equals(producto.Nombre, "string", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(producto.Descripcion, "string", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(producto.Marca, "string", StringComparison.OrdinalIgnoreCase) ||
               producto.Precio <= 0 ||
               producto.Talla <= 0 ||
               producto.CodCategoria <= 0 ||
               producto.Estado <= 0)

                return BadRequest("Por favor llenar todos los campos");

            if (producto == null) return BadRequest("Producto invalido");

            bool result = _Producto.CrearProducto(producto);
            return result ? Ok("Producto insertado correctamente") : StatusCode(500, "Error al insertar");
        }

        [HttpPut("ActualizarProducto/{CodigoProducto}")]
        public IActionResult Put([FromBody] Producto producto)
        {
            if (string.Equals(producto.Nombre, "string", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(producto.Descripcion, "string", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(producto.Marca, "string", StringComparison.OrdinalIgnoreCase) ||
               producto.Precio <= 0 ||
               producto.Talla <= 0 ||
               producto.CodCategoria <= 0 ||
               producto.Estado <= 0)
                return BadRequest("Por favor llenar todos los campos");

            if (producto == null) return BadRequest("Producto invalido");
            bool result = _Producto.ActualizarProducto(producto);
            return result ? Ok("Producto actualizado correctamente") : StatusCode(500, "Error al actualizar");
        }

        [HttpGet("ObtenerProductos/{CodigoProducto}")]
        public IActionResult Get(int CodigoProducto )
        {
            if (CodigoProducto <= 0) return BadRequest("Codigo invalido");
            var productos = _Producto.ObtenerProductos(CodigoProducto);
            return productos != null ? Ok(productos) : StatusCode(500, "Error al obtener productos");
        }

        [HttpGet("ObtenerStockActual/{CodigoProducto}")]

        public IActionResult GetStock(int CodigoProducto)

            {
            if (CodigoProducto <= 0) return BadRequest("Codigo invalido");
            var stock = _Producto.Stock(CodigoProducto);
            return stock != null ? Ok(stock) : StatusCode(500, "Error al obtener stock");
        }


        [HttpDelete("EliminarProducto/{CodigoProducto}")]
        public IActionResult Delete(int CodigoProducto)
        {
            if (CodigoProducto <= 0) return BadRequest("Codigo invalido");
            bool result = _Producto.EliminarProducto(CodigoProducto);
            return result ? Ok("Producto eliminado correctamente") : StatusCode(500, "Error al eliminar");
        }
    }
}

