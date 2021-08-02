using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using NetPdf;
using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;
using Sigesoft.Node.WinClient.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace SoporteSigesoft
{
   public class ProcesoEnvioCorreos
    {
        public DateTime? _fecha { get; set; }
        string _rutaFichasCovid = Sigesoft.Common.Utils.GetApplicationConfigValue("rutaFichasCovid");

        public ProcesoEnvioCorreos(DateTime? fecha)
        {
            _fecha = fecha;
        }
       
        public string Regularizar() {            
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            var servicios = new List<DatosServicio>();           

            #region Listar Servicios         

            if (_fecha == null) _fecha = DateTime.Parse("20-07-2020");

            var posiblesResultados = (from A in dbContext.systemparameter where A.i_GroupId == 303 select A).ToList();

            var valores = (from A in dbContext.service
                           join B in dbContext.servicecomponent on A.v_ServiceId equals B.v_ServiceId
                           join C in dbContext.servicecomponentfields on B.v_ServiceComponentId equals C.v_ServiceComponentId
                           join D in dbContext.servicecomponentfieldvalues on C.v_ServiceComponentFieldsId equals D.v_ServiceComponentFieldsId
                           where (A.d_ServiceDate >= _fecha)
                           && A.i_IsDeleted == 0
                           && B.i_IsRequiredId == 1
                           && (B.v_ComponentId == Constants.CERTIFICADO_COVID_ID ||B.v_ComponentId == Constants.COVID_ID)
                           && (C.v_ComponentFieldId == Constants.CERTIFICADO_COVID_RES_1_PRUEBA_ID || C.v_ComponentFieldId == Constants.COVID_RES_1_PRUEBA_ID)
                           select new
                           {
                               ServiceId = A.v_ServiceId,
                               Value1 = D.v_Value1,
                               ComponentId = B.v_ComponentId
                           }).ToList();

            servicios = (from A in dbContext.service
                            join B in dbContext.person on A.v_PersonId equals B.v_PersonId
                            join C in dbContext.servicecomponent on A.v_ServiceId equals C.v_ServiceId
                            join D in dbContext.protocol on A.v_ProtocolId equals D.v_ProtocolId
                            join E in dbContext.organization on D.v_CustomerOrganizationId equals E.v_OrganizationId
                            where A.i_IsDeleted == 0
                            && (A.d_ServiceDate >= _fecha)
                            select new DatosServicio
                            {
                                ServiceId = A.v_ServiceId,
                                ApellidoPaterno = B.v_FirstLastName,
                                ApellidoMaterno = B.v_SecondLastName,
                                Nombres = B.v_FirstName,                                
                                Documento = B.v_DocNumber,
                                ComponentId = C.v_ComponentId,
                                FechaServicio = A.d_ServiceDate.Value,
                                FechaInsert =  A.d_InsertDate.Value,
                                TipoFormato = E.i_TipoFormatoCovid19,
                                Sede = A.v_Sede,
                                Tecnico = A.TecnicoCovid
                            }).ToList();

            var serviciosCovid19 = new List<DatosServicio>();
            serviciosCovid19 = servicios.FindAll(p => p.ComponentId == Constants.COVID_ID).ToList();
            serviciosCovid19 = serviciosCovid19.GroupBy(p => p.ServiceId).Select(s => s.First()).OrderBy(f => f.FechaInsert).ToList();

            var serviciosCertificadoCovid19 = new List<DatosServicio>();
            serviciosCertificadoCovid19 = servicios.FindAll(p => p.ComponentId == Constants.CERTIFICADO_COVID_ID).ToList();
            serviciosCertificadoCovid19 = serviciosCertificadoCovid19.GroupBy(p => p.ServiceId).Select(s => s.First()).OrderBy(f => f.FechaInsert).ToList();

            #endregion

            #region Buscar en servidor Covid19
            var countEncontrados = 0;
            var countNoEncontrados = 0;
            var negativos = 0;
            var NoValido = 0;
            var IgMPositivo = 0;
            var IgGPositivo = 0;
            var IgMeIfGPositivo = 0;
            var NoRealizado = 0;
            var SinResultados = 0;

            foreach (var item in serviciosCovid19)
            {
                var resultadoPrimeraPrueba = "En espera";
                var tipoExamen="--";
                var registro = valores.Find(p => p.ServiceId == item.ServiceId);

                if (registro != null)
                {
                    var valorResultado = registro.Value1;
                    if (valorResultado == "0")
                    {
                        negativos++;
                    }
                    else if (valorResultado == "1")
                    {
                        NoValido++;
                    }
                    else if (valorResultado == "2")
                    {
                        IgMPositivo++;
                    }
                    else if (valorResultado == "3")
                    {
                        IgGPositivo++;
                    }
                    else if (valorResultado == "4")
                    {
                        IgMeIfGPositivo++;
                    }
                    else if (valorResultado == "5")
                    {
                        NoRealizado++;
                    }
                    resultadoPrimeraPrueba = posiblesResultados.Find(p => p.i_ParameterId.ToString() == valorResultado).v_Value1;
                    tipoExamen = registro.ComponentId == Constants.CERTIFICADO_COVID_ID ? "CER" : "PR";
                }
                else
                {
                    SinResultados++;
                }
                string[] file = Directory.GetFiles(_rutaFichasCovid, item.ServiceId + ".pdf");
                if (file.Count() > 0)
                {
                    var info = string.Format("Trabajador: {0}-{3}, Fecha: {1}****{6}, Servicio: {2} Resultado: {4}, tipo Examen: {5}, Sede:{7} Técnico:{8}", item.Nombres + " " + item.ApellidoPaterno + " " + item.ApellidoMaterno, item.FechaServicio, item.ServiceId, item.Documento, resultadoPrimeraPrueba, tipoExamen,item.FechaInsert, item.Sede, item.Tecnico);
                    
                    Impresiones.ImprimirLinea(info);
                    countEncontrados++;
                }
                else
                {
                    var info = string.Format("Trabajador: {0}-{3}, Fecha: {1}****{6}, Servicio: {2} Resultado: {4}, tipo Examen: {5}, Sede:{7} Técnico:{8}", item.Nombres + " " + item.ApellidoPaterno + " " + item.ApellidoMaterno, item.FechaServicio, item.ServiceId, item.Documento, resultadoPrimeraPrueba, tipoExamen, item.FechaInsert, item.Sede, item.Tecnico);
                    Impresiones.ImprimirLinea(ConsoleColor.Red,ConsoleColor.White,info,false);
                    

                    if (registro != null)
                    {
                        if (item.TipoFormato != null)
                        {
                            GenerarReporte(item.TipoFormato.Value, item.ServiceId);
                            Impresiones.ImprimirLinea(ConsoleColor.Green, ConsoleColor.White, info, false);
                        }
                        else
                        {
                            GenerarReporte(1, item.ServiceId);
                            Impresiones.ImprimirLinea(ConsoleColor.Green, ConsoleColor.Red, info, false);
                        }                                               
                        
                        countNoEncontrados++;
                    }
                }
            }         

            #endregion
            Impresiones.ImprimirLinea("------------------------------------");
            Impresiones.ImprimirLinea("------------RESULTADOS--------------");
            Impresiones.ImprimirLinea(string.Format("Negativos: {0}, No Validos: {1}, IgM Positivo: {2}, IgGPositivo: {3}, IgM e IgG Positivo: {4}, No se realizó: {5}, Sin resultados: {6}", negativos, NoValido, IgMPositivo, IgGPositivo, IgMeIfGPositivo, NoRealizado, SinResultados));
            return string.Format("Fichas encontradas {0}, fichas no encontradas {1}",countEncontrados,countNoEncontrados);
        }

        public void GenerarReporte(int formatoReporte, string serviceId) {
            
            try
            {
                if (serviceId == "N102-SR000001718" || serviceId == "N102-SR000002032" || serviceId == "N101-SR000000551")
                {
                    return;
                }
                OperationResult objOperationResult = new OperationResult();
                MergeExPDF _mergeExPDF = new MergeExPDF();
                ReportDocument rp = new ReportDocument();
                List<string> _filesNameToMerge = new List<string>();

                
                var COVID_ID = new List<ReportCertificadoCovid>();
                var dsGetRepo = new DataSet();
                if (formatoReporte == 1)//detallado
                {
                    COVID_ID = new ServiceBL().GetCovid(ref objOperationResult, serviceId);
                    if (COVID_ID == null) return;
                    
                    DataTable dt_COVID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(COVID_ID);
                    dt_COVID.TableName = "dtCertificadoCovid";
                    dsGetRepo.Tables.Add(dt_COVID);
                    rp = new Reportes.crCovid();

                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    var objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = _rutaFichasCovid + serviceId + "-" + Constants.COVID_ID + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    rp.Close();

                    var x = _filesNameToMerge.ToList();
                    _mergeExPDF.FilesName = x;
                    _mergeExPDF.DestinationFile = _rutaFichasCovid + serviceId + ".pdf"; ;
                    _mergeExPDF.Execute();
                }
                else
                {
                    COVID_ID = new ServiceBL().GetCovidResumido(ref objOperationResult, serviceId);
                    if (COVID_ID == null) return;
                    DataTable dt_COVID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(COVID_ID);
                    dt_COVID.TableName = "dtCertificadoCovid";
                    dsGetRepo.Tables.Add(dt_COVID);
                    rp = new Reportes.rCovidResumido();

                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    var objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = _rutaFichasCovid + serviceId + "-" + Constants.COVID_ID + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    rp.Close();

                    var x = _filesNameToMerge.ToList();
                    _mergeExPDF.FilesName = x;
                    _mergeExPDF.DestinationFile = _rutaFichasCovid + serviceId + ".pdf"; ;
                    _mergeExPDF.Execute();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public class DatosServicio
        {
            public string ServiceId { get; set; }
            public string ApellidoPaterno { get; set; }
            public string ApellidoMaterno { get; set; }
            public string Nombres { get; set; }
            public string Documento { get; set; }
            public string ComponentId { get; set; }
            public DateTime FechaServicio { get; set; }
            public DateTime FechaInsert { get; set; }
            public int? TipoFormato { get; set; }
            public string Sede { get; set; }
            public string Tecnico { get; set; }
        }    
    }
}
