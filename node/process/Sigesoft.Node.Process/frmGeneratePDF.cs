using Sigesoft.Common;
using Sigesoft.Node.WinClient.BLL;
using Sigesoft.Node.WinClient.UI.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using NetPdf;
using System.Windows.Forms;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using Sigesoft.Node.WinClient.UI.Reports;
using Sigesoft.Node.WinClient.BE;

namespace Sigesoft.Node.Process
{
    public partial class frmGeneratePDF : Form
    {
        private List<string> _filesNameToMerge = null;
        ServiceBL _serviceBL = new ServiceBL();
        private MergeExPDF _mergeExPDF = new MergeExPDF();
        string ruta;
        PacientBL _pacientBL = new PacientBL();
        HistoryBL _historyBL = new HistoryBL();
        private string _customerOrganizationName;

        public frmGeneratePDF()
        {
            InitializeComponent();
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            timer1.Stop();

            ServiceBL _serviceBL = new ServiceBL();
            var ListaServicios = _serviceBL.ListarServiciosSinReportes();

            if (ListaServicios.Count() == 0)
            {
                timer1.Interval = 600000;
            }
            else
            {
                foreach (var item_0 in ListaServicios)
                {
                    try
                    {
                        GenerarReportes(item_0.ServiceId, item_0.PacienteId, item_0.NombrePaciente, "MAS");
                        OperationResult objOperationResult = new OperationResult();

                        _serviceBL.UpdateStatusPreLiquidation(ref objOperationResult, 2, item_0.ServiceId, null);

                    }
                    catch (Exception ex)
                    {
                        txtMensaje.Text = ex.Message;
                    }
                }

                txtMensaje.Text = txtMensaje.Text + "\r\n" + "Se Generaron: " + ListaServicios.Count() + "HCs Completas";

                timer1.Interval = 300000;
                timer1.Start();
            }
        }

        private void GenerarReportes(string serviceId, string personId, string pstrPaciente, string pstrbtn)
        {
            string ruta = Common.Utils.GetApplicationConfigValue("rutaReportes").ToString();
           
            string btn = pstrbtn;
            switch (btn)
            {
                case "MAS":
                    using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
                    {
                        CrearReportesCrystal(serviceId, personId, DevolverListaReportes(serviceId), null, ruta, btn);
                    };

                    var x = _filesNameToMerge.ToList();
                    _mergeExPDF.FilesName = x;
                    _mergeExPDF.DestinationFile = ruta + serviceId + "_" + pstrPaciente + ".pdf"; ;
                    _mergeExPDF.Execute();
                    //_mergeExPDF.RunFile();
                    //_serviceBL.UpdateStatusPreLiquidation(ref objOperationResult, 2, serviceId, Globals.ClientSession.GetAsList());  
                    break;
                    default: throw new System.ArgumentException("No se encuentra parámetro establecido", "original");
            }

        }

        public void CrearReportesCrystal(string serviceId, string personId, List<string> reportesId, List<Sigesoft.Node.WinClient.BE.ServiceComponentList> ListaDosaje, string pstrRutaReportes, string pstrbtn)
        {
            crConsolidatedReports rp = null;
            OperationResult objOperationResult = new OperationResult();
            MultimediaFileBL _multimediaFileBL = new MultimediaFileBL();
            rp = new Sigesoft.Node.WinClient.UI.Reports.crConsolidatedReports();
            _filesNameToMerge = new List<string>();
            foreach (var com in reportesId)
            {
                ChooseReport(com, serviceId, pstrRutaReportes, personId);
            }

            var ListaPdf = _serviceBL.GetFilePdfsByServiceId(ref objOperationResult, serviceId);


            var o = _serviceBL.GetServiceShort(serviceId);
            string Fecha = o.FechaServicio.Value.Day.ToString().PadLeft(2, '0') + o.FechaServicio.Value.Month.ToString().PadLeft(2, '0') + o.FechaServicio.Value.Year.ToString();
            DirectoryInfo rutaOrigen = null;

            //ELECTRO
            rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgEKGOrigen").ToString());
            FileInfo[] files1 = rutaOrigen.GetFiles();

            foreach (FileInfo file in files1)
            {
                if (file.ToString().Count() > 16)
                {
                    if (file.ToString().Substring(0, 17) == o.DNI + "-" + Fecha)
                    {
                        _filesNameToMerge.Add(rutaOrigen + file.ToString());
                    };
                }
            }

            //ESPIRO
            rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgESPIROOrigen").ToString());
            FileInfo[] files2 = rutaOrigen.GetFiles();

            foreach (FileInfo file in files2)
            {
                if (file.ToString().Count() > 16)
                {
                    if (file.ToString().Substring(0, 17) == o.DNI + "-" + Fecha)
                    {
                        _filesNameToMerge.Add(rutaOrigen + file.ToString());
                    };
                }
            }

            //RX
            rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgRxOrigen").ToString());
            FileInfo[] files3 = rutaOrigen.GetFiles();

            foreach (FileInfo file in files3)
            {
                if (file.ToString().Count() > 16)
                {
                    if (file.ToString().Substring(0, 17) == o.DNI + "-" + Fecha)
                    {
                        _filesNameToMerge.Add(rutaOrigen + file.ToString());
                    };
                }
            }

            //LAB
            rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgLABOrigen").ToString());
            FileInfo[] files4 = rutaOrigen.GetFiles();

            foreach (FileInfo file in files4)
            {
                if (file.ToString().Count() > 16)
                {
                    if (file.ToString().Substring(0, 17) == o.DNI + "-" + Fecha)
                    {
                        _filesNameToMerge.Add(rutaOrigen + file.ToString());
                    };
                }
            }

            //MED
            rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgMedicinaOrigen").ToString());
            FileInfo[] files5 = rutaOrigen.GetFiles();

            foreach (FileInfo file in files5)
            {
                if (file.ToString().Count() > 16)
                {
                    if (file.ToString().Substring(0, 17) == o.DNI + "-" + Fecha)
                    {
                        _filesNameToMerge.Add(rutaOrigen + file.ToString());
                    };
                }
            }

            //PSICO
            rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgPsicoOrigen").ToString());
            FileInfo[] files6 = rutaOrigen.GetFiles();

            foreach (FileInfo file in files6)
            {
                if (file.ToString().Count() > 16)
                {
                    if (file.ToString().Substring(0, 17) == o.DNI + "-" + Fecha)
                    {
                        _filesNameToMerge.Add(rutaOrigen + file.ToString());
                    };
                }
            }

            //ADMIN
            rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgAdminOrigen").ToString());
            FileInfo[] files7 = rutaOrigen.GetFiles();

            foreach (FileInfo file in files7)
            {
                if (file.ToString().Count() > 16)
                {
                    if (file.ToString().Substring(0, 17) == o.DNI + "-" + Fecha)
                    {
                        _filesNameToMerge.Add(rutaOrigen + file.ToString());
                    };
                }
            }

            //OFTALMO
            rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgOftalmoOrigen").ToString());
            FileInfo[] files8 = rutaOrigen.GetFiles();

            foreach (FileInfo file in files8)
            {
                if (file.ToString().Count() > 16)
                {
                    if (file.ToString().Substring(0, 17) == o.DNI + "-" + Fecha)
                    {
                        _filesNameToMerge.Add(rutaOrigen + file.ToString());
                    };
                }
            }


            var x = _filesNameToMerge.ToList();
            _mergeExPDF.FilesName = x;
            _mergeExPDF.DestinationFile = Application.StartupPath + @"\TempMerge\" + serviceId + ".pdf"; ;
            _mergeExPDF.DestinationFile = ruta + serviceId + ".pdf"; ;
            _mergeExPDF.Execute();

        }

        #region ChooseReport Antiguo
        //private void ChooseReport(string componentId, string psrtServiceId, string pstrRutaReportes, string pstrPersonId)
        //{
        //    DataSet dsGetRepo = null;
        //    DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
        //    OperationResult objOperationResult = new OperationResult();
        //    ReportDocument rp;

        //    try
        //    {           
        //    switch (componentId)
        //    {
        //        case Constants.INFORME_CERTIFICADO_APTITUD:
        //            var INFORME_CERTIFICADO_APTITUD = new ServiceBL().GetAptitudeCertificate(ref objOperationResult, psrtServiceId);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_INFORME_CERTIFICADO_APTITUD = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(INFORME_CERTIFICADO_APTITUD);
        //            dt_INFORME_CERTIFICADO_APTITUD.TableName = "AptitudeCertificate";
        //            dsGetRepo.Tables.Add(dt_INFORME_CERTIFICADO_APTITUD);

        //            rp = new Sigesoft.Node.WinClient.UI.Reports.crOccupationalMedicalAptitudeCertificate();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_CERTIFICADO_APTITUD + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            //Adicional para limpiar la Memoria RAM
        //            rp.Close();

        //            break;
        //        //case Constants.OFTALMOLOGIA_ID:

        //        //    var OFTALMOLOGIA = new ServiceBL().ReportOftalmologia(psrtServiceId, Constants.AGUDEZA_VISUAL);

        //        //    dsGetRepo = new DataSet();

