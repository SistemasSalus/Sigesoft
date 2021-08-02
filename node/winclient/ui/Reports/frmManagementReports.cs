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

namespace Sigesoft.Node.WinClient.UI.Reports
{
    public partial class frmManagementReports : Form
    {
        #region Declarations
             
        ServiceBL _serviceBL = new ServiceBL();
        private string _serviceId;
        private MergeExPDF _mergeExPDF = new MergeExPDF();
        PacientBL _pacientBL = new PacientBL();
        HistoryBL _historyBL = new HistoryBL();
        private string _pacientId;
        private string _tempSourcePath;
        private string _customerOrganizationName;
        private SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        private string _personFullName;
        private List<string> _filesNameToMerge = null;
        private int _esoTypeId;
        //private DateTime _serviceDate;

        #endregion

        public frmManagementReports()
        {
            InitializeComponent();
        }

        public frmManagementReports(string serviceId, string pacientId, string customerOrganizationName, string personFullName, int esoTypeId)
        {
            InitializeComponent();
            _serviceId = serviceId;
            _pacientId = pacientId;
            _customerOrganizationName = customerOrganizationName;
            _personFullName = personFullName;
            _esoTypeId = esoTypeId;
            //_serviceDate = ServiceDate;
        }

        private void frmManagementReports_Load(object sender, EventArgs e)
        {
            chklFichasMedicas.SelectedValueChanged -= chklFichasMedicas_SelectedValueChanged;

            var serviceComponents = _serviceBL.GetServiceComponentsForManagementReport(_serviceId);

            #region Examen For Print
                       
            string[] examenForPrint = new string[] 
            { 
                Constants.ALTURA_FISICA_M_18,
                Constants.ALTURA_7D_ID,
                Constants.ODONTOGRAMA_ID,               
                Constants.RX_ID,
                Constants.PRUEBA_ESFUERZO_ID,
                Constants.ELECTROCARDIOGRAMA_ID,
                Constants.TAMIZAJE_DERMATOLOGIO_ID,
                //Constants.PSICOLOGIA_ID,
                Constants.AUDIOMETRIA_ID,
                Constants.ESPIROMETRIA_ID,
                //Constants.GINECOLOGIA_ID,
                //Constants.EVALUACION_PSICOLABORAL,
                Constants.EXAMEN_PSICOLOGICO,
            };

            #endregion

            #region Comp Oftalmologia
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
            #endregion
             
            #region Musculo esqueletico 1 /2
		 	       
            string[] compMusEsque = new string[]
            {    
                Constants.MUSCULO_ESQUELETICO_1_ID,
                Constants.MÚSCULO_ESQUELÉTICO_2_ID,              
            };   

             #endregion

            // buscar los comp de oftalmo si se encuentra solo mostrar 1 item [Oftalmologia]
            var oftalmo = serviceComponents.FindAll(p => compOftalmo.Contains(p.v_ComponentId));

            // buscar exa musculo esqueletico 1 o 2, si se encuentra solo mostrar 1 item [Musculo esqueletico]
            var musEsque = serviceComponents.FindAll(p => compMusEsque.Contains(p.v_ComponentId));

            // Cargar ListBox de examenes
            serviceComponents = serviceComponents.FindAll(p => examenForPrint.Contains(p.v_ComponentId));

            ServiceComponentList ent = null;

            if (oftalmo != null && oftalmo.Count > 0)
            {
                ent = new ServiceComponentList();
                ent.v_ComponentId = Constants.OFT;
                ent.v_ComponentName = "OFTALMOLOGIA";
                serviceComponents.Add(ent);
            }

            if (musEsque != null && musEsque.Count > 0)
            {
                ent = new ServiceComponentList();
                ent.v_ComponentId = Constants.MUS_ESQUE;
                ent.v_ComponentName = "Músculo esquelético";
                serviceComponents.Add(ent);
            }    

            serviceComponents.Insert(0, new ServiceComponentList { v_ComponentName = "Certificado de Aptitud", v_ComponentId = Constants.INFORME_CERTIFICADO_APTITUD });
            serviceComponents.Insert(1, new ServiceComponentList { v_ComponentName = "Historia Ocupacional", v_ComponentId = Constants.INFORME_HISTORIA_OCUPACIONAL });

            // Si la prueba de RX esta entonces tambien insertar <Informe Radiografico OIT>
            var findRX = serviceComponents.Find(p => p.v_ComponentId == Constants.RX_ID);
            var findEspiro = serviceComponents.Find(p => p.v_ComponentId == Constants.ESPIROMETRIA_ID);
            
            if (findRX != null)
            {
                var newPosition = serviceComponents.IndexOf(findRX) + 1;
                serviceComponents.Insert(newPosition, new ServiceComponentList { v_ComponentName = "Informe Radiografico OIT", v_ComponentId = Constants.INFORME_RADIOGRAFICO_OIT });
            }

            if (findEspiro != null)
            {
                var newPosition = serviceComponents.IndexOf(findEspiro) + 1;
                serviceComponents.Insert(newPosition, new ServiceComponentList { v_ComponentName = "Cuestionario de Espirometria", v_ComponentId = Constants.ESPIROMETRIA_CUESTIONARIO_ID });
            }

            chklExamenes.SelectedValueChanged -= chklExamenes_SelectedValueChanged;
            chklExamenes.DataSource = serviceComponents;
            chklExamenes.DisplayMember = "v_ComponentName";
            chklExamenes.ValueMember = "v_ComponentId";
            chklExamenes.SelectedValueChanged += chklExamenes_SelectedValueChanged;

    
            // Cargar ListBox de Fichas            
            List<ServiceComponentList> fichasMedicas = new List<ServiceComponentList>() 
            {
                new ServiceComponentList { v_ComponentName = "Anexo 312", v_ComponentId = Constants.INFORME_ANEXO_312 },
                new ServiceComponentList { v_ComponentName = "Anexo 7C", v_ComponentId = Constants.INFORME_ANEXO_7C },
                new ServiceComponentList { v_ComponentName = "Ficha Médica del trabajador", v_ComponentId = Constants.INFORME_FICHA_MEDICA_TRABAJADOR },      
                new ServiceComponentList { v_ComponentName = "Ficha Examen Clínico", v_ComponentId = Constants.INFORME_CLINICO },   
                new ServiceComponentList { v_ComponentName = "Laboratorio Clínico", v_ComponentId = Constants.INFORME_LABORATORIO_CLINICO },  
            };


            chklFichasMedicas.DataSource = fichasMedicas;
            chklFichasMedicas.DisplayMember = "v_ComponentName";
            chklFichasMedicas.ValueMember = "v_ComponentId";

            // Marcar todos los cheks

            chklChekedAll(chklExamenes, true);
            chklChekedAll(chklFichasMedicas, true);

            LoadlbAdjuntos();

            lblRecordCount1.Text = string.Format("Se encontraron {0} registros.", serviceComponents.Count());
            lblRecordCountFichaMedica.Text = string.Format("Se encontraron {0} registros.", fichasMedicas.Count());

            _tempSourcePath = Path.Combine(Application.StartupPath, "TempMerge");

            chklFichasMedicas.SelectedValueChanged += chklFichasMedicas_SelectedValueChanged;
           
           
        }

