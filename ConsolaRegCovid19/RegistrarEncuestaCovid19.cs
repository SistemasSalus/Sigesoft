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
   public class RegistrarEncuestaCovid19
    {
        private int _NodeId { get; set; }
        private string _CalendarId { get; set; }
        private string _ServiceComponentId { get; set; }
        
        private string _TipoDomicilioId { get; set; }
        private string _Geolocalizacion { get; set; }
        private string _PersonalSaludId { get; set; }
        private string _ProfesionId { get; set; }
        private string _SintomasId { get; set; }
        private string _InicioSintomas { get; set; }

        private bool _TosId { get; set; }
        private bool _DolorGargantaId { get; set; }
        private bool _CongestionNasalId { get; set; }
        private bool _DificultadRespiratoriaId { get; set; }

        private bool _FiebreEscalofrioId { get; set; }
        private bool _MalestarGeneralId { get; set; }
        private bool _DiarreaId { get; set; }
        private bool _NauseasVomitosId { get; set; }

        private bool _CefaleaId { get; set; }
        private bool _IrritabilidadConfusionId { get; set; }
        private bool _DolorId { get; set; }
        private bool _ExpectoracionId { get; set; }

        private bool _MuscularId { get; set; }
        private bool _AbdominalId { get; set; }
        private bool _PechoId { get; set; }
        private bool _ArticulacionesId { get; set; }

        private string _OtrosSintomas { get; set; }

        private bool _DiabetesId { get; set; }
        private bool _PulmonarCronicaId { get; set; }
        private bool _CancerId { get; set; }
        private bool _HipertensionArterialId { get; set; }

        private bool _ObesidadId { get; set; }
        private bool _Mayor65Id { get; set; }
        private bool _InsuficienciaRenalId { get; set; }
        private bool _EmbarazoPuerperioId { get; set; }

        private bool _EnfCardioVascularId { get; set; }
        private bool _AsmaId { get; set; }
        private bool _InmunosupresorId { get; set; }
        private bool _RiesgoPersonalSaludId { get; set; }

        private bool _CertificacionId { get; set; }

        public RegistrarEncuestaCovid19(            
                int NodeId ,
                string CalendarId,
                string ServiceComponentId ,               

                string TipoDomicilioId ,
                string Geolocalizacion ,
                string PersonalSaludId ,
                string ProfesionId ,
                string SintomasId ,
                string InicioSintomas ,

                bool TosId ,
                bool DolorGargantaId ,
                bool CongestionNasalId ,
                bool DificultadRespiratoriaId ,

                bool FiebreEscalofrioId ,
                bool MalestarGeneralId ,
                bool DiarreaId ,
                bool NauseasVomitosId ,

                bool CefaleaId ,
                bool IrritabilidadConfusionId ,
                bool DolorId ,
                bool ExpectoracionId ,

                bool MuscularId ,
                bool AbdominalId ,
                bool PechoId ,
                bool ArticulacionesId ,

                string OtrosSintomas ,

                bool DiabetesId ,
                bool PulmonarCronicaId ,
                bool CancerId ,
                bool HipertensionArterialId ,

                bool ObesidadId ,
                bool Mayor65Id ,
                bool InsuficienciaRenalId ,
                bool EmbarazoPuerperioId ,

                bool EnfCardioVascularId ,
                bool AsmaId ,
                bool InmunosupresorId ,
                bool RiesgoPersonalSaludId ,

                bool CertificacionId
            )
        {
         _NodeId  = NodeId;
         _CalendarId = CalendarId;
         _ServiceComponentId  = ServiceComponentId;

         _TipoDomicilioId  = TipoDomicilioId;
         _Geolocalizacion  = Geolocalizacion;
         _PersonalSaludId  = PersonalSaludId;
         _ProfesionId  = ProfesionId;
         _SintomasId  = SintomasId;
         _InicioSintomas  = InicioSintomas;

         _TosId  = TosId;
         _DolorGargantaId  = DolorGargantaId;
         _CongestionNasalId  = CongestionNasalId;
         _DificultadRespiratoriaId  = DificultadRespiratoriaId;

         _FiebreEscalofrioId  = FiebreEscalofrioId;
         _MalestarGeneralId  = MalestarGeneralId;
         _DiarreaId  = DiarreaId;
         _NauseasVomitosId  = NauseasVomitosId;

         _CefaleaId  = CefaleaId;
         _IrritabilidadConfusionId  = IrritabilidadConfusionId;
         _DolorId  = DolorId;
         _ExpectoracionId  = ExpectoracionId;

         _MuscularId  = MuscularId ;
         _AbdominalId  = AbdominalId;
         _PechoId  = PechoId;
         _ArticulacionesId  = ArticulacionesId;

         _OtrosSintomas  = OtrosSintomas;

         _DiabetesId  = DiabetesId;
         _PulmonarCronicaId  = PulmonarCronicaId;
         _CancerId  = CancerId;
         _HipertensionArterialId  = HipertensionArterialId;

         _ObesidadId  = ObesidadId;
         _Mayor65Id  = Mayor65Id;
         _InsuficienciaRenalId  = InsuficienciaRenalId;
         _EmbarazoPuerperioId  = EmbarazoPuerperioId;

         _EnfCardioVascularId  = EnfCardioVascularId;
         _AsmaId  = AsmaId;
         _InmunosupresorId  = InmunosupresorId;
         _RiesgoPersonalSaludId  = RiesgoPersonalSaludId;

         _CertificacionId = CertificacionId;

        }

        public string GrabarEncuesta()
        {
            ServiceBL oServiceBL = new ServiceBL();
            OperationResult objOperationResult = new OperationResult();
            var serviceComponentFieldsList = new List<ServiceComponentFieldsList>();
            List<string> ClientSession = new List<string>();

            #region Set ServiceComponentId
            var datos = SetServiceComponentId(_CalendarId);
            _ServiceComponentId = datos.serviceComponentId;            
            var _PersonId = datos.PersonId;
            #endregion

            #region TipoDomicilioId
            var scfTipoDomicilioId = new ServiceComponentFieldsList();
            scfTipoDomicilioId.v_ComponentFieldsId = Constants.COVID_DOMICILIO_ID;
            scfTipoDomicilioId.v_ServiceComponentId = _ServiceComponentId;
            scfTipoDomicilioId.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _TipoDomicilioId } };
            serviceComponentFieldsList.Add(scfTipoDomicilioId);
            #endregion

            #region Geolocalizacion
            var scfGeolocalizacion = new ServiceComponentFieldsList();
            scfGeolocalizacion.v_ComponentFieldsId = Constants.COVID_GEOLOCALIZACION_ID;
            scfGeolocalizacion.v_ServiceComponentId = _ServiceComponentId;
            scfGeolocalizacion.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _Geolocalizacion } };
            serviceComponentFieldsList.Add(scfGeolocalizacion);
            #endregion

            #region PersonalSalud
            var scfPersonalSalud = new ServiceComponentFieldsList();
            scfPersonalSalud.v_ComponentFieldsId = Constants.COVID_ES_PERSONAL_SALUD_ID;
            scfPersonalSalud.v_ServiceComponentId = _ServiceComponentId;
            scfPersonalSalud.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _PersonalSaludId } };
            serviceComponentFieldsList.Add(scfPersonalSalud);
            #endregion

            #region Profesion
            var scfProfesion = new ServiceComponentFieldsList();
            scfProfesion.v_ComponentFieldsId = Constants.COVID_PROFESION_ID;
            scfProfesion.v_ServiceComponentId = _ServiceComponentId;
            scfProfesion.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _ProfesionId } };
            serviceComponentFieldsList.Add(scfProfesion);
            #endregion

            #region Sintomas
            var scfSintomas = new ServiceComponentFieldsList();
            scfSintomas.v_ComponentFieldsId = Constants.COVID_TIENE_SINTOMAS_ID;
            scfSintomas.v_ServiceComponentId = _ServiceComponentId;
            scfSintomas.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _SintomasId } };
            serviceComponentFieldsList.Add(scfSintomas);
            #endregion

            #region InicioSintomas
            var scfInicioSintomas = new ServiceComponentFieldsList();
            scfInicioSintomas.v_ComponentFieldsId = Constants.COVID_INICIO_SINTOMAS_ID;
            scfInicioSintomas.v_ServiceComponentId = _ServiceComponentId;
            scfInicioSintomas.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _InicioSintomas } };
            serviceComponentFieldsList.Add(scfInicioSintomas);
            #endregion

            #region TosId
            var scfTosId = new ServiceComponentFieldsList();
            scfTosId.v_ComponentFieldsId = Constants.COVID_TOS_ID;
            scfTosId.v_ServiceComponentId = _ServiceComponentId;
            scfTosId.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _TosId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfTosId);
            #endregion

            #region DolorGarganta
            var scfDolorGarganta = new ServiceComponentFieldsList();
            scfDolorGarganta.v_ComponentFieldsId = Constants.COVID_DOLOR_GARGANTA_ID;
            scfDolorGarganta.v_ServiceComponentId = _ServiceComponentId;
            scfDolorGarganta.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _DolorGargantaId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfDolorGarganta);
            #endregion

            #region CongestionNasal
            var scfCongestionNasal = new ServiceComponentFieldsList();
            scfCongestionNasal.v_ComponentFieldsId = Constants.COVID_CONGESTION_NASAL_ID;
            scfCongestionNasal.v_ServiceComponentId = _ServiceComponentId;
            scfCongestionNasal.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _CongestionNasalId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfCongestionNasal);
            #endregion

            #region DificultadRespiratoria
            var scfDificultadRespiratoria = new ServiceComponentFieldsList();
            scfDificultadRespiratoria.v_ComponentFieldsId = Constants.COVID_DIFIC_RESPIRA_ID;
            scfDificultadRespiratoria.v_ServiceComponentId = _ServiceComponentId;
            scfDificultadRespiratoria.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _DificultadRespiratoriaId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfDificultadRespiratoria);
            #endregion

            #region FiebreEscalofrio
            var scfFiebreEscalofrio = new ServiceComponentFieldsList();
            scfFiebreEscalofrio.v_ComponentFieldsId = Constants.COVID_FIEBRE_ESCALOFRIO_ID;
            scfFiebreEscalofrio.v_ServiceComponentId = _ServiceComponentId;
            scfFiebreEscalofrio.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _FiebreEscalofrioId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfFiebreEscalofrio);
            #endregion

            #region MalestarGeneral
            var scfMalestarGeneral = new ServiceComponentFieldsList();
            scfMalestarGeneral.v_ComponentFieldsId = Constants.COVID_MALESTAR_GENERAL_ID;
            scfMalestarGeneral.v_ServiceComponentId = _ServiceComponentId;
            scfMalestarGeneral.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _MalestarGeneralId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfMalestarGeneral);
            #endregion

            #region Diarrea
            var scfDiarrea = new ServiceComponentFieldsList();
            scfDiarrea.v_ComponentFieldsId = Constants.COVID_DIARREA_ID;
            scfDiarrea.v_ServiceComponentId = _ServiceComponentId;
            scfDiarrea.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _DiarreaId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfDiarrea);
            #endregion

            #region NauseasVomitos
            var scfNauseasVomitos = new ServiceComponentFieldsList();
            scfNauseasVomitos.v_ComponentFieldsId = Constants.COVID_NAUSEAS_ID;
            scfNauseasVomitos.v_ServiceComponentId = _ServiceComponentId;
            scfNauseasVomitos.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _NauseasVomitosId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfNauseasVomitos);
            #endregion

            #region Cefalea
            var scfCefalea = new ServiceComponentFieldsList();
            scfCefalea.v_ComponentFieldsId = Constants.COVID_CEFALEA_ID;
            scfCefalea.v_ServiceComponentId = _ServiceComponentId;
            scfCefalea.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _CefaleaId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfCefalea);
            #endregion

            #region IrritabilidadConfusion
            var scfIrritabilidadConfusion = new ServiceComponentFieldsList();
            scfIrritabilidadConfusion.v_ComponentFieldsId = Constants.COVID_IRRITABILIDAD_ID;
            scfIrritabilidadConfusion.v_ServiceComponentId = _ServiceComponentId;
            scfIrritabilidadConfusion.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _IrritabilidadConfusionId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfIrritabilidadConfusion);
            #endregion

            #region Dolor
            var scfDolor = new ServiceComponentFieldsList();
            scfDolor.v_ComponentFieldsId = Constants.COVID_DOLOR_ID;
            scfDolor.v_ServiceComponentId = _ServiceComponentId;
            scfDolor.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _DolorId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfDolor);
            #endregion

            #region Expectoracion
            var scfExpectoracion = new ServiceComponentFieldsList();
            scfExpectoracion.v_ComponentFieldsId = Constants.COVID_OTROS_ID;
            scfExpectoracion.v_ServiceComponentId = _ServiceComponentId;
            scfExpectoracion.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _ExpectoracionId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfExpectoracion);
            #endregion

            #region Muscular
            var scfMuscular = new ServiceComponentFieldsList();
            scfMuscular.v_ComponentFieldsId = Constants.COVID_MUSCULAR_ID;
            scfMuscular.v_ServiceComponentId = _ServiceComponentId;
            scfMuscular.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _MuscularId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfMuscular);
            #endregion

            #region Abdominal
            var scfAbdominal = new ServiceComponentFieldsList();
            scfAbdominal.v_ComponentFieldsId = Constants.COVID_ABDOMINAL_ID;
            scfAbdominal.v_ServiceComponentId = _ServiceComponentId;
            scfAbdominal.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _AbdominalId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfAbdominal);
            #endregion

            #region Pecho
            var scfPecho = new ServiceComponentFieldsList();
            scfPecho.v_ComponentFieldsId = Constants.COVID_PECHO_ID;
            scfPecho.v_ServiceComponentId = _ServiceComponentId;
            scfPecho.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _PechoId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfPecho);
            #endregion

            #region Articulaciones
            var scfArticulaciones = new ServiceComponentFieldsList();
            scfArticulaciones.v_ComponentFieldsId = Constants.COVID_ARTICULACIONES_ID;
            scfArticulaciones.v_ServiceComponentId = _ServiceComponentId;
            scfArticulaciones.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _ArticulacionesId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfArticulaciones);
            #endregion

            #region OtrosSintomas
            var scfOtrosSintomas = new ServiceComponentFieldsList();
            scfOtrosSintomas.v_ComponentFieldsId = Constants.COVID_OTROS_SINTOMAS_ID;
            scfOtrosSintomas.v_ServiceComponentId = _ServiceComponentId;
            scfOtrosSintomas.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _OtrosSintomas } };
            serviceComponentFieldsList.Add(scfOtrosSintomas);
            #endregion

            #region Diabetes
            var scfDiabetes = new ServiceComponentFieldsList();
            scfDiabetes.v_ComponentFieldsId = Constants.COVID_DIABETES_ID;
            scfDiabetes.v_ServiceComponentId = _ServiceComponentId;
            scfDiabetes.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _DiabetesId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfDiabetes);
            #endregion

            #region EnfPulmonarCronica
            var scfEnfPulmonarCronica = new ServiceComponentFieldsList();
            scfEnfPulmonarCronica.v_ComponentFieldsId = Constants.COVID_ENF_PULMONAR_ID;
            scfEnfPulmonarCronica.v_ServiceComponentId = _ServiceComponentId;
            scfEnfPulmonarCronica.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _PulmonarCronicaId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfEnfPulmonarCronica);
            #endregion

            #region Cáncer
            var scfCancer = new ServiceComponentFieldsList();
            scfCancer.v_ComponentFieldsId = Constants.COVID_CANCER_ID;
            scfCancer.v_ServiceComponentId = _ServiceComponentId;
            scfCancer.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _CancerId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfCancer);
            #endregion

            #region HipertensionArterial
            var scfHipertensionArterial = new ServiceComponentFieldsList();
            scfHipertensionArterial.v_ComponentFieldsId = Constants.COVID_HIPERTENCION_ARTERIAL_ID;
            scfHipertensionArterial.v_ServiceComponentId = _ServiceComponentId;
            scfHipertensionArterial.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _HipertensionArterialId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfHipertensionArterial);
            #endregion

            #region Obesidad
            var scfObesidad = new ServiceComponentFieldsList();
            scfObesidad.v_ComponentFieldsId = Constants.COVID_OBESIDAD_ID;
            scfObesidad.v_ServiceComponentId = _ServiceComponentId;
            scfObesidad.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _ObesidadId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfObesidad);
            #endregion

            #region Mayor65
            var scfMayor65 = new ServiceComponentFieldsList();
            scfMayor65.v_ComponentFieldsId = Constants.COVID_MAYOR_60_ID;
            scfMayor65.v_ServiceComponentId = _ServiceComponentId;
            scfMayor65.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _Mayor65Id == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfMayor65);
            #endregion

            #region InsuficienciaRenalCronica
            var scfInsuficienciaRenalCronica = new ServiceComponentFieldsList();
            scfInsuficienciaRenalCronica.v_ComponentFieldsId = Constants.COVID_INSUFICIENCIA_RENAL_ID;
            scfInsuficienciaRenalCronica.v_ServiceComponentId = _ServiceComponentId;
            scfInsuficienciaRenalCronica.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _InsuficienciaRenalId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfInsuficienciaRenalCronica);
            #endregion

            #region Embarazo
            var scfEmbarazo = new ServiceComponentFieldsList();
            scfEmbarazo.v_ComponentFieldsId = Constants.COVID_EMBARAZO_ID;
            scfEmbarazo.v_ServiceComponentId = _ServiceComponentId;
            scfEmbarazo.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _EmbarazoPuerperioId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfEmbarazo);
            #endregion

            #region EnfCardiovascular
            var scfEnfCardiovascular = new ServiceComponentFieldsList();
            scfEnfCardiovascular.v_ComponentFieldsId = Constants.COVID_ENF_CARDIO_ID;
            scfEnfCardiovascular.v_ServiceComponentId = _ServiceComponentId;
            scfEnfCardiovascular.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _EnfCardioVascularId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfEnfCardiovascular);
            #endregion

            #region Asma
            var scfAsma = new ServiceComponentFieldsList();
            scfAsma.v_ComponentFieldsId = Constants.COVID_ASMA_ID;
            scfAsma.v_ServiceComponentId = _ServiceComponentId;
            scfAsma.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _AsmaId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfAsma);
            #endregion

            #region Inmunosupresor
            var scfInmunosupresor = new ServiceComponentFieldsList();
            scfInmunosupresor.v_ComponentFieldsId = Constants.COVID_INMUNOSUPRESOR_ID;
            scfInmunosupresor.v_ServiceComponentId = _ServiceComponentId;
            scfInmunosupresor.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _InmunosupresorId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfInmunosupresor);
            #endregion

            #region RiesgoPersonalSalud
            var scfRiesgoPersonalSalud = new ServiceComponentFieldsList();
            scfRiesgoPersonalSalud.v_ComponentFieldsId = Constants.COVID_PERSONAL_SALUD_ID;
            scfRiesgoPersonalSalud.v_ServiceComponentId = _ServiceComponentId;
            scfRiesgoPersonalSalud.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = _RiesgoPersonalSaludId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfRiesgoPersonalSalud);
            #endregion

            #region GlobalSession
            var GlobalSession = new List<string>() { "6", "", "11" };
            #endregion

            oServiceBL.AddServiceComponentValues(ref objOperationResult, serviceComponentFieldsList, GlobalSession, _PersonId, _ServiceComponentId);

            return null;
        }

        private Datos SetServiceComponentId(string calendarId)
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var objEntity = (from c in dbContext.calendar
                             join a in dbContext.service on c.v_ServiceId equals a.v_ServiceId
                             join b in dbContext.servicecomponent on a.v_ServiceId equals b.v_ServiceId
                             where c.v_CalendarId == calendarId && c.i_IsDeleted == 0 && a.i_IsDeleted ==0 && c.i_IsDeleted ==0
                             select new Datos
                             {
                                 PersonId =  a.v_PersonId,
                                 serviceComponentId = b.v_ServiceComponentId
                             }).ToList();

            if (objEntity.Count > 1)
            {
                return null;
            }
            else if (objEntity.Count  == 0)
            {
                return null;
            }


            return objEntity[0];
        }

        class Datos {
            public string PersonId { get; set; }
            public string serviceComponentId { get; set; }
        
        }

    }
}
