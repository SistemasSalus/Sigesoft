using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Sigesoft.Common.HelperDb
{
    public sealed class SqlHelper: AbstractAdoHelper
    {
        /// <summary>
        /// Obtiene una nueva instancia de la clase SqlConnection, dada una cadena que contiene la cadena de conexión.
        /// </summary>
        /// <returns></returns>
        public override IDbConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings[Constants.CONEXION_NAME].ConnectionString);
        }

        /// <summary>
        /// Retorna una nueva instancia de la clase SqlParameter. 
        /// </summary>
        /// <param name="parameterName">Nombre del parámetro que se va a asignar.</param>
        /// <param name="dbType">Uno de los valores de DbType.</param>
        /// <returns></returns>
        public override IDataParameter GetParameter(string parameterName, DbType dbType)
        {
            SqlParameter parameter = new SqlParameter();
            parameter.ParameterName = parameterName;
            parameter.DbType = dbType;
            return parameter;
        }

        /// <summary>
        /// Retorna una nueva instancia de la clase SqlParameter. 
        /// </summary>
        /// <param name="parameterName">Nombre del parámetro que se va a asignar.</param>
        /// <param name="dbType">Uno de los valores de DbType.</param>
        /// <param name="value">Object que es el valor de SqlParameter.</param>
        /// <returns></returns>
        public override IDataParameter GetParameter(string parameterName, DbType dbType, object value)
        {
            SqlParameter parameter = new SqlParameter();
            parameter.ParameterName = parameterName;
            parameter.DbType = dbType;
            parameter.Value = value ?? DBNull.Value;
            return parameter;
        }

        /// <summary>
        /// Retorna una nueva instancia de la clase SqlParameter.
        /// </summary>
        /// <param name="parameterName">Nombre del parámetro que se va a asignar.</param>
        /// <param name="dbType">Uno de los valores de DbType.</param>
        /// <param name="direction">Uno de los valores de ParameterDirection.</param>
        /// <returns></returns>
        public override IDataParameter GetParameter(string parameterName, DbType dbType, ParameterDirection direction)
        {
            SqlParameter parameter = new SqlParameter();
            parameter.ParameterName = parameterName;
            parameter.DbType = dbType;
            parameter.Direction = direction;
            return parameter;
        }
    }
}