        //        //    DataTable dt_OFTALMOLOGIA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(OFTALMOLOGIA);
        //        //    dt_OFTALMOLOGIA_ID.TableName = "dtOFTALMOLOGIA";
        //        //    dsGetRepo.Tables.Add(dt_OFTALMOLOGIA_ID);

        //        //    rp = new Sigesoft.Node.WinClient.UI.Reports.crInformeOftalmologico();
        //        //    rp.SetDataSource(dsGetRepo);
        //        //    //rp.Subreports["crInformeOftalmologico.rpt"].SetDataSource(dsGetRepo);

        //        //    var xExploracionClinicaEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["ExploracionClinicaEstaEnProtolo"];
        //        //    var xVisionColoresEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["VisionColoresEstaEnProtolo"];
        //        //    var xVisionEstereoscopicaEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["VisionEstereoscopicaEstaEnProtolo"];
        //        //    var xCampoVisualEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["CampoVisualEstaEnProtolo"];
        //        //    var xPresionIntraocularEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["PresionIntraocularEstaEnProtolo"];
        //        //    var xFondoOjoEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["FondoOjoEstaEnProtolo"];
        //        //    var xRefraccionEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["RefraccionEstaEnProtolo"];
        //        //    var xAgudezaVisualEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["AgudezaVisualEstaEnProtolo"];

        //        //    if (xExploracionClinicaEstaEnProtolo == true)
        //        //        rp.ReportDefinition.Sections["SectionExploracionClinica"].SectionFormat.EnableSuppress = false;

        //        //    if (xVisionColoresEstaEnProtolo == true)
        //        //        rp.ReportDefinition.Sections["SectionVisionColores"].SectionFormat.EnableSuppress = false;

        //        //    if (xVisionEstereoscopicaEstaEnProtolo == true)
        //        //        rp.ReportDefinition.Sections["SectionVisionEsteroscopica"].SectionFormat.EnableSuppress = false;

        //        //    if (xCampoVisualEstaEnProtolo == true)
        //        //        rp.ReportDefinition.Sections["SectionCampoVisual"].SectionFormat.EnableSuppress = false;

        //        //    if (xPresionIntraocularEstaEnProtolo == true)
        //        //        rp.ReportDefinition.Sections["SectionPresionIntraocular"].SectionFormat.EnableSuppress = false;

        //        //    if (xFondoOjoEstaEnProtolo == true)
        //        //        rp.ReportDefinition.Sections["SectionFondoOjo"].SectionFormat.EnableSuppress = false;

        //        //    if (xRefraccionEstaEnProtolo == true)
        //        //        rp.ReportDefinition.Sections["SectionRefraccion"].SectionFormat.EnableSuppress = false;

        //        //    if (xAgudezaVisualEstaEnProtolo == true)
        //        //        rp.ReportDefinition.Sections["SectionAgudezaVisual"].SectionFormat.EnableSuppress = false;

        //        //    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //        //    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //        //    objDiskOpt = new DiskFileDestinationOptions();
        //        //    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.OFTALMOLOGIA + ".pdf";
        //        //    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //        //    rp.ExportOptions.DestinationOptions = objDiskOpt;
        //        //    rp.Export();
        //        //    rp.Close();

        //        //    break;
        //        case Constants.EXAMEN_FISICO_ID:
        //            var EXAMEN_FISICO_ID = new ServiceBL().ReportMusculoEsqueletico1(psrtServiceId, Constants.MUSCULO_ESQUELETICO_1_ID);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_EXAMEN_FISICO_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(EXAMEN_FISICO_ID);
        //            dt_EXAMEN_FISICO_ID.TableName = "dtMusculoEsqueletico1";
        //            dsGetRepo.Tables.Add(dt_EXAMEN_FISICO_ID);

        //            rp = new Sigesoft.Node.WinClient.UI.Reports.crMusculoEsqueletico1();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            //objDiskOpt.DiskFileName = pstrRutaReportes + Constants.EXAMEN_FISICO_ID + ".pdf";
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.EXAMEN_FISICO_ID + "1.pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            //Adicional para limpiar la Memoria RAM
        //            rp.Close();

        //            var EXAMEN_FISICO_ID2 = new ServiceBL().ReportMusculoEsqueletico2(psrtServiceId, Constants.MÚSCULO_ESQUELÉTICO_2_ID);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_EXAMEN_FISICO_ID2 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(EXAMEN_FISICO_ID2);
        //            dt_EXAMEN_FISICO_ID2.TableName = "dtMusculoEsqueletico2";
        //            dsGetRepo.Tables.Add(dt_EXAMEN_FISICO_ID2);

        //            rp = new Sigesoft.Node.WinClient.UI.Reports.crMusculoEsqueletico2();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            //objDiskOpt.DiskFileName = pstrRutaReportes + Constants.EXAMEN_FISICO_ID + "2.pdf";
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.EXAMEN_FISICO_ID + "2.pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            //Adicional para limpiar la Memoria RAM
        //            rp.Close();

        //            break;

        //        case Constants.INFORME_HISTORIA_OCUPACIONAL:
        //            var INFORME_HISTORIA_OCUPACIONAL = new ServiceBL().ReportHistoriaOcupacional(psrtServiceId);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_INFORME_HISTORIA_OCUPACIONAL = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(INFORME_HISTORIA_OCUPACIONAL);
        //            dt_INFORME_HISTORIA_OCUPACIONAL.TableName = "HistoriaOcupacional";
        //            dsGetRepo.Tables.Add(dt_INFORME_HISTORIA_OCUPACIONAL);

        //            rp = new Sigesoft.Node.WinClient.UI.Reports.crHistoriaOcupacional();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_HISTORIA_OCUPACIONAL + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            //Adicional para limpiar la Memoria RAM
        //            rp.Close();

        //            break;

        //        case Constants.ALTURA_7D_ID:
        //            var AscensoAlturas = new ServiceBL().ReportAscensoGrandesAlturas(psrtServiceId, Constants.ALTURA_7D_ID);
        //            var FuncionesVitales = new ServiceBL().ReportFuncionesVitales(psrtServiceId, Constants.FUNCIONES_VITALES_ID);
        //            var Antropometria = new ServiceBL().ReportAntropometria(psrtServiceId, Constants.ANTROPOMETRIA_ID);

        //            dsGetRepo = new DataSet("Anexo7D");

        //            DataTable dt_ALTURA_7D_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(AscensoAlturas);
        //            dt_ALTURA_7D_ID.TableName = "dtAnexo7D";
        //            dsGetRepo.Tables.Add(dt_ALTURA_7D_ID);

        //            DataTable dt1 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(FuncionesVitales);
        //            dt1.TableName = "dtFuncionesVitales";
        //            dsGetRepo.Tables.Add(dt1);

        //            DataTable dt2 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(Antropometria);
        //            dt2.TableName = "dtAntropometria";
        //            dsGetRepo.Tables.Add(dt2);

        //            rp = new Sigesoft.Node.WinClient.UI.Reports.crAnexo7D();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ALTURA_7D_ID + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            //Adicional para limpiar la Memoria RAM
        //            rp.Close();

        //            break;

        //        case Constants.ALTURA_FISICA_M_18:
        //            var ALTURA_FISICA_M_18 = new PacientBL().ReportAlturaFisica(psrtServiceId, Constants.ALTURA_FISICA_M_18);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_ALTURA_FISICA_M_18 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ALTURA_FISICA_M_18);
        //            dt_ALTURA_FISICA_M_18.TableName = "dtAlturaFisica";
        //            dsGetRepo.Tables.Add(dt_ALTURA_FISICA_M_18);

        //            rp = new Sigesoft.Node.WinClient.UI.Reports.crAlturaFisica();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ALTURA_FISICA_M_18 + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            //Adicional para limpiar la Memoria RAM
        //            rp.Close();

        //            break;

        //        case Constants.ELECTROCARDIOGRAMA_ID:
        //            var ELECTROCARDIOGRAMA_ID = new ServiceBL().GetReportEstudioElectrocardiografico(psrtServiceId, Constants.ELECTROCARDIOGRAMA_ID);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_ELECTROCARDIOGRAMA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ELECTROCARDIOGRAMA_ID);
        //            dt_ELECTROCARDIOGRAMA_ID.TableName = "dtEstudioElectrocardiografico";
        //            dsGetRepo.Tables.Add(dt_ELECTROCARDIOGRAMA_ID);

        //            rp = new Sigesoft.Node.WinClient.UI.Reports.crEstudioElectrocardiografico();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ELECTROCARDIOGRAMA_ID + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            //Adicional para limpiar la Memoria RAM
        //            rp.Close();

        //            break;

        //        case Constants.PRUEBA_ESFUERZO_ID:
        //            var PRUEBA_ESFUERZO_ID = new ServiceBL().GetReportPruebaEsfuerzo(psrtServiceId, Constants.PRUEBA_ESFUERZO_ID);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_PRUEBA_ESFUERZO_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(PRUEBA_ESFUERZO_ID);
        //            dt_PRUEBA_ESFUERZO_ID.TableName = "dtFichaErgonometrica";
        //            dsGetRepo.Tables.Add(dt_PRUEBA_ESFUERZO_ID);

