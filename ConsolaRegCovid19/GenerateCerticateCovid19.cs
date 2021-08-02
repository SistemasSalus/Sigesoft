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
using System.Linq;
using System.Text;

namespace ConsolaRegCovid19
{
    public class GenerateCerticateCovid19
    {
        public string _FechaInicio { get; set; }
        public string _FechaFin { get; set; }
        public string _OrganizationId { get; set; }
        public string _NodoId { get; set; }
        public string _Ruta { get; set; }
        public List<string> _dnis { get; set; }

        public GenerateCerticateCovid19(string FechaInicio, string FechaFin, string OrganizationId, string NodeId, string Ruta)
        {
            _FechaInicio = FechaInicio;
            _OrganizationId = OrganizationId;
            _FechaFin = FechaFin;
            _NodoId = NodeId;
            _Ruta = Ruta + "\\";
        }

        public GenerateCerticateCovid19(List<string> dnis, string Ruta)
        {
            _dnis = dnis;
            _Ruta = Ruta + "\\";
        }

        public List<Service> ServicesCovid19()
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var fechaInicio = DateTime.Parse(_FechaInicio);
            var fechaFin = DateTime.Parse(_FechaFin);
            fechaFin = fechaFin.AddDays(1);
            var objEntity = (from c in dbContext.service
                             join a in dbContext.servicecomponent on c.v_ServiceId equals a.v_ServiceId
                             join p in dbContext.person on c.v_PersonId equals p.v_PersonId
                             join prt in dbContext.protocol on c.v_ProtocolId equals prt.v_ProtocolId
                             where c.i_IsDeleted == 0
                             && c.d_ServiceDate >= fechaInicio && c.d_ServiceDate <= fechaFin
                             && prt.v_CustomerOrganizationId == _OrganizationId
                             && (a.v_ComponentId == Constants.COVID_ID || a.v_ComponentId == Constants.ANTIGENOS_ID)
                             && a.i_IsRequiredId == 1
                             && a.i_IsDeleted == 0
                             select new Service
                             {
                                 NodoId = c.v_ServiceId.Substring(0, 4),
                                 ServiceId = c.v_ServiceId,
                                 PersonId = p.v_PersonId,
                                 Trabajador = p.v_FirstName + " " +  p.v_FirstLastName + " " + p.v_SecondLastName,
                                 ComponentID = a.v_ComponentId
                             }).ToList();

            objEntity = objEntity.GroupBy(g => g.ServiceId).Select(s => s.First()).ToList();

            if (!string.IsNullOrEmpty(_NodoId))
            {
                objEntity = objEntity.FindAll(p => p.NodoId == _NodoId).ToList();     
            }           

            return objEntity;

        }

        public List<Service> ServicesCertificadoCovid19()
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();

            var fechaInicio = DateTime.Parse(_FechaInicio);
            var fechaFin = DateTime.Parse(_FechaFin);
            fechaFin = fechaFin.AddDays(1);
            var objEntity = (from c in dbContext.service
                             join a in dbContext.servicecomponent on c.v_ServiceId equals a.v_ServiceId
                             join p in dbContext.person on c.v_PersonId equals p.v_PersonId
                             join prt in dbContext.protocol on c.v_ProtocolId equals prt.v_ProtocolId
                             where c.i_IsDeleted == 0
                             && c.d_ServiceDate >= fechaInicio && c.d_ServiceDate <= fechaFin
                             && prt.v_CustomerOrganizationId == _OrganizationId
                             && a.v_ComponentId == Constants.CERTIFICADO_COVID_ID
                             && a.i_IsRequiredId == 1
                             && a.i_IsDeleted == 0
                             select new Service
                             {
                                 NodoId = c.v_ServiceId.Substring(0, 4),
                                 ServiceId = c.v_ServiceId,
                                 PersonId = p.v_PersonId,
                                 Trabajador = p.v_FirstName + " " + p.v_FirstLastName + " " + p.v_SecondLastName
                             }).ToList();

