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
using System.IO;
using NetPdf;
using Infragistics.Win.UltraWinGrid;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using Sigesoft.Node.WinClient.UI.Reports;
using System.Runtime.InteropServices;

//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.pdf.draw;

namespace Sigesoft.Node.WinClient.UI
{
    public partial class frmService : Form
    {
        PacientBL _pacientBL = new PacientBL();
        HistoryBL _historyBL = new HistoryBL();
        private MergeExPDF _mergeExPDF = new MergeExPDF();
        private List<string> _filesNameToMerge = null;
        string strFilterExpression;
        ServiceBL _serviceBL = new ServiceBL();
        private string _serviceId;
        private string _pacientId;
        private string _protocolId;
        private string _customerOrganizationName;
        private string _personFullName;
        private int _esoTypeId;
        string ruta;
        private SaveFileDialog saveFileDialog1 = new SaveFileDialog();

        private SaveFileDialog saveFileDialog2 = new SaveFileDialog();

        public frmService()
        {
            InitializeComponent();
        }

        private void frmService_Load(object sender, EventArgs e)
        {
            #region Simular sesion
            //ClientSession objClientSession = new ClientSession();
            //objClientSession.i_SystemUserId = 1;
            //objClientSession.v_UserName = "sa";
            //objClientSession.i_CurrentExecutionNodeId = 9;
            //objClientSession.v_CurrentExecutionNodeName = "SALUS";
            ////_ClientSession.i_CurrentOrganizationId = 57;
            //objClientSession.v_PersonId = "N000-P0000000001";

            //// Pasar el objeto de sesión al gestor de objetos globales
            //Globals.ClientSession = objClientSession;
            #endregion     

            this.Width = 1240;
            this.Height = 765;

            UltraGridColumn c = grdDataService.DisplayLayout.Bands[0].Columns["b_Seleccionar"];
            c.CellActivation = Activation.AllowEdit;
            c.CellClickAction = CellClickAction.Edit;

            dtpDateTimeStar.Value = dtpDateTimeStar.Value.AddDays(-1);

            OperationResult objOperationResult = new OperationResult();

            Utils.LoadDropDownList(ddlServiceTypeId, "Value1", "Id", BLL.Utils.GetServiceType(ref objOperationResult, Globals.ClientSession.i_CurrentExecutionNodeId), DropDownListAction.All);
            Utils.LoadDropDownList(ddlMasterServiceId, "Value1", "Id", BLL.Utils.GetMasterService(ref objOperationResult, -1, Globals.ClientSession.i_CurrentExecutionNodeId), DropDownListAction.All);
            //Utils.LoadDropDownList(ddlEsoType, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, -1, null), DropDownListAction.All);

            Utils.LoadDropDownList(ddlEsoType, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 118, null), DropDownListAction.All);

            var clientOrganization = BLL.Utils.GetJoinOrganizationAndLocation(ref objOperationResult, Globals.ClientSession.i_CurrentExecutionNodeId);