        //            rp = new Sigesoft.Node.WinClient.UI.Reports.crFichaErgonometrica();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.PRUEBA_ESFUERZO_ID + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            //Adicional para limpiar la Memoria RAM
        //            rp.Close();

        //            break;

        //        case Constants.ODONTOGRAMA_ID:
        //            var ruta = Application.StartupPath;
        //            var ODONTOGRAMA_ID = new ServiceBL().ReportOdontograma(psrtServiceId, Constants.ODONTOGRAMA_ID, ruta);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_ODONTOGRAMA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ODONTOGRAMA_ID);
        //            dt_ODONTOGRAMA_ID.TableName = "dtOdontograma";
        //            dsGetRepo.Tables.Add(dt_ODONTOGRAMA_ID);

        //            rp = new Sigesoft.Node.WinClient.UI.Reports.crOdontograma();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ODONTOGRAMA_ID + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            rp.Close();

        //            break;

        //        case Constants.AUDIOMETRIA_ID:

        //            var serviceBL = new ServiceBL();
        //            DataSet dsAudiometria = new DataSet();

        //            var dxList = serviceBL.GetDiagnosticRepositoryByComponent(psrtServiceId, Constants.AUDIOMETRIA_ID);

        //            if (dxList.Count != 0)
        //            {
        //                var dtDx = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dxList);
        //                dtDx.TableName = "dtDiagnostic";
        //                dsAudiometria.Tables.Add(dtDx);

        //                var recom = dxList.SelectMany(s1 => s1.Recomendations).ToList();

        //                var dtReco = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(recom);
        //                dtReco.TableName = "dtRecomendation";
        //                dsAudiometria.Tables.Add(dtReco);
        //            }
        //            else
        //            {
        //                List<DiagnosticRepositoryList> ldx = new List<DiagnosticRepositoryList>();
        //                DiagnosticRepositoryList oDiagnosticRepositoryList = new DiagnosticRepositoryList();
        //                oDiagnosticRepositoryList.v_DiseasesName = "";
        //                ldx.Add(oDiagnosticRepositoryList);
        //                var dtDx1 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ldx);
        //                dtDx1.TableName = "dtDiagnostic";
        //                dsAudiometria.Tables.Add(dtDx1);

        //                List<RecomendationList> lReco = new List<RecomendationList>();
        //                RecomendationList oRecomendationList = new RecomendationList();

        //                var dtReco1 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(lReco);
        //                dtReco1.TableName = "dtRecomendation";
        //                dsAudiometria.Tables.Add(dtReco1);
        //            }

        //            //-------******************************************************************************************

        //            var audioUserControlList = serviceBL.ReportAudiometriaUserControl(psrtServiceId, Constants.AUDIOMETRIA_ID);
        //            //aqui hay error corregir despues del cine
        //            var audioCabeceraList = serviceBL.ReportAudiometria(psrtServiceId, Constants.AUDIOMETRIA_ID);

        //            var dtAudiometriaUserControl = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(audioUserControlList);

        //            var dtCabecera = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(audioCabeceraList);

        //            dtCabecera.TableName = "dtAudiometria";
        //            dtAudiometriaUserControl.TableName = "dtAudiometriaUserControl";

        //            dsAudiometria.Tables.Add(dtCabecera);
        //            dsAudiometria.Tables.Add(dtAudiometriaUserControl);

        //            rp = new Sigesoft.Node.WinClient.UI.Reports.crFichaAudiometria2016();
        //            rp.SetDataSource(dsAudiometria);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.AUDIOMETRIA_ID + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            //Adicional para limpiar la Memoria RAM
        //            rp.Close();

        //            // Historia Ocupacional Audiometria
        //            var dataListForReport_1 = new ServiceBL().ReportHistoriaOcupacionalAudiometria(psrtServiceId);
        //            if (dataListForReport_1.Count != 0)
        //            {
        //                dsGetRepo = new DataSet();

        //                DataTable dt_dataListForReport_1 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport_1);

        //                dt_dataListForReport_1.TableName = "dtHistoriaOcupacional";

        //                dsGetRepo.Tables.Add(dt_dataListForReport_1);

        //                rp = new Sigesoft.Node.WinClient.UI.Reports.crHistoriaOcupacionalAudiometria();
        //                rp.SetDataSource(dsGetRepo);
        //                rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //                rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //                objDiskOpt = new DiskFileDestinationOptions();
        //                objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + "AUDIOMETRIA_ID_HISTORIA" + ".pdf";
        //                _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //                rp.ExportOptions.DestinationOptions = objDiskOpt;
        //                rp.Export();
        //                //Adicional para limpiar la Memoria RAM
        //                rp.Close();
        //            }

        //            break;

        //        case Constants.RX_ID:
        //            var RX_ID = new ServiceBL().ReportRadiologico(psrtServiceId, Constants.RX_ID);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_RX_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(RX_ID);
        //            dt_RX_ID.TableName = "dtRadiologico";
        //            dsGetRepo.Tables.Add(dt_RX_ID);

        //            rp = new Sigesoft.Node.WinClient.UI.Reports.crInformeRadiologico();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.RX_ID + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            //Adicional para limpiar la Memoria RAM
        //            rp.Close();

        //            break;

        //        case Constants.OIT_ID:
        //            var INFORME_RADIOGRAFICO_OIT = new ServiceBL().ReportInformeRadiografico(psrtServiceId, Constants.OIT_ID);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_INFORME_RADIOGRAFICO_OIT = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(INFORME_RADIOGRAFICO_OIT);
        //            dt_INFORME_RADIOGRAFICO_OIT.TableName = "dtInformeRadiografico";
        //            dsGetRepo.Tables.Add(dt_INFORME_RADIOGRAFICO_OIT);

        //            rp = new Sigesoft.Node.WinClient.UI.Reports.crInformeRadiograficoOIT();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_RADIOGRAFICO_OIT + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            //Adicional para limpiar la Memoria RAM
        //            rp.Close();

        //            break;

        //        case Constants.ESPIROMETRIA_ID:

        //            var ESPIROMETRIA_CUESTIONARIO_ID = new ServiceBL().GetReportCuestionarioEspirometria(psrtServiceId, Constants.ESPIROMETRIA_ID);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_ESPIROMETRIA_CUESTIONARIO_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ESPIROMETRIA_CUESTIONARIO_ID);
        //            dt_ESPIROMETRIA_CUESTIONARIO_ID.TableName = "dtCuestionarioEspirometria";
        //            dsGetRepo.Tables.Add(dt_ESPIROMETRIA_CUESTIONARIO_ID);

        //            rp = new Sigesoft.Node.WinClient.UI.Reports.crCuestionarioEspirometria2016();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ESPIROMETRIA_CUESTIONARIO_ID + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            //Adicional para limpiar la Memoria RAM
        //            rp.Close();

        //            var ESPIROMETRIA_ID = new ServiceBL().GetReportInformeEspirometria(psrtServiceId, Constants.ESPIROMETRIA_ID);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_ESPIROMETRIA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ESPIROMETRIA_ID);
        //            dt_ESPIROMETRIA_ID.TableName = "dtInformeEspirometria";
        //            dsGetRepo.Tables.Add(dt_ESPIROMETRIA_ID);

        //            rp = new Sigesoft.Node.WinClient.UI.Reports.crInformeEspirometria();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ESPIROMETRIA_ID + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            //Adicional para limpiar la Memoria RAM
        //            rp.Close();

        //            break;

