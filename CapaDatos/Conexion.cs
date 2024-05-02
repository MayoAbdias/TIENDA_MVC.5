using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.NetworkInformation;

namespace CapaDatos
{
    public class Conexion
    {
        public static string conex = ConfigurationManager.ConnectionStrings["cadena"].ToString();
    }
}
