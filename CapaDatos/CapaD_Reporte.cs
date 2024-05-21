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
    public class CapaD_Reporte
    {
        public List<Reporte> Ventas(string fechaInicio,string fechaFin,string idTransaccion)
        {
            List<Reporte> lista = new List<Reporte>();
            try
            {
                using (SqlConnection objconexion = new SqlConnection(Conexion.conex))
                { 
                    SqlCommand comando = new SqlCommand("SP_REPORTE_VENTA", objconexion);
                   
                    comando.Parameters.AddWithValue("FechaInicio", fechaInicio);
                    comando.Parameters.AddWithValue("FechaFin", fechaFin);
                    comando.Parameters.AddWithValue("IdTransaccion", idTransaccion);
                    comando.CommandType = CommandType.StoredProcedure;

                    objconexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(
                                new Reporte()
                                {
                                    FechaVenta = reader["FechaVenta"].ToString(),
                                    Cliente = reader["Cliente"].ToString(),
                                    Producto = reader["Producto"].ToString(),
                                    Precio = Convert.ToDecimal(reader["Precio"], new CultureInfo("es-AR")),
                                    Cantidad = Convert.ToInt32(reader["Cantidad"].ToString()),
                                    Total = Convert.ToDecimal(reader["Total"], new CultureInfo("es-AR")),
                                    IdTransaccion = reader["IdTransaccion"].ToString()
                                }                                
                            );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Reporte>();
            }
            return lista;
        }
        public CuadroDeMando VerCuadroDeMando()
        {
            CuadroDeMando objeto = new CuadroDeMando();
            try
            {
                using (SqlConnection objconexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("SP_REPORTE_DASHBOARD", objconexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    objconexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            objeto = new CuadroDeMando()
                            {
                                TotalCliente = Convert.ToInt32(reader["TotalCliente"]),
                                TotalVenta = Convert.ToInt32(reader["TotalVenta"]),
                                TotalProducto = Convert.ToInt32(reader["TotalProducto"])
                            };
                        }
                    }
                }
            }
            catch
            {
                objeto = new CuadroDeMando();
            }

            return objeto;
        }
    }
}
