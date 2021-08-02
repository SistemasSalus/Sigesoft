using MvcSigesoft.ViewModels.Session;
using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcSigesoft.Controllers
{
    public class Covid19RetornoController : Controller
    {

        public ActionResult Index(string personId, string serviceComponentId, string serviceId, string componentId, string organizationId)
        {
            OperationResult objOperationResult = new OperationResult();

            ViewBag.PERSONID = personId;
            ViewBag.SERVICECOMPONENTID = serviceComponentId;
            ViewBag.SERVICEID = serviceId;
            ViewBag.COMPONENTID = componentId;
            ViewBag.ORGANIZATIONID = organizationId;

            var listComponents = new List<string>();
            if (componentId == Constants.CERTIFICADO_DESCENSO_COVID_ID)
            {
                listComponents.Add(Constants.CERTIFICADO_DESCENSO_COVID_ID);
                listComponents.Add(Constants.FUNCIONES_VITALES_ID);
            }       

            var Datos = new ServiceBL().ValoresComponenteByListComponents(serviceId, listComponents);
            return View(Datos);
        }

        [HttpPost]
        public JsonResult SaveTriaje(entidadTriajeRetorno triaje)
        {
            ServiceBL oServiceBL = new ServiceBL();
            OperationResult objOperationResult = new OperationResult();
            var serviceComponentFieldsList = new List<ServiceComponentFieldsList>();
            List<string> ClientSession = new List<string>();

            #region Reemplazar serviceComponentId
            triaje.ServiceComponentId = new ServiceBL().obtenerServiceComponentIdByServiceAndComponentId(triaje.ServiceId, Constants.FUNCIONES_VITALES_ID);
            #endregion
          
            #region Temperatura
            var scfTemperatura = new ServiceComponentFieldsList();
            scfTemperatura.v_ComponentFieldsId = Constants.FUNCIONES_VITALES_TEMPERATURA_ID;
            scfTemperatura.v_ServiceComponentId = triaje.ServiceComponentId;
            scfTemperatura.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = triaje.Temperatura } };
            serviceComponentFieldsList.Add(scfTemperatura);
            #endregion

            #region FR
            var scfFc = new ServiceComponentFieldsList();
            scfFc.v_ComponentFieldsId = Constants.FUNCIONES_VITALES_FREC_RESPIRATORIA_ID;
            scfFc.v_ServiceComponentId = triaje.ServiceComponentId;
            scfFc.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = triaje.Fr } };
            serviceComponentFieldsList.Add(scfFc);
            #endregion

            #region SatO2
            var scfSatO2 = new ServiceComponentFieldsList();
            scfSatO2.v_ComponentFieldsId = Constants.FUNCIONES_VITALES_SAT_O2_ID;
            scfSatO2.v_ServiceComponentId = triaje.ServiceComponentId;
            scfSatO2.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = triaje.SatO2 } };
            serviceComponentFieldsList.Add(scfSatO2);
            #endregion

            #region GlobalSession
            var sessione = (SessionModel)Session[Resources.Constante.SessionUsuario];
            var GlobalSession = new List<string>() { sessione.NodeId.ToString(), "", sessione.SystemUserId.ToString() };
            #endregion

            oServiceBL.AddServiceComponentValues(ref objOperationResult, serviceComponentFieldsList, GlobalSession, triaje.PersonId, triaje.ServiceComponentId);

            return null;
        }

        [HttpPost]
        public JsonResult SaveEncuesta(entidadEncuestaRetorno encuesta)
        {
            ServiceBL oServiceBL = new ServiceBL();
            OperationResult objOperationResult = new OperationResult();
            var serviceComponentFieldsList = new List<ServiceComponentFieldsList>();
            List<string> ClientSession = new List<string>();

            #region EstablecimientoSaludId
            var scfEstablecimientoSaludId = new ServiceComponentFieldsList();
            scfEstablecimientoSaludId.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_ESTABLECIMIENTO_SALUD_ID;
            scfEstablecimientoSaludId.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfEstablecimientoSaludId.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.EstablecimientoSaludId } };
            serviceComponentFieldsList.Add(scfEstablecimientoSaludId);
            #endregion

            #region TomaMedicacionId
            var scfTomaMedicacionId = new ServiceComponentFieldsList();
            scfTomaMedicacionId.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_TOMA_MEDICACION_ID;
            scfTomaMedicacionId.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfTomaMedicacionId.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.TomaMedicacionId } };
            serviceComponentFieldsList.Add(scfTomaMedicacionId);
            #endregion

            #region TipoDomicilioId
            var scfTipoDomicilioId = new ServiceComponentFieldsList();
            scfTipoDomicilioId.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_DOMICILIO_ID;
            scfTipoDomicilioId.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfTipoDomicilioId.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.TipoDomicilioId } };
            serviceComponentFieldsList.Add(scfTipoDomicilioId);
            #endregion

            #region PersonalSalud
            var scfPersonalSalud = new ServiceComponentFieldsList();
            scfPersonalSalud.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_PREGUNTA_ES_PERSONAL_SALUD_ID;
            scfPersonalSalud.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfPersonalSalud.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.PreguntaPersonalSaludId } };
            serviceComponentFieldsList.Add(scfPersonalSalud);
            #endregion

            #region Profesion
            var scfProfesion = new ServiceComponentFieldsList();
            scfProfesion.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_PROFESION_ID;
            scfProfesion.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfProfesion.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.ProfesionId } };
            serviceComponentFieldsList.Add(scfProfesion);
            #endregion

            #region Sintomas
            var scfSintomas = new ServiceComponentFieldsList();
            scfSintomas.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_TIENE_SINTOMAS_ID;
            scfSintomas.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfSintomas.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.SintomasId } };
            serviceComponentFieldsList.Add(scfSintomas);
            #endregion

            #region InicioSintomas
            var scfInicioSintomas = new ServiceComponentFieldsList();
            scfInicioSintomas.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_INICIO_SINTOMAS_ID;
            scfInicioSintomas.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfInicioSintomas.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.InicioSintomas } };
            serviceComponentFieldsList.Add(scfInicioSintomas);
            #endregion

            #region TosId
            var scfTosId = new ServiceComponentFieldsList();
            scfTosId.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_TOS_ID;
            scfTosId.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfTosId.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.TosId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfTosId);
            #endregion

            #region DolorGarganta
            var scfDolorGarganta = new ServiceComponentFieldsList();
            scfDolorGarganta.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_DOLOR_GARGANTA_ID;
            scfDolorGarganta.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfDolorGarganta.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.DolorGargantaId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfDolorGarganta);
            #endregion

            #region CongestionNasal
            var scfCongestionNasal = new ServiceComponentFieldsList();
            scfCongestionNasal.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_CONGESTION_NASAL_ID;
            scfCongestionNasal.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfCongestionNasal.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.CongestionNasalId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfCongestionNasal);
            #endregion

            #region DificultadRespiratoria
            var scfDificultadRespiratoria = new ServiceComponentFieldsList();
            scfDificultadRespiratoria.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_DIFIC_RESPIRA_ID;
            scfDificultadRespiratoria.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfDificultadRespiratoria.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.DificultadRespiratoriaId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfDificultadRespiratoria);
            #endregion

            #region FiebreEscalofrio
            var scfFiebreEscalofrio = new ServiceComponentFieldsList();
            scfFiebreEscalofrio.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_FIEBRE_ESCALOFRIO_ID;
            scfFiebreEscalofrio.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfFiebreEscalofrio.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.FiebreEscalofrioId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfFiebreEscalofrio);
            #endregion

            #region MalestarGeneral
            var scfMalestarGeneral = new ServiceComponentFieldsList();
            scfMalestarGeneral.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_MALESTAR_GENERAL_ID;
            scfMalestarGeneral.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfMalestarGeneral.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.MalestarGeneralId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfMalestarGeneral);
            #endregion

            #region Diarrea
            var scfDiarrea = new ServiceComponentFieldsList();
            scfDiarrea.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_DIARREA_ID;
            scfDiarrea.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfDiarrea.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.DiarreaId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfDiarrea);
            #endregion

            #region NauseasVomitos
            var scfNauseasVomitos = new ServiceComponentFieldsList();
            scfNauseasVomitos.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_NAUSEAS_ID;
            scfNauseasVomitos.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfNauseasVomitos.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.NauseasVomitosId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfNauseasVomitos);
            #endregion

            #region Cefalea
            var scfCefalea = new ServiceComponentFieldsList();
            scfCefalea.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_CEFALEA_ID;
            scfCefalea.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfCefalea.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.CefaleaId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfCefalea);
            #endregion

            #region IrritabilidadConfusion
            var scfIrritabilidadConfusion = new ServiceComponentFieldsList();
            scfIrritabilidadConfusion.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_IRRITABILIDAD_ID;
            scfIrritabilidadConfusion.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfIrritabilidadConfusion.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.IrritabilidadConfusionId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfIrritabilidadConfusion);
            #endregion

            #region Dolor
            var scfDolor = new ServiceComponentFieldsList();
            scfDolor.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_DOLOR_ID;
            scfDolor.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfDolor.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.DolorId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfDolor);
            #endregion

            #region Expectoracion
            var scfExpectoracion = new ServiceComponentFieldsList();
            scfExpectoracion.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_EXPECTORACION_ID;
            scfExpectoracion.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfExpectoracion.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.ExpectoracionId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfExpectoracion);
            #endregion

            #region Muscular
            var scfMuscular = new ServiceComponentFieldsList();
            scfMuscular.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_MUSCULAR_ID;
            scfMuscular.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfMuscular.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.MuscularId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfMuscular);
            #endregion

            #region Abdominal
            var scfAbdominal = new ServiceComponentFieldsList();
            scfAbdominal.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_ABDOMINAL_ID;
            scfAbdominal.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfAbdominal.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.AbdominalId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfAbdominal);
            #endregion

            #region Pecho
            var scfPecho = new ServiceComponentFieldsList();
            scfPecho.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_PECHO_ID;
            scfPecho.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfPecho.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.PechoId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfPecho);
            #endregion

            #region Articulaciones
            var scfArticulaciones = new ServiceComponentFieldsList();
            scfArticulaciones.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_ARTICULACIONES_ID;
            scfArticulaciones.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfArticulaciones.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.ArticulacionesId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfArticulaciones);
            #endregion

            #region OtrosSintomas
            var scfOtrosSintomas = new ServiceComponentFieldsList();
            scfOtrosSintomas.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_OTROS_SINTOMAS_ID;
            scfOtrosSintomas.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfOtrosSintomas.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.OtrosSintomas } };
            serviceComponentFieldsList.Add(scfOtrosSintomas);
            #endregion

            #region Diabetes
            var scfDiabetes = new ServiceComponentFieldsList();
            scfDiabetes.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_DIABETES_ID;
            scfDiabetes.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfDiabetes.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.DiabetesId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfDiabetes);
            #endregion

            #region EnfPulmonarCronica
            var scfEnfPulmonarCronica = new ServiceComponentFieldsList();
            scfEnfPulmonarCronica.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_ENF_PULMONAR_ID;
            scfEnfPulmonarCronica.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfEnfPulmonarCronica.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.PulmonarCronicaId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfEnfPulmonarCronica);
            #endregion

            #region Cancer
            var scfCancer = new ServiceComponentFieldsList();
            scfCancer.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_CANCER_ID;
            scfCancer.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfCancer.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.CancerId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfCancer);
            #endregion

            #region HipertensionArterial
            var scfHipertensionArterial = new ServiceComponentFieldsList();
            scfHipertensionArterial.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_HIPERTENCION_ARTERIAL_ID;
            scfHipertensionArterial.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfHipertensionArterial.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.HipertensionArterialId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfHipertensionArterial);
            #endregion

            #region Obesidad
            var scfObesidad = new ServiceComponentFieldsList();
            scfObesidad.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_OBESIDAD_ID;
            scfObesidad.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfObesidad.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.ObesidadId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfObesidad);
            #endregion

            #region Mayor65
            var scfMayor65 = new ServiceComponentFieldsList();
            scfMayor65.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_MAYOR_60_ID;
            scfMayor65.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfMayor65.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.Mayor65Id == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfMayor65);
            #endregion

            #region InsuficienciaRenalCronica
            var scfInsuficienciaRenalCronica = new ServiceComponentFieldsList();
            scfInsuficienciaRenalCronica.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_INSUFICIENCIA_RENAL_ID;
            scfInsuficienciaRenalCronica.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfInsuficienciaRenalCronica.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.InsuficienciaRenalId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfInsuficienciaRenalCronica);
            #endregion

            #region Embarazo
            var scfEmbarazo = new ServiceComponentFieldsList();
            scfEmbarazo.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_EMBARAZO_ID;
            scfEmbarazo.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfEmbarazo.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.EmbarazoPuerperioId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfEmbarazo);
            #endregion

            #region EnfCardiovascular
            var scfEnfCardiovascular = new ServiceComponentFieldsList();
            scfEnfCardiovascular.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_ENF_CARDIO_ID;
            scfEnfCardiovascular.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfEnfCardiovascular.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.EnfCardioVascularId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfEnfCardiovascular);
            #endregion

            #region Asma
            var scfAsma = new ServiceComponentFieldsList();
            scfAsma.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_ASMA_ID;
            scfAsma.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfAsma.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.AsmaId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfAsma);
            #endregion

            #region Inmunosupresor
            var scfInmunosupresor = new ServiceComponentFieldsList();
            scfInmunosupresor.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_INMUNOSUPRESOR_ID;
            scfInmunosupresor.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfInmunosupresor.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.InmunosupresorId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfInmunosupresor);
            #endregion

            #region RiesgoPersonalSalud
            var scfRiesgoPersonalSalud = new ServiceComponentFieldsList();
            scfRiesgoPersonalSalud.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_CHECK_PERSONAL_SALUD_ID;
            scfRiesgoPersonalSalud.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfRiesgoPersonalSalud.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.CheckPersonalSaludId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfRiesgoPersonalSalud);
            #endregion

            #region Certificacion
            var scfCertificacion = new ServiceComponentFieldsList();
            scfCertificacion.v_ComponentFieldsId = Constants.CERTIFICADO_DESCENSO_COVID_PROFESION_ID;
            scfCertificacion.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfCertificacion.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.CertificacionId } };
            serviceComponentFieldsList.Add(scfCertificacion);
            #endregion

            #region GlobalSession
            var sessione = (SessionModel)Session[Resources.Constante.SessionUsuario];
            var GlobalSession = new List<string>() { sessione.NodeId.ToString(), "", sessione.SystemUserId.ToString() };
            #endregion

            oServiceBL.AddServiceComponentValues(ref objOperationResult, serviceComponentFieldsList, GlobalSession, encuesta.PersonId, encuesta.ServiceComponentId);

            return null;
        }

    }
}
