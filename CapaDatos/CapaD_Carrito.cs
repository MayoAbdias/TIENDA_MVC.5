﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace CapaDatos
{
    public class CapaD_Carrito
    {
        public bool ExisteCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("SP_ExisteCarrito", objConexion);
                    comando.Parameters.AddWithValue("IdCliente", idcliente);
                    comando.Parameters.AddWithValue("IdProducto", idproducto);
                    comando.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    comando.CommandType = CommandType.StoredProcedure;

                    objConexion.Open();
                    comando.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(comando.Parameters["Resultado"].Value);
                }
            }
            catch (Exception ex)
            {
                resultado = false;

            }
            return resultado;
        }
        public bool OperacionCarrito(int idcliente, int idproducto, bool sumar, out string Mensaje)
        {
            bool resultado = true;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("SP_OperacionCarrito", objConexion);
                    comando.Parameters.AddWithValue("IdCliente", idcliente);
                    comando.Parameters.AddWithValue("IdProducto", idproducto);
                    comando.Parameters.AddWithValue("Sumar", sumar);
                    comando.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    comando.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    comando.CommandType = CommandType.StoredProcedure;

                    objConexion.Open();
                    comando.ExecuteNonQuery();


                    resultado = Convert.ToBoolean(comando.Parameters["Resultado"].Value);
                    Mensaje = comando.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;

            }
            return resultado;
        }
        public int CantidadEnCarrito(int idcliente)
        {
            int resultado = 0;

            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("select count(*) from carrito where idcliente = @idcliente", objConexion);
                    comando.Parameters.AddWithValue("@idcliente", idcliente);
                    comando.CommandType = CommandType.Text;

                    objConexion.Open();
                    resultado = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
            }
            return resultado;
        }
        public List<Carrito> ListarProductos(int idcliente)
        {
            List<Carrito> lista = new List<Carrito>();
            try
            {
                using (SqlConnection objconexion = new SqlConnection(Conexion.conex))
                {
                    string query = "select * from FN_ObtenerCarritoCliente(@idcliente)";
                  

                    SqlCommand comando = new SqlCommand(query, objconexion);
                    comando.Parameters.AddWithValue("@idcliente", idcliente);
                    comando.CommandType = CommandType.Text;

                    objconexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Carrito()
                            {
                                ObjProducto = new Producto()
                                {
                                    IdProducto = Convert.ToInt32(reader["IdProducto"]),
                                    Nombre = reader["Nombre"].ToString(),
                                    ObjMarca = new Marca() { Descripcion = reader["DesMarca"].ToString() },
                                    Precio = Convert.ToDecimal(reader["Precio"], new CultureInfo("es-AR")),
                                    RutaImagen = reader["RutaImagen"].ToString(),
                                    NombreImagen = reader["NombreImagen"].ToString(),
                                },
                                Cantidad = Convert.ToInt32(reader["Cantidad"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Carrito>();
            }
            return lista;
        }
        public bool EliminarCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;
            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("SP_EliminarCarrito", objConexion);
                    comando.Parameters.AddWithValue("IdCliente", idcliente);
                    comando.Parameters.AddWithValue("IdProducto", idproducto);
                    comando.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    comando.CommandType = CommandType.StoredProcedure;

                    objConexion.Open();
                    comando.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(comando.Parameters["Resultado"].Value);
                }
            }
            catch (Exception ex)
            {
                resultado = false;

            }
            return resultado;
        }
    }
}