            objEntity = objEntity.GroupBy(g => g.ServiceId).Select(s => s.First()).ToList();

            if (!string.IsNullOrEmpty(_NodoId))
            {
                objEntity = objEntity.FindAll(p => p.NodoId == _NodoId).ToList();
            }

            return objEntity;

        }

        public bool GeneratePdfResumido(string serviceId)
        {
            try
            {

                    OperationResult objOperationResult = new OperationResult();
                    MergeExPDF _mergeExPDF = new MergeExPDF();
                    ReportDocument rp = new ReportDocument();
                    List<string> _filesNameToMerge = new List<string>();

                    var pstrRutaReportes = _Ruta;

                    var COVID_ID = new List<ReportCertificadoCovid>();
                    var dsGetRepo = new DataSet();
                    //cambiar para soportar antígenos
                    COVID_ID = new ServiceBL().GetCovidResumido(ref objOperationResult, serviceId);

                    //if (COVID_ID[0].ResultadoPrueba == "0" || COVID_ID[0].ResultadoPrueba == "1" || COVID_ID[0].ResultadoPrueba == "5")
                    //{
                    //    return false;    
                    //}
                    if (COVID_ID == null)
                    {
                        GeneratePdfCertificadoCovid19(serviceId);
                        return true;
                    }
                    DataTable dt_COVID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(COVID_ID);
                    dt_COVID.TableName = "dtCertificadoCovid";
                    dsGetRepo.Tables.Add(dt_COVID);

                    rp = new ConsolaRegCovid19.Reports.rCovidResumido();

                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    var objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + serviceId + "-" + Constants.COVID_ID + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    rp.Close();

                    var x = _filesNameToMerge.ToList();
                    _mergeExPDF.FilesName = x;
                    _mergeExPDF.DestinationFile = pstrRutaReportes + serviceId + ".pdf"; ;
                    _mergeExPDF.Execute();

                    return true;

             
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
           
        }

        public bool GeneratePdfDetallado(string serviceId)
        {
            try
            {
                OperationResult objOperationResult = new OperationResult();
                MergeExPDF _mergeExPDF = new MergeExPDF();
                ReportDocument rp = new ReportDocument();
                List<string> _filesNameToMerge = new List<string>();

                var pstrRutaReportes = _Ruta;

                var COVID_ID = new List<ReportCertificadoCovid>();
                var dsGetRepo = new DataSet();
                COVID_ID = new ServiceBL().GetCovid(ref objOperationResult, serviceId);
                if (COVID_ID == null)
                {
                    GeneratePdfCertificadoCovid19(serviceId);
                    return true;
                }
                DataTable dt_COVID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(COVID_ID);
                dt_COVID.TableName = "dtCertificadoCovid";
                dsGetRepo.Tables.Add(dt_COVID);

                rp = new ConsolaRegCovid19.Reports.crCovid();

                rp.SetDataSource(dsGetRepo);
                rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                var objDiskOpt = new DiskFileDestinationOptions();
                objDiskOpt.DiskFileName = pstrRutaReportes + serviceId + "-" + Constants.COVID_ID + ".pdf";
                _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                rp.ExportOptions.DestinationOptions = objDiskOpt;
                rp.Export();
                rp.Close();

                var x = _filesNameToMerge.ToList();
                _mergeExPDF.FilesName = x;
                _mergeExPDF.DestinationFile = pstrRutaReportes + serviceId + ".pdf"; ;
                _mergeExPDF.Execute();

                return true;


            }
            catch (Exception ex)
            {
                return false;
                throw;
            }

        }

        public bool GeneratePdfCertificadoCovid19(string serviceId)
        {
            try
            {
                List<string> _filesNameToMergeCovid19 = new List<string>();
                OperationResult objOperationResult = new OperationResult();
                MergeExPDF _mergeExPDF = new MergeExPDF();
                ReportDocument rp = new ReportDocument();
                List<string> _filesNameToMerge = new List<string>();

                var pstrRutaReportes = _Ruta;

                var CERTIFICADO_COVID_ID = new List<ReportCertificadoCovid>();
                var dsGetRepo = new DataSet();
                CERTIFICADO_COVID_ID = new ServiceBL().GetCertificateCovid(ref objOperationResult, serviceId);
                DataTable dt_CERTIFICADO_COVID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(CERTIFICADO_COVID_ID);
                dt_CERTIFICADO_COVID.TableName = "dtCertificadoCovid";
                dsGetRepo.Tables.Add(dt_CERTIFICADO_COVID);

                rp = new ConsolaRegCovid19.Reports.crCertificadoCovid();
                rp.SetDataSource(dsGetRepo);
                rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                var objDiskOpt = new DiskFileDestinationOptions();
                objDiskOpt.DiskFileName = pstrRutaReportes + serviceId + "-" + Constants.COVID_ID + ".pdf";

                _filesNameToMergeCovid19.Add(objDiskOpt.DiskFileName);
                _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                rp.ExportOptions.DestinationOptions = objDiskOpt;
                rp.Export();

                dsGetRepo = new DataSet();

                DataTable dt_CERTIFICADO_COVID_2 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(CERTIFICADO_COVID_ID);
                dt_CERTIFICADO_COVID_2.TableName = "dtCertificadoCovid";
                dsGetRepo.Tables.Add(dt_CERTIFICADO_COVID_2);

                rp = new Reports.crCertificadoCovid2();
                rp.SetDataSource(dsGetRepo);
                rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                objDiskOpt = new DiskFileDestinationOptions();
                objDiskOpt.DiskFileName = pstrRutaReportes + serviceId + "-" + Constants.CERTIFICADO_COVID_ID + "_2.pdf";

                _filesNameToMergeCovid19.Add(objDiskOpt.DiskFileName);
                rp.ExportOptions.DestinationOptions = objDiskOpt;
                rp.Export();
                
                //MERGE PDF COVID
                var reportesCovid19 = _filesNameToMergeCovid19.ToList();
                _mergeExPDF.FilesName = reportesCovid19;
                _mergeExPDF.DestinationFile = pstrRutaReportes + "\\" + serviceId + ".pdf"; ;
                _mergeExPDF.Execute();

                return true;


            }
            catch (Exception ex)
            {
                return false;
                throw;
            }

        }

        public List<Service> ServicesCovid19PorDnis()
        {
            SigesoftEntitiesModel dbContext = new SigesoftEntitiesModel();
            //_dnis.Add("10777189");
            
            var objEntity = (from c in dbContext.service
                             join a in dbContext.servicecomponent on c.v_ServiceId equals a.v_ServiceId
                             join p in dbContext.person on c.v_PersonId equals p.v_PersonId
                             join prt in dbContext.protocol on c.v_ProtocolId equals prt.v_ProtocolId
                             where c.i_IsDeleted == 0
                             && prt.v_CustomerOrganizationId == Constants.EMPRESA_BACKUS_ID
                             && a.v_ComponentId == Constants.COVID_ID
                             && a.i_IsRequiredId == 1
                             && a.i_IsDeleted == 0
                             && _dnis.Contains(p.v_DocNumber)
                             select new Service
                             {
                                 NodoId = c.v_ServiceId.Substring(0, 4),
                                 ServiceId = c.v_ServiceId,
                                 PersonId = p.v_PersonId,
                                 Trabajador = p.v_FirstName + " " + p.v_FirstLastName + " " + p.v_SecondLastName
                             }).ToList();

            //objEntity = objEntity.GroupBy(g => g.ServiceId).Select(s => s.First()).ToList();           

            return objEntity;

        }

        public class Service
        {
            public string NodoId { get; set; }
            public string ServiceId { get; set; }
            public string PersonId { get; set; }
            public string Trabajador { get; set; }
            public string ComponentID { get; set; }
        }
    }
}