        //        case Constants.INFORME_ANEXO_312:
        //            GenerateAnexo312(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_ANEXO_312)), pstrPersonId, psrtServiceId);
        //            _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_ANEXO_312)));
        //            break;
        //        case Constants.INFORME_FICHA_MEDICA_TRABAJADOR:
        //            GenerateInformeMedicoTrabajador(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_FICHA_MEDICA_TRABAJADOR)), pstrPersonId, psrtServiceId);
        //            _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_FICHA_MEDICA_TRABAJADOR)));
        //            break;
        //        case Constants.INFORME_ANEXO_7C:
        //            GenerateAnexo7C(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_ANEXO_7C)), pstrPersonId, psrtServiceId);
        //            _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_ANEXO_7C)));
        //            break;
        //        case Constants.INFORME_CLINICO:
        //            GenerateInformeExamenClinico(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_CLINICO)), pstrPersonId, psrtServiceId);
        //            _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_CLINICO)));
        //            break;
        //        case Constants.INFORME_LABORATORIO_CLINICO:
        //            GenerateLaboratorioReport(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_LABORATORIO_CLINICO)), psrtServiceId);
        //            _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_LABORATORIO_CLINICO)));
        //            break;
        //        default:
        //            break;
        //    }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //}
        #endregion
        private void ChooseReport(string componentId, string psrtServiceId, string pstrRutaReportes, string pstrPersonId)
        {
            DataSet dsGetRepo = null;
            DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
            OperationResult objOperationResult = new OperationResult();
            ReportDocument rp;

            switch (componentId)
            {
                #region Certificado de Aptitud
                case Constants.INFORME_CERTIFICADO_APTITUD:

                    var INFORME_CERTIFICADO_APTITUD = new ServiceBL().GetAptitudeCertificate(ref objOperationResult, psrtServiceId);

                    dsGetRepo = new DataSet();

                    DataTable dt_INFORME_CERTIFICADO_APTITUD = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(INFORME_CERTIFICADO_APTITUD);
                    dt_INFORME_CERTIFICADO_APTITUD.TableName = "AptitudeCertificate";
                    dsGetRepo.Tables.Add(dt_INFORME_CERTIFICADO_APTITUD);

                    rp = new Sigesoft.Node.WinClient.UI.Reports.crOccupationalMedicalAptitudeCertificate();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_CERTIFICADO_APTITUD + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    //Adicional para limpiar la Memoria RAM
                    rp.Close();

                    //var CONSENTIMIENTOINFORMADO = new PacientBL().GetReportConsentimiento(psrtServiceId);

                    //dsGetRepo = new DataSet();
                    //DataTable dt_CONSENTIMIENTOINFORMADO = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(CONSENTIMIENTOINFORMADO);
                    //dt_CONSENTIMIENTOINFORMADO.TableName = "dtConsentimiento";
                    //dsGetRepo.Tables.Add(dt_CONSENTIMIENTOINFORMADO);
                    //rp = new Sigesoft.Node.WinClient.UI.Reports.crConsentimiento();
                    //rp.SetDataSource(dsGetRepo);
                    //rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    //rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    //objDiskOpt = new DiskFileDestinationOptions();
                    //objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_CONSENTIMIENTO_INFORMADO + ".pdf";
                    //_filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    //rp.ExportOptions.DestinationOptions = objDiskOpt;
                    //rp.Export();
                    //rp.Close();

                    break;
                #endregion

                #region Oftalmologia
                //SOLUCIONAR DAVID PORQUE NO GENERA DESDE COMERCIAL
                case Constants.OFTALMOLOGIA_ID:

                    var OFTALMOLOGIA = new ServiceBL().ReportOftalmologia(psrtServiceId, Constants.AGUDEZA_VISUAL);

                    dsGetRepo = new DataSet();

                    DataTable dt_OFTALMOLOGIA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(OFTALMOLOGIA);
                    dt_OFTALMOLOGIA_ID.TableName = "dtOFTALMOLOGIA";
                    dsGetRepo.Tables.Add(dt_OFTALMOLOGIA_ID);

                    rp = new Sigesoft.Node.WinClient.UI.Reports.crInformeOftalmologico();
                    rp.SetDataSource(dsGetRepo);
                    //rp.Subreports["crInformeOftalmologico.rpt"].SetDataSource(dsGetRepo);

                    var xExploracionClinicaEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["ExploracionClinicaEstaEnProtolo"];
                    var xVisionColoresEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["VisionColoresEstaEnProtolo"];
                    var xVisionEstereoscopicaEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["VisionEstereoscopicaEstaEnProtolo"];
                    var xCampoVisualEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["CampoVisualEstaEnProtolo"];
                    var xPresionIntraocularEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["PresionIntraocularEstaEnProtolo"];
                    var xFondoOjoEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["FondoOjoEstaEnProtolo"];
                    var xRefraccionEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["RefraccionEstaEnProtolo"];
                    var xAgudezaVisualEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["AgudezaVisualEstaEnProtolo"];

                    if (xExploracionClinicaEstaEnProtolo == true)
                        rp.ReportDefinition.Sections["SectionExploracionClinica"].SectionFormat.EnableSuppress = false;

                    if (xVisionColoresEstaEnProtolo == true)
                        rp.ReportDefinition.Sections["SectionVisionColores"].SectionFormat.EnableSuppress = false;

                    if (xVisionEstereoscopicaEstaEnProtolo == true)
                        rp.ReportDefinition.Sections["SectionVisionEsteroscopica"].SectionFormat.EnableSuppress = false;

                    if (xCampoVisualEstaEnProtolo == true)
                        rp.ReportDefinition.Sections["SectionCampoVisual"].SectionFormat.EnableSuppress = false;

                    if (xPresionIntraocularEstaEnProtolo == true)
                        rp.ReportDefinition.Sections["SectionPresionIntraocular"].SectionFormat.EnableSuppress = false;

                    if (xFondoOjoEstaEnProtolo == true)
                        rp.ReportDefinition.Sections["SectionFondoOjo"].SectionFormat.EnableSuppress = false;

                    if (xRefraccionEstaEnProtolo == true)
                        rp.ReportDefinition.Sections["SectionRefraccion"].SectionFormat.EnableSuppress = false;

                    if (xAgudezaVisualEstaEnProtolo == true)
                        rp.ReportDefinition.Sections["SectionAgudezaVisual"].SectionFormat.EnableSuppress = false;

                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.OFTALMOLOGIA + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    rp.Close();

                    break;
                #endregion

                #region Examen Fisico
                case Constants.EXAMEN_FISICO_ID:
                    var EXAMEN_FISICO_ID = new ServiceBL().ReportMusculoEsqueletico1(psrtServiceId, Constants.MUSCULO_ESQUELETICO_1_ID);

                    dsGetRepo = new DataSet();

                    DataTable dt_EXAMEN_FISICO_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(EXAMEN_FISICO_ID);
                    dt_EXAMEN_FISICO_ID.TableName = "dtMusculoEsqueletico1";
                    dsGetRepo.Tables.Add(dt_EXAMEN_FISICO_ID);

                    rp = new Sigesoft.Node.WinClient.UI.Reports.crMusculoEsqueletico1();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    //objDiskOpt.DiskFileName = pstrRutaReportes + Constants.EXAMEN_FISICO_ID + ".pdf";
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.EXAMEN_FISICO_ID + "1.pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    //Adicional para limpiar la Memoria RAM
                    rp.Close();

                    var EXAMEN_FISICO_ID2 = new ServiceBL().ReportMusculoEsqueletico2(psrtServiceId, Constants.MÚSCULO_ESQUELÉTICO_2_ID);

                    dsGetRepo = new DataSet();

                    DataTable dt_EXAMEN_FISICO_ID2 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(EXAMEN_FISICO_ID2);
                    dt_EXAMEN_FISICO_ID2.TableName = "dtMusculoEsqueletico2";
                    dsGetRepo.Tables.Add(dt_EXAMEN_FISICO_ID2);

                    rp = new Sigesoft.Node.WinClient.UI.Reports.crMusculoEsqueletico2();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    //objDiskOpt.DiskFileName = pstrRutaReportes + Constants.EXAMEN_FISICO_ID + "2.pdf";
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.EXAMEN_FISICO_ID + "2.pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    //Adicional para limpiar la Memoria RAM
                    rp.Close();

                    break;
                #endregion

                #region Historia Ocupacional
                case Constants.INFORME_HISTORIA_OCUPACIONAL:
                    var INFORME_HISTORIA_OCUPACIONAL = new ServiceBL().ReportHistoriaOcupacional(psrtServiceId);

                    dsGetRepo = new DataSet();

                    DataTable dt_INFORME_HISTORIA_OCUPACIONAL = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(INFORME_HISTORIA_OCUPACIONAL);
                    dt_INFORME_HISTORIA_OCUPACIONAL.TableName = "HistoriaOcupacional";
                    dsGetRepo.Tables.Add(dt_INFORME_HISTORIA_OCUPACIONAL);

                    rp = new Sigesoft.Node.WinClient.UI.Reports.crHistoriaOcupacional();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_HISTORIA_OCUPACIONAL + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    //Adicional para limpiar la Memoria RAM
                    rp.Close();

                    break;
                #endregion

                #region ANEXO 16A
                case Constants.ALTURA_7D_ID:
                    var AscensoAlturas = new ServiceBL().ReportAscensoGrandesAlturas(psrtServiceId, Constants.ALTURA_7D_ID);
                    var FuncionesVitales = new ServiceBL().ReportFuncionesVitales(psrtServiceId, Constants.FUNCIONES_VITALES_ID);
                    var Antropometria = new ServiceBL().ReportAntropometria(psrtServiceId, Constants.ANTROPOMETRIA_ID);

                    dsGetRepo = new DataSet("Anexo7D");

                    DataTable dt_ALTURA_7D_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(AscensoAlturas);
                    dt_ALTURA_7D_ID.TableName = "dtAnexo7D";
                    dsGetRepo.Tables.Add(dt_ALTURA_7D_ID);

                    DataTable dt1 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(FuncionesVitales);
                    dt1.TableName = "dtFuncionesVitales";
                    dsGetRepo.Tables.Add(dt1);

                    DataTable dt2 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(Antropometria);
                    dt2.TableName = "dtAntropometria";
                    dsGetRepo.Tables.Add(dt2);

                    rp = new Sigesoft.Node.WinClient.UI.Reports.crAnexo7D();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ALTURA_7D_ID + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    //Adicional para limpiar la Memoria RAM
                    rp.Close();

                    break;
                #endregion

                #region Altura 1.8
                case Constants.ALTURA_FISICA_M_18:
                    var ALTURA_FISICA_M_18 = new PacientBL().ReportAlturaFisica(psrtServiceId, Constants.ALTURA_FISICA_M_18);

                    dsGetRepo = new DataSet();

                    DataTable dt_ALTURA_FISICA_M_18 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ALTURA_FISICA_M_18);
                    dt_ALTURA_FISICA_M_18.TableName = "dtAlturaFisica";
                    dsGetRepo.Tables.Add(dt_ALTURA_FISICA_M_18);

                    rp = new Sigesoft.Node.WinClient.UI.Reports.crAlturaFisica();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ALTURA_FISICA_M_18 + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    //Adicional para limpiar la Memoria RAM
                    rp.Close();

                    break;
                #endregion

                #region Electrocardiograma
                case Constants.ELECTROCARDIOGRAMA_ID:
                    var ELECTROCARDIOGRAMA_ID = new ServiceBL().GetReportEstudioElectrocardiografico(psrtServiceId, Constants.ELECTROCARDIOGRAMA_ID);

                    dsGetRepo = new DataSet();

                    DataTable dt_ELECTROCARDIOGRAMA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ELECTROCARDIOGRAMA_ID);
                    dt_ELECTROCARDIOGRAMA_ID.TableName = "dtEstudioElectrocardiografico";
                    dsGetRepo.Tables.Add(dt_ELECTROCARDIOGRAMA_ID);

