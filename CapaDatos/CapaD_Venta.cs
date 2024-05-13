using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace CapaDatos
{
    public class CapaD_Venta
    {
        public bool Registrar(Venta objventa,DataTable detalleVenta, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("E_SP_DetalleVenta", objConexion);
                    comando.Parameters.AddWithValue("IdCliente", objventa.IdCliente);
                    comando.Parameters.AddWithValue("TotalProducto", objventa.TotalProducto);
                    comando.Parameters.AddWithValue("MontoTotal", objventa.MontoTotal);
                    comando.Parameters.AddWithValue("Contacto", objventa.Contacto);
                    comando.Parameters.AddWithValue("IdCiudad", objventa.IdCiudad);
                    comando.Parameters.AddWithValue("Telefono", objventa.Telefono);
                    comando.Parameters.AddWithValue("Direccion", objventa.Direccion);
                    comando.Parameters.AddWithValue("IdTransaccion", objventa.IdTransaccion);
                    comando.Parameters.AddWithValue("DetalleVenta", detalleVenta);
                    comando.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    comando.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    comando.CommandType = CommandType.StoredProcedure;

                    objConexion.Open();
                    comando.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(comando.Parameters["Resultado"].Value);
                    Mensaje = comando.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;

            }
            return respuesta;
        }
        public List<DetalleVenta> ListarCompras(int idcliente)
        {
            List<DetalleVenta> lista = new List<DetalleVenta>();
            try
            {
                using (SqlConnection objconexion = new SqlConnection(Conexion.conex))
                {
                    string query = "select * from FN_ListarCompra(@idcliente)";


                    SqlCommand comando = new SqlCommand(query, objconexion);
                    comando.Parameters.AddWithValue("@idcliente", idcliente);
                    comando.CommandType = CommandType.Text;

                    objconexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new DetalleVenta()
                            {
                                ObjProducto = new Producto()
                                {
                                    Nombre = reader["Nombre"].ToString(),
                                    Precio = Convert.ToDecimal(reader["Precio"], new CultureInfo("es-AR")),
                                    RutaImagen = reader["RutaImagen"].ToString(),
                                    NombreImagen = reader["NombreImagen"].ToString(),
                                },
                                Cantidad = Convert.ToInt32(reader["Cantidad"]),
                                Total = Convert.ToDecimal(reader["Total"], new CultureInfo("es-AR")),
                                IdTransaccion = reader["IdTransaccion"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<DetalleVenta>();
            }
            return lista;
        }
    }
}
