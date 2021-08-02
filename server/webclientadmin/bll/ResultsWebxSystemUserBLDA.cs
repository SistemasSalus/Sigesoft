using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Sigesoft.Server.WebClientAdmin.BE.Custom;

namespace Sigesoft.Server.WebClientAdmin.BLL
{
    public class ResultsWebxSystemUserBLDA
    {
        #region Construir Entidades

        private ResultsWebxSystemUser CrearResultsWebxSystemUser(IDataReader DReader) 
        {
            ResultsWebxSystemUser xresultswebxSystemUserBE = new ResultsWebxSystemUser();
            int Indice;

            Indice = DReader.GetOrdinal("v_ServiceId");
            xresultswebxSystemUserBE.v_ServiceId = DReader.IsDBNull(Indice) ? String.Empty : DReader.GetString(Indice);

            Indice = DReader.GetOrdinal("v_PersonId");
            xresultswebxSystemUserBE.v_IdTrabajador = DReader.IsDBNull(Indice) ? String.Empty : DReader.GetString(Indice);

            Indice = DReader.GetOrdinal("Paciente");
            xresultswebxSystemUserBE.v_Trabajador = DReader.IsDBNull(Indice) ? String.Empty : DReader.GetString(Indice);

            Indice = DReader.GetOrdinal("Fecha");
            if (!DReader.IsDBNull(Indice)) { xresultswebxSystemUserBE.d_ServiceDate = DReader.GetDateTime(Indice); }

            Indice = DReader.GetOrdinal("v_Name");
            xresultswebxSystemUserBE.EmpresaCliente = DReader.IsDBNull(Indice) ? String.Empty : DReader.GetString(Indice);

            Indice = DReader.GetOrdinal("v_ProtocolId");
            xresultswebxSystemUserBE.v_ProtocolId = DReader.IsDBNull(Indice) ? String.Empty : DReader.GetString(Indice);

            Indice = DReader.GetOrdinal("Protocolo");
            xresultswebxSystemUserBE.Protocolo = DReader.IsDBNull(Indice) ? String.Empty : DReader.GetString(Indice);

            Indice = DReader.GetOrdinal("ESTADO");
            xresultswebxSystemUserBE.v_AptitudeStatusName = DReader.IsDBNull(Indice) ? String.Empty : DReader.GetString(Indice);

            Indice = DReader.GetOrdinal("Pendiente");
            xresultswebxSystemUserBE.Pendiente = DReader.IsDBNull(Indice) ? String.Empty : DReader.GetString(Indice);

            return xresultswebxSystemUserBE;
        }

        #endregion

        #region Ejecutar Stores

        // LUIS HAZLO ASÍ COMO LO MANEJA dAVID OK A VER 
        public List<ResultsWebxSystemUser> GetResultsWebxSystemUser(string pstrConn, DateTime pfini, DateTime pffin, Int32 psystemUserId, string ppaciente, string pempresa, string pprotocolo)
        {
            List<ResultsWebxSystemUser> xresultswebxSystemUsers = new List<ResultsWebxSystemUser>();
            IDataReader DReader = null;

            try
            {
                string pDBConn = pstrConn;
                using (Database db = new Database())
                {
                    db.Open(pDBConn);
                    db.ProcedureName = "[dbo].[ObtenerResultados WebxSystemUser2]";
                    db.AddParameter("@fecha_ini", DbType.Date, ParameterDirection.Input, pfini);
                    db.AddParameter("@fecha_fin", DbType.Date, ParameterDirection.Input, pffin);
                    db.AddParameter("@systemuserid", DbType.Int32, ParameterDirection.Input, psystemUserId);
                    db.AddParameter("@paciente", DbType.String, ParameterDirection.Input, ppaciente);
                    db.AddParameter("@empresa", DbType.String, ParameterDirection.Input, pempresa);
                    db.AddParameter("@protocolo", DbType.String, ParameterDirection.Input, pprotocolo);

                    DReader = db.GetDataReader();
                }
                while (DReader.Read())
                {
                    ResultsWebxSystemUser xresultswebxSystemUser = CrearResultsWebxSystemUser(DReader);
                    xresultswebxSystemUsers.Add(xresultswebxSystemUser);
                }
                DReader.Close();
            }
            catch(Exception ex)
            {
                if (DReader != null && !DReader.IsClosed) { DReader.Close(); }
                throw;
            }
            return xresultswebxSystemUsers;
        }

        #endregion
    }
}