                    rp = new Sigesoft.Node.WinClient.UI.Reports.crEstudioElectrocardiografico();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ELECTROCARDIOGRAMA_ID + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    //Adicional para limpiar la Memoria RAM
                    rp.Close();

                    break;
                #endregion

                #region Prueba de Esfuerzo
                case Constants.PRUEBA_ESFUERZO_ID:
                    var PRUEBA_ESFUERZO_ID = new ServiceBL().GetReportPruebaEsfuerzo(psrtServiceId, Constants.PRUEBA_ESFUERZO_ID);

                    dsGetRepo = new DataSet();

                    DataTable dt_PRUEBA_ESFUERZO_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(PRUEBA_ESFUERZO_ID);
                    dt_PRUEBA_ESFUERZO_ID.TableName = "dtFichaErgonometrica";
                    dsGetRepo.Tables.Add(dt_PRUEBA_ESFUERZO_ID);

                    rp = new Sigesoft.Node.WinClient.UI.Reports.crFichaErgonometrica();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.PRUEBA_ESFUERZO_ID + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    //Adicional para limpiar la Memoria RAM
                    rp.Close();

                    break;
                #endregion

                #region Odontología
                case Constants.ODONTOGRAMA_ID:
                    var ruta = Application.StartupPath;
                    var ODONTOGRAMA_ID = new ServiceBL().ReportOdontograma(psrtServiceId, Constants.ODONTOGRAMA_ID, ruta);

                    dsGetRepo = new DataSet();

                    DataTable dt_ODONTOGRAMA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ODONTOGRAMA_ID);
                    dt_ODONTOGRAMA_ID.TableName = "dtOdontograma";
                    dsGetRepo.Tables.Add(dt_ODONTOGRAMA_ID);

                    rp = new Sigesoft.Node.WinClient.UI.Reports.crOdontograma();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ODONTOGRAMA_ID + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    rp.Close();

                    break;
                #endregion

                #region Audiometria
                case Constants.AUDIOMETRIA_ID:

                    var serviceBL = new ServiceBL();
                    DataSet dsAudiometria = new DataSet();

                    var dxList = serviceBL.GetDiagnosticRepositoryByComponent(psrtServiceId, Constants.AUDIOMETRIA_ID);

                    if (dxList.Count != 0)
                    {
                        var dtDx = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dxList);
                        dtDx.TableName = "dtDiagnostic";
                        dsAudiometria.Tables.Add(dtDx);

                        var recom = dxList.SelectMany(s1 => s1.Recomendations).ToList();

