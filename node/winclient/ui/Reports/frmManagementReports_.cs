using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Sigesoft.Common;
using Sigesoft.Node.WinClient.BLL;
using Sigesoft.Node.WinClient.BE;
using NetPdf;
using System.IO;
using System.Diagnostics;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;


namespace Sigesoft.Node.WinClient.UI.Reports
{
    public partial class frmManagementReports_ : Form
    {
        private string _serviceId;
        private string _pacientId;
        private string _customerOrganizationName;
        private string _personFullName;
        ServiceBL _serviceBL = new ServiceBL();
        private MergeExPDF _mergeExPDF = new MergeExPDF();
        private List<string> _filesNameToMerge = null;
        PacientBL _pacientBL = new PacientBL();
        HistoryBL _historyBL = new HistoryBL();
        private string _tempSourcePath;
        string ruta;

        public frmManagementReports_(string serviceId, string pacientId, string customerOrganizationName, string personFullName, int pintFlagPantalla)
        {
            InitializeComponent();
            _serviceId = serviceId;
            _pacientId = pacientId;
            _customerOrganizationName = customerOrganizationName;
            _personFullName = personFullName;
        }

        private void frmManagementReports__Load(object sender, EventArgs e)
        {
            List<ServiceComponentList> serviceComponents = new List<ServiceComponentList>();
            List<ServiceComponentList> ListaOrdenada = new List<ServiceComponentList>();
            string[] ConsolidadoReportesForPrint = new string[] 
            {
                Constants.ALTURA_FISICA_M_18,
                Constants.ALTURA_ESTRUCTURAL_ID,
                Constants.ALTURA_7D_ID,
                Constants.ODONTOGRAMA_ID,  
               Constants.OFTALMOLOGIA,
                Constants.RX_ID,
                Constants.PRUEBA_ESFUERZO_ID,
                Constants.ELECTROCARDIOGRAMA_ID,
                Constants.TAMIZAJE_DERMATOLOGIO_ID,
                Constants.AUDIOMETRIA_ID,
                Constants.ESPIROMETRIA_ID,
                //Constants.GINECOLOGIA_ID,
                //Constants.EVALUACION_PSICOLABORAL,
                Constants.EXAMEN_PSICOLOGICO,
                Constants.PSICOLOGIA_ID,
                Constants.OIT_ID,
                Constants.EXAMEN_FISICO_ID,
                Constants.CERTIFICADO_COVID_ID,
                Constants.COVID_ID,
                Constants.CERTIFICADO_DESCENSO_COVID_ID,
                Constants.ANTIGENOS_ID
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


            // buscar los comp de oftalmo si se encuentra solo mostrar 1 item [Oftalmologia]
           

       

            serviceComponents = _serviceBL.GetServiceComponentsForManagementReport(_serviceId);
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
                if (item.v_ComponentId == Constants.ALTURA_7D_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 12;
                }
                else if (item.v_ComponentId == Constants.OFTALMOLOGIA_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 14;
                }              
                else if (item.v_ComponentId == Constants.ALTURA_ESTRUCTURAL_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 13;
                }
                else if (item.v_ComponentId == Constants.AUDIOMETRIA_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 7;
                }
                else if (item.v_ComponentId == Constants.ESPIROMETRIA_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 9;
                }
                else if (item.v_ComponentId == Constants.ESPIROMETRIA_CUESTIONARIO_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 8;
                }
                else if (item.v_ComponentId == Constants.OIT_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 15;
                }
                else if (item.v_ComponentId == Constants.ELECTROCARDIOGRAMA_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 16;
                }
                else if (item.v_ComponentId == Constants.EXAMEN_PSICOLOGICO)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 17;
                }
                else if (item.v_ComponentId == Constants.ODONTOGRAMA_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 18;
                }
                else if (item.v_ComponentId == Constants.RX_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 19;
                }
                else if (item.v_ComponentId == Constants.PRUEBA_ESFUERZO_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 20;
                }
                else if (item.v_ComponentId == Constants.ALTURA_FISICA_M_18)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 10;
                }
                else if (item.v_ComponentId == Constants.EXAMEN_FISICO_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 11;
                }

                else if (item.v_ComponentId == Constants.CERTIFICADO_COVID_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 31;
                }
                else if (item.v_ComponentId == Constants.COVID_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 31;
                }
                else if (item.v_ComponentId == Constants.CERTIFICADO_DESCENSO_COVID_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 31;
                }
                else if (item.v_ComponentId == Constants.ANTIGENOS_ID)
                {
                    var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
                    ent.Orden = 31;
                }
                
            }

            ServiceShort _serviceShort = new ServiceShort();
            _serviceShort = _serviceBL.GetServiceShort(_serviceId);
            //var SectorEmpresa = _serviceShort.EmpresaRubro.ToString();

            //List<ServiceComponentList> ConsolidadoReportes = new List<ServiceComponentList>();
            //ConsolidadoReportes.Add(new ServiceComponentList { Orden = 1, v_ComponentName = "CERTIFICADO APTITUD", v_ComponentId = Constants.INFORME_CERTIFICADO_APTITUD });
            //ConsolidadoReportes.Add(new ServiceComponentList { Orden = 5, v_ComponentName = "HISTORIA OCUPACIONAL", v_ComponentId = Constants.INFORME_HISTORIA_OCUPACIONAL });
            //if (SectorEmpresa == "MINERÍA")
            //{
            //    ConsolidadoReportes.Add(new ServiceComponentList { Orden = 4, v_ComponentName = "ANEXO 16", v_ComponentId = Constants.INFORME_ANEXO_7C });
            //}
            //else
            //{
            //    ConsolidadoReportes.Add(new ServiceComponentList { Orden = 3, v_ComponentName = "ANEXO 312", v_ComponentId = Constants.INFORME_ANEXO_312 });
            //}
            ////
            //ConsolidadoReportes.Add(new ServiceComponentList { Orden = 2, v_ComponentName = "FICHA MEDICA DEL TRABAJADOR", v_ComponentId = Constants.INFORME_FICHA_MEDICA_TRABAJADOR });
            //ConsolidadoReportes.Add(new ServiceComponentList { Orden = 6, v_ComponentName = "LABORATORIO CLINICO", v_ComponentId = Constants.INFORME_LABORATORIO_CLINICO });
            //ConsolidadoReportes.Add(new ServiceComponentList { Orden = 7, v_ComponentName = "OFTALMOLOGÍA", v_ComponentId = Constants.OFTALMOLOGIA_ID });

            //ConsolidadoReportes.AddRange(serviceComponents.FindAll(p => ConsolidadoReportesForPrint.Contains(p.v_ComponentId)));

            var ProtTipoReporte = _serviceShort.ProtTipoReporte.ToString();

            List<ServiceComponentList> ConsolidadoReportes = new List<ServiceComponentList>();

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

            chklConsolidadoReportes.DataSource = ListaOrdenada;
            chklConsolidadoReportes.DisplayMember = "v_ComponentName";
            chklConsolidadoReportes.ValueMember = "v_ComponentId";

