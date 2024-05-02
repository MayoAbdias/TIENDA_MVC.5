using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CapaN_Ubicacion
    {
        private CapaD_Ubicacion ObjCapaDato = new CapaD_Ubicacion();

        public List<Provincia> ObtenerProvincia()
        {
            return ObjCapaDato.ObtenerProvincia();
        }
        public List<Ciudad> ObtenerCiudad(string idprovincia)
        {
            return ObjCapaDato.ObtenerCiudad(idprovincia);
        }
    }
}
