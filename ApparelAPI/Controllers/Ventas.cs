using CapaNegocio;
using CapaEntidades;
using Microsoft.AspNetCore.Mvc;

namespace ApparelAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly VentaBLL _bll;

        public VentasController(VentaBLL bll)
        {
            _bll = bll;
        }

        [HttpPost("RegistrarVenta")]
        public IActionResult RegistrarVenta([FromBody] Ventas venta)
        {
            try
            {
                var id = _bll.RegistrarVenta(venta);
                return Ok(new { success = true, codVenta = id });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    detalle = ex.InnerException?.Message
                });
            }
        }

        // GET api/Ventas/ListarVentas
        [HttpGet("ListarVentas")]
        public IActionResult ListarVentas()
        {
            try
            {
                var ventas = _bll.ListarVentas();
                return Ok(new { success = true, data = ventas });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    detalle = ex.InnerException?.Message
                });
            }
        }
    }
}