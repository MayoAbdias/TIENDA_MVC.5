using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CapaN_Venta
    {
        private CapaD_Venta objCapaDato = new CapaD_Venta();

        public bool Registrar(Venta objventa, DataTable detalleVenta, out string Mensaje)
        {
            return objCapaDato.Registrar(objventa, detalleVenta, out Mensaje);
        }
    }
}