        private void chklChekedAll(CheckedListBox chkl, bool checkedState)
        {
            for (int i = 0; i < chkl.Items.Count; i++)
            {
                chkl.SetItemChecked(i, checkedState);
            }
        }

        private void rbTodosExamenes_CheckedChanged(object sender, EventArgs e)
        {
            chklChekedAll(chklExamenes, true);
            chklExamenes.Enabled = false;
            SelectChangeExamenes();
        }

        private void rbSeleccioneExamenes_CheckedChanged(object sender, EventArgs e)
        {
            chklExamenes.Enabled = true;
            chklChekedAll(chklExamenes, false);
            SelectChangeExamenes();
        }

        private void btnGenerarReporteExamenes_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            var componentId = GetChekedItems(chklExamenes);
            var frm = new Reports.frmConsolidatedReports(_serviceId, componentId, _esoTypeId);
            frm.ShowDialog();
            this.Enabled = true;
        }

        // Alejandro
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

        // Alejandro 08-12-15
        private List<string> GetServiceComponentId(CheckedListBox chkl)
        {
            List<string> serviceComponentId = new List<string>();

            for (int i = 0; i < chkl.CheckedItems.Count; i++)
            {
                ServiceComponentList com = (ServiceComponentList)chkl.CheckedItems[i];             
                serviceComponentId.Add(com.v_ServiceComponentId);
            }

            return serviceComponentId.Count == 0 ? null : serviceComponentId;
        }

        private void rbTodosFichaMedica_CheckedChanged(object sender, EventArgs e)
        {
            chklChekedAll(chklFichasMedicas, true);
            chklFichasMedicas.Enabled = false;
            SelectChangeFichasMedicas();
        }

        private void rbSeleccioneFichaMedica_CheckedChanged(object sender, EventArgs e)
        {
            chklFichasMedicas.Enabled = true;
            chklChekedAll(chklFichasMedicas, false);
            SelectChangeFichasMedicas();
        }

