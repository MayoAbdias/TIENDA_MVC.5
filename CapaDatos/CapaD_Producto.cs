using System;
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
    public class CapaD_Producto
    {
        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();
            try
            {
                using (SqlConnection objconexion = new SqlConnection(Conexion.conex))
                {
                    //string query = "Select P.IdProducto,P.Nombre,P.Descripcion,M.IdMarca,M.Descripcion[MarcaDes],C.IdCategoria,C.Descripcion[CategoriaDes],P.Precio,P.Stock,P.RutaImagen,P.NombreImagen,P.Activo from PRODUCTO P inner join MARCA M on M.IdMarca = P.IdMarca inner join CATEGORIA C on C.IdCategoria = P.IdCategoria";
                   // stringBuilder me permite hacer saltos de linea
                    StringBuilder sb = new StringBuilder();


                    sb.AppendLine("Select P.IdProducto,P.Nombre,P.Descripcion,");
                    sb.AppendLine("M.IdMarca,M.Descripcion[MarcaDes],");
                    sb.AppendLine("C.IdCategoria,C.Descripcion[CategoriaDes],");
                    sb.AppendLine("P.Precio,P.Stock,P.RutaImagen,P.NombreImagen,P.Activo");
                    sb.AppendLine("from PRODUCTO P");
                    sb.AppendLine("inner join MARCA M on M.IdMarca = P.IdMarca");
                    sb.AppendLine("inner join CATEGORIA C on C.IdCategoria = P.IdCategoria");


                    SqlCommand comando = new SqlCommand(sb.ToString(), objconexion);
                    comando.CommandType = CommandType.Text;

                    objconexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Producto()
                            {
                                IdProducto = Convert.ToInt32(reader["IdProducto"]),
                                Nombre = reader["Nombre"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),                                
                                ObjMarca = new Marca() {IdMarca = Convert.ToInt32(reader["IdMarca"]),Descripcion = reader["MarcaDes"].ToString() },
                                ObjCategoria = new Categoria() {IdCategoria = Convert.ToInt32(reader["IdCategoria"]),Descripcion = reader["CategoriaDes"].ToString() },
                                Precio = Convert.ToDecimal(reader["Precio"], new CultureInfo("es-AR")),
                                Stock = Convert.ToInt32(reader["Stock"]),
                                RutaImagen = reader["RutaImagen"].ToString(),
                                NombreImagen = reader["NombreImagen"].ToString(),
                                Activo = Convert.ToBoolean(reader["Activo"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Producto>();
            }
            return lista;
        }

        public int Registrar(Producto producto, out string Mensaje)
        {
            int IdAutogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("SP_RegistrarProducto", objConexion);
                    comando.Parameters.AddWithValue("Nombre", producto.Nombre);
                    comando.Parameters.AddWithValue("Descripcion", producto.Descripcion);
                    comando.Parameters.AddWithValue("IdMarca", producto.ObjMarca.IdMarca);
                    comando.Parameters.AddWithValue("IdCategoria", producto.ObjCategoria.IdCategoria);
                    comando.Parameters.AddWithValue("Precio", producto.Precio);
                    comando.Parameters.AddWithValue("Stock", producto.Stock);
                    comando.Parameters.AddWithValue("Activo", producto.Activo);
                    comando.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    comando.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    comando.CommandType = CommandType.StoredProcedure;

                    objConexion.Open();
                    comando.ExecuteNonQuery();


                    IdAutogenerado = Convert.ToInt32(comando.Parameters["Resultado"].Value);
                    Mensaje = comando.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                IdAutogenerado = 0;
                Mensaje = ex.Message;

            }
            return IdAutogenerado;
        }

        public bool Editar(Producto producto, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("SP_EditarProducto", objConexion);
                    comando.Parameters.AddWithValue("IdProducto", producto.IdProducto);
                    comando.Parameters.AddWithValue("Nombre", producto.Nombre);
                    comando.Parameters.AddWithValue("Descripcion", producto.Descripcion);
                    comando.Parameters.AddWithValue("IdMarca", producto.ObjMarca.IdMarca);
                    comando.Parameters.AddWithValue("IdCategoria", producto.ObjCategoria.IdCategoria);
                    comando.Parameters.AddWithValue("Precio", producto.Precio);
                    comando.Parameters.AddWithValue("Stock", producto.Stock);
                    comando.Parameters.AddWithValue("Activo", producto.Activo);
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

        public bool GuardarDatosImagen(Producto producto,out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;


            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    string query = "Update PRODUCTO set RutaImagen = @RutaImagen,NombreImagen = @NombreImagen where IdProducto = @IdProducto";
                    SqlCommand comando = new SqlCommand(query, objConexion);
          
                    comando.Parameters.AddWithValue("@RutaImagen", producto.RutaImagen);
                    comando.Parameters.AddWithValue("@NombreImagen", producto.NombreImagen);
                    comando.Parameters.AddWithValue("@IdProducto", producto.IdProducto);
                    comando.CommandType = CommandType.Text;

                    objConexion.Open();

                    if( comando.ExecuteNonQuery() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        Mensaje = "No se pudo actualizar imagen";
                    }

                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;

            }
            return resultado;
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("SP_EliminarProducto", objConexion);
                    comando.Parameters.AddWithValue("IdProducto", id);
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
    }
}
