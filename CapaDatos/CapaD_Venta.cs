using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using System.Data;
using System.Data.SqlClient;

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
    }
}