            Utils.LoadDropDownList(ddlCustomerOrganization, "Value1", "Id", clientOrganization, DropDownListAction.All, true);
            Utils.LoadDropDownList(ddServiceStatusId, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 125, null), DropDownListAction.All);
            Utils.LoadDropDownList(ddlStatusAptitudId, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 124, null), DropDownListAction.All);

            Utils.LoadDropDownList(ddlProtocolId, "Value1", "Id", BLL.Utils.GetProtocolsByOrganizationForCombo(ref objOperationResult, "-1", "-1", null), DropDownListAction.All, true); 

            dtpDateTimeStar.CustomFormat = "dd/MM/yyyy";
            dptDateTimeEnd.CustomFormat = "dd/MM/yyyy";
            // Establecer el filtro inicial para los datos
            strFilterExpression = null;
        }

        private void ddlMasterServiceId_SelectedIndexChanged(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();

            if (ddlMasterServiceId.SelectedValue !=null )
            {
                var clientOrganization = BLL.Utils.GetJoinOrganizationAndLocation(ref objOperationResult, Globals.ClientSession.i_CurrentExecutionNodeId);

                if (ddlMasterServiceId.SelectedValue.ToString() == ((int)Common.MasterService.Eso).ToString())
                {
                    ddlEsoType.Enabled = true;
                    ddlProtocolId.Enabled = true;
                    Utils.LoadDropDownList(ddlCustomerOrganization, "Value1", "Id", clientOrganization, DropDownListAction.All);
          
                    //Utils.LoadDropDownList(ddlEsoType, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 118, null), DropDownListAction.All);

                }
                else
                {
                    Utils.LoadDropDownList(ddlCustomerOrganization, "Value1", "Id", clientOrganization, DropDownListAction.All);
          
                    //Utils.LoadDropDownList(ddlEsoType, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, -1, null), DropDownListAction.All);
                    ddlEsoType.Enabled = false;
                    ddlProtocolId.Enabled = false;
                }
            }

        }

        private void ddlEsoType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //OperationResult objOperationResult = new OperationResult();
            //string idOrg = String.Empty;
            //string idLoc = String.Empty;
            //if (ddlEsoType.SelectedValue !=null)
            //{
            //    if (ddlEsoType.SelectedValue.ToString() != "-1")
            //    {
            //        if (ddlCustomerOrganization.SelectedValue.ToString() == "-1") return;
            //        var dataList = ddlCustomerOrganization.SelectedValue.ToString().Split('|');
            //        idOrg = dataList[1];
            //        idLoc = dataList[2];
            //    }
            //    Utils.LoadDropDownList(ddlProtocolId, "Value1", "Id", BLL.Utils.GetProtocolByLocation(ref objOperationResult, idLoc, int.Parse(ddlEsoType.SelectedValue.ToString())), DropDownListAction.All);
       
            //}
          
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            // Get the filters from the UI
            List<string> Filters = new List<string>();
            if (!string.IsNullOrEmpty(txtPacient.Text)) Filters.Add("v_PacientDocument.Contains(\"" + txtPacient.Text.Trim() + "\")");
            if (ddServiceStatusId.SelectedValue.ToString() != "-1") Filters.Add("i_ServiceStatusId==" + ddServiceStatusId.SelectedValue);

            var id1 = ddlCustomerOrganization.SelectedValue.ToString().Split('|');

            if (ddlCustomerOrganization.SelectedValue.ToString() != "-1")
            {
                var id3 = ddlCustomerOrganization.SelectedValue.ToString().Split('|');
                Filters.Add("v_CustomerOrganizationId==" + "\"" + id3[0] + "\"&&v_CustomerLocationId==" + "\"" + id3[1] + "\"");
            }

            //if (ddlCustomerOrganization.SelectedValue.ToString() != "-1") Filters.Add("v_LocationId==" + "\"" + id1[2] + "\"");

            if (ddlMasterServiceId.SelectedValue.ToString() != "-1") Filters.Add("i_MasterServiceId==" + ddlMasterServiceId.SelectedValue);
            if (ddlServiceTypeId.SelectedValue.ToString() != "-1") Filters.Add("i_ServiceTypeId==" + ddlServiceTypeId.SelectedValue);
            if (ddlEsoType.SelectedValue.ToString() != "-1") Filters.Add("i_EsoTypeId==" + ddlEsoType.SelectedValue);
            if (ddlProtocolId.SelectedValue.ToString() != "-1") Filters.Add("v_ProtocolId=="+ "\"" + ddlProtocolId.SelectedValue + "\"");
            if (ddlStatusAptitudId.SelectedValue.ToString() != "-1") Filters.Add("i_AptitudeStatusId==" + ddlStatusAptitudId.SelectedValue);

            //if (!string.IsNullOrEmpty(txtPacient.Text)) Filters.Add("v_PacientDocument.Contains(\"" + txtComponente.Text.Trim() + "\")");
            
            // Create the Filter Expression
            strFilterExpression = null;
            if (Filters.Count > 0)
            {
                foreach (string item in Filters)
                {
                    strFilterExpression = strFilterExpression + item + " && ";
                }
                strFilterExpression = strFilterExpression.Substring(0, strFilterExpression.Length - 4);
            }

            this.BindGrid();
        }

        private void BindGrid()
        {
            var objData = GetData(0, null, "", strFilterExpression);

            grdDataService.DataSource = objData;
            lblRecordCountCalendar.Text = string.Format("Se encontraron {0} registros.", objData.Count());

            if (grdDataService.Rows.Count > 0)
                grdDataService.Rows[0].Selected = true;
        }

        private List<ServiceList> GetData(int pintPageIndex, int? pintPageSize, string pstrSortExpression, string pstrFilterExpression)
        {
            OperationResult objOperationResult = new OperationResult();
            DateTime? pdatBeginDate = dtpDateTimeStar.Value.Date;
            DateTime? pdatEndDate = dptDateTimeEnd.Value.Date.AddDays(1);

            //var _objData = _serviceBL.GetServicesPagedAndFiltered(ref objOperationResult, pintPageIndex, pintPageSize, pstrSortExpression, pstrFilterExpression, pdatBeginDate, pdatEndDate);
            var _objData = _serviceBL.GetServicesPagedAndFilteredFullNode(ref objOperationResult, pintPageIndex, pintPageSize, pstrSortExpression, pstrFilterExpression, pdatBeginDate, pdatEndDate);
            
            if (objOperationResult.Success != 1)
            {
                MessageBox.Show("Error en operación:" + System.Environment.NewLine + objOperationResult.ExceptionMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return _objData;
        }

        private void ddlOrganizationLocationId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCustomerOrganization.SelectedValue == null)
                return;

            if (ddlCustomerOrganization.SelectedValue.ToString() == "-1")
            {
                ddlProtocolId.SelectedValue = "-1";
                ddlProtocolId.Enabled = false;
                return;
            }

            ddlProtocolId.Enabled = true;

            OperationResult objOperationResult = new OperationResult();
                
            var id3 = ddlCustomerOrganization.SelectedValue.ToString().Split('|');

            //Utils.LoadDropDownList(ddlEsoType, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 118, null), DropDownListAction.All);
            Utils.LoadDropDownList(ddlProtocolId, "Value1", "Id", BLL.Utils.GetProtocolsByOrganizationForCombo(ref objOperationResult, id3[0], id3[1], null), DropDownListAction.All);          
            
        }

        private void ddlServiceTypeId_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (ddlServiceTypeId.SelectedValue.ToString() == "-1")
            {
                ddlMasterServiceId.SelectedValue = "-1";
                ddlMasterServiceId.Enabled = false;
                return;
            }

            ddlMasterServiceId.Enabled = true;
            OperationResult objOperationResult = new OperationResult();
            Utils.LoadDropDownList(ddlMasterServiceId, "Value1", "Id", BLL.Utils.GetMasterService(ref objOperationResult, int.Parse(ddlServiceTypeId.SelectedValue.ToString()), Globals.ClientSession.i_CurrentExecutionNodeId), DropDownListAction.All);
          
        }

        private void ddlMasterServiceId_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (ddlMasterServiceId.SelectedValue == null)
                return;

            if (ddlMasterServiceId.SelectedValue.ToString() == "-1")
            {
                ddlEsoType.SelectedValue = "-1";
                ddlEsoType.Enabled = false;
                return;
            }

            OperationResult objOperationResult = new OperationResult();
          
            //var clientOrganization = BLL.Utils.GetJoinOrganizationAndLocation(ref objOperationResult, Globals.ClientSession.i_CurrentExecutionNodeId);

            if (ddlMasterServiceId.SelectedValue.ToString() == ((int)Common.MasterService.Eso).ToString() ||
                ddlMasterServiceId.SelectedValue.ToString() == "12")
            {

                ddlEsoType.Enabled = true;

                //ddlProtocolId.Enabled = true;
                ddlStatusAptitudId.Enabled = true;
                //Utils.LoadDropDownList(ddlCustomerOrganization, "Value1", "Id", clientOrganization, DropDownListAction.All);

                //Utils.LoadDropDownList(ddlEsoType, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 118, null), DropDownListAction.All);

            }
            else
            {

                ddlEsoType.SelectedValue = "-1";
                ddlEsoType.Enabled = false;

                //Utils.LoadDropDownList(ddlCustomerOrganization, "Value1", "Id", clientOrganization, DropDownListAction.All);

                ////Utils.LoadDropDownList(ddlEsoType, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, -1, null), DropDownListAction.All);
                
                //ddlProtocolId.SelectedValue = "-1";
                //ddlProtocolId.Enabled = false;
                ddlStatusAptitudId.Enabled = false;
                ddlStatusAptitudId.SelectedValue = "-1";
            }
            
        }

        private void CertificadoAptitud_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmOccupationalMedicalAptitudeCertificate(_serviceId);
            frm.ShowDialog();
        }

        private void btnEditarESO_Click(object sender, EventArgs e)
        {
            //var frm = new Operations.frmEsoNew(_serviceId, "TRIAJE", "Service");
            //this.Enabled = false;
            ////frm.MdiParent = this.MdiParent;
            //frm.Show();
            //this.Enabled = true;

            Form frm = new Operations.FrmEsoV2(_serviceId, "TRIAJE", "Service", Globals.ClientSession.i_RoleId.Value, Globals.ClientSession.i_CurrentExecutionNodeId, Globals.ClientSession.i_SystemUserId);
            frm.ShowDialog();

        }

        private void grdDataService_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
        {
            btn7D.Enabled = 
            btnOdontograma.Enabled =
            btnHistoriaOcupacional.Enabled = 
            btnRadiologico.Enabled =
            btnOsteomuscular.Enabled = 
            btnPruebaEsfuerzo.Enabled = 
            btnInformeRadiologicoOIT.Enabled = 
            btnEstudioEKG.Enabled =
            btnDermatologico.Enabled = 
            btnEditarESO.Enabled = 
            btnReporteCovid19.Enabled=
            btnImprimirCertificadoAptitud.Enabled = 
            btnInformeMedicoTrabajador.Enabled =
            btnImprimirInformeMedicoEPS.Enabled = 
            btnAdminReportes.Enabled = 
            btnInforme312.Enabled = 
            btnInformeMusculoEsqueletico.Enabled = 
            btnInformeAlturaEstructural.Enabled = 
            btnInformePsicologico.Enabled = 
            btnInformeOftalmo.Enabled = 
            btnGenerarLiquidacion.Enabled = 
            //btnImprimirFichaOcupacional.Enabled = 
            (grdDataService.Selected.Rows.Count > 0);
       
            if (grdDataService.Selected.Rows.Count == 0)
                return;       

            _serviceId = grdDataService.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();
            _pacientId = grdDataService.Selected.Rows[0].Cells["v_PersonId"].Value.ToString();
            _protocolId = grdDataService.Selected.Rows[0].Cells["v_ProtocolId"].Value.ToString();
            _customerOrganizationName = grdDataService.Selected.Rows[0].Cells["v_OrganizationName"].Value.ToString();
            _personFullName = grdDataService.Selected.Rows[0].Cells["v_Pacient"].Value.ToString();

            _esoTypeId = int.Parse(grdDataService.Selected.Rows[0].Cells["i_EsoTypeId"].Value.ToString());
        }

        private void Examenes_Click(object sender, EventArgs e)
        {                        
            ReportCustom.FichaOcupacional frm = new ReportCustom.FichaOcupacional(_serviceId, _pacientId, _protocolId, (int)TypePrinter.Printer);
        }

        private void btnImprimirCertificadoAptitud_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmOccupationalMedicalAptitudeCertificate(_serviceId);
            frm.ShowDialog();
        }

        private void btnImprimirFichaOcupacional_Click(object sender, EventArgs e)
        {

        }

        private void vistaPreviaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _serviceId = grdDataService.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();
            _pacientId = grdDataService.Selected.Rows[0].Cells["v_PersonId"].Value.ToString();
            _protocolId = grdDataService.Selected.Rows[0].Cells["v_ProtocolId"].Value.ToString();
            ReportCustom.FichaOcupacional frm = new ReportCustom.FichaOcupacional(_serviceId, _pacientId, _protocolId, (int)TypePrinter.Image);
        }

        private void dtpDateTimeStar_Validating(object sender, CancelEventArgs e)
        {
            if (dtpDateTimeStar.Value.Date > dptDateTimeEnd.Value.Date)
            {
                e.Cancel = true;
                MessageBox.Show("El campo fecha Inicial no puede ser Mayor a la fecha final.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void dptDateTimeEnd_Validating(object sender, CancelEventArgs e)
        {
            if (dptDateTimeEnd.Value.Date < dtpDateTimeStar.Value.Date)
            {
                e.Cancel = true;
                MessageBox.Show("El campo fecha Final no puede ser Menor a la fecha Inicial.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void txtPacient_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnFilter_Click(null, null);
            }
        }

        private void btnImprimirInformeMedicoEPS_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "Informe Médico EPS";
            saveFileDialog1.Filter = "Files (*.pdf;)|*.pdf;";

            try
            {
                
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                   
                    using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
                    {
                        this.Enabled = false;                    

                        var filiationData = _pacientBL.GetPacientReportEPS(_serviceId);

                        var personMedicalHistory = _historyBL.GetPersonMedicalHistoryReport(_pacientId);

                        var noxiousHabit = _historyBL.GetNoxiousHabitsReport(_pacientId);

                        var familyMedicalAntecedent = _historyBL.GetFamilyMedicalAntecedentsReport(_pacientId);

                        var anamnesis = _serviceBL.GetAnamnesisReport(_serviceId);

                        var serviceComponents = _serviceBL.GetServiceComponentsReport(_serviceId);

                        var diagnosticRepository = _serviceBL.GetServiceComponentConclusionesDxServiceIdReport(_serviceId);

                        ReportPDF.CreateMedicalReportEPS(filiationData,
                                                        personMedicalHistory,
                                                        noxiousHabit,
                                                        familyMedicalAntecedent,
                                                        anamnesis,
                                                        serviceComponents,
                                                        diagnosticRepository,
                                                        saveFileDialog1.FileName);

                        this.Enabled = true;
                    }

                  

                }  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }         
   
        }

        private void btnInformeMedicoTrabajador_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = string.Format("{0} Informe Resumen", _personFullName);
            saveFileDialog1.Filter = "Files (*.pdf;)|*.pdf;";

            //try
            //{

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {               
                    using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
                    {
                        this.Enabled = false;
                      
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
                                                        saveFileDialog1.FileName);

                        this.Enabled = true;
                    }
                                  
                }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.Enabled = true;
            //}              
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var inside = _serviceBL.IsPsicoExamIntoServiceComponent(_serviceId);

            if (!inside)
            {
                MessageBox.Show("El examen de Psicologia no aplica a la atención", "INFORMACIÓN!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var frm = new Reports.frmInformePsicologicoOcupacional(_serviceId);
            frm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog2.FileName = string.Format("{0} 312", _personFullName);         
            saveFileDialog2.Filter = "Files (*.pdf;)|*.pdf;";

            //try
            //{
                if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                {
                    using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
                    {
                        this.Enabled = false;

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
                        var Oftalmologia_UC = _serviceBL.ValoresComponentesUserControl(_serviceId, Constants.AGUDEZA_VISUAL);
                        var Psicologia = _serviceBL.ValoresExamenComponete(_serviceId, Constants.PSICOLOGIA_ID, 195);
                        var RX = _serviceBL.ValoresExamenComponete(_serviceId, Constants.RX_ID, 211);
                        var RX1 = _serviceBL.ValoresExamenComponete(_serviceId, Constants.RX_ID, 135);
                        var Laboratorio = _serviceBL.ValoresComponente(_serviceId, Constants.LABORATORIO_ID);
                        //var Audiometria = _serviceBL.ValoresComponente(_serviceId, Constants.AUDIOMETRIA_ID);
                        var Audiometria = _serviceBL.GetDiagnosticForAudiometria(_serviceId, Constants.AUDIOMETRIA_ID);
                        var Espirometria = _serviceBL.ValoresExamenComponete(_serviceId, Constants.ESPIROMETRIA_ID, 210);
                        var _DiagnosticRepository = _serviceBL.GetServiceDisgnosticsReports(_serviceId);
                        var _Recomendation = _serviceBL.GetServiceRecommendationByServiceId(_serviceId);
                        var _ExamenesServicio = _serviceBL.GetServiceComponentsReport(_serviceId);

                        var ElectroCardiograma = _serviceBL.ValoresComponente(_serviceId, Constants.ELECTROCARDIOGRAMA_ID);
                        var PruebaEsfuerzo = _serviceBL.ValoresComponente(_serviceId, Constants.PRUEBA_ESFUERZO_ID);
                        var Altura7D = _serviceBL.ValoresComponente(_serviceId, Constants.ALTURA_7D_ID);
                        var AlturaEstructural = _serviceBL.ValoresComponente(_serviceId, Constants.ALTURA_ESTRUCTURAL_ID);
                        var Odontologia = _serviceBL.ValoresComponente(_serviceId, Constants.ODONTOGRAMA_ID);
                        var OsteoMuscular = _serviceBL.ValoresComponente(_serviceId, Constants.OSTEO_MUSCULAR_ID);
                        var VISIONESTEREOSCOPICA = _serviceBL.ValoresComponente(_serviceId, Constants.VISION_ESTEREOSCOPICA_ID);
                        var VISIONCOLORES = _serviceBL.ValoresComponente(_serviceId, Constants.VISION_DE_COLORES_ID);
                        var MedicalCenter = _serviceBL.GetInfoMedicalCenter();
                        var _Restricciones = _serviceBL.GetServiceRestriccionByServiceId(_serviceId);
                        FichaMedicaOcupacional312.CreateFichaMedicalOcupacional312Report(_DataService,
                                  filiationData, _listAtecedentesOcupacionales, _listaPatologicosFamiliares,
                                  _listMedicoPersonales, _listaHabitoNocivos, Antropometria, FuncionesVitales,
                                  ExamenFisico, Oftalmologia, Oftalmologia_UC, Psicologia, RX, RX1, Laboratorio, Audiometria, Espirometria,
                                  _DiagnosticRepository, _Recomendation, _Restricciones, _ExamenesServicio, MedicalCenter, VISIONESTEREOSCOPICA,VISIONCOLORES,                 
                                  saveFileDialog2.FileName);

                        this.Enabled = true;
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            //}

        }
     
        private void button3_Click(object sender, EventArgs e)
        {
           
            //var frm = new Reports.frmMusculoesqueletico(_serviceId, Constants.OSTEO_MUSCULAR_ID);
            //frm.ShowDialog();
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmAlturaEstructural(_serviceId, Constants.ALTURA_ESTRUCTURAL_ID);
            frm.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmOftalmologia(_serviceId, Constants.OFTALMOLOGIA_ID);
            frm.ShowDialog();
        }

        private void btn7D_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmAnexo7D(_serviceId, Constants.ALTURA_7D_ID);
            frm.ShowDialog();
        }

        private void btnOdontograma_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmOdontograma(_serviceId, Constants.ODONTOGRAMA_ID);
            frm.ShowDialog();
        }

        private void btnHistoriaOcupacional_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmHistoriaOcupacional(_serviceId);
            frm.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmRadiologico(_serviceId, Constants.RX_ID);
            frm.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void btnRadiologico_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmRadiologico(_serviceId, Constants.RX_ID);
            frm.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmOsteomuscular(_serviceId, Constants.OSTEO_MUSCULAR_ID);
            frm.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmPruebaEsfuerzo(_serviceId, Constants.PRUEBA_ESFUERZO_ID);
            frm.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmInformeRadiograficoOIT(_serviceId, Constants.RX_ID);
            frm.ShowDialog();
            
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            var frm = new Reports.frmEstudioElectrocardiografico(_serviceId, Constants.ELECTROCARDIOGRAMA_ID);
            frm.ShowDialog();
        }

        private void btnConsolidadoReportes_Click(object sender, EventArgs e)
        {
                var frm = new Reports.frmManagementReports_(_serviceId, _pacientId, _customerOrganizationName, _personFullName, _esoTypeId);
                frm.MdiParent = this.MdiParent;
                frm.Show(); ;
                btnFilter_Click(sender, e);
        }

        private void btnDermatologico_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmTamizajeDermatologico(_serviceId, Constants.TAMIZAJE_DERMATOLOGIO_ID);
            frm.ShowDialog();
        }

        private void btnGenerarLiquidacion_Click(object sender, EventArgs e)
        {
            var service = new ServiceList();

            service.v_ServiceId = _serviceId;
            service.v_ProtocolName = grdDataService.Selected.Rows[0].Cells["v_ProtocolName"].Value.ToString();
            service.v_CustomerOrganizationName = _customerOrganizationName;
            service.v_Pacient = _personFullName;
            service.v_MasterServiceName = grdDataService.Selected.Rows[0].Cells["v_MasterServiceName"].Value.ToString();
            service.v_EsoTypeName = grdDataService.Selected.Rows[0].Cells["v_EsoTypeName"].Value.ToString();
            service.i_StatusLiquidation = (int?)grdDataService.Selected.Rows[0].Cells["i_StatusLiquidation"].Value;

            var frm = new frmBeforeLiquidationProcess(service);
            frm.ShowDialog();

            if (frm.DialogResult == DialogResult.Cancel)
                return;
            
            BindGrid();

        }

        private void grdDataService_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["i_StatusLiquidation"].Value == null)
                return;

            if ((int)e.Row.Cells["i_StatusLiquidation"].Value == (int)PreLiquidationStatus.Generada)
            {
                e.Row.Cells["Liq"].Value = Resources.accept;
                e.Row.Cells["Liq"].ToolTipText = "Generada";
            }

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void grdDataService_ClickCell(object sender, ClickCellEventArgs e)
        {
            if ((e.Cell.Column.Key == "b_Seleccionar"))
            {
                if ((e.Cell.Value.ToString() == "False"))
                {
                    e.Cell.Value = true;
                }
                else
                {
                    e.Cell.Value = false;
                }

            }
        }

        private void btnMasivos_Click(object sender, EventArgs e)
        {          
            foreach (var item in grdDataService.Rows)
            {
                if ((bool)item.Cells["b_Seleccionar"].Value)
                {
                    string serviceId = item.Cells["v_ServiceId"].Value.ToString();
                    string personId = item.Cells["v_PersonId"].Value.ToString();
                    string Paciente = item.Cells["v_Pacient"].Value.ToString();

                    GenerarReportes(serviceId, personId, Paciente, "MAS");
                    OperationResult objOperationResult = new OperationResult();

                    _serviceBL.UpdateStatusPreLiquidation(ref objOperationResult, 2, serviceId, Globals.ClientSession.GetAsList());

                }

            }
            btnFilter_Click(sender, e);
            MessageBox.Show("Generación Correcta.", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCAPs_Click(object sender, EventArgs e)
        {
            foreach (var item in grdDataService.Rows)
            {
                if ((bool)item.Cells["b_Seleccionar"].Value)
                {
                    string serviceId = item.Cells["v_ServiceId"].Value.ToString();
                    string personId = item.Cells["v_PersonId"].Value.ToString();
                    string Paciente = item.Cells["v_Pacient"].Value.ToString();

                    GenerarReportes(serviceId, personId, Paciente, "CAP");
                }
            }
            btnFilter_Click(sender, e);
            MessageBox.Show("Generación de CAPs Correcta.", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnFMTs_Click(object sender, EventArgs e)
        {
            foreach (var item in grdDataService.Rows)
            {
                if ((bool)item.Cells["b_Seleccionar"].Value)
                {
                    string serviceId = item.Cells["v_ServiceId"].Value.ToString();
                    string personId = item.Cells["v_PersonId"].Value.ToString();
                    string Paciente = item.Cells["v_Pacient"].Value.ToString();

                    GenerarReportes(serviceId, personId, Paciente, "FMT");
                }
            }
            btnFilter_Click(sender, e);
            MessageBox.Show("Generación de FMTs Correcta.", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void GenerarReportes(string serviceId, string personId, string pstrPaciente, string pstrbtn)
        {
            string ruta = Common.Utils.GetApplicationConfigValue("rutaReportes").ToString();
            OperationResult objOperationResult = new OperationResult();

            string btn = pstrbtn;
            switch (btn)
            { 
                case "MAS":
                    using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
                    {
                        CrearReportesCrystal(serviceId,personId, DevolverListaReportes(serviceId), null, ruta, btn);
                    };
                   
                    var x = _filesNameToMerge.ToList();
                    _mergeExPDF.FilesName = x;
                    _mergeExPDF.DestinationFile = ruta + serviceId + "_" + pstrPaciente + ".pdf"; ;
                    _mergeExPDF.Execute();
                    //_mergeExPDF.RunFile();
                    //_serviceBL.UpdateStatusPreLiquidation(ref objOperationResult, 2, _serviceId, Globals.ClientSession.GetAsList());  

                    break;

                case "CAP":
                    using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
                    {
                        CrearReportesCrystal(serviceId,personId, DevolverListaCAPs(serviceId), null, ruta, btn);
                    };
                    break;

                case "FMT":
                    using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
                    {
                        CrearReportesCrystal(serviceId,personId, DevolverListaFMTs(serviceId), null, ruta, btn);
                    };
                    break;
            }

        }

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
                    //Aun esta pendiente esta lógica
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

        List<string> DevolverListaCAPs(string pstrServiceId)
        {
            List<ServiceComponentList> serviceComponents = new List<ServiceComponentList>();
            List<ServiceComponentList> ListaOrdenada = new List<ServiceComponentList>();
            
            serviceComponents = _serviceBL.GetServiceComponentsForManagementReport(pstrServiceId);

            List<ServiceComponentList> ConsolidadoReportes = new List<ServiceComponentList>();
            ConsolidadoReportes.Add(new ServiceComponentList { Orden = 1, v_ComponentName = "CERTIFICADO APTITUD", v_ComponentId = Constants.INFORME_CERTIFICADO_APTITUD });
            
            ListaOrdenada = ConsolidadoReportes.OrderBy(p => p.Orden).ToList();
            List<string> Lista = new List<string>();

            foreach (var item in ListaOrdenada)
            {
                Lista.Add(item.v_ComponentId);
            }
            return Lista;
        }

        List<string> DevolverListaFMTs(string pstrServiceId)
        {
            List<ServiceComponentList> serviceComponents = new List<ServiceComponentList>();
            List<ServiceComponentList> ListaOrdenada = new List<ServiceComponentList>();
            
            serviceComponents = _serviceBL.GetServiceComponentsForManagementReport(pstrServiceId);
            
            List<ServiceComponentList> ConsolidadoReportes = new List<ServiceComponentList>();
            ConsolidadoReportes.Add(new ServiceComponentList { Orden = 2, v_ComponentName = "FICHA MEDICA DEL TRABAJADOR", v_ComponentId = Constants.INFORME_FICHA_MEDICA_TRABAJADOR });

            ListaOrdenada = ConsolidadoReportes.OrderBy(p => p.Orden).ToList();
            List<string> Lista = new List<string>();

            foreach (var item in ListaOrdenada)
            {
                Lista.Add(item.v_ComponentId);
            }
            return Lista;
        }

        public void CrearReportesCrystal(string serviceId, string personId, List<string> reportesId, List<ServiceComponentList> ListaDosaje, string pstrRutaReportes, string pstrbtn)
        {
            crConsolidatedReports rp = null;
            OperationResult objOperationResult = new OperationResult();
            MultimediaFileBL _multimediaFileBL = new MultimediaFileBL();
            rp = new Reports.crConsolidatedReports();
            _filesNameToMerge = new List<string>();
            foreach (var com in reportesId)
            {
                ChooseReport(com, serviceId, pstrRutaReportes, personId);
            }

            var ListaPdf = _serviceBL.GetFilePdfsByServiceId(ref objOperationResult, _serviceId);


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
            _mergeExPDF.DestinationFile = Application.StartupPath + @"\TempMerge\" + _serviceId + ".pdf"; ;
            _mergeExPDF.DestinationFile = ruta + _serviceId + ".pdf"; ;
            _mergeExPDF.Execute();
         
        }

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

                    rp = new Reports.crOccupationalMedicalAptitudeCertificate();
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
                    //rp = new Reports.crConsentimiento();
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
                    //Adicional para limpiar la Memoria RAM
                    rp.Close();

                    var EXAMEN_FISICO_ID2 = new ServiceBL().ReportMusculoEsqueletico2(psrtServiceId, Constants.MÚSCULO_ESQUELÉTICO_2_ID);

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

                    rp = new Reports.crHistoriaOcupacional();
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

                    rp = new Reports.crAnexo7D();
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

                    rp = new Reports.crAlturaFisica();
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

                    rp = new Reports.crEstudioElectrocardiografico();
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

                    rp = new Reports.crFichaErgonometrica();
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

                    rp = new Reports.crFichaAudiometria2016();
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

                        rp = new Reports.crHistoriaOcupacionalAudiometria();
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

                    rp = new Reports.crInformeRadiologico();
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

                    rp = new Reports.crInformeRadiograficoOIT();
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

                    rp = new Reports.crCuestionarioEspirometria2016();
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

                    rp = new Reports.crInformeEspirometria();
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
                    rp = new Reports.crConsentimiento();
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
                    GenerateLaboratorioReport(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_LABORATORIO_CLINICO)),  psrtServiceId);
                    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_LABORATORIO_CLINICO)));
                    break;
                #endregion

                default:
                    break;
            }

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

            ReportPDF.CreateAnexo7C(_DataService, _Valores, _listMedicoPersonales, _listaPatologicosFamiliares, _listaHabitoNocivos,
                                    CuadroVacio, CuadroCheck, Pulmones, _PiezasCaries, _PiezasAusentes, Oftalmologia_UC, VISIONCOLORES, VISIONESTEREOSCOPICA,
                                    Audiometria, diagnosticRepository, MedicalCenter, pathFile);

        }

        private void GenerateInformeExamenClinico(string pathFile,string pstrPeronsId, string pstrServiceId)
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

        private void btnDescargarImagenes_Click(object sender, EventArgs e)
        {
            using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
            {
                //Obtener Lista de MultifileId
                OperationResult operationResult = new OperationResult();
                MultimediaFileBL oMultimediaFileBL = new MultimediaFileBL();
                var Lista = oMultimediaFileBL.DevolverTodosArchivos();
                foreach (var item in Lista)
                {
                    var multimediaFile = oMultimediaFileBL.GetMultimediaFileById(ref operationResult, item.v_MultimediaFileId);

                    if (multimediaFile != null)
                    {
                        string mdoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                        using (SaveFileDialog sfd = new SaveFileDialog())
                        {

                            string Fecha = multimediaFile.FechaServicio.Value.Day.ToString().PadLeft(2, '0') + multimediaFile.FechaServicio.Value.Month.ToString().PadLeft(2, '0') + multimediaFile.FechaServicio.Value.Year.ToString();

                            //Obtener la extensión del archivo
                            string Ext = multimediaFile.FileName.Substring(multimediaFile.FileName.Length - 3, 3);

                            sfd.Title = multimediaFile.dni + "-" + Fecha + "-" + multimediaFile.FileName + "." + Ext;
                            sfd.FileName = mdoc + "\\" + sfd.Title;

                            string path = sfd.FileName;
                            if (multimediaFile.ByteArrayFile != null)
                            {
                                File.WriteAllBytes(path, multimediaFile.ByteArrayFile);
                            }


                        }
                    }

                }
            }
        }

        private void verEditarServicioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new Operations.frmEso(_serviceId, null, "Service");
            this.Enabled = false;
            //frm.MdiParent = this.MdiParent;
            frm.Show();
            this.Enabled = true;
        }

        private void grdDataService_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            ServiceBL objServiceBL = new ServiceBL();
            OperationResult objOperationResult = new OperationResult();
            int ProfesionId =int.Parse(Globals.ClientSession.i_ProfesionId.Value.ToString());
            if (ProfesionId == (int)TipoProfesional.Auditor)
            {
                  
                var alert = objServiceBL.GetServiceComponentsCulminados(ref objOperationResult, _serviceId);

                if (alert != null && alert.Count > 0)
                {
                    MessageBox.Show("El servicio tiene examenes por culminar", "INFORMACIÓN!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
               
            }

            //var frm = new Operations.frmEsoNew(_serviceId, "TRIAJE", "Service");
            //this.Enabled = false;
            ////frm.MdiParent = this.MdiParent;
            //frm.Show();
            //this.Enabled = true;

            Form frm = new Operations.frmEso(_serviceId, "TRIAJE", "Service");
            frm.ShowDialog();
        }

        private void btnEsoNew_Click(object sender, EventArgs e)
        {
            var frm = new Operations.frmEsoNew(_serviceId, null, null);
            this.Enabled = false;
            frm.Show();
            this.Enabled = true;
        }

        private void btnPrintLabelsLAB_Click(object sender, EventArgs e)
        {
            ImprimirTicket oImprimirTicket = null;
            List<ImprimirTicket> ListImprimirTicket = new List<ImprimirTicket>();
            int j = 0;

            oImprimirTicket = new ImprimirTicket();
            string x = "2";

            if (x != "0" || x != "")
            {
                for (int i = 0; i < int.Parse(x); i++)
                {
                    oImprimirTicket.v_ServicioId = grdDataService.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString().Remove(0, 7);

                    if (grdDataService.Selected.Rows[0].Cells["v_Pacient"].Value.ToString().Length > 27)
                    { oImprimirTicket.NombreTrabajador = grdDataService.Selected.Rows[0].Cells["v_Pacient"].Value.ToString().Substring(0, 27); }
                    else
                    { oImprimirTicket.NombreTrabajador = grdDataService.Selected.Rows[0].Cells["v_Pacient"].Value.ToString(); }

                    var f = DateTime.Parse(grdDataService.Selected.Rows[0].Cells["d_ServiceDate"].Value.ToString());
                    oImprimirTicket.Fecha = f.Date;

                    ListImprimirTicket.Add(oImprimirTicket);
                }
            }
            else
            {
                MessageBox.Show("No hay Tickets para imprimir", "ADVERTENCIA!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            foreach (ImprimirTicket xTicket in ListImprimirTicket)
            {
                if (j == 0)
                {
                    TSCLIB_DLL.openport("TSC TTP-244 Pro");                                             //Open specified printer driver
                    TSCLIB_DLL.setup("108", "24", "4", "8", "0", "0", "0");                           //Setup the media size and sensor type info
                    TSCLIB_DLL.clearbuffer();                                                           //Clear image buffer
                    TSCLIB_DLL.sendcommand("DIRECTION 1,0");
                    TSCLIB_DLL.sendcommand("GAP 3 mm,0 mm");

                    TSCLIB_DLL.printerfont("30", "10", "2", "0", "1", "1", xTicket.NombreTrabajador);        //Drawing printer font
                    TSCLIB_DLL.printerfont("410", "30", "2", "90", "1", "1", xTicket.Fecha.ToString());      //Drawing printer font
                    TSCLIB_DLL.barcode("140", "40", "128", "130", "2", "0", "1", "2", xTicket.v_ServicioId); //Drawing barcode

                    j = j + 1;
                }
                else
                {
                    TSCLIB_DLL.printerfont("450", "10", "2", "0", "1", "1", xTicket.NombreTrabajador);       //Drawing printer font
                    TSCLIB_DLL.printerfont("820", "30", "2", "90", "1", "1", xTicket.Fecha.ToString());      //Drawing printer font
                    TSCLIB_DLL.barcode("560", "40", "128", "130", "2", "0", "1", "2", xTicket.v_ServicioId); //Drawing barcode

                    TSCLIB_DLL.printlabel("1", "1");
                    TSCLIB_DLL.closeport();

                    j = j - 1;
                }
            }

            if (ListImprimirTicket.Count() % 2 == 0)
            {
                MessageBox.Show("Se terminó de imprimir los Tickets", "AVISO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                TSCLIB_DLL.printlabel("1", "1");
                TSCLIB_DLL.closeport();

                MessageBox.Show("Se terminó de imprimir los Tickets", "AVISO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnReporteCovid19_Click(object sender, EventArgs e)
        {
            if (ddlCustomerOrganization.SelectedValue.ToString() == "-1")
            {
                MessageBox.Show("Necesita filtrar por una empresa", "AVISO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nombreEmpresa = ddlCustomerOrganization.Text;
            var servicios = new List<string>();
            foreach (var item in grdDataService.Rows)
            {
                servicios.Add(item.Cells["v_ServiceId"].Value.ToString());
            }
            var form = new frmReporteCovid19(nombreEmpresa, servicios);
            form.ShowDialog();
        }

        private void btnFichasCovid19_Click(object sender, EventArgs e)
        {
            if (Globals.ClientSession.i_SystemUserId == 11)
            {
                var frm = new frmSoporteCovid19();
                frm.ShowDialog();
            }
        }
    }

    public class TSCLIB_DLL
    {
        [DllImport("TSCLIB.dll", EntryPoint = "about")]
        public static extern int about();

        [DllImport("TSCLIB.dll", EntryPoint = "openport")]
        public static extern int openport(string printername);

        [DllImport("TSCLIB.dll", EntryPoint = "barcode")]
        public static extern int barcode(string x, string y, string type,
                    string height, string readable, string rotation,
                    string narrow, string wide, string code);

        [DllImport("TSCLIB.dll", EntryPoint = "clearbuffer")]
        public static extern int clearbuffer();

        [DllImport("TSCLIB.dll", EntryPoint = "closeport")]
        public static extern int closeport();

        [DllImport("TSCLIB.dll", EntryPoint = "downloadpcx")]
        public static extern int downloadpcx(string filename, string image_name);

        [DllImport("TSCLIB.dll", EntryPoint = "formfeed")]
        public static extern int formfeed();

        [DllImport("TSCLIB.dll", EntryPoint = "nobackfeed")]
        public static extern int nobackfeed();

        [DllImport("TSCLIB.dll", EntryPoint = "printerfont")]
        public static extern int printerfont(string x, string y, string fonttype,
                        string rotation, string xmul, string ymul,
                        string text);

        [DllImport("TSCLIB.dll", EntryPoint = "printlabel")]
        public static extern int printlabel(string set, string copy);

        [DllImport("TSCLIB.dll", EntryPoint = "sendcommand")]
        public static extern int sendcommand(string printercommand);

        [DllImport("TSCLIB.dll", EntryPoint = "setup")]
        public static extern int setup(string width, string height,
                  string speed, string density,
                  string sensor, string vertical,
                  string offset);

        [DllImport("TSCLIB.dll", EntryPoint = "windowsfont")]
        public static extern int windowsfont(int x, int y, int fontheight,
                        int rotation, int fontstyle, int fontunderline,
                        string szFaceName, string content);
    }

}