                        var dtReco = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(recom);
                        dtReco.TableName = "dtRecomendation";
                        dsAudiometria.Tables.Add(dtReco);
                    }
                    else
                    {
                        List<DiagnosticRepositoryList> ldx = new List<DiagnosticRepositoryList>();
                        DiagnosticRepositoryList oDiagnosticRepositoryList = new DiagnosticRepositoryList();
                        oDiagnosticRepositoryList.v_DiseasesName = "";
                        ldx.Add(oDiagnosticRepositoryList);
                        var dtDx1 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ldx);
                        dtDx1.TableName = "dtDiagnostic";
                        dsAudiometria.Tables.Add(dtDx1);

                        List<RecomendationList> lReco = new List<RecomendationList>();
                        RecomendationList oRecomendationList = new RecomendationList();

                        var dtReco1 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(lReco);
                        dtReco1.TableName = "dtRecomendation";
                        dsAudiometria.Tables.Add(dtReco1);
                    }

                    //-------******************************************************************************************

                    var audioUserControlList = serviceBL.ReportAudiometriaUserControl(psrtServiceId, Constants.AUDIOMETRIA_ID);
                    //aqui hay error corregir despues del cine
                    var audioCabeceraList = serviceBL.ReportAudiometria(psrtServiceId, Constants.AUDIOMETRIA_ID);

                    var dtAudiometriaUserControl = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(audioUserControlList);

                    var dtCabecera = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(audioCabeceraList);

                    dtCabecera.TableName = "dtAudiometria";
                    dtAudiometriaUserControl.TableName = "dtAudiometriaUserControl";

                    dsAudiometria.Tables.Add(dtCabecera);
                    dsAudiometria.Tables.Add(dtAudiometriaUserControl);

                    rp = new Sigesoft.Node.WinClient.UI.Reports.crFichaAudiometria2016();
                    rp.SetDataSource(dsAudiometria);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.AUDIOMETRIA_ID + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    //Adicional para limpiar la Memoria RAM
                    rp.Close();

                    // Historia Ocupacional Audiometria
                    var dataListForReport_1 = new ServiceBL().ReportHistoriaOcupacionalAudiometria(psrtServiceId);
                    if (dataListForReport_1.Count != 0)
                    {
                        dsGetRepo = new DataSet();

                        DataTable dt_dataListForReport_1 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport_1);

                        dt_dataListForReport_1.TableName = "dtHistoriaOcupacional";

                        dsGetRepo.Tables.Add(dt_dataListForReport_1);

                        rp = new Sigesoft.Node.WinClient.UI.Reports.crHistoriaOcupacionalAudiometria();
                        rp.SetDataSource(dsGetRepo);
                        rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        objDiskOpt = new DiskFileDestinationOptions();
                        objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + "AUDIOMETRIA_ID_HISTORIA" + ".pdf";
                        _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                        rp.ExportOptions.DestinationOptions = objDiskOpt;
                        rp.Export();
                        //Adicional para limpiar la Memoria RAM
                        rp.Close();
                    }

                    break;
                #endregion

                #region Radiografía de Torax
                case Constants.RX_ID:
                    var RX_ID = new ServiceBL().ReportRadiologico(psrtServiceId, Constants.RX_ID);

                    dsGetRepo = new DataSet();

                    DataTable dt_RX_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(RX_ID);
                    dt_RX_ID.TableName = "dtRadiologico";
                    dsGetRepo.Tables.Add(dt_RX_ID);

                    rp = new Sigesoft.Node.WinClient.UI.Reports.crInformeRadiologico();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.RX_ID + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    //Adicional para limpiar la Memoria RAM
                    rp.Close();

                    break;
                #endregion

                #region Radiografía OIT
                case Constants.OIT_ID:
                    var INFORME_RADIOGRAFICO_OIT = new ServiceBL().ReportInformeRadiografico(psrtServiceId, Constants.OIT_ID);

                    dsGetRepo = new DataSet();

                    DataTable dt_INFORME_RADIOGRAFICO_OIT = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(INFORME_RADIOGRAFICO_OIT);
                    dt_INFORME_RADIOGRAFICO_OIT.TableName = "dtInformeRadiografico";
                    dsGetRepo.Tables.Add(dt_INFORME_RADIOGRAFICO_OIT);

                    rp = new Sigesoft.Node.WinClient.UI.Reports.crInformeRadiograficoOIT();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_RADIOGRAFICO_OIT + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    //Adicional para limpiar la Memoria RAM
                    rp.Close();

                    break;
                #endregion

                #region Espirometría
                case Constants.ESPIROMETRIA_ID:

                    var ESPIROMETRIA_CUESTIONARIO_ID = new ServiceBL().GetReportCuestionarioEspirometria(psrtServiceId, Constants.ESPIROMETRIA_ID);

                    dsGetRepo = new DataSet();

                    DataTable dt_ESPIROMETRIA_CUESTIONARIO_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ESPIROMETRIA_CUESTIONARIO_ID);
                    dt_ESPIROMETRIA_CUESTIONARIO_ID.TableName = "dtCuestionarioEspirometria";
                    dsGetRepo.Tables.Add(dt_ESPIROMETRIA_CUESTIONARIO_ID);

                    rp = new Sigesoft.Node.WinClient.UI.Reports.crCuestionarioEspirometria2016();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ESPIROMETRIA_CUESTIONARIO_ID + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    //Adicional para limpiar la Memoria RAM
                    rp.Close();

                    var ESPIROMETRIA_ID = new ServiceBL().GetReportInformeEspirometria(psrtServiceId, Constants.ESPIROMETRIA_ID);

                    dsGetRepo = new DataSet();

                    DataTable dt_ESPIROMETRIA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ESPIROMETRIA_ID);
                    dt_ESPIROMETRIA_ID.TableName = "dtInformeEspirometria";
                    dsGetRepo.Tables.Add(dt_ESPIROMETRIA_ID);

                    rp = new Sigesoft.Node.WinClient.UI.Reports.crInformeEspirometria();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ESPIROMETRIA_ID + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    //Adicional para limpiar la Memoria RAM
                    rp.Close();

                    break;
                #endregion

                #region Ficha 312
                case Constants.INFORME_ANEXO_312:
                    GenerateAnexo312(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_ANEXO_312)), pstrPersonId, psrtServiceId);
                    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_ANEXO_312)));
                    break;
                #endregion

                #region Informe para el Trabajador
                case Constants.INFORME_FICHA_MEDICA_TRABAJADOR:
                    GenerateInformeMedicoTrabajador(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_FICHA_MEDICA_TRABAJADOR)), pstrPersonId, psrtServiceId);
                    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_FICHA_MEDICA_TRABAJADOR)));

                    var CONSENTIMIENTOINFORMADO = new PacientBL().GetReportConsentimiento(psrtServiceId);

                    dsGetRepo = new DataSet();
                    DataTable dt_CONSENTIMIENTOINFORMADO = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(CONSENTIMIENTOINFORMADO);
                    dt_CONSENTIMIENTOINFORMADO.TableName = "dtConsentimiento";
                    dsGetRepo.Tables.Add(dt_CONSENTIMIENTOINFORMADO);
                    rp = new Sigesoft.Node.WinClient.UI.Reports.crConsentimiento();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_CONSENTIMIENTO_INFORMADO + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    rp.Close();

                    break;
                #endregion

                #region Ficha Anexo 16
                case Constants.INFORME_ANEXO_7C:
                    GenerateAnexo7C(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_ANEXO_7C)), pstrPersonId, psrtServiceId);
                    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_ANEXO_7C)));
                    break;
                #endregion

                #region Informe Clinico *No se Usa
                case Constants.INFORME_CLINICO:
                    GenerateInformeExamenClinico(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_CLINICO)), pstrPersonId, psrtServiceId);
                    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_CLINICO)));
                    break;
                #endregion

                #region Laboratorio Clínico
                case Constants.INFORME_LABORATORIO_CLINICO:
                    GenerateLaboratorioReport(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_LABORATORIO_CLINICO)), psrtServiceId);
                    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_LABORATORIO_CLINICO)));
                    break;
                #endregion

                default:
                    break;
            }

        }


        #region Devolver Antiguo
        //List<string> DevolverListaReportes(string pstrServiceId)
        //{
        //    List<ServiceComponentList> serviceComponents = new List<ServiceComponentList>();
        //    List<ServiceComponentList> ListaOrdenada = new List<ServiceComponentList>();
        //    string[] ConsolidadoReportesForPrint = new string[] 
        //    {
        //        Constants.ALTURA_FISICA_M_18,
        //        Constants.ALTURA_ESTRUCTURAL_ID,
        //        Constants.ALTURA_7D_ID,
        //        Constants.ODONTOGRAMA_ID,               
        //        Constants.RX_ID,
        //        Constants.PRUEBA_ESFUERZO_ID,
        //        Constants.ELECTROCARDIOGRAMA_ID,
        //        Constants.TAMIZAJE_DERMATOLOGIO_ID,
        //        Constants.AUDIOMETRIA_ID,
        //        Constants.ESPIROMETRIA_ID,
        //        Constants.EXAMEN_PSICOLOGICO,
        //        Constants.PSICOLOGIA_ID,
        //        Constants.OIT_ID,
        //        Constants.ESPIROMETRIA_CUESTIONARIO_ID,
        //        Constants.EXAMEN_FISICO_ID,
        //        Constants.OFTALMOLOGIA_ID
        //        //Constants.EVALUACION_PSICOLABORAL,
        //    };

        //    string[] compOftalmo = new string[]
        //    {    
        //        Constants.AGUDEZA_VISUAL,
        //        Constants.EXPLORACIÓN_CLÍNICA_ID,
        //        Constants.VISION_DE_COLORES_ID,
        //        Constants.VISION_ESTEREOSCOPICA_ID,
        //        Constants.CAMPO_VISUAL_ID, 
        //        Constants.PRESION_INTRAOCULAR_ID,
        //        Constants.FONDO_DE_OJO_ID,
        //        Constants.REFRACCION_ID,
        //    };

        //    string[] compMusEsque = new string[]
        //    {    
        //        Constants.MUSCULO_ESQUELETICO_1_ID,
        //        Constants.MÚSCULO_ESQUELÉTICO_2_ID,              
        //    };

        //    serviceComponents = _serviceBL.GetServiceComponentsForManagementReport(pstrServiceId);

        //    var oftalmo = serviceComponents.FindAll(p => compOftalmo.Contains(p.v_ComponentId));

        //    ServiceComponentList entS = null;
        //    if (oftalmo != null && oftalmo.Count > 0)
        //    {
        //        entS = new ServiceComponentList();
        //        entS.v_ComponentId = Constants.OFTALMOLOGIA_ID;
        //        entS.v_ComponentName = "OFTALMOLOGIA";
        //        serviceComponents.Add(entS);
        //    }

        //    //ORDENDAVID
        //    foreach (var item in serviceComponents)
        //    {
        //        if (item.v_ComponentId == Constants.AUDIOMETRIA_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 7;
        //        }
        //        else if (item.v_ComponentId == Constants.ESPIROMETRIA_CUESTIONARIO_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 8;
        //        }
        //        else if (item.v_ComponentId == Constants.ESPIROMETRIA_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 9;
        //        }
        //        else if (item.v_ComponentId == Constants.ALTURA_FISICA_M_18)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 10;
        //        }
        //        else if (item.v_ComponentId == Constants.EXAMEN_FISICO_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 11;
        //        }
        //        else if (item.v_ComponentId == Constants.ALTURA_7D_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 12;
        //        }
        //        else if (item.v_ComponentId == Constants.ALTURA_ESTRUCTURAL_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 13;
        //        }
        //        else if (item.v_ComponentId == Constants.OFTALMOLOGIA_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 14;
        //        }
        //        else if (item.v_ComponentId == Constants.OIT_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 15;
        //        }
        //        else if (item.v_ComponentId == Constants.ELECTROCARDIOGRAMA_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 16;
        //        }
        //        else if (item.v_ComponentId == Constants.EXAMEN_PSICOLOGICO)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 17;
        //        }
        //        else if (item.v_ComponentId == Constants.ODONTOGRAMA_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 18;
        //        }
        //        else if (item.v_ComponentId == Constants.RX_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 19;
        //        }
        //        else if (item.v_ComponentId == Constants.PRUEBA_ESFUERZO_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 20;
        //        }
        //    }

        //    ServiceShort _serviceShort = new ServiceShort();
        //    _serviceShort = _serviceBL.GetServiceShort(pstrServiceId);
        //    var SectorEmpresa = _serviceShort.EmpresaRubro.ToString();

        //    List<ServiceComponentList> ConsolidadoReportes = new List<ServiceComponentList>();
        //    ConsolidadoReportes.Add(new ServiceComponentList { Orden = 1, v_ComponentName = "CERTIFICADO APTITUD", v_ComponentId = Constants.INFORME_CERTIFICADO_APTITUD });
        //    ConsolidadoReportes.Add(new ServiceComponentList { Orden = 2, v_ComponentName = "FICHA MEDICA DEL TRABAJADOR", v_ComponentId = Constants.INFORME_FICHA_MEDICA_TRABAJADOR });
        //    if (SectorEmpresa == "MINERÍA")
        //    {
        //        ConsolidadoReportes.Add(new ServiceComponentList { Orden = 4, v_ComponentName = "ANEXO 16", v_ComponentId = Constants.INFORME_ANEXO_7C });
        //    }
        //    else
        //    {
        //        ConsolidadoReportes.Add(new ServiceComponentList { Orden = 3, v_ComponentName = "ANEXO 312", v_ComponentId = Constants.INFORME_ANEXO_312 });
        //    }
        //    //ConsolidadoReportes.Add(new ServiceComponentList { Orden = 3, v_ComponentName = "ANEXO 312", v_ComponentId = Constants.INFORME_ANEXO_312 });
        //    //ConsolidadoReportes.Add(new ServiceComponentList { Orden = 4, v_ComponentName = "ANEXO 16", v_ComponentId = Constants.INFORME_ANEXO_7C });
        //    ConsolidadoReportes.Add(new ServiceComponentList { Orden = 5, v_ComponentName = "HISTORIA OCUPACIONAL", v_ComponentId = Constants.INFORME_HISTORIA_OCUPACIONAL });
        //    ConsolidadoReportes.Add(new ServiceComponentList { Orden = 6, v_ComponentName = "LABORATORIO CLINICO", v_ComponentId = Constants.INFORME_LABORATORIO_CLINICO });

        //    ConsolidadoReportes.AddRange(serviceComponents.FindAll(p => ConsolidadoReportesForPrint.Contains(p.v_ComponentId)));


        //    ListaOrdenada = ConsolidadoReportes.OrderBy(p => p.Orden).ToList();
        //    List<string> Lista = new List<string>();

        //    foreach (var item in ListaOrdenada)
        //    {
        //        Lista.Add(item.v_ComponentId);
        //    }
        //    return Lista;
        //}
        #endregion
        List<string> DevolverListaReportes(string pstrServiceId)
        {
            List<ServiceComponentList> serviceComponents = new List<ServiceComponentList>();
            List<ServiceComponentList> ListaOrdenada = new List<ServiceComponentList>();
            string[] ConsolidadoReportesForPrint = new string[] 
            {
                Constants.ALTURA_FISICA_M_18,
                Constants.ALTURA_ESTRUCTURAL_ID,
                Constants.ALTURA_7D_ID,
                Constants.ODONTOGRAMA_ID,               
                Constants.RX_ID,
                Constants.PRUEBA_ESFUERZO_ID,
                Constants.ELECTROCARDIOGRAMA_ID,
                Constants.TAMIZAJE_DERMATOLOGIO_ID,
                Constants.AUDIOMETRIA_ID,
                Constants.ESPIROMETRIA_ID,
                Constants.EXAMEN_PSICOLOGICO,
                Constants.PSICOLOGIA_ID,
                Constants.OIT_ID,
                Constants.ESPIROMETRIA_CUESTIONARIO_ID,
                Constants.EXAMEN_FISICO_ID,
                Constants.OFTALMOLOGIA_ID
                //Constants.EVALUACION_PSICOLABORAL,
            };

            string[] compOftalmo = new string[]
            {    
                Constants.AGUDEZA_VISUAL,
                Constants.EXPLORACIÓN_CLÍNICA_ID,
                Constants.VISION_DE_COLORES_ID,
                Constants.VISION_ESTEREOSCOPICA_ID,
                Constants.CAMPO_VISUAL_ID, 
                Constants.PRESION_INTRAOCULAR_ID,
                Constants.FONDO_DE_OJO_ID,
                Constants.REFRACCION_ID,
            };

            string[] compMusEsque = new string[]
            {    
                Constants.MUSCULO_ESQUELETICO_1_ID,
                Constants.MÚSCULO_ESQUELÉTICO_2_ID,              
            };

            serviceComponents = _serviceBL.GetServiceComponentsForManagementReport(pstrServiceId);

            var oftalmo = serviceComponents.FindAll(p => compOftalmo.Contains(p.v_ComponentId));

            ServiceComponentList entS = null;
            if (oftalmo != null && oftalmo.Count > 0)
            {
                entS = new ServiceComponentList();
                entS.v_ComponentId = Constants.OFTALMOLOGIA_ID;
                entS.v_ComponentName = "OFTALMOLOGIA";
                serviceComponents.Add(entS);
            }

            //ORDENDAVID
            foreach (var item in serviceComponents)
            {
                if (item.v_ComponentId == Constants.EXAMEN_FISICO_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 6;
                }
                else if (item.v_ComponentId == Constants.ALTURA_7D_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 7;
                }
                else if (item.v_ComponentId == Constants.ALTURA_ESTRUCTURAL_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 8;
                }
                else if (item.v_ComponentId == Constants.ALTURA_FISICA_M_18)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 9;
                }
                else if (item.v_ComponentId == Constants.ODONTOGRAMA_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 10;
                }
                else if (item.v_ComponentId == Constants.ESPIROMETRIA_CUESTIONARIO_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 11;
                }
                else if (item.v_ComponentId == Constants.ESPIROMETRIA_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 12;
                }
                else if (item.v_ComponentId == Constants.OFTALMOLOGIA_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 13;
                }
                else if (item.v_ComponentId == Constants.AUDIOMETRIA_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 14;
                }
                else if (item.v_ComponentId == Constants.RX_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 15;
                }
                else if (item.v_ComponentId == Constants.OIT_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 16;
                }
                else if (item.v_ComponentId == Constants.EXAMEN_PSICOLOGICO)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 18;
                }
                else if (item.v_ComponentId == Constants.ELECTROCARDIOGRAMA_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 19;
                }
                else if (item.v_ComponentId == Constants.PRUEBA_ESFUERZO_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 20;
                }
            }

            ServiceShort _serviceShort = new ServiceShort();
            _serviceShort = _serviceBL.GetServiceShort(pstrServiceId);
            var ProtTipoReporte = _serviceShort.ProtTipoReporte.ToString();

            List<ServiceComponentList> ConsolidadoReportes = new List<ServiceComponentList>();
            //ConsolidadoReportes.Add(new ServiceComponentList { Orden = 1, v_ComponentName = "CERTIFICADO APTITUD", v_ComponentId = Constants.INFORME_CERTIFICADO_APTITUD });
            //ConsolidadoReportes.Add(new ServiceComponentList { Orden = 2, v_ComponentName = "FICHA MEDICA DEL TRABAJADOR", v_ComponentId = Constants.INFORME_FICHA_MEDICA_TRABAJADOR });

            //if (ProtTipoReporte == "FICHA312")
            //{
            //    ConsolidadoReportes.Add(new ServiceComponentList { Orden = 4, v_ComponentName = "ANEXO 312", v_ComponentId = Constants.INFORME_ANEXO_312 });
            //}
            //if (ProtTipoReporte == "ANEXO16")
            //{
            //    ConsolidadoReportes.Add(new ServiceComponentList { Orden = 5, v_ComponentName = "ANEXO 16", v_ComponentId = Constants.INFORME_ANEXO_7C });
            //}
            //if (ProtTipoReporte == "AMBOS")
            //{
            //    ConsolidadoReportes.Add(new ServiceComponentList { Orden = 4, v_ComponentName = "ANEXO 312", v_ComponentId = Constants.INFORME_ANEXO_312 });
            //    ConsolidadoReportes.Add(new ServiceComponentList { Orden = 5, v_ComponentName = "ANEXO 16", v_ComponentId = Constants.INFORME_ANEXO_7C });
            //}
            //ConsolidadoReportes.Add(new ServiceComponentList { Orden = 3, v_ComponentName = "HISTORIA OCUPACIONAL", v_ComponentId = Constants.INFORME_HISTORIA_OCUPACIONAL });

            if (ProtTipoReporte == "FICHAS COMPONENTES")
            {
                //Solo se imprime las FICHAS AUXILIARES no CAP,FMT,COIN,312,16
            }
            else
            {
                ConsolidadoReportes.Add(new ServiceComponentList { Orden = 1, v_ComponentName = "CERTIFICADO APTITUD", v_ComponentId = Constants.INFORME_CERTIFICADO_APTITUD });
                ConsolidadoReportes.Add(new ServiceComponentList { Orden = 2, v_ComponentName = "FICHA MEDICA DEL TRABAJADOR", v_ComponentId = Constants.INFORME_FICHA_MEDICA_TRABAJADOR });
                ConsolidadoReportes.Add(new ServiceComponentList { Orden = 3, v_ComponentName = "HISTORIA OCUPACIONAL", v_ComponentId = Constants.INFORME_HISTORIA_OCUPACIONAL });

                if (ProtTipoReporte == "FICHA312")
                {
                    ConsolidadoReportes.Add(new ServiceComponentList { Orden = 4, v_ComponentName = "ANEXO 312", v_ComponentId = Constants.INFORME_ANEXO_312 });
                }
                if (ProtTipoReporte == "ANEXO16")
                {
                    ConsolidadoReportes.Add(new ServiceComponentList { Orden = 5, v_ComponentName = "ANEXO 16", v_ComponentId = Constants.INFORME_ANEXO_7C });
                }
                if (ProtTipoReporte == "AMBOS")
                {
                    ConsolidadoReportes.Add(new ServiceComponentList { Orden = 4, v_ComponentName = "ANEXO 312", v_ComponentId = Constants.INFORME_ANEXO_312 });
                    ConsolidadoReportes.Add(new ServiceComponentList { Orden = 5, v_ComponentName = "ANEXO 16", v_ComponentId = Constants.INFORME_ANEXO_7C });
                }
                if (ProtTipoReporte == "MIGRACION")
                {

                }
            }

            ConsolidadoReportes.Add(new ServiceComponentList { Orden = 17, v_ComponentName = "LABORATORIO CLINICO", v_ComponentId = Constants.INFORME_LABORATORIO_CLINICO });

            ConsolidadoReportes.AddRange(serviceComponents.FindAll(p => ConsolidadoReportesForPrint.Contains(p.v_ComponentId)));


            ListaOrdenada = ConsolidadoReportes.OrderBy(p => p.Orden).ToList();
            List<string> Lista = new List<string>();

            foreach (var item in ListaOrdenada)
            {
                Lista.Add(item.v_ComponentId);
            }
            return Lista;
        }


        private void GenerateAnexo312(string pathFile, string pstrPersonId, string pstrServiceId)
        {
            var filiationData = _pacientBL.GetPacientReportEPS(pstrServiceId);
            var _listAtecedentesOcupacionales = _historyBL.GetHistoryReport(pstrPersonId);
            var _listaPatologicosFamiliares = _historyBL.GetFamilyMedicalAntecedentsReport(pstrPersonId);
            var _listMedicoPersonales = _historyBL.GetPersonMedicalHistoryReport(pstrPersonId);
            var _DataService = _serviceBL.GetServiceReport(pstrServiceId);
            var _listaHabitoNocivos = _historyBL.GetNoxiousHabitsReport(pstrPersonId);

            var Antropometria = _serviceBL.ValoresComponente(pstrServiceId, Constants.ANTROPOMETRIA_ID);
            var FuncionesVitales = _serviceBL.ValoresComponente(pstrServiceId, Constants.FUNCIONES_VITALES_ID);
            var ExamenFisico = _serviceBL.ValoresComponente(pstrServiceId, Constants.EXAMEN_FISICO_ID);
            var Oftalmologia = _serviceBL.ValoresComponente(pstrServiceId, Constants.OFTALMOLOGIA_ID);
            var Oftalmologia_UC = _serviceBL.ValoresComponentesUserControl(pstrServiceId, Constants.VISION_DE_COLORES_ID);
            var Psicologia = _serviceBL.ValoresExamenComponete(pstrServiceId, Constants.EXAMEN_PSICOLOGICO, 195);
            var OIT = _serviceBL.ValoresExamenComponete(pstrServiceId, Constants.OIT_ID, 211);
            var RX = _serviceBL.ValoresExamenComponete(pstrServiceId, Constants.RX_ID, 135);
            var Laboratorio = _serviceBL.ValoresComponente(pstrServiceId, Constants.LABORATORIO_ID);
            //var Audiometria = _serviceBL.ValoresComponente(_serviceId, Constants.AUDIOMETRIA_ID);
            var Audiometria = _serviceBL.GetDiagnosticForAudiometria(pstrServiceId, Constants.AUDIOMETRIA_ID);
            var Espirometria = _serviceBL.ValoresExamenComponete(pstrServiceId, Constants.ESPIROMETRIA_ID, 210);
            var _DiagnosticRepository = _serviceBL.GetServiceDisgnosticsReports(pstrServiceId);
            var _Recomendation = _serviceBL.GetServiceRecommendationByServiceId(pstrServiceId);
            var _ExamenesServicio = _serviceBL.GetServiceComponentsReport(pstrServiceId);
            var VISIONESTEREOSCOPICA = _serviceBL.ValoresComponente(pstrServiceId, Constants.VISION_ESTEREOSCOPICA_ID);
            var VISIONCOLORES = _serviceBL.ValoresComponente(pstrServiceId, Constants.VISION_DE_COLORES_ID);
            var MedicalCenter = _serviceBL.GetInfoMedicalCenter();
            var _Restricciones = _serviceBL.GetServiceRestriccionByServiceId(pstrServiceId);
            FichaMedicaOcupacional312.CreateFichaMedicalOcupacional312Report(_DataService,
                        filiationData, _listAtecedentesOcupacionales, _listaPatologicosFamiliares,
                        _listMedicoPersonales, _listaHabitoNocivos, Antropometria, FuncionesVitales,
                        ExamenFisico, Oftalmologia, Oftalmologia_UC, Psicologia, OIT, RX, Laboratorio, Audiometria, Espirometria,
                        _DiagnosticRepository, _Recomendation, _Restricciones, _ExamenesServicio, MedicalCenter, VISIONESTEREOSCOPICA, VISIONCOLORES,
                        pathFile);
        }

        private void GenerateInformeMedicoTrabajador(string pathFile, string pstrPersonId, string pstrServiceId)
        {
            var filiationData = _pacientBL.GetPacientReportEPS(pstrServiceId);
            var personMedicalHistory = _historyBL.GetPersonMedicalHistoryReport(pstrPersonId);
            var noxiousHabit = _historyBL.GetNoxiousHabitsReport(pstrPersonId);
            var familyMedicalAntecedent = _historyBL.GetFamilyMedicalAntecedentsReport(pstrPersonId);
            var anamnesis = _serviceBL.GetAnamnesisReport(pstrServiceId);
            var serviceComponents = _serviceBL.GetServiceComponentsReport(pstrServiceId);
            var diagnosticRepository = _serviceBL.GetServiceComponentConclusionesDxServiceIdReport(pstrServiceId);

            var MedicalCenter = _serviceBL.GetInfoMedicalCenter();

            ReportPDF.CreateMedicalReportForTheWorker(filiationData,
                                            personMedicalHistory,
                                            noxiousHabit,
                                            familyMedicalAntecedent,
                                            anamnesis,
                                            serviceComponents,
                                            diagnosticRepository,
                                            _customerOrganizationName,
                                            MedicalCenter,
                                            pathFile);

        }

        private void GenerateAnexo7C(string pathFile, string pstrPeronsId, string pstrServiceId)
        {
            var _DataService = _serviceBL.GetServiceReport(pstrServiceId);
            var _listMedicoPersonales = _historyBL.GetPersonMedicalHistoryReport(pstrPeronsId);
            var _listaPatologicosFamiliares = _historyBL.GetFamilyMedicalAntecedentsReport(pstrPeronsId);
            var _Valores = _serviceBL.GetServiceComponentsReport(pstrServiceId);
            var _listaHabitoNocivos = _historyBL.GetNoxiousHabitsReport(pstrPeronsId);
            var _PiezasCaries = _serviceBL.GetCantidadCaries(pstrServiceId, Constants.ODONTOGRAMA_ID, Constants.ODONTOGRAMA_PIEZAS_CARIES_ID);
            var _PiezasAusentes = _serviceBL.GetCantidadAusentes(pstrServiceId, Constants.ODONTOGRAMA_ID, Constants.ODONTOGRAMA_PIEZAS_AUSENTES_ID);
            var Oftalmologia_UC = _serviceBL.ValoresComponentesUserControl(pstrServiceId, Constants.VISION_DE_COLORES_ID);
            var VISIONCOLORES = _serviceBL.ValoresComponente(pstrServiceId, Constants.VISION_DE_COLORES_ID);
            var VISIONESTEREOSCOPICA = _serviceBL.ValoresComponente(pstrServiceId, Constants.VISION_ESTEREOSCOPICA_ID);
            var CuadroVacio = Common.Utils.BitmapToByteArray(Resources.CuadradoVacio);
            var CuadroCheck = Common.Utils.BitmapToByteArray(Resources.CuadradoCheck);
            var Pulmones = Common.Utils.BitmapToByteArray(Resources.MisPulmones);
            var Audiometria = _serviceBL.ValoresComponenteOdontogramaValue1(pstrServiceId, Constants.AUDIOMETRIA_ID);
            var diagnosticRepository = _serviceBL.GetServiceComponentConclusionesDxServiceIdReport(pstrServiceId);

            var MedicalCenter = _serviceBL.GetInfoMedicalCenter();

            ReportPDF.CreateAnexo7C(_DataService, _Valores, _listMedicoPersonales,
                                    _listaPatologicosFamiliares, _listaHabitoNocivos,
                                    CuadroVacio, CuadroCheck, Pulmones, _PiezasCaries,
                                    _PiezasAusentes, Oftalmologia_UC, VISIONCOLORES, VISIONESTEREOSCOPICA, Audiometria, diagnosticRepository, MedicalCenter,
                                    pathFile);

        }

        private void GenerateInformeExamenClinico(string pathFile, string pstrPeronsId, string pstrServiceId)
        {
            var filiationData = _pacientBL.GetPacientReportEPS(pstrServiceId);
            var personMedicalHistory = _historyBL.GetPersonMedicalHistoryReport(pstrPeronsId);
            var noxiousHabit = _historyBL.GetNoxiousHabitsReport(pstrPeronsId);
            var familyMedicalAntecedent = _historyBL.GetFamilyMedicalAntecedentsReport(pstrPeronsId);
            var anamnesis = _serviceBL.GetAnamnesisReport(pstrServiceId);
            var serviceComponents = _serviceBL.GetServiceComponentsReport(pstrServiceId);
            var diagnosticRepository = _serviceBL.GetServiceComponentConclusionesDxServiceIdReport(pstrServiceId);
            var doctoPhisicalExam = _serviceBL.GetDoctoPhisicalExam(pstrServiceId);

            var MedicalCenter = _serviceBL.GetInfoMedicalCenter();

            ReportPDF.CreateMedicalReportForExamenClinico(filiationData,
                                            personMedicalHistory,
                                            noxiousHabit,
                                            familyMedicalAntecedent,
                                            anamnesis,
                                            serviceComponents,
                                            diagnosticRepository,
                                            "",
                                            MedicalCenter,
                                            pathFile,
                                            doctoPhisicalExam);


        }

        private void GenerateLaboratorioReport(string pathFile, string pstrServiceId)
        {
            var MedicalCenter = _serviceBL.GetInfoMedicalCenter();
            var filiationData = _pacientBL.GetPacientReportEPS(pstrServiceId);
            var serviceComponents = _serviceBL.GetServiceComponentsReport(pstrServiceId);

            LaboratorioReport.CreateLaboratorioReport(filiationData, serviceComponents, MedicalCenter, pathFile);
        }

        private void frmGeneratePDF_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            btnGenerar_Click(sender, e);
        }


    }
}
