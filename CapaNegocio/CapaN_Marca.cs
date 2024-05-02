using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CapaN_Marca
    {
        private CapaD_Marca objCapaDato = new CapaD_Marca();

        public List<Marca> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Marca marca, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(marca.Descripcion) || string.IsNullOrWhiteSpace(marca.Descripcion))
            {
                Mensaje = "La descripcion de la marca no puede estar vacia";

            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Registrar(marca, out Mensaje);
            }
            else
            {
                return 0;
            }

        }

        public bool Editar(Marca marca, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(marca.Descripcion) || string.IsNullOrWhiteSpace(marca.Descripcion))
            {
                Mensaje = "La descripcion de la marca no puede estar vacia";

            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Editar(marca, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }
        public List<Marca> ListarMarcaPorCategoria(int idCategoria)
        {
            return objCapaDato.ListarMarcaPorCategoria(idCategoria);
        }
    }
}
