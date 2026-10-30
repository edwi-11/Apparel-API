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
        [Authorize(Roles = "Admin")]
        public IActionResult Post([FromBody] Producto producto)
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid("No estás autorizado para usar esta opción");
            }

            if (producto == null)
                return BadRequest("Producto inválido");

            if (string.IsNullOrWhiteSpace(producto.Nombre) ||
                string.Equals(producto.Nombre, "string", StringComparison.OrdinalIgnoreCase) ||

                string.IsNullOrWhiteSpace(producto.Descripcion) ||
                string.Equals(producto.Descripcion, "string", StringComparison.OrdinalIgnoreCase) ||

                string.IsNullOrWhiteSpace(producto.Marca) ||
                string.Equals(producto.Marca, "string", StringComparison.OrdinalIgnoreCase) ||

                producto.Precio <= 0 ||
                producto.Estado <= 0 ||

                producto.Stock <= 0)
            {
                return BadRequest("Por favor llena todos los campos correctamente.");
            }

            bool result = _Producto.CrearProducto(producto);

            return result
                ? Ok("Producto insertado correctamente")
                : StatusCode(500, "Error al insertar el producto.");
        }

        [HttpPut("ActualizarProducto/{CodigoProducto}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Put([FromBody] Producto producto)
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid("No estás autorizado para usar esta opción");
            }

            if (producto == null)
                return BadRequest("Producto inválido");

            if (string.IsNullOrWhiteSpace(producto.Nombre) ||
                string.Equals(producto.Nombre, "string", StringComparison.OrdinalIgnoreCase) ||

                string.IsNullOrWhiteSpace(producto.Descripcion) ||
                string.Equals(producto.Descripcion, "string", StringComparison.OrdinalIgnoreCase) ||

                string.IsNullOrWhiteSpace(producto.Marca) ||
                string.Equals(producto.Marca, "string", StringComparison.OrdinalIgnoreCase) ||

                producto.Talla <= 0 ||
                producto.Precio <= 0 ||
                producto.Estado <= 0)
            {
                return BadRequest("Por favor llena todos los campos correctamente.");
            }

            bool result = _Producto.ActualizarProducto(producto);

            return result
                ? Ok("Producto actualizado correctamente")
                : StatusCode(500, "Error al actualizar el producto.");
        }

        [HttpGet("ObtenerProductos/{CodigoProducto}")]
        public IActionResult Get(int CodigoProducto )
        {
            if (CodigoProducto <= 0) return BadRequest("Codigo invalido");
            var productos = _Producto.ObtenerProductos(CodigoProducto);
            return productos != null ? Ok(productos) : StatusCode(500, "Error al obtener productos");
        }

        [HttpGet("ObtenerTodosProductos")]
        public IActionResult GetTodos()
        {
            var productos = _Producto.ObtenerTodosProductos();
            return productos != null && productos.Count > 0
                ? Ok(productos)
                : StatusCode(404, "No hay productos registrados");
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

