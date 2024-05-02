using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Carrito
    {
        public int IdCarrito { get; set; }
        public Cliente ObjCliente { get; set; }
        public Producto ObjProducto { get; set; }
        public int Cantidad { get; set; }
    }
}
