using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;

namespace Sigesoft.Common.HelperDb
{
    public abstract class AbstractAdoHelper
    {
        public abstract IDbConnection GetConnection();                                                                  //Retorna un objeto connection
        public abstract IDataParameter GetParameter(string parameterName, DbType dbType);                               //Retorna un objeto parameter para el comando
        public abstract IDataParameter GetParameter(string parameterName, DbType dbType, object value);                 //Retorna un objeto parameter para el comando
        public abstract IDataParameter GetParameter(string parameterName, DbType dbType, ParameterDirection direction); //Retorna un objeto parameter para el comando
        object objSyn = new object();

        //Retorna un SqlCommand configurado y un flag si el metodo abrio la conexion
        private static IDbCommand PrepareCommand(IDbConnection connection, CommandType commandType, string commandText, IEnumerable<IDataParameter> commandParameters, out bool mustCloseConnection)
        {
            mustCloseConnection = false;

            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;

                if (commandParameters != null)
                {
                    foreach (var commandParameter in commandParameters)
                    {
                        cmd.Parameters.Add(commandParameter);
                    }
                }

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    mustCloseConnection = true;
                }
                return cmd;
            }
        }

        #region " ExecuteNonQuery "

        //Si se necesita ejecutar varios comandos, lo correcto es usar la misma conexion para la ejecucion
        //ExecuteNonQuery con parametros SQL y el objeto connection para ejecutar consultas dinamicas o llamadas a SP
        /// <summary>
        /// Ejecuta una instrucción SQL en el objeto Connection de un proveedor de datos de .NET Framework y devuelve el número de filas afectadas. 
        /// Valor devuelto : Número de filas afectadas.
        /// </summary>
        /// <param name="connection">Representa una conexión abierta/cerrada a un origen de datos.</param>
        /// <param name="commandType">Especifica cómo se interpreta una cadena de comando.</param>
        /// <param name="commandText">Especifica el comando de texto que se debe ejecutar en el origen de datos.</param>
        /// <param name="commandParameters">Especifica un arreglo de parámetros para un objeto Command.</param>
        /// <returns></returns>
        public int ExecuteNonQuery(IDbConnection connection, CommandType commandType, string commandText, IDataParameter[] commandParameters)
        {
            //Si la conexion viene abierta, no tengo responsabilidad de cerrarla en la ejecucion del comando 
            //Si la conexion viene cerrada, tengo responsabilidad de abrir y cerrar la conexion en la ejecucion del comando
            //Uso el try finally para el chequeo final

            lock (objSyn)
            {
                var mustCloseConnection = false;

                try
                {
                    using (var cmd = PrepareCommand(connection, commandType, commandText, commandParameters, out mustCloseConnection))
                    {
                        return cmd.ExecuteNonQuery();
                    }
                }
                finally
                {
                    if (mustCloseConnection) connection.Close();
                }
            }
        }

        /// <summary>
        /// Ejecuta una instrucción SQL en el objeto Connection de un proveedor de datos de .NET Framework y devuelve el número de filas afectadas. 
        /// Valor devuelto : Número de filas afectadas.
        /// </summary>
        /// <param name="connection">Representa una conexión abierta/cerrada a un origen de datos.</param>
        /// <param name="commandType">Especifica cómo se interpreta una cadena de comando.</param>
        /// <param name="commandText">Especifica el comando de texto que se debe ejecutar en el origen de datos.</param>
        /// <returns></returns>
        public int ExecuteNonQuery(IDbConnection connection, CommandType commandType, string commandText)
        {
            //No es necesario incluir using porque la conexión se abre y se cierra dentro del método que la consume
            return ExecuteNonQuery(connection, commandType, commandText, null);
        }


        //ExecuteNonQuery con parametros SQL para ejecutar consultas dinamicas o llamadas a SP
        /// <summary>
        /// Ejecuta una instrucción SQL en el objeto Connection de un proveedor de datos de .NET Framework y devuelve el número de filas afectadas. 
        /// Valor devuelto : Número de filas afectadas.
        /// </summary>
        /// <param name="commandType">Especifica cómo se interpreta una cadena de comando.</param>
        /// <param name="commandText">Especifica el comando de texto que se debe ejecutar en el origen de datos.</param>
        /// <param name="commandParameters">Especifica un arreglo de parámetros para un objeto Command.</param>
        /// <returns>Número de filas afectadas.</returns>
        public int ExecuteNonQuery(CommandType commandType, string commandText, IDataParameter[] commandParameters)
        {
            //No es necesario incluir using porque la conexión se abre y se cierra dentro del método que la consume
            return ExecuteNonQuery(this.GetConnection(), commandType, commandText, commandParameters);
        }

        /// <summary>
        /// Ejecuta una instrucción SQL en el objeto Connection de un proveedor de datos de .NET Framework y devuelve el número de filas afectadas.
        /// Valor devuelto : Número de filas afectadas.
        /// </summary>
        /// <param name="commandType">Especifica cómo se interpreta una cadena de comando.</param>
        /// <param name="commandText">Especifica el comando de texto que se debe ejecutar en el origen de datos.</param>
        /// <returns>Número de filas afectadas.</returns>
        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(this.GetConnection(), commandType, commandText, null);
        }

        #endregion

        #region " ExecuteReader "

        //ExecuteReader con parametros SQL y el objeto connection para ejecutar consultas dinamicas o llamadas a SP
        /// <summary>
        /// Ejecuta CommandText en Connection y genera un IDataReader.
        /// Valor devuelto : IDataReader.
        /// </summary>
        /// <param name="connection">Representa una conexión abierta/cerrada a un origen de datos.</param>
        /// <param name="commandType">Especifica cómo se interpreta una cadena de comando.</param>
        /// <param name="commandText">Especifica el comando de texto que se debe ejecutar en el origen de datos.</param>
        /// <param name="commandParameters">Especifica un arreglo de parámetros para un objeto Command.</param>
        /// <returns></returns>
        public IDataReader ExecuteReader(IDbConnection connection, CommandType commandType, string commandText, IDataParameter[] commandParameters)
        {
            //Necesita que la conexion este abierta
            //Si la conexion viene abierta, ejecuto el comando 
            //Si la conexion viene cerrada, abro la conexion y ejecuto el comando,pero como abri la conexion
            //tengo la responsabilidad de cerrar la conexion cuando cierra el DataReader u ocurre un error
            //Uso el try Cath para atrapar el error

            //lock (objSyn)
            //{
            var mustCloseConnection = false;

            try
            {
                var cmd = PrepareCommand(connection, commandType, commandText, commandParameters, out mustCloseConnection);
                return (mustCloseConnection) ? cmd.ExecuteReader(CommandBehavior.CloseConnection) : cmd.ExecuteReader();
            }
            catch
            {
                if (mustCloseConnection) connection.Close();
                throw;
            }
            //}
        }

        /// <summary>
        /// Ejecuta CommandText en Connection y genera un IDataReader.
        /// Valor devuelto : IDataReader.
        /// </summary>
        /// <param name="connection">Representa una conexión abierta/cerrada a un origen de datos.</param>
        /// <param name="commandType">Especifica cómo se interpreta una cadena de comando.</param>
        /// <param name="commandText">Especifica el comando de texto que se debe ejecutar en el origen de datos.</param>
        /// <returns></returns>
        public IDataReader ExecuteReader(IDbConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteReader(connection, commandType, commandText, null);
        }

        //ExecuteReader con parametros SQL para ejecutar consultas dinamicas o llamadas a SP
        /// <summary>
        /// Ejecuta CommandText y genera un IDataReader.
        /// Valor devuelto : IDataReader.
        /// </summary>
        /// <param name="commandType">Especifica cómo se interpreta una cadena de comando.</param>
        /// <param name="commandText">Especifica el comando de texto que se debe ejecutar en el origen de datos.</param>
        /// <param name="commandParameters">Especifica un arreglo de parámetros para un objeto Command.</param>
        /// <returns></returns>
        public IDataReader ExecuteReader(CommandType commandType, string commandText, IDataParameter[] commandParameters)
        {
            return ExecuteReader(this.GetConnection(), commandType, commandText, commandParameters);
        }

        //ExecuteReader para ejecutar consultas dinamicas o llamadas a SP
        /// <summary>
        /// Ejecuta CommandText y genera un IDataReader.
        /// Valor devuelto : IDataReader.
        /// </summary>
        /// <param name="commandType">Especifica cómo se interpreta una cadena de comando.</param>
        /// <param name="commandText">Especifica el comando de texto que se debe ejecutar en el origen de datos.</param>
        /// <returns></returns>
        public IDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            return ExecuteReader(this.GetConnection(), commandType, commandText, null);
        }

        #endregion

        #region " ExecuteScalar "

        //ExecuteScalar con parametros SQL y el objeto connection para ejecutar consultas dinamicas o llamadas a SP
        /// <summary>
        /// Ejecuta la consulta y devuelve la primera columna de la primera fila del conjunto de resultados que devuelve la consulta.
        /// Valor devuelto : Primera columna de la primera fila del conjunto de resultados.
        /// </summary>
        /// <typeparam name="T">Especifica el tipo de dato del valor devuelto.</typeparam>
        /// <param name="connection">Representa una conexión abierta/cerrada a un origen de datos.</param>
        /// <param name="commandType">Especifica cómo se interpreta una cadena de comando.</param>
        /// <param name="commandText">Especifica el comando de texto que se debe ejecutar en el origen de datos.</param>
        /// <param name="commandParameters">Especifica un arreglo de parámetros para un objeto Command.</param>
        /// <returns></returns>
        public T ExecuteScalar<T>(IDbConnection connection, CommandType commandType, string commandText, IDataParameter[] commandParameters)
        {
            //La conexion viene cerrada, tengo responsabilidad de abrir y cerrar la conexion en la ejecucion del comando
            var mustCloseConnection = false;

            try
            {
                IDbCommand cmd = PrepareCommand(connection, commandType, commandText, commandParameters, out mustCloseConnection);
                return (T)cmd.ExecuteScalar();
            }
            finally
            {
                if (mustCloseConnection) connection.Close();
            }
        }

        /// <summary>
        /// Ejecuta la consulta y devuelve la primera columna de la primera fila del conjunto de resultados que devuelve la consulta.
        /// Valor devuelto : Primera columna de la primera fila del conjunto de resultados.
        /// </summary>
        /// <typeparam name="T">Especifica el tipo de dato del valor devuelto.</typeparam>
        /// <param name="connection">Representa una conexión abierta/cerrada a un origen de datos.</param>
        /// <param name="commandType">Especifica cómo se interpreta una cadena de comando.</param>
        /// <param name="commandText">Especifica el comando de texto que se debe ejecutar en el origen de datos.</param>
        /// <returns></returns>
        public T ExecuteScalar<T>(IDbConnection connection, CommandType commandType, string commandText)
        {
            return (T)ExecuteScalar<T>(connection, commandType, commandText, null);
        }

        /// <summary>
        /// Ejecuta la consulta y devuelve la primera columna de la primera fila del conjunto de resultados que devuelve la consulta.
        /// Valor devuelto : Primera columna de la primera fila del conjunto de resultados.
        /// </summary>
        /// <typeparam name="T">Especifica el tipo de dato del valor devuelto.</typeparam>
        /// <param name="commandType">Especifica cómo se interpreta una cadena de comando.</param>
        /// <param name="commandText">Especifica el comando de texto que se debe ejecutar en el origen de datos.</param>
        /// <param name="commandParameters">Especifica un arreglo de parámetros para un objeto Command.</param>
        /// <returns></returns>
        public T ExecuteScalar<T>(CommandType commandType, string commandText, IDataParameter[] commandParameters)
        {
            return (T)ExecuteScalar<T>(this.GetConnection(), commandType, commandText, commandParameters);
        }

        /// <summary>
        /// Ejecuta la consulta y devuelve la primera columna de la primera fila del conjunto de resultados que devuelve la consulta.
        /// Valor devuelto : Primera columna de la primera fila del conjunto de resultados.
        /// </summary>
        /// <typeparam name="T">Especifica el tipo de dato del valor devuelto.</typeparam>
        /// <param name="commandType">Especifica cómo se interpreta una cadena de comando.</param>
        /// <param name="commandText">Especifica el comando de texto que se debe ejecutar en el origen de datos.</param>
        /// <returns></returns>
        public T ExecuteScalar<T>(CommandType commandType, string commandText)
        {
            return (T)ExecuteScalar<T>(this.GetConnection(), commandType, commandText, null);
        }

        #endregion

        #region " ExecuteDataTable "

        //Si se necesita ejecutar varios DataTable , lo correcto es un usar la misma conexion para la ejecucion
        //ExecuteDataTable con parametros SQL y el objeto connection para ejecutar consultas dinamicas o llamadas a SP

        /// <summary>
        /// Ejecuta CommandText en Connection y genera un DataTable.
        /// Valor devuelto : DataTable.
        /// </summary>
        /// <param name="connection">Representa una conexión abierta/cerrada a un origen de datos.</param>
        /// <param name="commandType">Especifica cómo se interpreta una cadena de comando.</param>
        /// <param name="commandText">Especifica el comando de texto que se debe ejecutar en el origen de datos.</param>
        /// <param name="commandParameters">Especifica un arreglo de parámetros para un objeto Command.</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(IDbConnection connection, CommandType commandType, string commandText, IDataParameter[] commandParameters)
        {
            //Si la conexion viene abierta, no tengo responsabilidad de cerrarla en la ejecucion del comando 
            //Si la conexion viene cerrada, tengo responsabilidad de abrir y cerrar la conexion en la ejecucion del comando
            //Uso el try finally para el chequeo final

            lock (objSyn)
            {
                var mustCloseConnection = false;

                try
                {
                    DataTable dtb = new DataTable();
                    IDbCommand cmd = PrepareCommand(connection, commandType, commandText, commandParameters, out mustCloseConnection);
                    using (var drd = cmd.ExecuteReader())
                    {
                        dtb.Load(drd);
                    }

                    return dtb;
                }
                finally
                {
                    if (mustCloseConnection) connection.Close();
                }
            }
        }

        /// <summary>
        /// Ejecuta CommandText en Connection y genera un DataTable.
        /// Valor devuelto : DataTable.
        /// </summary>
        /// <param name="connection">Representa una conexión abierta/cerrada a un origen de datos.</param>
        /// <param name="commandType">Especifica cómo se interpreta una cadena de comando.</param>
        /// <param name="commandText">Especifica el comando de texto que se debe ejecutar en el origen de datos.</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(IDbConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteDataTable(connection, commandType, commandText, null);
        }

        /// <summary>
        /// Ejecuta CommandText en Connection y genera un DataTable.
        /// Valor devuelto : DataTable.
        /// </summary>
        /// <param name="commandType">Especifica cómo se interpreta una cadena de comando.</param>
        /// <param name="commandText">Especifica el comando de texto que se debe ejecutar en el origen de datos.</param>
        /// <param name="commandParameters">Especifica un arreglo de parámetros para un objeto Command.</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(CommandType commandType, string commandText, IDataParameter[] commandParameters)
        {
            return ExecuteDataTable(this.GetConnection(), commandType, commandText, commandParameters);
        }

        /// <summary>
        /// Ejecuta CommandText en Connection y genera un DataTable.
        /// Valor devuelto : DataTable.
        /// </summary>
        /// <param name="commandType">Especifica cómo se interpreta una cadena de comando.</param>
        /// <param name="commandText">Especifica el comando de texto que se debe ejecutar en el origen de datos.</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(CommandType commandType, string commandText)
        {
            return ExecuteDataTable(this.GetConnection(), commandType, commandText, null);
        }

        #endregion
    }
}
