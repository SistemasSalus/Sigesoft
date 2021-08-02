using Sigesoft.Node.WinClient.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneracionFichasCovid19
{
    class Program
    {
        static void Main(string[] args)
        {

            ServiceBL oServiceBL = new ServiceBL();

            try
            {
                MergeExPDF _mergeExPDF = new MergeExPDF();
                ReportDocument rp = new ReportDocument();
                List<string> _filesNameToMerge = new List<string>();
                var pstrRutaReportes = Server.MapPath("~/FichasCovid/");

                byte[] firma = ObtenerFirmaDoctoraNancy();

                var COVID_ID = new ServiceBL().GetCovid(ref objOperationResult, laboratorio.ServiceId, sessione.Sede, firma);

                var dsGetRepo = new DataSet();

                DataTable dt_COVID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(COVID_ID);
                dt_COVID.TableName = "dtCertificadoCovid";
                dsGetRepo.Tables.Add(dt_COVID);
                string rptPath = Server.MapPath(@"~\Reports\crCovid.rpt");
                rp.Load(rptPath);
                //rp = new Sigesoft.Node.WinClient.UI.Reports.crCovid();
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
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
