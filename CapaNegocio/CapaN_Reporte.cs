using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CapaN_Reporte
    {
        private CapaD_Reporte objCapaDato = new CapaD_Reporte();

        public List<Reporte> Ventas(string fechaInicio, string fechaFin, string idTransaccion)
        {
            return objCapaDato.Ventas(fechaInicio, fechaFin, idTransaccion);
        }

        public CuadroDeMando VerCuadroDeMando()
        {
            return objCapaDato.VerCuadroDeMando();
        }
    }
}
