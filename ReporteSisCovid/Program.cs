using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReporteSisCovid
{
    class Program
    {
        static void Main(string[] args)
        {
            var startDate = DateTime.Now;
            var EndDate = DateTime.Now;

            Console.WriteLine("Ingresar Fecha de inicio (2020-11-30)");
            var readLine1 = Console.ReadLine();
            if (!string.IsNullOrEmpty(readLine1))
                startDate = DateTime.Parse(readLine1);            
            
            Console.WriteLine("Ingresar Fecha de fin (2020-11-30)");
            var readLine2 = Console.ReadLine();
            if (!string.IsNullOrEmpty(readLine2))
                EndDate = DateTime.Parse(readLine2);

            Console.WriteLine("procesando...");

            var Services = new Services();
            var Covid19ServiceValues = new Covid19ExamValues();

            var getServicesList = Services.ServiceList(startDate, EndDate);

            //getServicesList = getServicesList.FindAll(p => p.EmpresaCliente == "").ToList();
            var listForExcel =  new List<ListServiceValuesForExcel>();
            foreach (var service in getServicesList)
            {
                var serviceData = service;
                var serviceValues = Covid19ServiceValues.ListServiceValues(service.ServiceId, service.ComponentId);
                
                listForExcel.Add(BuilServiceForExcel(serviceData, serviceValues));
                
            }

            var oExcelFile = new ExcelFile();
            string path = Sigesoft.Common.Utils.GetApplicationConfigValue("rutaReporte").ToString();
            oExcelFile.GenerateExcel(path, listForExcel);

            Console.WriteLine("terminó...");
            Console.ReadLine();
        }

        public static ListServiceValuesForExcel BuilServiceForExcel(Services serviceData, List<Covid19ExamValues> covid19ExamValues) 
        {
            var oListServiceValuesForExcel = new ListServiceValuesForExcel();
            oListServiceValuesForExcel.TipoDocumento = serviceData.DocumentType;
            oListServiceValuesForExcel.NumeroDocumento = serviceData.DocumentNumber;
            oListServiceValuesForExcel.ApellidoPaterno = serviceData.FirstLastName;
            oListServiceValuesForExcel.ApellidoMaterno = serviceData.SecondLastName;
            oListServiceValuesForExcel.Nombres = serviceData.FirstName;
            oListServiceValuesForExcel.FechaNacimiento = serviceData.BirthDate == null ? "" : serviceData.BirthDate.ToString();
            oListServiceValuesForExcel.Sexo = serviceData.SexType;
            oListServiceValuesForExcel.TipoSeguro = "....";
            oListServiceValuesForExcel.OtroTipoSeguro = "....";
            oListServiceValuesForExcel.Ubigueo = "....";

            if (serviceData.ComponentId == Sigesoft.Common.Constants.COVID_ID)
            {
                oListServiceValuesForExcel.PersonalSalud = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_DOMICILIO_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_DOMICILIO_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_DOMICILIO_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.FechaEjecucion = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_FECHA_EJECUCION_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_FECHA_EJECUCION_ID).Value1;
                oListServiceValuesForExcel.Procedencia = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_PROCEDENCIA_SOLICITUD_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_PROCEDENCIA_SOLICITUD_ID).Value1Name;
                oListServiceValuesForExcel.TipoResultado1 = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_RES_1_PRUEBA_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_RES_1_PRUEBA_ID).Value1Name;
                oListServiceValuesForExcel.TipoResultado2 = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_RES_2_PRUEBA_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_RES_2_PRUEBA_ID).Value1 == null? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_RES_2_PRUEBA_ID).Value1Name;
                oListServiceValuesForExcel.Mayor60 = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_MAYOR_60_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_MAYOR_60_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_MAYOR_60_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.HipertensionArterial = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_HIPERTENCION_ARTERIAL_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_HIPERTENCION_ARTERIAL_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_HIPERTENCION_ARTERIAL_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.EnfermedadCardiovascular = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_ENF_PULMONAR_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_ENF_PULMONAR_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_ENF_PULMONAR_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.Diabetes = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_DIABETES_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_DIABETES_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_DIABETES_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.Obesidad = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_OBESIDAD_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_OBESIDAD_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_OBESIDAD_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.Asma = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_ASMA_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_ASMA_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_ASMA_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.EnfPulmonarCronica = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_ENF_PULMONAR_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_ENF_PULMONAR_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_ENF_PULMONAR_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.InsuficienciaRenalCronica = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_INSUFICIENCIA_RENAL_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_INSUFICIENCIA_RENAL_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_INSUFICIENCIA_RENAL_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.EnfTratamientoInmunosupresor = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_INMUNOSUPRESOR_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_INMUNOSUPRESOR_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_INMUNOSUPRESOR_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.Cancer = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_CANCER_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_CANCER_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_CANCER_ID).Value1 == "0" ? "NO" :"SÍ";
                oListServiceValuesForExcel.EmbarazoPuerperio = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_EMBARAZO_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_EMBARAZO_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_EMBARAZO_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.PersonalSalud = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_PERSONAL_SALUD_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_PERSONAL_SALUD_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.COVID_PERSONAL_SALUD_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.Empleador = serviceData.Empleador;
                oListServiceValuesForExcel.EmpresaPrincipal = serviceData.EmpresaPrincipal == null ? serviceData.EmpresaCliente : serviceData.EmpresaPrincipal;
                oListServiceValuesForExcel.Sede = serviceData.Sede;
                oListServiceValuesForExcel.Tecnico = serviceData.Tecnico;
                oListServiceValuesForExcel.TipoExamen = "PRUEBA RÁPIDA";
            }
            else if (serviceData.ComponentId == Sigesoft.Common.Constants.ANTIGENOS_ID)
            {
                oListServiceValuesForExcel.PersonalSalud = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_DOMICILIO_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_DOMICILIO_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_DOMICILIO_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.FechaEjecucion = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_FECHA_EJECUCION_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_FECHA_EJECUCION_ID).Value1;
                oListServiceValuesForExcel.Procedencia = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_PROCEDENCIA_SOLICITUD_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_PROCEDENCIA_SOLICITUD_ID).Value1Name;
                oListServiceValuesForExcel.TipoResultado1 = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_RES_1_PRUEBA_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_RES_1_PRUEBA_ID).Value1Name;
                oListServiceValuesForExcel.TipoResultado2 = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_RES_2_PRUEBA_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_RES_2_PRUEBA_ID).Value1 == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_RES_2_PRUEBA_ID).Value1Name;
                oListServiceValuesForExcel.Mayor60 = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_MAYOR_60_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_MAYOR_60_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_MAYOR_60_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.HipertensionArterial = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_HIPERTENCION_ARTERIAL_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_HIPERTENCION_ARTERIAL_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_HIPERTENCION_ARTERIAL_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.EnfermedadCardiovascular = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_ENF_PULMONAR_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_ENF_PULMONAR_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_ENF_PULMONAR_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.Diabetes = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_DIABETES_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_DIABETES_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_DIABETES_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.Obesidad = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_OBESIDAD_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_OBESIDAD_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_OBESIDAD_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.Asma = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_ASMA_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_ASMA_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_ASMA_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.EnfPulmonarCronica = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_ENF_PULMONAR_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_ENF_PULMONAR_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_ENF_PULMONAR_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.InsuficienciaRenalCronica = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_INSUFICIENCIA_RENAL_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_INSUFICIENCIA_RENAL_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_INSUFICIENCIA_RENAL_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.EnfTratamientoInmunosupresor = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_INMUNOSUPRESOR_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_INMUNOSUPRESOR_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_INMUNOSUPRESOR_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.Cancer = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_CANCER_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_CANCER_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_CANCER_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.EmbarazoPuerperio = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_EMBARAZO_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_EMBARAZO_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_EMBARAZO_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.PersonalSalud = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_PERSONAL_SALUD_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_PERSONAL_SALUD_ID).Value1 == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.ANTIGENO_PERSONAL_SALUD_ID).Value1 == "0" ? "NO" : "SÍ";
                oListServiceValuesForExcel.Empleador = serviceData.Empleador;
                oListServiceValuesForExcel.EmpresaPrincipal = serviceData.EmpresaPrincipal == null ? serviceData.EmpresaCliente : serviceData.EmpresaPrincipal;
                oListServiceValuesForExcel.Sede = serviceData.Sede;
                oListServiceValuesForExcel.Tecnico = serviceData.Tecnico;
                oListServiceValuesForExcel.TipoExamen = "PRUEBA ANTÍGENOS";
            }
            else if (serviceData.ComponentId == Sigesoft.Common.Constants.CERTIFICADO_COVID_ID)
            {
                oListServiceValuesForExcel.PersonalSalud = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_DOMICILIO_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_DOMICILIO_ID).Value1Name;
                oListServiceValuesForExcel.FechaEjecucion = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_FECHA_EJECUCION_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_FECHA_EJECUCION_ID).Value1;
                oListServiceValuesForExcel.Procedencia = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_PROCEDENCIA_SOLICITUD_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_PROCEDENCIA_SOLICITUD_ID).Value1Name;
                oListServiceValuesForExcel.TipoResultado1 = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_RES_1_PRUEBA_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_RES_1_PRUEBA_ID).Value1Name;
                oListServiceValuesForExcel.TipoResultado2 = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_RES_2_PRUEBA_ID) == null || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_RES_2_PRUEBA_ID).Value1 == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_RES_2_PRUEBA_ID).Value1Name;
                oListServiceValuesForExcel.Mayor60 = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_MAYOR_60_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_MAYOR_60_ID).Value1;
                oListServiceValuesForExcel.HipertensionArterial = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_HIPERTENCION_ARTERIAL_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_HIPERTENCION_ARTERIAL_ID).Value1;
                oListServiceValuesForExcel.EnfermedadCardiovascular = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_ENF_PULMONAR_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_ENF_PULMONAR_ID).Value1;
                oListServiceValuesForExcel.Diabetes = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_DIABETES_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_DIABETES_ID).Value1;
                oListServiceValuesForExcel.Obesidad = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_OBESIDAD_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_OBESIDAD_ID).Value1;
                oListServiceValuesForExcel.Asma = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_ASMA_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_ASMA_ID).Value1;
                oListServiceValuesForExcel.EnfPulmonarCronica = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_ENF_PULMONAR_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_ENF_PULMONAR_ID).Value1;
                oListServiceValuesForExcel.InsuficienciaRenalCronica = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_INSUFICIENCIA_RENAL_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_INSUFICIENCIA_RENAL_ID).Value1;
                oListServiceValuesForExcel.EnfTratamientoInmunosupresor = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_INMUNOSUPRESOR_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_INMUNOSUPRESOR_ID).Value1;
                oListServiceValuesForExcel.Cancer = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_CANCER_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_CANCER_ID).Value1;
                oListServiceValuesForExcel.EmbarazoPuerperio = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_EMBARAZO_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_EMBARAZO_ID).Value1;
                oListServiceValuesForExcel.PersonalSalud = covid19ExamValues.Count == 0 || covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_PERSONAL_SALUD_ID) == null ? "" : covid19ExamValues.Find(p => p.ComponentFieldId == Sigesoft.Common.Constants.CERTIFICADO_COVID_PERSONAL_SALUD_ID).Value1Name;
                oListServiceValuesForExcel.Empleador = serviceData.Empleador;
                oListServiceValuesForExcel.EmpresaPrincipal = serviceData.EmpresaPrincipal == null ? serviceData.EmpresaCliente : serviceData.EmpresaPrincipal;
                oListServiceValuesForExcel.Sede = serviceData.Sede;
                oListServiceValuesForExcel.Tecnico = serviceData.Tecnico;
            }
            else
            {
                oListServiceValuesForExcel.PersonalSalud = "NO ES COVID";
                oListServiceValuesForExcel.FechaEjecucion = "NO ES COVID";
                oListServiceValuesForExcel.Procedencia = "NO ES COVID";
                oListServiceValuesForExcel.TipoResultado1 = "NO ES COVID";
                oListServiceValuesForExcel.TipoResultado2 = "NO ES COVID";
                oListServiceValuesForExcel.Mayor60 = "NO ES COVID";
                oListServiceValuesForExcel.HipertensionArterial = "NO ES COVID";
                oListServiceValuesForExcel.EnfermedadCardiovascular = "NO ES COVID";
                oListServiceValuesForExcel.Diabetes = "NO ES COVID";
                oListServiceValuesForExcel.Obesidad = "NO ES COVID";
                oListServiceValuesForExcel.Asma = "NO ES COVID";
                oListServiceValuesForExcel.EnfPulmonarCronica = "NO ES COVID";
                oListServiceValuesForExcel.InsuficienciaRenalCronica = "NO ES COVID";
                oListServiceValuesForExcel.EnfTratamientoInmunosupresor = "NO ES COVID";
                oListServiceValuesForExcel.Cancer = "NO ES COVID";
                oListServiceValuesForExcel.EmbarazoPuerperio = "NO ES COVID";
                oListServiceValuesForExcel.PersonalSalud = "NO ES COVID";
                oListServiceValuesForExcel.Empleador = "NO ES COVID";
                oListServiceValuesForExcel.EmpresaPrincipal = "NO ES COVID";
                oListServiceValuesForExcel.Sede = serviceData.Sede;
                oListServiceValuesForExcel.Tecnico = serviceData.Tecnico;
            }
            

            return oListServiceValuesForExcel;
        }

    }
}

 //value.ComponentFieldId = Constants.DOMICILIO_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.tipoDomicilio.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.GEOLOCALIZACION_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.geolocalizacion.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.PROFESION_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.profesion.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.ES_PERSONAL_SALUD_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.esPersonalSaludCbo.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.TIENE_SINTOMAS_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.tieneSintomas.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.INICIO_SINTOMAS_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.inicioSintomas.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.CEFALEA_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.cefalea == null ? null : item.cefalea.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.CONGESTION_NASAL_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.congestionNasal == null ? null : item.congestionNasal.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.DIARREA_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.diarrea == null ? null : item.diarrea.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.DIFIC_RESPIRA_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.dificultadRespiratoria == null ? null : item.dificultadRespiratoria.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.DOLOR_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.dolor == null ? null : item.dolor.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.DOLOR_GARGANTA_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.dolorGarganta == null ? null : item.dolorGarganta.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.FIEBRE_ESCALOFRIO_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.fiebreEscalofrio == null ? null : item.fiebreEscalofrio.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.IRRITABILIDAD_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.irritabilidadConfusion == null ? null : item.irritabilidadConfusion.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.MALESTAR_GENERAL_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.malestarGeneral == null ? null : item.malestarGeneral.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.NAUSEAS_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.nauseasVomitos == null ? null : item.nauseasVomitos.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.OTROS_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.otrosSintomas == null ? "": item.otrosSintomas.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.TOS_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.tos == null ? null : item.tos.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.ABDOMINAL_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.dolorAbdominal == null ? null : item.dolorAbdominal.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.ARTICULACIONES_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.dolorArticulaciones == null ? null : item.dolorArticulaciones.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.MUSCULAR_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.dolorMuscular == null ? null : item.dolorMuscular.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.PECHO_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.dolorPecho == null ? null : item.dolorPecho.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.OTROS_SINTOMAS_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.otrosSintomas == null ? "": item.otrosSintomas.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.CLASIFICACION_CLINICA_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.clasificacionClinica.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.FECHA_EJECUCION_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.fechaEjecucion.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.PROCEDENCIA_SOLICITUD_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.procedenciaSolicitud.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.RES_1_PRUEBA_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.resultadoPrimeraPrueba.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.RES_2_PRUEBA_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.resultadoSegundaPrueba.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.ASMA_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.asma == null ? null : item.asma.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.CANCER_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.cancer == null ? null : item.cancer.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.DIABETES_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.diabetes == null ? null : item.diabetes.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.EMBARAZO_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.embarazo == null ? null : item.embarazo.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.ENF_CARDIO_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.enfCardiovascular == null ? null : item.enfCardiovascular.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.INMUNOSUPRESOR_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.enfInmunosupresor == null ? null : item.enfInmunosupresor.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.ENF_PULMONAR_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.enfPulmonarCronica == null ? null : item.enfPulmonarCronica.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.HIPERTENCION_ARTERIAL_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.hipertensionArterial == null ? null : item.hipertensionArterial.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.INSUFICIENCIA_RENAL_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.insuficienciaCronica == null ? null : item.insuficienciaCronica.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.MAYOR_60_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.mayor65 == null ? null : item.mayor65.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.OBESIDAD_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.obesidad == null ? null : item.obesidad.ToString();
 //           list.Add(value);

 //           value = new ServiceValues();
 //           value.ComponentFieldId = Constants.PERSONAL_SALUD_ID;
 //           value.ServiceComponentId = serviceComponentId;
 //           value.Value1 = item.personalSalud == null ? null : item.personalSalud.ToString();
 //           list.Add(value);
