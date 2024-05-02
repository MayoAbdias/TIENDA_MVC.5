using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CapaDatos
{
    public class CapaD_Usuarios
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();
            try
            {
                using (SqlConnection objconexion = new SqlConnection(Conexion.conex))
                {
                    string query = "Select IdUsuario,Nombres, Apellidos, Correo,Clave,Reestablecer,Activo from USUARIO";
                    SqlCommand comando = new SqlCommand(query, objconexion);
                    comando.CommandType = CommandType.Text;

                    objconexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(
                                new Usuario()
                                {
                                    IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                                    Nombres = reader["Nombres"].ToString(),
                                    Apellidos = reader["Apellidos"].ToString(),
                                    Correo = reader["Correo"].ToString(),
                                    Clave = reader["Clave"].ToString(),
                                    Reestablecer = Convert.ToBoolean(reader["Reestablecer"]),
                                    Activo = Convert.ToBoolean(reader["Activo"])
                                }

                                );
                        }
                    }
                }
            }
            catch 
            {
                lista = new List<Usuario>();
            }

            return lista;
        }
        //Creo un metodo que devuelve un entero(int).Recibe un objeto del tipo Usuario y estoy declarando un 
        //parametro de salida de tipo string que diga mensaje.
        public int Registrar(Usuario user,out string Mensaje)
        {
            int IdAutogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using(SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("SP_RegistrarUsuario", objConexion);
                    comando.Parameters.AddWithValue("Nombres", user.Nombres);
                    comando.Parameters.AddWithValue("Apellidos", user.Apellidos);
                    comando.Parameters.AddWithValue("Correo",user.Correo);
                    comando.Parameters.AddWithValue("Clave", user.Clave);
                    comando.Parameters.AddWithValue("Activo", user.Activo);
                    comando.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    comando.Parameters.Add("Mensaje", SqlDbType.VarChar,500).Direction = ParameterDirection.Output;
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

        public bool Editar(Usuario user, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("SP_EditarUsuario", objConexion);
                    comando.Parameters.AddWithValue("IdUsuario", user.IdUsuario);
                    comando.Parameters.AddWithValue("Nombres", user.Nombres);
                    comando.Parameters.AddWithValue("Apellidos", user.Apellidos);
                    comando.Parameters.AddWithValue("Correo", user.Correo);
                    comando.Parameters.AddWithValue("Activo", user.Activo);
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

        public bool Eliminar(int id,out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("delete top (1) from usuario where IdUsuario = @id", objConexion);
                    comando.Parameters.AddWithValue("@id", id);
                    comando.CommandType = CommandType.Text;

                    objConexion.Open();
                    resultado = comando.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
                
            }
            return resultado;
           
        }

        public bool CambiarClave(int idUsuario,string nuevaClave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("update USUARIO set Clave = @nuevaClave, Reestablecer = 0 where IdUsuario = @Id", objConexion);
                    comando.Parameters.AddWithValue("@id", idUsuario);
                    comando.Parameters.AddWithValue("@nuevaClave", nuevaClave);
                    comando.CommandType = CommandType.Text;

                    objConexion.Open();
                    resultado = comando.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;

            }
            return resultado;

        }
        public bool ReestablecerClave(int idUsuario,string clave, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection objConexion = new SqlConnection(Conexion.conex))
                {
                    SqlCommand comando = new SqlCommand("update USUARIO set Clave = @clave, Reestablecer = 1 where IdUsuario = @Id", objConexion);
                    comando.Parameters.AddWithValue("@id", idUsuario);
                    comando.Parameters.AddWithValue("@clave", clave);
                    comando.CommandType = CommandType.Text;

                    objConexion.Open();
                    resultado = comando.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;

            }
            return resultado;

        }
    }
}
