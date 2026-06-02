using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CapaEntidades
{
    public class Producto
    {
        public int CodigoProducto { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;
        [Required(ErrorMessage = "La descripcion es obligatoria")]
        public string Descripcion { get; set; } = null!;
        [Required(ErrorMessage = "El precio es obligatorio")]
        public decimal Precio { get; set; }
        [Required(ErrorMessage = "La talla es obligatoria")]
        public int Talla { get; set; }
        [Required(ErrorMessage = "La cantidad es obligatoria")]
        public int CodCategoria { get; set; }
        [Required(ErrorMessage = "El estado es obligatorio")]
        public int Estado { get; set; }
        [Required(ErrorMessage = "La marca es obligatoria")]
        public string Marca { get; set; } = null!;
       
    }
}
