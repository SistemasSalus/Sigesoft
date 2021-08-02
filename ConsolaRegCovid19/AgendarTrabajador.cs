using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsolaRegCovid19
{
   public class AgendarTrabajador
    {
        private string _FechaServicio { get; set; }
        private string _Nombres { get; set; }
        private string _ApePaterno { get; set; }
        private string _ApeMaterno { get; set; }
        private string _TipoDocumento { get; set; }
        private string _NroDocumento { get; set; }
        private string _Genero { get; set; }
        private string _FechaNacimiento { get; set; }
        private string _Email { get; set; }
        private string _Celulares { get; set; }
        private string _Puesto { get; set; }
        private string _Direccion { get; set; }
        private string _NombreSede { get; set; }
        private int _TipoEmpresaCovidId { get; set; }

        private int _NodeId { get; set; }
        private string _Tecnico { get; set; }

        public AgendarTrabajador(
            string FechaServicio,
            string Nombres,
            string ApePaterno,
            string ApeMaterno,
            string TipoDocumento,
            string NroDocumento,
            string Genero,
            string FechaNacimiento,
            string Email,
            string Celulares,
            string Puesto,
            string Direccion,
            string NombreSede,
            int TipoEmpresaCovidId,
            int NodeId,
            string Tecnico
            )
        {

            _FechaServicio = FechaServicio;
            _Nombres = Nombres;
            _ApePaterno = ApePaterno;
            _ApeMaterno = ApeMaterno;
            _TipoDocumento = TipoDocumento;
            _NroDocumento = NroDocumento;
            _Genero = Genero;
            _FechaNacimiento = FechaNacimiento;
            _Email = Email;
            _Celulares = Celulares;
            _Puesto = Puesto;
            _Direccion = Direccion;
            _NombreSede = NombreSede;
            _TipoEmpresaCovidId = TipoEmpresaCovidId;
            _NodeId = NodeId;
            _Tecnico = Tecnico;
        }

        public string Agendar()
        {
            OperationResult objOperationResult = new OperationResult();
            CalendarBL _objCalendarBL = new CalendarBL();
            PacientBL objPacientBL = new PacientBL();
            calendarDto objCalendarDto = new calendarDto();

            var PersonId = "";
            var ProtocolId = "";
            var Sede = "";
            var OrganizationId = "";

            #region GlobalSession
            //var sessione = (SessionModel)Session[Resources.Constante.SessionUsuario];
            var username = _Tecnico;
            var GlobalSession = new List<string>() { _NodeId.ToString(), "", "11", "", "", username };
            #endregion

            #region Obtener ProtocoloId
            ProtocolId = Constants.EMPRESA_BACKUS_PROTOCOLO_PRUEBA_RAPIDA_COVID_ID;
            OrganizationId = Constants.EMPRESA_BACKUS_ID;
            Sede = _NombreSede;
           
            #endregion

            #region Trabajador
            personDto objPersonDto = new personDto();
            objPersonDto = objPacientBL.GetPersonByNroDocument(ref objOperationResult, _NroDocumento);
            if (objPersonDto != null)
            {
                objPersonDto.v_FirstName = _Nombres;
                objPersonDto.v_FirstLastName = _ApePaterno;
                objPersonDto.v_SecondLastName = _ApeMaterno;
                objPersonDto.i_DocTypeId = int.Parse(_TipoDocumento.ToString());
                objPersonDto.v_DocNumber = _NroDocumento;
                objPersonDto.i_SexTypeId = int.Parse(_Genero.ToString());
                objPersonDto.d_Birthdate = DateTime.Parse(_FechaNacimiento.ToString());
                objPersonDto.v_Mail = _Email;
                objPersonDto.v_TelephoneNumber = _Celulares;
                objPersonDto.i_LevelOfId = -1;
                objPersonDto.i_MaritalStatusId = -1;
                objPersonDto.v_CurrentOccupation = _Puesto;
                objPersonDto.v_AdressLocation = _Direccion;

                objPacientBL.UpdatePacient(ref objOperationResult, objPersonDto, GlobalSession, objPersonDto.v_DocNumber, objPersonDto.v_DocNumber);
                PersonId = objPersonDto.v_PersonId;
            }
            else
            {
                objPersonDto = new personDto();
                objPersonDto.v_FirstName = _Nombres;
                objPersonDto.v_FirstLastName = _ApePaterno;
                objPersonDto.v_SecondLastName = _ApeMaterno;
                objPersonDto.i_DocTypeId = int.Parse(_TipoDocumento.ToString());
                objPersonDto.v_DocNumber = _NroDocumento;
                objPersonDto.i_SexTypeId = int.Parse(_Genero.ToString());
                objPersonDto.d_Birthdate = DateTime.Parse(_FechaNacimiento.ToString());
                objPersonDto.v_Mail = _Email;
                objPersonDto.v_TelephoneNumber = _Celulares;
                objPersonDto.i_LevelOfId = -1;
                objPersonDto.i_MaritalStatusId = -1;
                objPersonDto.v_CurrentOccupation = _Puesto;
                objPersonDto.v_AdressLocation = _Direccion;

                PersonId = objPacientBL.AddPacient(ref objOperationResult, objPersonDto, GlobalSession);
            }
            #endregion

            #region EntidadCalendar

            DateTime dateTime;
            if (!DateTime.TryParse(_FechaServicio, out dateTime))
                return "Error de fecha";

            objCalendarDto.d_CircuitStartDate = dateTime;
            objCalendarDto.d_EntryTimeCM = dateTime;
            objCalendarDto.d_DateTimeCalendar = dateTime;

            objCalendarDto.v_PersonId = PersonId;

            objCalendarDto.i_ServiceTypeId = (int)Sigesoft.Common.ServiceType.Empresarial;
            objCalendarDto.i_CalendarStatusId = (int)Sigesoft.Common.CalendarStatus.Atendido;
            objCalendarDto.i_ServiceId = (int)Sigesoft.Common.MasterService.Eso;
            objCalendarDto.v_ProtocolId = ProtocolId;
            objCalendarDto.i_NewContinuationId = (int)Sigesoft.Common.modality.NuevoServicio;
            objCalendarDto.i_LineStatusId = (int)Sigesoft.Common.LineStatus.EnCircuito;
            objCalendarDto.i_IsVipId = (int)Sigesoft.Common.SiNo.NO;
            #endregion

           var calendarId = _objCalendarBL.AddSheduleWEB(ref objOperationResult, objCalendarDto, GlobalSession, ProtocolId, PersonId, objCalendarDto.i_ServiceId.Value, "Nuevo", OrganizationId, Sede, dateTime, _TipoEmpresaCovidId);

           return calendarId;

        }

    }
}
