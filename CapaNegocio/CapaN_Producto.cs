using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CapaN_Producto
    {
        private CapaD_Producto objCapaDato = new CapaD_Producto();

        public List<Producto> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Producto producto, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(producto.Nombre) || string.IsNullOrWhiteSpace(producto.Nombre))
            {
                Mensaje = "El nombre del producto no puede estar vacio..";
            }
            else if (string.IsNullOrEmpty(producto.Descripcion) || string.IsNullOrWhiteSpace(producto.Descripcion))
            {
                Mensaje = "La descripcion del producto no puede estar vacia..";
            }
            else if(producto.ObjMarca.IdMarca == 0)
            {
                Mensaje = "Debe seleccionar una Marca..";
            }
            else if(producto.ObjCategoria.IdCategoria == 0)
            {
                Mensaje = "Debe seleccionar una Categoria..";
            }
            else if(producto.Precio == 0)
            {
                Mensaje = "Debe ingresar el Precio del producto";
            }
            else if(producto.Stock == 0)
            {
                Mensaje = "Debe ingresar el Stock del producto";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Registrar(producto, out Mensaje);
            }
            else
            {
                return 0;
            }    
        }

        public bool Editar(Producto producto, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(producto.Nombre) || string.IsNullOrWhiteSpace(producto.Nombre))
            {
                Mensaje = "El nombre del producto no puede estar vacio";
            }
            if (string.IsNullOrEmpty(producto.Descripcion) || string.IsNullOrWhiteSpace(producto.Descripcion))
            {
                Mensaje = "La descripcion del producto no puede estar vacia";
            }
            if(producto.ObjMarca.IdMarca == 0)
            {
                Mensaje = "Debe seleccionar una MARCA..";
            }
            if(producto.ObjCategoria.IdCategoria == 0)
            {
                Mensaje = "Debe seleccionar una CATEGORIA";
            }
            if(producto.Precio == 0)
            {
                Mensaje = "Debe ingresar el precio del producto";
            }
            if(producto.Stock == 0)
            {
                Mensaje = "Debe ingresar el Stock del producto";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Editar(producto, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool GuardarDatosImagen(Producto producto, out string Mensaje)
        {
            return objCapaDato.GuardarDatosImagen(producto, out Mensaje);
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }
    }
}