        private void btnGenerarReporteFichasMedicas_Click(object sender, EventArgs e)
        {
           
            // Elegir la ruta para guardar el PDF combinado
            saveFileDialog1.FileName = string.Format("{0} Ficha Médica", _personFullName);
            saveFileDialog1.Filter = "Files (*.pdf;)|*.pdf;";

            // Merge PDFs only one document
            var informesSeleccionados = GetChekedItems(chklFichasMedicas);

            if (informesSeleccionados.Count == 1)
            {
                var sfd = saveFileDialog1.ShowDialog();

                if (sfd == DialogResult.OK)
                {
                    using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
                    {
                        foreach (var item in informesSeleccionados)
                        {
                            GeneratePDFOnlyOne(item);
                        }

                        RunFile(saveFileDialog1.FileName);                      
                    }

                   
                }
               
            }
            else
            {
                _filesNameToMerge = new List<string>();

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
                    {
                        foreach (var item in informesSeleccionados)
                        {
                            GeneratePDF(item);
                        }

                        _mergeExPDF.FilesName = _filesNameToMerge;
                        _mergeExPDF.DestinationFile = saveFileDialog1.FileName;

                        _mergeExPDF.Execute();
                        _mergeExPDF.RunFile();
                    }
                }
            }                  
            
        }

        private void GeneratePDF(string componentId)
        {
            switch (componentId)
            {
                case Constants.INFORME_ANEXO_312:
                    GenerateAnexo312(string.Format("{0}.pdf", Path.Combine(_tempSourcePath, Constants.INFORME_ANEXO_312)));
                    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(_tempSourcePath, componentId)));
                    break;
                case Constants.INFORME_FICHA_MEDICA_TRABAJADOR:
                    GenerateInformeMedicoTrabajador(string.Format("{0}.pdf", Path.Combine(_tempSourcePath, Constants.INFORME_FICHA_MEDICA_TRABAJADOR)));
                    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(_tempSourcePath, componentId)));
                    break;
                case Constants.INFORME_ANEXO_7C:
                    GenerateAnexo7C(string.Format("{0}.pdf", Path.Combine(_tempSourcePath, Constants.INFORME_ANEXO_7C)));
                    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(_tempSourcePath, componentId)));
                    break;
                case Constants.INFORME_CLINICO:
                    GenerateInformeExamenClinico(string.Format("{0}.pdf", Path.Combine(_tempSourcePath, Constants.INFORME_CLINICO)));
                    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(_tempSourcePath, componentId)));
                    break;
                case Constants.INFORME_LABORATORIO_CLINICO:
                    GenerateLaboratorioReport(string.Format("{0}.pdf", Path.Combine(_tempSourcePath, Constants.INFORME_LABORATORIO_CLINICO)));
                    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(_tempSourcePath, componentId)));
                    break;
                default:
                    break;
            }
        }

        private void GeneratePDFOnlyOne(string componentId)
        {
            switch (componentId)
            {
                case Constants.INFORME_ANEXO_312:
                    GenerateAnexo312(saveFileDialog1.FileName);
                    break;
                case Constants.INFORME_FICHA_MEDICA_TRABAJADOR:
                    GenerateInformeMedicoTrabajador(saveFileDialog1.FileName);
                    break;
                case Constants.INFORME_ANEXO_7C:
                    GenerateAnexo7C(saveFileDialog1.FileName);
                    break;
                case Constants.INFORME_CLINICO:
                    GenerateInformeExamenClinico(saveFileDialog1.FileName);
                    break;
                case Constants.INFORME_LABORATORIO_CLINICO:
                    GenerateLaboratorioReport(saveFileDialog1.FileName);
                    break;
                default:
                    break;
            }
        }

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
            var Oftalmologia_UC = _serviceBL.ValoresComponentesUserControl(_serviceId, Constants.OFTALMOLOGIA_ID);
            var Psicologia = _serviceBL.ValoresExamenComponete(_serviceId, Constants.PSICOLOGIA_ID, 195);
            var OIT = _serviceBL.ValoresExamenComponete(_serviceId, Constants.OIT_ID, 211);
            var RX = _serviceBL.ValoresExamenComponete(_serviceId, Constants.RX_ID, 135);
            var Laboratorio = _serviceBL.ValoresComponente(_serviceId, Constants.LABORATORIO_ID);
            //var Audiometria = _serviceBL.ValoresComponente(_serviceId, Constants.AUDIOMETRIA_ID);
            var Audiometria = _serviceBL.GetDiagnosticForAudiometria(_serviceId, Constants.AUDIOMETRIA_ID);
            var Espirometria = _serviceBL.ValoresExamenComponete(_serviceId, Constants.ESPIROMETRIA_ID, 210);
            var _DiagnosticRepository = _serviceBL.GetServiceDisgnosticsReports(_serviceId);
            var _Recomendation = _serviceBL.GetServiceRecommendationByServiceId(_serviceId);
            var _Restricciones = _serviceBL.GetServiceRestriccionByServiceId(_serviceId);
            var _ExamenesServicio = _serviceBL.GetServiceComponentsReport(_serviceId);
            var VISIONESTEREOSCOPICA = _serviceBL.ValoresComponente(_serviceId, Constants.VISION_ESTEREOSCOPICA_ID);
            var VISIONCOLORES = _serviceBL.ValoresComponente(_serviceId, Constants.VISION_DE_COLORES_ID);
            var MedicalCenter = _serviceBL.GetInfoMedicalCenter();

            FichaMedicaOcupacional312.CreateFichaMedicalOcupacional312Report(_DataService,
                        filiationData, _listAtecedentesOcupacionales, _listaPatologicosFamiliares,
                        _listMedicoPersonales, _listaHabitoNocivos, Antropometria, FuncionesVitales,
                        ExamenFisico, Oftalmologia, Oftalmologia_UC, Psicologia, OIT, RX, Laboratorio, Audiometria, Espirometria,
                        _DiagnosticRepository, _Recomendation, _Restricciones, _ExamenesServicio, MedicalCenter, VISIONESTEREOSCOPICA,VISIONCOLORES,
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

        public void RunFile(string fileName)
        {
            Process proceso = Process.Start(fileName);
            //proceso.WaitForExit();
            proceso.Close();

        }

        private void chklExamenes_SelectedValueChanged(object sender, EventArgs e)
        {
            SelectChangeExamenes();
            
        }

        private void chklFichasMedicas_SelectedValueChanged(object sender, EventArgs e)
        {
            SelectChangeFichasMedicas();
        }

        private void SelectChangeExamenes()
        {
            var s = GetChekedItems(chklExamenes);

            if (s != null)
            {             
                btnGenerarReporteExamenes.Enabled = true;
                LoadlbAdjuntos();             
            }
            else
            {
                btnGenerarReporteExamenes.Enabled = false;
                lvAdjuntos.Items.Clear();
                lblRecordCountAdjuntos.Text = string.Format("Se encontraron {0} registros.", 0);
                btnDownloadFile.Enabled = false;
            }
        }

        private void SelectChangeFichasMedicas()
        {
            var s = GetChekedItems(chklFichasMedicas);

            if (s != null)
            {             
                btnGenerarReporteFichasMedicas.Enabled = true;
            }
            else
            {
                btnGenerarReporteFichasMedicas.Enabled = false;             
            }
        }

        private void LoadlbAdjuntos()
        {
            int nreg = 0;
            var sc = GetServiceComponentId(chklExamenes);

            OperationResult operationResult = new OperationResult();
            var multimediaFiles = new MultimediaFileBL().GetMultimediaFiles(ref operationResult, sc.ToArray());

            lvAdjuntos.Items.Clear();

            if (multimediaFiles != null && multimediaFiles.Count > 0)
            {              
                foreach (var item in multimediaFiles)
                {
                    var row = new ListViewItem(new[] { item.FileName, item.MultimediaFileId });
                    lvAdjuntos.Items.Add(row);
                }
                nreg = lvAdjuntos.Items.Count;
            }

            lblRecordCountAdjuntos.Text = string.Format("Se encontraron {0} registros.", nreg);         

            //MessageBox.Show(string.Join(",", sc));
        }

        private void lvAdjuntos_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            btnDownloadFile.Enabled = (lvAdjuntos.SelectedItems.Count > 0);
        }

        private void btnDownloadFile_Click(object sender, EventArgs e)
        {
            var selectedItem = lvAdjuntos.SelectedItems[0];
            var multimediaFileId = selectedItem.SubItems[1].Text;

            OperationResult operationResult = new OperationResult();
            var multimediaFile = new MultimediaFileBL().GetMultimediaFileByIdFromReportManager(ref operationResult, multimediaFileId);

            // Analizar el resultado de la operación
            if (operationResult.Success != 1)
            {
                MessageBox.Show(Constants.GenericErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            #region Download file
            // 
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = multimediaFile.v_FileName;
                sfd.FileName = string.Format("{0}_{1}_{2}_{3}_{4}",
                                            multimediaFile.d_ServiceDate.Value.ToString("dd-MM-yyyy"),
                                            _customerOrganizationName,
                                            multimediaFile.v_ComponentName,
                                            multimediaFile.v_DocNumber,
                                            multimediaFile.v_FileName);

                DialogResult dialogResult = sfd.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    if (String.IsNullOrEmpty(sfd.FileName))
                    {
                        MessageBox.Show("Escriba un nombre para el archivo.", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    string path = sfd.FileName;
                    File.WriteAllBytes(path, multimediaFile.b_File);
                    MessageBox.Show("Descarga completada con exito", "INFORMACION!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //Inform the user
                }
            }

            #endregion

        }

        private void btnConsolidadoReportes_Click(object sender, EventArgs e)
        {

        }

    }
}
