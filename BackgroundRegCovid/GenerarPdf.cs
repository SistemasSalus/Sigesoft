using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using NetPdf;
using Sigesoft.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using Sigesoft.Node.WinClient.BLL;
using Sigesoft.Node.WinClient.BE;


namespace BackgroundRegCovid
{
   public class GenerarPdf
    {

        public string GenerarReporte(string serviceId, int formato, string ruta, string componentId)
        {
            OperationResult objOperationResult = new OperationResult();
            MergeExPDF _mergeExPDF = new MergeExPDF();
            ReportDocument rp = new ReportDocument();
            List<string> _filesNameToMerge = new List<string>();
            var COVID_ID = new List<ReportCertificadoCovid>();
            var dsGetRepo = new DataSet();

            try
            {
                if (componentId == Constants.COVID_ID)
                {
                    if (formato == 1)//detallado
                    {
                        COVID_ID = new ServiceBL().GetCovid(ref objOperationResult, serviceId);
                        if (COVID_ID == null) return "----";

                        DataTable dt_COVID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(COVID_ID);
                        dt_COVID.TableName = "dtCertificadoCovid";
                        dsGetRepo.Tables.Add(dt_COVID);
                        rp = new Reportes.crCovid();

                        rp.SetDataSource(dsGetRepo);
                        rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        var objDiskOpt = new DiskFileDestinationOptions();
                        objDiskOpt.DiskFileName = ruta + serviceId + "-" + Constants.COVID_ID + " PR.pdf";
                        _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                        rp.ExportOptions.DestinationOptions = objDiskOpt;
                        rp.Export();
                        rp.Close();

                        var x = _filesNameToMerge.ToList();
                        _mergeExPDF.FilesName = x;
                        _mergeExPDF.DestinationFile = ruta + serviceId + " PR.pdf"; ;
                        _mergeExPDF.Execute();
                    }
                    else
                    {
                        COVID_ID = new ServiceBL().GetCovidResumido(ref objOperationResult, serviceId);
                        if (COVID_ID == null) return "----";
                        DataTable dt_COVID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(COVID_ID);
                        dt_COVID.TableName = "dtCertificadoCovid";
                        dsGetRepo.Tables.Add(dt_COVID);
                        rp = new Reportes.rCovidResumido();

                        rp.SetDataSource(dsGetRepo);
                        rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        var objDiskOpt = new DiskFileDestinationOptions();
                        objDiskOpt.DiskFileName = ruta + serviceId + "-" + Constants.COVID_ID + " PR.pdf";
                        _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                        rp.ExportOptions.DestinationOptions = objDiskOpt;
                        rp.Export();
                        rp.Close();

                        var x = _filesNameToMerge.ToList();
                        _mergeExPDF.FilesName = x;
                        _mergeExPDF.DestinationFile = ruta + serviceId + " PR.pdf"; ;
                        _mergeExPDF.Execute();
                    }

                    if (COVID_ID[0].ResultadoSegundaPrueba != null)
                    {
                        if (COVID_ID[0].ResultadoSegundaPrueba != "-1")
                            return COVID_ID[0].SegundoResultadoCovid;
                    }
                        

                    return COVID_ID[0].PrimerResultadoCovid;
                }
                else if (componentId == Constants.ANTIGENOS_ID)
                {
                    COVID_ID = new ServiceBL().GetAntigenoResumido(ref objOperationResult, serviceId);
                    if (COVID_ID == null) return "----";
                    DataTable dt_COVID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(COVID_ID);
                    dt_COVID.TableName = "dtCertificadoCovid";
                    dsGetRepo.Tables.Add(dt_COVID);
                    rp = new Reportes.crAntigenoResumido();

                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    var objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = ruta + serviceId + "-" + Constants.COVID_ID + " ANT.pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    rp.Close();

                    var x = _filesNameToMerge.ToList();
                    _mergeExPDF.FilesName = x;
                    _mergeExPDF.DestinationFile = ruta + serviceId + " ANT.pdf"; ;
                    _mergeExPDF.Execute();

                    //if (COVID_ID[0].ResultadoSegundaPrueba != null && COVID_ID[0].ResultadoSegundaPrueba != "-1")
                    //{
                    //    return COVID_ID[0].SegundoResultadoCovid;
                    //}

                    return COVID_ID[0].PrimerResultadoCovid;
                }
                else
                {
                    return null;
                }
                               
            }
            catch (Exception ex)
            {                
                throw;
            }

   
        }
        

    }
}
