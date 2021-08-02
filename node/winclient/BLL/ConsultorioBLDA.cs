using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Sigesoft.Node.WinClient.BE.Custom;


namespace Sigesoft.Node.WinClient.BLL
{
    public class ConsultorioBLDA
    {
        #region Construir Entidades

        private PacienteAgendado CrearPacienteAgendado(IDataReader DReader)
        {
            PacienteAgendado xpacienteagendadoBE = new PacienteAgendado();
            int Indice;

            Indice = DReader.GetOrdinal("i_NroTickets");
            if (!DReader.IsDBNull(Indice)) { xpacienteagendadoBE.i_NroTickets = DReader.GetInt32(Indice); }

            Indice = DReader.GetOrdinal("d_CircuitStartDate");
            if (!DReader.IsDBNull(Indice)) { xpacienteagendadoBE.d_CircuitStartDate = DReader.GetDateTime(Indice); }

            Indice = DReader.GetOrdinal("Paciente");
            xpacienteagendadoBE.Paciente = DReader.IsDBNull(Indice) ? String.Empty : DReader.GetString(Indice);

            Indice = DReader.GetOrdinal("NroDocum");
            xpacienteagendadoBE.NroDocum = DReader.IsDBNull(Indice) ? String.Empty : DReader.GetString(Indice);

            Indice = DReader.GetOrdinal("Edad");
            if (!DReader.IsDBNull(Indice)) { xpacienteagendadoBE.Edad = DReader.GetInt32(Indice); }

            Indice = DReader.GetOrdinal("Empleador");
            xpacienteagendadoBE.Empleador = DReader.IsDBNull(Indice) ? String.Empty : DReader.GetString(Indice);

            Indice = DReader.GetOrdinal("Cliente");
            xpacienteagendadoBE.Cliente = DReader.IsDBNull(Indice) ? String.Empty : DReader.GetString(Indice);

            Indice = DReader.GetOrdinal("Protocolo");
            xpacienteagendadoBE.Protocolo = DReader.IsDBNull(Indice) ? String.Empty : DReader.GetString(Indice);

            Indice = DReader.GetOrdinal("v_Value1");
            xpacienteagendadoBE.v_Value1 = DReader.IsDBNull(Indice) ? String.Empty : DReader.GetString(Indice);

            Indice = DReader.GetOrdinal("v_CurrentOccupation");
            xpacienteagendadoBE.v_CurrentOccupation = DReader.IsDBNull(Indice) ? String.Empty : DReader.GetString(Indice);

            Indice = DReader.GetOrdinal("i_IsVipId");
            if (!DReader.IsDBNull(Indice)) { xpacienteagendadoBE.i_IsVipId = DReader.GetInt32(Indice); }

            Indice = DReader.GetOrdinal("v_ServiceId");
            xpacienteagendadoBE.v_ServiceId = DReader.IsDBNull(Indice) ? String.Empty : DReader.GetString(Indice);

            Indice = DReader.GetOrdinal("v_PersonId");
            xpacienteagendadoBE.v_PersonId = DReader.IsDBNull(Indice) ? String.Empty : DReader.GetString(Indice);

            return xpacienteagendadoBE;
        }

        private EstadoComponentes CrearEstadoComponentes(IDataReader DReader)
        {
            EstadoComponentes xestadocomponentesBE = new EstadoComponentes();
            int Indice;

            Indice = DReader.GetOrdinal("v_Name");
            xestadocomponentesBE.v_Name = DReader.IsDBNull(Indice) ? String.Empty : DReader.GetString(Indice);

            Indice = DReader.GetOrdinal("Llamado");
            xestadocomponentesBE.Llamado = DReader.IsDBNull(Indice) ? String.Empty : DReader.GetString(Indice);

            Indice = DReader.GetOrdinal("Estado");
            xestadocomponentesBE.Estado = DReader.IsDBNull(Indice) ? String.Empty : DReader.GetString(Indice);

            return xestadocomponentesBE;
        }


        #endregion

        #region Ejecutar Stores

        public List<PacienteAgendado> GetPacienteAgendado(string pstrConn, int pcategoria, string pnodo)
        {
            List<PacienteAgendado> xpacienteagendados = new List<PacienteAgendado>();
            IDataReader DReader = null;

            try
            {
                string pDBConn = pstrConn;
                using (Database db = new Database())
                {
                    db.Open(pDBConn);
                    db.ProcedureName = "[dbo].[ObtenerAgendaporCategoriaNODO]";
                    db.AddParameter("@categoria", DbType.Int32, ParameterDirection.Input, pcategoria);
                    db.AddParameter("@nodeid", DbType.String, ParameterDirection.Input, pnodo);

                    DReader = db.GetDataReader();
                }
                while (DReader.Read())
                {
                    PacienteAgendado xpacienteagendado = CrearPacienteAgendado(DReader);
                    xpacienteagendados.Add(xpacienteagendado);
                }
                DReader.Close();
            }
            catch
            {
                if (DReader != null && !DReader.IsClosed) { DReader.Close(); }
                throw;
            }
            return xpacienteagendados;
        }

        public List<EstadoComponentes> GetEstadoComponentes(string pstrConn, string pserviceid)
        {
            List<EstadoComponentes> xestadocomponentess = new List<EstadoComponentes>();
            IDataReader DReader = null;

            try
            {
                string pDBConn = pstrConn;
                using (Database db = new Database())
                {
                    db.Open(pDBConn);
                    db.ProcedureName = "[dbo].[ObtenerEstadoxComponentes]";
                    db.AddParameter("@serviceid", DbType.String, ParameterDirection.Input, pserviceid);

                    DReader = db.GetDataReader();
                }
                while (DReader.Read())
                {
                    EstadoComponentes xestadocomponentes = CrearEstadoComponentes(DReader);
                    xestadocomponentess.Add(xestadocomponentes);
                }
                DReader.Close();
            }
            catch
            {
                if (DReader != null && !DReader.IsClosed) { DReader.Close(); }
                throw;
            }
            return xestadocomponentess;
        }


        #endregion

    }
}
