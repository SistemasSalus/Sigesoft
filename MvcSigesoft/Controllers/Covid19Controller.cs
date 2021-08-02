using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using MvcSigesoft.ViewModels.Session;
using NetPdf;
using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcSigesoft.Controllers
{
    public class Covid19Controller : Controller
    {
        //
        // GET: /Covid19/

        public ActionResult Index(string personId, string serviceComponentId, string serviceId, string componentId, string organizationId)
        {
            OperationResult objOperationResult = new OperationResult();

            ViewBag.PERSONID = personId;
            ViewBag.SERVICECOMPONENTID = serviceComponentId;
            ViewBag.SERVICEID = serviceId;
            ViewBag.COMPONENTID = componentId;
            ViewBag.ORGANIZATIONID = organizationId;

            var listComponents = new List<string>();
            if (componentId == Constants.CERTIFICADO_COVID_ID)
            {
                listComponents.Add(Constants.CERTIFICADO_COVID_ID);
                listComponents.Add(Constants.ANTROPOMETRIA_ID);
                listComponents.Add(Constants.FUNCIONES_VITALES_ID);
            }
            else
            {
                listComponents.Add(Constants.COVID_ID);
            }

            var Datos = new ServiceBL().ValoresComponenteByListComponents(serviceId, listComponents);
            return View(Datos);
        }

        [HttpPost]
        public JsonResult SaveEncuesta(entidadEncuesta encuesta)
        {
            ServiceBL oServiceBL = new ServiceBL();
            OperationResult objOperationResult = new OperationResult();
            var serviceComponentFieldsList = new List<ServiceComponentFieldsList>();
            List<string> ClientSession = new List<string>();

             #region TipoDomicilioId
             var scfTipoDomicilioId = new ServiceComponentFieldsList();
             scfTipoDomicilioId.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_DOMICILIO_ID : Constants.COVID_DOMICILIO_ID;
             scfTipoDomicilioId.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfTipoDomicilioId.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.TipoDomicilioId } };
             serviceComponentFieldsList.Add(scfTipoDomicilioId);
             #endregion

             #region Geolocalizacion
             var scfGeolocalizacion = new ServiceComponentFieldsList();
             scfGeolocalizacion.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_GEOLOCALIZACION_ID : Constants.COVID_GEOLOCALIZACION_ID;
             scfGeolocalizacion.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfGeolocalizacion.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.Geolocalizacion } };
             serviceComponentFieldsList.Add(scfGeolocalizacion);
             #endregion

             #region PersonalSalud
             var scfPersonalSalud = new ServiceComponentFieldsList();
             scfPersonalSalud.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_ES_PERSONAL_SALUD_ID : Constants.COVID_ES_PERSONAL_SALUD_ID;
             scfPersonalSalud.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfPersonalSalud.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.PersonalSaludId } };
             serviceComponentFieldsList.Add(scfPersonalSalud);
             #endregion

             #region Profesion
             var scfProfesion = new ServiceComponentFieldsList();
             scfProfesion.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_PROFESION_ID : Constants.COVID_PROFESION_ID;
             scfProfesion.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfProfesion.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.ProfesionId } };
             serviceComponentFieldsList.Add(scfProfesion);
             #endregion

             #region Sintomas
             var scfSintomas = new ServiceComponentFieldsList();
             scfSintomas.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_TIENE_SINTOMAS_ID : Constants.COVID_TIENE_SINTOMAS_ID;
             scfSintomas.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfSintomas.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.SintomasId } };
             serviceComponentFieldsList.Add(scfSintomas);
             #endregion

             #region InicioSintomas
             var scfInicioSintomas = new ServiceComponentFieldsList();
             scfInicioSintomas.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_INICIO_SINTOMAS_ID : Constants.COVID_INICIO_SINTOMAS_ID;
             scfInicioSintomas.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfInicioSintomas.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.InicioSintomas } };
             serviceComponentFieldsList.Add(scfInicioSintomas);
             #endregion

             #region TosId
             var scfTosId = new ServiceComponentFieldsList();
             scfTosId.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_TOS_ID : Constants.COVID_TOS_ID;
             scfTosId.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfTosId.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.TosId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfTosId);
             #endregion

             #region DolorGarganta
             var scfDolorGarganta = new ServiceComponentFieldsList();
             scfDolorGarganta.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_DOLOR_GARGANTA_ID : Constants.COVID_DOLOR_GARGANTA_ID;
             scfDolorGarganta.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfDolorGarganta.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.DolorGargantaId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfDolorGarganta);
             #endregion

             #region CongestionNasal
             var scfCongestionNasal = new ServiceComponentFieldsList();
             scfCongestionNasal.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_CONGESTION_NASAL_ID : Constants.COVID_CONGESTION_NASAL_ID;
             scfCongestionNasal.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfCongestionNasal.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.CongestionNasalId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfCongestionNasal);
             #endregion

             #region DificultadRespiratoria
             var scfDificultadRespiratoria = new ServiceComponentFieldsList();
             scfDificultadRespiratoria.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_DIFIC_RESPIRA_ID : Constants.COVID_DIFIC_RESPIRA_ID;
             scfDificultadRespiratoria.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfDificultadRespiratoria.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.DificultadRespiratoriaId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfDificultadRespiratoria);
             #endregion

             #region FiebreEscalofrio
             var scfFiebreEscalofrio = new ServiceComponentFieldsList();
             scfFiebreEscalofrio.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_FIEBRE_ESCALOFRIO_ID : Constants.COVID_FIEBRE_ESCALOFRIO_ID;
             scfFiebreEscalofrio.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfFiebreEscalofrio.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.FiebreEscalofrioId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfFiebreEscalofrio);
             #endregion

             #region MalestarGeneral
             var scfMalestarGeneral = new ServiceComponentFieldsList();
             scfMalestarGeneral.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_MALESTAR_GENERAL_ID : Constants.COVID_MALESTAR_GENERAL_ID;
             scfMalestarGeneral.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfMalestarGeneral.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.MalestarGeneralId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfMalestarGeneral);
             #endregion

             #region Diarrea
             var scfDiarrea = new ServiceComponentFieldsList();
             scfDiarrea.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_DIARREA_ID : Constants.COVID_DIARREA_ID;
             scfDiarrea.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfDiarrea.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.DiarreaId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfDiarrea);
             #endregion

             #region NauseasVomitos
             var scfNauseasVomitos = new ServiceComponentFieldsList();
             scfNauseasVomitos.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_NAUSEAS_ID : Constants.COVID_NAUSEAS_ID;
             scfNauseasVomitos.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfNauseasVomitos.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.NauseasVomitosId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfNauseasVomitos);
             #endregion

             #region Cefalea
             var scfCefalea = new ServiceComponentFieldsList();
             scfCefalea.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_CEFALEA_ID : Constants.COVID_CEFALEA_ID;
             scfCefalea.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfCefalea.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.CefaleaId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfCefalea);
             #endregion

             #region IrritabilidadConfusion
             var scfIrritabilidadConfusion = new ServiceComponentFieldsList();
             scfIrritabilidadConfusion.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_IRRITABILIDAD_ID : Constants.COVID_IRRITABILIDAD_ID;
             scfIrritabilidadConfusion.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfIrritabilidadConfusion.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.IrritabilidadConfusionId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfIrritabilidadConfusion);
             #endregion

             #region Dolor
             var scfDolor = new ServiceComponentFieldsList();
             scfDolor.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_DOLOR_ID : Constants.COVID_DOLOR_ID;
             scfDolor.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfDolor.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.DolorId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfDolor);
             #endregion

             #region Expectoracion
             var scfExpectoracion = new ServiceComponentFieldsList();
             scfExpectoracion.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_OTROS_ID : Constants.COVID_OTROS_ID;
             scfExpectoracion.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfExpectoracion.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.ExpectoracionId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfExpectoracion);
             #endregion

             #region Muscular
             var scfMuscular = new ServiceComponentFieldsList();
             scfMuscular.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_MUSCULAR_ID : Constants.COVID_MUSCULAR_ID;
             scfMuscular.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfMuscular.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.MuscularId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfMuscular);
             #endregion

             #region Abdominal
             var scfAbdominal = new ServiceComponentFieldsList();
             scfAbdominal.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_ABDOMINAL_ID : Constants.COVID_ABDOMINAL_ID;
             scfAbdominal.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfAbdominal.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.AbdominalId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfAbdominal);
             #endregion

             #region Pecho
             var scfPecho = new ServiceComponentFieldsList();
             scfPecho.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_PECHO_ID : Constants.COVID_PECHO_ID;
             scfPecho.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfPecho.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.PechoId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfPecho);
             #endregion

             #region Articulaciones
             var scfArticulaciones = new ServiceComponentFieldsList();
             scfArticulaciones.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_ARTICULACIONES_ID : Constants.COVID_ARTICULACIONES_ID;
             scfArticulaciones.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfArticulaciones.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.ArticulacionesId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfArticulaciones);
             #endregion

             #region OtrosSintomas
             var scfOtrosSintomas = new ServiceComponentFieldsList();
             scfOtrosSintomas.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_OTROS_SINTOMAS_ID : Constants.COVID_OTROS_SINTOMAS_ID;
             scfOtrosSintomas.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfOtrosSintomas.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.OtrosSintomas } };
             serviceComponentFieldsList.Add(scfOtrosSintomas);
             #endregion

             #region Diabetes
             var scfDiabetes = new ServiceComponentFieldsList();
             scfDiabetes.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_DIABETES_ID : Constants.COVID_DIABETES_ID;
             scfDiabetes.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfDiabetes.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.DiabetesId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfDiabetes);
             #endregion

             #region EnfPulmonarCronica
             var scfEnfPulmonarCronica = new ServiceComponentFieldsList();
             scfEnfPulmonarCronica.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_ENF_PULMONAR_ID : Constants.COVID_ENF_PULMONAR_ID;
             scfEnfPulmonarCronica.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfEnfPulmonarCronica.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.PulmonarCronicaId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfEnfPulmonarCronica);
             #endregion

             #region Cancer
             var scfCancer = new ServiceComponentFieldsList();
             scfCancer.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_CANCER_ID : Constants.COVID_CANCER_ID;
             scfCancer.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfCancer.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.CancerId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfCancer);
             #endregion

             #region HipertensionArterial
             var scfHipertensionArterial = new ServiceComponentFieldsList();
             scfHipertensionArterial.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_HIPERTENCION_ARTERIAL_ID : Constants.COVID_HIPERTENCION_ARTERIAL_ID;
             scfHipertensionArterial.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfHipertensionArterial.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.HipertensionArterialId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfHipertensionArterial);
             #endregion

             #region Obesidad
             var scfObesidad = new ServiceComponentFieldsList();
             scfObesidad.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_OBESIDAD_ID : Constants.COVID_OBESIDAD_ID;
             scfObesidad.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfObesidad.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.ObesidadId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfObesidad);
             #endregion

             #region Mayor65
             var scfMayor65 = new ServiceComponentFieldsList();
             scfMayor65.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_MAYOR_60_ID : Constants.COVID_MAYOR_60_ID;
             scfMayor65.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfMayor65.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.Mayor65Id == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfMayor65);
             #endregion

             #region InsuficienciaRenalCronica
             var scfInsuficienciaRenalCronica = new ServiceComponentFieldsList();
             scfInsuficienciaRenalCronica.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_INSUFICIENCIA_RENAL_ID : Constants.COVID_INSUFICIENCIA_RENAL_ID;
             scfInsuficienciaRenalCronica.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfInsuficienciaRenalCronica.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.InsuficienciaRenalId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfInsuficienciaRenalCronica);
             #endregion

             #region Embarazo
             var scfEmbarazo = new ServiceComponentFieldsList();
             scfEmbarazo.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_EMBARAZO_ID : Constants.COVID_EMBARAZO_ID;
             scfEmbarazo.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfEmbarazo.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.EmbarazoPuerperioId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfEmbarazo);
             #endregion

             #region EnfCardiovascular
             var scfEnfCardiovascular = new ServiceComponentFieldsList();
             scfEnfCardiovascular.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_ENF_CARDIO_ID : Constants.COVID_ENF_CARDIO_ID;
             scfEnfCardiovascular.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfEnfCardiovascular.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.EnfCardioVascularId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfEnfCardiovascular);
             #endregion

             #region Asma
             var scfAsma = new ServiceComponentFieldsList();
             scfAsma.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_ASMA_ID : Constants.COVID_ASMA_ID;
             scfAsma.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfAsma.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.AsmaId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfAsma);
             #endregion

             #region Inmunosupresor
             var scfInmunosupresor = new ServiceComponentFieldsList();
             scfInmunosupresor.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_INMUNOSUPRESOR_ID : Constants.COVID_INMUNOSUPRESOR_ID;
             scfInmunosupresor.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfInmunosupresor.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.InmunosupresorId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfInmunosupresor);
             #endregion
            
             #region RiesgoPersonalSalud
             var scfRiesgoPersonalSalud = new ServiceComponentFieldsList();
             scfRiesgoPersonalSalud.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_PERSONAL_SALUD_ID : Constants.COVID_PERSONAL_SALUD_ID;
             scfRiesgoPersonalSalud.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfRiesgoPersonalSalud.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.RiesgoPersonalSaludId == true ? "1" :"0" } };
             serviceComponentFieldsList.Add(scfRiesgoPersonalSalud);
             #endregion

             #region Certificacion
             var scfCertificacion = new ServiceComponentFieldsList();
             scfCertificacion.v_ComponentFieldsId = encuesta.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_DESCENSO_COVID_CERTIFICACION_ID : Constants.CERTIFICADO_DESCENSO_COVID_CERTIFICACION_ID;
             scfCertificacion.v_ServiceComponentId = encuesta.ServiceComponentId;
             scfCertificacion.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.CertificacionId == true ? "1" : "0" } };
             serviceComponentFieldsList.Add(scfCertificacion);
             #endregion

             #region GlobalSession
             var sessione = (SessionModel)Session[Resources.Constante.SessionUsuario];
             var GlobalSession = new List<string>() { sessione.NodeId.ToString(), "", sessione.SystemUserId.ToString() };
             #endregion

             oServiceBL.AddServiceComponentValues(ref objOperationResult, serviceComponentFieldsList, GlobalSession, encuesta.PersonId, encuesta.ServiceComponentId);

            return null;
        }

        [HttpPost]
        public JsonResult SaveLaboratorio(entidadLaboratorio laboratorio)
        {
            Covid19BL oCovid19BL = new Covid19BL();
            ServiceBL oServiceBL = new ServiceBL();
            OperationResult objOperationResult = new OperationResult();
            var serviceComponentFieldsList = new List<ServiceComponentFieldsList>();
            List<string> ClientSession = new List<string>();

            #region FechaEjecucion
             var scfFechaEjecucion = new ServiceComponentFieldsList();
             scfFechaEjecucion.v_ComponentFieldsId = laboratorio.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_FECHA_EJECUCION_ID : Constants.COVID_FECHA_EJECUCION_ID;
             scfFechaEjecucion.v_ServiceComponentId = laboratorio.ServiceComponentId;
             scfFechaEjecucion.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = laboratorio.FechaEjecucion } };
             serviceComponentFieldsList.Add(scfFechaEjecucion);
             #endregion            

            #region ProcedenciaSolicitudId
            var scfProcedenciaSolicitudId = new ServiceComponentFieldsList();
            scfProcedenciaSolicitudId.v_ComponentFieldsId = laboratorio.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_PROCEDENCIA_SOLICITUD_ID : Constants.COVID_PROCEDENCIA_SOLICITUD_ID;
            scfProcedenciaSolicitudId.v_ServiceComponentId = laboratorio.ServiceComponentId;
            scfProcedenciaSolicitudId.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = laboratorio.ProcedenciaSolicitudId } };
            serviceComponentFieldsList.Add(scfProcedenciaSolicitudId);
            #endregion

            #region ResultadoPrimeraPruebaId
            var scfResultadoPrimeraPruebaId = new ServiceComponentFieldsList();
            scfResultadoPrimeraPruebaId.v_ComponentFieldsId = laboratorio.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_RES_1_PRUEBA_ID : Constants.COVID_RES_1_PRUEBA_ID;
            scfResultadoPrimeraPruebaId.v_ServiceComponentId = laboratorio.ServiceComponentId;
            scfResultadoPrimeraPruebaId.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = laboratorio.ResultadoPrimeraPruebaId } };
            serviceComponentFieldsList.Add(scfResultadoPrimeraPruebaId);
            #endregion

            #region ResultadoSegundaPruebaId
            var scfResultadoSegundaPruebaId = new ServiceComponentFieldsList();
            scfResultadoSegundaPruebaId.v_ComponentFieldsId = laboratorio.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_RES_2_PRUEBA_ID : Constants.COVID_RES_2_PRUEBA_ID;
            scfResultadoSegundaPruebaId.v_ServiceComponentId = laboratorio.ServiceComponentId;
            scfResultadoSegundaPruebaId.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = laboratorio.ResultadoSegundaPruebaId } };
            serviceComponentFieldsList.Add(scfResultadoSegundaPruebaId);
            #endregion

            #region ClasificacionClinica
            var scfClasificacionClinica = new ServiceComponentFieldsList();
            scfClasificacionClinica.v_ComponentFieldsId = laboratorio.ComponentId == Constants.CERTIFICADO_COVID_ID ? Constants.CERTIFICADO_COVID_CLASIFICACION_CLINICA_ID : Constants.COVID_CLASIFICACION_CLINICA_ID;
            scfClasificacionClinica.v_ServiceComponentId = laboratorio.ServiceComponentId;
            scfClasificacionClinica.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = laboratorio.ClasificacionClinicaId } };
            serviceComponentFieldsList.Add(scfClasificacionClinica);
            #endregion
          
            #region GlobalSession
            var sessione = (SessionModel)Session[Resources.Constante.SessionUsuario];
            var GlobalSession = new List<string>() { sessione.NodeId.ToString(), "", sessione.SystemUserId.ToString() };
            #endregion

            oServiceBL.AddServiceComponentValues(ref objOperationResult, serviceComponentFieldsList, GlobalSession, laboratorio.PersonId, laboratorio.ServiceComponentId);

            if (laboratorio.ComponentId == Constants.COVID_ID)
            {
            #region Generar Reporte Covid
            var sedeEmpresa = "----";
            MergeExPDF _mergeExPDF = new MergeExPDF();
            ReportDocument rp = new ReportDocument();
            List<string> _filesNameToMerge = new List<string>();
            var pstrRutaReportes = Server.MapPath("~/FichasCovid/");

            var COVID_ID = new List<ReportCertificadoCovid>();
            var dsGetRepo = new DataSet();
            var option = oCovid19BL.ObtenerOpcionFormato(laboratorio.OrganizationId);
            if (option == 1)//detallado
            {
                COVID_ID = new ServiceBL().GetCovid(ref objOperationResult, laboratorio.ServiceId);

                DataTable dt_COVID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(COVID_ID);

                dt_COVID.TableName = "dtCertificadoCovid";
                dsGetRepo.Tables.Add(dt_COVID);

                rp = new MvcSigesoft.Reports.crCovid();
                rp.SetDataSource(dsGetRepo);
                rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                var objDiskOpt = new DiskFileDestinationOptions();
                objDiskOpt.DiskFileName = pstrRutaReportes + laboratorio.ServiceId + "-" + Constants.COVID_ID + ".pdf";
                _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                rp.ExportOptions.DestinationOptions = objDiskOpt;
                rp.Export();

                var x = _filesNameToMerge.ToList();
                _mergeExPDF.FilesName = x;
                _mergeExPDF.DestinationFile = pstrRutaReportes + laboratorio.ServiceId + ".pdf"; ;
                _mergeExPDF.Execute();
            }
            else
            {
                COVID_ID = new ServiceBL().GetCovidResumido(ref objOperationResult, laboratorio.ServiceId);
                DataTable dt_COVID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(COVID_ID);

                dt_COVID.TableName = "dtCertificadoCovid";
                dsGetRepo.Tables.Add(dt_COVID);

                rp = new MvcSigesoft.Reports.rCovidResumido();
                rp.SetDataSource(dsGetRepo);
                rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                var objDiskOpt = new DiskFileDestinationOptions();
                objDiskOpt.DiskFileName = pstrRutaReportes + laboratorio.ServiceId + "-" + Constants.COVID_ID + ".pdf";
                _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                rp.ExportOptions.DestinationOptions = objDiskOpt;
                rp.Export();

                var x = _filesNameToMerge.ToList();
                _mergeExPDF.FilesName = x;
                _mergeExPDF.DestinationFile = pstrRutaReportes + laboratorio.ServiceId + ".pdf"; ;
                _mergeExPDF.Execute();
            }

            if (COVID_ID.Count != 0)
            {
                sedeEmpresa = COVID_ID[0].Sede;
            }

            #endregion  
 
            #region Enviar Email
            //try
            //{

            //    var resultadoCovid19 = "";
            //    if (laboratorio.ResultadoPrimeraPruebaId.ToString() == ((int)ResultadoCovid.Negativo).ToString())
            //    {
            //        if (laboratorio.OrganizationId == Constants.EMPRESA_BACKUS_ID)
            //        {
            //            resultadoCovid19 = "Negativo";
            //            EnviarPdfTrabajador(laboratorio.ServiceId, laboratorio.OrganizationId, resultadoCovid19, sedeEmpresa);
            //        }

            //    }
            //    else if (laboratorio.ResultadoPrimeraPruebaId.ToString() == ((int)ResultadoCovid.No_valido).ToString())
            //    {
            //        resultadoCovid19 = "Negativo";
            //    }
            //    else if (laboratorio.ResultadoPrimeraPruebaId.ToString() == ((int)ResultadoCovid.IgM_Positivo).ToString())
            //    {
            //        resultadoCovid19 = "IgM Positivo";
            //        EnviarPdfTrabajador(laboratorio.ServiceId, laboratorio.OrganizationId, resultadoCovid19, sedeEmpresa);
            //    }
            //    else if (laboratorio.ResultadoPrimeraPruebaId.ToString() == ((int)ResultadoCovid.IgG_Positivo).ToString())
            //    {
            //        resultadoCovid19 = "IgG Positivo";
            //        EnviarPdfTrabajador(laboratorio.ServiceId, laboratorio.OrganizationId, resultadoCovid19, sedeEmpresa);
            //    }
            //    else if (laboratorio.ResultadoPrimeraPruebaId.ToString() == ((int)ResultadoCovid.IgM_e_IgG_positivo).ToString())
            //    {
            //        resultadoCovid19 = "IgM e IgG Positivo";
            //        EnviarPdfTrabajador(laboratorio.ServiceId, laboratorio.OrganizationId, resultadoCovid19, sedeEmpresa);
            //    }
            //    else if (laboratorio.ResultadoPrimeraPruebaId.ToString() == ((int)ResultadoCovid.No_se_realizo).ToString())
            //    {
            //        resultadoCovid19 = "No se realizó";
            //        EnviarPdfTrabajador(laboratorio.ServiceId, laboratorio.OrganizationId, resultadoCovid19, sedeEmpresa);
            //    }

            //}

            //catch (Exception ex)
            //{
            //    throw;
            //}
            #endregion
    
            }
            return null;
        }

        [HttpPost]
        public JsonResult SaveTriaje(entidadTriaje triaje)
        {
            ServiceBL oServiceBL = new ServiceBL();
            OperationResult objOperationResult = new OperationResult();
            var serviceComponentFieldsList = new List<ServiceComponentFieldsList>();
            List<string> ClientSession = new List<string>();

            #region Reemplazar serviceComponentId
            triaje.ServiceComponentId = new ServiceBL().obtenerServiceComponentIdByServiceAndComponentId(triaje.ServiceId, Constants.FUNCIONES_VITALES_ID);
            #endregion

            #region Peso
            var scfPeso = new ServiceComponentFieldsList();
            scfPeso.v_ComponentFieldsId = Constants.ANTROPOMETRIA_PESO_ID;
            scfPeso.v_ServiceComponentId = triaje.ServiceComponentId;
            scfPeso.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = triaje.Peso } };
            serviceComponentFieldsList.Add(scfPeso);
            #endregion

            #region Talla
            var scfTalla = new ServiceComponentFieldsList();
            scfTalla.v_ComponentFieldsId = Constants.ANTROPOMETRIA_TALLA_ID;
            scfTalla.v_ServiceComponentId = triaje.ServiceComponentId;
            scfTalla.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = triaje.Talla } };
            serviceComponentFieldsList.Add(scfTalla);
            #endregion

            #region Imc
            var scfImc = new ServiceComponentFieldsList();
            scfImc.v_ComponentFieldsId = Constants.ANTROPOMETRIA_IMC_ID;
            scfImc.v_ServiceComponentId = triaje.ServiceComponentId;
            scfImc.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = triaje.Imc } };
            serviceComponentFieldsList.Add(scfImc);
            #endregion

            #region Temperatura
            var scfTemperatura = new ServiceComponentFieldsList();
            scfTemperatura.v_ComponentFieldsId = Constants.FUNCIONES_VITALES_TEMPERATURA_ID;
            scfTemperatura.v_ServiceComponentId = triaje.ServiceComponentId;
            scfTemperatura.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = triaje.Temperatura } };
            serviceComponentFieldsList.Add(scfTemperatura);
            #endregion

            #region Fc
            var scfFc = new ServiceComponentFieldsList();
            scfFc.v_ComponentFieldsId = Constants.FUNCIONES_VITALES_FREC_CARDIACA_ID;
            scfFc.v_ServiceComponentId = triaje.ServiceComponentId;
            scfFc.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = triaje.Fc } };
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

        private void EnviarPdfTrabajador(string serviceId, string organizationId, string resultadoCovid, string sedeEmpresa)
        {
            OperationResult objOperationResult = new OperationResult();
            try
            {
                var pstrRutaReportes = Server.MapPath("~/FichasCovid/");
                var configEmail = Sigesoft.Node.WinClient.BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 161, "i_ParameterId");

                var empresaData = new OrganizationBL().GetOrganization(ref objOperationResult, organizationId);

                var trabajadorData = new ServiceBL().GetWorkerData(serviceId);
                if (string.IsNullOrEmpty(trabajadorData.Email)) return;

                var correoTrabajador = trabajadorData.Email;
                string smtp = configEmail[0].Value1.ToLower();
                int port = int.Parse(configEmail[1].Value1);
                string from = configEmail[2].Value1.ToLower();
                string fromPassword = configEmail[4].Value1;
                string subject = "Resultado Covid-19 NODO LIMA - SEDE " + sedeEmpresa;
                bool enableSsl = Convert.ToBoolean(int.Parse(configEmail[3].Value1));
                string message = string.Format("Buenos días, el resultado del trabajador {0} , es: {1}", trabajadorData.Trabajador, resultadoCovid);
                string ruta = pstrRutaReportes;
                var atachFiles = new List<string>();
                string atachFile = ruta + serviceId + ".pdf";
                atachFiles.Add(atachFile);

                var enviarA = empresaData.v_Mail ;
                var copiaCc = correoTrabajador;
                var copiaBcc = "francisco.collantes@saluslaboris.com.pe";
                //var copiaBcc = resultadoCovid == "IgM Positivo" ? "alberto.merchan@saluslaboris.com.pe" : null;
                Sigesoft.Common.Utils.SendMessage(smtp, port, enableSsl, true, from, fromPassword, enviarA, copiaCc, copiaBcc, subject, message, atachFiles);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }


    public class entidadEncuesta {

        public int NodeId { get; set; }
        public string ServiceComponentId { get; set; }
        public string PersonId { get; set; }
        public string SystemUserId { get; set; }
        public string ComponentId { get; set; }

        public string TipoDomicilioId { get; set; }
        public string Geolocalizacion { get; set; }
        public string PersonalSaludId { get; set; }
        public string ProfesionId { get; set; }
        public string SintomasId { get; set; }
        public string InicioSintomas { get; set; }

        public bool TosId { get; set; }
        public bool DolorGargantaId { get; set; }
        public bool CongestionNasalId { get; set; }
        public bool DificultadRespiratoriaId { get; set; }

        public bool FiebreEscalofrioId { get; set; }
        public bool MalestarGeneralId { get; set; }
        public bool DiarreaId { get; set; }
        public bool NauseasVomitosId { get; set; }

        public bool CefaleaId { get; set; }
        public bool IrritabilidadConfusionId { get; set; }
        public bool DolorId { get; set; }
        public bool ExpectoracionId { get; set; }

        public bool MuscularId { get; set; }
        public bool AbdominalId { get; set; }
        public bool PechoId { get; set; }
        public bool ArticulacionesId { get; set; }

        public string OtrosSintomas { get; set; }

        public bool DiabetesId { get; set; }
        public bool PulmonarCronicaId { get; set; }
        public bool CancerId { get; set; }
        public bool HipertensionArterialId { get; set; }

        public bool ObesidadId { get; set; }
        public bool Mayor65Id { get; set; }
        public bool InsuficienciaRenalId { get; set; }
        public bool EmbarazoPuerperioId { get; set; }

        public bool EnfCardioVascularId { get; set; }
        public bool AsmaId { get; set; }
        public bool InmunosupresorId { get; set; }
        public bool RiesgoPersonalSaludId { get; set; }

        public bool CertificacionId { get; set; }
    }

    public class entidadLaboratorio {

        public int NodeId { get; set; }
        public string ServiceId { get; set; }
        public string ServiceComponentId { get; set; }
        public string PersonId { get; set; }
        public string SystemUserId { get; set; }
        public string ComponentId { get; set; }
        public string OrganizationId { get; set; }

        public string FechaEjecucion { get; set; }
        public string ProcedenciaSolicitudId { get; set; }
        public string ResultadoPrimeraPruebaId { get; set; }
        public string ResultadoSegundaPruebaId { get; set; }
        public string ClasificacionClinicaId { get; set; }

    }

    public class entidadTriaje
    {
        public int NodeId { get; set; }
        public string ServiceId { get; set; }
        public string ServiceComponentId { get; set; }
        public string PersonId { get; set; }
        public string SystemUserId { get; set; }
        public string ComponentId { get; set; }
        public string OrganizationId { get; set; }

        public string Peso { get; set; }
        public string Talla { get; set; }
        public string Imc { get; set; }
        public string Temperatura { get; set; }
        public string Fc { get; set; }
        public string SatO2 { get; set; }

    }

    public class entidadTriajeRetorno
    {
        public int NodeId { get; set; }
        public string ServiceId { get; set; }
        public string ServiceComponentId { get; set; }
        public string PersonId { get; set; }
        public string SystemUserId { get; set; }
        public string ComponentId { get; set; }
        public string OrganizationId { get; set; }
        
        public string Temperatura { get; set; }
        public string Fr { get; set; }
        public string SatO2 { get; set; }

    }

    public class entidadEncuestaRetorno
    {

        public int NodeId { get; set; }
        public string ServiceComponentId { get; set; }
        public string PersonId { get; set; }
        public string SystemUserId { get; set; }
        public string ComponentId { get; set; }

        public string EstablecimientoSaludId { get; set; }
        public string TomaMedicacionId { get; set; }

        public string TipoDomicilioId { get; set; }
        public string PreguntaPersonalSaludId { get; set; }
        public string ProfesionId { get; set; }
        public string SintomasId { get; set; }
        public string InicioSintomas { get; set; }

        public bool TosId { get; set; }
        public bool DolorGargantaId { get; set; }
        public bool CongestionNasalId { get; set; }
        public bool DificultadRespiratoriaId { get; set; }

        public bool FiebreEscalofrioId { get; set; }
        public bool MalestarGeneralId { get; set; }
        public bool DiarreaId { get; set; }
        public bool NauseasVomitosId { get; set; }

        public bool CefaleaId { get; set; }
        public bool IrritabilidadConfusionId { get; set; }
        public bool DolorId { get; set; }
        public bool ExpectoracionId { get; set; }

        public bool MuscularId { get; set; }
        public bool AbdominalId { get; set; }
        public bool PechoId { get; set; }
        public bool ArticulacionesId { get; set; }

        public string OtrosSintomas { get; set; }

        public bool DiabetesId { get; set; }
        public bool PulmonarCronicaId { get; set; }
        public bool CancerId { get; set; }
        public bool HipertensionArterialId { get; set; }

        public bool ObesidadId { get; set; }
        public bool Mayor65Id { get; set; }
        public bool InsuficienciaRenalId { get; set; }
        public bool EmbarazoPuerperioId { get; set; }
        public bool CheckPersonalSaludId { get; set; }

        public bool EnfCardioVascularId { get; set; }
        public bool AsmaId { get; set; }
        public bool InmunosupresorId { get; set; }
        public string CertificacionId { get; set; }
    }

    enum ResultadoCovid
    {
        Negativo =  0,
        No_valido =  1,
        IgM_Positivo =  2,
        IgG_Positivo =  3,
        IgM_e_IgG_positivo =  4,
        No_se_realizo = 5,
    }

    public class entidadRegistrarTrabajador{

        public string EmpresaId { get; set; }
        public string SedeId { get; set; }
        public string ProtocoloId { get; set; }

        public string EmpresaNombre { get; set; }
        public string SedeNombre { get; set; }
        public string ProtocoloNombre { get; set; }

        public string FechaServicio { get; set; }
        public string Nombres { get; set; }
        public string ApePaterno { get; set; }
        public string ApeMaterno { get; set; }
        public string TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string Genero { get; set; }
        public string FechaNacimiento { get; set; }
        public string Email { get; set; }
        public string Celulares { get; set; }
        public string Puesto { get; set; }
        public string Direccion { get; set; }
        public string NombreSede { get; set; }
        public int TipoEmpresaCovidId { get; set; }
        public int ClinicaExternad { get; set; }
    }
  
}