            _tempSourcePath = Path.Combine(Application.StartupPath, "TempMerge");
        }

        private void btnConsolidadoReportes_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("¿Desea publicar a la WEB?", "ADVERTENCIA!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            string ruta = Common.Utils.GetApplicationConfigValue("rutaReportes").ToString();
            string rutaBasura = Common.Utils.GetApplicationConfigValue("rutaReportesBasura").ToString();

            if (Result == System.Windows.Forms.DialogResult.Yes)
            {
                OperationResult objOperationResult = new OperationResult();

                var Reportes = GetChekedItems(chklConsolidadoReportes);
                using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
                {
                    CrearReportesCrystal(_serviceId, Reportes, null, ruta);
                };

                var x = _filesNameToMerge.ToList();
                _mergeExPDF.FilesName = x;
                _mergeExPDF.DestinationFile = ruta +"\\"+ _serviceId + ".pdf"; ;
                _mergeExPDF.Execute();
                _mergeExPDF.RunFile();

                _serviceBL.UpdateStatusPreLiquidation(ref objOperationResult, 2, _serviceId, Globals.ClientSession.GetAsList());         
            }
            else
            {
                var Reportes = GetChekedItems(chklConsolidadoReportes);
                using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
                {
                    CrearReportesCrystal(_serviceId, Reportes, null, rutaBasura);
                };

                var x = _filesNameToMerge.ToList();
                _mergeExPDF.FilesName = x;
                _mergeExPDF.DestinationFile = rutaBasura + "\\" + _serviceId + ".pdf"; ;
                _mergeExPDF.Execute();
                _mergeExPDF.RunFile();
            }

         
         
        }
     
        private List<string> GetChekedItems(CheckedListBox chkl)
        {
            List<string> componentId = new List<string>();

            for (int i = 0; i < chkl.CheckedItems.Count; i++)
            {
                ServiceComponentList com = (ServiceComponentList)chkl.CheckedItems[i];
                componentId.Add(com.v_ComponentId);
            }

            return componentId.Count == 0 ? null : componentId;
        }

        public void CrearReportesCrystal(string serviceId, List<string> reportesId, List<ServiceComponentList> ListaDosaje, string pstrRutaReportes)
        {
            crConsolidatedReports rp = null;
            OperationResult objOperationResult = new OperationResult();
            MultimediaFileBL _multimediaFileBL = new MultimediaFileBL();
            rp = new Reports.crConsolidatedReports();
            _filesNameToMerge = new List<string>();
            foreach (var com in reportesId)
            {
                ChooseReport(com, serviceId, pstrRutaReportes);
            }

            var ListaPdf = _serviceBL.GetFilePdfsByServiceId(ref objOperationResult, _serviceId);
            //if (ListaPdf != null)
            //{
            //    if (ListaPdf.ToList().Count != 0)
            //    {
            //        foreach (var item in ListaPdf)
            //        {
            //            var multimediaFile = _multimediaFileBL.GetMultimediaFileById(ref objOperationResult, item.v_MultimediaFileId);
            //            string rutaOrigenArchivo = "";
            //            if (multimediaFile.ByteArrayFile == null)
            //            {
            //                var a = multimediaFile.FileName.Split('-');
            //                var consultorio = a[2].Substring(0, a[2].Length - 4);
            //                if (consultorio == "NEUMOLOGÍA")
            //                {
            //                    rutaOrigenArchivo = Common.Utils.GetApplicationConfigValue("ImgESPIROOrigen").ToString();
            //                }
            //                else if (consultorio == "RAYOS X")
            //                {
            //                    rutaOrigenArchivo = Common.Utils.GetApplicationConfigValue("ImgRxOrigen").ToString();
            //                }
            //                else if (consultorio == "CARDIOLOGÍA")
            //                {
            //                    rutaOrigenArchivo = Common.Utils.GetApplicationConfigValue("ImgEKGOrigen").ToString();
            //                }
            //                else if (consultorio == "LABORATORIO")
            //                {
            //                    rutaOrigenArchivo = Common.Utils.GetApplicationConfigValue("ImgLABOrigen").ToString();
            //                }
            //                else if (consultorio == "PSICOLOGÍA")
            //                {
            //                    rutaOrigenArchivo = Common.Utils.GetApplicationConfigValue("ImgPsicoOrigen").ToString();
            //                }
            //                else if (consultorio == "MEDICINA")
            //                {
            //                    rutaOrigenArchivo = Common.Utils.GetApplicationConfigValue("ImgMedicinaOrigen").ToString();
            //                }
            //                else if (consultorio == "ADMINISTRACIÓN")
            //                {
            //                    rutaOrigenArchivo = Common.Utils.GetApplicationConfigValue("ImgAdminOrigen").ToString();
            //                }

            //                if (rutaOrigenArchivo == null)
            //                {
            //                    MessageBox.Show("No se ha configurado una ruta para subir el archivo.", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                    return;
            //                }
            //                var path = rutaOrigenArchivo + item.v_FileName;
            //                //File.WriteAllBytes(path, multimediaFile.ByteArrayFile);
            //                _filesNameToMerge.Add(path);
            //            }
            //            else
            //            {
            //                var path = ruta + _serviceId + "-" + item.v_FileName;
            //                File.WriteAllBytes(path, multimediaFile.ByteArrayFile);
            //                _filesNameToMerge.Add(path);
            //            }


            //        }
            //    }
            //}

            var o = _serviceBL.GetServiceShort(serviceId);

          
            //if (ListaPdf != null)
            //{
            //    if (ListaPdf.ToList().Count != 0)
            //    {
            //        foreach (var item in ListaPdf)
            //        {
            //            var multimediaFile = _multimediaFileBL.GetMultimediaFileById(ref objOperationResult, item.v_MultimediaFileId);
            //            var path = ruta + _serviceId + "-" + item.v_FileName;
            //            File.WriteAllBytes(path, multimediaFile.ByteArrayFile);
            //            _filesNameToMerge.Add(path);

            //        }
            //    }
            //}

            //Obtner DNI y Fecha del servicio

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
            _mergeExPDF.DestinationFile = Application.StartupPath + @"\TempMerge\" + _serviceId + ".pdf"; ;
            _mergeExPDF.DestinationFile = ruta + _serviceId + ".pdf"; ;
            _mergeExPDF.Execute();

        }

        private void ChooseReport(string componentId, string psrtServiceId, string pstrRutaReportes)
        {
            DataSet dsGetRepo = null;
            DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
            OperationResult objOperationResult = new OperationResult();
            ReportDocument rp;

            switch (componentId)
            {
                case Constants.CERTIFICADO_COVID_ID:
                    List<string> _filesNameToMergeCovid19 = new List<string>();
                    string rutaCovid = Common.Utils.GetApplicationConfigValue("RutaCovid").ToString();
                    var CERTIFICADO_COVID_ID = new ServiceBL().GetCertificateCovid(ref objOperationResult, _serviceId);

                    dsGetRepo = new DataSet();

                    DataTable dt_CERTIFICADO_COVID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(CERTIFICADO_COVID_ID);
                    dt_CERTIFICADO_COVID.TableName = "dtCertificadoCovid";
                    dsGetRepo.Tables.Add(dt_CERTIFICADO_COVID);

                    rp = new Reports.crCertificadoCovid();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.CERTIFICADO_COVID_ID + ".pdf";

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
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.CERTIFICADO_COVID_ID + "_2.pdf";

                    _filesNameToMergeCovid19.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();

                    //MERGE PDF COVID
                    var reportesCovid19 = _filesNameToMergeCovid19.ToList();
                    _mergeExPDF.FilesName = reportesCovid19;
                    _mergeExPDF.DestinationFile = rutaCovid +"\\"+ _serviceId + ".pdf"; ;
                    _mergeExPDF.Execute();
                    _mergeExPDF.RunFile();


                    break;

                case Constants.COVID_ID:
                    rutaCovid = Common.Utils.GetApplicationConfigValue("RutaCovid").ToString();
                    var COVID_ID = new ServiceBL().GetCovidForWin(ref objOperationResult, _serviceId);

                    dsGetRepo = new DataSet();

                    DataTable dt_COVID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(COVID_ID);
                    dt_COVID.TableName = "dtCertificadoCovid";
                    dsGetRepo.Tables.Add(dt_COVID);

                    rp = new Reports.crCovid();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.COVID_ID + ".pdf";
                    objDiskOpt.DiskFileName = rutaCovid + psrtServiceId + "-" + Constants.COVID_ID + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();


                    break;

                case Constants.ANTIGENOS_ID:
                    rutaCovid = Common.Utils.GetApplicationConfigValue("RutaCovid").ToString();
                    var ANTIGENOS_ID = new ServiceBL().GetAntigenoResumidoForWin(ref objOperationResult, _serviceId);

                    dsGetRepo = new DataSet();

                    DataTable dt_ANTIGENO = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ANTIGENOS_ID);
                    dt_ANTIGENO.TableName = "dtCertificadoCovid";
                    dsGetRepo.Tables.Add(dt_ANTIGENO);

                    rp = new Reports.crAntigenoResumido();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ANTIGENOS_ID + ".pdf";
                    objDiskOpt.DiskFileName = rutaCovid + psrtServiceId + "-" + Constants.ANTIGENOS_ID + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();


                    break;

                case Constants.CERTIFICADO_DESCENSO_COVID_ID:
                    var CERTIFICADO_DESCENSO_COVID_ID = new ServiceBL().GetCertificateDescensoCovid(ref objOperationResult, _serviceId);

                    dsGetRepo = new DataSet();

                    DataTable dt_CERTIFICADO_DESCENSO_COVID_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(CERTIFICADO_DESCENSO_COVID_ID);
                    dt_CERTIFICADO_DESCENSO_COVID_ID.TableName = "dtCovidDescenso";
                    dsGetRepo.Tables.Add(dt_CERTIFICADO_DESCENSO_COVID_ID);

                    rp = new Reports.crCovidDescenso();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.CERTIFICADO_DESCENSO_COVID_ID + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();


                    break;

                case Constants.INFORME_CERTIFICADO_APTITUD:
                    var INFORME_CERTIFICADO_APTITUD = new ServiceBL().GetAptitudeCertificate(ref objOperationResult, _serviceId);

                    dsGetRepo = new DataSet();

                    DataTable dt_INFORME_CERTIFICADO_APTITUD = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(INFORME_CERTIFICADO_APTITUD);
                    dt_INFORME_CERTIFICADO_APTITUD.TableName = "AptitudeCertificate";
                    dsGetRepo.Tables.Add(dt_INFORME_CERTIFICADO_APTITUD);

                    rp = new Reports.crOccupationalMedicalAptitudeCertificate();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_CERTIFICADO_APTITUD + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();

                    break;

                case Constants.EXAMEN_FISICO_ID:
                    var EXAMEN_FISICO_ID = new ServiceBL().ReportMusculoEsqueletico1(_serviceId, Constants.MUSCULO_ESQUELETICO_1_ID);

                    dsGetRepo = new DataSet();

                    DataTable dt_EXAMEN_FISICO_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(EXAMEN_FISICO_ID);
                    dt_EXAMEN_FISICO_ID.TableName = "dtMusculoEsqueletico1";
                    dsGetRepo.Tables.Add(dt_EXAMEN_FISICO_ID);

                    rp = new Reports.crMusculoEsqueletico1();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    //objDiskOpt.DiskFileName = pstrRutaReportes + Constants.EXAMEN_FISICO_ID + ".pdf";
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.EXAMEN_FISICO_ID + "1.pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();

                    var EXAMEN_FISICO_ID2 = new ServiceBL().ReportMusculoEsqueletico2(_serviceId, Constants.MÚSCULO_ESQUELÉTICO_2_ID);

                    dsGetRepo = new DataSet();

                    DataTable dt_EXAMEN_FISICO_ID2 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(EXAMEN_FISICO_ID2);
                    dt_EXAMEN_FISICO_ID2.TableName = "dtMusculoEsqueletico2";
                    dsGetRepo.Tables.Add(dt_EXAMEN_FISICO_ID2);

                    rp = new Reports.crMusculoEsqueletico2();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    //objDiskOpt.DiskFileName = pstrRutaReportes + Constants.EXAMEN_FISICO_ID + "2.pdf";
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.EXAMEN_FISICO_ID + "2.pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();

                    break;

                case Constants.INFORME_HISTORIA_OCUPACIONAL:
                    var INFORME_HISTORIA_OCUPACIONAL = new ServiceBL().ReportHistoriaOcupacional(_serviceId);

                    dsGetRepo = new DataSet();

                    DataTable dt_INFORME_HISTORIA_OCUPACIONAL = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(INFORME_HISTORIA_OCUPACIONAL);
                    dt_INFORME_HISTORIA_OCUPACIONAL.TableName = "HistoriaOcupacional";
                    dsGetRepo.Tables.Add(dt_INFORME_HISTORIA_OCUPACIONAL);

                    rp = new Reports.crHistoriaOcupacional();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_HISTORIA_OCUPACIONAL + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    break;

                case Constants.ALTURA_7D_ID:
                    var AscensoAlturas = new ServiceBL().ReportAscensoGrandesAlturas(_serviceId, Constants.ALTURA_7D_ID);
                    var FuncionesVitales = new ServiceBL().ReportFuncionesVitales(_serviceId, Constants.FUNCIONES_VITALES_ID);
                    var Antropometria = new ServiceBL().ReportAntropometria(_serviceId, Constants.ANTROPOMETRIA_ID);

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

                    rp = new Reports.crAnexo7D();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ALTURA_7D_ID + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    break;

                case Constants.ALTURA_FISICA_M_18:
                    var ALTURA_FISICA_M_18 = new PacientBL().ReportAlturaFisica(_serviceId, Constants.ALTURA_FISICA_M_18);

                    dsGetRepo = new DataSet();

                    DataTable dt_ALTURA_FISICA_M_18 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ALTURA_FISICA_M_18);
                    dt_ALTURA_FISICA_M_18.TableName = "dtAlturaFisica";
                    dsGetRepo.Tables.Add(dt_ALTURA_FISICA_M_18);

                    rp = new Reports.crAlturaFisica();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ALTURA_FISICA_M_18 + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    break;

                case Constants.ELECTROCARDIOGRAMA_ID:
                    var ELECTROCARDIOGRAMA_ID = new ServiceBL().GetReportEstudioElectrocardiografico(_serviceId, Constants.ELECTROCARDIOGRAMA_ID);

                    dsGetRepo = new DataSet();

                    DataTable dt_ELECTROCARDIOGRAMA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ELECTROCARDIOGRAMA_ID);
                    dt_ELECTROCARDIOGRAMA_ID.TableName = "dtEstudioElectrocardiografico";
                    dsGetRepo.Tables.Add(dt_ELECTROCARDIOGRAMA_ID);

                    rp = new Reports.crEstudioElectrocardiografico();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ELECTROCARDIOGRAMA_ID + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    break;

                case Constants.PRUEBA_ESFUERZO_ID:
                    var PRUEBA_ESFUERZO_ID = new ServiceBL().GetReportPruebaEsfuerzo(_serviceId, Constants.PRUEBA_ESFUERZO_ID);

                    dsGetRepo = new DataSet();

                    DataTable dt_PRUEBA_ESFUERZO_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(PRUEBA_ESFUERZO_ID);
                    dt_PRUEBA_ESFUERZO_ID.TableName = "dtFichaErgonometrica";
                    dsGetRepo.Tables.Add(dt_PRUEBA_ESFUERZO_ID);

                    rp = new Reports.crFichaErgonometrica();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.PRUEBA_ESFUERZO_ID + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    break;

                case Constants.ODONTOGRAMA_ID:
                    var ruta = Application.StartupPath;
                    var ODONTOGRAMA_ID = new ServiceBL().ReportOdontograma(_serviceId, Constants.ODONTOGRAMA_ID, ruta);

                    dsGetRepo = new DataSet();

                    DataTable dt_ODONTOGRAMA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ODONTOGRAMA_ID);
                    dt_ODONTOGRAMA_ID.TableName = "dtOdontograma";
                    dsGetRepo.Tables.Add(dt_ODONTOGRAMA_ID);

                    rp = new Reports.crOdontograma();
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

                case Constants.OFTALMOLOGIA_ID:

                    var OFTALMOLOGIA = new ServiceBL().ReportOftalmologia(_serviceId, Constants.AGUDEZA_VISUAL);

                    dsGetRepo = new DataSet();

                    DataTable dt_OFTALMOLOGIA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(OFTALMOLOGIA);
                    dt_OFTALMOLOGIA_ID.TableName = "dtOFTALMOLOGIA";
                    dsGetRepo.Tables.Add(dt_OFTALMOLOGIA_ID);
                    
                    rp = new Reports.crInformeOftalmologico();
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
                    objDiskOpt.DiskFileName = pstrRutaReportes + "\\" + psrtServiceId + "-" + Constants.OFTALMOLOGIA + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    rp.Close();

                    break;

                case Constants.AUDIOMETRIA_ID:

                    var serviceBL = new ServiceBL();
                    DataSet dsAudiometria = new DataSet();

                    var dxList = serviceBL.GetDiagnosticRepositoryByComponent(_serviceId, Constants.AUDIOMETRIA_ID);

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

                    var audioUserControlList = serviceBL.ReportAudiometriaUserControl(_serviceId, Constants.AUDIOMETRIA_ID);
                    //aqui hay error corregir despues del cine
                    var audioCabeceraList = serviceBL.ReportAudiometria(_serviceId, Constants.AUDIOMETRIA_ID);

                    var dtAudiometriaUserControl = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(audioUserControlList);

                    var dtCabecera = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(audioCabeceraList);

                    dtCabecera.TableName = "dtAudiometria";
                    dtAudiometriaUserControl.TableName = "dtAudiometriaUserControl";

                    dsAudiometria.Tables.Add(dtCabecera);
                    dsAudiometria.Tables.Add(dtAudiometriaUserControl);

                    rp = new Reports.crFichaAudiometria2016();
                    rp.SetDataSource(dsAudiometria);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.AUDIOMETRIA_ID + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();


                    // Historia Ocupacional Audiometria
                    var dataListForReport_1 = new ServiceBL().ReportHistoriaOcupacionalAudiometria(_serviceId);
                    if (dataListForReport_1.Count != 0)
                    {
                        dsGetRepo = new DataSet();

                        DataTable dt_dataListForReport_1 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport_1);

                        dt_dataListForReport_1.TableName = "dtHistoriaOcupacional";

                        dsGetRepo.Tables.Add(dt_dataListForReport_1);

                        rp = new Reports.crHistoriaOcupacionalAudiometria();
                        rp.SetDataSource(dsGetRepo);
                        rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        objDiskOpt = new DiskFileDestinationOptions();
                        objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + "AUDIOMETRIA_ID_HISTORIA" + ".pdf";
                        _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                        rp.ExportOptions.DestinationOptions = objDiskOpt;
                        rp.Export();
                    }

                    break;


                //case Constants.GINECOLOGIA_ID:
                //    var GINECOLOGIA_ID = new ServiceBL().GetReportEvaluacionGinecologico(_serviceId, Constants.GINECOLOGIA_ID);
                //    dsGetRepo = new DataSet();

                //    DataTable dt_GINECOLOGIA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(GINECOLOGIA_ID);
                //    dt_GINECOLOGIA_ID.TableName = "dtEvaGinecologico";
                //    dsGetRepo.Tables.Add(dt_GINECOLOGIA_ID);

                //    rp = new Reports.crEvaluacionGenecologica();
                //    rp.SetDataSource(dsGetRepo);
                //    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                //    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //    objDiskOpt = new DiskFileDestinationOptions();
                //    objDiskOpt.DiskFileName = pstrRutaReportes + Constants.GINECOLOGIA_ID + ".pdf";
                //    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                //    rp.ExportOptions.DestinationOptions = objDiskOpt;
                //    rp.Export();
                //    break;



                //case Constants.OFT:

                //    var OFT = new ServiceBL().ReportOftalmologia(_serviceId, Constants.OFT);
                //    dsGetRepo = new DataSet();


                //    var xExploracionClinicaEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["ExploracionClinicaEstaEnProtolo"];
                //    var xVisionColoresEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["VisionColoresEstaEnProtolo"];
                //    var xVisionEstereoscopicaEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["VisionEstereoscopicaEstaEnProtolo"];
                //    var xCampoVisualEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["CampoVisualEstaEnProtolo"];
                //    var xPresionIntraocularEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["PresionIntraocularEstaEnProtolo"];
                //    var xFondoOjoEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["FondoOjoEstaEnProtolo"];
                //    var xRefraccionEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["RefraccionEstaEnProtolo"];
                //    var xAgudezaVisualEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["AgudezaVisualEstaEnProtolo"];

                //      if (xExploracionClinicaEstaEnProtolo == true)
                //          rp = new Reports.crInformeOftalmologico();                   
                //            //rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionExploracionClinica"].SectionFormat.EnableSuppress = false;

                //        if (xVisionColoresEstaEnProtolo == true)
                //            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionVisionColores"].SectionFormat.EnableSuppress = false;

                //        if (xVisionEstereoscopicaEstaEnProtolo == true)
                //            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionVisionEsteroscopica"].SectionFormat.EnableSuppress = false;

                //        if (xCampoVisualEstaEnProtolo == true)
                //            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionCampoVisual"].SectionFormat.EnableSuppress = false;

                //        if (xPresionIntraocularEstaEnProtolo == true)
                //            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionPresionIntraocular"].SectionFormat.EnableSuppress = false;

                //        if (xFondoOjoEstaEnProtolo == true)
                //            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionFondoOjo"].SectionFormat.EnableSuppress = false;

                //        if (xRefraccionEstaEnProtolo == true)
                //            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionRefraccion"].SectionFormat.EnableSuppress = false;

                //        if (xAgudezaVisualEstaEnProtolo == true)
                //            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionAgudezaVisual"].SectionFormat.EnableSuppress = false;


                //    rp.SetDataSource(dsGetRepo);
                //    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                //    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //    objDiskOpt = new DiskFileDestinationOptions();
                //    objDiskOpt.DiskFileName = pstrRutaReportes + Constants.OFT + ".pdf";
                //    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                //    rp.ExportOptions.DestinationOptions = objDiskOpt;
                //    rp.Export();
                //    break;



                //case Constants.EXAMEN_PSICOLOGICO:
                //    //dsGetRepo = new DataSet();
                //    dsGetRepo = GetReportInformePsicologicoOcupacional();
                //    rp.Subreports["crInformePsicologicoOcupacional.rpt"].SetDataSource(dsGetRepo);

                //    //var esoTypeId = (int)ds.Tables["dtOccupationalPsychologicalReport"].Rows[0]["i_EsoTypeId"];
                //    var cb_GrupoOcupacional = Convert.ToInt32(ds.Tables["dtOccupationalPsychologicalReport"].Rows[0]["cb_GrupoOcupacional"]);

                //    if (_esoTypeId == (int)TypeESO.PreOcupacional)
                //    {
                //        rp.Subreports["crInformePsicologicoOcupacional.rpt"].ReportDefinition.Sections["SectionResultadoEvaluacion_EMPO"].SectionFormat.EnableSuppress = false;
                //    }
                //    else if (_esoTypeId == (int)TypeESO.PeriodicoAnual)
                //    {
                //        if (cb_GrupoOcupacional == (int)GrupoOcupacional_EMOA_Psicologia.OperariosYTecnicos)
                //        {
                //            rp.Subreports["crInformePsicologicoOcupacional.rpt"].ReportDefinition.Sections["SectionResultadoEvaluacion_EMOA_OperariosTecnicos"].SectionFormat.EnableSuppress = false;
                //        }
                //        else if (cb_GrupoOcupacional == (int)GrupoOcupacional_EMOA_Psicologia.Conductores)
                //        {
                //            rp.Subreports["crInformePsicologicoOcupacional.rpt"].ReportDefinition.Sections["SectionResultadoEvaluacion_EMOA_Conductores"].SectionFormat.EnableSuppress = false;
                //        }
                //        else if (cb_GrupoOcupacional == (int)GrupoOcupacional_EMOA_Psicologia.Universitarios)
                //        {
                //            rp.Subreports["crInformePsicologicoOcupacional.rpt"].ReportDefinition.Sections["SectionResultadoEvaluacion_EMOA_UniversitariosAdministrativos"].SectionFormat.EnableSuppress = false;
                //        }
                //    }
                //    else if (_esoTypeId == (int)TypeESO.Retiro)
                //    {

                //    }

                //    rp.SectionPsicologia.SectionFormat.EnableSuppress = false;
                //    break;



                case Constants.RX_ID:
                    var RX_ID = new ServiceBL().ReportRadiologico(_serviceId, Constants.RX_ID);

                    dsGetRepo = new DataSet();

                    DataTable dt_RX_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(RX_ID);
                    dt_RX_ID.TableName = "dtRadiologico";
                    dsGetRepo.Tables.Add(dt_RX_ID);

                    rp = new Reports.crInformeRadiologico();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.RX_ID + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    break;


                case Constants.OIT_ID:
                    var INFORME_RADIOGRAFICO_OIT = new ServiceBL().ReportInformeRadiografico(_serviceId, Constants.OIT_ID);

                    dsGetRepo = new DataSet();

                    DataTable dt_INFORME_RADIOGRAFICO_OIT = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(INFORME_RADIOGRAFICO_OIT);
                    dt_INFORME_RADIOGRAFICO_OIT.TableName = "dtInformeRadiografico";
                    dsGetRepo.Tables.Add(dt_INFORME_RADIOGRAFICO_OIT);

                    rp = new Reports.crInformeRadiograficoOIT();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_RADIOGRAFICO_OIT + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    break;

                //case Constants.TAMIZAJE_DERMATOLOGIO_ID:
                //    var TAMIZAJE_DERMATOLOGIO_ID = new ServiceBL().ReportInformeRadiografico(_serviceId, Constants.OIT_ID);

                //    dsGetRepo = new DataSet();

                //    DataTable dt_TAMIZAJE_DERMATOLOGIO_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(TAMIZAJE_DERMATOLOGIO_ID);
                //    dt_TAMIZAJE_DERMATOLOGIO_ID.TableName = "dtInformeRadiografico";
                //    dsGetRepo.Tables.Add(dt_TAMIZAJE_DERMATOLOGIO_ID);

                //    rp = new Reports.crTamizajeDermatologico();
                //    rp.SetDataSource(dsGetRepo);
                //    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                //    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //    objDiskOpt = new DiskFileDestinationOptions();
                //    objDiskOpt.DiskFileName = pstrRutaReportes + Constants.TAMIZAJE_DERMATOLOGIO_ID + ".pdf";
                //    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                //    rp.ExportOptions.DestinationOptions = objDiskOpt;
                //    rp.Export();
                //    break;


                case Constants.ESPIROMETRIA_CUESTIONARIO_ID:
                    var ESPIROMETRIA_CUESTIONARIO_ID = new ServiceBL().GetReportCuestionarioEspirometria(_serviceId, Constants.ESPIROMETRIA_ID);

                    dsGetRepo = new DataSet();

                    DataTable dt_ESPIROMETRIA_CUESTIONARIO_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ESPIROMETRIA_CUESTIONARIO_ID);
                    dt_ESPIROMETRIA_CUESTIONARIO_ID.TableName = "dtCuestionarioEspirometria";
                    dsGetRepo.Tables.Add(dt_ESPIROMETRIA_CUESTIONARIO_ID);

                    rp = new Reports.crCuestionarioEspirometria2016();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ESPIROMETRIA_CUESTIONARIO_ID + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    break;


                case Constants.ESPIROMETRIA_ID:
                    var ESPIROMETRIA_ID = new ServiceBL().GetReportInformeEspirometria(_serviceId, Constants.ESPIROMETRIA_ID);

                    dsGetRepo = new DataSet();

                    DataTable dt_ESPIROMETRIA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ESPIROMETRIA_ID);
                    dt_ESPIROMETRIA_ID.TableName = "dtInformeEspirometria";
                    dsGetRepo.Tables.Add(dt_ESPIROMETRIA_ID);

                    rp = new Reports.crInformeEspirometria();
                    rp.SetDataSource(dsGetRepo);
                    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ESPIROMETRIA_ID + ".pdf";
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();
                    break;


                //case Constants.EVALUACION_PSICOLABORAL:
                //    var EVALUACION_PSICOLABORAL = new ServiceBL().GetReportEvaluacionPsicolaborlaPersonal(_serviceId, Constants.EVALUACION_PSICOLABORAL);

                //    dsGetRepo = new DataSet();

                //    DataTable dt_EVALUACION_PSICOLABORAL = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(EVALUACION_PSICOLABORAL);
                //    dt_EVALUACION_PSICOLABORAL.TableName = "dtEvaluacionPsicolaboralPersonal";
                //    dsGetRepo.Tables.Add(dt_EVALUACION_PSICOLABORAL);

                //    rp = new Reports.crEvaluacionPsicolaboralPersonal();
                //    rp.SetDataSource(dsGetRepo);
                //    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                //    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //    objDiskOpt = new DiskFileDestinationOptions();
                //    objDiskOpt.DiskFileName = pstrRutaReportes + Constants.EVALUACION_PSICOLABORAL + ".pdf";
                //    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                //    rp.ExportOptions.DestinationOptions = objDiskOpt;
                //    rp.Export();
                //    break;



                //case Constants.SINTOMATICO_RESPIRATORIO:
                //    var SINTOMATICO_RESPIRATORIO = new ServiceBL().ReporteSintomaticoRespiratorio(_serviceId, Constants.SINTOMATICO_RESPIRATORIO);

                //    dsGetRepo = new DataSet();

                //    DataTable dt_SINTOMATICO_RESPIRATORIO = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(SINTOMATICO_RESPIRATORIO);
                //    dt_SINTOMATICO_RESPIRATORIO.TableName = "dtSintomaticoRes";
                //    dsGetRepo.Tables.Add(dt_SINTOMATICO_RESPIRATORIO);

                //    rp = new Reports.crSintomaticoResp();
                //    rp.SetDataSource(dsGetRepo);
                //    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                //    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //    objDiskOpt = new DiskFileDestinationOptions();
                //    //objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.SINTOMATICO_RESPIRATORIO + ".pdf";
                //    objDiskOpt.DiskFileName = ruta + serviceId + "-" + Constants.SINTOMATICO_RESPIRATORIO + ".pdf";
                //    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                //    rp.ExportOptions.DestinationOptions = objDiskOpt;

                //    rp.Export();
                //    rp.Close();
                //    break;

                //case Constants.FICHA_OTOSCOPIA:
                //    var FICHA_OTOSCOPIA = new ServiceBL().GetReportFichaOtoscopia(_serviceId, Constants.FICHA_OTOSCOPIA);

                //    dsGetRepo = new DataSet();

                //    DataTable dt_FICHA_OTOSCOPIA = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(FICHA_OTOSCOPIA);
                //    dt_FICHA_OTOSCOPIA.TableName = "dtOtoscopia";
                //    dsGetRepo.Tables.Add(dt_FICHA_OTOSCOPIA);

                //    rp = new Reports.crEvaluacionNeurologica();
                //    rp.SetDataSource(dsGetRepo);
                //    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                //    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //    objDiskOpt = new DiskFileDestinationOptions();
                //    //objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.FICHA_OTOSCOPIA + ".pdf";
                //    objDiskOpt.DiskFileName = ruta + serviceId + "-" + Constants.FICHA_OTOSCOPIA + ".pdf";
                //    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                //    rp.ExportOptions.DestinationOptions = objDiskOpt;

                //    rp.Export();
                //    rp.Close();
                //    break;


                //case Constants.SOMNOLENCIA_ID:
                //    var SOMNOLENCIA_ID = new ServiceBL().ReporteSomnolencia(_serviceId, Constants.SOMNOLENCIA_ID);

                //    dsGetRepo = new DataSet();

                //    DataTable dt_SOMNOLENCIA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(SOMNOLENCIA_ID);
                //    dt_SOMNOLENCIA_ID.TableName = "dtSomnolencia";
                //    dsGetRepo.Tables.Add(dt_SOMNOLENCIA_ID);

                //    rp = new Reports.crTestEpwotrh();
                //    rp.SetDataSource(dsGetRepo);
                //    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                //    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //    objDiskOpt = new DiskFileDestinationOptions();
                //    //objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.FICHA_OTOSCOPIA + ".pdf";
                //    objDiskOpt.DiskFileName = ruta + serviceId + "-" + Constants.SOMNOLENCIA_ID + ".pdf";
                //    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                //    rp.ExportOptions.DestinationOptions = objDiskOpt;

                //    rp.Export();
                //    rp.Close();
                //    break;



                //case Constants.ACUMETRIA_ID:
                //    var ACUMETRIA_ID = new ServiceBL().ReporteAcumetria(_serviceId, Constants.ACUMETRIA_ID);

                //    dsGetRepo = new DataSet();

                //    DataTable dt_ACUMETRIA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ACUMETRIA_ID);
                //    dt_ACUMETRIA_ID.TableName = "dtAcumetria";
                //    dsGetRepo.Tables.Add(dt_ACUMETRIA_ID);

                //    rp = new Reports.crFichaAcumetria();
                //    rp.SetDataSource(dsGetRepo);
                //    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                //    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //    objDiskOpt = new DiskFileDestinationOptions();
                //    //objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.FICHA_OTOSCOPIA + ".pdf";
                //    objDiskOpt.DiskFileName = ruta + serviceId + "-" + Constants.ACUMETRIA_ID + ".pdf";
                //    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                //    rp.ExportOptions.DestinationOptions = objDiskOpt;

                //    rp.Export();
                //    rp.Close();
                //    break;


                //case Constants.EVA_ERGONOMICA_ID:
                //    var EVA_ERGONOMICA_ID = new ServiceBL().ReporteErgnomia(_serviceId, Constants.EVA_ERGONOMICA_ID);

                //    dsGetRepo = new DataSet();

                //    DataTable dt_EVA_ERGONOMICA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(EVA_ERGONOMICA_ID);
                //    dt_EVA_ERGONOMICA_ID.TableName = "dtErgonomia";
                //    dsGetRepo.Tables.Add(dt_EVA_ERGONOMICA_ID);

                //    rp = new Reports.crEvaluacionErgonomica01();
                //    rp.SetDataSource(dsGetRepo);
                //    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                //    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //    objDiskOpt = new DiskFileDestinationOptions();
                //    objDiskOpt.DiskFileName = ruta + serviceId + "-" + Constants.EVA_ERGONOMICA_ID + ".pdf";
                //    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                //    rp.ExportOptions.DestinationOptions = objDiskOpt;

                //    rp.Export();
                //    rp.Close();



                //    rp = new Reports.crEvaluacionErgonomica02();
                //    rp.SetDataSource(dsGetRepo);
                //    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                //    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //    objDiskOpt = new DiskFileDestinationOptions();
                //    objDiskOpt.DiskFileName = ruta + serviceId + "-" + "EVA_ERGONOMICA_ID2" + ".pdf";
                //    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                //    rp.ExportOptions.DestinationOptions = objDiskOpt;

                //    rp.Export();
                //    rp.Close();



                //    rp = new Reports.crEvaluacionErgonomica03();
                //    rp.SetDataSource(dsGetRepo);
                //    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                //    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //    objDiskOpt = new DiskFileDestinationOptions();
                //    objDiskOpt.DiskFileName = ruta + serviceId + "-" + "EVA_ERGONOMICA_ID3" + ".pdf";
                //    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                //    rp.ExportOptions.DestinationOptions = objDiskOpt;

                //    rp.Export();
                //    rp.Close();
                //    break;


                //case Constants.OTOSCOPIA_ID:
                //    var OTOSCOPIA_ID = new ServiceBL().ReporteOtoscopia(_serviceId, Constants.OTOSCOPIA_ID);

                //    dsGetRepo = new DataSet();

                //    DataTable dt_OTOSCOPIA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(OTOSCOPIA_ID);
                //    dt_OTOSCOPIA_ID.TableName = "dtOtoscopia";
                //    dsGetRepo.Tables.Add(dt_OTOSCOPIA_ID);

                //    rp = new Reports.crFichaOtoscopia();
                //    rp.SetDataSource(dsGetRepo);
                //    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                //    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //    objDiskOpt = new DiskFileDestinationOptions();
                //    //objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.FICHA_OTOSCOPIA + ".pdf";
                //    objDiskOpt.DiskFileName = ruta + serviceId + "-" + Constants.FICHA_OTOSCOPIA + ".pdf";
                //    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                //    rp.ExportOptions.DestinationOptions = objDiskOpt;

                //    rp.Export();
                //    rp.Close();
                //    break;

                //case Constants.SINTOMATICO_ID:
                //    var SINTOMATICO_ID = new ServiceBL().ReporteSintomaticoRespiratorio(_serviceId, Constants.SINTOMATICO_ID);

                //    dsGetRepo = new DataSet();

                //    DataTable dt_SINTOMATICO_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(SINTOMATICO_ID);
                //    dt_SINTOMATICO_ID.TableName = "dtSintomaticoRes";
                //    dsGetRepo.Tables.Add(dt_SINTOMATICO_ID);

                //    rp = new Reports.crSintomaticoResp();
                //    rp.SetDataSource(dsGetRepo);
                //    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                //    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //    objDiskOpt = new DiskFileDestinationOptions();
                //    //objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.FICHA_OTOSCOPIA + ".pdf";
                //    objDiskOpt.DiskFileName = ruta + serviceId + "-" + Constants.SINTOMATICO_ID + ".pdf";
                //    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                //    rp.ExportOptions.DestinationOptions = objDiskOpt;

                //    rp.Export();
                //    rp.Close();
                //    break;


                //case Constants.LUMBOSACRA_ID:
                //    var Componente = "";

                //    var Busqueda = ConsolidadoReportes.Find(p => p.v_ComponentId == Constants.RX_TORAX_ID);
                //    if (Busqueda == null)
                //    {
                //        Componente = Constants.LUMBOSACRA_ID;
                //    }
                //    else
                //    {
                //        Componente = Constants.RX_TORAX_ID;
                //    }
                //    var LUMBOSACRA_ID = new ServiceBL().ReporteLumboSaca(_serviceId, Componente);

                //    dsGetRepo = new DataSet();

                //    DataTable dt_LUMBOSACRA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(LUMBOSACRA_ID);
                //    dt_LUMBOSACRA_ID.TableName = "dtLumboSacra";
                //    dsGetRepo.Tables.Add(dt_LUMBOSACRA_ID);

                //    rp = new Reports.crInformeRadiologicoLumbar();
                //    rp.SetDataSource(dsGetRepo);
                //    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                //    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //    objDiskOpt = new DiskFileDestinationOptions();
                //    //objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.FICHA_OTOSCOPIA + ".pdf";
                //    objDiskOpt.DiskFileName = ruta + serviceId + "-" + Constants.LUMBOSACRA_ID + ".pdf";
                //    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                //    rp.ExportOptions.DestinationOptions = objDiskOpt;

                //    rp.Export();
                //    rp.Close();
                //    break;




                //case Constants.OJO_SECO_ID:
                //    var OJO_SECO_ID = new ServiceBL().ReporteOjoSeco(_serviceId, Constants.OJO_SECO_ID);

                //    dsGetRepo = new DataSet();

                //    DataTable dt_OJO_SECO_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(OJO_SECO_ID);
                //    dt_OJO_SECO_ID.TableName = "dtOjoSeco";
                //    dsGetRepo.Tables.Add(dt_OJO_SECO_ID);

                //    rp = new Reports.crCuestionarioOjoSeco();
                //    rp.SetDataSource(dsGetRepo);
                //    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                //    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //    objDiskOpt = new DiskFileDestinationOptions();
                //    //objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.FICHA_OTOSCOPIA + ".pdf";
                //    objDiskOpt.DiskFileName = ruta + serviceId + "-" + Constants.FICHA_OTOSCOPIA + ".pdf";
                //    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                //    rp.ExportOptions.DestinationOptions = objDiskOpt;

                //    rp.Export();
                //    rp.Close();
                //    break;

                case Constants.INFORME_ANEXO_312:
                    GenerateAnexo312(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_ANEXO_312)));
                    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_ANEXO_312)));
                    break;
                case Constants.INFORME_FICHA_MEDICA_TRABAJADOR:
                    GenerateInformeMedicoTrabajador(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_FICHA_MEDICA_TRABAJADOR)));
                    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_FICHA_MEDICA_TRABAJADOR)));
                    break;
                case Constants.INFORME_ANEXO_7C:
                    GenerateAnexo7C(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_ANEXO_7C)));
                    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_ANEXO_7C)));
                    break;
                case Constants.INFORME_CLINICO:
                    GenerateInformeExamenClinico(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_CLINICO)));
                    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_CLINICO)));
                    break;
                case Constants.INFORME_LABORATORIO_CLINICO:
                    GenerateLaboratorioReport(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_LABORATORIO_CLINICO)));
                    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_LABORATORIO_CLINICO)));
                    break;
                default:
                    break;
            }

        }

        //private void ChooseReport(string componentId)
        //{
        //    DataSet dsGetRepo = null;
        //    DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
        //    OperationResult objOperationResult = new OperationResult();
        //    ReportDocument rp;

        //    switch (componentId)
        //    {
        //        case Constants.INFORME_CERTIFICADO_APTITUD:
        //            var INFORME_CERTIFICADO_APTITUD = new ServiceBL().GetAptitudeCertificate(ref objOperationResult,_serviceId);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_INFORME_CERTIFICADO_APTITUD = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(INFORME_CERTIFICADO_APTITUD);
        //            dt_INFORME_CERTIFICADO_APTITUD.TableName = "AptitudeCertificate";
        //            dsGetRepo.Tables.Add(dt_INFORME_CERTIFICADO_APTITUD);

        //            rp = new Reports.crOccupationalMedicalAptitudeCertificate();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.INFORME_CERTIFICADO_APTITUD + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();

        //            break;

                

        //        case Constants.INFORME_HISTORIA_OCUPACIONAL:
        //            var INFORME_HISTORIA_OCUPACIONAL = new ServiceBL().ReportHistoriaOcupacional(_serviceId);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_INFORME_HISTORIA_OCUPACIONAL = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(INFORME_HISTORIA_OCUPACIONAL);
        //            dt_INFORME_HISTORIA_OCUPACIONAL.TableName = "HistoriaOcupacional";
        //            dsGetRepo.Tables.Add(dt_INFORME_HISTORIA_OCUPACIONAL);

        //            rp = new Reports.crHistoriaOcupacional();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.INFORME_HISTORIA_OCUPACIONAL + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            break;

        //        case Constants.ALTURA_7D_ID:
        //             var AscensoAlturas = new ServiceBL().ReportAscensoGrandesAlturas(_serviceId, Constants.ALTURA_7D_ID);
        //            var FuncionesVitales = new ServiceBL().ReportFuncionesVitales(_serviceId, Constants.FUNCIONES_VITALES_ID);
        //            var Antropometria = new ServiceBL().ReportAntropometria(_serviceId, Constants.ANTROPOMETRIA_ID);

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

        //            rp = new Reports.crAnexo7D();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.ALTURA_7D_ID + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            break;

        //        case Constants.ALTURA_FISICA_M_18:
        //            var ALTURA_FISICA_M_18 = new PacientBL().ReportAlturaFisica(_serviceId, Constants.ALTURA_FISICA_M_18);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_ALTURA_FISICA_M_18 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ALTURA_FISICA_M_18);
        //            dt_ALTURA_FISICA_M_18.TableName = "dtAlturaFisica";
        //            dsGetRepo.Tables.Add(dt_ALTURA_FISICA_M_18);

        //            rp = new Reports.crAlturaFisica();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.ALTURA_FISICA_M_18 + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            break;

        //        case Constants.ELECTROCARDIOGRAMA_ID:
        //            var ELECTROCARDIOGRAMA_ID = new ServiceBL().GetReportEstudioElectrocardiografico(_serviceId, Constants.ELECTROCARDIOGRAMA_ID);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_ELECTROCARDIOGRAMA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ELECTROCARDIOGRAMA_ID);
        //            dt_ELECTROCARDIOGRAMA_ID.TableName = "dtEstudioElectrocardiografico";
        //            dsGetRepo.Tables.Add(dt_ELECTROCARDIOGRAMA_ID);

        //            rp = new Reports.crEstudioElectrocardiografico();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.ELECTROCARDIOGRAMA_ID + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            break;

        //        case Constants.PRUEBA_ESFUERZO_ID:
        //            var PRUEBA_ESFUERZO_ID = new ServiceBL().GetReportPruebaEsfuerzo(_serviceId, Constants.PRUEBA_ESFUERZO_ID);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_PRUEBA_ESFUERZO_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(PRUEBA_ESFUERZO_ID);
        //            dt_PRUEBA_ESFUERZO_ID.TableName = "dtFichaErgonometrica";
        //            dsGetRepo.Tables.Add(dt_PRUEBA_ESFUERZO_ID);

        //            rp = new Reports.crFichaErgonometrica();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.PRUEBA_ESFUERZO_ID + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            break;

        //        case Constants.ODONTOGRAMA_ID:
        //            var ruta = Application.StartupPath;
        //            var ODONTOGRAMA_ID = new ServiceBL().ReportOdontograma(_serviceId, Constants.ODONTOGRAMA_ID, ruta);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_ODONTOGRAMA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ODONTOGRAMA_ID);
        //            dt_ODONTOGRAMA_ID.TableName = "dtOdontograma";
        //            dsGetRepo.Tables.Add(dt_ODONTOGRAMA_ID);

        //            rp = new Reports.crOdontograma();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.ODONTOGRAMA_ID + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            break;

        //        case Constants.AUDIOMETRIA_ID:  

        //            var serviceBL = new ServiceBL();
        //            DataSet dsAudiometria = new DataSet();

        //            var dxList = serviceBL.GetDiagnosticRepositoryByComponent(_serviceId, Constants.AUDIOMETRIA_ID);

              
        //            var dtDx = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dxList);
        //            dtDx.TableName = "dtDiagnostic";
        //            dsAudiometria.Tables.Add(dtDx);

        //            var recom = dxList.SelectMany(s1 => s1.Recomendations).ToList();

        //            var dtReco = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(recom);
        //            dtReco.TableName = "dtRecomendation";
        //            dsAudiometria.Tables.Add(dtReco);

        //            //-------******************************************************************************************

        //            var audioUserControlList = serviceBL.ReportAudiometriaUserControl(_serviceId, Constants.AUDIOMETRIA_ID);
        //            //aqui hay error corregir despues del cine
        //            var audioCabeceraList = serviceBL.ReportAudiometria(_serviceId, Constants.AUDIOMETRIA_ID);

        //            var dtAudiometriaUserControl = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(audioUserControlList);

        //            var dtCabecera = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(audioCabeceraList);

        //            dtCabecera.TableName = "dtAudiometria";
        //            dtAudiometriaUserControl.TableName = "dtAudiometriaUserControl";

        //            dsAudiometria.Tables.Add(dtCabecera);
        //            dsAudiometria.Tables.Add(dtAudiometriaUserControl);

        //            rp = new Reports.crFichaAudiometria2016();
        //            rp.SetDataSource(dsAudiometria);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.AUDIOMETRIA_ID + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();


        //            // Historia Ocupacional Audiometria
        //            var dataListForReport_1 = new ServiceBL().ReportHistoriaOcupacionalAudiometria(_serviceId);
        //            if (dataListForReport_1.Count != 0)
        //            {
        //                dsGetRepo = new DataSet();

        //                DataTable dt_dataListForReport_1 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(dataListForReport_1);

        //                dt_dataListForReport_1.TableName = "dtHistoriaOcupacional";

        //                dsGetRepo.Tables.Add(dt_dataListForReport_1);

        //                rp = new Reports.crHistoriaOcupacionalAudiometria();
        //                rp.SetDataSource(dsGetRepo);
        //                rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //                rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //                objDiskOpt = new DiskFileDestinationOptions();
        //                objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + "AUDIOMETRIA_ID_HISTORIA" + ".pdf";
        //                _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //                rp.ExportOptions.DestinationOptions = objDiskOpt;
        //                rp.Export();
        //            }
                    
        //            break;


        //        //case Constants.GINECOLOGIA_ID:
        //        //    var GINECOLOGIA_ID = new ServiceBL().GetReportEvaluacionGinecologico(_serviceId, Constants.GINECOLOGIA_ID);
        //        //    dsGetRepo = new DataSet();

        //        //    DataTable dt_GINECOLOGIA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(GINECOLOGIA_ID);
        //        //    dt_GINECOLOGIA_ID.TableName = "dtEvaGinecologico";
        //        //    dsGetRepo.Tables.Add(dt_GINECOLOGIA_ID);

        //        //    rp = new Reports.crEvaluacionGenecologica();
        //        //    rp.SetDataSource(dsGetRepo);
        //        //    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //        //    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //        //    objDiskOpt = new DiskFileDestinationOptions();
        //        //    objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.GINECOLOGIA_ID + ".pdf";
        //        //    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //        //    rp.ExportOptions.DestinationOptions = objDiskOpt;
        //        //    rp.Export();
        //        //    break;



        //        //case Constants.OFT:

        //        //    var OFT = new ServiceBL().ReportOftalmologia(_serviceId, Constants.OFT);
        //        //    dsGetRepo = new DataSet();


        //        //    var xExploracionClinicaEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["ExploracionClinicaEstaEnProtolo"];
        //        //    var xVisionColoresEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["VisionColoresEstaEnProtolo"];
        //        //    var xVisionEstereoscopicaEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["VisionEstereoscopicaEstaEnProtolo"];
        //        //    var xCampoVisualEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["CampoVisualEstaEnProtolo"];
        //        //    var xPresionIntraocularEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["PresionIntraocularEstaEnProtolo"];
        //        //    var xFondoOjoEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["FondoOjoEstaEnProtolo"];
        //        //    var xRefraccionEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["RefraccionEstaEnProtolo"];
        //        //    var xAgudezaVisualEstaEnProtolo = (bool)dsGetRepo.Tables["dtOftalmologia"].Rows[0]["AgudezaVisualEstaEnProtolo"];

        //        //      if (xExploracionClinicaEstaEnProtolo == true)
        //        //          rp = new Reports.crInformeOftalmologico();                   
        //        //            //rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionExploracionClinica"].SectionFormat.EnableSuppress = false;

        //        //        if (xVisionColoresEstaEnProtolo == true)
        //        //            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionVisionColores"].SectionFormat.EnableSuppress = false;

        //        //        if (xVisionEstereoscopicaEstaEnProtolo == true)
        //        //            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionVisionEsteroscopica"].SectionFormat.EnableSuppress = false;

        //        //        if (xCampoVisualEstaEnProtolo == true)
        //        //            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionCampoVisual"].SectionFormat.EnableSuppress = false;

        //        //        if (xPresionIntraocularEstaEnProtolo == true)
        //        //            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionPresionIntraocular"].SectionFormat.EnableSuppress = false;

        //        //        if (xFondoOjoEstaEnProtolo == true)
        //        //            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionFondoOjo"].SectionFormat.EnableSuppress = false;

        //        //        if (xRefraccionEstaEnProtolo == true)
        //        //            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionRefraccion"].SectionFormat.EnableSuppress = false;

        //        //        if (xAgudezaVisualEstaEnProtolo == true)
        //        //            rp.Subreports["crInformeOftalmologico.rpt"].ReportDefinition.Sections["SectionAgudezaVisual"].SectionFormat.EnableSuppress = false;


        //        //    rp.SetDataSource(dsGetRepo);
        //        //    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //        //    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //        //    objDiskOpt = new DiskFileDestinationOptions();
        //        //    objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.OFT + ".pdf";
        //        //    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //        //    rp.ExportOptions.DestinationOptions = objDiskOpt;
        //        //    rp.Export();
        //        //    break;



        //        //case Constants.EXAMEN_PSICOLOGICO:
        //        //    ds = GetReportInformePsicologicoOcupacional();
        //        //    rp.Subreports["crInformePsicologicoOcupacional.rpt"].SetDataSource(ds);

        //        //    //var esoTypeId = (int)ds.Tables["dtOccupationalPsychologicalReport"].Rows[0]["i_EsoTypeId"];
        //        //    var cb_GrupoOcupacional = Convert.ToInt32(ds.Tables["dtOccupationalPsychologicalReport"].Rows[0]["cb_GrupoOcupacional"]);

        //        //    if (_esoTypeId == (int)TypeESO.PreOcupacional)
        //        //    {
        //        //        rp.Subreports["crInformePsicologicoOcupacional.rpt"].ReportDefinition.Sections["SectionResultadoEvaluacion_EMPO"].SectionFormat.EnableSuppress = false;
        //        //    }
        //        //    else if (_esoTypeId == (int)TypeESO.PeriodicoAnual)
        //        //    {
        //        //        if (cb_GrupoOcupacional == (int)GrupoOcupacional_EMOA_Psicologia.OperariosYTecnicos)
        //        //        {
        //        //            rp.Subreports["crInformePsicologicoOcupacional.rpt"].ReportDefinition.Sections["SectionResultadoEvaluacion_EMOA_OperariosTecnicos"].SectionFormat.EnableSuppress = false;
        //        //        }
        //        //        else if (cb_GrupoOcupacional == (int)GrupoOcupacional_EMOA_Psicologia.Conductores)
        //        //        {
        //        //            rp.Subreports["crInformePsicologicoOcupacional.rpt"].ReportDefinition.Sections["SectionResultadoEvaluacion_EMOA_Conductores"].SectionFormat.EnableSuppress = false;
        //        //        }
        //        //        else if (cb_GrupoOcupacional == (int)GrupoOcupacional_EMOA_Psicologia.Universitarios)
        //        //        {
        //        //            rp.Subreports["crInformePsicologicoOcupacional.rpt"].ReportDefinition.Sections["SectionResultadoEvaluacion_EMOA_UniversitariosAdministrativos"].SectionFormat.EnableSuppress = false;
        //        //        }
        //        //    }
        //        //    else if (_esoTypeId == (int)TypeESO.Retiro)
        //        //    {

        //        //    }

        //        //    rp.SectionPsicologia.SectionFormat.EnableSuppress = false;
        //        //    break;



        //        case Constants.RX_ID:
        //            var RX_ID = new ServiceBL().ReportRadiologico(_serviceId, Constants.RX_ID);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_RX_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(RX_ID);
        //            dt_RX_ID.TableName = "dtRadiologico";
        //            dsGetRepo.Tables.Add(dt_RX_ID);

        //            rp = new Reports.crInformeRadiologico();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.RX_ID + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            break;


        //        case Constants.INFORME_RADIOGRAFICO_OIT:
        //            var INFORME_RADIOGRAFICO_OIT = new ServiceBL().ReportInformeRadiografico(_serviceId, Constants.OIT_ID);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_INFORME_RADIOGRAFICO_OIT = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(INFORME_RADIOGRAFICO_OIT);
        //            dt_INFORME_RADIOGRAFICO_OIT.TableName = "dtInformeRadiografico";
        //            dsGetRepo.Tables.Add(dt_INFORME_RADIOGRAFICO_OIT);

        //            rp = new Reports.crInformeRadiograficoOIT();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.INFORME_RADIOGRAFICO_OIT + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            break;

        //        //case Constants.TAMIZAJE_DERMATOLOGIO_ID:
        //        //    var TAMIZAJE_DERMATOLOGIO_ID = new ServiceBL().ReportInformeRadiografico(_serviceId, Constants.OIT_ID);

        //        //    dsGetRepo = new DataSet();

        //        //    DataTable dt_TAMIZAJE_DERMATOLOGIO_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(TAMIZAJE_DERMATOLOGIO_ID);
        //        //    dt_TAMIZAJE_DERMATOLOGIO_ID.TableName = "dtInformeRadiografico";
        //        //    dsGetRepo.Tables.Add(dt_TAMIZAJE_DERMATOLOGIO_ID);

        //        //    rp = new Reports.crTamizajeDermatologico();
        //        //    rp.SetDataSource(dsGetRepo);
        //        //    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //        //    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //        //    objDiskOpt = new DiskFileDestinationOptions();
        //        //    objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.TAMIZAJE_DERMATOLOGIO_ID + ".pdf";
        //        //    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //        //    rp.ExportOptions.DestinationOptions = objDiskOpt;
        //        //    rp.Export();
        //        //    break;


        //        case Constants.ESPIROMETRIA_CUESTIONARIO_ID:
        //            var ESPIROMETRIA_CUESTIONARIO_ID = new ServiceBL().GetReportCuestionarioEspirometria(_serviceId, Constants.ESPIROMETRIA_ID);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_ESPIROMETRIA_CUESTIONARIO_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ESPIROMETRIA_CUESTIONARIO_ID);
        //            dt_ESPIROMETRIA_CUESTIONARIO_ID.TableName = "dtCuestionarioEspirometria";
        //            dsGetRepo.Tables.Add(dt_ESPIROMETRIA_CUESTIONARIO_ID);

        //            rp = new Reports.crCuestionarioEspirometria2016();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.ESPIROMETRIA_CUESTIONARIO_ID + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            break;


        //        case Constants.ESPIROMETRIA_ID:
        //            var ESPIROMETRIA_ID = new ServiceBL().GetReportInformeEspirometria(_serviceId, Constants.ESPIROMETRIA_ID);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_ESPIROMETRIA_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(ESPIROMETRIA_ID);
        //            dt_ESPIROMETRIA_ID.TableName = "dtInformeEspirometria";
        //            dsGetRepo.Tables.Add(dt_ESPIROMETRIA_ID);

        //            rp = new Reports.crInformeEspirometria();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.ESPIROMETRIA_ID + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            break;


        //        //case Constants.EVALUACION_PSICOLABORAL:
        //        //    var EVALUACION_PSICOLABORAL = new ServiceBL().GetReportEvaluacionPsicolaborlaPersonal(_serviceId, Constants.EVALUACION_PSICOLABORAL);

        //        //    dsGetRepo = new DataSet();

        //        //    DataTable dt_EVALUACION_PSICOLABORAL = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(EVALUACION_PSICOLABORAL);
        //        //    dt_EVALUACION_PSICOLABORAL.TableName = "dtEvaluacionPsicolaboralPersonal";
        //        //    dsGetRepo.Tables.Add(dt_EVALUACION_PSICOLABORAL);

        //        //    rp = new Reports.crEvaluacionPsicolaboralPersonal();
        //        //    rp.SetDataSource(dsGetRepo);
        //        //    rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //        //    rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //        //    objDiskOpt = new DiskFileDestinationOptions();
        //        //    objDiskOpt.DiskFileName = Application.StartupPath + @"\TempMerge\" + Constants.EVALUACION_PSICOLABORAL + ".pdf";
        //        //    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //        //    rp.ExportOptions.DestinationOptions = objDiskOpt;
        //        //    rp.Export();
        //        //    break;


        //        case Constants.INFORME_ANEXO_312:
        //            GenerateAnexo312(string.Format("{0}.pdf", Path.Combine(_tempSourcePath, Constants.INFORME_ANEXO_312)));
        //            _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(_tempSourcePath, componentId)));
        //            break;
        //        case Constants.INFORME_FICHA_MEDICA_TRABAJADOR:
        //            GenerateInformeMedicoTrabajador(string.Format("{0}.pdf", Path.Combine(_tempSourcePath, Constants.INFORME_FICHA_MEDICA_TRABAJADOR)));
        //            _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(_tempSourcePath, componentId)));
        //            break;
        //        case Constants.INFORME_ANEXO_7C:
        //            GenerateAnexo7C(string.Format("{0}.pdf", Path.Combine(_tempSourcePath, Constants.INFORME_ANEXO_7C)));
        //            _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(_tempSourcePath, componentId)));
        //            break;
        //        case Constants.INFORME_CLINICO:
        //            GenerateInformeExamenClinico(string.Format("{0}.pdf", Path.Combine(_tempSourcePath, Constants.INFORME_CLINICO)));
        //            _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(_tempSourcePath, componentId)));
        //            break;
        //        case Constants.INFORME_LABORATORIO_CLINICO:
        //            GenerateLaboratorioReport(string.Format("{0}.pdf", Path.Combine(_tempSourcePath, Constants.INFORME_LABORATORIO_CLINICO)));
        //            _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(_tempSourcePath, componentId)));
        //            break;              
        //        default:
        //            break;
        //    }

        //}

        private void GenerateAnexo312(string pathFile)
        {
            var filiationData = _pacientBL.GetPacientReportEPS(_serviceId);
            var _listAtecedentesOcupacionales = _historyBL.GetHistoryReport(_pacientId);
            var _listaPatologicosFamiliares = _historyBL.GetFamilyMedicalAntecedentsReport(_pacientId);
            var _listMedicoPersonales = _historyBL.GetPersonMedicalHistoryReport(_pacientId);
            var _DataService = _serviceBL.GetServiceReport(_serviceId);
            var _listaHabitoNocivos = _historyBL.GetNoxiousHabitsReport(_pacientId);

            var Antropometria = _serviceBL.ValoresComponente(_serviceId, Constants.ANTROPOMETRIA_ID);
            var FuncionesVitales = _serviceBL.ValoresComponente(_serviceId, Constants.FUNCIONES_VITALES_ID);
            var ExamenFisico = _serviceBL.ValoresComponente(_serviceId, Constants.EXAMEN_FISICO_ID);
            var Oftalmologia = _serviceBL.ValoresComponente(_serviceId, Constants.OFTALMOLOGIA_ID);
            var VisionColores = _serviceBL.ValoresComponente(_serviceId, Constants.VISION_DE_COLORES_ID);
            var VISIONESTEREOSCOPICA = _serviceBL.ValoresComponente(_serviceId, Constants.VISION_ESTEREOSCOPICA_ID);

            var Oftalmologia_UC = _serviceBL.ValoresComponentesUserControl(_serviceId, Constants.VISION_DE_COLORES_ID);
            var Psicologia = _serviceBL.ValoresExamenComponete(_serviceId, Constants.EXAMEN_PSICOLOGICO, 195);
            var OIT = _serviceBL.ValoresExamenComponete(_serviceId, Constants.OIT_ID, 211);
            var RX = _serviceBL.ValoresExamenComponete(_serviceId, Constants.RX_ID, 135);
            var Laboratorio = _serviceBL.ValoresComponente(_serviceId, Constants.LABORATORIO_ID);
            //var Audiometria = _serviceBL.ValoresComponente(_serviceId, Constants.AUDIOMETRIA_ID);
            var Audiometria = _serviceBL.GetDiagnosticForAudiometria(_serviceId, Constants.AUDIOMETRIA_ID);
            var Espirometria = _serviceBL.ValoresExamenComponete(_serviceId, Constants.ESPIROMETRIA_ID, 210);
            var _DiagnosticRepository = _serviceBL.GetServiceDisgnosticsReports(_serviceId);
            var _Recomendation = _serviceBL.GetServiceRecommendationByServiceId(_serviceId);
            var _ExamenesServicio = _serviceBL.GetServiceComponentsReport(_serviceId);
            var _Restricciones = _serviceBL.GetServiceRestriccionByServiceId(_serviceId);
            var MedicalCenter = _serviceBL.GetInfoMedicalCenter();

            FichaMedicaOcupacional312.CreateFichaMedicalOcupacional312Report(_DataService,
                        filiationData, _listAtecedentesOcupacionales, _listaPatologicosFamiliares,
                        _listMedicoPersonales, _listaHabitoNocivos, Antropometria, FuncionesVitales,
                        ExamenFisico, VisionColores, Oftalmologia_UC, Psicologia, OIT, RX, Laboratorio, Audiometria, Espirometria,
                        _DiagnosticRepository, _Recomendation, _Restricciones, _ExamenesServicio, MedicalCenter, VISIONESTEREOSCOPICA,VisionColores,
                        pathFile);
        }

        private void GenerateInformeMedicoTrabajador(string pathFile)
        {
            var filiationData = _pacientBL.GetPacientReportEPS(_serviceId);
            var personMedicalHistory = _historyBL.GetPersonMedicalHistoryReport(_pacientId);
            var noxiousHabit = _historyBL.GetNoxiousHabitsReport(_pacientId);
            var familyMedicalAntecedent = _historyBL.GetFamilyMedicalAntecedentsReport(_pacientId);
            var anamnesis = _serviceBL.GetAnamnesisReport(_serviceId);
            var serviceComponents = _serviceBL.GetServiceComponentsReport(_serviceId);
            var diagnosticRepository = _serviceBL.GetServiceComponentConclusionesDxServiceIdReport(_serviceId);

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

        private void GenerateAnexo7C(string pathFile)
        {
            var _DataService = _serviceBL.GetServiceReport(_serviceId);
            var _listMedicoPersonales = _historyBL.GetPersonMedicalHistoryReport(_pacientId);
            var _listaPatologicosFamiliares = _historyBL.GetFamilyMedicalAntecedentsReport(_pacientId);
            var _Valores = _serviceBL.GetServiceComponentsReport(_serviceId);
            var _listaHabitoNocivos = _historyBL.GetNoxiousHabitsReport(_pacientId);
            var _PiezasCaries = _serviceBL.GetCantidadCaries(_serviceId, Constants.ODONTOGRAMA_ID, Constants.ODONTOGRAMA_PIEZAS_CARIES_ID);
            var _PiezasAusentes = _serviceBL.GetCantidadAusentes(_serviceId, Constants.ODONTOGRAMA_ID, Constants.ODONTOGRAMA_PIEZAS_AUSENTES_ID);
            var Oftalmologia_UC = _serviceBL.ValoresComponentesUserControl(_serviceId, Constants.VISION_DE_COLORES_ID);
            var VISIONCOLORES = _serviceBL.ValoresComponente(_serviceId, Constants.VISION_DE_COLORES_ID);
            var VISIONESTEREOSCOPICA = _serviceBL.ValoresComponente(_serviceId, Constants.VISION_ESTEREOSCOPICA_ID);
            var CuadroVacio = Common.Utils.BitmapToByteArray(Resources.CuadradoVacio);
            var CuadroCheck = Common.Utils.BitmapToByteArray(Resources.CuadradoCheck);
            var Pulmones = Common.Utils.BitmapToByteArray(Resources.MisPulmones);
            var Audiometria = _serviceBL.ValoresComponenteOdontogramaValue1(_serviceId, Constants.AUDIOMETRIA_ID);
            var diagnosticRepository = _serviceBL.GetServiceComponentConclusionesDxServiceIdReport(_serviceId);

            var MedicalCenter = _serviceBL.GetInfoMedicalCenter();

            ReportPDF.CreateAnexo7C(_DataService, _Valores, _listMedicoPersonales,
                                    _listaPatologicosFamiliares, _listaHabitoNocivos,
                                    CuadroVacio, CuadroCheck, Pulmones, _PiezasCaries,
                                    _PiezasAusentes, Oftalmologia_UC, VISIONCOLORES, VISIONESTEREOSCOPICA, Audiometria, diagnosticRepository, MedicalCenter,
                                    pathFile);

        }

        private void GenerateInformeExamenClinico(string pathFile)
        {
            var filiationData = _pacientBL.GetPacientReportEPS(_serviceId);
            var personMedicalHistory = _historyBL.GetPersonMedicalHistoryReport(_pacientId);
            var noxiousHabit = _historyBL.GetNoxiousHabitsReport(_pacientId);
            var familyMedicalAntecedent = _historyBL.GetFamilyMedicalAntecedentsReport(_pacientId);
            var anamnesis = _serviceBL.GetAnamnesisReport(_serviceId);
            var serviceComponents = _serviceBL.GetServiceComponentsReport(_serviceId);
            var diagnosticRepository = _serviceBL.GetServiceComponentConclusionesDxServiceIdReport(_serviceId);
            var doctoPhisicalExam = _serviceBL.GetDoctoPhisicalExam(_serviceId);

            var MedicalCenter = _serviceBL.GetInfoMedicalCenter();

            ReportPDF.CreateMedicalReportForExamenClinico(filiationData,
                                            personMedicalHistory,
                                            noxiousHabit,
                                            familyMedicalAntecedent,
                                            anamnesis,
                                            serviceComponents,
                                            diagnosticRepository,
                                            _customerOrganizationName,
                                            MedicalCenter,
                                            pathFile,
                                            doctoPhisicalExam);


        }

        private void GenerateLaboratorioReport(string pathFile)
        {
            var MedicalCenter = _serviceBL.GetInfoMedicalCenter();
            var filiationData = _pacientBL.GetPacientReportEPS(_serviceId);
            var serviceComponents = _serviceBL.GetServiceComponentsReport(_serviceId);

            LaboratorioReport.CreateLaboratorioReport(filiationData, serviceComponents, MedicalCenter, pathFile);
        }

        private void chklChekedAll(CheckedListBox chkl, bool checkedState)
        {
            for (int i = 0; i < chkl.Items.Count; i++)
            {
                chkl.SetItemChecked(i, checkedState);
            }
        }

        private void SelectChangeConsolidadoReportes()
        {
            var s = GetChekedItems(chklConsolidadoReportes);

        }

        private void chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTodos.Checked)
            {
                chklChekedAll(chklConsolidadoReportes, true);
                chklConsolidadoReportes.Enabled = false;
                SelectChangeConsolidadoReportes();
                chkTodos.Text = "Deseleccionar Todos";
            }
            else
            {
                chklConsolidadoReportes.Enabled = true;
                chklChekedAll(chklConsolidadoReportes, false);
                SelectChangeConsolidadoReportes();
                chkTodos.Text = "Seleccionar Todos";
            }
        }


    }
}
