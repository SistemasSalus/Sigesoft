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
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MvcSigesoft.Controllers
{
    public class PruebaRapidaCovid19Controller : Controller
    {
        //
        // GET: /PruebaRapidaCovid19/

        public ActionResult Index()
        {
            var lista = new List<ListaCovid19>();
            Covid19BL oCovid19BL = new Covid19BL();
            ServiceBL oServiceBL = new ServiceBL();
            OperationResult objOperationResult = new OperationResult();

            var fechaServicio = DateTime.Now.Date;
            //var fechaServicio = DateTime.Parse("03-08-2020");

            var sessione = (SessionModel)Session[Resources.Constante.SessionUsuario];
            ViewBag.NOMBRENODO = string.Format("Nodo: {0}, Empresa: {1}, Protocolo:{2}", sessione.Nodo, sessione.Empresa, sessione.Protocol);
            ViewBag.SEDESBACKUS = ObtenerSedesPorEmpresa(sessione.EmpresaId);

            ViewBag.TIPOEMPRESACOVID = Sigesoft.Node.WinClient.BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 310, null);

            lista = oCovid19BL.ObtenerLista(fechaServicio, sessione.EmpresaId, sessione.NodeId.ToString());

            if (lista.Count > 0)
            {
                ViewBag.CountNegativo = lista[0].CountNegativo;
                ViewBag.CountNoValido = lista[0].CountNoValido;
                ViewBag.CountIgMPositivo = lista[0].CountIgMPositivo;
                ViewBag.CountIgGPositivo = lista[0].CountIgGPositivo;
                ViewBag.CountIgMIgGPositivo = lista[0].CountIgMIgGPositivo;
                ViewBag.CountTotal = lista.Count;
            }
            else
            {
                ViewBag.CountNegativo = 0;
                ViewBag.CountNoValido = 0;
                ViewBag.CountIgMPositivo = 0;
                ViewBag.CountIgGPositivo = 0;
                ViewBag.CountIgMIgGPositivo = 0;
                ViewBag.CountTotal = 0;
            }
            
            return View(lista);
        }

        public ActionResult Examen(string personId, string serviceComponentId, string serviceId)
        {
            OperationResult objOperationResult = new OperationResult();
            Covid19BL oCovid19BL = new Covid19BL();
            var sessione = (SessionModel)Session[Resources.Constante.SessionUsuario];

            ViewBag.PERSONID = personId;
            ViewBag.SERVICECOMPONENTID = serviceComponentId;
            ViewBag.SERVICEID = serviceId;
            ViewBag.COMPONENTID = Constants.COVID_ID;
            ViewBag.ORGANIZATIONID = sessione.EmpresaId;
            //ViewBag.ORGANIZATIONID = Constants.EMPRESA_BACKUS_ID;

            var listComponents = new List<string>();
            listComponents.Add(Constants.COVID_ID);

            var Datos = new ServiceBL().ValoresComponenteByListComponents(serviceId, listComponents);

            ViewBag.UltimosServicios = oCovid19BL.ObtenerUltimosResultadoPruebaCovid19(personId);
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
            scfTipoDomicilioId.v_ComponentFieldsId = Constants.COVID_DOMICILIO_ID;
            scfTipoDomicilioId.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfTipoDomicilioId.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.TipoDomicilioId } };
            serviceComponentFieldsList.Add(scfTipoDomicilioId);
            #endregion

            #region Geolocalizacion
            var scfGeolocalizacion = new ServiceComponentFieldsList();
            scfGeolocalizacion.v_ComponentFieldsId = Constants.COVID_GEOLOCALIZACION_ID;
            scfGeolocalizacion.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfGeolocalizacion.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.Geolocalizacion } };
            serviceComponentFieldsList.Add(scfGeolocalizacion);
            #endregion

            #region PersonalSalud
            var scfPersonalSalud = new ServiceComponentFieldsList();
            scfPersonalSalud.v_ComponentFieldsId = Constants.COVID_ES_PERSONAL_SALUD_ID;
            scfPersonalSalud.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfPersonalSalud.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.PersonalSaludId } };
            serviceComponentFieldsList.Add(scfPersonalSalud);
            #endregion

            #region Profesion
            var scfProfesion = new ServiceComponentFieldsList();
            scfProfesion.v_ComponentFieldsId = Constants.COVID_PROFESION_ID;
            scfProfesion.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfProfesion.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.ProfesionId } };
            serviceComponentFieldsList.Add(scfProfesion);
            #endregion

            #region Sintomas
            var scfSintomas = new ServiceComponentFieldsList();
            scfSintomas.v_ComponentFieldsId = Constants.COVID_TIENE_SINTOMAS_ID;
            scfSintomas.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfSintomas.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.SintomasId } };
            serviceComponentFieldsList.Add(scfSintomas);
            #endregion

            #region InicioSintomas
            var scfInicioSintomas = new ServiceComponentFieldsList();
            scfInicioSintomas.v_ComponentFieldsId = Constants.COVID_INICIO_SINTOMAS_ID;
            scfInicioSintomas.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfInicioSintomas.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.InicioSintomas } };
            serviceComponentFieldsList.Add(scfInicioSintomas);
            #endregion

            #region TosId
            var scfTosId = new ServiceComponentFieldsList();
            scfTosId.v_ComponentFieldsId = Constants.COVID_TOS_ID;
            scfTosId.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfTosId.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.TosId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfTosId);
            #endregion

            #region DolorGarganta
            var scfDolorGarganta = new ServiceComponentFieldsList();
            scfDolorGarganta.v_ComponentFieldsId = Constants.COVID_DOLOR_GARGANTA_ID;
            scfDolorGarganta.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfDolorGarganta.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.DolorGargantaId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfDolorGarganta);
            #endregion

            #region CongestionNasal
            var scfCongestionNasal = new ServiceComponentFieldsList();
            scfCongestionNasal.v_ComponentFieldsId = Constants.COVID_CONGESTION_NASAL_ID;
            scfCongestionNasal.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfCongestionNasal.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.CongestionNasalId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfCongestionNasal);
            #endregion

            #region DificultadRespiratoria
            var scfDificultadRespiratoria = new ServiceComponentFieldsList();
            scfDificultadRespiratoria.v_ComponentFieldsId = Constants.COVID_DIFIC_RESPIRA_ID;
            scfDificultadRespiratoria.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfDificultadRespiratoria.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.DificultadRespiratoriaId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfDificultadRespiratoria);
            #endregion

            #region FiebreEscalofrio
            var scfFiebreEscalofrio = new ServiceComponentFieldsList();
            scfFiebreEscalofrio.v_ComponentFieldsId = Constants.COVID_FIEBRE_ESCALOFRIO_ID;
            scfFiebreEscalofrio.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfFiebreEscalofrio.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.FiebreEscalofrioId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfFiebreEscalofrio);
            #endregion

            #region MalestarGeneral
            var scfMalestarGeneral = new ServiceComponentFieldsList();
            scfMalestarGeneral.v_ComponentFieldsId = Constants.COVID_MALESTAR_GENERAL_ID;
            scfMalestarGeneral.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfMalestarGeneral.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.MalestarGeneralId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfMalestarGeneral);
            #endregion

            #region Diarrea
            var scfDiarrea = new ServiceComponentFieldsList();
            scfDiarrea.v_ComponentFieldsId = Constants.COVID_DIARREA_ID;
            scfDiarrea.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfDiarrea.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.DiarreaId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfDiarrea);
            #endregion

            #region NauseasVomitos
            var scfNauseasVomitos = new ServiceComponentFieldsList();
            scfNauseasVomitos.v_ComponentFieldsId = Constants.COVID_NAUSEAS_ID;
            scfNauseasVomitos.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfNauseasVomitos.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.NauseasVomitosId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfNauseasVomitos);
            #endregion

            #region Cefalea
            var scfCefalea = new ServiceComponentFieldsList();
            scfCefalea.v_ComponentFieldsId = Constants.COVID_CEFALEA_ID;
            scfCefalea.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfCefalea.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.CefaleaId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfCefalea);
            #endregion

            #region IrritabilidadConfusion
            var scfIrritabilidadConfusion = new ServiceComponentFieldsList();
            scfIrritabilidadConfusion.v_ComponentFieldsId = Constants.COVID_IRRITABILIDAD_ID;
            scfIrritabilidadConfusion.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfIrritabilidadConfusion.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.IrritabilidadConfusionId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfIrritabilidadConfusion);
            #endregion

            #region Dolor
            var scfDolor = new ServiceComponentFieldsList();
            scfDolor.v_ComponentFieldsId = Constants.COVID_DOLOR_ID;
            scfDolor.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfDolor.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.DolorId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfDolor);
            #endregion

            #region Expectoracion
            var scfExpectoracion = new ServiceComponentFieldsList();
            scfExpectoracion.v_ComponentFieldsId = Constants.COVID_OTROS_ID;
            scfExpectoracion.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfExpectoracion.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.ExpectoracionId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfExpectoracion);
            #endregion

            #region Muscular
            var scfMuscular = new ServiceComponentFieldsList();
            scfMuscular.v_ComponentFieldsId = Constants.COVID_MUSCULAR_ID;
            scfMuscular.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfMuscular.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.MuscularId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfMuscular);
            #endregion

            #region Abdominal
            var scfAbdominal = new ServiceComponentFieldsList();
            scfAbdominal.v_ComponentFieldsId = Constants.COVID_ABDOMINAL_ID;
            scfAbdominal.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfAbdominal.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.AbdominalId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfAbdominal);
            #endregion

            #region Pecho
            var scfPecho = new ServiceComponentFieldsList();
            scfPecho.v_ComponentFieldsId = Constants.COVID_PECHO_ID;
            scfPecho.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfPecho.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.PechoId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfPecho);
            #endregion

            #region Articulaciones
            var scfArticulaciones = new ServiceComponentFieldsList();
            scfArticulaciones.v_ComponentFieldsId = Constants.COVID_ARTICULACIONES_ID;
            scfArticulaciones.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfArticulaciones.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.ArticulacionesId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfArticulaciones);
            #endregion

            #region OtrosSintomas
            var scfOtrosSintomas = new ServiceComponentFieldsList();
            scfOtrosSintomas.v_ComponentFieldsId = Constants.COVID_OTROS_SINTOMAS_ID;
            scfOtrosSintomas.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfOtrosSintomas.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.OtrosSintomas } };
            serviceComponentFieldsList.Add(scfOtrosSintomas);
            #endregion

            #region Diabetes
            var scfDiabetes = new ServiceComponentFieldsList();
            scfDiabetes.v_ComponentFieldsId = Constants.COVID_DIABETES_ID;
            scfDiabetes.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfDiabetes.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.DiabetesId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfDiabetes);
            #endregion

            #region EnfPulmonarCronica
            var scfEnfPulmonarCronica = new ServiceComponentFieldsList();
            scfEnfPulmonarCronica.v_ComponentFieldsId = Constants.COVID_ENF_PULMONAR_ID;
            scfEnfPulmonarCronica.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfEnfPulmonarCronica.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.PulmonarCronicaId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfEnfPulmonarCronica);
            #endregion

            #region Cáncer
            var scfCancer = new ServiceComponentFieldsList();
            scfCancer.v_ComponentFieldsId = Constants.COVID_CANCER_ID;
            scfCancer.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfCancer.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.CancerId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfCancer);
            #endregion

            #region HipertensionArterial
            var scfHipertensionArterial = new ServiceComponentFieldsList();
            scfHipertensionArterial.v_ComponentFieldsId = Constants.COVID_HIPERTENCION_ARTERIAL_ID;
            scfHipertensionArterial.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfHipertensionArterial.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.HipertensionArterialId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfHipertensionArterial);
            #endregion

            #region Obesidad
            var scfObesidad = new ServiceComponentFieldsList();
            scfObesidad.v_ComponentFieldsId = Constants.COVID_OBESIDAD_ID;
            scfObesidad.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfObesidad.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.ObesidadId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfObesidad);
            #endregion

            #region Mayor65
            var scfMayor65 = new ServiceComponentFieldsList();
            scfMayor65.v_ComponentFieldsId = Constants.COVID_MAYOR_60_ID;
            scfMayor65.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfMayor65.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.Mayor65Id == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfMayor65);
            #endregion

            #region InsuficienciaRenalCronica
            var scfInsuficienciaRenalCronica = new ServiceComponentFieldsList();
            scfInsuficienciaRenalCronica.v_ComponentFieldsId = Constants.COVID_INSUFICIENCIA_RENAL_ID;
            scfInsuficienciaRenalCronica.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfInsuficienciaRenalCronica.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.InsuficienciaRenalId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfInsuficienciaRenalCronica);
            #endregion

            #region Embarazo
            var scfEmbarazo = new ServiceComponentFieldsList();
            scfEmbarazo.v_ComponentFieldsId = Constants.COVID_EMBARAZO_ID;
            scfEmbarazo.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfEmbarazo.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.EmbarazoPuerperioId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfEmbarazo);
            #endregion

            #region EnfCardiovascular
            var scfEnfCardiovascular = new ServiceComponentFieldsList();
            scfEnfCardiovascular.v_ComponentFieldsId = Constants.COVID_ENF_CARDIO_ID;
            scfEnfCardiovascular.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfEnfCardiovascular.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.EnfCardioVascularId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfEnfCardiovascular);
            #endregion

            #region Asma
            var scfAsma = new ServiceComponentFieldsList();
            scfAsma.v_ComponentFieldsId = Constants.COVID_ASMA_ID;
            scfAsma.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfAsma.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.AsmaId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfAsma);
            #endregion

            #region Inmunosupresor
            var scfInmunosupresor = new ServiceComponentFieldsList();
            scfInmunosupresor.v_ComponentFieldsId = Constants.COVID_INMUNOSUPRESOR_ID;
            scfInmunosupresor.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfInmunosupresor.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.InmunosupresorId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfInmunosupresor);
            #endregion

            #region RiesgoPersonalSalud
            var scfRiesgoPersonalSalud = new ServiceComponentFieldsList();
            scfRiesgoPersonalSalud.v_ComponentFieldsId = Constants.COVID_PERSONAL_SALUD_ID;
            scfRiesgoPersonalSalud.v_ServiceComponentId = encuesta.ServiceComponentId;
            scfRiesgoPersonalSalud.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = encuesta.RiesgoPersonalSaludId == true ? "1" : "0" } };
            serviceComponentFieldsList.Add(scfRiesgoPersonalSalud);
            #endregion

            #region GlobalSession
            var sessione = (SessionModel)Session[Resources.Constante.SessionUsuario];
            var GlobalSession = new List<string>() { "6", "", sessione.SystemUserId.ToString() };
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

            var valoresEncuesta = oCovid19BL.EstadoEncuesta(new List<string> { laboratorio.ServiceComponentId});
            
            var estadoEncuesta = valoresEncuesta.Find(p => p.ServiceComponentId == laboratorio.ServiceComponentId) == null ? "pendiente" : "realizado";
            if (estadoEncuesta == "pendiente")
            {
                return Json("La encuesta no ha sido culminada", "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }

            #region FechaEjecucion
            var scfFechaEjecucion = new ServiceComponentFieldsList();
            scfFechaEjecucion.v_ComponentFieldsId = Constants.COVID_FECHA_EJECUCION_ID;
            scfFechaEjecucion.v_ServiceComponentId = laboratorio.ServiceComponentId;
            scfFechaEjecucion.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = laboratorio.FechaEjecucion } };
            serviceComponentFieldsList.Add(scfFechaEjecucion);
            #endregion

            #region ProcedenciaSolicitudId
            var scfProcedenciaSolicitudId = new ServiceComponentFieldsList();
            scfProcedenciaSolicitudId.v_ComponentFieldsId = Constants.COVID_PROCEDENCIA_SOLICITUD_ID;
            scfProcedenciaSolicitudId.v_ServiceComponentId = laboratorio.ServiceComponentId;
            scfProcedenciaSolicitudId.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = laboratorio.ProcedenciaSolicitudId } };
            serviceComponentFieldsList.Add(scfProcedenciaSolicitudId);
            #endregion

            #region ResultadoPrimeraPruebaId
            var scfResultadoPrimeraPruebaId = new ServiceComponentFieldsList();
            scfResultadoPrimeraPruebaId.v_ComponentFieldsId = Constants.COVID_RES_1_PRUEBA_ID;
            scfResultadoPrimeraPruebaId.v_ServiceComponentId = laboratorio.ServiceComponentId;
            scfResultadoPrimeraPruebaId.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = laboratorio.ResultadoPrimeraPruebaId } };
            serviceComponentFieldsList.Add(scfResultadoPrimeraPruebaId);
            #endregion

            #region ResultadoSegundaPruebaId
            var scfResultadoSegundaPruebaId = new ServiceComponentFieldsList();
            scfResultadoSegundaPruebaId.v_ComponentFieldsId = Constants.COVID_RES_2_PRUEBA_ID;
            scfResultadoSegundaPruebaId.v_ServiceComponentId = laboratorio.ServiceComponentId;
            scfResultadoSegundaPruebaId.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = laboratorio.ResultadoSegundaPruebaId } };
            serviceComponentFieldsList.Add(scfResultadoSegundaPruebaId);
            #endregion

            #region ClasificacionClinica
            var scfClasificacionClinica = new ServiceComponentFieldsList();
            scfClasificacionClinica.v_ComponentFieldsId = Constants.COVID_CLASIFICACION_CLINICA_ID;
            scfClasificacionClinica.v_ServiceComponentId = laboratorio.ServiceComponentId;
            scfClasificacionClinica.ServiceComponentFieldValues = new List<ServiceComponentFieldValuesList>() { new ServiceComponentFieldValuesList { v_Value1 = laboratorio.ClasificacionClinicaId } };
            serviceComponentFieldsList.Add(scfClasificacionClinica);
            #endregion

            #region GlobalSession
            var sessione = (SessionModel)Session[Resources.Constante.SessionUsuario];
            var GlobalSession = new List<string>() { "6", "", sessione.SystemUserId.ToString() };
            #endregion

            oServiceBL.AddServiceComponentValues(ref objOperationResult, serviceComponentFieldsList, GlobalSession, laboratorio.PersonId, laboratorio.ServiceComponentId);

            var sedeEmpresa = "----";
            if (laboratorio.ComponentId == Constants.COVID_ID)
            {
                #region Generar Reporte Covid
                try
                {
                    MergeExPDF _mergeExPDF = new MergeExPDF();
                    ReportDocument rp = new ReportDocument();
                    List<string> _filesNameToMerge = new List<string>();

                    var pstrRutaReportes = Server.MapPath("~/FichasCovid/");

                    byte[] firma = ObtenerFirmaDoctoraNancy();

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



                }
                catch (Exception ex)
                {

                    throw;
                }
                #endregion

                
            }
            return null;
        }

        private byte[] ObtenerFirmaParaFichaCovid19(string username)
        {
            var rutaFirmas = Server.MapPath("~/assets/Firmas/");

            var nombreFirma = username + ".jpg";            

            var buscarFirma = Directory.GetFiles(rutaFirmas, nombreFirma).FirstOrDefault();

            if (!String.IsNullOrEmpty(buscarFirma))            
                return Sigesoft.Common.Utils.FileToByteArray(rutaFirmas + nombreFirma);
            
            return Sigesoft.Common.Utils.FileToByteArray(rutaFirmas + "firmablanco.jpg");            
                        
        }

        private byte[] ObtenerFirmaDoctoraNancy()
        {
            var rutaFirmas = Server.MapPath("~/assets/Firmas/");
            return Sigesoft.Common.Utils.FileToByteArray(rutaFirmas + "firmaDoctoraNancy.JPG");
        }

        public JsonResult RegitrarTrabajador(entidadRegistrarTrabajador registroTrabajador)
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
            var sessione = (SessionModel)Session[Resources.Constante.SessionUsuario];
            var username = sessione.UserName;
            var GlobalSession = new List<string>() { sessione.NodeId.ToString(), "", sessione.SystemUserId.ToString(),"","",username };
            #endregion

            #region Obtener ProtocoloId
            //if (registroTrabajador.EmpresaId == Constants.EMPRESA_BACKUS_ID)
            //{
                //ProtocolId = Constants.EMPRESA_BACKUS_PROTOCOLO_PRUEBA_RAPIDA_COVID_ID;
                //OrganizationId = Constants.EMPRESA_BACKUS_ID;
                ProtocolId = sessione.ProtocolId;
                OrganizationId = sessione.EmpresaId;
                Sede = registroTrabajador.NombreSede;
            //}
            //else {
            //    OrganizationId = registroTrabajador.EmpresaId;
            //    ProtocolId = registroTrabajador.ProtocoloId;
            //    Sede = registroTrabajador.SedeNombre;
            
            //}
         
            #endregion

            #region Trabajador
            personDto objPersonDto = new personDto();
            objPersonDto = objPacientBL.GetPersonByNroDocument(ref objOperationResult, registroTrabajador.NroDocumento);
            if (objPersonDto != null)
            {
                objPersonDto.v_FirstName = registroTrabajador.Nombres;
                objPersonDto.v_FirstLastName = registroTrabajador.ApePaterno;
                objPersonDto.v_SecondLastName = registroTrabajador.ApeMaterno;
                objPersonDto.i_DocTypeId = int.Parse(registroTrabajador.TipoDocumento.ToString());
                objPersonDto.v_DocNumber = registroTrabajador.NroDocumento;
                objPersonDto.i_SexTypeId = int.Parse(registroTrabajador.Genero.ToString());
                objPersonDto.d_Birthdate = DateTime.Parse(registroTrabajador.FechaNacimiento.ToString());
                objPersonDto.v_Mail = registroTrabajador.Email;
                objPersonDto.v_TelephoneNumber = registroTrabajador.Celulares;
                objPersonDto.i_LevelOfId = -1;
                objPersonDto.i_MaritalStatusId = -1;
                objPersonDto.v_CurrentOccupation = registroTrabajador.Puesto;
                objPersonDto.v_AdressLocation = registroTrabajador.Direccion;

                objPacientBL.UpdatePacient(ref objOperationResult, objPersonDto, GlobalSession, objPersonDto.v_DocNumber, objPersonDto.v_DocNumber);
                PersonId = objPersonDto.v_PersonId;
            }
            else
            {
                objPersonDto = new personDto();
                objPersonDto.v_FirstName = registroTrabajador.Nombres;
                objPersonDto.v_FirstLastName = registroTrabajador.ApePaterno;
                objPersonDto.v_SecondLastName = registroTrabajador.ApeMaterno;
                objPersonDto.i_DocTypeId = int.Parse(registroTrabajador.TipoDocumento.ToString());
                objPersonDto.v_DocNumber = registroTrabajador.NroDocumento;
                objPersonDto.i_SexTypeId = int.Parse(registroTrabajador.Genero.ToString());
                objPersonDto.d_Birthdate = DateTime.Parse(registroTrabajador.FechaNacimiento.ToString());
                objPersonDto.v_Mail = registroTrabajador.Email;
                objPersonDto.v_TelephoneNumber = registroTrabajador.Celulares;
                objPersonDto.i_LevelOfId = -1;
                objPersonDto.i_MaritalStatusId = -1;
                objPersonDto.v_CurrentOccupation = registroTrabajador.Puesto;
                objPersonDto.v_AdressLocation = registroTrabajador.Direccion;

                PersonId = objPacientBL.AddPacient(ref objOperationResult, objPersonDto, GlobalSession);
            }
            #endregion

            #region EntidadCalendar

            DateTime dateTime = DateTime.Now;
            //if (!DateTime.TryParse(registroTrabajador.FechaServicio, out dateTime))
            //    return Json("FECHA INVALIDA", JsonRequestBehavior.AllowGet);  

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

            _objCalendarBL.AddSheduleWEB(ref objOperationResult, objCalendarDto, GlobalSession, ProtocolId, PersonId, objCalendarDto.i_ServiceId.Value, "Nuevo", OrganizationId, Sede, dateTime, registroTrabajador.TipoEmpresaCovidId);
            return null;
        }

        public JsonResult GetDatosHeadCount(string dni)
        {
            try
            {
                Covid19BL oCovid19BL = new Covid19BL();

                var data = oCovid19BL.GetHcByDni(dni);

                return Json(data, JsonRequestBehavior.AllowGet);  
            }
            catch (Exception ex)
            {
                throw;
            }           
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
                string subject = "Resultado Covid-19";
                bool enableSsl = Convert.ToBoolean(int.Parse(configEmail[3].Value1));
                string message = string.Format("Buenos días, el resultado del trabajador {0} , es: {1}", trabajadorData.Trabajador, resultadoCovid);
                string ruta = pstrRutaReportes;
                var atachFiles = new List<string>();
                string atachFile = ruta + serviceId + ".pdf";
                atachFiles.Add(atachFile);

                var sessione = (SessionModel)Session[Resources.Constante.SessionUsuario];

                var nodeId = sessione.NodeId;

                var enviarA = "";

                //CONO NORTE
                if (nodeId == 100)
                {
                    subject = "Resultado Covid-19 NODO NORTE - SEDE " + sedeEmpresa;
                    enviarA = "BLANCA.MORIN@AB-INBEV.COM;Antoinette.Lostaunaufelix@ab-inbev.com;emiko.mori@ab-inbev.com; usebackuslima.tecnica@saluslaboris.com.pe;marlene.pachas@saluslaboris.com.pe;jessica.oblitas@saluslaboris.com.pe";
                }
                //RIMAC
                else if (nodeId == 101)
                {
                    subject = "Resultado Covid-19 NODO RIMAC - SEDE " + sedeEmpresa;
                    enviarA = "ALEX.LACOTERA@BACKUS.SABMILLER.COM;Lostaunaufelix@ab-inbev.com; emiko.mori@ab-inbev.com;usebackuslima.tecnica@saluslaboris.com.pe;marlene.pachas@saluslaboris.com.pe;jessica.oblitas@saluslaboris.com.pe";
                }
                //ATE
                else if (nodeId == 102)
                {
                    subject = "Resultado Covid-19 NODO ATE - SEDE " + sedeEmpresa;
                    enviarA = "JOSE.ALIAGA@AB-INBEV.COM;Antoinette.Lostaunaufelix@ab-inbev.com;emiko.mori@ab-inbev.com;usebackuslima.tecnica@saluslaboris.com.pe;marlene.pachas@saluslaboris.com.pe;jessica.oblitas@saluslaboris.com.pe";

                }
                //CONO SUR
                else if (nodeId == 103)
                {
                    subject = "Resultado Covid-19 NODO CONO SUR - SEDE " + sedeEmpresa;
                    enviarA = "JOSE.RISCO@AB-INBEV.COM;Antoinette.Lostaunaufelix@ab-inbev.com;emiko.mori@ab-inbev.com;usebackuslima.tecnica@saluslaboris.com.pe;marlene.pachas@saluslaboris.com.pe;jessica.oblitas@saluslaboris.com.pe";
                }
                //CALLAO
                else if (nodeId == 104)
                {
                    subject = "Resultado Covid-19 NODO CALLAO - SEDE " + sedeEmpresa;
                    enviarA = "KENNY.BOLIVAR@AB-INBEV.COM;Antoinette.Lostaunaufelix@ab-inbev.com;emiko.mori@ab-inbev.com;usebackuslima.tecnica@saluslaboris.com.pe;marlene.pachas@saluslaboris.com.pe;jessica.oblitas@saluslaboris.com.pe";
                }
                //VEGUETA
                else if (nodeId == (int)SedeBackus.Vegueta)
                {
                    subject = "Resultado Covid-19 NODO VEGUETA - SEDE " + sedeEmpresa;
                    enviarA = "BLANCA.MORIN@AB-INBEV.COM;GLADYS.FLORES@ab-inbev.com;emiko.mori@ab-inbev.com;usebackuslima.tecnica@saluslaboris.com.pe;marlene.pachas@saluslaboris.com.pe;jessica.oblitas@saluslaboris.com.pe";
                }   
                //RUTA 2
                else if (nodeId == (int)SedeBackus.TingoMaria)
                {
                    subject = "Resultado Covid-19 NODO TINGO MARIA - SEDE " + sedeEmpresa;
                    enviarA = "RONALD.MARTINEZ@AB-INBEV.COM;tjauregui@wellcom.com.pe;Vania.Guerrero@ab-inbev.com;usebackuslima.tecnica@saluslaboris.com.pe;marlene.pachas@saluslaboris.com.pe;jessica.oblitas@saluslaboris.com.pe";
                }
                //RUTA 3
                else if (nodeId == (int)SedeBackus.Nazca)
                {
                    subject = "Resultado Covid-19 NODO NAZCA - SEDE " + sedeEmpresa;
                    enviarA = "Pedro.moritenorio@backus.sabmiller.com;GLADYS.FLORES@ab-inbev.com;Vania.Guerrero@ab-inbev.com;usebackuslima.tecnica@saluslaboris.com.pe;marlene.pachas@saluslaboris.com.pe;jessica.oblitas@saluslaboris.com.pe";
                }
                else if (nodeId == (int)SedeBackus.Trujillo)
                {
                    subject = "Resultado Covid-19 NODO TRUJILLO - SEDE " + sedeEmpresa;
                    enviarA = "JOSE.BOZZETA@AB-INBEV.COM;Graciela.Ferrel@ab-inbev.com;Vania.Guerrero@ab-inbev.com;usebackuslima.tecnica@saluslaboris.com.pe;marlene.pachas@saluslaboris.com.pe;jessica.oblitas@saluslaboris.com.pe";
                }
                else if (nodeId == (int)SedeBackus.Chimbote)
                {
                    subject = "Resultado Covid-19 NODO CHIMBOTE - SEDE " + sedeEmpresa;
                    enviarA = "JOSE.BOZZETA@AB-INBEV.COM;Graciela.Ferrel@ab-inbev.com;Vania.Guerrero@ab-inbev.com;usebackuslima.tecnica@saluslaboris.com.pe;marlene.pachas@saluslaboris.com.pe;jessica.oblitas@saluslaboris.com.pe";
                }
                else if (nodeId == (int)SedeBackus.Piura)
                {
                    subject = "Resultado Covid-19 NODO PIURA - SEDE " + sedeEmpresa;
                    enviarA = "JOSE.BOZZETA@AB-INBEV.COM;Graciela.Ferrel@ab-inbev.com;Vania.Guerrero@ab-inbev.com;usebackuslima.tecnica@saluslaboris.com.pe;marlene.pachas@saluslaboris.com.pe;jessica.oblitas@saluslaboris.com.pe";
                }
                else if (nodeId == (int)SedeBackus.Tumbes)
                {
                    subject = "Resultado Covid-19 NODO TUMBES - SEDE " + sedeEmpresa;
                    enviarA = "JOSE.BOZZETA@AB-INBEV.COM;Graciela.Ferrel@ab-inbev.com;Vania.Guerrero@ab-inbev.com;usebackuslima.tecnica@saluslaboris.com.pe;marlene.pachas@saluslaboris.com.pe;jessica.oblitas@saluslaboris.com.pe";
                }
                else if (nodeId == (int)SedeBackus.Talara)
                {
                    subject = "Resultado Covid-19 NODO TALARA - SEDE " + sedeEmpresa;
                    enviarA = "JOSE.BOZZETA@AB-INBEV.COM;Graciela.Ferrel@ab-inbev.com;Vania.Guerrero@ab-inbev.com;usebackuslima.tecnica@saluslaboris.com.pe;marlene.pachas@saluslaboris.com.pe;jessica.oblitas@saluslaboris.com.pe";
                }
                else if (nodeId == (int)SedeBackus.Huanuco)
                {
                    subject = "Resultado Covid-19 NODO HUANUCO - SEDE " + sedeEmpresa;
                    enviarA = "RONALD.MARTINEZ@AB-INBEV.COM;tjauregui@wellcom.com.pe;Vania.Guerrero@ab-inbev.com;usebackuslima.tecnica@saluslaboris.com.pe;marlene.pachas@saluslaboris.com.pe;jessica.oblitas@saluslaboris.com.pe";
                }
                else if (nodeId == (int)SedeBackus.Chanchamayo)
                {
                    subject = "Resultado Covid-19 NODO CHANCHAMAYO - SEDE " + sedeEmpresa;
                    enviarA = "RONALD.MARTINEZ@AB-INBEV.COM;tjauregui@wellcom.com.pe;Vania.Guerrero@ab-inbev.com;usebackuslima.tecnica@saluslaboris.com.pe;marlene.pachas@saluslaboris.com.pe;jessica.oblitas@saluslaboris.com.pe";
                }
                else if (nodeId == (int)SedeBackus.Ica)
                {
                    subject = "Resultado Covid-19 NODO ICA - SEDE " + sedeEmpresa;
                    enviarA = "Pedro.moritenorio@backus.sabmiller.com;GLADYS.FLORES@ab-inbev.com;Vania.Guerrero@ab-inbev.com;usebackuslima.tecnica@saluslaboris.com.pe;marlene.pachas@saluslaboris.com.pe;jessica.oblitas@saluslaboris.com.pe";
                }
                else if (nodeId == (int)SedeBackus.Canete)
                {
                    subject = "Resultado Covid-19 NODO CAÑETE - SEDE " + sedeEmpresa;
                    enviarA = "Pedro.moritenorio@backus.sabmiller.com;GLADYS.FLORES@ab-inbev.com;Vania.Guerrero@ab-inbev.com;usebackuslima.tecnica@saluslaboris.com.pe;marlene.pachas@saluslaboris.com.pe;jessica.oblitas@saluslaboris.com.pe";
                }
                else if (nodeId == (int)SedeBackus.satipo)
                {
                    subject = "Resultado Covid-19 NODO SATIPO - SEDE " + sedeEmpresa;
                    enviarA = "RONALD.MARTINEZ@AB-INBEV.COM;tjauregui@wellcom.com.pe;Vania.Guerrero@ab-inbev.com;usebackuslima.tecnica@saluslaboris.com.pe;marlene.pachas@saluslaboris.com.pe;jessica.oblitas@saluslaboris.com.pe";
                }

                else if (nodeId == (int)SedeBackus.PLANTA_AREQUIPA)
                {
                    subject = "Resultado Covid-19 NODO PLANTA AREQUIPA - SEDE " + sedeEmpresa;
                    enviarA = "Alejandra.Cateriano@ab-inbev.com;Flor.Vargas@pe.ab-inbev.com";
                }


                var copiaCc = correoTrabajador;
                var copiaBcc = "francisco.collantes@saluslaboris.com.pe";
                Sigesoft.Common.Utils.SendMessage(smtp, port, enableSsl, true, from, fromPassword, enviarA, copiaCc, copiaBcc, subject, message, atachFiles);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string ObtenerSedeDeUsuarioTemperal(int nodeId)
        {
            var value = "";
            if (nodeId == 100)
            {
                value = "|| CONONORTE";
            }
            else if (nodeId == 101)
            {
                value = "|| RIMAC";
            }else if (nodeId == 102)
            {
                value = "|| ATE";
            }
            else if (nodeId == 103)
            {
                value = "|| CONOSUR";
            }
            else if (nodeId == 104)
            {
                value = "|| CALLAO";
            }
            else if (nodeId == (int)SedeBackus.Vegueta)
            {
                value = "|| VEGUETA";
            }



            else if (nodeId == (int)SedeBackus.TingoMaria)
            {
                value = "|| TINGO MARIA";
            }
            else if (nodeId == (int)SedeBackus.Nazca)
            {
                value = "|| NAZCA";
            }
            else if (nodeId == (int)SedeBackus.Trujillo)
            {
                value = "|| TRUJILLO";
            }
            else if (nodeId == (int)SedeBackus.Chimbote)
            {
                value = "|| CHIMBOTE";
            }
            else if (nodeId == (int)SedeBackus.Piura)
            {
                value = "|| PIURA";
            }
            else if (nodeId == (int)SedeBackus.Tumbes)
            {
                value = "|| TUMBES";
            }
            else if (nodeId == (int)SedeBackus.Chanchamayo)
            {
                value = "|| CHANCHAMAYO";
            }
            else if (nodeId == (int)SedeBackus.Canete)
            {
                value = "|| CAÑETE";
            }
            else if (nodeId == (int)SedeBackus.Talara)
            {
                value = "|| TALARA";
            }
            else if (nodeId == (int)SedeBackus.satipo)
            {
                value = "|| SATIPO";
            }
            else if (nodeId == (int)SedeBackus.Huanuco)
            {
                value = "|| HUANUCO";
            }
            else if (nodeId == (int)SedeBackus.Ica)
            {
                value = "|| ICA";
            }

            else if (nodeId == 108)
            {
                value = "|| CHICLAYO";
            }

            else if (nodeId == (int)SedeBackus.PLANTA_AREQUIPA)
            {
                value = "|| PLANTA - AREQUIPA";
            }

            return value;
        }

        [HttpGet]
        public JsonResult EsEditable(string serviceComponentId)
        {
            OperationResult objOperationResult = new OperationResult();
            Covid19BL _objCalendarBL = new Covid19BL();

            var obj = _objCalendarBL.EsEditable(serviceComponentId);

            return Json(obj, JsonRequestBehavior.AllowGet);  
        }

        public List<SedesBackus> ObtenerSedesPorEmpresa(string empresaId)
        {
            Covid19BL _objCalendarBL = new Covid19BL();
            var sedes = _objCalendarBL.ObtenerSedesBackus(empresaId);

            return sedes;
        }

        public JsonResult ObtenerEmpresas(string nombre) {
            try
            {
                OrganizationBL oOrganizationBL = new OrganizationBL();

                var data = oOrganizationBL.GetOrganizationPorNombre(nombre);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }         
        }

        public JsonResult ObtenerSedes(string empresaId, string nombre)
        {
            try
            {
                OrganizationBL oOrganizationBL = new OrganizationBL();

                var data = oOrganizationBL.GetLocationPorNombre(empresaId, nombre);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public JsonResult ObtenerProtocolos(string empresaId, string nombre)
        {
            try
            {
                OrganizationBL oOrganizationBL = new OrganizationBL();

                var data = oOrganizationBL.GetProtocolPorNombre(empresaId, nombre);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void SetearEmpresaEnSession(string empresaId) {

            var sessione = (SessionModel)Session[Resources.Constante.SessionUsuario];

            var sessionModel = new SessionModel();
            sessionModel.UserName = sessione.UserName;
            sessionModel.FullName = "USUARIO TEMPORAL";
            sessionModel.SystemUserId = 11;
            sessionModel.NodeId = sessione.NodeId;
            sessionModel.Nodo = sessione.Nodo;
            sessionModel.EmpresaId = empresaId;
            //FormsAuthentication.SetAuthCookie(sessionModel.UserName, false);
            MvcSigesoft.Security.HttpSessionContext.SetAccount(sessionModel);

        }
    }
}
