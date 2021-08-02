using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;

namespace Sigesoft.Server.WebClientAdmin.BLL
{
    public static class DataBaseHelper
    {
        public static string GetDbProvider()
        {
            return ConfigurationManager.ConnectionStrings["SLProvider"].ProviderName;
        }

        public static string GetDbConnectionString(string pstrDBConn)
        {
            return ConfigurationManager.ConnectionStrings[pstrDBConn].ConnectionString;
        }
    }
}
