using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;
using Sigesoft.Node.WinClient.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsolaRegCovid19
{
    public class RegistrarLaboratorioCovid19
    {
        private int _NodeId { get; set; }
        private string _CalendarId { get; set; }

        private string _FechaEjecucion { get; set; }
        private string _ProcedenciaSolicitudId { get; set; }
        private string _ResultadoPrimeraPruebaId { get; set; }
        private string _ResultadoSegundaPruebaId { get; set; }
        private string _ClasificacionClinicaId { get; set; }

        public RegistrarLaboratorioCovid19(
            
            int NodeId ,
            string CalendarId,
            string FechaEjecucion ,
            string ProcedenciaSolicitudId ,
            string ResultadoPrimeraPruebaId ,
            string ResultadoSegundaPruebaId ,
            string ClasificacionClinicaId             
            )
        {

            _NodeId = NodeId;
            _CalendarId = CalendarId;

            _FechaEjecucion = FechaEjecucion;
            _ProcedenciaSolicitudId = ProcedenciaSolicitudId;
            _ResultadoPrimeraPruebaId = ResultadoPrimeraPruebaId;
            _ResultadoSegundaPruebaId = ResultadoSegundaPruebaId;
            _ClasificacionClinicaId = ClasificacionClinicaId;
        }

        public bool GrabarLaboratorio()
        {
            Covid19BL oCovid19BL = new Covid19BL();
            ServiceBL oServiceBL = new ServiceBL();
            OperationResult objOperationResult = new OperationResult();
            var serviceComponentFieldsList = new List<ServiceComponentFieldsList>();
            List<string> ClientSession = new List<string>();

            #region Set ServiceComponentId
            var datos = SetServiceComponentId(_CalendarId);
            var _ServiceComponentId = datos.serviceComponentId;
            var _PersonId = datos.PersonId;
            #endregion

            var valoresEncuesta = oCovid19BL.EstadoEncuesta(new List<string> { _ServiceComponentId });

            var estadoEncuesta = valoresEncuesta.Find(p => p.ServiceComponentId == _ServiceComponentId) == null ? "pendiente" : "realizado";
            if (estadoEncuesta == "pendiente")
            {
                return false;
            }

            #region FechaEjecucion
            var scfFechaEjecucion = new ServiceComponentFieldsList();
            scfFechaEjecucion.v_ComponentFieldsId = Constants.COVID_FECHA_EJECUCION_ID;
            scfFechaEjecucion.v_ServiceComponentId = _ServiceComponentId;
            scfFechaEjecucion.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _FechaEjecucion } };
            serviceComponentFieldsList.Add(scfFechaEjecucion);
            #endregion

            #region ProcedenciaSolicitudId
            var scfProcedenciaSolicitudId = new ServiceComponentFieldsList();
            scfProcedenciaSolicitudId.v_ComponentFieldsId = Constants.COVID_PROCEDENCIA_SOLICITUD_ID;
            scfProcedenciaSolicitudId.v_ServiceComponentId = _ServiceComponentId;
            scfProcedenciaSolicitudId.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _ProcedenciaSolicitudId } };
            serviceComponentFieldsList.Add(scfProcedenciaSolicitudId);
            #endregion

            #region ResultadoPrimeraPruebaId
            var scfResultadoPrimeraPruebaId = new ServiceComponentFieldsList();
            scfResultadoPrimeraPruebaId.v_ComponentFieldsId = Constants.COVID_RES_1_PRUEBA_ID;
            scfResultadoPrimeraPruebaId.v_ServiceComponentId = _ServiceComponentId;
            scfResultadoPrimeraPruebaId.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _ResultadoPrimeraPruebaId } };
            serviceComponentFieldsList.Add(scfResultadoPrimeraPruebaId);
            #endregion

            #region ResultadoSegundaPruebaId
            var scfResultadoSegundaPruebaId = new ServiceComponentFieldsList();
            scfResultadoSegundaPruebaId.v_ComponentFieldsId = Constants.COVID_RES_2_PRUEBA_ID;
            scfResultadoSegundaPruebaId.v_ServiceComponentId = _ServiceComponentId;
            scfResultadoSegundaPruebaId.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _ResultadoSegundaPruebaId } };
            serviceComponentFieldsList.Add(scfResultadoSegundaPruebaId);
            #endregion

            #region ClasificacionClinica
            var scfClasificacionClinica = new ServiceComponentFieldsList();
            scfClasificacionClinica.v_ComponentFieldsId = Constants.COVID_CLASIFICACION_CLINICA_ID;
            scfClasificacionClinica.v_ServiceComponentId = _ServiceComponentId;
            scfClasificacionClinica.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _ClasificacionClinicaId } };
            serviceComponentFieldsList.Add(scfClasificacionClinica);
            #endregion

            #region GlobalSession
            var GlobalSession = new List<string>() { "6", "", "11" };
            #endregion

            return oServiceBL.AddServiceComponentValues(ref objOperationResult, serviceComponentFieldsList, GlobalSession, _PersonId, _ServiceComponentId);

        }

        private Datos SetServiceComponentId(string calendarId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var objEntity = (from c in dbContext.calendar
                             join a in dbContext.service on c.v_ServiceId equals a.v_ServiceId
                             join b in dbContext.servicecomponent on a.v_ServiceId equals b.v_ServiceId
                             where c.v_CalendarId == calendarId && c.i_IsDeleted == 0 && a.i_IsDeleted == 0 && c.i_IsDeleted == 0
                             select new Datos
                             {
                                 PersonId = a.v_PersonId,
                                 serviceComponentId = b.v_ServiceComponentId
                             }).ToList();

            if (objEntity.Count > 1)
            {
                return null;
            }
            else if (objEntity.Count == 0)
            {
                return null;
            }


            return objEntity[0];
        }

        class Datos
        {
            public string PersonId { get; set; }
            public string serviceComponentId { get; set; }

        }
    }
}
