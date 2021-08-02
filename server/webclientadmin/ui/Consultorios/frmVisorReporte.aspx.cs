using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using Sigesoft.Common;
using Sigesoft.Server.WebClientAdmin.BLL;
using System.Data;
using System.IO;
using System.Diagnostics;
using NetPdf;
using Sigesoft.Server.WebClientAdmin.BE;
using System.Web.Configuration;
using FineUI;

namespace Sigesoft.Server.WebClientAdmin.UI.Consultorios
{
    public partial class frmVisorReporte : System.Web.UI.Page
    {

        string _ruta;
        DataSet dsGetRepo = null;
        ReportDocument rp;
        DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
        List<string> _filesNameToMerge = new List<string>();
        ServiceBL _serviceBL = new ServiceBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            _ruta = WebConfigurationManager.AppSettings["rutaReportes"];
            string path;

            string Mode = Request.QueryString["Mode"].ToString();

            if (Mode == "Cardio")
            {
                path = _ruta + Session["ServiceId"].ToString() + "-" + "N002-ME000000025";
                GenerateElectro(_ruta, Session["ServiceId"].ToString());
                ShowPdf1.FilePath = @"files\" + Session["ServiceId"].ToString() + "-" + "N002-ME000000025.pdf";
            }
        }

        private void GenerateElectro(string _ruta, string p)
        {
            var ELECTROCARDIOGRAMA_ID = new ServiceBL().GetReportEstudioElectrocardiografico(p, Constants.ELECTROCARDIOGRAMA_ID);

            dsGetRepo = new DataSet();

            DataTable dt_ELECTROCARDIOGRAMA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ELECTROCARDIOGRAMA_ID);
            dt_ELECTROCARDIOGRAMA_ID.TableName = "dtEstudioElectrocardiografico";
            dsGetRepo.Tables.Add(dt_ELECTROCARDIOGRAMA_ID);
            rp = new Sigesoft.Server.WebClientAdmin.UI.Consultorios.crEstudioElectrocardiografico();
            rp.SetDataSource(dsGetRepo);
            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            objDiskOpt = new DiskFileDestinationOptions();
            objDiskOpt.DiskFileName = _ruta + p + "-" + Constants.ELECTROCARDIOGRAMA_ID + ".pdf";
            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
            rp.ExportOptions.DestinationOptions = objDiskOpt;
            rp.Export();

            objDiskOpt.DiskFileName = Server.MapPath("files/" + p + "-" + Constants.ELECTROCARDIOGRAMA_ID + ".pdf");
            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
            rp.ExportOptions.DestinationOptions = objDiskOpt;
            rp.Export();
            rp.Close();
        }


    }
}