using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Sigesoft.Common;
using Sigesoft.Common.HelperDb;
using Sigesoft.Node.WinClient.BE;

namespace Sigesoft.Node.WinClient.BLL
{
    public class CalendarInterface
    {
        SqlHelper _sqlHelperDb = null;
        IDbConnection _connection = null;

        public CalendarInterface()
        {
            _sqlHelperDb = new SqlHelper();
            _connection = _sqlHelperDb.GetConnection();
           
        }

        public List<CalendarInterfaceLS> GetSystemUserAuthorization(int pSystemUserId)
        {
            List<CalendarInterfaceLS> oList = new List<CalendarInterfaceLS>();
            CommandType commandType = CommandType.StoredProcedure;
            string commandText = "[dbo].[01SIGESOFT_Agendado]";

            //IDataParameter[] commandParameters = new[] 
            //{              
            //    _sqlHelperDb.GetParameter("@i_SystemUserId", DbType.Int32, pSystemUserId),                             
            //};

            using (IDataReader drReader = _sqlHelperDb.ExecuteReader(_connection, commandType, commandText))
            {
                oList = new GenericList().CreateList<CalendarInterfaceLS>(drReader as SqlDataReader);
            }

            return oList;
        }
    }
}
