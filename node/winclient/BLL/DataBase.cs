using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace Sigesoft.Node.WinClient.BLL
{
    internal class Database : IDisposable
    {
        DbConnection connection;
        DbProviderFactory factory;
        DbCommand command;
        String _pstrDBConn;

        public Database()
        {
            factory = DbProviderFactories.GetFactory(DataBaseHelper.GetDbProvider());
        }

        public void Open(string pstrDBConn)
        {
            _pstrDBConn = pstrDBConn;
            connection = factory.CreateConnection();
            connection.ConnectionString = DataBaseHelper.GetDbConnectionString(_pstrDBConn);
            try
            {
                connection.Open();
            }
            catch
            {
                throw;
            }
        }

        public void Close()
        {
            connection.Close();
        }

        public string CommandText
        {
            set
            {
                if (command == null)
                {
                    command = factory.CreateCommand();

                }
                if (connection == null)
                {
                    this.Open(_pstrDBConn);
                }
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = value;
            }
        }

        public string ProcedureName
        {
            set
            {
                if (command == null)
                {
                    command = factory.CreateCommand();
                }
                if (connection == null)
                {
                    this.Open(_pstrDBConn);
                }
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = value;
            }
        }

        public void AddParameter(string parameterName, DbType parameterType, ParameterDirection parameterDirection, Object parameterValue)
        {
            if (command != null)
            {
                DbParameter parameter = factory.CreateParameter();
                parameter.ParameterName = parameterName;
                parameter.DbType = parameterType;
                parameter.Direction = parameterDirection;
                parameter.Value = parameterValue;
                parameter.SourceColumnNullMapping = true;
                command.Parameters.Add(parameter);
            }
        }

        public IDataReader GetDataReader()
        {
            command.CommandTimeout = 1200;
            return command.ExecuteReader(CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection);
        }

        public int Execute()
        {
            return command.ExecuteNonQuery();
        }

        public object ExecuteScalar()
        {
            return command.ExecuteScalar();
        }

        ~Database()
        {
            this.Dispose();
        }

        #region IDisposable Members
        public void Dispose()
        {
            connection = null;
            command = null;
            factory = null;
            GC.SuppressFinalize(this);
            GC.Collect();
        }
        #endregion
    }
}
