using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
namespace CapaDatos
{
    public class CapaD_Ubicacion
    {
        public List<Provincia> ObtenerProvincia()
        {
            List<Provincia> lista = new List<Provincia>();
            try
            {
                using (SqlConnection objconexion = new SqlConnection(Conexion.conex))
                {
                    string query = "select * from Provincia";
                    SqlCommand comando = new SqlCommand(query, objconexion);
                    comando.CommandType = CommandType.Text;

                    objconexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(
                                new Provincia()
                                {
                                    IdProvincia = reader["IdProvincia"].ToString(),
                                    Descripcion = reader["Descripcion"].ToString()
                                });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Provincia>();
            }
            return lista;
        }
        public List<Ciudad> ObtenerCiudad(string idprovincia)
        {
            List<Ciudad> lista = new List<Ciudad>();
            try
            {
                using (SqlConnection objconexion = new SqlConnection(Conexion.conex))
                {
                    string query = "select * from CIUDAD where IdProvincia = @idprovincia";
                    SqlCommand comando = new SqlCommand(query, objconexion);
                    comando.Parameters.AddWithValue("@idprovincia", idprovincia);
                    comando.CommandType = CommandType.Text;

                    objconexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(
                                new Ciudad()
                                {
                                    IdCiudad = reader["IdCiudad"].ToString(),
                                    Descripcion = reader["Descripcion"].ToString()
                                });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Ciudad>();
            }
            return lista;
        }
    }
}
