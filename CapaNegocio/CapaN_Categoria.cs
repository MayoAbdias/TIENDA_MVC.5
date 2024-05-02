using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CapaN_Categoria
    {
        private CapaD_Categoria objCapaDato = new CapaD_Categoria();

        public List<Categoria> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Categoria categoria, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(categoria.Descripcion) || string.IsNullOrWhiteSpace(categoria.Descripcion))
            {
                Mensaje = "La descripcion de la categoria no puede estar vacia";

            }           
            if (string.IsNullOrEmpty(Mensaje))
            {             
                return objCapaDato.Registrar(categoria, out Mensaje);              
            }
            else
            {
                return 0;
            }

        }

        public bool Editar(Categoria categoria, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(categoria.Descripcion) || string.IsNullOrWhiteSpace(categoria.Descripcion))
            {
                Mensaje = "La descripcion de la categoria no puede estar vacia";

            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Editar(categoria, out Mensaje);
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
    }
}
