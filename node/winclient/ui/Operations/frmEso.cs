using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win;
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinGrid;
using System.Text.RegularExpressions;
using Infragistics.Win.UltraWinMaskedEdit;
using Sigesoft.Node.WinClient.UI.UserControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using NetPdf;
using Sigesoft.Node.WinClient.UI.Reports;

namespace Sigesoft.Node.WinClient.UI.Operations
{
    public partial class frmEso : Form
    {
        public class RunWorkerAsyncPackage
        {
            public Infragistics.Win.UltraWinTabControl.UltraTabPageControl SelectedTab { get; set; }
            public List<DiagnosticRepositoryList> ExamDiagnosticComponentList { get; set; }
            public servicecomponentDto ServiceComponent { get; set; }
            public int? i_SystemUserSuplantadorId { get; set; }
        }

        #region Declarations
        private string _Dni;
        private string _OrganizationId;
        private DateTime? _FechaServico;
        private MergeExPDF _mergeExPDF = new MergeExPDF();
        PacientBL _pacientBL = new PacientBL();
        HistoryBL _historyBL = new HistoryBL();
        private List<string> _filesNameToMerge = null;
        private List<BE.ComponentList> _tmpServiceComponentsForBuildMenuList = null;
        private ServiceBL _serviceBL = new ServiceBL();
        private string _serviceId = null;
        private string _componentId;
        private string _serviceComponentId;
        string ruta;
        private string _componentIdByDefault;
        private int? _rowIndex;
        private int? _rowIndexConclucionesDX;
        private int? _rowIndexDescansoMedico;
        public string _diagnosticId = null;
        private Keys _currentDownKey = Keys.None;
        private List<ComponentFieldsList> groupedFields = new List<ComponentFieldsList>();

        /// <summary>
        /// lista temporal (solo diagnosticos vinculados a examenes / componentes)
        /// </summary>
        private List<DiagnosticRepositoryList> _tmpExamDiagnosticComponentList = null;
        /// <summary>
        /// Almacena Temporalmente la lista de los diagnósticos totales
        /// </summary>
        private List<DiagnosticRepositoryList> _tmpTotalDiagnosticList = null;
        private List<DiagnosticRepositoryList> _tmpTotalDiagnosticByServiceIdList = null;

        /// <summary>
        /// almacena temporalmente lista de diagnosticos [Definitivos / Presuntivos]
        /// en la pestaña Conclusiones y tratamiento
        private List<DiagnosticRepositoryList> _tmpTotalConclusionesDxByServiceIdList = null;

        /// <summary>
        /// almacena temporalmente lista de diagnosticos [Definitivos / Presuntivos] para generar descanso Médico 
        /// en la pestaña Conclusiones y tratamiento
        /// </summary>
        private List<DiagnosticRepositoryList> _tmpTotalConclusionesDxForMedicalBreakList = null;

        private DiagnosticRepositoryList _tmpTotalDiagnostic = null;

        /// <summary>
        /// Lista temporal de restricciones usadas en la pestaña de Analisis de Diagnósticos
        /// </summary>
        private List<RestrictionList> _tmpRestrictionList = null;
        /// <summary>
        /// Lista temporal de restricciones usadas en la pestaña de Conclusiones
        /// </summary>
        private List<RestrictionList> _tmpRestrictionListConclusiones = null;
        private List<RecomendationList> _tmpRecomendationList = null;

        /// <summary>
        /// Lista temporal de recomendaciones usada en la pestaña de Conclusiones
        /// </summary>
        private List<RecomendationList> _tmpRecomendationConclusionesList = null;

        private List<MedicationList> _tmpMedicationList = null;
        private List<ProcedureByServiceList> _tmpProcedureList = null;

        private List<ServiceComponentFieldsList> _serviceComponentFieldsList = null;
        private List<ServiceComponentFieldValuesList> _serviceComponentFieldValuesList = null;

        private ServiceComponentList _serviceComponentsInfo = null;

        private bool flagValueChange = false;

        private string _componentIdConclusionesDX = null;

        private Dictionary<string, UltraValidator> _dicUltraValidators = null;
        private bool _isChangeValue = false;
        private string _oldValue;
        private int _age;
        private Gender _sexType;
        private string _personId;
        private string _serviceIdByWiewServiceHistory;
        private string _action;
        private string _pharmacyWarehouseId;
        private int _masterServiceId;
        private List<KeyValueDTO> _formActions = null;
        private string _examName;
        bool isDisabledButtonsExamDx = false;
        byte[] _personImage;
        string _personName;
        TypeESO _esoTypeId;

        private List<ServiceComponentFieldValuesList> _tmpListValuesOdontograma = null;
        frmWaiting frmWaiting = new frmWaiting("Grabando...");

        private bool _cancelEventSelectedIndexChange;

        private bool _removerTotalDiagnostico;
        private bool _removerRecomendacion_AnalisisDx;
        private bool _removerRestriccion_Analisis;

        private bool _removerRecomendaciones_Conclusiones;
        private bool _removerRestricciones_ConclusionesTratamiento;
        private bool _chkApprovedEnabled;

        #endregion

        public frmEso(string serviceId, string componentIdByDefault, string action)
        {
            InitializeComponent();
            _serviceId = serviceId;
            _componentIdByDefault = componentIdByDefault;
            _action = action;
        }

        private void frmEso_Load(object sender, EventArgs e)
        {
            if (Globals.ClientSession.i_SystemUserId == 11 || Globals.ClientSession.i_SystemUserId == 111 ||Globals.ClientSession.i_SystemUserId == 124)
            {
                chkUtilizarFirma.Visible = true;
                chkUtilizaFirmaControlAuditoria.Visible = true;
                chkUtilizaFirmaAptitud.Visible = true;
            }
            else
            {
                chkUtilizarFirma.Visible = false;
                chkUtilizaFirmaControlAuditoria.Visible = false;
                chkUtilizaFirmaAptitud.Visible = false;
            }

            if (_action == "View")
            {

                gbAntecedentes.Enabled = false;
                gbServiciosAnteriores.Enabled = false;

                #region Anamnesis

                gbSintomasySignos.Enabled = false;
                gbFuncionesBiologicas.Enabled = false;
                btnGuardarAnamnesis.Enabled = false;

                #endregion

                #region Examenes

                gbDiagnosticoExamen.Enabled = false;
                txtComentario.Enabled = false;
                cbEstadoComponente.Enabled = false;
                cbTipoProcedenciaExamen.Enabled = false;
                btnGuardarExamen.Enabled = false;

                #endregion

                #region Analisis de diagnosticos

                gbTotalDiagnostico.Enabled = false;
                gbEdicionDiagnosticoTotal.Enabled = false;
                btnAceptarDX.Enabled = false;

                #endregion

                #region Conclusiones

                cbAptitudEso.Enabled = false;
                gbConclusionesDiagnosticas.Enabled = false;
                gbRecomendaciones_Conclusiones.Enabled = false;
                gbRestricciones_Conclusiones.Enabled = false;
                btnGuardarConclusiones.Enabled = false;

                #endregion

            }

           

            using (new LoadingClass.PleaseWait(this.Location, "Cargando..."))
            {
                //ZerveR XXXXXXX
                LoadComboBox();
                InitializeData();
                BuildMenu();

                //((frmService)this.Owner).Enabled = true;
                #region FormActions

                _formActions = Sigesoft.Node.WinClient.BLL.Utils.SetFormActionsInSession("frmEso",
                                                                                   Globals.ClientSession.i_CurrentExecutionNodeId,
                                                                                   Globals.ClientSession.i_RoleId.Value,
                                                                                   Globals.ClientSession.i_SystemUserId);
                // Setear privilegios / permisos

                //var enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_ANADX_EDIT", _formActions);

                #region Examenes

                btnGuardarExamen.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_EXAMENES_SAVE", _formActions);

                #endregion

                #region Analisis de diagnosticos

                btnAgregarTotalDiagnostico.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_ANADX_ADDDX", _formActions);

                _removerTotalDiagnostico = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_ANADX_REMOVEDX", _formActions);
                //btnRemoverTotalDiagnostico.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_ANADX_REMOVEDX", _formActions);

                btnAgregarRecomendaciones_AnalisisDx.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_ANADX_ADDRECOME", _formActions);
                //btnRemoverRecomendacion_AnalisisDx.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_ANADX_REMOVERECOME", _formActions);

                _removerRecomendacion_AnalisisDx = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_ANADX_REMOVERECOME", _formActions);

                btnAgregarRestriccion_Analisis.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_ANADX_ADDRESTRIC", _formActions);
                //btnRemoverRestriccion_Analisis.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_ANADX_REMOVERESTRIC", _formActions);

                _removerRestriccion_Analisis = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_ANADX_REMOVERESTRIC", _formActions);

                btnAceptarDX.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_ANADX_SAVE", _formActions);

                if (!btnAceptarDX.Enabled)
                {
                    cbCalificacionFinal.Enabled = false;
                    cbTipoDx.Enabled = false;
                    cbEnviarAntecedentes.Enabled = false;
                    dtpFechaVcto.Enabled = false;
                }

                #endregion

                #region Conclusiones

                cbAptitudEso.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_CONCLUSIONES_DEFAPTITUD", _formActions);
                btnGuardarConclusiones.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_CONCLUSIONES_SAVE", _formActions);

                btnAgregarRecomendaciones_Conclusiones.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_CONCLUSIONES_ADDRECOME", _formActions);
                btnAgregarRestriccion_ConclusionesTratamiento.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_CONCLUSIONES_ADDRESTRIC", _formActions);

                //btnRemoverRecomendaciones_Conclusiones.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_CONCLUSIONES_REMOVERECOME", _formActions);
                //btnRemoverRestricciones_ConclusionesTratamiento.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_CONCLUSIONES_REMOVERESTRIC", _formActions);

                _removerRecomendaciones_Conclusiones = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_CONCLUSIONES_REMOVERECOME", _formActions);
                _removerRestricciones_ConclusionesTratamiento = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_CONCLUSIONES_REMOVERESTRIC", _formActions);

                #endregion

                #endregion

                // PESTAÑA Antecedentes X DEFECTO
                tcSubMain.SelectedIndex = 0;
                // Setear por default un examen componente desde consusltorio

                #region Set Tab x default

                if (!string.IsNullOrEmpty(_componentIdByDefault))
                {
                    var comp = _componentIdByDefault.Split('|');

                    foreach (var tab in tcExamList.Tabs)
                    {
                        var arrfind = Array.FindAll(comp, p => tab.Key.Contains(p));

                        if (arrfind.Length > 0)
                        {
                            tab.Selected = true;
                            break;
                        }
                    }

                    //var findComponent = _tmpServiceComponentsForBuildMenuList.Find(p => p.v_ComponentId == _componentIdByDefault);

                    //if (findComponent != null)
                    //{                      
                    //    tcExamList.Tabs[_componentIdByDefault].Selected = true;
                    //}
                    //else
                    //{
                    //    tcExamList.Tabs[0].Selected = true;
                    //}
                }
                else
                {
                    tcExamList.Tabs[0].Selected = true;
                }

                #endregion

                // Setear pestaña de Aptitud x default
                if (_action == "Service")
                    tcSubMain.SelectedIndex = 3;

                // Información para grillas
                GetTotalDiagnosticsForGridView();
                ConclusionesyTratamiento_LoadAllGrid();
                gbEdicionDiagnosticoTotal.Enabled = false;

            }

          

        }

        private void BuildMenu()
        {

            try
            {
                //this.tcExamList.Tabs.Clear();
                // construir menu dinamico    

                // Lista de Componentes / Campos / Values
                OperationResult objOperationResult = new OperationResult();
                _tmpServiceComponentsForBuildMenuList = new ServiceBL().GetServiceComponentsForBuildMenu(ref objOperationResult, _serviceId);

                #region Declarations Controls

                Label lbl = null;
                Infragistics.Win.UltraWinTabControl.UltraTab tab = null;
                UltraNumericEditor une = null;
                TextBox txt = null;
                ComboBox cb = null;
                GroupBox gb = null;
                Control ctl = null;
                UltraValidator uv = null;
                GroupBox gbGroupedComponent = null;

                #endregion

                int i = 1;
                int fieldsByGroupBoxCount = 1;

                foreach (BE.ComponentList com in _tmpServiceComponentsForBuildMenuList)
                {

                    #region crear y configurar Tab Component y FlowLayoutPanel (padre) por cada tab que se agregue dinamicamente

                    // Crear TAB del componente
                    tab = new Infragistics.Win.UltraWinTabControl.UltraTab();
                    tab.Text = com.v_Name;
                    tab.Key = com.v_ComponentId;
                    tab.Tag = com.v_ServiceComponentId;
                    tab.ToolTipText = com.v_Name + " / " + com.v_ServiceComponentId + " / " + com.v_ComponentId;
                    tcExamList.Tabs.Add(tab);

                    // Incrustar el flpParent por cada tab
                    // Crear Flowlayout del Componente
                    TableLayoutPanel tblpParent;
                    tblpParent = new TableLayoutPanel();
                    tblpParent.Name = "tblpParent";
                    tblpParent.ColumnCount = 1;
                    tblpParent.RowCount = groupedFields.Count;
                    tab.TabPage.Controls.Add(tblpParent);

                    // Crear validadores para cada componente examen [triaje,Audio,etc]
                    uv = CreateUltraValidatorByComponentId(com.v_ComponentId);

                    #endregion

                    if (com.GroupedComponentsName != null)
                    {
                        foreach (var gcn in com.GroupedComponentsName)
                        {

                            #region Create GroupBox (Agrupador de Componente)

                            // Create GroupBox (Agrupador de Componente)

                            gbGroupedComponent = new GroupBox();
                            gbGroupedComponent.Text = gcn.v_GroupedComponentName;
                            gbGroupedComponent.Name = "gb_" + gcn.v_GroupedComponentName;
                            gbGroupedComponent.BackColor = Color.LightCyan;
                            gbGroupedComponent.AutoSize = true;
                            gbGroupedComponent.Dock = DockStyle.Top;

                            i++;

                            // Crear table layout para los GroupBoxes que van a contener los campos

                            TableLayoutPanel tblpGroupedComponent = new TableLayoutPanel();
                            tblpGroupedComponent.Name = "tblpGroup_" + gcn.v_GroupedComponentName;
                            tblpGroupedComponent.ColumnCount = 1;
                            tblpGroupedComponent.RowCount = 1;
                            tblpGroupedComponent.Dock = DockStyle.Fill;
                            tblpGroupedComponent.AutoSize = true;

                            // Obtener los campos de un componente especifico

                            var fieldsByComponent = _tmpServiceComponentsForBuildMenuList
                                        .SelectMany(p => p.Fields)
                                        .ToList()
                                        .FindAll(p => p.v_ComponentId == gcn.v_ComponentId);

                            // Obterner los Grupos de entre los campos obtenidos

                            var groupBoxes = fieldsByComponent.GroupBy(e => new { e.v_Group }).Select(g => g.First()).OrderBy(o => o.v_Group).ToList();

                            #endregion

                            // Recorrer los Grupos obtenidos

                            foreach (var g in groupBoxes)
                            {
                                #region Crear control GroupBox para agrupar los campos (controles)

                                // Crear y configurar GroupBox por cada grupo
                                gb = new GroupBox();
                                gb.Text = g.v_Group;
                                gb.Name = "gb_" + g.v_Group;
                                gb.BackColor = Color.Azure;
                                gb.AutoSize = true;
                                gb.Dock = DockStyle.Top;

                                fieldsByGroupBoxCount++;

                                // Definir el table layout para los controles del grupo
                                TableLayoutPanel tblpGroup;
                                tblpGroup = new TableLayoutPanel();
                                tblpGroup.Name = "tblpGroup_" + g.v_Group;
                                tblpGroup.ColumnCount = g.i_Column * Constants.COLUMNAS_POR_CONTROL;
                                tblpGroup.RowCount = RedondeoMayor(com.Fields.Count, g.i_Column);
                                tblpGroup.Dock = DockStyle.Fill;
                                tblpGroup.AutoSize = true;
                                //tblpGroup.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial;                        

                                #endregion

                                // Recorrer todos los controles del grupo
                                int nroControlNET = 1;
                                int fila, columna;

                                // Obtener los campos de c/u de los grupos
                                var fieldsByGroupBox = com.Fields.FindAll(p => p.v_Group == g.v_Group && p.v_ComponentId == gcn.v_ComponentId);

                                foreach (ComponentFieldsList f in fieldsByGroupBox)
                                {
                                    #region Buscar campos pertenecientes a un grupo, crearlos y configurarlos para Agregarlos dentro de de cada Tab / GroupBox


                                    // PONER EL LABEL
                                    lbl = new Label();
                                    lbl.Text = f.v_TextLabel;
                                    lbl.Width = f.i_LabelWidth;
                                    lbl.Height = f.i_HeightLabel == null ? 13 : f.i_HeightLabel.Value;
                                    lbl.TextAlign = ContentAlignment.BottomRight;
                                    lbl.AutoSize = false;
                                    lbl.Font = new Font(lbl.Font.FontFamily.Name, 7.25F);
                                    //lbl.TextAlign = ContentAlignment.TopLeft;
                                    fila = RedondeoMayor(nroControlNET, g.i_Column * Constants.COLUMNAS_POR_CONTROL);
                                    columna = nroControlNET - (fila - 1) * (g.i_Column * Constants.COLUMNAS_POR_CONTROL);
                                    tblpGroup.Controls.Add(lbl, columna - 1, fila - 1);
                                    nroControlNET++;

                                    switch ((ControlType)f.i_ControlId)
                                    {
                                        #region Creacion del control

                                        case ControlType.CadenaTextual:
                                            txt = new TextBox();
                                            txt.Width = f.i_ControlWidth;
                                            txt.Height = f.i_HeightControl;
                                            txt.MaxLength = f.i_MaxLenght;
                                            txt.Name = f.v_ComponentFieldId;

                                            if (f.i_IsCalculate == (int)SiNo.SI)
                                            {
                                                txt.Enabled = false;
                                            }
                                            else
                                            {
                                                txt.Leave += new EventHandler(txt_Leave);
                                            }

                                            if (f.i_IsRequired == (int)SiNo.SI)
                                                SetControlValidate(f.i_ControlId, txt, null, null, uv);

                                            txt.Enter += new EventHandler(Capture_Value);

                                             if (f.i_Enabled == (int)SiNo.NO)
                                            {
                                                txt.Enabled = true;
                                            }
                                            else
                                            {
                                                txt.Enabled = false;
                                            }


                                            if (f.i_ReadOnly == (int)SiNo.SI)
                                            {
                                                txt.ReadOnly = true;
                                            }
                                            else
                                            {
                                                txt.ReadOnly = false;
                                            }

                                            if (_action == "View")
                                            {
                                                txt.ReadOnly = true;
                                            }

                                            ctl = txt;
                                            break;
                                        case ControlType.CadenaMultilinea:
                                            txt = new TextBox()
                                            {
                                                Width = f.i_ControlWidth,
                                                Height = f.i_HeightControl,
                                                Multiline = true,
                                                MaxLength = f.i_MaxLenght,
                                                ScrollBars = ScrollBars.Vertical,
                                                Name = f.v_ComponentFieldId,
                                            };
                                             if (f.i_Enabled == (int)SiNo.NO)
                                            {
                                                txt.Enabled = true;
                                            }
                                            else
                                            {
                                                txt.Enabled = false;
                                            }


                                            if (f.i_ReadOnly == (int)SiNo.SI)
                                            {
                                                txt.ReadOnly = true;
                                            }
                                            else
                                            {
                                                txt.ReadOnly = false;
                                            }
                                            txt.Enter += new EventHandler(Capture_Value);
                                            txt.Leave += new EventHandler(txt_Leave);

                                            if (f.i_IsRequired == (int)SiNo.SI)
                                                SetControlValidate(f.i_ControlId, txt, null, null, uv);

                                            if (_action == "View")
                                            {
                                                txt.ReadOnly = true;
                                            }

                                            ctl = txt;
                                            break;
                                        case ControlType.NumeroEntero:
                                            une = new UltraNumericEditor()
                                            {
                                                Width = f.i_ControlWidth,
                                                Height = f.i_HeightControl,
                                                NumericType = NumericType.Integer,
                                                PromptChar = ' ',
                                                Name = f.v_ComponentFieldId,
                                                MaskDisplayMode = MaskMode.Raw

                                            };

                                            // Asociar el control a un evento
                                            une.Enter += new EventHandler(Capture_Value);

                                            if (f.i_IsCalculate == (int)SiNo.SI)
                                            {
                                                une.ReadOnly = true;
                                                une.ValueChanged += new EventHandler(txt_ValueChanged);
                                            }
                                            else
                                            {
                                                une.Leave += new EventHandler(txt_Leave);
                                            }

                                            if (f.i_IsRequired == (int)SiNo.SI)
                                            {
                                                // Establecer condición por rangos
                                                SetControlValidate(f.i_ControlId, une, f.r_ValidateValue1, f.r_ValidateValue2, uv);
                                            }

                                            if (_action == "View")
                                            {
                                                une.ReadOnly = true;
                                            }

                                            ctl = une;
                                            break;
                                        case ControlType.NumeroDecimal:
                                            une = new UltraNumericEditor()
                                            {
                                                Width = f.i_ControlWidth,
                                                Height = f.i_HeightControl,
                                                PromptChar = ' ',
                                                Name = f.v_ComponentFieldId,
                                                NumericType = NumericType.Double,
                                                MaskDisplayMode = MaskMode.Raw

                                            };

                                            // Asociar el control a un evento
                                            une.Enter += new EventHandler(Capture_Value);

                                            if (f.i_IsCalculate == (int)SiNo.SI)
                                            {
                                                une.ValueChanged += new EventHandler(txt_ValueChanged);
                                                une.ReadOnly = true;
                                            }
                                            else
                                            {
                                                une.Leave += new EventHandler(txt_Leave);
                                            }

                                            if (f.i_IsRequired == (int)SiNo.SI)
                                            {
                                                // Establecer condición por rangos                                                              
                                                SetControlValidate(f.i_ControlId, une, f.r_ValidateValue1, f.r_ValidateValue2, uv);
                                            }

                                            if (_action == "View")
                                            {
                                                une.ReadOnly = true;
                                            }

                                            ctl = une;
                                            break;
                                        case ControlType.SiNoCheck:
                                            ctl = new CheckBox()
                                            {
                                                Width = f.i_ControlWidth,
                                                Height = f.i_HeightControl,
                                                Text = "Si/No",
                                                Name = f.v_ComponentFieldId,
                                            };

                                            ctl.Enter += new EventHandler(Capture_Value);
                                            ctl.Leave += new EventHandler(txt_Leave);

                                            if (_action == "View")
                                            {
                                                ctl.Enabled = false;
                                            }

                                            break;
                                        case ControlType.SiNoRadioButton:
                                            ctl = new RadioButton()
                                            {
                                                Width = f.i_ControlWidth,
                                                Height = f.i_HeightControl,
                                                Text = "Si/No",
                                                Name = f.v_ComponentFieldId
                                            };

                                            ctl.Enter += new EventHandler(Capture_Value);
                                            ctl.Leave += new EventHandler(txt_Leave);

                                            if (_action == "View")
                                            {
                                                ctl.Enabled = false;
                                            }

                                            break;
                                        case ControlType.SiNoCombo:
                                            ctl = new ComboBox()
                                            {
                                                Width = f.i_ControlWidth,
                                                Height = f.i_HeightControl,
                                                DropDownStyle = ComboBoxStyle.DropDownList,
                                                Name = f.v_ComponentFieldId
                                            };

                                            Utils.LoadDropDownList((ComboBox)ctl, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, f.i_GroupId, null), DropDownListAction.Select);

                                            if (f.i_IsRequired == (int)SiNo.SI)
                                            {
                                                SetControlValidate(f.i_ControlId, ctl, null, null, uv);
                                            }

                                            ctl.Enter += new EventHandler(Capture_Value);
                                            ctl.Leave += new EventHandler(txt_Leave);

                                            if (_action == "View")
                                            {
                                                ctl.Enabled = false;
                                            }

                                            break;
                                        case ControlType.UcFileUpload:
                                            var ucFileUpload = new Sigesoft.Node.WinClient.UI.UserControls.ucFileUpload();
                                            ucFileUpload.PersonId = _personId;
                                            ucFileUpload.ServiceComponentId = com.v_ServiceComponentId;
                                            ucFileUpload.Name = f.v_ComponentFieldId;
                                            ucFileUpload.Dni = _Dni;
                                            //Regional DAVID
                                            var fecha = DateTime.Parse(lblFecInicio.Text.ToString());
                                            ServiceBL _xserviceBL = new ServiceBL();
                                            var o = _xserviceBL.GetServiceShort(_serviceId);
                                            string Fechax = o.FechaServicio.Value.Day.ToString().PadLeft(2, '0') + o.FechaServicio.Value.Month.ToString().PadLeft(2, '0') + o.FechaServicio.Value.Year.ToString();
                                            ucFileUpload.Fecha = Fechax.ToString();
                                            ucFileUpload.Consultorio = com.v_CategoryName;

                                            //ctl = new Sigesoft.Node.WinClient.UI.UserControls.ucFileUpload();
                                            ctl = ucFileUpload;
                                            break;
                                        case ControlType.UcOdontograma:
                                            var ucOdontograma = new Sigesoft.Node.WinClient.UI.UserControls.ucOdontograma();
                                            ucOdontograma.Name = f.v_ComponentFieldId;
                                            ctl = ucOdontograma;
                                            break;
                                        case ControlType.UcAudiometria:
                                            var ucAudiometria = new Sigesoft.Node.WinClient.UI.UserControls.ucAudiometria();
                                            ucAudiometria.Name = f.v_ComponentFieldId;
                                            ucAudiometria.PersonId = _personId;
                                            ucAudiometria.ServiceComponentId = com.v_ServiceComponentId;
                                            // Establecer evento
                                            ucAudiometria.AfterValueChange += new EventHandler<AudiometriaAfterValueChangeEventArgs>(ucAudiometria_AfterValueChange);
                                            ctl = ucAudiometria;
                                            break;
                                        case ControlType.ucEspirometria:
                                            var ucEspirometria = new Sigesoft.Node.WinClient.UI.UserControls.ucEspirometria();
                                            ucEspirometria.Name = f.v_ComponentFieldId;
                                            // Establecer evento
                                            ucEspirometria.AfterValueChange += new EventHandler<AudiometriaAfterValueChangeEventArgs>(ucAudiometria_AfterValueChange);
                                            ctl = ucEspirometria;
                                            break;
                                        case ControlType.ucOftalmologia:
                                            var ucOftalmologia = new Sigesoft.Node.WinClient.UI.UserControls.ucOftalmologia();
                                            ucOftalmologia.Name = f.v_ComponentFieldId;
                                            ucOftalmologia.Age = _age;
                                            // Establecer evento
                                            ucOftalmologia.AfterValueChange += new EventHandler<AudiometriaAfterValueChangeEventArgs>(ucAudiometria_AfterValueChange);
                                            ctl = ucOftalmologia;
                                            break;
                                        case ControlType.ucRadiografiaOIT:
                                            var ucRadiografiaOIT = new Sigesoft.Node.WinClient.UI.UserControls.ucRadiografiaOIT();
                                            ucRadiografiaOIT.Name = f.v_ComponentFieldId;
                                            // Establecer evento
                                            ucRadiografiaOIT.AfterValueChange += new EventHandler<AudiometriaAfterValueChangeEventArgs>(ucAudiometria_AfterValueChange);
                                            ctl = ucRadiografiaOIT;
                                            break;
                                        case ControlType.ucPsicologia:
                                            if (_esoTypeId == TypeESO.PreOcupacional)
                                            {
                                                var ucPsicologiaEMPO = new Sigesoft.Node.WinClient.UI.UserControls.ucPsychologicalExam(_esoTypeId);
                                                ucPsicologiaEMPO.Name = f.v_ComponentFieldId;
                                               
                                                // Establecer evento
                                                ucPsicologiaEMPO.AfterValueChange += new EventHandler<AudiometriaAfterValueChangeEventArgs>(ucAudiometria_AfterValueChange);
                                                ctl = ucPsicologiaEMPO;
                                            }
                                            else if (_esoTypeId == TypeESO.PeriodicoAnual)
                                            {
                                                var ucPsicologiaEMOA = new Sigesoft.Node.WinClient.UI.UserControls.ucPsychologicalExamAnual(_esoTypeId);
                                                ucPsicologiaEMOA.Name = f.v_ComponentFieldId;
                                               
                                                // Establecer evento
                                                ucPsicologiaEMOA.AfterValueChange += new EventHandler<AudiometriaAfterValueChangeEventArgs>(ucAudiometria_AfterValueChange);
                                                ctl = ucPsicologiaEMOA;
                                            }
                                            break;

                                        case ControlType.UcSomnolencia:
                                            var ucSomnolencia = new Sigesoft.Node.WinClient.UI.UserControls.ucSomnolencia();
                                            ucSomnolencia.Name = f.v_ComponentFieldId;
                                            // Establecer evento
                                            ucSomnolencia.AfterValueChange += new EventHandler<AudiometriaAfterValueChangeEventArgs>(ucSomnolencia_AfterValueChange);
                                            ctl = ucSomnolencia;
                                            break;

                                        case ControlType.UcBoton:
                                            var ucBoton = new Sigesoft.Node.WinClient.UI.UserControls.ucBoton();
                                            ucBoton.Name = f.v_ComponentFieldId;
                                            ucBoton.Dni = _Dni;
                                            ucBoton.Examen = com.v_Name;
                                            ucBoton.FechaServicio = _FechaServico.Value;
                                            // Establecer evento
                                            ctl = ucBoton;
                                            break;

                                        case ControlType.UcAcumetria:
                                            var ucAcumetria = new Sigesoft.Node.WinClient.UI.UserControls.ucAcumetria();
                                            ucAcumetria.Name = f.v_ComponentFieldId;
                                            // Establecer evento
                                            ctl = ucAcumetria;
                                            break;
                                        case ControlType.UcSintomaticoRespi:
                                            var ucSintomaticoRespi = new Sigesoft.Node.WinClient.UI.UserControls.ucSintomaticoResp();
                                            ucSintomaticoRespi.Name = f.v_ComponentFieldId;
                                            // Establecer evento
                                            ctl = ucSintomaticoRespi;
                                            break;
                                        case ControlType.UcRxLumboSacra:
                                            var ucRxLumboSacra = new Sigesoft.Node.WinClient.UI.UserControls.ucRXLumboSacra();
                                            ucRxLumboSacra.Name = f.v_ComponentFieldId;
                                            // Establecer evento
                                            ctl = ucRxLumboSacra;
                                            break;
                                        case ControlType.UcOjoSeco:
                                            var ucOjoSeco = new Sigesoft.Node.WinClient.UI.UserControls.ucOjoSeco();
                                            ucOjoSeco.Name = f.v_ComponentFieldId;
                                            // Establecer evento
                                            ctl = ucOjoSeco;
                                            break;

                                        case ControlType.UcOtoscopia:
                                            var ucOtoscopia = new Sigesoft.Node.WinClient.UI.UserControls.ucOtoscopia();
                                            ucOtoscopia.Name = f.v_ComponentFieldId;
                                            // Establecer evento
                                            ctl = ucOtoscopia;
                                            break;

                                        case ControlType.UcEvaluacionErgonomica:
                                            var ucEvaluacionErgonomica = new Sigesoft.Node.WinClient.UI.UserControls.ucEvaluacionErgonomica();
                                            ucEvaluacionErgonomica.Name = f.v_ComponentFieldId;
                                            // Establecer evento
                                            ctl = ucEvaluacionErgonomica;
                                            break;

                                        case ControlType.UcOsteoMuscular:
                                            var ucOsteo = new Sigesoft.Node.WinClient.UI.UserControls.ucOsteoMuscular();
                                            ucOsteo.Name = f.v_ComponentFieldId;
                                            // Establecer evento
                                            ctl = ucOsteo;
                                            break;

                                        case ControlType.Lista:
                                            cb = new ComboBox()
                                            {
                                                Width = f.i_ControlWidth,
                                                Height = f.i_HeightControl,
                                                DropDownStyle = ComboBoxStyle.DropDownList,
                                                Name = f.v_ComponentFieldId
                                            };

                                            //Utils.LoadDropDownList((ComboBox)ctl, "Value1", "Id", BLL.Utils.GetDataHierarchyForComboAndItemId(ref objOperationResult, f.i_GroupId, f.i_ItemId, null), DropDownListAction.Select);
                                            var data = BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, f.i_GroupId, null);

                                            Utils.LoadDropDownList(cb, "Value1", "Id", data, DropDownListAction.Select);

                                            if (f.i_IsRequired == (int)SiNo.SI)
                                            {
                                                SetControlValidate(f.i_ControlId, cb, null, null, uv);
                                            }

                                            // Setear levantamiento de popup para el ingreso de los hallazgos solo cuando 
                                            // se seleccione un valor alterado

                                            if ((f.v_ComponentId == Constants.EXAMEN_FISICO_ID
                                                || f.v_ComponentId == Constants.RX_ID
                                                || f.v_ComponentId == Constants.OFTALMOLOGIA_ID
                                                || f.v_ComponentId == Constants.ALTURA_ESTRUCTURAL_ID
                                                || f.v_ComponentId == Constants.TACTO_RECTAL_ID
                                                || f.v_ComponentId == Constants.EVAL_NEUROLOGICA_ID
                                                || f.v_ComponentId == Constants.TEST_ROMBERG_ID
                                                || f.v_ComponentId == Constants.TAMIZAJE_DERMATOLOGIO_ID
                                                || f.v_ComponentId == Constants.GINECOLOGIA_ID
                                                || f.v_ComponentId == Constants.EXAMEN_MAMA_ID
                                                || f.v_ComponentId == Constants.AUDIOMETRIA_ID
                                                || f.v_ComponentId == Constants.ELECTROCARDIOGRAMA_ID
                                                || f.v_ComponentId == Constants.ESPIROMETRIA_ID
                                                || f.v_ComponentId == Constants.OSTEO_MUSCULAR_ID
                                                || f.v_ComponentId == Constants.PRUEBA_ESFUERZO_ID
                                                || f.v_ComponentId == Constants.TAMIZAJE_DERMATOLOGIO_ID
                                                || f.v_ComponentId == Constants.ODONTOGRAMA_ID
                                                || f.v_ComponentId == Constants.EXAMEN_FISICO_7C_ID)
                                                && (f.i_GroupId == (int)SystemParameterGroups.ConHallazgoSinHallazgosNoSeRealizo))
                                            {
                                                cb.SelectedIndexChanged += new EventHandler(cb_SelectedIndexChanged);
                                            }

                                            // invocar popup de antecedentes medicos personales
                                            if ((f.v_ComponentId == Constants.RX_ID                                            
                                              || f.v_ComponentId == Constants.AUDIOMETRIA_ID )                                           
                                              && (f.i_GroupId == (int)SystemParameterGroups.SiNoNoDefine))
                                            {
                                                cb.SelectedIndexChanged += new EventHandler(cb_SelectedIndexChanged_Antecedentes);
                                            }

                                            cb.Enter += new EventHandler(Capture_Value);
                                            cb.Leave += new EventHandler(txt_Leave);

                                            if (_action == "View")
                                            {
                                                cb.Enabled = false;
                                            }

                                            ctl = cb;
                                            break;
                                        default:
                                            break;

                                        #endregion

                                    }

                                    //ctl.CreateControl();

                                    // PONER EL CONTROL ESPECIFICO
                                    ctl.Tag = new KeyTagControl
                                    {
                                        i_ControlId = f.i_ControlId,
                                        v_ComponentId = f.v_ComponentId,
                                        v_ComponentFieldsId = f.v_ComponentFieldId,
                                        i_IsSourceFieldToCalculate = f.i_IsSourceFieldToCalculate,
                                        v_Formula = f.v_Formula,
                                        v_TargetFieldOfCalculateId = f.v_TargetFieldOfCalculateId,
                                        v_SourceFieldToCalculateJoin = f.v_SourceFieldToCalculateJoin,
                                        v_FormulaChild = f.v_FormulaChild,
                                        Formula = f.Formula,
                                        TargetFieldOfCalculateId = f.TargetFieldOfCalculateId,
                                        v_TextLabel = f.v_TextLabel,
                                        v_ComponentName = com.v_Name
                                    };


                                    // Agregar el control al contenedor
                                    fila = RedondeoMayor(nroControlNET, g.i_Column * Constants.COLUMNAS_POR_CONTROL);
                                    columna = nroControlNET - (fila - 1) * (g.i_Column * Constants.COLUMNAS_POR_CONTROL);
                                    tblpGroup.Controls.Add(ctl, columna - 1, fila - 1);
                                    nroControlNET++;

                                    // label de unid medida.
                                    Label lbl1 = new Label();
                                    lbl1.AutoSize = false;
                                    lbl1.Width = 50;
                                    lbl1.Text = f.v_MeasurementUnitName;
                                    lbl1.Font = new Font(lbl1.Font, FontStyle.Bold | FontStyle.Italic);
                                    lbl1.TextAlign = ContentAlignment.BottomLeft;
                                    fila = RedondeoMayor(nroControlNET, g.i_Column * Constants.COLUMNAS_POR_CONTROL);
                                    columna = nroControlNET - (fila - 1) * (g.i_Column * Constants.COLUMNAS_POR_CONTROL);
                                    tblpGroup.Controls.Add(lbl1, columna - 1, fila - 1);
                                    nroControlNET++;



                                    #endregion

                                }

                                gb.Controls.Add(tblpGroup);
                                tblpGroupedComponent.Controls.Add(gb, 1, fieldsByGroupBoxCount);
                            }

                            gbGroupedComponent.Controls.Add(tblpGroupedComponent);
                            tblpParent.Controls.Add(gbGroupedComponent, 1, i);

                        }

                        tblpParent.AutoScroll = true;
                        tblpParent.Dock = DockStyle.Fill;
                        tblpParent.BackColor = Color.Gray;

                    } 
                    else       // Examenes sin categoria
                    {
                        #region Formar grupos para los campos (controles)

                        List<ComponentFieldsList> groups = com.Fields.GroupBy(e => new { e.v_Group }).Select(g => g.First()).OrderBy(o => o.v_Group).ToList();

                        #endregion

                        foreach (var g in groups)
                        {
                            #region Crear control GroupBox para agrupar los campos (controles)

                            // Crear y configurar GroupBox por cada grupo
                            gb = new GroupBox();
                            gb.Text = g.v_Group;
                            gb.Name = "gb_" + g.v_Group;
                            gb.BackColor = Color.Azure;
                            gb.AutoSize = true;
                            gb.Dock = DockStyle.Top;

                            i++;

                            // Definir el table layout para los controles del grupo
                            TableLayoutPanel tblpGroup;
                            tblpGroup = new TableLayoutPanel();
                            tblpGroup.Name = "tblpGroup_" + g.v_Group;
                            tblpGroup.ColumnCount = g.i_Column * Constants.COLUMNAS_POR_CONTROL;
                            tblpGroup.RowCount = RedondeoMayor(com.Fields.Count, g.i_Column);
                            tblpGroup.Dock = DockStyle.Fill;
                            tblpGroup.AutoSize = true;
                            //tblpGroup.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial;                        

                            #endregion

                            // Recorrer todos los controles del grupo
                            int nroControlNET = 1;
                            int fila, columna;

                            #region Buscar campos pertenecientes a un grupo, crearlos y configurarlos para Agregarlos dentro de de cada Tab / GroupBox

                            // Buscar campos para agruparlos 
                            groupedFields = com.Fields.FindAll(p => p.v_Group == g.v_Group);

                            foreach (ComponentFieldsList f in groupedFields)
                            {
                                // PONER EL LABEL
                                lbl = new Label();
                                lbl.Text = f.v_TextLabel;
                                lbl.Width = f.i_LabelWidth;
                                lbl.Height = f.i_HeightLabel == null ? 13 : f.i_HeightLabel.Value;
                                lbl.TextAlign = ContentAlignment.BottomRight;
                                lbl.AutoSize = false;
                                lbl.Font = new Font(lbl.Font.FontFamily.Name, 7.25F);
                                //lbl.TextAlign = ContentAlignment.TopLeft;
                                fila = RedondeoMayor(nroControlNET, g.i_Column * Constants.COLUMNAS_POR_CONTROL);
                                columna = nroControlNET - (fila - 1) * (g.i_Column * Constants.COLUMNAS_POR_CONTROL);
                                tblpGroup.Controls.Add(lbl, columna - 1, fila - 1);
                                nroControlNET++;

                                switch ((ControlType)f.i_ControlId)
                                {
                                    #region Creacion del control

                                    case ControlType.CadenaTextual:
                                        txt = new TextBox();
                                        txt.Width = f.i_ControlWidth;
                                        txt.Height = f.i_HeightControl;
                                        txt.MaxLength = f.i_MaxLenght;
                                        txt.Name = f.v_ComponentFieldId;

                                        if (f.i_IsCalculate == (int)SiNo.SI)
                                        {
                                            txt.Enabled = false;
                                        }
                                        else
                                        {
                                            txt.Leave += new EventHandler(txt_Leave);
                                        }

                                        if (f.i_IsRequired == (int)SiNo.SI)
                                            SetControlValidate(f.i_ControlId, txt, null, null, uv);
                                          if (f.i_Enabled == (int)SiNo.NO)
                                            {
                                                txt.Enabled = true;
                                            }
                                            else
                                            {
                                                txt.Enabled = false;
                                            }


                                            if (f.i_ReadOnly == (int)SiNo.SI)
                                            {
                                                txt.ReadOnly = true;
                                            }
                                            else
                                            {
                                                txt.ReadOnly = false;
                                            }
                                        txt.Enter += new EventHandler(Capture_Value);

                                        if (_action == "View")
                                        {
                                            txt.ReadOnly = true;
                                        }

                                        ctl = txt;
                                        break;
                                    case ControlType.CadenaMultilinea:
                                        txt = new TextBox()
                                        {
                                            Width = f.i_ControlWidth,
                                            Height = f.i_HeightControl,
                                            Multiline = true,
                                            MaxLength = f.i_MaxLenght,
                                            ScrollBars = ScrollBars.Vertical,
                                            Name = f.v_ComponentFieldId,
                                        };
                                          if (f.i_Enabled == (int)SiNo.NO)
                                            {
                                                txt.Enabled = true;
                                            }
                                            else
                                            {
                                                txt.Enabled = false;
                                            }


                                            if (f.i_ReadOnly == (int)SiNo.SI)
                                            {
                                                txt.ReadOnly = true;
                                            }
                                            else
                                            {
                                                txt.ReadOnly = false;
                                            }
                                        txt.Enter += new EventHandler(Capture_Value);
                                        txt.Leave += new EventHandler(txt_Leave);

                                        if (f.i_IsRequired == (int)SiNo.SI)
                                            SetControlValidate(f.i_ControlId, txt, null, null, uv);

                                        if (_action == "View")
                                        {
                                            txt.ReadOnly = true;
                                        }

                                        ctl = txt;
                                        break;
                                    case ControlType.NumeroEntero:
                                        une = new UltraNumericEditor()
                                        {
                                            Width = f.i_ControlWidth,
                                            Height = f.i_HeightControl,
                                            NumericType = NumericType.Integer,
                                            PromptChar = ' ',
                                            Name = f.v_ComponentFieldId,
                                            MaskDisplayMode = MaskMode.Raw

                                        };

                                        // Asociar el control a un evento
                                        une.Enter += new EventHandler(Capture_Value);

                                        if (f.i_IsCalculate == (int)SiNo.SI)
                                        {
                                            une.ReadOnly = true;
                                            une.ValueChanged += new EventHandler(txt_ValueChanged);
                                        }
                                        else
                                        {
                                            une.Leave += new EventHandler(txt_Leave);
                                        }

                                        if (f.i_IsRequired == (int)SiNo.SI)
                                        {
                                            // Establecer condición por rangos
                                            SetControlValidate(f.i_ControlId, une, f.r_ValidateValue1, f.r_ValidateValue2, uv);
                                        }

                                        if (_action == "View")
                                        {
                                            une.ReadOnly = true;
                                        }

                                        ctl = une;
                                        break;
                                    case ControlType.NumeroDecimal:
                                        une = new UltraNumericEditor()
                                        {
                                            Width = f.i_ControlWidth,
                                            Height = f.i_HeightControl,
                                            PromptChar = ' ',
                                            Name = f.v_ComponentFieldId,
                                            NumericType = NumericType.Double,
                                            MaskDisplayMode = MaskMode.Raw

                                        };

                                        // Asociar el control a un evento
                                        une.Enter += new EventHandler(Capture_Value);

                                        if (f.i_IsCalculate == (int)SiNo.SI)
                                        {
                                            une.ValueChanged += new EventHandler(txt_ValueChanged);
                                            une.ReadOnly = true;
                                        }
                                        else
                                        {
                                            une.Leave += new EventHandler(txt_Leave);
                                        }

                                        if (f.i_IsRequired == (int)SiNo.SI)
                                        {
                                            // Establecer condición por rangos                                                              
                                            SetControlValidate(f.i_ControlId, une, f.r_ValidateValue1, f.r_ValidateValue2, uv);
                                        }

                                        if (_action == "View")
                                        {
                                            une.ReadOnly = true;
                                        }

                                        ctl = une;
                                        break;
                                    case ControlType.SiNoCheck:
                                        ctl = new CheckBox()
                                        {
                                            Width = f.i_ControlWidth,
                                            Height = f.i_HeightControl,
                                            Text = "Si/No",
                                            Name = f.v_ComponentFieldId,
                                        };

                                        ctl.Enter += new EventHandler(Capture_Value);
                                        ctl.Leave += new EventHandler(txt_Leave);

                                        if (_action == "View")
                                        {
                                            ctl.Enabled = false;
                                        }

                                        break;
                                    case ControlType.SiNoRadioButton:
                                        ctl = new RadioButton()
                                        {
                                            Width = f.i_ControlWidth,
                                            Height = f.i_HeightControl,
                                            Text = "Si/No",
                                            Name = f.v_ComponentFieldId
                                        };

                                        ctl.Enter += new EventHandler(Capture_Value);
                                        ctl.Leave += new EventHandler(txt_Leave);

                                        if (_action == "View")
                                        {
                                            ctl.Enabled = false;
                                        }

                                        break;
                                    case ControlType.SiNoCombo:
                                        ctl = new ComboBox()
                                        {
                                            Width = f.i_ControlWidth,
                                            Height = f.i_HeightControl,
                                            DropDownStyle = ComboBoxStyle.DropDownList,
                                            Name = f.v_ComponentFieldId
                                        };

                                        Utils.LoadDropDownList((ComboBox)ctl, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, f.i_GroupId, null), DropDownListAction.Select);

                                        if (f.i_IsRequired == (int)SiNo.SI)
                                        {
                                            SetControlValidate(f.i_ControlId, ctl, null, null, uv);
                                        }

                                        ctl.Enter += new EventHandler(Capture_Value);
                                        ctl.Leave += new EventHandler(txt_Leave);

                                        if (_action == "View")
                                        {
                                            ctl.Enabled = false;
                                        }

                                        break;
                                    case ControlType.UcFileUpload:
                                        var ucFileUpload = new Sigesoft.Node.WinClient.UI.UserControls.ucFileUpload();
                                        ucFileUpload.PersonId = _personId;
                                        ucFileUpload.Dni = _Dni;
                                        ucFileUpload.Fecha = lblFecInicio.Text;
                                        ucFileUpload.Consultorio = com.v_CategoryName;
                                        ucFileUpload.ServiceComponentId = com.v_ServiceComponentId;
                                        ucFileUpload.Name = f.v_ComponentFieldId;

                                        //ctl = new Sigesoft.Node.WinClient.UI.UserControls.ucFileUpload();
                                        ctl = ucFileUpload;
                                        break;
                                    case ControlType.UcOdontograma:
                                        var ucOdontograma = new Sigesoft.Node.WinClient.UI.UserControls.ucOdontograma();
                                        ucOdontograma.Name = f.v_ComponentFieldId;
                                        ctl = ucOdontograma;
                                        break;
                                    case ControlType.UcAudiometria:
                                        var ucAudiometria = new Sigesoft.Node.WinClient.UI.UserControls.ucAudiometria();
                                        ucAudiometria.Name = f.v_ComponentFieldId;
                                        ucAudiometria.PersonId = _personId;
                                        ucAudiometria.ServiceComponentId = com.v_ServiceComponentId;
                                        // Establecer evento
                                        ucAudiometria.AfterValueChange += new EventHandler<AudiometriaAfterValueChangeEventArgs>(ucAudiometria_AfterValueChange);
                                        ctl = ucAudiometria;
                                        break;
                                    case ControlType.ucEspirometria:
                                        var ucEspirometria = new Sigesoft.Node.WinClient.UI.UserControls.ucEspirometria();
                                        ucEspirometria.Name = f.v_ComponentFieldId;
                                        // Establecer evento
                                        ucEspirometria.AfterValueChange += new EventHandler<AudiometriaAfterValueChangeEventArgs>(ucAudiometria_AfterValueChange);
                                        ctl = ucEspirometria;
                                        break;
                                    case ControlType.ucOftalmologia:
                                        var ucOftalmologia = new Sigesoft.Node.WinClient.UI.UserControls.ucOftalmologia();
                                        ucOftalmologia.Name = f.v_ComponentFieldId;
                                        ucOftalmologia.Age = _age;
                                        // Establecer evento
                                        ucOftalmologia.AfterValueChange += new EventHandler<AudiometriaAfterValueChangeEventArgs>(ucAudiometria_AfterValueChange);
                                        ctl = ucOftalmologia;
                                        break;
                                    case ControlType.ucRadiografiaOIT:
                                        var ucRadiografiaOIT = new Sigesoft.Node.WinClient.UI.UserControls.ucRadiografiaOIT();
                                        ucRadiografiaOIT.Name = f.v_ComponentFieldId;
                                        // Establecer evento
                                        ucRadiografiaOIT.AfterValueChange += new EventHandler<AudiometriaAfterValueChangeEventArgs>(ucAudiometria_AfterValueChange);
                                        ctl = ucRadiografiaOIT;
                                        break;


                                    case ControlType.UcSomnolencia:
                                        var ucSomnolencia = new Sigesoft.Node.WinClient.UI.UserControls.ucSomnolencia();
                                        ucSomnolencia.Name = f.v_ComponentFieldId;
                                        // Establecer evento
                                        ucSomnolencia.AfterValueChange += new EventHandler<AudiometriaAfterValueChangeEventArgs>(ucSomnolencia_AfterValueChange);
                                        ctl = ucSomnolencia;
                                        break;

                                    case ControlType.UcBoton:
                                        var ucBoton = new Sigesoft.Node.WinClient.UI.UserControls.ucBoton();
                                        ucBoton.Name = f.v_ComponentFieldId;
                                        ucBoton.Dni = _Dni;
                                        ucBoton.Examen = com.v_Name;
                                        ucBoton.FechaServicio = _FechaServico.Value;
                                        // Establecer evento
                                        ctl = ucBoton;
                                        break;

                                    case ControlType.UcAcumetria:
                                        var ucAcumetria = new Sigesoft.Node.WinClient.UI.UserControls.ucAcumetria();
                                        ucAcumetria.Name = f.v_ComponentFieldId;
                                        // Establecer evento
                                        ctl = ucAcumetria;
                                        break;
                                    case ControlType.UcSintomaticoRespi:
                                        var ucSintomaticoRespi = new Sigesoft.Node.WinClient.UI.UserControls.ucSintomaticoResp();
                                        ucSintomaticoRespi.Name = f.v_ComponentFieldId;
                                        // Establecer evento
                                        ctl = ucSintomaticoRespi;
                                        break;
                                    case ControlType.UcRxLumboSacra:
                                        var ucRxLumboSacra = new Sigesoft.Node.WinClient.UI.UserControls.ucRXLumboSacra();
                                        ucRxLumboSacra.Name = f.v_ComponentFieldId;
                                        // Establecer evento
                                        ctl = ucRxLumboSacra;
                                        break;
                                    case ControlType.UcOjoSeco:
                                        var ucOjoSeco = new Sigesoft.Node.WinClient.UI.UserControls.ucOjoSeco();
                                        ucOjoSeco.Name = f.v_ComponentFieldId;
                                        // Establecer evento
                                        ctl = ucOjoSeco;
                                        break;

                                    case ControlType.UcOtoscopia:
                                        var ucOtoscopia = new Sigesoft.Node.WinClient.UI.UserControls.ucOtoscopia();
                                        ucOtoscopia.Name = f.v_ComponentFieldId;
                                        // Establecer evento
                                        ctl = ucOtoscopia;
                                        break;

                                    case ControlType.UcEvaluacionErgonomica:
                                        var ucEvaluacionErgonomica = new Sigesoft.Node.WinClient.UI.UserControls.ucEvaluacionErgonomica();
                                        ucEvaluacionErgonomica.Name = f.v_ComponentFieldId;
                                        // Establecer evento
                                        ctl = ucEvaluacionErgonomica;
                                        break;

                                    case ControlType.UcOsteoMuscular:
                                        var ucOsteo = new Sigesoft.Node.WinClient.UI.UserControls.ucOsteoMuscular();
                                        ucOsteo.Name = f.v_ComponentFieldId;
                                        // Establecer evento
                                        ctl = ucOsteo;
                                        break;
                                    case ControlType.ucPsicologia:

                                        if (_esoTypeId == TypeESO.PreOcupacional)
                                        {
                                            var ucPsicologiaEMPO = new Sigesoft.Node.WinClient.UI.UserControls.ucPsychologicalExam(_esoTypeId);
                                            ucPsicologiaEMPO.Name = f.v_ComponentFieldId;

                                            // Establecer evento
                                            ucPsicologiaEMPO.AfterValueChange += new EventHandler<AudiometriaAfterValueChangeEventArgs>(ucAudiometria_AfterValueChange);
                                            ctl = ucPsicologiaEMPO;
                                        }
                                        else if (_esoTypeId == TypeESO.PeriodicoAnual)
                                        {
                                            var ucPsicologiaEMOA = new Sigesoft.Node.WinClient.UI.UserControls.ucPsychologicalExamAnual(_esoTypeId);
                                            ucPsicologiaEMOA.Name = f.v_ComponentFieldId;

                                            // Establecer evento
                                            ucPsicologiaEMOA.AfterValueChange += new EventHandler<AudiometriaAfterValueChangeEventArgs>(ucAudiometria_AfterValueChange);
                                            ctl = ucPsicologiaEMOA;
                                        }

                                        break;
                                    case ControlType.Lista:
                                        cb = new ComboBox()
                                        {
                                            Width = f.i_ControlWidth,
                                            Height = f.i_HeightControl,
                                            DropDownStyle = ComboBoxStyle.DropDownList,
                                            Name = f.v_ComponentFieldId
                                        };

                                        //Utils.LoadDropDownList((ComboBox)ctl, "Value1", "Id", BLL.Utils.GetDataHierarchyForComboAndItemId(ref objOperationResult, f.i_GroupId, f.i_ItemId, null), DropDownListAction.Select);
                                        var data = BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, f.i_GroupId, null);

                                        Utils.LoadDropDownList(cb, "Value1", "Id", data, DropDownListAction.Select);

                                        if (f.i_IsRequired == (int)SiNo.SI)
                                        {
                                            SetControlValidate(f.i_ControlId, cb, null, null, uv);
                                        }

                                        // Setear levantamiento de popup para el ingreso de los hallazgos solo cuando 
                                        // se seleccione un valor alterado

                                        if ((f.v_ComponentId == Constants.EXAMEN_FISICO_ID
                                            || f.v_ComponentId == Constants.RX_ID
                                            || f.v_ComponentId == Constants.OFTALMOLOGIA_ID
                                            || f.v_ComponentId == Constants.ALTURA_ESTRUCTURAL_ID
                                            || f.v_ComponentId == Constants.TACTO_RECTAL_ID
                                            || f.v_ComponentId == Constants.EVAL_NEUROLOGICA_ID
                                            || f.v_ComponentId == Constants.TEST_ROMBERG_ID
                                            || f.v_ComponentId == Constants.TAMIZAJE_DERMATOLOGIO_ID
                                            || f.v_ComponentId == Constants.GINECOLOGIA_ID
                                            || f.v_ComponentId == Constants.EXAMEN_MAMA_ID
                                            || f.v_ComponentId == Constants.AUDIOMETRIA_ID
                                            || f.v_ComponentId == Constants.ELECTROCARDIOGRAMA_ID
                                            || f.v_ComponentId == Constants.ESPIROMETRIA_ID
                                            || f.v_ComponentId == Constants.OSTEO_MUSCULAR_ID
                                            || f.v_ComponentId == Constants.PRUEBA_ESFUERZO_ID
                                            || f.v_ComponentId == Constants.TAMIZAJE_DERMATOLOGIO_ID
                                            || f.v_ComponentId == Constants.ODONTOGRAMA_ID
                                            || f.v_ComponentId == Constants.EXAMEN_FISICO_7C_ID)
                                            && (f.i_GroupId == (int)SystemParameterGroups.ConHallazgoSinHallazgosNoSeRealizo))
                                        {
                                            cb.SelectedIndexChanged += new EventHandler(cb_SelectedIndexChanged);
                                        }

                                        // invovar popup de antecedentes medicos personales
                                        if ((f.v_ComponentId == Constants.RX_ID
                                          || f.v_ComponentId == Constants.AUDIOMETRIA_ID)
                                          && (f.i_GroupId == (int)SystemParameterGroups.SiNoNoDefine))
                                        {
                                            cb.SelectedIndexChanged += new EventHandler(cb_SelectedIndexChanged_Antecedentes);
                                        }

                                        cb.Enter += new EventHandler(Capture_Value);
                                        cb.Leave += new EventHandler(txt_Leave);

                                        if (_action == "View")
                                        {
                                            cb.Enabled = false;
                                        }

                                        ctl = cb;
                                        break;
                                    default:
                                        break;

                                    #endregion

                                }

                                //ctl.CreateControl();

                                // PONER EL CONTROL ESPECIFICO
                                ctl.Tag = new KeyTagControl
                                {
                                    i_ControlId = f.i_ControlId,
                                    v_ComponentId = f.v_ComponentId,
                                    v_ComponentFieldsId = f.v_ComponentFieldId,
                                    i_IsSourceFieldToCalculate = f.i_IsSourceFieldToCalculate,
                                    v_Formula = f.v_Formula,
                                    v_TargetFieldOfCalculateId = f.v_TargetFieldOfCalculateId,
                                    v_SourceFieldToCalculateJoin = f.v_SourceFieldToCalculateJoin,
                                    v_FormulaChild = f.v_FormulaChild,
                                    Formula = f.Formula,
                                    TargetFieldOfCalculateId = f.TargetFieldOfCalculateId,
                                    v_TextLabel = f.v_TextLabel,
                                    v_ComponentName = com.v_Name
                                };


                                // Agregar el control al contenedor
                                fila = RedondeoMayor(nroControlNET, g.i_Column * Constants.COLUMNAS_POR_CONTROL);
                                columna = nroControlNET - (fila - 1) * (g.i_Column * Constants.COLUMNAS_POR_CONTROL);
                                tblpGroup.Controls.Add(ctl, columna - 1, fila - 1);
                                nroControlNET++;

                                // label de unid medida.
                                Label lbl1 = new Label();
                                lbl1.AutoSize = false;
                                lbl1.Width = 50;
                                lbl1.Text = f.v_MeasurementUnitName;
                                lbl1.Font = new Font(lbl1.Font, FontStyle.Bold | FontStyle.Italic);
                                lbl1.TextAlign = ContentAlignment.BottomLeft;
                                fila = RedondeoMayor(nroControlNET, g.i_Column * Constants.COLUMNAS_POR_CONTROL);
                                columna = nroControlNET - (fila - 1) * (g.i_Column * Constants.COLUMNAS_POR_CONTROL);
                                tblpGroup.Controls.Add(lbl1, columna - 1, fila - 1);
                                nroControlNET++;

                            }

                            #endregion

                            gb.Controls.Add(tblpGroup);
                            tblpParent.Controls.Add(gb, 1, i);
                        }

                        tblpParent.AutoScroll = true;
                        tblpParent.Dock = DockStyle.Fill;

                    }  // Fin Si GroupedComponentsName

                }

                // Flag para disparar el evento del selectedIndexChange luego de setear los valores x default
                _cancelEventSelectedIndexChange = true;
                SetDefaultValueAfterBuildMenu();
                _cancelEventSelectedIndexChange = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void InitializeData()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(InitializeData));
            }
            else
            {

                // Cargar datos generales del paciente
                OperationResult objOperationResult = new OperationResult();

                ServiceList personData = _serviceBL.GetServicePersonData(ref objOperationResult, _serviceId);
                _OrganizationId = personData.v_OrganizationId;
                _Dni = personData.v_DocNumber;
                _FechaServico = personData.d_ServiceDate;
                _personName = string.Format("{0} {1} {2}", personData.v_FirstLastName, personData.v_SecondLastName, personData.v_FirstName);
                gbDatosPaciente.Text = string.Format("Datos del Paciente: {0} Tipo de Servicio: {1} Servicio: {2}", _personName, personData.v_ServiceTypeName, personData.v_MasterServiceName);
                _personId = personData.v_PersonId;

                if (personData.b_PersonImage != null)
                    pbPersonImage.Image = Common.Utils.byteArrayToImage(personData.b_PersonImage);
                _personImage = personData.b_PersonImage;

                _esoTypeId = (TypeESO)personData.i_EsoTypeId;

                lblTipoEso.Text = personData.v_EsoTypeName;
                lblProtocolName.Text = personData.v_ProtocolName + " /" + personData.Sede;
                lblFecVctoGlobal.Text = personData.d_GlobalExpirationDate == null ? "NO REQUIERE" : personData.d_GlobalExpirationDate.Value.ToShortDateString();
                lblFecVctoObs.Text = personData.d_ObsExpirationDate == null ? string.Empty : personData.d_ObsExpirationDate.Value.ToShortDateString();
                txtFecVctoGlobal.Text = personData.d_GlobalExpirationDate == null ? "NO REQUIERE" : personData.d_GlobalExpirationDate.Value.ToShortDateString();
                //dtpFecVctoGlobal.Value = personData.d_GlobalExpirationDate == null ? personData.d_ServiceDate.Value : personData.d_GlobalExpirationDate.Value;

                lblGeso.Text = personData.v_GroupOcupationName;
                //lblTipoServ.Text = personData.v_ServiceTypeName;
                //lblServicio.Text = personData.v_MasterServiceName;
                _masterServiceId = personData.i_MasterServiceId;
                cbAptitudEso.SelectedValue = personData.i_AptitudeStatusId.ToString();
                //cbNuevoControl.SelectedValue = personData.i_IsNewControl;
                lblFecInicio.Text = personData.d_ServiceDate == null ? string.Empty : personData.d_ServiceDate.Value.ToShortDateString();

                // calcular edad
                _age = DateTime.Today.AddTicks(-personData.d_BirthDate.Value.Ticks).Year - 1;
                lblEdad.Text = _age.ToString();
                lblGenero.Text = personData.v_GenderName;
                lblPuesto.Text = personData.v_CurrentOccupation;

                // cargar datos INICIALES de ANAMNESIS
                chkPresentaSisntomas.Checked = Convert.ToBoolean(personData.i_HasSymptomId);
                // Activar / Desactivar segun check presenta sintomas
                txtSintomaPrincipal.Enabled = chkPresentaSisntomas.Checked;
                txtValorTiempoEnfermedad.Enabled = chkPresentaSisntomas.Checked;
                cbCalendario.Enabled = chkPresentaSisntomas.Checked;

                txtSintomaPrincipal.Text = string.IsNullOrEmpty(personData.v_MainSymptom) ? "Niega" : personData.v_MainSymptom;
                txtValorTiempoEnfermedad.Text = personData.i_TimeOfDisease == null ? string.Empty : personData.i_TimeOfDisease.ToString();
                cbCalendario.SelectedValue = personData.i_TimeOfDiseaseTypeId == null ? "1" : personData.i_TimeOfDiseaseTypeId.ToString();
                txtRelato.Text = string.IsNullOrEmpty(personData.v_Story) ? "Paciente Asintomático" : personData.v_Story;

                _cancelEventSelectedIndexChange = true;
                cbSueño.SelectedValue = personData.i_DreamId == null ? "1" : personData.i_DreamId.ToString();
                cbOrina.SelectedValue = personData.i_UrineId == null ? "1" : personData.i_UrineId.ToString();
                cbDeposiciones.SelectedValue = personData.i_DepositionId == null ? "1" : personData.i_DepositionId.ToString();
                cbApetito.SelectedValue = personData.i_AppetiteId == null ? "1" : personData.i_AppetiteId.ToString();
                cbSed.SelectedValue = personData.i_ThirstId == null ? "1" : personData.i_ThirstId.ToString();
                _cancelEventSelectedIndexChange = false;

                txtHallazgos.Text = string.IsNullOrEmpty(personData.v_Findings) ? "Sin Alteración" : personData.v_Findings;
                if (personData.d_Fur != null)
                {
                    dtpFur.Checked = true;
                    dtpFur.Value = personData.d_Fur.Value.Date;
                }
                txtRegimenCatamenial.Text = personData.v_CatemenialRegime;
                cbMac.SelectedValue = personData.i_MacId == null ? "1" : personData.i_MacId.ToString();


                //-----------------------------------------------------------------
                if (personData.d_PAP != null)
                {
                    dtpPAP.Value = personData.d_PAP.Value;
                    dtpPAP.Checked = true;
                }


                if (personData.d_Mamografia != null)
                {
                    dtpMamografia.Value = personData.d_Mamografia.Value;
                    dtpMamografia.Checked = true;
                }


                txtGestapara.Text = string.IsNullOrEmpty(personData.v_Gestapara) ? "G ( )  P ( ) ( ) ( ) ( ) " : personData.v_Gestapara;
                txtMenarquia.Text = personData.v_Menarquia;
                txtCiruGine.Text = personData.v_CiruGine;

                //-----------------------------------------------------------------

                _sexType = (Gender)personData.i_SexTypeId;

                switch (_sexType)
                {
                    case Gender.MASCULINO:
                        gbAntGinecologicos.Enabled = false;
                        dtpFur.Enabled = false;
                        txtRegimenCatamenial.Enabled = false;
                        break;
                    case Gender.FEMENINO:
                        gbAntGinecologicos.Enabled = true;
                        dtpFur.Enabled = true;
                        txtRegimenCatamenial.Enabled = true;
                        break;
                    default:
                        break;
                }

                #region Antecedentes / Servicios

                // Cargar grilla
                GetAntecedentConsolidateForService(_personId);
                GetServicesConsolidateForService(_personId);

                #endregion


                // Analizar el resultado de la operación
                if (objOperationResult.Success != 1)
                {
                    MessageBox.Show(Constants.GenericErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }

        private void LoadComboBox()
        {
            // Llenado de combos

            OperationResult objOperationResult = new OperationResult();

            #region Cabecera servicio

            Utils.LoadDropDownList(cbAptitudEso, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 124, null), DropDownListAction.Select);

            #endregion

            #region Anamnesis

            Utils.LoadDropDownList(cbCalendario, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 133, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbSueño, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 183, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbOrina, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 183, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbDeposiciones, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 183, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbApetito, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 183, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbSed, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 183, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbMac, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 183, null), DropDownListAction.Select);


            //// Setear valor x defecto
            //cbSueño.SelectedValue = "1";
            //cbOrina.SelectedValue = "1";
            //cbDeposiciones.SelectedValue = "1";
            //cbApetito.SelectedValue = "1";
            //cbSed.SelectedValue = "1";

            #endregion

            #region Examenes
            var serviceComponent = BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 127, null);
            Utils.LoadDropDownList(cbEstadoComponente, "Value1", "Id", serviceComponent.FindAll(p => p.Id != ((int)ServiceComponentStatus.PorIniciar).ToString()));

            Utils.LoadDropDownList(cbTipoProcedenciaExamen, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 132, null));
            cbTipoProcedenciaExamen.SelectedValue = Convert.ToInt32(ComponenteProcedencia.Interno).ToString();

            #endregion

            #region Analisis de Diagnosticos

            Utils.LoadDropDownList(cbCalificacionFinal, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 138, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbTipoDx, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 139, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbEnviarAntecedentes, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 111, null), DropDownListAction.Select);

            #endregion


            if (objOperationResult.Success != 1)
            {
                MessageBox.Show("Error en operación:" + System.Environment.NewLine + objOperationResult.ExceptionMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void pbPersonImage_Click(object sender, EventArgs e)
        {
            if (_personImage != null)
            {
                var frm = new Popups.frmPreviewImagePerson(_personImage, _personName);
                frm.ShowDialog();
            }

        }

        #region Anamnesis

        private void btnGuardarAnamnesis_Click(object sender, EventArgs e)
        {
            if (uvAnamnesis.Validate(true, false).IsValid)
            {
                DialogResult Result = MessageBox.Show("¿Está seguro de grabar este registro?:", "CONFIRMACIÓN!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (Result == DialogResult.Yes)
                {

                    OperationResult objOperationResult = new OperationResult();

                    serviceDto serviceDTO = new serviceDto();

                    serviceDTO.v_ServiceId = _serviceId;
                    serviceDTO.v_MainSymptom = chkPresentaSisntomas.Checked ? txtSintomaPrincipal.Text : null;
                    serviceDTO.i_TimeOfDisease = chkPresentaSisntomas.Checked ? int.Parse(txtValorTiempoEnfermedad.Text) : (int?)null;
                    serviceDTO.i_TimeOfDiseaseTypeId = chkPresentaSisntomas.Checked ? int.Parse(cbCalendario.SelectedValue.ToString()) : -1;
                    serviceDTO.v_Story = txtRelato.Text;
                    serviceDTO.i_DreamId = int.Parse(cbSueño.SelectedValue.ToString());
                    serviceDTO.i_UrineId = int.Parse(cbOrina.SelectedValue.ToString());
                    serviceDTO.i_DepositionId = int.Parse(cbDeposiciones.SelectedValue.ToString());
                    serviceDTO.v_Findings = txtHallazgos.Text;
                    serviceDTO.i_AppetiteId = int.Parse(cbApetito.SelectedValue.ToString());
                    serviceDTO.i_ThirstId = int.Parse(cbSed.SelectedValue.ToString());
                    serviceDTO.d_Fur = dtpFur.Checked ? dtpFur.Value : (DateTime?)null;
                    serviceDTO.v_CatemenialRegime = txtRegimenCatamenial.Text;
                    serviceDTO.i_MacId = int.Parse(cbMac.SelectedValue.ToString());
                    serviceDTO.i_HasSymptomId = Convert.ToInt32(chkPresentaSisntomas.Checked);

                    serviceDTO.d_PAP = dtpPAP.Checked ? dtpPAP.Value : (DateTime?)null;
                    serviceDTO.d_Mamografia = dtpMamografia.Checked ? dtpMamografia.Value : (DateTime?)null;
                    serviceDTO.v_Gestapara = txtGestapara.Text;
                    serviceDTO.v_Menarquia = txtMenarquia.Text;
                    serviceDTO.v_CiruGine = txtCiruGine.Text;
                    serviceDTO.v_Findings = txtHallazgos.Text;



                    // datos de cabecera del Servicio
                    serviceDTO.i_AptitudeStatusId = int.Parse(cbAptitudEso.SelectedValue.ToString());

                    // Actualizar
                    _serviceBL.UpdateAnamnesis(ref objOperationResult, serviceDTO, Globals.ClientSession.GetAsList());

                    // Analizar el resultado de la operación
                    if (objOperationResult.Success != 1)
                    {
                        MessageBox.Show(Constants.GenericErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            else
            {
                MessageBox.Show("Por favor corrija la información ingresada. Vea los indicadores de error.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtValorTiempoEnfermedad_m(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso
                {
                    e.Handled = false;
                }
                else
                {
                    //el resto de teclas pulsadas se desactivan
                    e.Handled = true;
                }
            }
        }

        private void chkPresentaSisntomas_CheckedChanged(object sender, EventArgs e)
        {
            txtSintomaPrincipal.Enabled = chkPresentaSisntomas.Checked;
            txtValorTiempoEnfermedad.Enabled = chkPresentaSisntomas.Checked;
            cbCalendario.Enabled = chkPresentaSisntomas.Checked;

            if (chkPresentaSisntomas.Checked)
            {
                uvAnamnesis.GetValidationSettings(txtSintomaPrincipal).Condition = new OperatorCondition(ConditionOperator.NotEquals, "", true, typeof(string));
                uvAnamnesis.GetValidationSettings(txtSintomaPrincipal).IsRequired = true;
                uvAnamnesis.GetValidationSettings(txtValorTiempoEnfermedad).Condition = new OperatorCondition(ConditionOperator.NotEquals, "", true, typeof(string));
                uvAnamnesis.GetValidationSettings(txtValorTiempoEnfermedad).IsRequired = true;
                uvAnamnesis.GetValidationSettings(cbCalendario).Condition = new OperatorCondition(ConditionOperator.NotEquals, "--Seleccionar--", true, typeof(string));
                uvAnamnesis.GetValidationSettings(cbCalendario).IsRequired = true;
            }
            else
            {
                uvAnamnesis.GetValidationSettings(txtSintomaPrincipal).IsRequired = false;
                uvAnamnesis.GetValidationSettings(txtValorTiempoEnfermedad).IsRequired = false;
                uvAnamnesis.GetValidationSettings(cbCalendario).Condition = new OperatorCondition(ConditionOperator.NotEquals, "", false, typeof(string));
                uvAnamnesis.GetValidationSettings(cbCalendario).IsRequired = false;
            }

        }

        #endregion

        #region Examen

        private int RedondeoMayor(int a, int b)
        {
            return (int)Math.Ceiling((double)a / (double)b);
        }

        private void tcExamList_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            flagValueChange = false;

            _examName = e.Tab.Text;
            _componentId = e.Tab.Key;
            _serviceComponentId = e.Tab.Tag.ToString();

            EXAMENES_lblComentarios.Text = string.Format("Comentarios de {0}", _examName);
            EXAMENES_lblEstadoComponente.Text = string.Format("Estado del exámen ({0})", _examName);
            btnGuardarExamen.Text = string.Format("&Guardar ({0})", _examName);

            using (new LoadingClass.PleaseWait(this.Location, "Cargando..."))
            {
                LoadDataBySelectedComponent(_componentId);
            }

        }

        private void LoadDataBySelectedComponent(string pstrComponentId)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(LoadDataBySelectedComponent), pstrComponentId);
            }
            else
            {
                var arrComponentId = _componentId.Split('|');

                if (arrComponentId.Contains(Constants.AUDIOMETRIA_ID)
                    || arrComponentId.Contains(Constants.OFTALMOLOGIA_ID)
                    || arrComponentId.Contains(Constants.PSICOLOGIA_ID))
                {
                    btnVisorReporteExamen.Text = string.Format("&Ver Reporte de ({0})", _examName);
                    btnVisorReporteExamen.Visible = true;
                }
                else
                {
                    btnVisorReporteExamen.Visible = false;
                }
               
                
                OperationResult objOperationResult = new OperationResult();

                if (_serviceComponentsInfo != null)
                    _serviceComponentsInfo = null;

                // Mostrar data de serviceComponent
                _serviceComponentsInfo = _serviceBL.GetServiceComponentsInfo(ref objOperationResult, _serviceComponentId, _serviceId);

                if (_serviceComponentsInfo != null)
                {
                    txtComentario.Text = _serviceComponentsInfo.v_Comment;
                    cbEstadoComponente.SelectedValue = _serviceComponentsInfo.i_ServiceComponentStatusId == (int)ServiceComponentStatus.PorIniciar ? ((int)ServiceComponentStatus.Iniciado).ToString() : _serviceComponentsInfo.i_ServiceComponentStatusId.ToString();
                    cbTipoProcedenciaExamen.SelectedValue = _serviceComponentsInfo.i_ExternalInternalId == null ? "1" : _serviceComponentsInfo.i_ExternalInternalId.ToString();
                    chkApproved.Checked = Convert.ToBoolean(_serviceComponentsInfo.i_IsApprovedId);


                    #region Permisos de lectura / Escritura x componente de acuerdo al rol del usuario

                    SetSecurityByComponent();

                    #endregion

                    if (_serviceComponentsInfo.ServiceComponentFields.Count != 0)
                    {
                        // Flag para disparar el evento del selectedIndexChange luego de setear los valores x default
                        _cancelEventSelectedIndexChange = true;
                        // Llenar valores            
                        SearchControlAndSetValue(tcExamList.SelectedTab.TabPage);
                        _cancelEventSelectedIndexChange = false;
                    }
                    else
                    {
                        // Setear valores x defecto configurados en BD   
                        _cancelEventSelectedIndexChange = true;
                        SetDefaultValueBySelectedTab();
                        _cancelEventSelectedIndexChange = false;
                    }

                    // Setear campos de auditoria
                    tslUsuarioCrea.Text = string.Format("Usuario Crea : {0}", _serviceComponentsInfo.v_CreationUser);
                    tslFechaCrea.Text = string.Format("Fecha Crea : {0} {1}", _serviceComponentsInfo.d_CreationDate.Value.ToShortDateString(), _serviceComponentsInfo.d_CreationDate.Value.ToShortTimeString());
                    tslUsuarioAct.Text = string.Format("Usuario Act : {0}", _serviceComponentsInfo.v_UpdateUser == null ? string.Empty : _serviceComponentsInfo.v_UpdateUser);
                    tslFechaAct.Text = string.Format("Fecha Act : {0} {1}", _serviceComponentsInfo.d_UpdateDate == null ? string.Empty : _serviceComponentsInfo.d_UpdateDate.Value.ToShortDateString(), _serviceComponentsInfo.d_UpdateDate == null ? string.Empty : _serviceComponentsInfo.d_UpdateDate.Value.ToShortTimeString());

                }

                var diagnosticList = _serviceBL.GetServiceComponentDisgnosticsForGridView(ref objOperationResult,
                                                                                        _serviceId,
                                                                                        pstrComponentId);

                // Limpiar variable que contiene los Dx sugeridos / manuales
                if (_tmpExamDiagnosticComponentList != null)
                    _tmpExamDiagnosticComponentList = null;

                if (diagnosticList != null && diagnosticList.Count != 0)
                {
                    // Cargar la grilla de DX sugeridos / manuales
                    grdDiagnosticoPorExamenComponente.DataSource = diagnosticList;
                    lblRecordCountDiagnosticoPorExamenCom.Text = string.Format("Se encontraron {0} registros.", diagnosticList.Count());

                    // Cargar mi lista temporal con data k viene de BD
                    _tmpExamDiagnosticComponentList = diagnosticList;

                    // Analizar el resultado de la operación
                    if (objOperationResult.Success != 1)
                    {
                        MessageBox.Show(Constants.GenericErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Limpiar la grilla de DX con una entidad vacia
                    grdDiagnosticoPorExamenComponente.DataSource = new DiagnosticRepositoryList();
                    lblRecordCountDiagnosticoPorExamenCom.Text = string.Format("Se encontraron {0} registros.", 0);

                }

            }

        }

        private void ProcessControlBySelectedTab(Infragistics.Win.UltraWinTabControl.UltraTabPageControl selectedTab)
        {
            if (_serviceComponentFieldsList == null)
                _serviceComponentFieldsList = new List<ServiceComponentFieldsList>();

            KeyTagControl keyTagControl = null;

            string value1 = null;

            ServiceComponentFieldsList serviceComponentFields = null;
            ServiceComponentFieldValuesList serviceComponentFieldValues = null;

            if (this.InvokeRequired)
            {
                this.Invoke(new Action<Infragistics.Win.UltraWinTabControl.UltraTabPageControl>(ProcessControlBySelectedTab), selectedTab);
            }
            else
            {
                var serviceComponentId = selectedTab.Tab.Tag.ToString();
                var componentId = selectedTab.Tab.Key;
                var component = _tmpServiceComponentsForBuildMenuList.Find(p => p.v_ComponentId == componentId);

                foreach (var item in component.Fields)
                {
                    #region Nueva logica de busqueda de los campos por ID

                    var fields = selectedTab.Controls.Find(item.v_ComponentFieldId, true);

                    if (fields.Length != 0)
                    {
                        // Capturar objeto tag
                        keyTagControl = (KeyTagControl)fields[0].Tag;

                        // Datos de servicecomponentfieldValues Ejem: 1.80 ; 95 KG
                        value1 = GetValueControl(keyTagControl.i_ControlId, fields[0]);

                        if (keyTagControl.i_ControlId == (int)ControlType.UcOdontograma 
                            || keyTagControl.i_ControlId == (int)ControlType.UcAudiometria
                            || keyTagControl.i_ControlId == (int)ControlType.ucPsicologia
                            || keyTagControl.i_ControlId == (int)ControlType.ucEspirometria
                            || keyTagControl.i_ControlId == (int)ControlType.ucOftalmologia
                            || keyTagControl.i_ControlId == (int)ControlType.ucRadiografiaOIT)
                        {
                            foreach (var value in _tmpListValuesOdontograma)
                            {
                                #region Armar entidad de datos desde los user controls [Odontograma / Audiometria]

                                _serviceComponentFieldValuesList = new List<ServiceComponentFieldValuesList>();
                                serviceComponentFields = new ServiceComponentFieldsList();
                                serviceComponentFieldValues = new ServiceComponentFieldValuesList();

                                serviceComponentFields.v_ComponentFieldsId = value.v_ComponentFieldId;
                                serviceComponentFields.v_ServiceComponentId = serviceComponentId;

                                serviceComponentFieldValues.v_Value1 = value.v_Value1;
                                _serviceComponentFieldValuesList.Add(serviceComponentFieldValues);

                                serviceComponentFields.ServiceComponentFieldValues = _serviceComponentFieldValuesList;
                                // Agregar a mi lista
                                _serviceComponentFieldsList.Add(serviceComponentFields);

                                #endregion
                            }
                        }
                        else    // Todos los demas examenes
                        {
                            #region Armar entidad de datos que se va a grabar

                            // Datos de servicecomponentfields Ejem: Talla ; Peso ; etc
                            serviceComponentFields = new ServiceComponentFieldsList();

                            serviceComponentFields.v_ComponentFieldsId = keyTagControl.v_ComponentFieldsId;
                            serviceComponentFields.v_ServiceComponentId = serviceComponentId;

                            _serviceComponentFieldValuesList = new List<ServiceComponentFieldValuesList>();
                            serviceComponentFieldValues = new ServiceComponentFieldValuesList();

                            serviceComponentFieldValues.v_ComponentFieldValuesId = keyTagControl.v_ComponentFieldValuesId;
                            serviceComponentFieldValues.v_Value1 = value1;
                            _serviceComponentFieldValuesList.Add(serviceComponentFieldValues);

                            serviceComponentFields.ServiceComponentFieldValues = _serviceComponentFieldValuesList;

                            // Agregar a mi lista
                            _serviceComponentFieldsList.Add(serviceComponentFields);

                            #endregion
                        }
                    }

                    #endregion

                }

            }

        }

        private void btnGuardarExamen_Click(object sender, EventArgs e)
        {

            if (_isChangeValue && cbEstadoComponente.SelectedValue.ToString() == ((int)ServiceComponentStatus.Iniciado).ToString())
            {
                //e.Cancel = true;

                var result = MessageBox.Show("Ha realizado cambios, por lo tanto el estado del examen NO debe ser INICIADO", "CONFIRMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;

            }
            else
            {
                if (cbEstadoComponente.SelectedValue.ToString() == ((int)ServiceComponentStatus.Iniciado).ToString())
                {
                    MessageBox.Show("El estado debe ser diferente de INICIADO para poder grabar.", "CONFIRMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    _chkApprovedEnabled = chkApproved.Enabled;

                    SaveExamBySelectedTab(tcExamList.SelectedTab.TabPage);     
                }
                
            }
            //// logica solo afecta a laboratorio
            //if (tcExamList.SelectedTab.Text == "LABORATORIO")
            //{
            //    // buscar el combo positivo / negativo
            //    var field = (ComboBox)FindControlInCurrentTab(Constants.TOXICOLOGICO_RESULTADO_POSITIVO_NEGATIVO_ID)[0];
            //    var item = (KeyValueDTO)field.SelectedItem;

            //    if (item.Id == "1") // positivo
            //    {
            //        var result = MessageBox.Show("¿Desea finalizar el servicio completo del paciente?.", "Pregunta", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            //        if (result == DialogResult.Yes)
            //        {
            //            new ServiceBL().UpdateServiceStatus(_serviceId);
            //            return;
            //        }

            //    }
            //}

        }

        private void SaveExamBySelectedTab(Infragistics.Win.UltraWinTabControl.UltraTabPageControl selectedTab)
        {
            // Desactivar el flag de hubo alguna modificacion
            _isChangeValue = false;

            UltraValidator uv = null;

            try
            {
                var result = _dicUltraValidators.TryGetValue(_componentId, out uv);

                if (!result)
                    return;

                if (uv.Validate(false, true).IsValid)
                {
                    DialogResult Result = MessageBox.Show("¿Está seguro de grabar este registro?", "CONFIRMACIÓN!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (Result == DialogResult.Yes)
                    {
                        // Mostrar pantalla grabando...
                        this.Enabled = false;
                        //frmWaiting.Show(this);

                        #region Capturar [Comentarios, estado, procedencia de un exmanen componente]

                        var serviceComponentDto = new servicecomponentDto();
                        serviceComponentDto.v_ServiceComponentId = _serviceComponentId;
                        serviceComponentDto.v_Comment = txtComentario.Text;
                        serviceComponentDto.i_ServiceComponentStatusId = int.Parse(cbEstadoComponente.SelectedValue.ToString());
                        serviceComponentDto.i_ExternalInternalId = int.Parse(cbTipoProcedenciaExamen.SelectedValue.ToString());
                        serviceComponentDto.i_IsApprovedId = Convert.ToInt32(chkApproved.Checked);

                        serviceComponentDto.v_ComponentId = _componentId;
                        serviceComponentDto.v_ServiceId = _serviceId;

                        #endregion

                        // Generar packete con data para grabar y pasarselo al hilo 
                        RunWorkerAsyncPackage packageForSave = new RunWorkerAsyncPackage();

                        if (chkUtilizarFirma.Checked)
                        {
                            var frm = new Popups.frmSelectSignature();
                            frm.ShowDialog();

                            if (frm.DialogResult != System.Windows.Forms.DialogResult.Cancel)
                            {
                                packageForSave.i_SystemUserSuplantadorId = frm.i_SystemUserSuplantadorId;
                            }

                        }

                        packageForSave.SelectedTab = selectedTab;
                        packageForSave.ExamDiagnosticComponentList = _tmpExamDiagnosticComponentList;
                        packageForSave.ServiceComponent = serviceComponentDto;
                        //de aca que sigue
                        // Aqui se carga esa entidad package
                        //entra en un hilo paralelo bgwSaveExamen_DoWork
                        //ok pero donde lo graba en la db en el codigo ps amigo

                        bgwSaveExamen.RunWorkerAsync(packageForSave);

                    }
                }
                else
                {
                    //MessageBox.Show("Por favor corrija la información ingresada. Vea los indicadores de error.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch (Exception ex)
            {
                //CloseErrorfrmWaiting();             
                MessageBox.Show(Common.Utils.ExceptionFormatter(ex), "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bgwSaveExamen_DoWork(object sender, DoWorkEventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();

            //try
            //{
            using (new LoadingClass.PleaseWait(this.Location, "Grabando..."))
            {
                RunWorkerAsyncPackage packageForSave = (RunWorkerAsyncPackage)e.Argument;

                #region GRABAR CONTROLES DINAMICOS

                var selectedTab = (Infragistics.Win.UltraWinTabControl.UltraTabPageControl)packageForSave.SelectedTab;

                var serviceComponentId = selectedTab.Tab.Tag.ToString();

                ProcessControlBySelectedTab(selectedTab);

                var result = _serviceBL.AddServiceComponentValues(ref objOperationResult,
                                                            _serviceComponentFieldsList,
                                                            Globals.ClientSession.GetAsList(),
                                                            _personId,
                                                            serviceComponentId);

                #endregion

                #region GRABAR DATOS ADICIONALES COMO [Diagnósticos + restricciones + recomendaciones]

                // Grabar Dx por examen componente mas sus restricciones
                if (packageForSave.i_SystemUserSuplantadorId != null)
                {
                    Globals.ClientSession.i_SystemUserId = (int)packageForSave.i_SystemUserSuplantadorId;
                }
                else
                {
                    Globals.ClientSession.i_SystemUserId = Globals.ClientSession.i_SystemUserCopyId;
                }

                _serviceBL.AddDiagnosticRepository(ref objOperationResult,
                                                    packageForSave.ExamDiagnosticComponentList,
                                                    packageForSave.ServiceComponent,
                                                    Globals.ClientSession.GetAsList(),
                                                    _chkApprovedEnabled);


                #endregion

                // Limpiar lista temp
                _serviceComponentFieldsList = null;
                _tmpListValuesOdontograma = null;

                if (!result)
                {
                    MessageBox.Show("Error al grabar los componentes. Comunicarse con el administrador del sistema", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                // Analizar el resultado de la operación
                if (objOperationResult.Success != 1)
                {
                    //CloseErrorfrmWaiting();
                    MessageBox.Show(Constants.GenericErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                this.Invoke((MethodInvoker)(() =>
                {
                    #region refrescar

                    flagValueChange = false;
                    InitializeData();
                    LoadDataBySelectedComponent(_componentId);
                    GetTotalDiagnosticsForGridView();
                    ConclusionesyTratamiento_LoadAllGrid();

                    #endregion

                }));

            }

            //}
            //catch (Exception ex)
            //{
            //    CloseErrorfrmWaiting();
            //    MessageBox.Show(ex.Message);
            //}

        }

        //private void bgwSaveExamen_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    OperationResult objOperationResult = new OperationResult();

        //    //try
        //    //{
        //    using (new LoadingClass.PleaseWait(this.Location, "Grabando..."))
        //    {
        //        RunWorkerAsyncPackage packageForSave = (RunWorkerAsyncPackage)e.Argument;

        //        bool result = false;

        //        #region GRABAR CONTROLES DINAMICOS

        //        var selectedTab = (Infragistics.Win.UltraWinTabControl.UltraTabPageControl)packageForSave.SelectedTab;


        //        ProcessControlBySelectedTab(selectedTab);


        //        //Verficar si es nuevo o es una actualización

        //        if (packageForSave.ServiceComponent.d_UpdateDate == null)
        //        {
        //            result = _serviceBL.AddServiceComponentValues(ref objOperationResult,
        //                                               _serviceComponentFieldsList,
        //                                               Globals.ClientSession.GetAsList(),
        //                                               _personId,
        //                                               _serviceComponentId);
        //        }
        //        else
        //        {


        //            var DatosAntiguos = _oListValidacionAMC.AsEnumerable()
        //          .GroupBy(x => x.v_ServiceComponentFieldValuesId)
        //          .Select(group => group.First()).ToList();

        //            List<DatosModificados> ListaDatoModificado = new List<DatosModificados>();
        //            DatosModificados oDatosModificados = null;

        //            foreach (var itemAntiguo in DatosAntiguos)
        //            {
        //                var Coincidencia = _serviceComponentFieldsList.FindAll(p => p.v_ComponentFieldsId == itemAntiguo.v_ComponentFieldsId);

        //                if (Coincidencia.Count != 0)
        //                {
        //                    if (itemAntiguo.v_Value1 != Coincidencia[0].ServiceComponentFieldValues[0].v_Value1)
        //                    {
        //                        oDatosModificados = new DatosModificados();
        //                        oDatosModificados.v_ComponentFieldsId = itemAntiguo.v_ComponentFieldsId;
        //                        ListaDatoModificado.Add(oDatosModificados);
        //                    }
        //                }
        //            }

        //            //Agregar los datos nuevos

        //            foreach (var item in _serviceComponentFieldsList)
        //            {
        //                var Coincidencia = DatosAntiguos.FindAll(p => p.v_ComponentFieldsId == item.v_ComponentFieldsId);
        //                if (Coincidencia.Count == 0)
        //                {
        //                    oDatosModificados = new DatosModificados();
        //                    oDatosModificados.v_ComponentFieldsId = item.v_ComponentFieldsId;
        //                    ListaDatoModificado.Add(oDatosModificados);
        //                }
        //            }

        //            string[] ListaDatoModificado_ = new string[ListaDatoModificado.Count()];
        //            for (int i = 0; i < ListaDatoModificado.Count(); i++)
        //            {
        //                ListaDatoModificado_[i] = ListaDatoModificado[i].v_ComponentFieldsId;
        //            }

        //            //var DatosGrabar = _serviceComponentFieldsList.FindAll(p => p.v_ComponentFieldsId.Contains(ListaDatoModificado.));
        //            var DatosGrabar = _serviceComponentFieldsList.FindAll(p => ListaDatoModificado_.Contains(p.v_ComponentFieldsId));


        //            result = _serviceBL.AddServiceComponentValues(ref objOperationResult,
        //                                                        DatosGrabar,
        //                                                        Globals.ClientSession.GetAsList(),
        //                                                        _personId,
        //                                                        _serviceComponentId);
        //        }



        //        #endregion

        //        #region GRABAR DATOS ADICIONALES COMO [Diagnósticos + restricciones + recomendaciones]

        //        // Grabar Dx por examen componente mas sus restricciones
        //        if (packageForSave.i_SystemUserSuplantadorId != null)
        //        {
        //            Globals.ClientSession.i_SystemUserId = (int)packageForSave.i_SystemUserSuplantadorId;
        //        }
        //        else
        //        {
        //            Globals.ClientSession.i_SystemUserId = Globals.ClientSession.i_SystemUserCopyId;
        //        }

        //        _serviceBL.AddDiagnosticRepository(ref objOperationResult,
        //                                            packageForSave.ExamDiagnosticComponentList,
        //                                            packageForSave.ServiceComponent,
        //                                            Globals.ClientSession.GetAsList(),
        //                                            _chkApprovedEnabled);


        //        #endregion

        //        // Limpiar lista temp
        //        _serviceComponentFieldsList = null;
        //        _tmpListValuesOdontograma = null;

        //        //if (!result)
        //        //{
        //        //    MessageBox.Show("Error al grabar los componentes. Comunicarse con el administrador del sistema", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);

        //        //}

        //        // Analizar el resultado de la operación
        //        if (objOperationResult.Success != 1)
        //        {
        //            //CloseErrorfrmWaiting();
        //            MessageBox.Show(Constants.GenericErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }

        //        #region refrescar

        //        flagValueChange = false;
        //        InitializeData();
        //        LoadDataBySelectedComponent(_componentId);
        //        GetTotalDiagnosticsForGridView();
        //        ConclusionesyTratamiento_LoadAllGrid();



        //        #endregion



        //    }

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    CloseErrorfrmWaiting();
        //    //    MessageBox.Show(ex.Message);
        //    //}

        //}

        private void bgwSaveExamen_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //#region refrescar

            //flagValueChange = false;
            //InitializeData();
            //LoadDataBySelectedComponent(_componentId);
            //GetTotalDiagnosticsForGridView();
            //ConclusionesyTratamiento_LoadAllGrid();

            //#endregion      

            this.Enabled = true;
            //frmWaiting.Visible = false;     
        }

        private void btnAgregarDxExamen_Click(object sender, EventArgs e)
        {
            var frm = new Popups.frmAddExamDiagnosticComponent("New");
            frm._componentId = _componentId;
            frm._serviceId = _serviceId;

            if (_tmpExamDiagnosticComponentList != null)
            {
                frm._tmpExamDiagnosticComponentList = _tmpExamDiagnosticComponentList;
            }

            frm.ShowDialog();

            if (frm.DialogResult == DialogResult.Cancel)
                return;

            // Refrescar grilla
            // Actualizar variable
            if (frm._tmpExamDiagnosticComponentList != null)
            {
                _tmpExamDiagnosticComponentList = frm._tmpExamDiagnosticComponentList;

                var dataList = _tmpExamDiagnosticComponentList.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);
                grdDiagnosticoPorExamenComponente.DataSource = dataList;
                lblRecordCountDiagnosticoPorExamenCom.Text = string.Format("Se encontraron {0} registros.", dataList.Count());
            }
        }

        private void btnEditarDxExamen_Click(object sender, EventArgs e)
        {
            if (grdDiagnosticoPorExamenComponente.Selected.Rows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un registro.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var frm = new Popups.frmAddExamDiagnosticComponent("Edit");

            var diagnosticRepositoryId = grdDiagnosticoPorExamenComponente.Selected.Rows[0].Cells["v_DiagnosticRepositoryId"].Value.ToString();
            frm._diagnosticRepositoryId = diagnosticRepositoryId;

            frm._componentId = _componentId;
            frm._serviceId = _serviceId;

            if (_tmpExamDiagnosticComponentList != null)
            {
                frm._tmpExamDiagnosticComponentList = _tmpExamDiagnosticComponentList;
            }

            frm.ShowDialog();

            // Refrescar grilla
            // Actualizar variable

            if (frm.DialogResult == DialogResult.Cancel)
                return;

            if (frm._tmpExamDiagnosticComponentList != null)
            {
                _tmpExamDiagnosticComponentList = frm._tmpExamDiagnosticComponentList;

                var dataList = _tmpExamDiagnosticComponentList.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);
                grdDiagnosticoPorExamenComponente.DataSource = new DiagnosticRepositoryList();
                grdDiagnosticoPorExamenComponente.DataSource = dataList;
                grdDiagnosticoPorExamenComponente.Refresh();
                lblRecordCountDiagnosticoPorExamenCom.Text = string.Format("Se encontraron {0} registros.", dataList.Count());

                PintargrdDiagnosticoPorExamenComponente();
            }
        }

        private void btnRemoverDxExamen_Click(object sender, EventArgs e)
        {
            if (grdDiagnosticoPorExamenComponente.Selected.Rows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un registro.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult Result = MessageBox.Show("¿Está seguro de eliminar este registro?:", "ADVERTENCIA!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (Result == DialogResult.Yes)
            {
                // Delete the item

                // Capturar id desde la griila de restricciones
                var diagnosticRepositoryId = grdDiagnosticoPorExamenComponente.Selected.Rows[0].Cells["v_DiagnosticRepositoryId"].Value.ToString();

                int recordType = int.Parse(grdDiagnosticoPorExamenComponente.Selected.Rows[0].Cells["i_RecordType"].Value.ToString());

                // Buscar registro para remover
                var findResult = _tmpExamDiagnosticComponentList.Find(p => p.v_DiagnosticRepositoryId == diagnosticRepositoryId);

                if (recordType == (int)RecordType.Temporal)
                {
                    _tmpExamDiagnosticComponentList.Remove(findResult);
                }
                else if (recordType == (int)RecordType.NoTemporal)
                {
                    findResult.i_RecordStatus = (int)RecordStatus.EliminadoLogico;
                }

                var dataList = _tmpExamDiagnosticComponentList.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);

                //grdDiagnosticoPorExamenComponente.DataSource = new DiagnosticRepositoryList();
                grdDiagnosticoPorExamenComponente.DataSource = dataList;
                lblRecordCountDiagnosticoPorExamenCom.Text = string.Format("Se encontraron {0} registros.", _tmpExamDiagnosticComponentList.Count());
            }
        }

        private DiagnosticRepositoryList SearchDxSugeridoOfSystem(string valueToAnalyze, string pComponentFieldsId)
        {
            DiagnosticRepositoryList diagnosticRepository = null;
            string matchValId = null;
            bool exitLoop = false;
            var componentField = _tmpServiceComponentsForBuildMenuList
                                .Find(p => p.v_ComponentId == _componentId)
                                .Fields.Find(p => p.v_ComponentFieldId == pComponentFieldsId);

            if (componentField != null)
            {
                // Obtener el tipo de dato al cual se va castear un control especifico
                string dataTypeControlToParse = GetDataTypeControl(componentField.i_ControlId);

                if (componentField != null)
                {
                    foreach (ComponentFieldValues val in componentField.Values)
                    {
                        switch ((Operator2Values)val.i_OperatorId)
                        {
                            #region Analizar valor ingresado x el medico contra una serie de valores k se obtinen desde la BD

                            case Operator2Values.X_esIgualque_A:
                                if (dataTypeControlToParse == "int")
                                {
                                    if (int.Parse(valueToAnalyze) == int.Parse(val.v_AnalyzingValue1))
                                        exitLoop = true;
                                }
                                else if (dataTypeControlToParse == "double")
                                {
                                    if (double.Parse(valueToAnalyze) == double.Parse(val.v_AnalyzingValue1))
                                        exitLoop = true;
                                }
                                break;
                            case Operator2Values.X_noesIgualque_A:
                                if (dataTypeControlToParse == "int")
                                {
                                    if (int.Parse(valueToAnalyze) != int.Parse(val.v_AnalyzingValue1))
                                        exitLoop = true;
                                }
                                else if (dataTypeControlToParse == "double")
                                {
                                    if (double.Parse(valueToAnalyze) != double.Parse(val.v_AnalyzingValue1))
                                        exitLoop = true;
                                }
                                break;
                            case Operator2Values.X_esMenorque_A:
                                if (dataTypeControlToParse == "int")
                                {
                                    if (int.Parse(valueToAnalyze) < int.Parse(val.v_AnalyzingValue1))
                                        exitLoop = true;
                                }
                                else if (dataTypeControlToParse == "double")
                                {
                                    // X < 18.5 (bajo peso)
                                    if (double.Parse(valueToAnalyze) < double.Parse(val.v_AnalyzingValue1))
                                        exitLoop = true;
                                }
                                break;
                            case Operator2Values.X_esMenorIgualque_A:
                                if (dataTypeControlToParse == "int")
                                {
                                    if (int.Parse(valueToAnalyze) <= int.Parse(val.v_AnalyzingValue1))
                                        exitLoop = true;
                                }
                                else if (dataTypeControlToParse == "double")
                                {
                                    // X < 18.5 (bajo peso)
                                    if (double.Parse(valueToAnalyze) <= double.Parse(val.v_AnalyzingValue1))
                                        exitLoop = true;
                                }
                                break;
                            case Operator2Values.X_esMayorque_A:
                                if (dataTypeControlToParse == "int")
                                {
                                    if (int.Parse(valueToAnalyze) > int.Parse(val.v_AnalyzingValue1))
                                        exitLoop = true;
                                }
                                else if (dataTypeControlToParse == "double")
                                {
                                    if (double.Parse(valueToAnalyze) > double.Parse(val.v_AnalyzingValue1))
                                        exitLoop = true;
                                }
                                break;
                            case Operator2Values.X_esMayorIgualque_A:
                                // X >= 40.0 (Obesidad clase III)
                                if (dataTypeControlToParse == "int")
                                {
                                    if (int.Parse(valueToAnalyze) >= int.Parse(val.v_AnalyzingValue1))
                                        exitLoop = true;
                                }
                                else if (dataTypeControlToParse == "double")
                                {
                                    // X < 18.5 (bajo peso)
                                    if (double.Parse(valueToAnalyze) >= double.Parse(val.v_AnalyzingValue1))
                                        exitLoop = true;
                                }
                                break;
                            case Operator2Values.X_esMayorque_A_yMenorque_B:

                                if (dataTypeControlToParse == "int")
                                {
                                    if (int.Parse(valueToAnalyze) > int.Parse(val.v_AnalyzingValue1) && int.Parse(valueToAnalyze) < int.Parse(val.v_AnalyzingValue2))
                                        exitLoop = true;
                                }
                                else if (dataTypeControlToParse == "double")
                                {
                                    if (double.Parse(valueToAnalyze) > double.Parse(val.v_AnalyzingValue1) && double.Parse(valueToAnalyze) < double.Parse(val.v_AnalyzingValue2))
                                        exitLoop = true;
                                }
                                break;
                            case Operator2Values.X_esMayorque_A_yMenorIgualque_B:
                                if (dataTypeControlToParse == "int")
                                {
                                    if (int.Parse(valueToAnalyze) > int.Parse(val.v_AnalyzingValue1) && int.Parse(valueToAnalyze) <= int.Parse(val.v_AnalyzingValue2))
                                        exitLoop = true;
                                }
                                else if (dataTypeControlToParse == "double")
                                {
                                    // X < A && X <= B 
                                    if (double.Parse(valueToAnalyze) > double.Parse(val.v_AnalyzingValue1) && double.Parse(valueToAnalyze) <= double.Parse(val.v_AnalyzingValue2))
                                        exitLoop = true;
                                }
                                break;
                            case Operator2Values.X_esMayorIgualque_A_yMenorque_B:
                                if (dataTypeControlToParse == "int")
                                {
                                    if (int.Parse(valueToAnalyze) >= int.Parse(val.v_AnalyzingValue1) && int.Parse(valueToAnalyze) < int.Parse(val.v_AnalyzingValue2))
                                        exitLoop = true;
                                }
                                else if (dataTypeControlToParse == "double")
                                {
                                    if (double.Parse(valueToAnalyze) >= double.Parse(val.v_AnalyzingValue1) && double.Parse(valueToAnalyze) < double.Parse(val.v_AnalyzingValue2))
                                        exitLoop = true;
                                }
                                break;
                            case Operator2Values.X_esMayorIgualque_A_yMenorIgualque_B:
                                if (dataTypeControlToParse == "int")
                                {
                                    if (int.Parse(valueToAnalyze) >= int.Parse(val.v_AnalyzingValue1) && int.Parse(valueToAnalyze) <= int.Parse(val.v_AnalyzingValue2))
                                        exitLoop = true;
                                }
                                else if (dataTypeControlToParse == "double")
                                {
                                    var parse = double.Parse(valueToAnalyze);
                                    if (double.Parse(valueToAnalyze) >= double.Parse(val.v_AnalyzingValue1) && double.Parse(valueToAnalyze) <= double.Parse(val.v_AnalyzingValue2))
                                        exitLoop = true;
                                }
                                break;
                            default:
                                MessageBox.Show("valor no encontrado " + valueToAnalyze);
                                break;

                            #endregion
                        }

                        if (exitLoop)
                        {
                            #region CREAR / AGREGAR DX (automático)

                            matchValId = val.v_ComponentFieldValuesId;

                            // Si el valor analizado se encuentra en el rango de valores NORMALES, 
                            // entonces NO se genera un DX (automático).
                            if (val.v_DiseasesId == null)
                                break;

                            val.Recomendations.ForEach(item => { item.v_RecommendationId = Guid.NewGuid().ToString(); });
                            val.Restrictions.ForEach(item => { item.v_RestrictionByDiagnosticId = Guid.NewGuid().ToString(); });
                            // Insertar DX sugerido (automático) a la bolsa de DX 
                            diagnosticRepository = new DiagnosticRepositoryList();
                            diagnosticRepository.v_DiagnosticRepositoryId = Guid.NewGuid().ToString();
                            diagnosticRepository.v_DiseasesId = val.v_DiseasesId;
                            diagnosticRepository.i_AutoManualId = (int)AutoManual.Automático;
                            diagnosticRepository.i_PreQualificationId = (int)PreQualification.SinPreCalificar;
                            diagnosticRepository.i_FinalQualificationId = (int)FinalQualification.SinCalificar;
                            diagnosticRepository.v_ServiceId = _serviceId;
                            diagnosticRepository.v_ComponentId = val.v_ComponentId;
                            diagnosticRepository.v_DiseasesName = val.v_DiseasesName;
                            diagnosticRepository.v_AutoManualName = "AUTOMÁTICO";
                            diagnosticRepository.v_RestrictionsName = ConcatenateRestrictions(val.Restrictions);
                            diagnosticRepository.v_RecomendationsName = ConcatenateRecommendations(val.Recomendations);
                            diagnosticRepository.v_PreQualificationName = "SIN PRE-CALIFICAR";
                            // ID enlace DX automatico para grabar valores dinamicos
                            diagnosticRepository.v_ComponentFieldValuesId = val.v_ComponentFieldValuesId;
                            diagnosticRepository.v_ComponentFieldsId = pComponentFieldsId;
                            diagnosticRepository.Recomendations = RefreshRecomendationList(val.Recomendations);
                            diagnosticRepository.Restrictions = RefreshRestrictionList(val.Restrictions);
                            diagnosticRepository.i_RecordStatus = (int)RecordStatus.Agregado;
                            diagnosticRepository.i_RecordType = (int)RecordType.Temporal;

                            int vm = val.i_ValidationMonths == null ? 0 : val.i_ValidationMonths.Value;
                            diagnosticRepository.d_ExpirationDateDiagnostic = DateTime.Now.AddMonths(vm);

                            #endregion
                            break;
                        }

                    }
                }
            }

            return diagnosticRepository;

        }

        private void grdDiagnosticoPorExamenComponente_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            var caliFinal = (PreQualification)e.Row.Cells["i_PreQualificationId"].Value;

            switch (caliFinal)
            {
                case PreQualification.SinPreCalificar:
                    e.Row.Appearance.BackColor = Color.Pink;
                    e.Row.Appearance.BackColor2 = Color.Pink;
                    break;
                case PreQualification.Aceptado:
                    e.Row.Appearance.BackColor = Color.LawnGreen;
                    e.Row.Appearance.BackColor2 = Color.LawnGreen;
                    break;
                case PreQualification.Rechazado:
                    e.Row.Appearance.BackColor = Color.DarkGray;
                    e.Row.Appearance.BackColor2 = Color.DarkGray;
                    break;
                default:
                    break;
            }

            //Y doy el efecto degradado vertical
            e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
        }

        private void grdDiagnosticoPorExamenComponente_AfterPerformAction(object sender, AfterUltraGridPerformActionEventArgs e)
        {
            ValidateRemoveDxAutomatic();
        }

        private void grdDiagnosticoPorExamenComponente_ClickCell(object sender, ClickCellEventArgs e)
        {
            ValidateRemoveDxAutomatic();
        }

        private void SaveExamWherePendingChange()
        {
            #region Validacion antes de navegar de tab en tab

            if (_componentId != null)
            {
                var audiometria = _tmpServiceComponentsForBuildMenuList
                                      .Find(p => p.v_ComponentId == _componentId)
                                      .Fields.Find(p => p.i_ControlId == (int)ControlType.UcAudiometria);

                if (audiometria != null)
                {
                    var ucAudiometria = (UserControls.ucAudiometria)FindControlInCurrentTab(audiometria.v_ComponentFieldId)[0];

                    if (ucAudiometria.IsChangeValueControl)
                    {
                        _isChangeValue = true;
                    }
                }

                var odontograma = _tmpServiceComponentsForBuildMenuList
                                      .Find(p => p.v_ComponentId == _componentId)
                                      .Fields.Find(p => p.i_ControlId == (int)ControlType.UcOdontograma);

                if (odontograma != null)
                {
                    var ucOdontograma = (UserControls.ucOdontograma)FindControlInCurrentTab(odontograma.v_ComponentFieldId)[0];

                    //if (ucOdontograma.IsChangeValueControl)
                    //{
                    //    _isChangeValue = true;

                    //}

                    if (ucOdontograma.IsChangeValueControl)
                    {
                        _isChangeValue = true;
                        ucOdontograma.IsChangeValueControl = false;
                    }

                }

                var ExamenPsicologico = _tmpServiceComponentsForBuildMenuList
                                         .Find(p => p.v_ComponentId == _componentId)
                                         .Fields.Find(p => p.i_ControlId == (int)ControlType.ucPsicologia);

                if (ExamenPsicologico != null)
                {
                    if (_esoTypeId == TypeESO.PeriodicoAnual)
                    {
                        var ucPsicologiaEMOA = (UserControls.ucPsychologicalExamAnual)FindControlInCurrentTab(ExamenPsicologico.v_ComponentFieldId)[0];

                        if (ucPsicologiaEMOA.IsChangeValueControl)
                        {
                            _isChangeValue = true;
                            ucPsicologiaEMOA.IsChangeValueControl = false;
                        }
                    }
                    else if (_esoTypeId == TypeESO.PreOcupacional)
                    {
                        var ucPsicologiaEMPO = (UserControls.ucPsychologicalExam)FindControlInCurrentTab(ExamenPsicologico.v_ComponentFieldId)[0];

                        if (ucPsicologiaEMPO.IsChangeValueControl)
                        {
                            _isChangeValue = true;
                            ucPsicologiaEMPO.IsChangeValueControl = false;
                        }
                    }

                }

            }

            if (_isChangeValue)
            {
                //e.Cancel = true;

                var result = MessageBox.Show("Ha realizado cambios, desea guardarlos antes de ir a otro exámen.", "CONFIRMACIÓN", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveExamBySelectedTab(tcExamList.SelectedTab.TabPage);
                }
                else
                {
                    _isChangeValue = false;
                    //e.Cancel = false;                  
                }

            }

            #endregion
        }

        private void tcExamList_SelectedTabChanging(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangingEventArgs e)
        {
            var obj = e.Tab.TabControl.ActiveTab;

            SaveExamWherePendingChange();
        }

        #region Util

        private void CloseErrorfrmWaiting()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(CloseErrorfrmWaiting));
            }
            else
            {
                this.Enabled = true;
                frmWaiting.Visible = false;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void SetSecurityByComponent()
        {
            OperationResult objOperationResult = new OperationResult();

            var nodeId = Globals.ClientSession.i_CurrentExecutionNodeId;
            var roleId = Globals.ClientSession.i_RoleId.Value;

            bool isReadOnly = false;
            bool isWriteOnly = false;

            // Obtener campos de un componente especifico
            var componentFields = _tmpServiceComponentsForBuildMenuList.Find(p => p.v_ComponentId == _componentId).Fields;
            // Obtener permisos de cada examen de un rol especifico
            var componentProfile = _serviceBL.GetRoleNodeComponentProfileAll(ref objOperationResult, nodeId, roleId, _componentId);

            // todos los examenes de un consultorio permitidos o no
            var groupCompo = componentFields.GroupBy(g => g.v_ComponentId).Select(g => g.First()).OrderBy(o => o.v_ComponentId).ToList();                

            if (componentProfile != null)
            {
                foreach (var item in groupCompo)
                {
                    // buscar dentro de todos los examenes de un consultorio los exmenes permitidos
                    var cp = componentProfile.Find(p => p.v_ComponentId == item.v_ComponentId);

                    if (cp != null)
                    {

                        #region Evaluar lectura/escritura

                        if (cp.i_Read == (int)SiNo.SI && cp.i_Write == (int)SiNo.SI)
                        {
                            isReadOnly = false;
                            btnGuardarExamen.Enabled = true;
                        }
                        else
                        {
                            isReadOnly = true;
                            btnGuardarExamen.Enabled = false;
                        }

                        if (cp.i_Write == (int)SiNo.SI)
                        {
                            isWriteOnly = true;
                            btnGuardarExamen.Enabled = true;
                        }
                        else
                        {
                            isWriteOnly = false;
                            btnGuardarExamen.Enabled = false;
                        }

                        #endregion

                        #region Es Diagnosticable

                        if (cp.i_Dx == (int)SiNo.SI)
                        {
                            //la sección se activa o desactiva dependiendo del PERMISO. Los diagnósticos automáticos deben seguir funcionando y reportándose.
                            btnAgregarDxExamen.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_EXAMENES_ADDDX", _formActions);
                            btnEditarDxExamen.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_EXAMENES_EDITDX", _formActions);
                            btnRemoverDxExamen.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_EXAMENES_REMOVEDX", _formActions);

                        }

                        if (cp.i_Dx == (int)SiNo.NO)
                        {
                            // toda la sección esta desactivada, pero los diagnósticos automáticos deben seguir funcionando y reportándose.
                            btnAgregarDxExamen.Enabled = false;
                            btnEditarDxExamen.Enabled = false;
                            btnRemoverDxExamen.Enabled = false;
                            isDisabledButtonsExamDx = true;
                        }

                        #endregion

                        #region Es Aprobable?

                        if (cp.i_Approved == (int)SiNo.NO)
                        {
                            // el check de APROBADO está desactivado. No importando el permiso del rol
                            chkApproved.Enabled = false;
                        }
                        else if (cp.i_Approved == (int)SiNo.SI)
                        {
                            // el check se activa o desactiva dependiendo del rol
                            chkApproved.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_EXAMENES_APPROVED", _formActions);
                        }

                        #endregion              

                        // Obtener campos de un componente
                        var fieldsByCompo = componentFields.FindAll(p => p.v_ComponentId == cp.v_ComponentId);

                        foreach (ComponentFieldsList cf in fieldsByCompo)
                        {
                            var ctrl__ = tcExamList.SelectedTab.TabPage.Controls.Find(cf.v_ComponentFieldId, true);

                            if (ctrl__.Length != 0)
                            {
                                #region Setear valor

                                switch ((ControlType)cf.i_ControlId)
                                {
                                    case ControlType.CadenaTextual:
                                        TextBox txtt = (TextBox)ctrl__[0];
                                        txtt.CreateControl();
                                        txtt.ReadOnly = isReadOnly;
                                        if (_action == "View")
                                        {
                                            txtt.ReadOnly = true;
                                        }
                                        break;
                                    case ControlType.CadenaMultilinea:
                                        TextBox txtm = (TextBox)ctrl__[0];
                                        txtm.CreateControl();
                                        txtm.ReadOnly = isReadOnly;
                                        if (_action == "View")
                                        {
                                            txtm.ReadOnly = true;
                                        }
                                        break;
                                    case ControlType.NumeroEntero:
                                        UltraNumericEditor uni = (UltraNumericEditor)ctrl__[0];
                                        uni.CreateControl();
                                        uni.ReadOnly = isReadOnly;
                                        if (_action == "View")
                                        {
                                            uni.ReadOnly = true;
                                        }
                                        break;
                                    case ControlType.NumeroDecimal:
                                        UltraNumericEditor und = (UltraNumericEditor)ctrl__[0];
                                        und.CreateControl();
                                        und.ReadOnly = isReadOnly;
                                        if (_action == "View")
                                        {
                                            und.ReadOnly = true;
                                        }
                                        break;
                                    case ControlType.SiNoCheck:
                                        CheckBox chkSiNo = (CheckBox)ctrl__[0];
                                        chkSiNo.CreateControl();
                                        chkSiNo.Enabled = isWriteOnly;
                                        if (_action == "View")
                                        {
                                            chkSiNo.Enabled = false;
                                        }
                                        break;
                                    case ControlType.SiNoRadioButton:
                                        RadioButton rbSiNo = (RadioButton)ctrl__[0];
                                        rbSiNo.CreateControl();
                                        rbSiNo.Enabled = isWriteOnly;
                                        if (_action == "View")
                                        {
                                            rbSiNo.Enabled = false;
                                        }
                                        break;
                                    case ControlType.SiNoCombo:
                                        ComboBox cbSiNo = (ComboBox)ctrl__[0];
                                        cbSiNo.CreateControl();
                                        cbSiNo.Enabled = isWriteOnly;
                                        if (_action == "View")
                                        {
                                            cbSiNo.Enabled = false;
                                        }
                                        break;
                                    case ControlType.UcFileUpload:
                                        break;
                                    case ControlType.Lista:
                                        ComboBox cbList = (ComboBox)ctrl__[0];
                                        cbList.CreateControl();
                                        cbList.Enabled = isWriteOnly;
                                        if (_action == "View")
                                        {
                                            cbList.Enabled = false;
                                        }
                                        break;
                                    default:
                                        break;
                                }

                                #endregion
                            }

                        }

                    }
                    else
                    {
                        // Obtener campos de un componente
                        var fieldsByCompo = componentFields.FindAll(p => p.v_ComponentId == item.v_ComponentId);

                        foreach (ComponentFieldsList cf in fieldsByCompo)
                        {                           
                            var ctrl__ = tcExamList.SelectedTab.TabPage.Controls.Find(cf.v_ComponentFieldId, true);

                            if (ctrl__.Length != 0)
                            {
                                #region Setear valor

                                switch ((ControlType)cf.i_ControlId)
                                {
                                    case ControlType.CadenaTextual:
                                        TextBox txtt = (TextBox)ctrl__[0];
                                        txtt.CreateControl();
                                        txtt.ReadOnly = true;
                                        if (_action == "View")
                                        {
                                            txtt.ReadOnly = true;
                                        }
                                        break;
                                    case ControlType.CadenaMultilinea:
                                        TextBox txtm = (TextBox)ctrl__[0];
                                        txtm.CreateControl();
                                        txtm.ReadOnly = true;
                                        if (_action == "View")
                                        {
                                            txtm.ReadOnly = true;
                                        }
                                        break;
                                    case ControlType.NumeroEntero:
                                        UltraNumericEditor uni = (UltraNumericEditor)ctrl__[0];
                                        uni.CreateControl();
                                        uni.ReadOnly = true;
                                        if (_action == "View")
                                        {
                                            uni.ReadOnly = true;
                                        }
                                        break;
                                    case ControlType.NumeroDecimal:
                                        UltraNumericEditor und = (UltraNumericEditor)ctrl__[0];
                                        und.CreateControl();
                                        und.ReadOnly = true;
                                        if (_action == "View")
                                        {
                                            und.ReadOnly = true;
                                        }
                                        break;
                                    case ControlType.SiNoCheck:
                                        CheckBox chkSiNo = (CheckBox)ctrl__[0];
                                        chkSiNo.CreateControl();
                                        chkSiNo.Enabled = false;
                                        if (_action == "View")
                                        {
                                            chkSiNo.Enabled = false;
                                        }
                                        break;
                                    case ControlType.SiNoRadioButton:
                                        RadioButton rbSiNo = (RadioButton)ctrl__[0];
                                        rbSiNo.CreateControl();
                                        rbSiNo.Enabled = false;
                                        if (_action == "View")
                                        {
                                            rbSiNo.Enabled = false;
                                        }
                                        break;
                                    case ControlType.SiNoCombo:
                                        ComboBox cbSiNo = (ComboBox)ctrl__[0];
                                        cbSiNo.CreateControl();
                                        cbSiNo.Enabled = false;
                                        if (_action == "View")
                                        {
                                            cbSiNo.Enabled = false;
                                        }
                                        break;
                                    case ControlType.UcFileUpload:
                                        break;
                                    case ControlType.Lista:
                                        ComboBox cbList = (ComboBox)ctrl__[0];
                                        cbList.CreateControl();
                                        cbList.Enabled = false;
                                        if (_action == "View")
                                        {
                                            cbList.Enabled = false;
                                        }
                                        break;
                                    default:
                                        break;
                                }

                                #endregion
                            }

                        }
                    }                  

                }

                //foreach (var cp in componentProfile)
                //{
                //    #region Evaluar lectura/escritura
                                     
                //    if (cp.i_Read == (int)SiNo.SI && cp.i_Write == (int)SiNo.SI)
                //    {
                //        isReadOnly = false;
                //        btnGuardarExamen.Enabled = true;
                //    }
                //    else
                //    {
                //        isReadOnly = true;
                //        btnGuardarExamen.Enabled = false;
                //    }

                //    if (cp.i_Write == (int)SiNo.SI)
                //    {
                //        isWriteOnly = true;
                //        btnGuardarExamen.Enabled = true;
                //    }
                //    else
                //    {
                //        isWriteOnly = false;
                //        btnGuardarExamen.Enabled = false;
                //    }

                //    #endregion

                //    // Obtener campos de un componente
                //    var fieldsByCompo = componentFields.FindAll(p => p.v_ComponentId == cp.v_ComponentId);                 
                     
                //    #region Establecer permisos Lectura / escritura a cada campo de un examen componente

                //    foreach (ComponentFieldsList cf in componentFields)
                //    {
                //        var xxx = componentFields.FindAll(p => p.v_ComponentId == cf.v_ComponentId);  

                //        var ctrl__ = tcExamList.SelectedTab.TabPage.Controls.Find(cf.v_ComponentFieldId, true);
                        
                //        if (ctrl__.Length != 0)
                //        {
                //            //var tagControl = (KeyTagControl)ctrl__[0].Tag;

                //            if (cf.v_ComponentId == cp.v_ComponentId)
                //            {

                //                #region Setear valor

                //                switch ((ControlType)cf.i_ControlId)
                //                {
                //                    case ControlType.CadenaTextual:
                //                        TextBox txtt = (TextBox)ctrl__[0];
                //                        txtt.CreateControl();
                //                        txtt.ReadOnly = isReadOnly;
                //                        if (_action == "View")
                //                        {
                //                            txtt.ReadOnly = true;
                //                        }
                //                        break;
                //                    case ControlType.CadenaMultilinea:
                //                        TextBox txtm = (TextBox)ctrl__[0];
                //                        txtm.CreateControl();
                //                        txtm.ReadOnly = isReadOnly;
                //                        if (_action == "View")
                //                        {
                //                            txtm.ReadOnly = true;
                //                        }
                //                        break;
                //                    case ControlType.NumeroEntero:
                //                        UltraNumericEditor uni = (UltraNumericEditor)ctrl__[0];
                //                        uni.CreateControl();
                //                        uni.ReadOnly = isReadOnly;
                //                        if (_action == "View")
                //                        {
                //                            uni.ReadOnly = true;
                //                        }
                //                        break;
                //                    case ControlType.NumeroDecimal:
                //                        UltraNumericEditor und = (UltraNumericEditor)ctrl__[0];
                //                        und.CreateControl();
                //                        und.ReadOnly = isReadOnly;
                //                        if (_action == "View")
                //                        {
                //                            und.ReadOnly = true;
                //                        }
                //                        break;
                //                    case ControlType.SiNoCheck:
                //                        CheckBox chkSiNo = (CheckBox)ctrl__[0];
                //                        chkSiNo.CreateControl();
                //                        chkSiNo.Enabled = isWriteOnly;
                //                        if (_action == "View")
                //                        {
                //                            chkSiNo.Enabled = false;
                //                        }
                //                        break;
                //                    case ControlType.SiNoRadioButton:
                //                        RadioButton rbSiNo = (RadioButton)ctrl__[0];
                //                        rbSiNo.CreateControl();
                //                        rbSiNo.Enabled = isWriteOnly;
                //                        if (_action == "View")
                //                        {
                //                            rbSiNo.Enabled = false;
                //                        }
                //                        break;
                //                    case ControlType.SiNoCombo:
                //                        ComboBox cbSiNo = (ComboBox)ctrl__[0];
                //                        cbSiNo.CreateControl();
                //                        cbSiNo.Enabled = isWriteOnly;
                //                        if (_action == "View")
                //                        {
                //                            cbSiNo.Enabled = false;
                //                        }
                //                        break;
                //                    case ControlType.UcFileUpload:
                //                        break;
                //                    case ControlType.Lista:
                //                        ComboBox cbList = (ComboBox)ctrl__[0];
                //                        cbList.CreateControl();
                //                        cbList.Enabled = isWriteOnly;
                //                        if (_action == "View")
                //                        {
                //                            cbList.Enabled = false;
                //                        }
                //                        break;
                //                    default:
                //                        break;
                //                }

                //                #endregion

                //            }
                //            else
                //            {
                //                #region Setear valor

                //                switch ((ControlType)cf.i_ControlId)
                //                {
                //                    case ControlType.CadenaTextual:
                //                        TextBox txtt = (TextBox)ctrl__[0];
                //                        txtt.CreateControl();
                //                        txtt.ReadOnly = true;
                //                        if (_action == "View")
                //                        {
                //                            txtt.ReadOnly = true;
                //                        }
                //                        break;
                //                    case ControlType.CadenaMultilinea:
                //                        TextBox txtm = (TextBox)ctrl__[0];
                //                        txtm.CreateControl();
                //                        txtm.ReadOnly = true;
                //                        if (_action == "View")
                //                        {
                //                            txtm.ReadOnly = true;
                //                        }
                //                        break;
                //                    case ControlType.NumeroEntero:
                //                        UltraNumericEditor uni = (UltraNumericEditor)ctrl__[0];
                //                        uni.CreateControl();
                //                        uni.ReadOnly = true;
                //                        if (_action == "View")
                //                        {
                //                            uni.ReadOnly = true;
                //                        }
                //                        break;
                //                    case ControlType.NumeroDecimal:
                //                        UltraNumericEditor und = (UltraNumericEditor)ctrl__[0];
                //                        und.CreateControl();
                //                        und.ReadOnly = true;
                //                        if (_action == "View")
                //                        {
                //                            und.ReadOnly = true;
                //                        }
                //                        break;
                //                    case ControlType.SiNoCheck:
                //                        CheckBox chkSiNo = (CheckBox)ctrl__[0];
                //                        chkSiNo.CreateControl();
                //                        chkSiNo.Enabled = false;
                //                        if (_action == "View")
                //                        {
                //                            chkSiNo.Enabled = false;
                //                        }
                //                        break;
                //                    case ControlType.SiNoRadioButton:
                //                        RadioButton rbSiNo = (RadioButton)ctrl__[0];
                //                        rbSiNo.CreateControl();
                //                        rbSiNo.Enabled = false;
                //                        if (_action == "View")
                //                        {
                //                            rbSiNo.Enabled = false;
                //                        }
                //                        break;
                //                    case ControlType.SiNoCombo:
                //                        ComboBox cbSiNo = (ComboBox)ctrl__[0];
                //                        cbSiNo.CreateControl();
                //                        cbSiNo.Enabled = false;
                //                        if (_action == "View")
                //                        {
                //                            cbSiNo.Enabled = false;
                //                        }
                //                        break;
                //                    case ControlType.UcFileUpload:
                //                        break;
                //                    case ControlType.Lista:
                //                        ComboBox cbList = (ComboBox)ctrl__[0];
                //                        cbList.CreateControl();
                //                        cbList.Enabled = false;
                //                        if (_action == "View")
                //                        {
                //                            cbList.Enabled = false;
                //                        }
                //                        break;
                //                    default:
                //                        break;
                //                }

                //                #endregion
                //            }

                //        }
                //    }

                //    #endregion

                //    #region Es Diagnosticable

                //    if (cp.i_Dx == (int)SiNo.SI)
                //    {
                //        //la sección se activa o desactiva dependiendo del PERMISO. Los diagnósticos automáticos deben seguir funcionando y reportándose.
                //        btnAgregarDxExamen.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_EXAMENES_ADDDX", _formActions);
                //        btnEditarDxExamen.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_EXAMENES_EDITDX", _formActions);
                //        btnRemoverDxExamen.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_EXAMENES_REMOVEDX", _formActions);

                //    }

                //    if (cp.i_Dx == (int)SiNo.NO)
                //    {
                //        // toda la sección esta desactivada, pero los diagnósticos automáticos deben seguir funcionando y reportándose.
                //        btnAgregarDxExamen.Enabled = false;
                //        btnEditarDxExamen.Enabled = false;
                //        btnRemoverDxExamen.Enabled = false;
                //        isDisabledButtonsExamDx = true;
                //    }

                //    #endregion

                //    #region Es Aprobable?

                //    if (cp.i_Approved == (int)SiNo.NO)
                //    {
                //        // el check de APROBADO está desactivado. No importando el permiso del rol
                //        chkApproved.Enabled = false;
                //    }
                //    else if (cp.i_Approved == (int)SiNo.SI)
                //    {
                //        // el check se activa o desactiva dependiendo del rol
                //        chkApproved.Enabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmEso_EXAMENES_APPROVED", _formActions);
                //    }

                //    #endregion              

                //}

            }
            else
            {
                #region Establecer permisos Lectura / escritura a cada campo de un examen componente

                foreach (ComponentFieldsList cf in componentFields)
                {
                    var ctrl__ = tcExamList.SelectedTab.TabPage.Controls.Find(cf.v_ComponentFieldId, true);

                    if (ctrl__.Length != 0)
                    {
                        #region Setear valor

                        switch ((ControlType)cf.i_ControlId)
                        {
                            case ControlType.CadenaTextual:
                                TextBox txtt = (TextBox)ctrl__[0];
                                txtt.CreateControl();
                                txtt.ReadOnly = true;
                                if (_action == "View")
                                {
                                    txtt.ReadOnly = true;
                                }
                                break;
                            case ControlType.CadenaMultilinea:
                                TextBox txtm = (TextBox)ctrl__[0];
                                txtm.CreateControl();
                                txtm.ReadOnly = true;
                                if (_action == "View")
                                {
                                    txtm.ReadOnly = true;
                                }
                                break;
                            case ControlType.NumeroEntero:
                                UltraNumericEditor uni = (UltraNumericEditor)ctrl__[0];
                                uni.CreateControl();
                                uni.ReadOnly = true;
                                if (_action == "View")
                                {
                                    uni.ReadOnly = true;
                                }
                                break;
                            case ControlType.NumeroDecimal:
                                UltraNumericEditor und = (UltraNumericEditor)ctrl__[0];
                                und.CreateControl();
                                und.ReadOnly = true;
                                if (_action == "View")
                                {
                                    und.ReadOnly = true;
                                }
                                break;
                            case ControlType.SiNoCheck:
                                CheckBox chkSiNo = (CheckBox)ctrl__[0];
                                chkSiNo.CreateControl();
                                chkSiNo.Enabled = false;
                                if (_action == "View")
                                {
                                    chkSiNo.Enabled = false;
                                }
                                break;
                            case ControlType.SiNoRadioButton:
                                RadioButton rbSiNo = (RadioButton)ctrl__[0];
                                rbSiNo.CreateControl();
                                rbSiNo.Enabled = false;
                                if (_action == "View")
                                {
                                    rbSiNo.Enabled = false;
                                }
                                break;
                            case ControlType.SiNoCombo:
                                ComboBox cbSiNo = (ComboBox)ctrl__[0];
                                cbSiNo.CreateControl();
                                cbSiNo.Enabled = false;
                                if (_action == "View")
                                {
                                    cbSiNo.Enabled = false;
                                }
                                break;
                            case ControlType.UcFileUpload:
                                break;
                            case ControlType.Lista:
                                ComboBox cbList = (ComboBox)ctrl__[0];
                                cbList.CreateControl();
                                cbList.Enabled = false;
                                if (_action == "View")
                                {
                                    cbList.Enabled = false;
                                }
                                break;
                            default:
                                break;
                        }

                        #endregion
                    }
                }

                #endregion

                // toda la sección esta desactivada, pero los diagnósticos automáticos deben seguir funcionando y reportándose.
                btnGuardarExamen.Enabled = false;
                btnAgregarDxExamen.Enabled = false;
                btnEditarDxExamen.Enabled = false;
                btnRemoverDxExamen.Enabled = false;
                isDisabledButtonsExamDx = true;

                // el check se activa o desactiva dependiendo del rol
                chkApproved.Enabled = false;
            }

        }

        private UltraValidator CreateUltraValidatorByComponentId(string componentId)
        {
            UltraValidator uv = new UltraValidator(this.components);

            if (_dicUltraValidators == null)
                _dicUltraValidators = new Dictionary<string, UltraValidator>();

            _dicUltraValidators.Add(componentId, uv);
            return uv;
        }

        private Control[] FindDynamicControl(string key)
        {
            // Obtener TabPage actual
            var currentTabPage = tcExamList.SelectedTab.TabPage;
            //var findControl = currentTabPage.Controls.Find(key, true);

            var findControl = tcExamList.Tabs.TabControl.Controls.Find(key, true);

            return findControl;
        }

        private Control[] FindControlInCurrentTab(string key)
        {
            // Obtener TabPage actual
            var currentTabPage = tcExamList.SelectedTab.TabPage;
            var findControl = currentTabPage.Controls.Find(key, true);
            return findControl;
        }

        private void SearchControlAndSetValue(Control ctrlContainer)
        {
            KeyTagControl keyTagControl = null;
            bool breakHazChildrenUC = false;
            List<ServiceComponentFieldValuesList> dataSourceUserControls = null;

            foreach (Control ctrl in ctrlContainer.Controls)
            {
                if (ctrl.Tag != null)
                {
                    var t = ctrl.Tag.GetType();

                    // Los controles que tienen el objeto KeyTagControl en su propiedad Tag son los controles que se crean dinamicamente
                    // y tienen una logica particular de muestreo de datos

                    if (t == typeof(KeyTagControl))
                    {
                        // Capturar objeto tag
                        keyTagControl = (KeyTagControl)ctrl.Tag;

                        if (keyTagControl.i_ControlId == (int)ControlType.UcOdontograma)
                        {
                            #region Setear valores en Odontograma

                            dataSourceUserControls = _serviceComponentsInfo.ServiceComponentFields.SelectMany(p => p.ServiceComponentFieldValues).ToList();
                            dataSourceUserControls = dataSourceUserControls.FindAll(p => p.v_ComponentFieldId.Contains("ODO"));
                            ((UserControls.ucOdontograma)ctrl).OdoContext = OdoContext.FillFromESO;
                            ((UserControls.ucOdontograma)ctrl)._ListaOdontograma = new List<ServiceComponentFieldValuesList>();
                            //((UserControls.ucOdontograma)ctrl).DataSource = new List<ServiceComponentFieldValuesList>();
                            ((UserControls.ucOdontograma)ctrl).DataSource = dataSourceUserControls;
                            breakHazChildrenUC = true;

                            #endregion

                        }
                        else if (keyTagControl.i_ControlId == (int)ControlType.UcAudiometria)
                        {
                            #region Setear valores en Audiometria

                            dataSourceUserControls = _serviceComponentsInfo.ServiceComponentFields.SelectMany(p => p.ServiceComponentFieldValues).ToList();
                            dataSourceUserControls = dataSourceUserControls.FindAll(p => p.v_ComponentFieldId.Contains("AUD"));
                            ((UserControls.ucAudiometria)ctrl).DataSource = new List<ServiceComponentFieldValuesList>();
                            ((UserControls.ucAudiometria)ctrl).DataSource = dataSourceUserControls;
                            breakHazChildrenUC = true;

                            #endregion
                        }
                        else if (keyTagControl.i_ControlId == (int)ControlType.ucPsicologia)
                        {
                            #region Setear valores en Psicologia

                            dataSourceUserControls = _serviceComponentsInfo.ServiceComponentFields.SelectMany(p => p.ServiceComponentFieldValues).ToList();
                            dataSourceUserControls = dataSourceUserControls.FindAll(p => p.v_ComponentFieldId.Contains("PSI"));

                            if (_esoTypeId == TypeESO.PreOcupacional)
                            {
                                ((UserControls.ucPsychologicalExam)ctrl).DataSource = new List<ServiceComponentFieldValuesList>();
                                ((UserControls.ucPsychologicalExam)ctrl).DataSource = dataSourceUserControls;
                            }
                            else if (_esoTypeId == TypeESO.PeriodicoAnual)
                            {
                                ((UserControls.ucPsychologicalExamAnual)ctrl).DataSource = new List<ServiceComponentFieldValuesList>();
                                ((UserControls.ucPsychologicalExamAnual)ctrl).DataSource = dataSourceUserControls;
                            }

                            breakHazChildrenUC = true;

                            #endregion
                        }
                        else if (keyTagControl.i_ControlId == (int)ControlType.ucEspirometria)
                        {
                            #region Setear valores en Espirometria

                            dataSourceUserControls = _serviceComponentsInfo.ServiceComponentFields.SelectMany(p => p.ServiceComponentFieldValues).ToList();
                            dataSourceUserControls = dataSourceUserControls.FindAll(p => p.v_ComponentFieldId.Contains("ESP"));
                            ((UserControls.ucEspirometria)ctrl).DataSource = new List<ServiceComponentFieldValuesList>();
                            ((UserControls.ucEspirometria)ctrl).DataSource = dataSourceUserControls;
                            breakHazChildrenUC = true;

                            #endregion
                        }
                        else if (keyTagControl.i_ControlId == (int)ControlType.ucOftalmologia)
                        {
                            #region Setear valores en Oftalmologia

                            dataSourceUserControls = _serviceComponentsInfo.ServiceComponentFields.SelectMany(p => p.ServiceComponentFieldValues).ToList();
                            dataSourceUserControls = dataSourceUserControls.FindAll(p => p.v_ComponentFieldId.Contains("OFT"));
                            ((UserControls.ucOftalmologia)ctrl).DataSource = new List<ServiceComponentFieldValuesList>();
                            ((UserControls.ucOftalmologia)ctrl).DataSource = dataSourceUserControls;
                            breakHazChildrenUC = true;

                            #endregion
                        }
                        else if (keyTagControl.i_ControlId == (int)ControlType.ucRadiografiaOIT)
                        {
                            #region Setear valores en Oftalmologia

                            dataSourceUserControls = _serviceComponentsInfo.ServiceComponentFields.SelectMany(p => p.ServiceComponentFieldValues).ToList();
                            dataSourceUserControls = dataSourceUserControls.FindAll(p => p.v_ComponentFieldId.Contains("OIT"));
                            ((UserControls.ucRadiografiaOIT)ctrl).DataSource = new List<ServiceComponentFieldValuesList>();
                            ((UserControls.ucRadiografiaOIT)ctrl).DataSource = dataSourceUserControls;
                            breakHazChildrenUC = true;

                            #endregion
                        }
                        else
                        {
                            foreach (var item in _serviceComponentsInfo.ServiceComponentFields)
                            {
                                var componentFieldsId = item.v_ComponentFieldsId;

                                foreach (var fv in item.ServiceComponentFieldValues)
                                {
                                    #region Setear valores en el caso de controles dinamicos

                                    SetValueControl(keyTagControl.i_ControlId,
                                                    ctrl,
                                                    componentFieldsId,
                                                    keyTagControl.v_ComponentFieldsId,
                                                    fv.v_Value1,
                                                    item.i_HasAutomaticDxId == null ? (int)SiNo.NO : (SiNo)item.i_HasAutomaticDxId);

                                    #endregion
                                }
                            }
                        }
                    }
                }

                if (ctrl.HasChildren)
                {
                    if (!breakHazChildrenUC && keyTagControl == null)
                    {
                        SearchControlAndSetValue(ctrl);
                    }
                }
            }

        }

        private Control GetControl(int ControlId, Control ctrl)
        {
            Control ctrlToCast = null;

            switch ((ControlType)ControlId)
            {
                case ControlType.NumeroEntero:
                    ctrlToCast = (UltraNumericEditor)ctrl;
                    break;
                case ControlType.NumeroDecimal:
                    ctrlToCast = (UltraNumericEditor)ctrl;
                    break;
                case ControlType.SiNoCheck:
                    ctrlToCast = (CheckBox)ctrl;
                    break;
                case ControlType.SiNoRadioButton:
                    ctrlToCast = (RadioButton)ctrl;
                    break;
                case ControlType.SiNoCombo:
                    ctrlToCast = (ComboBox)ctrl;
                    break;
                case ControlType.Lista:
                    ctrlToCast = (ComboBox)ctrl;
                    break;
                default:
                    break;
            }

            return ctrl;
        }

        private string GetDataTypeControl(int ControlId)
        {
            string dataType = null;

            switch ((ControlType)ControlId)
            {
                case ControlType.NumeroEntero:
                    dataType = "int";
                    break;
                case ControlType.NumeroDecimal:
                    dataType = "double";
                    break;
                case ControlType.SiNoCheck:
                    dataType = "int";
                    break;
                case ControlType.SiNoRadioButton:
                    break;
                case ControlType.SiNoCombo:
                    break;
                case ControlType.Lista:
                    dataType = "int";
                    break;
                default:
                    break;
            }

            return dataType;
        }

        private string GetValueControl(int ControlId, Control ctrl)
        {
            string value1 = null;

            switch ((ControlType)ControlId)
            {
                case ControlType.CadenaTextual:
                    value1 = ((TextBox)ctrl).Text;
                    break;
                case ControlType.CadenaMultilinea:
                    value1 = ((TextBox)ctrl).Text;
                    break;
                case ControlType.NumeroEntero:
                    value1 = ((UltraNumericEditor)ctrl).Value.ToString();
                    break;
                case ControlType.NumeroDecimal:
                    //value1 = ((UltraNumericEditor)ctrl).Value.ToString();
                    value1 = ctrl.Text.Trim();
                    break;
                case ControlType.SiNoCheck:
                    value1 = Convert.ToInt32(((CheckBox)ctrl).Checked).ToString();
                    break;
                case ControlType.SiNoRadioButton:
                    value1 = Convert.ToInt32(((RadioButton)ctrl).Checked).ToString();
                    break;
                case ControlType.SiNoCombo:
                    value1 = ((ComboBox)ctrl).SelectedValue.ToString();
                    break;
                case ControlType.Lista:
                    value1 = ((ComboBox)ctrl).SelectedValue.ToString();
                    break;
                case ControlType.UcOdontograma:
                    _tmpListValuesOdontograma = ((UserControls.ucOdontograma)ctrl).DataSource;
                    break;
                case ControlType.UcAudiometria:
                    _tmpListValuesOdontograma = ((UserControls.ucAudiometria)ctrl).DataSource;
                    break;
                case ControlType.ucPsicologia:
                    if (_esoTypeId == TypeESO.PreOcupacional)
                    {
                        _tmpListValuesOdontograma = ((UserControls.ucPsychologicalExam)ctrl).DataSource;
                    }
                    else if (_esoTypeId == TypeESO.PeriodicoAnual)
                    {
                        _tmpListValuesOdontograma = ((UserControls.ucPsychologicalExamAnual)ctrl).DataSource;
                    }             
                    break;
                case ControlType.ucEspirometria:
                    _tmpListValuesOdontograma = ((UserControls.ucEspirometria)ctrl).DataSource;
                    break;
                case ControlType.ucOftalmologia:
                    _tmpListValuesOdontograma = ((UserControls.ucOftalmologia)ctrl).DataSource;
                    break;
                case ControlType.ucRadiografiaOIT:
                    _tmpListValuesOdontograma = ((UserControls.ucRadiografiaOIT)ctrl).DataSource;
                    break;
                default:
                    break;
            }

            return value1;
        }

        private void SetValueControl(int ControlId, Control ctrl, string ComponentFieldsId, string Tag_ComponentFieldsId, string Value1, SiNo HasAutomaticDx)
        {
            switch ((ControlType)ControlId)
            {
                case ControlType.CadenaTextual:
                    if (ComponentFieldsId == Tag_ComponentFieldsId)
                    {
                        ((TextBox)ctrl).Text = Value1;
                        if (HasAutomaticDx == SiNo.SI)
                            ctrl.BackColor = Color.Pink;
                        else
                            ctrl.BackColor = Color.White;

                    }
                    break;
                case ControlType.CadenaMultilinea:
                    if (ComponentFieldsId == Tag_ComponentFieldsId)
                    {
                        ((TextBox)ctrl).Text = Value1;
                        if (HasAutomaticDx == SiNo.SI)
                            ctrl.BackColor = Color.Pink;
                        else
                            ctrl.BackColor = Color.White;

                    }
                    break;
                case ControlType.NumeroEntero:
                    if (ComponentFieldsId == Tag_ComponentFieldsId)
                    {
                        if (string.IsNullOrEmpty(Value1))
                            Value1 = "0";

                        ((UltraNumericEditor)ctrl).Value = Value1;
                        if (HasAutomaticDx == SiNo.SI)
                            ctrl.BackColor = Color.Pink;
                        else
                            ctrl.BackColor = Color.White;
                    }
                    break;
                case ControlType.NumeroDecimal:
                    if (ComponentFieldsId == Tag_ComponentFieldsId)
                    {
                        if (string.IsNullOrEmpty(Value1))
                            Value1 = "0";

                        ((UltraNumericEditor)ctrl).Value = Value1;
                        if (HasAutomaticDx == SiNo.SI)
                            ctrl.BackColor = Color.Pink;
                        else
                            ctrl.BackColor = Color.White;

                    }
                    break;
                case ControlType.SiNoCheck:
                    if (ComponentFieldsId == Tag_ComponentFieldsId)
                    {
                        if (Value1 == null) Value1 = "0";
                        if (Value1 == "true") Value1 = "1";
                        if (Value1 == "false") Value1 = "0";
                        ((CheckBox)ctrl).Checked = Convert.ToBoolean(int.Parse(Value1));
                    }
                    break;
                case ControlType.SiNoRadioButton:
                    if (ComponentFieldsId == Tag_ComponentFieldsId)
                    {
                        ((RadioButton)ctrl).Checked = Convert.ToBoolean(int.Parse(Value1));
                    }
                    break;
                case ControlType.SiNoCombo:
                    if (ComponentFieldsId == Tag_ComponentFieldsId)
                    {
                        ((ComboBox)ctrl).SelectedValue = Value1;
                    }
                    break;

                    //DAVID Importante a Revisar para Replicar IDA y VUELTA de Antecedentes
                case ControlType.Lista:
                    if (ComponentFieldsId == Tag_ComponentFieldsId)
                    {
                        var cb = (ComboBox)ctrl;
                      
                        //if (ComponentFieldsId == Constants.AUDIOMETRIA_ANTECEDENTES_SORDERA)
                        //{
                        //    var ent = new ServiceBL().GetPersonMedicalHistoryForESOBuildMenu(_personId, Constants.SORDERA);
                        //    var selectedValue = string.Empty;

                        //    if (ent != null)
                        //    {
                        //        if (ent.i_AnswerId == (int)SiNo.NO)
                        //        {
                        //            selectedValue = ((int)SiNoNoDefine.No).ToString();
                        //        }
                        //        else if (ent.i_AnswerId == (int)SiNo.SI)
                        //        {
                        //            selectedValue = ((int)SiNoNoDefine.Si).ToString();
                        //        }
                        //        else if (ent.i_AnswerId == (int)SiNo.NONE)
                        //        {
                        //            selectedValue = ((int)SiNoNoDefine.NoDefine).ToString();
                        //        }
                        //        else
                        //        {
                        //            selectedValue = "-1";
                        //        }
                        //        cb.SelectedValue = selectedValue;
                        //    }
                        //    else
                        //    {
                        //        cb.SelectedValue = string.IsNullOrEmpty(ComponentFieldsId) ? "-1" : ComponentFieldsId;
                        //    }

                        //}
                        //else
                        //{
                        //    cb.SelectedValue = Value1;
                        //}
                        cb.SelectedValue = Value1 == null ? "-1" : Value1;
                    }
                    break;
                default:
                    break;
            }
        }

        private void ClearAndSetValueControlByDefault(int ControlId, Control ctrl)
        {
            switch ((ControlType)ControlId)
            {
                case ControlType.CadenaTextual:
                    ((TextBox)ctrl).Text = string.Empty;
                    ctrl.BackColor = Color.White;
                    break;
                case ControlType.CadenaMultilinea:
                    ((TextBox)ctrl).Text = string.Empty;
                    ctrl.BackColor = Color.White;
                    break;
                case ControlType.NumeroEntero:
                    ((UltraNumericEditor)ctrl).Value = 0;
                    ctrl.BackColor = Color.White;
                    break;
                case ControlType.NumeroDecimal:
                    ((UltraNumericEditor)ctrl).Value = 0.00;
                    ctrl.BackColor = Color.White;
                    break;
                case ControlType.SiNoCheck:
                    ((CheckBox)ctrl).Checked = false;
                    break;
                case ControlType.SiNoRadioButton:
                    ((RadioButton)ctrl).Checked = false;
                    break;
                case ControlType.SiNoCombo:
                    ((ComboBox)ctrl).SelectedValue = "-1";
                    break;
                case ControlType.Lista:
                    var cb = (ComboBox)ctrl;
                    cb.SelectedValue = "-1";
                    break;
                default:
                    break;
            }
        }

        private List<RestrictionList> RefreshRestrictionList(List<RestrictionList> prestrictions)
        {
            var restrictionsList = new List<RestrictionList>();

            foreach (var item in prestrictions)
            {
                // Agregar restricciones (Automáticas) a la Lista mas lo que ya tiene
                RestrictionList restriction = new RestrictionList();

                restriction.v_RestrictionByDiagnosticId = item.v_RestrictionByDiagnosticId;
                restriction.v_ServiceId = _serviceId;
                restriction.v_DiagnosticRepositoryId = item.v_DiagnosticRepositoryId;
                restriction.v_MasterRestrictionId = item.v_MasterRestrictionId;
                restriction.v_RestrictionName = item.v_RestrictionName;
                restriction.i_RecordStatus = (int)RecordStatus.Agregado;
                restriction.i_RecordType = (int)RecordType.Temporal;
                restriction.v_ComponentId = item.v_ComponentId;

                restrictionsList.Add(restriction);
            }

            return restrictionsList;
        }

        private List<RecomendationList> RefreshRecomendationList(List<RecomendationList> precomendations)
        {
            var recomendationsList = new List<RecomendationList>();

            foreach (var item in precomendations)
            {
                // Agregar restricciones a la Lista mas lo que ya tiene
                RecomendationList recomendation = new RecomendationList();

                recomendation.v_RecommendationId = item.v_RecommendationId;
                recomendation.v_ServiceId = _serviceId;
                recomendation.v_DiagnosticRepositoryId = item.v_DiagnosticRepositoryId;
                recomendation.v_RecommendationId = item.v_RecommendationId;
                recomendation.v_MasterRecommendationId = item.v_MasterRecommendationId;  // ID -> RECOME / RESTRIC (BOLSA CONFIG POR M. MENDEZ)
                recomendation.v_RecommendationName = item.v_RecommendationName;
                recomendation.i_RecordStatus = (int)RecordStatus.Agregado;
                recomendation.i_RecordType = (int)RecordType.Temporal;
                recomendation.v_ComponentId = item.v_ComponentId;

                recomendationsList.Add(recomendation);
            }

            return recomendationsList;
        }

        private string ConcatenateRestrictions(List<RestrictionList> prestrictions)
        {
            if (prestrictions == null)
                return string.Empty;

            var qry = (from a in prestrictions  // RESTRICCIONES POR Diagnosticos                                           
                       where a.i_RecordStatus != (int)RecordStatus.EliminadoLogico
                       select new
                       {
                           v_RestrictionsName = a.v_RestrictionName
                       }).ToList();

            return string.Join(", ", qry.Select(p => p.v_RestrictionsName));
        }

        private string ConcatenateRecommendations(List<RecomendationList> precomendations)
        {
            if (precomendations == null)
                return string.Empty;

            var qry = (from a in precomendations  // RESTRICCIONES POR Diagnosticos                                           
                       where a.i_RecordStatus != (int)RecordStatus.EliminadoLogico
                       select new
                       {
                           v_RecommendationName = a.v_RecommendationName
                       }).ToList();

            return string.Join(", ", qry.Select(p => p.v_RecommendationName));
        }

        private void SetControlValidate(int ControlId, Control ctrl, float? ValidateValue1, float? ValidateValue2, UltraValidator uv)
        {
            // Objetos para validar
            RangeCondition rc = null;
            ValidationSettings vs = null;

            uv.ErrorAppearance.BackColor = Color.FromArgb(255, 255, 192);
            uv.ErrorAppearance.BackGradientStyle = GradientStyle.Vertical;
            uv.ErrorAppearance.BorderColor = Color.Pink;
            uv.NotificationSettings.Action = NotificationAction.MessageBox;

            switch ((ControlType)ControlId)
            {
                case ControlType.CadenaTextual:
                    uv.GetValidationSettings((TextBox)ctrl).IsRequired = true;
                    break;
                case ControlType.CadenaMultilinea:
                    uv.GetValidationSettings((TextBox)ctrl).IsRequired = true;
                    break;
                case ControlType.NumeroEntero:
                    // Establecer condición por rangos
                    rc = new Infragistics.Win.RangeCondition(ValidateValue1,
                                                             ValidateValue2,
                                                             typeof(int));
                    vs = uv.GetValidationSettings((UltraNumericEditor)ctrl);
                    vs.Condition = rc;
                    break;
                case ControlType.NumeroDecimal:
                    // Establecer condición por rangos
                    rc = new Infragistics.Win.RangeCondition(ValidateValue1,
                                                             ValidateValue2,
                                                             typeof(double));
                    vs = uv.GetValidationSettings((UltraNumericEditor)ctrl);
                    vs.Condition = rc;
                    break;
                case ControlType.SiNoCheck:
                    break;
                case ControlType.SiNoRadioButton:
                    break;
                case ControlType.SiNoCombo:
                    uv.GetValidationSettings(ctrl).Condition = new OperatorCondition(ConditionOperator.NotEquals, "--Seleccionar--", true, typeof(string));
                    uv.GetValidationSettings(ctrl).EmptyValueCriteria = EmptyValueCriteria.NullOrEmptyString;
                    uv.GetValidationSettings(ctrl).IsRequired = true;
                    break;
                case ControlType.UcFileUpload:
                    break;
                case ControlType.Lista:
                    uv.GetValidationSettings(ctrl).Condition = new OperatorCondition(ConditionOperator.NotEquals, "--Seleccionar--", true, typeof(string));
                    uv.GetValidationSettings(ctrl).EmptyValueCriteria = EmptyValueCriteria.NullOrEmptyString;
                    uv.GetValidationSettings(ctrl).IsRequired = true;
                    break;
                default:
                    break;
            }
        }

        private void PintargrdDiagnosticoPorExamenComponente()
        {
            // Pinta fila seleccionada
            for (int i = 0; i < grdDiagnosticoPorExamenComponente.Rows.Count; i++)
            {
                var caliFinal = (PreQualification)grdDiagnosticoPorExamenComponente.Rows[i].Cells["i_PreQualificationId"].Value;

                switch (caliFinal)
                {
                    case PreQualification.SinPreCalificar:
                        grdDiagnosticoPorExamenComponente.Rows[i].Appearance.BackColor = Color.Pink;
                        grdDiagnosticoPorExamenComponente.Rows[i].Appearance.BackColor2 = Color.Pink;
                        break;
                    case PreQualification.Aceptado:
                        grdDiagnosticoPorExamenComponente.Rows[i].Appearance.BackColor = Color.LawnGreen;
                        grdDiagnosticoPorExamenComponente.Rows[i].Appearance.BackColor2 = Color.LawnGreen;
                        break;
                    case PreQualification.Rechazado:
                        grdDiagnosticoPorExamenComponente.Rows[i].Appearance.BackColor = Color.DarkGray;
                        grdDiagnosticoPorExamenComponente.Rows[i].Appearance.BackColor2 = Color.DarkGray;

                        break;
                    default:
                        break;
                }
                //Y doy el efecto degradado vertical
                grdDiagnosticoPorExamenComponente.Rows[i].Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;

            }
        }

        private void ValidateRemoveDxAutomatic()
        {
            if (grdDiagnosticoPorExamenComponente.Selected.Rows.Count > 0)
            {
                if (isDisabledButtonsExamDx)
                {
                    var autoManualId = (AutoManual)grdDiagnosticoPorExamenComponente.Selected.Rows[0].Cells["i_AutoManualId"].Value;

                    switch (autoManualId)
                    {
                        case AutoManual.Automático:
                            btnRemoverDxExamen.Enabled = false;
                            break;
                        case AutoManual.Manual:
                            btnRemoverDxExamen.Enabled = true;
                            break;
                        default:
                            break;
                    }
                }
            }

        }

        private void SetDefaultValueAfterBuildMenu()
        {
            try
            {
                // Establecer valores x defecto  a los controles
                foreach (BE.ComponentList com in _tmpServiceComponentsForBuildMenuList)
                {
                    // Capturar tab [Triaje, sensometria]
                    var findTab = tcExamList.Tabs[com.v_ComponentId];

                    foreach (ComponentFieldsList cf in com.Fields)
                    {
                        var ctrl__ = findTab.TabPage.Controls.Find(cf.v_ComponentFieldId, true);

                        if (ctrl__.Length != 0)
                        {
                            #region Setear valor x defecto del control

                            switch ((ControlType)cf.i_ControlId)
                            {
                                case ControlType.CadenaTextual:
                                    TextBox txtt = (TextBox)ctrl__[0];
                                    txtt.CreateControl();
                                    txtt.Text = cf.v_DefaultText;
                                    txtt.BackColor = Color.White;
                                    break;
                                case ControlType.CadenaMultilinea:
                                    TextBox txtm = (TextBox)ctrl__[0];
                                    txtm.CreateControl();
                                    txtm.Text = cf.v_DefaultText;
                                    txtm.BackColor = Color.White;
                                    break;
                                case ControlType.NumeroEntero:
                                    UltraNumericEditor uni = (UltraNumericEditor)ctrl__[0];
                                    uni.CreateControl();
                                    uni.Value = string.IsNullOrEmpty(cf.v_DefaultText) ? 0 : int.Parse(cf.v_DefaultText);
                                    uni.BackColor = Color.White;
                                    break;
                                case ControlType.NumeroDecimal:
                                    UltraNumericEditor und = (UltraNumericEditor)ctrl__[0];
                                    und.CreateControl();
                                    und.Value = string.IsNullOrEmpty(cf.v_DefaultText) ? 0 : double.Parse(cf.v_DefaultText);
                                    und.BackColor = Color.White;
                                    break;
                                case ControlType.SiNoCheck:
                                    CheckBox chkSiNo = (CheckBox)ctrl__[0];
                                    chkSiNo.CreateControl();
                                    chkSiNo.Checked = string.IsNullOrEmpty(cf.v_DefaultText) ? false : Convert.ToBoolean(int.Parse(cf.v_DefaultText));
                                    break;
                                case ControlType.SiNoRadioButton:
                                    RadioButton rbSiNo = (RadioButton)ctrl__[0];
                                    rbSiNo.CreateControl();
                                    rbSiNo.Checked = string.IsNullOrEmpty(cf.v_DefaultText) ? false : Convert.ToBoolean(int.Parse(cf.v_DefaultText));
                                    break;
                                case ControlType.SiNoCombo:
                                    ComboBox cbSiNo = (ComboBox)ctrl__[0];
                                    cbSiNo.CreateControl();
                                    cbSiNo.SelectedValue = string.IsNullOrEmpty(cf.v_DefaultText) ? "-1" : cf.v_DefaultText;
                                    break;
                                case ControlType.UcFileUpload:
                                    break;
                                case ControlType.Lista:
                                    ComboBox cbList = (ComboBox)ctrl__[0];
                                    cbList.CreateControl();                                 
                                    cbList.SelectedValue = string.IsNullOrEmpty(cf.v_DefaultText) ? "-1" : cf.v_DefaultText;                                
                                    break;
                                case ControlType.UcOdontograma:
                                    //((UserControls.ucOdontograma)ctrl__[0]).ClearValueControl();;
                                    break;
                                case ControlType.UcAudiometria:
                                    ((UserControls.ucAudiometria)ctrl__[0]).ClearValueControl();
                                    break;
                                default:
                                    break;
                            }

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetDefaultValueBySelectedTab()
        {
            try
            {
                var component = _tmpServiceComponentsForBuildMenuList.Find(p => p.v_ComponentId == _componentId);

                foreach (ComponentFieldsList cf in component.Fields)
                {
                    var field = tcExamList.SelectedTab.TabPage.Controls.Find(cf.v_ComponentFieldId, true);

                    if (field.Length != 0)
                    {
                        #region Setear valor x defecto del control

                        switch ((ControlType)cf.i_ControlId)
                        {
                            case ControlType.CadenaTextual:
                                TextBox txtt = (TextBox)field[0];
                                txtt.CreateControl();
                                txtt.Text = cf.v_DefaultText;
                                txtt.BackColor = Color.White;
                                break;
                            case ControlType.CadenaMultilinea:
                                TextBox txtm = (TextBox)field[0];
                                txtm.CreateControl();
                                txtm.Text = cf.v_DefaultText;
                                txtm.BackColor = Color.White;
                                break;
                            case ControlType.NumeroEntero:
                                UltraNumericEditor uni = (UltraNumericEditor)field[0];
                                uni.CreateControl();
                                uni.Value = string.IsNullOrEmpty(cf.v_DefaultText) ? 0 : int.Parse(cf.v_DefaultText);
                                uni.BackColor = Color.White;
                                break;
                            case ControlType.NumeroDecimal:
                                UltraNumericEditor und = (UltraNumericEditor)field[0];
                                und.CreateControl();
                                und.Value = string.IsNullOrEmpty(cf.v_DefaultText) ? 0 : double.Parse(cf.v_DefaultText);
                                und.BackColor = Color.White;
                                break;
                            case ControlType.SiNoCheck:
                                CheckBox chkSiNo = (CheckBox)field[0];
                                chkSiNo.CreateControl();
                                chkSiNo.Checked = string.IsNullOrEmpty(cf.v_DefaultText) ? false : Convert.ToBoolean(int.Parse(cf.v_DefaultText));
                                break;
                            case ControlType.SiNoRadioButton:
                                RadioButton rbSiNo = (RadioButton)field[0];
                                rbSiNo.CreateControl();
                                rbSiNo.Checked = string.IsNullOrEmpty(cf.v_DefaultText) ? false : Convert.ToBoolean(int.Parse(cf.v_DefaultText));
                                break;
                            case ControlType.SiNoCombo:
                                ComboBox cbSiNo = (ComboBox)field[0];
                                cbSiNo.CreateControl();
                                cbSiNo.SelectedValue = string.IsNullOrEmpty(cf.v_DefaultText) ? "-1" : cf.v_DefaultText;
                                break;
                            case ControlType.UcFileUpload:
                                break;
                                
                            // DAVID Revisar para IDA y VUELTA de Antecedentes
                            case ControlType.Lista:
                                ComboBox cbList = (ComboBox)field[0];
                                cbList.CreateControl();
                                //if (cbList.Name == Constants.AUDIOMETRIA_ANTECEDENTES_SORDERA)
                                //{
                                //    var ent = new ServiceBL().GetPersonMedicalHistoryForESOBuildMenu(_personId, Constants.SORDERA);
                                //    var selectedValue = string.Empty;

                                //    if (ent != null)
                                //    {
                                //        if (ent.i_AnswerId == (int)SiNo.NO)
                                //        {
                                //            selectedValue = ((int)SiNoNoDefine.No).ToString();
                                //        }
                                //        else if (ent.i_AnswerId == (int)SiNo.SI)
                                //        {
                                //            selectedValue = ((int)SiNoNoDefine.Si).ToString();
                                //        }
                                //        else if (ent.i_AnswerId == (int)SiNo.NONE)
                                //        {
                                //            selectedValue = ((int)SiNoNoDefine.NoDefine).ToString();
                                //        }
                                //        else
                                //        {
                                //            selectedValue = "-1";
                                //        }
                                //        cbList.SelectedValue = selectedValue;
                                //    }
                                //    else
                                //    {
                                //        cbList.SelectedValue = string.IsNullOrEmpty(cf.v_DefaultText) ? "-1" : cf.v_DefaultText;
                                //    }

                                //}
                                //else
                                //{
                                //    cbList.SelectedValue = string.IsNullOrEmpty(cf.v_DefaultText) ? "-1" : cf.v_DefaultText;
                                //}
                                cbList.SelectedValue = string.IsNullOrEmpty(cf.v_DefaultText) ? "-1" : cf.v_DefaultText;
                                break;
                            case ControlType.UcOdontograma:
                                ((UserControls.ucOdontograma)field[0]).ClearValueControl();
                                break;
                            case ControlType.UcAudiometria:
                                ((UserControls.ucAudiometria)field[0]).ClearValueControl();
                                break;
                            default:
                                break;
                        }

                        #endregion
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #endregion

        #region Análisis de Diagnósticos

        private void Refrescar_Click(object sender, EventArgs e)
        {
            // Refrescar grilla de Total de DX
            GetTotalDiagnosticsForGridView();
        }

        private void SetCurrentRow()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(SetCurrentRow));
            }
            else
            {
                if (_rowIndex != null && grdTotalDiagnosticos.Rows.Count != 0)
                {
                    int rowCount = grdTotalDiagnosticos.Rows.Count;
                    int rows = rowCount - _rowIndex.Value;
                    int i = 0;

                    if (rows != 0)
                        i = _rowIndex.Value;
                    else
                        i = _rowIndex.Value - 1;

                    grdTotalDiagnosticos.Rows[i].Selected = true;
                    grdTotalDiagnosticos.ActiveRowScrollRegion.ScrollRowIntoView(grdTotalDiagnosticos.Rows[i]);
                    ////grdTotalDiagnosticos.ActiveRowScrollRegion.ScrollPosition = _rowIndex;
                }
            }
        }

        private void GetTotalDiagnosticsForGridView()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(GetTotalDiagnosticsForGridView));
            }
            else
            {
                OperationResult objOperationResult = new OperationResult();
                _tmpTotalDiagnosticByServiceIdList = _serviceBL.GetServiceComponentDisgnosticsByServiceId(ref objOperationResult, _serviceId);

                if (_tmpTotalDiagnosticByServiceIdList == null)
                    return;

                grdTotalDiagnosticos.DataSource = _tmpTotalDiagnosticByServiceIdList;
                lblRecordCountTotalDiagnosticos.Text = string.Format("Se encontraron {0} registros.", _tmpTotalDiagnosticByServiceIdList.Count());

                // Analizar el resultado de la operación
                if (objOperationResult.Success != 1)
                {
                    MessageBox.Show(Constants.GenericErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Seleccionar la fila que estaba marcada antes del refrescado
                SetCurrentRow();
            }

        }

        private void RefreshTmpRestrictionList(List<RestrictionList> prestrictions)
        {
            _tmpRestrictionList = new List<RestrictionList>();

            foreach (var item in prestrictions)
            {
                // Agregar restricciones a la Lista mas lo que ya tiene
                RestrictionList restrictionByDiagnostic = new RestrictionList();

                restrictionByDiagnostic.v_RestrictionByDiagnosticId = item.v_RestrictionByDiagnosticId;
                restrictionByDiagnostic.v_DiagnosticRepositoryId = item.v_DiagnosticRepositoryId;
                restrictionByDiagnostic.v_MasterRestrictionId = item.v_MasterRestrictionId;
                restrictionByDiagnostic.v_RestrictionName = item.v_RestrictionName;
                restrictionByDiagnostic.i_RecordStatus = item.i_RecordStatus;
                restrictionByDiagnostic.i_RecordType = item.i_RecordType;

                _tmpRestrictionList.Add(restrictionByDiagnostic);
            }
        }

        private void RefreshTmpRecomendationList(List<RecomendationList> precomendations)
        {
            _tmpRecomendationList = new List<RecomendationList>();

            foreach (var item in precomendations)
            {
                // Agregar restricciones a la Lista mas lo que ya tiene
                RecomendationList recomendation = new RecomendationList();

                recomendation.v_RecommendationId = item.v_RecommendationId;
                recomendation.v_DiagnosticRepositoryId = item.v_DiagnosticRepositoryId;
                recomendation.v_MasterRecommendationId = item.v_MasterRecommendationId;
                recomendation.v_RecommendationName = item.v_RecommendationName;
                recomendation.i_RecordStatus = item.i_RecordStatus;
                recomendation.i_RecordType = item.i_RecordType;

                _tmpRecomendationList.Add(recomendation);
            }
        }

        private void grdTotalDiagnosticos_ClickCell(object sender, Infragistics.Win.UltraWinGrid.ClickCellEventArgs e)
        {
            //_rowIndex = e.Cell.Row.Index;
            //EditTotalDiagnosticos(_rowIndex);
        }

        private void EditTotalDiagnosticos(int? rowIndex)
        {

            // Refrescar grilla de Total de DX
            GetTotalDiagnosticsForGridView();  // (pendiente)

            // Capturar ID para buscar en BD
            string diagnosticRepositoryId = grdTotalDiagnosticos.Rows[_rowIndex.Value].Cells["v_DiagnosticRepositoryId"].Value.ToString();
            //string diagnosticRepositoryId = e.Cell.Row.Cells["v_DiagnosticRepositoryId"].Value.ToString(); 

            OperationResult objOperationResult = new OperationResult();
            _tmpTotalDiagnostic = _serviceBL.GetServiceComponentTotalDiagnostics(ref objOperationResult, diagnosticRepositoryId);

            _componentIdConclusionesDX = _tmpTotalDiagnostic.v_ComponentId;

            #region Validaciones

            var califiFinalId = (FinalQualification)_tmpTotalDiagnostic.i_FinalQualificationId;

            if (califiFinalId == FinalQualification.Definitivo
                    || califiFinalId == FinalQualification.Presuntivo)
            {
                cbTipoDx.Enabled = true;
                cbEnviarAntecedentes.Enabled = true;
                dtpFechaVcto.Enabled = true;
            }
            else
            {
                cbTipoDx.Enabled = false;
                cbEnviarAntecedentes.Enabled = false;
                dtpFechaVcto.Enabled = false;
            }

            if (_tmpTotalDiagnostic.v_ComponentId == null)
                btnAgregarDX.Enabled = true;
            else
                btnAgregarDX.Enabled = false;

            //var automaticManual = (AutoManual)_tmpTotalDiagnostic.i_AutoManualId;
            //switch (automaticManual)
            //{
            //    case AutoManual.Automático:
            //        btnAgregarDX.Enabled = false;
            //        break;
            //    case AutoManual.Manual:
            //        btnAgregarDX.Enabled = true;
            //        break;
            //    default:
            //        break;
            //}

            #endregion

            //Mostar data
            lblDiagnostico.Text = _tmpTotalDiagnostic.v_DiseasesName;
            cbCalificacionFinal.SelectedValue = ((int)califiFinalId).ToString();
            cbTipoDx.SelectedValue = _tmpTotalDiagnostic.i_DiagnosticTypeId == null ? "-1" : _tmpTotalDiagnostic.i_DiagnosticTypeId.ToString();
            cbEnviarAntecedentes.SelectedValue = _tmpTotalDiagnostic.i_IsSentToAntecedent == null ? "-1" : _tmpTotalDiagnostic.i_IsSentToAntecedent.ToString();
            if (_tmpTotalDiagnostic.d_ExpirationDateDiagnostic != null)
                dtpFechaVcto.Value = (DateTime)_tmpTotalDiagnostic.d_ExpirationDateDiagnostic;

            // refrescar mis listas temporales [Restricciones / Recomendaciones] con data k viene de BD
            _tmpRestrictionList = _tmpTotalDiagnostic.Restrictions;
            _tmpRecomendationList = _tmpTotalDiagnostic.Recomendations;

            // Cargar grilla Restricciones
            grdRestricciones_AnalisisDiagnostico.DataSource = new RestrictionList();
            grdRestricciones_AnalisisDiagnostico.DataSource = _tmpRestrictionList;
            grdRestricciones_AnalisisDiagnostico.Refresh();
            lblRecordCountRestricciones_AnalisisDx.Text = string.Format("Se encontraron {0} registros.", _tmpRestrictionList.Count());

            // Cargar grilla Recomendaciones
            grdRecomendaciones_AnalisisDiagnostico.DataSource = new RecomendationList();
            grdRecomendaciones_AnalisisDiagnostico.DataSource = _tmpRecomendationList;
            grdRecomendaciones_AnalisisDiagnostico.Refresh();
            lblRecordCountRecomendaciones_AnalisisDx.Text = string.Format("Se encontraron {0} registros.", _tmpRecomendationList.Count());

            gbEdicionDiagnosticoTotal.Enabled = true;

        }

        private void EditTotalDiagnosticos()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(EditTotalDiagnosticos));
            }
            else
            {
                if (grdTotalDiagnosticos.Selected.Rows.Count == 0)
                    return;

                _rowIndex = grdTotalDiagnosticos.Selected.Rows[0].Cells["v_DiagnosticRepositoryId"].Row.Index;

                // Capturar ID para buscar en BD
                string diagnosticRepositoryId = grdTotalDiagnosticos.Selected.Rows[0].Cells["v_DiagnosticRepositoryId"].Value.ToString();

                OperationResult objOperationResult = new OperationResult();
                _tmpTotalDiagnostic = _serviceBL.GetServiceComponentTotalDiagnostics(ref objOperationResult, diagnosticRepositoryId);

                _componentIdConclusionesDX = _tmpTotalDiagnostic.v_ComponentId;

                var califiFinalId = (FinalQualification)_tmpTotalDiagnostic.i_FinalQualificationId;

                // Alejandro: Mostar data
                lblDiagnostico.Text = _tmpTotalDiagnostic.v_DiseasesName;
                cbCalificacionFinal.SelectedValue = califiFinalId == FinalQualification.SinCalificar ? ((int)FinalQualification.Presuntivo).ToString() : ((int)califiFinalId).ToString();
                cbTipoDx.SelectedValue = _tmpTotalDiagnostic.i_DiagnosticTypeId == null ? ((int)TipoDx.Enfermedad_crónica).ToString() : _tmpTotalDiagnostic.i_DiagnosticTypeId.ToString();
                cbEnviarAntecedentes.SelectedValue = _tmpTotalDiagnostic.i_IsSentToAntecedent == null ? ((int)SiNo.NO).ToString() : _tmpTotalDiagnostic.i_IsSentToAntecedent.ToString();

                // Alejandro: Set dx presuntivo x default si el estado es SinCalificar
                califiFinalId = califiFinalId == FinalQualification.SinCalificar ? FinalQualification.Presuntivo : califiFinalId;

                if (btnAceptarDX.Enabled)
                {
                    #region Validaciones

                    if (califiFinalId == FinalQualification.Definitivo
                            || califiFinalId == FinalQualification.Presuntivo)
                    {
                        cbTipoDx.Enabled = true;
                        cbEnviarAntecedentes.Enabled = true;
                        dtpFechaVcto.Enabled = true;
                    }
                    else
                    {
                        cbTipoDx.Enabled = false;
                        cbEnviarAntecedentes.Enabled = false;
                        dtpFechaVcto.Enabled = false;
                    }

                    if (_tmpTotalDiagnostic.v_ComponentId == null)
                        btnAgregarDX.Enabled = true;
                    else
                        btnAgregarDX.Enabled = false;

                    //var automaticManual = (AutoManual)_tmpTotalDiagnostic.i_AutoManualId;
                    //switch (automaticManual)
                    //{
                    //    case AutoManual.Automático:
                    //        btnAgregarDX.Enabled = false;
                    //        break;
                    //    case AutoManual.Manual:
                    //        btnAgregarDX.Enabled = true;
                    //        break;
                    //    default:
                    //        break;
                    //}

                    #endregion

                }

                if (_tmpTotalDiagnostic.d_ExpirationDateDiagnostic != null)
                {
                    //dtpFechaVcto.Checked = true;
                    dtpFechaVcto.Value = _tmpTotalDiagnostic.d_ExpirationDateDiagnostic.Value;
                }


                // refrescar mis listas temporales [Restricciones / Recomendaciones] con data k viene de BD
                _tmpRestrictionList = _tmpTotalDiagnostic.Restrictions;
                _tmpRecomendationList = _tmpTotalDiagnostic.Recomendations;

                // Cargar grilla Restricciones
                grdRestricciones_AnalisisDiagnostico.DataSource = new RestrictionList();
                grdRestricciones_AnalisisDiagnostico.DataSource = _tmpRestrictionList;
                grdRestricciones_AnalisisDiagnostico.Refresh();
                lblRecordCountRestricciones_AnalisisDx.Text = string.Format("Se encontraron {0} registros.", _tmpRestrictionList.Count());

                // Cargar grilla Recomendaciones
                grdRecomendaciones_AnalisisDiagnostico.DataSource = new RecomendationList();
                grdRecomendaciones_AnalisisDiagnostico.DataSource = _tmpRecomendationList;
                grdRecomendaciones_AnalisisDiagnostico.Refresh();
                lblRecordCountRecomendaciones_AnalisisDx.Text = string.Format("Se encontraron {0} registros.", _tmpRecomendationList.Count());

                gbEdicionDiagnosticoTotal.Enabled = true;

            }

        }

        private void btnAgregarRestriccion_Analisis_Click(object sender, EventArgs e)
        {
            var frm = new frmMasterRecommendationRestricction("Restricciones", (int)Typifying.Restricciones, ModeOperation.Total);
            frm.ShowDialog();

            if (_tmpRestrictionList == null)
            {
                _tmpRestrictionList = new List<RestrictionList>();
            }

            var restrictionId = frm._masterRecommendationRestricctionId;
            var restrictionName = frm._masterRecommendationRestricctionName;

            if (restrictionId != null && restrictionName != null)
            {
                var restriction = _tmpRestrictionList.Find(p => p.v_MasterRestrictionId == restrictionId);

                if (restriction == null)   // agregar con normalidad [insert]  a la bolsa  
                {
                    // Agregar restricciones a la Lista
                    RestrictionList restrictionByDiagnostic = new RestrictionList();

                    restrictionByDiagnostic.v_RestrictionByDiagnosticId = Guid.NewGuid().ToString();
                    restrictionByDiagnostic.v_DiagnosticRepositoryId = Guid.NewGuid().ToString();
                    restrictionByDiagnostic.v_MasterRestrictionId = restrictionId;
                    restrictionByDiagnostic.v_ServiceId = _serviceId;
                    restrictionByDiagnostic.v_ComponentId = _tmpTotalDiagnostic.v_ComponentId;
                    restrictionByDiagnostic.v_RestrictionName = restrictionName;
                    restrictionByDiagnostic.i_RecordStatus = (int)RecordStatus.Agregado;
                    restrictionByDiagnostic.i_RecordType = (int)RecordType.Temporal;

                    _tmpRestrictionList.Add(restrictionByDiagnostic);

                }
                else    // La restriccion ya esta agregado en la bolsa hay que actualizar su estado
                {
                    if (restriction.i_RecordStatus == (int)RecordStatus.EliminadoLogico)
                    {
                        if (restriction.i_RecordType == (int)RecordType.NoTemporal)   // El registro Tiene in ID de BD
                        {
                            restriction.v_MasterRestrictionId = restrictionId;
                            restriction.v_RestrictionName = restrictionName;
                            restriction.i_RecordStatus = (int)RecordStatus.Grabado;
                        }
                        else if (restriction.i_RecordType == (int)RecordType.Temporal)   // El registro tiene un ID temporal [GUID]
                        {
                            restriction.v_MasterRestrictionId = restrictionId;
                            restriction.v_RestrictionName = restrictionName;
                            restriction.i_RecordStatus = (int)RecordStatus.Agregado;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Por favor seleccione otra Restriccón. ya existe", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                }
            }

            var dataList = _tmpRestrictionList.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);

            // Cargar grilla
            grdRestricciones_AnalisisDiagnostico.DataSource = new RestrictionList();
            grdRestricciones_AnalisisDiagnostico.DataSource = dataList;
            grdRestricciones_AnalisisDiagnostico.Refresh();
            lblRecordCountRestricciones_AnalisisDx.Text = string.Format("Se encontraron {0} registros.", dataList.Count());

        }

        private void btnAgregarRecomendaciones_AnalisisDx_Click(object sender, EventArgs e)
        {
            var frm = new frmMasterRecommendationRestricction("Recomendaciones", (int)Typifying.Recomendaciones, ModeOperation.Total);
            frm.ShowDialog();

            if (_tmpRecomendationList == null)
            {
                _tmpRecomendationList = new List<RecomendationList>();
            }

            var recomendationId = frm._masterRecommendationRestricctionId;
            var recommendationName = frm._masterRecommendationRestricctionName;

            if (recomendationId != null && recommendationName != null)
            {
                var recomendation = _tmpRecomendationList.Find(p => p.v_MasterRecommendationId == recomendationId);

                if (recomendation == null)   // agregar con normalidad [insert]  a la bolsa  
                {
                    // Agregar restricciones a la Lista
                    RecomendationList recomendationList = new RecomendationList();

                    recomendationList.v_RecommendationId = Guid.NewGuid().ToString();
                    recomendationList.v_DiagnosticRepositoryId = Guid.NewGuid().ToString();
                    recomendationList.v_MasterRecommendationId = recomendationId;
                    recomendationList.v_ServiceId = _serviceId;
                    recomendationList.v_ComponentId = _tmpTotalDiagnostic.v_ComponentId;
                    recomendationList.v_RecommendationName = recommendationName;
                    recomendationList.i_RecordStatus = (int)RecordStatus.Agregado;
                    recomendationList.i_RecordType = (int)RecordType.Temporal;

                    _tmpRecomendationList.Add(recomendationList);
                }
                else    // La restriccion ya esta agregado en la bolsa hay que actualizar su estado
                {
                    if (recomendation.i_RecordStatus == (int)RecordStatus.EliminadoLogico)
                    {
                        if (recomendation.i_RecordType == (int)RecordType.NoTemporal)   // El registro Tiene in ID de BD
                        {
                            recomendation.v_MasterRecommendationId = recomendationId;
                            recomendation.v_RecommendationName = recommendationName;
                            recomendation.i_RecordStatus = (int)RecordStatus.Grabado;
                        }
                        else if (recomendation.i_RecordType == (int)RecordType.Temporal)   // El registro tiene un ID temporal [GUID]
                        {
                            recomendation.v_MasterRecommendationId = recomendationId;
                            recomendation.v_RecommendationName = recommendationName;
                            recomendation.i_RecordStatus = (int)RecordStatus.Agregado;
                        }

                    }
                    else
                    {
                        MessageBox.Show("Por favor seleccione otra Recomendación. ya existe", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                }
            }

            var dataList = _tmpRecomendationList.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);

            // Cargar grilla
            grdRecomendaciones_AnalisisDiagnostico.DataSource = new RecomendationList();
            grdRecomendaciones_AnalisisDiagnostico.DataSource = dataList;
            grdRecomendaciones_AnalisisDiagnostico.Refresh();
            lblRecordCountRecomendaciones_AnalisisDx.Text = string.Format("Se encontraron {0} registros.", dataList.Count());

        }

        private void btnRemoverRestriccion_Analisis_Click(object sender, EventArgs e)
        {
            if (grdRestricciones_AnalisisDiagnostico.Selected.Rows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un registro.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult Result = MessageBox.Show("¿Está seguro de eliminar este registro?:", "ADVERTENCIA!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (Result == DialogResult.Yes)
            {
                // Delete the item

                // Capturar id desde la grilla de restricciones
                var restrictionByDiagnosticId = grdRestricciones_AnalisisDiagnostico.Selected.Rows[0].Cells["v_RestrictionByDiagnosticId"].Value.ToString();

                // Buscar registro para remover
                var findResult = _tmpRestrictionList.Find(p => p.v_RestrictionByDiagnosticId == restrictionByDiagnosticId);
                // Borrado logico
                findResult.i_RecordStatus = (int)RecordStatus.EliminadoLogico;

                var dataList = _tmpRestrictionList.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);

                grdRestricciones_AnalisisDiagnostico.DataSource = new RestrictionList();
                grdRestricciones_AnalisisDiagnostico.DataSource = dataList;
                grdRestricciones_AnalisisDiagnostico.Refresh();
                lblRecordCountRestricciones_AnalisisDx.Text = string.Format("Se encontraron {0} registros.", dataList.Count());
            }
        }

        private void btnRemoverRecomendacion_AnalisisDx_Click(object sender, EventArgs e)
        {
            if (grdRecomendaciones_AnalisisDiagnostico.Selected.Rows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un registro.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult Result = MessageBox.Show("¿Está seguro de eliminar este registro?:", "ADVERTENCIA!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (Result == DialogResult.Yes)
            {
                // Delete the item

                // Capturar id desde la grilla de restricciones
                var recomendationId = grdRecomendaciones_AnalisisDiagnostico.Selected.Rows[0].Cells["v_RecommendationId"].Value.ToString();

                // Buscar registro para remover
                var findResult = _tmpRecomendationList.Find(p => p.v_RecommendationId == recomendationId);

                findResult.i_RecordStatus = (int)RecordStatus.EliminadoLogico;
                var dataList = _tmpRecomendationList.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);

                grdRecomendaciones_AnalisisDiagnostico.DataSource = new RecomendationList();
                grdRecomendaciones_AnalisisDiagnostico.DataSource = dataList;
                grdRecomendaciones_AnalisisDiagnostico.Refresh();
                lblRecordCountRecomendaciones_AnalisisDx.Text = string.Format("Se encontraron {0} registros.", dataList.Count());
            }

        }

        private void GuardarDX()
        {
            DialogResult Result = MessageBox.Show("¿Está seguro de grabar este registro?:", "CONFIRMACIÓN!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (Result == DialogResult.Yes)
            {

                OperationResult objOperationResult = new OperationResult();

                // Grabar Dx por examen componente mas sus restricciones
                if (_diagnosticId != null)
                    _tmpTotalDiagnostic.v_DiseasesId = _diagnosticId;
                if (cbCalificacionFinal.SelectedValue.ToString() != "-1")
                    _tmpTotalDiagnostic.i_FinalQualificationId = int.Parse(cbCalificacionFinal.SelectedValue.ToString());
                if (cbTipoDx.SelectedValue.ToString() != "-1")
                    _tmpTotalDiagnostic.i_DiagnosticTypeId = int.Parse(cbTipoDx.SelectedValue.ToString());
                if (cbEnviarAntecedentes.SelectedValue.ToString() != "-1")
                    _tmpTotalDiagnostic.i_IsSentToAntecedent = int.Parse(cbEnviarAntecedentes.SelectedValue.ToString());

                _tmpTotalDiagnostic.d_ExpirationDateDiagnostic = dtpFechaVcto.Checked ? dtpFechaVcto.Value.Date : (DateTime?)null;

                _tmpTotalDiagnostic.Restrictions = _tmpRestrictionList;
                _tmpTotalDiagnostic.Recomendations = _tmpRecomendationList;

                #region UTILIZAR FIRMA (Suplantar profesional)
                              
                if (chkUtilizaFirmaControlAuditoria.Checked)
                {
                    var frm = new Popups.frmSelectSignature();
                    frm.ShowDialog();

                    if (frm.DialogResult != System.Windows.Forms.DialogResult.Cancel)
                    {
                        if (frm.i_SystemUserSuplantadorId != null)
                        {
                            Globals.ClientSession.i_SystemUserId = (int)frm.i_SystemUserSuplantadorId;
                        }
                        else
                        {
                            Globals.ClientSession.i_SystemUserId = Globals.ClientSession.i_SystemUserCopyId;
                        }
                    }
                }
                else
                {
                    Globals.ClientSession.i_SystemUserId = Globals.ClientSession.i_SystemUserCopyId;
                }

                #endregion

                _serviceBL.UpdateTotalDiagnostic(ref objOperationResult,
                                                 _tmpTotalDiagnostic,
                                                 _serviceId,
                                                 Globals.ClientSession.GetAsList());


                InitializeData();
                //EditTotalDiagnosticos(_rowIndex);
                EditTotalDiagnosticos();
                // Refrescar grilla de Total de DX
                GetTotalDiagnosticsForGridView();
                ConclusionesyTratamiento_LoadAllGrid();

                // Analizar el resultado de la operación
                if (objOperationResult.Success != 1)
                {
                    MessageBox.Show(Constants.GenericErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    #region Mensaje de información de guardado

                    MessageBox.Show("se guardó correctamente.", "CORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    #endregion
                }
            }
        }

        private void btnAceptarDX_Click(object sender, EventArgs e)
        {

            var califiFinalId = (FinalQualification)int.Parse(cbCalificacionFinal.SelectedValue.ToString());

            if (califiFinalId == FinalQualification.Descartado || califiFinalId == FinalQualification.SinCalificar)
            {
                GuardarDX();
            }
            else
            {
                if (uvAnalisisDx.Validate(true, false).IsValid)
                {

                    GuardarDX();

                }
                else
                {
                    MessageBox.Show("Por favor corrija la información ingresada. Vea los indicadores de error.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }

        private void btnAgregarTotalDiagnostico_Click(object sender, EventArgs e)
        {
            var frm = new Popups.frmAddTotalDiagnostic();
            //frm._componentId = _componentId;
            frm._serviceId = _serviceId;

            if (_tmpTotalDiagnosticList != null)
            {
                frm._tmpTotalDiagnosticList = _tmpTotalDiagnosticByServiceIdList;
            }

            frm.ShowDialog();

            if (frm.DialogResult == DialogResult.Cancel)
                return;

            // Refrescar grilla de Total de DX
            GetTotalDiagnosticsForGridView();
            ConclusionesyTratamiento_LoadAllGrid();

        }

        private void grdTotalDiagnosticos_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            var caliFinal = (FinalQualification)e.Row.Cells["i_FinalQualificationId"].Value;
            var dx = e.Row.Cells["v_DiseasesId"].Value.ToString();

            switch (caliFinal)
            {
                case FinalQualification.SinCalificar:

                    if (dx != Constants.EXAMEN_DE_SALUD_SIN_ALTERACION)
                    {
                        e.Row.Appearance.BackColor = Color.Pink;
                        e.Row.Appearance.BackColor2 = Color.Pink;
                    }
                    else
                    {
                        e.Row.Appearance.BackColor = Color.White;
                        e.Row.Appearance.BackColor2 = Color.White;
                    }

                    break;
                case FinalQualification.Definitivo:
                    e.Row.Appearance.BackColor = Color.LawnGreen;
                    e.Row.Appearance.BackColor2 = Color.LawnGreen;
                    break;
                case FinalQualification.Presuntivo:
                    e.Row.Appearance.BackColor = Color.LawnGreen;
                    e.Row.Appearance.BackColor2 = Color.LawnGreen;
                    break;
                case FinalQualification.Descartado:
                    e.Row.Appearance.BackColor = Color.DarkGray;
                    e.Row.Appearance.BackColor2 = Color.DarkGray;
                    break;
                default:
                    break;
            }

            //Y doy el efecto degradado vertical
            e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;


        }

        private void grdTotalDiagnosticos_KeyDown(object sender, KeyEventArgs e)
        {
            _currentDownKey = e.KeyData;

            if (e.KeyData == Keys.Right)
            {
                grdTotalDiagnosticos.ActiveColScrollRegion.Scroll(ColScrollAction.Right);
            }
            else if (e.KeyData == Keys.Left)
            {
                grdTotalDiagnosticos.ActiveColScrollRegion.Scroll(ColScrollAction.Left);
            }
        }

        private void grdTotalDiagnosticos_KeyUp(object sender, KeyEventArgs e)
        {
            _currentDownKey = Keys.None;
        }

        private void grdTotalDiagnosticos_BeforePerformAction(object sender, BeforeUltraGridPerformActionEventArgs e)
        {
            if ((e.UltraGridAction == UltraGridAction.NextRow && _currentDownKey == Keys.Right)
                    || (e.UltraGridAction == UltraGridAction.PrevRow && _currentDownKey == Keys.Left))
            {
                e.Cancel = true;
            }
        }

        private void btnRemoverTotalDiagnostico_Click(object sender, EventArgs e)
        {
            if (grdTotalDiagnosticos.Selected.Rows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un registro.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OperationResult objOperationResult = new OperationResult();
            // Obtener los IDs de la fila seleccionada

            DialogResult Result = MessageBox.Show("¿Está seguro de eliminar este registro?:" + System.Environment.NewLine + objOperationResult.ExceptionMessage, "ADVERTENCIA!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (Result == DialogResult.Yes)
            {
                // Delete the item
                string diagnosticRepositoryId = grdTotalDiagnosticos.Selected.Rows[0].Cells["v_DiagnosticRepositoryId"].Value.ToString();
                _serviceBL.DeleteTotalDiagnostic(ref objOperationResult, diagnosticRepositoryId, Globals.ClientSession.GetAsList());
                // Analizar el resultado de la operación
                if (objOperationResult.Success != 1)
                {
                    MessageBox.Show(Constants.GenericErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                GetTotalDiagnosticsForGridView();
                ConclusionesyTratamiento_LoadAllGrid();

                // Limpiar grillas despues de borrar y la grilla quede vacia
                if (grdTotalDiagnosticos.Rows.Count == 0)
                {
                    gbEdicionDiagnosticoTotal.Enabled = false;
                    lblDiagnostico.Text = string.Empty;
                    cbCalificacionFinal.SelectedValue = "-1";
                    cbEnviarAntecedentes.SelectedValue = "-1";
                    cbTipoDx.SelectedValue = "-1";
                    dtpFechaVcto.Value = DateTime.Now;
                }

            }

        }

        private void cbCalificacionFinal_TextChanged(object sender, EventArgs e)
        {
            if (btnAceptarDX.Enabled)
            {
                if (grdTotalDiagnosticos.Selected.Rows.Count != 0)
                {
                    var califiFinalId = (FinalQualification)int.Parse(cbCalificacionFinal.SelectedValue.ToString());

                    if (califiFinalId == FinalQualification.Definitivo
                            || califiFinalId == FinalQualification.Presuntivo)
                    {
                        cbTipoDx.Enabled = true;
                        cbEnviarAntecedentes.Enabled = true;
                        dtpFechaVcto.Enabled = true;

                        // Alejandro Set value x default

                        cbTipoDx.SelectedValue = _tmpTotalDiagnostic.i_DiagnosticTypeId == null ? ((int)TipoDx.Enfermedad_crónica).ToString() : _tmpTotalDiagnostic.i_DiagnosticTypeId.ToString();
                        cbEnviarAntecedentes.SelectedValue = _tmpTotalDiagnostic.i_IsSentToAntecedent == null ? ((int)SiNo.NO).ToString() : _tmpTotalDiagnostic.i_IsSentToAntecedent.ToString();

                    }
                    else
                    {
                        cbTipoDx.SelectedValue = "-1";
                        cbEnviarAntecedentes.SelectedValue = "-1";

                        cbTipoDx.Enabled = false;
                        cbEnviarAntecedentes.Enabled = false;
                        dtpFechaVcto.Enabled = false;
                    }
                }
            }
        }

        private void btnAgregarDX_Click(object sender, EventArgs e)
        {
            DiseasesList returnDiseasesList = new DiseasesList();
            frmDiseases frm = new frmDiseases();

            frm.ShowDialog();
            returnDiseasesList = frm._objDiseasesList;

            if (returnDiseasesList.v_DiseasesId != null)
            {
                lblDiagnostico.Text = returnDiseasesList.v_Name + " / " + returnDiseasesList.v_CIE10Id;
                _diagnosticId = returnDiseasesList.v_DiseasesId;
            }
        }

        private void grdTotalDiagnosticos_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            EditTotalDiagnosticos();

            bool flag = false;

            if (btnAceptarDX.Enabled)
            {
                if (((UltraGrid)sender).Selected.Rows.Count != 0)
                {
                    #region Validación

                    int? autoManual = (int?)((UltraGrid)sender).Selected.Rows[0].Cells["i_AutoManualId"].Value;

                    if (autoManual != null)
                    {
                        if (autoManual == (int)AutoManual.Manual)
                        {
                            //btnRemoverTotalDiagnostico.Enabled = true;
                            flag = true;
                        }
                        else if (autoManual == (int)AutoManual.Automático)
                        {
                            //btnRemoverTotalDiagnostico.Enabled = false;
                            flag = false;
                        }

                    }

                    #endregion
                }
            }

            btnRemoverTotalDiagnostico.Enabled = (grdTotalDiagnosticos.Selected.Rows.Count > 0 && _removerTotalDiagnostico && flag);
        }

        private void btnRefrescarTotalDiagnostico_Click(object sender, EventArgs e)
        {
            // Refrescar grilla de Total de DX
            GetTotalDiagnosticsForGridView();
        }

        #endregion

        #region Conclusiones

        private void GetConclusionesDiagnosticasForGridView()
        {
            OperationResult objOperationResult = new OperationResult();
            _tmpTotalConclusionesDxByServiceIdList = _serviceBL.GetServiceComponentConclusionesDxServiceId(ref objOperationResult, _serviceId);

            if (_tmpTotalConclusionesDxByServiceIdList == null)
                return;

            grdConclusionesDiagnosticas.DataSource = _tmpTotalConclusionesDxByServiceIdList;
            lblRecordCountConclusionesDiagnosticas.Text = string.Format("Se encontraron {0} registros.", _tmpTotalConclusionesDxByServiceIdList.Count());

            // Analizar el resultado de la operación
            if (objOperationResult.Success != 1)
            {
                MessageBox.Show(Constants.GenericErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (_rowIndexConclucionesDX != null)
            {
                if (grdConclusionesDiagnosticas.Rows.Count != 0)
                {
                    // Seleccionar la fila
                    grdConclusionesDiagnosticas.Rows[_rowIndexConclucionesDX.Value].Selected = true;
                    grdConclusionesDiagnosticas.ActiveRowScrollRegion.ScrollRowIntoView(grdConclusionesDiagnosticas.Rows[_rowIndexConclucionesDX.Value]);
                    //grdTotalDiagnosticos.ActiveRowScrollRegion.ScrollPosition = _rowIndex;
                }
            }

        }

        private void btnAgregarRestriccion_ConclusionesTratamiento_Click(object sender, EventArgs e)
        {
            var frm = new frmMasterRecommendationRestricction("Restricciones", (int)Typifying.Restricciones, ModeOperation.Total);
            frm.ShowDialog();

            if (_tmpRestrictionListConclusiones == null)
                _tmpRestrictionListConclusiones = new List<RestrictionList>();

            var restrictionId = frm._masterRecommendationRestricctionId;
            var restrictionName = frm._masterRecommendationRestricctionName;

            if (restrictionId != null && restrictionName != null)
            {
                var restriction = _tmpRestrictionListConclusiones.Find(p => p.v_MasterRestrictionId == restrictionId);

                if (restriction == null)   // agregar con normalidad [insert]  a la bolsa  
                {
                    // Agregar restricciones a la Lista
                    RestrictionList restrictionByDiagnostic = new RestrictionList();

                    restrictionByDiagnostic.i_ItemId = _tmpRestrictionListConclusiones.Count + 1;
                    restrictionByDiagnostic.v_ServiceId = _serviceId;
                    restrictionByDiagnostic.v_MasterRestrictionId = restrictionId;
                    restrictionByDiagnostic.v_RestrictionName = restrictionName;
                    restrictionByDiagnostic.i_RecordStatus = (int)RecordStatus.Agregado;
                    restrictionByDiagnostic.i_RecordType = (int)RecordType.Temporal;

                    _tmpRestrictionListConclusiones.Add(restrictionByDiagnostic);
                }
                else    // La restriccion ya esta agregado en la bolsa hay que actualizar su estado
                {
                    if (restriction.i_RecordStatus == (int)RecordStatus.EliminadoLogico)
                    {
                        if (restriction.i_RecordType == (int)RecordType.NoTemporal)   // El registro Tiene in ID de BD
                        {
                            restriction.i_RecordStatus = (int)RecordStatus.Grabado;
                        }
                        else if (restriction.i_RecordType == (int)RecordType.Temporal)   // El registro tiene un ID temporal [GUID]
                        {
                            restriction.i_RecordStatus = (int)RecordStatus.Agregado;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Por favor seleccione otra Restriccón. ya existe", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            var dataList = _tmpRestrictionListConclusiones.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);

            // Cargar grilla
            grdRestricciones_Conclusiones.DataSource = new RestrictionList();
            grdRestricciones_Conclusiones.DataSource = dataList;
            grdRestricciones_Conclusiones.Refresh();
            lblRecordCountRestricciones_Conclusiones.Text = string.Format("Se encontraron {0} registros.", dataList.Count());

        }

        private void btnRemoverRestricciones_ConclusionesTratamiento_Click(object sender, EventArgs e)
        {
            if (grdRestricciones_Conclusiones.Selected.Rows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un registro.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult Result = MessageBox.Show("¿Está seguro de eliminar este registro?:", "CONFIRMA LA ELIMINACIÓN!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (Result == DialogResult.Yes)
            {
                // Capturar id desde la grilla 
                var restrictionId = grdRestricciones_Conclusiones.Selected.Rows[0].Cells["v_MasterRestrictionId"].Value.ToString();

                // Buscar registro para remover
                var findResult = _tmpRestrictionListConclusiones.Find(p => p.v_MasterRestrictionId == restrictionId);
                // Borrado logico
                findResult.i_RecordStatus = (int)RecordStatus.EliminadoLogico;

                var dataList = _tmpRestrictionListConclusiones.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);

                grdRestricciones_Conclusiones.DataSource = new RestrictionList();
                grdRestricciones_Conclusiones.DataSource = dataList;
                grdRestricciones_Conclusiones.Refresh();
                lblRecordCountRestricciones_Conclusiones.Text = string.Format("Se encontraron {0} registros.", dataList.Count());
            }
        }

        private void btnGuardarConclusiones_Click(object sender, EventArgs e)
        {
            DialogResult Result = MessageBox.Show("¿Está seguro de grabar este registro?:", "CONFIRMACIÓN!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (Result == DialogResult.Yes)
            {

                OperationResult objOperationResult = new OperationResult();

                var serviceDTO = new serviceDto();

                //int? hazRestriction = null;

                // Datos de Servicio
                serviceDTO.v_ServiceId = _serviceId;
                //serviceDTO.i_HasRestrictionId = hazRestriction;

                // datos de cabecera del Servicio
                serviceDTO.i_AptitudeStatusId = int.Parse(cbAptitudEso.SelectedValue.ToString());

                if (txtFecVctoGlobal.Text != dtpFecVctoGlobal.Value.ToShortTimeString())
                {
                    DialogResult Result2 = MessageBox.Show("¿Está seguro de Actualizar la Fecha de Vencimiento?:", "CONFIRMACIÓN!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (Result2 == DialogResult.Yes)
                    { serviceDTO.d_GlobalExpirationDate = dtpFecVctoGlobal.Value; }
                    else
                    { serviceDTO.d_GlobalExpirationDate = DateTime.Parse(txtFecVctoGlobal.Text); ; }
                }

                #region UTILIZAR FIRMA (Suplantar profesional)

                if (chkUtilizaFirmaAptitud.Checked)
                {
                    var frm = new Popups.frmSelectSignature();
                    frm.ShowDialog();

                    if (frm.DialogResult != System.Windows.Forms.DialogResult.Cancel)
                    {
                        if (frm.i_SystemUserSuplantadorId != null)
                        {
                            Globals.ClientSession.i_SystemUserId = (int)frm.i_SystemUserSuplantadorId;
                        }
                        else
                        {
                            Globals.ClientSession.i_SystemUserId = Globals.ClientSession.i_SystemUserCopyId;
                        }
                    }
                }
                else
                {
                    Globals.ClientSession.i_SystemUserId = Globals.ClientSession.i_SystemUserCopyId;
                }

                #endregion

                _serviceBL.AddConclusiones(ref objOperationResult,
                                           _tmpRestrictionListConclusiones,
                                           _tmpRecomendationConclusionesList,
                                           serviceDTO,
                                           null,
                                           Globals.ClientSession.GetAsList());


                // Refrescar todas las grillas
                InitializeData();
                ConclusionesyTratamiento_LoadAllGrid();

                // Analizar el resultado de la operación
                if (objOperationResult.Success != 1)
                {
                    MessageBox.Show(Constants.GenericErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    #region Mensaje de información de guardado

                    ////DialogResult Result1 = MessageBox.Show("se guardó correctamente.¿Desea generar los reportes automáticamente?", "CORRECTO", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    ////if (Result1 == DialogResult.Yes)
                    ////{
                    ////    Generar Reportes
                    ////    string ruta = Common.Utils.GetApplicationConfigValue("rutaReportes").ToString();
                    ////    using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
                    ////    {
                    ////        CrearReportesCrystal(serviceDTO.v_ServiceId, DevolverListaReportes(serviceDTO.v_ServiceId), null, ruta);
                    ////    };

                    ////    var x = _filesNameToMerge.ToList();
                    ////    _mergeExPDF.FilesName = x;
                    ////    _mergeExPDF.DestinationFile = ruta + _serviceId + ".pdf"; ;
                    ////    _mergeExPDF.Execute();
                    ////    _mergeExPDF.RunFile();

                    ////    this.Close();
                    ////}
                    DialogResult Result1 = MessageBox.Show("Se guardó correctamente.", "CORRECTO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (Result1 == DialogResult.OK)
                    {
                        //Generar Reportes
                        string ruta = Common.Utils.GetApplicationConfigValue("rutaReportes").ToString();
                        using (new LoadingClass.PleaseWait(this.Location, "Generando..."))
                        {
                            CrearReportesCrystal(serviceDTO.v_ServiceId, serviceDTO.v_PersonId, DevolverListaCAPs(serviceDTO.v_ServiceId), null, ruta, "CAP");
                        };

                        //this.Close();
                    }

                    #endregion
                }

               
            }

            var ImprimirReporteCovid = MessageBox.Show("¿Desea generar reporte covid-19?:", "CONFIRMACIÓN!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (ImprimirReporteCovid == DialogResult.Yes)
            {
                var serviceComponents = _serviceBL.GetServiceComponentsForManagementReport(_serviceId).Select(s => s.v_ComponentId).ToList();
                string ruta = Common.Utils.GetApplicationConfigValue("rutaReportes").ToString();

                CrearReportesCrystal(_serviceId, _personId, serviceComponents, null, ruta, "CAP");

                var x = _filesNameToMerge.ToList();
                _mergeExPDF.FilesName = x;
                _mergeExPDF.DestinationFile = ruta + _serviceId + ".pdf"; ;
                _mergeExPDF.Execute();

                EnviarPdfTrabajador();
                //_mergeExPDF.RunFile();
            }

        }

        List<string> DevolverListaCAPs(string pstrServiceId)
        {
            List<ServiceComponentList> serviceComponents = new List<ServiceComponentList>();
            List<ServiceComponentList> ListaOrdenada = new List<ServiceComponentList>();

            //serviceComponents = _serviceBL.GetServiceComponentsForManagementReport(pstrServiceId);

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

        //public void CrearReportesCrystal(string serviceId, List<string> reportesId, List<ServiceComponentList> ListaDosaje, string pstrRutaReportes)
        //{
        //    crConsolidatedReports rp = null;
        //    OperationResult objOperationResult = new OperationResult();
        //    MultimediaFileBL _multimediaFileBL = new MultimediaFileBL();
        //    rp = new Reports.crConsolidatedReports();
        //    _filesNameToMerge = new List<string>();
        //    foreach (var com in reportesId)
        //    {
        //        ChooseReport(com, serviceId, pstrRutaReportes);
        //    }

        //    var ListaPdf = _serviceBL.GetFilePdfsByServiceId(ref objOperationResult, _serviceId);
        //    if (ListaPdf.ToList().Count != 0)
        //    {
        //        int count = 1;
        //        foreach (var item in ListaPdf)
        //        {
        //            var multimediaFile = _multimediaFileBL.GetMultimediaFileById(ref objOperationResult, item.v_MultimediaFileId);
        //            var path = Application.StartupPath + @"\TempMerge\" + item.v_FileName + " " + count;
        //            File.WriteAllBytes(path, multimediaFile.ByteArrayFile);
        //            _filesNameToMerge.Add(path);

        //        }
        //    }
        //}

        public void CrearReportesCrystal(string serviceId, string personId, List<string> reportesId, List<ServiceComponentList> ListaDosaje, string pstrRutaReportes, string pstrbtn)
        {
            string ruta = Common.Utils.GetApplicationConfigValue("rutaReportes").ToString();

            crConsolidatedReports rp = null;
            OperationResult objOperationResult = new OperationResult();
            MultimediaFileBL _multimediaFileBL = new MultimediaFileBL();
            rp = new Reports.crConsolidatedReports();
            _filesNameToMerge = new List<string>();
            foreach (var com in reportesId)
            {
                ChooseReport(com, serviceId, pstrRutaReportes, personId);
            }

           
        }
        
        //private void ChooseReport(string componentId, string psrtServiceId, string pstrRutaReportes)
        //{
        //    DataSet dsGetRepo = null;
        //    DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
        //    OperationResult objOperationResult = new OperationResult();
        //    ReportDocument rp;

        //    switch (componentId)
        //    {
        //        case Constants.INFORME_CERTIFICADO_APTITUD:
        //            var INFORME_CERTIFICADO_APTITUD = new ServiceBL().GetAptitudeCertificate(ref objOperationResult, _serviceId);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_INFORME_CERTIFICADO_APTITUD = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(INFORME_CERTIFICADO_APTITUD);
        //            dt_INFORME_CERTIFICADO_APTITUD.TableName = "AptitudeCertificate";
        //            dsGetRepo.Tables.Add(dt_INFORME_CERTIFICADO_APTITUD);

        //            rp = new Reports.crOccupationalMedicalAptitudeCertificate();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_CERTIFICADO_APTITUD + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();

        //            break;

        //        case Constants.EXAMEN_FISICO_ID:
        //            var EXAMEN_FISICO_ID = new ServiceBL().ReportMusculoEsqueletico1(_serviceId, Constants.MUSCULO_ESQUELETICO_1_ID);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_EXAMEN_FISICO_ID = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(EXAMEN_FISICO_ID);
        //            dt_EXAMEN_FISICO_ID.TableName = "dtMusculoEsqueletico1";
        //            dsGetRepo.Tables.Add(dt_EXAMEN_FISICO_ID);

        //            rp = new Reports.crMusculoEsqueletico1();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            //objDiskOpt.DiskFileName = pstrRutaReportes + Constants.EXAMEN_FISICO_ID + ".pdf";
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.EXAMEN_FISICO_ID + "1.pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();

        //            var EXAMEN_FISICO_ID2 = new ServiceBL().ReportMusculoEsqueletico2(_serviceId, Constants.MÚSCULO_ESQUELÉTICO_2_ID);

        //            dsGetRepo = new DataSet();

        //            DataTable dt_EXAMEN_FISICO_ID2 = Sigesoft.Node.WinClient.BLL.Utils.ConvertToDatatable(EXAMEN_FISICO_ID2);
        //            dt_EXAMEN_FISICO_ID2.TableName = "dtMusculoEsqueletico2";
        //            dsGetRepo.Tables.Add(dt_EXAMEN_FISICO_ID2);

        //            rp = new Reports.crMusculoEsqueletico2();
        //            rp.SetDataSource(dsGetRepo);
        //            rp.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            rp.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //            objDiskOpt = new DiskFileDestinationOptions();
        //            //objDiskOpt.DiskFileName = pstrRutaReportes + Constants.EXAMEN_FISICO_ID + "2.pdf";
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.EXAMEN_FISICO_ID + "2.pdf";
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
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_HISTORIA_OCUPACIONAL + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            break;

        //        case Constants.ALTURA_7D_ID:
        //            var AscensoAlturas = new ServiceBL().ReportAscensoGrandesAlturas(_serviceId, Constants.ALTURA_7D_ID);
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
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ALTURA_7D_ID + ".pdf";
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
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ALTURA_FISICA_M_18 + ".pdf";
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
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ELECTROCARDIOGRAMA_ID + ".pdf";
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
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.PRUEBA_ESFUERZO_ID + ".pdf";
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
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ODONTOGRAMA_ID + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            rp.Close();

        //            break;

        //        case Constants.AUDIOMETRIA_ID:

        //            var serviceBL = new ServiceBL();
        //            DataSet dsAudiometria = new DataSet();

        //            var dxList = serviceBL.GetDiagnosticRepositoryByComponent(_serviceId, Constants.AUDIOMETRIA_ID);

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
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.AUDIOMETRIA_ID + ".pdf";
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
        //                objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + "AUDIOMETRIA_ID_HISTORIA" + ".pdf";
        //                _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //                rp.ExportOptions.DestinationOptions = objDiskOpt;
        //                rp.Export();
        //            }

        //            break;

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
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.RX_ID + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            break;


        //        case Constants.OIT_ID:
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
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_RADIOGRAFICO_OIT + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            break;


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
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ESPIROMETRIA_CUESTIONARIO_ID + ".pdf";
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
        //            objDiskOpt.DiskFileName = pstrRutaReportes + psrtServiceId + "-" + Constants.ESPIROMETRIA_ID + ".pdf";
        //            _filesNameToMerge.Add(objDiskOpt.DiskFileName);
        //            rp.ExportOptions.DestinationOptions = objDiskOpt;
        //            rp.Export();
        //            break;

        //        case Constants.INFORME_ANEXO_312:
        //            GenerateAnexo312(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_ANEXO_312)));
        //            _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_ANEXO_312)));
        //            break;
        //        //case Constants.INFORME_FICHA_MEDICA_TRABAJADOR:
        //        //    GenerateInformeMedicoTrabajador(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_FICHA_MEDICA_TRABAJADOR)));
        //        //    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_FICHA_MEDICA_TRABAJADOR)));
        //        //    break;
        //        case Constants.INFORME_ANEXO_7C:
        //            GenerateAnexo7C(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_ANEXO_7C)));
        //            _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_ANEXO_7C)));
        //            break;
        //        case Constants.INFORME_CLINICO:
        //            GenerateInformeExamenClinico(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_CLINICO)));
        //            _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_CLINICO)));
        //            break;
        //        case Constants.INFORME_LABORATORIO_CLINICO:
        //            GenerateLaboratorioReport(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_LABORATORIO_CLINICO)));
        //            _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_LABORATORIO_CLINICO)));
        //            break;
        //        default:
        //            break;
        //    }

        //}

        private void ChooseReport(string componentId, string psrtServiceId, string pstrRutaReportes, string pstrPersonId)
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
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();

                    //MERGE PDF COVID
                    var reportesCovid19 = _filesNameToMergeCovid19.ToList();
                    _mergeExPDF.FilesName = reportesCovid19;
                    _mergeExPDF.DestinationFile = rutaCovid + _serviceId + ".pdf"; ;
                    _mergeExPDF.Execute();
                    ////_mergeExPDF.RunFile();

                    break;

                case Constants.COVID_ID:
                    var COVID_ID = new ServiceBL().GetCovid(ref objOperationResult, _serviceId);

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
                    _filesNameToMerge.Add(objDiskOpt.DiskFileName);
                    rp.ExportOptions.DestinationOptions = objDiskOpt;
                    rp.Export();


                    break;
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

                    break;
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

                //case Constants.INFORME_ANEXO_312:
                //    GenerateAnexo312(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_ANEXO_312)), pstrPersonId, psrtServiceId);
                //    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_ANEXO_312)));
                //    break;
                //case Constants.INFORME_FICHA_MEDICA_TRABAJADOR:
                //    GenerateInformeMedicoTrabajador(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_FICHA_MEDICA_TRABAJADOR)), pstrPersonId, psrtServiceId);
                //    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_FICHA_MEDICA_TRABAJADOR)));
                //    break;
                //case Constants.INFORME_ANEXO_7C:
                //    GenerateAnexo7C(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_ANEXO_7C)), pstrPersonId, psrtServiceId);
                //    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_ANEXO_7C)));
                //    break;
                //case Constants.INFORME_CLINICO:
                //    GenerateInformeExamenClinico(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_CLINICO)), pstrPersonId, psrtServiceId);
                //    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_CLINICO)));
                //    break;
                //case Constants.INFORME_LABORATORIO_CLINICO:
                //    GenerateLaboratorioReport(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes, psrtServiceId + "-" + Constants.INFORME_LABORATORIO_CLINICO)), psrtServiceId);
                //    _filesNameToMerge.Add(string.Format("{0}.pdf", Path.Combine(pstrRutaReportes + psrtServiceId + "-" + Constants.INFORME_LABORATORIO_CLINICO)));
                //    break;
                default:
                    break;
            }

        }

        private void GenerateAnexo312(string pathFile)
        {
            var filiationData = _pacientBL.GetPacientReportEPS(_serviceId);
            var _listAtecedentesOcupacionales = _historyBL.GetHistoryReport(_personId);
            var _listaPatologicosFamiliares = _historyBL.GetFamilyMedicalAntecedentsReport(_personId);
            var _listMedicoPersonales = _historyBL.GetPersonMedicalHistoryReport(_personId);
            var _DataService = _serviceBL.GetServiceReport(_serviceId);
            var _listaHabitoNocivos = _historyBL.GetNoxiousHabitsReport(_personId);

            var Antropometria = _serviceBL.ValoresComponente(_serviceId, Constants.ANTROPOMETRIA_ID);
            var FuncionesVitales = _serviceBL.ValoresComponente(_serviceId, Constants.FUNCIONES_VITALES_ID);
            var ExamenFisico = _serviceBL.ValoresComponente(_serviceId, Constants.EXAMEN_FISICO_ID);
            var Oftalmologia = _serviceBL.ValoresComponente(_serviceId, Constants.OFTALMOLOGIA_ID);
            var Oftalmologia_UC = _serviceBL.ValoresComponentesUserControl(_serviceId, Constants.AGUDEZA_VISUAL);
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
            var VISIONESTEREOSCOPICA = _serviceBL.ValoresComponente(_serviceId, Constants.VISION_ESTEREOSCOPICA_ID);
            var VISIONCOLORES = _serviceBL.ValoresComponente(_serviceId, Constants.VISION_DE_COLORES_ID);
            var MedicalCenter = _serviceBL.GetInfoMedicalCenter();
            var _Restricciones = _serviceBL.GetServiceRestriccionByServiceId(_serviceId);
            FichaMedicaOcupacional312.CreateFichaMedicalOcupacional312Report(_DataService,
                        filiationData, _listAtecedentesOcupacionales, _listaPatologicosFamiliares,
                        _listMedicoPersonales, _listaHabitoNocivos, Antropometria, FuncionesVitales,
                        ExamenFisico, Oftalmologia, Oftalmologia_UC, Psicologia, OIT, RX, Laboratorio, Audiometria, Espirometria,
                        _DiagnosticRepository, _Recomendation, _Restricciones, _ExamenesServicio, MedicalCenter, VISIONESTEREOSCOPICA,VISIONCOLORES,
                        pathFile);
        }

        private void GenerateInformeMedicoTrabajador(string pathFile)
        {
            var _DataService = _serviceBL.GetServiceReport(_serviceId);
            var MedicalCenter = _serviceBL.GetInfoMedicalCenter();
            FichaMedicaTrabajadorHoracio.CreateFichaMedicaTrabajadorHoracio(_DataService, MedicalCenter, pathFile);
        }

        private void GenerateAnexo7C(string pathFile)
        {
            var _DataService = _serviceBL.GetServiceReport(_serviceId);
            var _listMedicoPersonales = _historyBL.GetPersonMedicalHistoryReport(_personId);
            var _listaPatologicosFamiliares = _historyBL.GetFamilyMedicalAntecedentsReport(_personId);
            var _Valores = _serviceBL.GetServiceComponentsReport(_serviceId);
            var _listaHabitoNocivos = _historyBL.GetNoxiousHabitsReport(_personId);
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
            var personMedicalHistory = _historyBL.GetPersonMedicalHistoryReport(_personId);
            var noxiousHabit = _historyBL.GetNoxiousHabitsReport(_personId);
            var familyMedicalAntecedent = _historyBL.GetFamilyMedicalAntecedentsReport(_personId);
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
                                            "",
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



        private void ConclusionesyTratamiento_LoadAllGrid()
        {
            OperationResult objOperationResult = new OperationResult();

            #region Conclusiones Diagnósticas

            GetConclusionesDiagnosticasForGridView();

            #endregion

            #region Recomendación

            _tmpRecomendationConclusionesList = _serviceBL.GetServiceRecommendationByServiceId(ref objOperationResult, _serviceId);

            if (_tmpRecomendationConclusionesList == null)
                return;

            grdRecomendaciones_Conclusiones.DataSource = _tmpRecomendationConclusionesList;
            lblRecordCountRecomendaciones_Conclusiones.Text = string.Format("Se encontraron {0} registros.",
                                                            _tmpRecomendationConclusionesList.Count());

            #endregion

            #region Restricciones

            _tmpRestrictionListConclusiones = _serviceBL.GetServiceRestrictionsForGridView(ref objOperationResult, _serviceId);

            if (_tmpRestrictionListConclusiones == null)
                return;

            grdRestricciones_Conclusiones.DataSource = _tmpRestrictionListConclusiones;
            lblRecordCountRestricciones_Conclusiones.Text = string.Format("Se encontraron {0} registros.", _tmpRestrictionListConclusiones.Count());

            #endregion

            // Analizar el resultado de la operación
            if (objOperationResult.Success != 1)
            {
                MessageBox.Show(Constants.GenericErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void grdConclusionesDiagnosticas_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            var caliFinal = (FinalQualification)e.Row.Cells["i_FinalQualificationId"].Value;

            switch (caliFinal)
            {
                case FinalQualification.SinCalificar:
                    e.Row.Appearance.BackColor = Color.Pink;
                    e.Row.Appearance.BackColor2 = Color.Pink;
                    break;
                case FinalQualification.Definitivo:
                    e.Row.Appearance.BackColor = Color.LawnGreen;
                    e.Row.Appearance.BackColor2 = Color.LawnGreen;
                    break;
                case FinalQualification.Presuntivo:
                    e.Row.Appearance.BackColor = Color.LawnGreen;
                    e.Row.Appearance.BackColor2 = Color.LawnGreen;
                    break;
                case FinalQualification.Descartado:
                    e.Row.Appearance.BackColor = Color.DarkGray;
                    e.Row.Appearance.BackColor2 = Color.DarkGray;
                    break;
                default:
                    break;
            }

            //Y doy el efecto degradado vertical
            e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
        }

        private void grdConclusionesDiagnosticas_ClickCell(object sender, ClickCellEventArgs e)
        {
            _rowIndexConclucionesDX = e.Cell.Row.Index;
            GetConclusionesDiagnosticasForGridView();
        }

        private void btnAgregarRecomendaciones_Conclusiones_Click(object sender, EventArgs e)
        {
            var frm = new frmMasterRecommendationRestricction("Recomendaciones", (int)Typifying.Recomendaciones, ModeOperation.Total);
            frm.ShowDialog();

            if (_tmpRecomendationConclusionesList == null)
                _tmpRecomendationConclusionesList = new List<RecomendationList>();

            var recomendationId = frm._masterRecommendationRestricctionId;
            var recommendationName = frm._masterRecommendationRestricctionName;

            if (recomendationId != null && recommendationName != null)
            {
                var recomendation = _tmpRecomendationConclusionesList.Find(p => p.v_MasterRecommendationId == recomendationId);

                if (recomendation == null)   // agregar con normalidad [insert]  a la bolsa  
                {
                    // Agregar restricciones a la Lista
                    RecomendationList recomendationList = new RecomendationList();

                    recomendationList.v_RecommendationId = Guid.NewGuid().ToString();
                    recomendationList.v_MasterRecommendationId = recomendationId;
                    recomendationList.v_ServiceId = _serviceId;
                    recomendationList.v_RecommendationName = recommendationName;
                    recomendationList.i_RecordStatus = (int)RecordStatus.Agregado;
                    recomendationList.i_RecordType = (int)RecordType.Temporal;

                    _tmpRecomendationConclusionesList.Add(recomendationList);
                }
                else    // La restriccion ya esta agregado en la bolsa hay que actualizar su estado
                {
                    if (recomendation.i_RecordStatus == (int)RecordStatus.EliminadoLogico)
                    {
                        if (recomendation.i_RecordType == (int)RecordType.NoTemporal)   // El registro Tiene in ID de BD
                        {
                            recomendation.v_MasterRecommendationId = recomendationId;
                            recomendation.v_RecommendationName = recommendationName;
                            recomendation.i_RecordStatus = (int)RecordStatus.Grabado;
                        }
                        else if (recomendation.i_RecordType == (int)RecordType.Temporal)   // El registro tiene un ID temporal [GUID]
                        {
                            recomendation.v_MasterRecommendationId = recomendationId;
                            recomendation.v_RecommendationName = recommendationName;
                            recomendation.i_RecordStatus = (int)RecordStatus.Agregado;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Por favor seleccione otra Recomendación. ya existe", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            var dataList = _tmpRecomendationConclusionesList.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);

            // Cargar grilla
            grdRecomendaciones_Conclusiones.DataSource = new RecomendationList();
            grdRecomendaciones_Conclusiones.DataSource = dataList;
            grdRecomendaciones_Conclusiones.Refresh();
            lblRecordCountRecomendaciones_Conclusiones.Text = string.Format("Se encontraron {0} registros.", dataList.Count());
        }

        private void btnRemoverRecomendaciones_Conclusiones_Click(object sender, EventArgs e)
        {
            if (grdRecomendaciones_Conclusiones.Selected.Rows.Count == 0)
            {
                MessageBox.Show("Por favor seleccione un registro.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult Result = MessageBox.Show("¿Está seguro de eliminar este registro?:", "ADVERTENCIA!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (Result == DialogResult.Yes)
            {
                // Delete the item

                // Capturar id desde la grilla de restricciones
                var recomendationId = grdRecomendaciones_Conclusiones.Selected.Rows[0].Cells["v_RecommendationId"].Value.ToString();

                // Buscar registro para remover
                var findResult = _tmpRecomendationConclusionesList.Find(p => p.v_RecommendationId == recomendationId);

                findResult.i_RecordStatus = (int)RecordStatus.EliminadoLogico;
                var dataList = _tmpRecomendationConclusionesList.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);

                grdRecomendaciones_Conclusiones.DataSource = new RecomendationList();
                grdRecomendaciones_Conclusiones.DataSource = dataList;
                grdRecomendaciones_Conclusiones.Refresh();
                lblRecordCountRecomendaciones_Conclusiones.Text = string.Format("Se encontraron {0} registros.", dataList.Count());
            }
        }

        private void grdRecomendaciones_Conclusiones_ClickCell(object sender, ClickCellEventArgs e)
        {
            ValidateRemoveRecomendaciones();
        }

        private void grdRestricciones_Conclusiones_ClickCell(object sender, ClickCellEventArgs e)
        {
            ValidateRemoveRestricciones();
        }

        private void grdRecomendaciones_Conclusiones_AfterPerformAction(object sender, AfterUltraGridPerformActionEventArgs e)
        {
            ValidateRemoveRecomendaciones();
        }

        private void grdRestricciones_Conclusiones_AfterPerformAction(object sender, AfterUltraGridPerformActionEventArgs e)
        {
            ValidateRemoveRestricciones();
        }

        private void grdRecomendaciones_Conclusiones_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {

        }

        private void grdRestricciones_Conclusiones_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {

        }

        #region Util

        private void ValidateRemoveRecomendaciones()
        {
            bool flag = false;

            if (grdRecomendaciones_Conclusiones.Selected.Rows.Count > 0)
            {
                var diagnosticRepositoryId = grdRecomendaciones_Conclusiones.Selected.Rows[0].Cells["v_DiagnosticRepositoryId"].Value;

                if (diagnosticRepositoryId == null)
                {
                    //btnRemoverRecomendaciones_Conclusiones.Enabled = true;
                    flag = true;
                }
                else
                {
                    //btnRemoverRecomendaciones_Conclusiones.Enabled = false;
                    flag = false;
                }
            }

            btnRemoverRecomendaciones_Conclusiones.Enabled = (grdRecomendaciones_Conclusiones.Selected.Rows.Count > 0 && _removerRecomendaciones_Conclusiones && flag);
        }

        private void ValidateRemoveRestricciones()
        {
            bool flag = false;

            if (grdRestricciones_Conclusiones.Selected.Rows.Count > 0)
            {
                var diagnosticRepositoryId = grdRestricciones_Conclusiones.Selected.Rows[0].Cells["v_DiagnosticRepositoryId"].Value;

                if (diagnosticRepositoryId == null)
                {
                    //btnRemoverRestricciones_ConclusionesTratamiento.Enabled = true;
                    flag = true;
                }
                else
                {
                    //btnRemoverRestricciones_ConclusionesTratamiento.Enabled = false;
                    flag = false;
                }

            }

            btnRemoverRestricciones_ConclusionesTratamiento.Enabled = (grdRestricciones_Conclusiones.Selected.Rows.Count > 0 && _removerRestricciones_ConclusionesTratamiento && flag);

        }

        #endregion

        #endregion

        #region Custom Events

        private void GeneratedAutoDX(string valueToAnalyze, Control senderCtrl, KeyTagControl tagCtrl)
        {
            string componentFieldsId = tagCtrl.v_ComponentFieldsId;

            // Retorna el DX (automático) generado, luego de una serie de evaluaciones.
            var diagnosticRepository = SearchDxSugeridoOfSystem(valueToAnalyze, componentFieldsId);

            DiagnosticRepositoryList findControlResult = null;

            if (_tmpExamDiagnosticComponentList != null)
            {
                // Buscar control que haya generado algun DX automático
                findControlResult = _tmpExamDiagnosticComponentList.Find(p => p.v_ComponentFieldsId == componentFieldsId && p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);
            }

            // Remover DX (automático) encontrado.
            if (findControlResult != null)
            {
                if (findControlResult.i_RecordType == (int)RecordType.Temporal)
                    _tmpExamDiagnosticComponentList.Remove(findControlResult);
                else
                    findControlResult.i_RecordStatus = (int)RecordStatus.EliminadoLogico;
            }

            // Si se generó un DX (automático).
            if (diagnosticRepository != null)
            {
                // Setear v_ComponentFieldValuesId en mi variable de información TAG
                tagCtrl.v_ComponentFieldValuesId = diagnosticRepository.v_ComponentFieldValuesId;

                // Pintar de rojo el fondo del control que generó el DX (automático) 
                // en caso hubiera una alteracion si es normal NO se pinta.               
                senderCtrl.BackColor = Color.Pink;   // DX Alterado              

                if (_tmpExamDiagnosticComponentList != null)
                {
                    // Se agrega el DX obtenido a la lista de DX general.
                    _tmpExamDiagnosticComponentList.Add(diagnosticRepository);
                }
                else
                {
                    _tmpExamDiagnosticComponentList = new List<DiagnosticRepositoryList>();
                    _tmpExamDiagnosticComponentList.Add(diagnosticRepository);
                }
            }
            else        // No
            {
                senderCtrl.BackColor = Color.White;
            }

            if (_tmpExamDiagnosticComponentList != null)
            {
                // Filtar para Mostrar en la grilla solo registros que no están eliminados
                var dataList = _tmpExamDiagnosticComponentList.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);

                // Refrescar grilla                        
                grdDiagnosticoPorExamenComponente.DataSource = dataList;
                lblRecordCountDiagnosticoPorExamenCom.Text = string.Format("Se encontraron {0} registros.", dataList.Count());
            }
        }

        private void txt_ValueChanged(object sender, EventArgs e)
        {
            if (flagValueChange)
            {
                // Capturar el control invocador
                Control senderCtrl = (Control)sender;
                // Obtener información contenida en la propiedad Tag del control invocante
                var tagCtrl = (KeyTagControl)senderCtrl.Tag;
                string valueToAnalyze = GetValueControl(tagCtrl.i_ControlId, senderCtrl);
                int isSourceField = tagCtrl.i_IsSourceFieldToCalculate;
                Dictionary<string, object> Params = null;
                List<double> evalExpResultList = new List<double>();

                ////MessageBox.Show(senderCtrl.Text);
                if (isSourceField == (int)SiNo.SI)
                {
                    #region Nueva logica de calculo de formula soporta n parametros

                    // Recorrer las formulas en las cuales el campo esta referenciado
                    foreach (var formu in tagCtrl.Formula)
                    {
                        // Obtener Campos fuente participantes en el calculo
                        var sourceFields = Common.Utils.GetTextFromExpressionInCorchete(formu.v_Formula);
                        Params = new Dictionary<string, object>();

                        foreach (string sf in sourceFields)
                        {
                            // Buscar controles fuentes
                            var findCtrlResult = FindDynamicControl(sf);
                            var length = findCtrlResult.Length;
                            // La busqueda si tuvo exito
                            if (length != 0)
                            {
                                // Obtener información del control encontrado 
                                var tagSourceField = (KeyTagControl)findCtrlResult[0].Tag;
                                // Obtener el tipo de dato al cual se va castear un control encontrado
                                string dtc = GetDataTypeControl(tagSourceField.i_ControlId);
                                // Obtener value del control encontrado
                                var value = GetValueControl(tagSourceField.i_ControlId, findCtrlResult[0]);

                                if (dtc == "int")
                                {
                                    //var ival = int.Parse(value);
                                    Params[sf] = int.Parse(value);
                                }
                                else if (dtc == "double")
                                {
                                    Params[sf] = double.Parse(value);
                                }
                            }
                            else
                            {
                                if (sf.ToUpper() == "EDAD")
                                {
                                    Params[sf] = _age;
                                }
                                else if (sf.ToUpper() == "GENERO_2")
                                {
                                    Params[sf] = _sexType == Gender.FEMENINO ? 0 : 1;
                                }
                                else if (sf.ToUpper() == "GENERO_1")
                                {
                                    Params[sf] = _sexType == Gender.MASCULINO ? 0 : 1;
                                }
                            }

                        } // fin foreach sourceFields

                        bool isFound = false;

                        // Buscar algun cero
                        foreach (var item in Params)
                        {
                            if (item.Value.ToString() == "0" &&
                                item.Key != "EDAD" &&
                                item.Key != "GENERO_1" &&
                                item.Key != "GENERO_2")
                            {
                                isFound = true;
                                break;
                            }
                        }

                        if (!isFound)
                        {
                            var evalExpResult = Common.Utils.EvaluateExpression(formu.v_Formula, Params);
                            evalExpResultList.Add(evalExpResult);
                        }

                    } // fin foreach Formula

                    // Mostrar el resultado en el control indicado
                    if (evalExpResultList.Count != 0)
                    {
                        for (int i = 0; i < tagCtrl.TargetFieldOfCalculateId.Count; i++)
                        {
                            var targetFieldOfCalculate1 = FindDynamicControl(tagCtrl.TargetFieldOfCalculateId[i].v_TargetFieldOfCalculateId);

                            for (int j = 0; j < evalExpResultList.Count; j++)
                            {
                                if (i == j)
                                {
                                    targetFieldOfCalculate1[0].Text = evalExpResultList[j].ToString();
                                }
                            }
                        }
                    }

                    #endregion

                }

                GeneratedAutoDX(valueToAnalyze, senderCtrl, tagCtrl);

            }

        }

        private void txt_Leave(object sender, System.EventArgs e)
        {
            flagValueChange = true;

            // Capturar el control invocador
            Control senderCtrl = (Control)sender;
            // Obtener información contenida en la propiedad Tag del control invocante
            var tagCtrl = (KeyTagControl)senderCtrl.Tag;
            string valueToAnalyze = GetValueControl(tagCtrl.i_ControlId, senderCtrl);
            int isSourceField = tagCtrl.i_IsSourceFieldToCalculate;

            Dictionary<string, object> Params = null;
            List<double> evalExpResultList = new List<double>();

            #region logica de modificacion de flag [_isChangeValue]

            if (!_isChangeValue)
            {
                if (_oldValue != valueToAnalyze)
                {
                    _isChangeValue = true;
                }
            }

            #endregion

            if (isSourceField == (int)SiNo.SI)
            {

                #region Nueva logica de calculo de formula soporta n parametros

                // Recorrer las formulas en las cuales el campo esta referenciado
                foreach (var formu in tagCtrl.Formula)
                {
                    // Obtener Campos fuente participantes en el calculo
                    var sourceFields = Common.Utils.GetTextFromExpressionInCorchete(formu.v_Formula);
                    Params = new Dictionary<string, object>();

                    foreach (string sf in sourceFields)
                    {
                        // Buscar controles fuentes
                        var findCtrlResult = FindDynamicControl(sf);
                        var length = findCtrlResult.Length;
                        // La busqueda si tuvo exito
                        if (length != 0)
                        {
                            // Obtener información del control encontrado 
                            var tagSourceField = (KeyTagControl)findCtrlResult[0].Tag;
                            // Obtener el tipo de dato al cual se va castear un control encontrado
                            string dtc = GetDataTypeControl(tagSourceField.i_ControlId);
                            // Obtener value del control encontrado
                            var value = GetValueControl(tagSourceField.i_ControlId, findCtrlResult[0]);

                            if (dtc == "int")
                            {
                                //var ival = int.Parse(value);
                                Params[sf] = int.Parse(value);
                            }
                            else if (dtc == "double")
                            {
                                Params[sf] = double.Parse(value);
                            }
                        }
                        else
                        {
                            if (sf.ToUpper() == "EDAD")
                            {
                                Params[sf] = _age;
                            }
                            else if (sf.ToUpper() == "GENERO_2")
                            {
                                Params[sf] = _sexType == Gender.FEMENINO ? 0 : 1;
                            }
                            else if (sf.ToUpper() == "GENERO_1")
                            {
                                Params[sf] = _sexType == Gender.MASCULINO ? 0 : 1;
                            }
                        }

                    } // fin foreach sourceFields

                    bool isFound = false;

                    //// Buscar algun cero
                    //foreach (var item in Params)
                    //{
                    //    if (item.Value.ToString() == "0" &&
                    //        item.Key != "EDAD" &&
                    //        item.Key != "GENERO_1" &&
                    //        item.Key != "GENERO_2")
                    //    {
                    //        isFound = true;
                    //        break;
                    //    }
                    //}

                    var isContain = formu.v_Formula.Contains("/");
                    // si es una operacion de division evitar los ceros
                    if (isContain)
                    {
                        foreach (var item in Params)
                        {
                            if (item.Value.ToString() == "0")
                            {
                                isFound = true;
                                break;
                            }
                        }
                    }

                    if (!isFound)
                    {
                        var evalExpResult = Common.Utils.EvaluateExpression(formu.v_Formula, Params);
                        evalExpResultList.Add(evalExpResult);
                        var targetFieldOfCalculate1 = FindDynamicControl(formu.v_TargetFieldOfCalculateId);
                        targetFieldOfCalculate1[0].Text = evalExpResult.ToString();
                    }

                } // fin foreach Formula

                #endregion

                GeneratedAutoDX(valueToAnalyze, senderCtrl, tagCtrl);
            }
            else
            {
                GeneratedAutoDX(valueToAnalyze, senderCtrl, tagCtrl);
            }

            #region Covid confirmado email

            if (tagCtrl.v_ComponentFieldsId == Constants.CERTIFICADO_COVID_RES_1_PRUEBA_ID 
                || tagCtrl.v_ComponentFieldsId == Constants.CERTIFICADO_COVID_RES_2_PRUEBA_ID
                || tagCtrl.v_ComponentFieldsId == Constants.COVID_RES_1_PRUEBA_ID
                || tagCtrl.v_ComponentFieldsId == Constants.COVID_RES_2_PRUEBA_ID)
            {
                if (valueToAnalyze == "2" || valueToAnalyze == "3" || valueToAnalyze == "4")
                {
                    var result = MessageBox.Show("¿Desea enviar correo a médico de empresa?", "CONFIRMACIÓN", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        var resultado ="";
                        if (valueToAnalyze =="2")
	                        {
		                        resultado = "IgM Positivo";
                                EnviarCorreoCovid(tagCtrl.v_TextLabel + ": " + resultado);
	                        }
                        else if(valueToAnalyze =="3"){
                        resultado = "IgG Positivo";
                        EnviarCorreoCovid(tagCtrl.v_TextLabel + ": " + resultado);
                        }
                        else if(valueToAnalyze =="4"){
                        resultado = "IgM e IgG positivo";
                        EnviarCorreoCovid(tagCtrl.v_TextLabel + ": " + resultado);
                        }

                        
                    }

                    if (valueToAnalyze == "2")
                    {
                        _serviceBL.UpdateServiceAptitudeNOAPTO(_serviceId);
                    }                   
                }
            }


            #endregion

        }

        private void Capture_Value(object sender, EventArgs e)
        {
            Control senderCtrl = (Control)sender;
            // Obtener información contenida en la propiedad Tag del control invocante
            var tagCtrl = (KeyTagControl)senderCtrl.Tag;
            // Capturar valor inicial
            _oldValue = GetValueControl(tagCtrl.i_ControlId, senderCtrl);

        }

        private void cb_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // Flag
            if (_cancelEventSelectedIndexChange)
                return;

            var tagCtrl = (KeyTagControl)((ComboBox)sender).Tag;
            var componentId = tagCtrl.v_ComponentId;
            var value1 = int.Parse(((ComboBox)sender).SelectedValue.ToString());

            TextBox field = null;
           
//DAVID MIRA ACA
            if (value1 == (int)NormalAlterado.Alterado)
            {
                var frm = new Operations.Popups.frmRegisterFinding(tagCtrl.v_ComponentName, "", tagCtrl.v_TextLabel);

                frm.ShowDialog();

                if (frm.DialogResult == DialogResult.Cancel)
                    return;         

                if (_componentId.Contains(Constants.EXAMEN_FISICO_ID))
                {
                    field = (TextBox)FindControlInCurrentTab(Constants.EXAMEN_FISICO_HALLAZGOS_ID)[0];

                    if (field != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        if (field.Text == string.Empty)
                        {
                            sb.Append(frm.FindingText);
                        }
                        else
                        {
                            sb.Append(field.Text);
                            sb.Append("\r\n");
                            sb.Append(frm.FindingText);
                        }

                        field.Text = sb.ToString();
                    }
                }

                if (_componentId.Contains(Constants.EXAMEN_FISICO_ID))
                {}
            }
        }

        private void cb_SelectedIndexChanged_Antecedentes(object sender, System.EventArgs e)
        {
            // Flag
            if (_cancelEventSelectedIndexChange)
                return;

            OperationResult objOperationResult = new OperationResult();

            var cb = (ComboBox)sender;
            var tagCtrl = (KeyTagControl)cb.Tag;
            var componentId = tagCtrl.v_ComponentId;
            var value1 = int.Parse(cb.SelectedValue.ToString());
            var nameCtrl = cb.Name;
            var label = tagCtrl.v_TextLabel;
            var diseasesId = string.Empty;

            // combo [Audiometria -> sordera]
            if (nameCtrl == Constants.AUDIOMETRIA_ANTECEDENTES_SORDERA)
            {
                diseasesId = Constants.SORDERA;
            }

            if (value1 == (int)SiNoNoDefine.Si)
            {              
                //History.frmPersonMedicalPopup frm = new History.frmPersonMedicalPopup(label, -1, DateTime.Now.Date, null, DateTime.Now.Date, null, _personId, diseasesId, "frmEso");              
                //frm.ShowDialog();
               
                //if (frm.DialogResult == DialogResult.Cancel)
                //    return;
            }
            else if (value1 == (int)SiNoNoDefine.No)
            {
                var exist = new ServiceBL().ExistPersonMedicalHistory(_personId, diseasesId);
                //// solo si existe un antecedente medico personal registrado entonces se procede a removerlo
                //if (exist)
                //{
                //    var result = MessageBox.Show("¿Desea realmente remover este antecedente medico personal?", "CONFIRMACIÓN", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                //    if (result == DialogResult.Yes)
                //    {
                //        new HistoryBL().DeletePersonMedicalHistoryFromESO(ref objOperationResult,
                //                                       _personId,
                //                                       diseasesId,
                //                                       Globals.ClientSession.GetAsList());
                //    }
                //}
            }
            else if (value1 == (int)SiNoNoDefine.NoDefine)
            {
                
                //var result = MessageBox.Show("¿Desea realmente poner NO DEFINE para este antecedente?", "CONFIRMACIÓN", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                //if (result == DialogResult.Yes)
                //{

                //    personmedicalhistoryDto personmedicalhistoryDtoDto = new personmedicalhistoryDto();

                //    personmedicalhistoryDtoDto.v_PersonId = _personId;
                //    personmedicalhistoryDtoDto.v_DiseasesId = diseasesId;
                //    personmedicalhistoryDtoDto.i_AnswerId = (int)SiNo.NONE;

                //    new HistoryBL().NoDefinePersonMedicalHistoryFromESO(ref objOperationResult,
                //                                        personmedicalhistoryDtoDto,
                //                                        Globals.ClientSession.GetAsList());
                //}
               
            }
        }

        private void ucAudiometria_AfterValueChange(object sender, AudiometriaAfterValueChangeEventArgs e)
        {
            var diagnosticRepository = e.PackageSynchronization as List<DiagnosticRepositoryList>;

            #region Delete Dx

            List<DiagnosticRepositoryList> findControlResult = null;

            if (_tmpExamDiagnosticComponentList != null)
            {
                // Buscar control que haya generado algun DX automático
                findControlResult = _tmpExamDiagnosticComponentList.FindAll(p => e.ListcomponentFieldsId.Contains(p.v_ComponentFieldsId) && p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);
            }

            // Remover DX (automático) encontrado.
            if (findControlResult != null)
            {
                foreach (var item in findControlResult)
                {
                    if (item.i_RecordType == (int)RecordType.Temporal)
                    {
                        _tmpExamDiagnosticComponentList.Remove(item);
                    }
                    else
                    {
                        item.i_RecordStatus = (int)RecordStatus.EliminadoLogico;
                        item.Recomendations.ForEach(f => f.i_RecordStatus = (int)RecordStatus.EliminadoLogico);
                        item.Restrictions.ForEach(f => f.i_RecordStatus = (int)RecordStatus.EliminadoLogico);
                    }
                }

            }

            #endregion

            // Si se generó un DX (automático).
            if (diagnosticRepository != null)
            {
                // Set id servicio
                diagnosticRepository.ForEach(p => p.v_ServiceId = _serviceId);
                diagnosticRepository.SelectMany(p => p.Recomendations).ToList().ForEach(f => f.v_ServiceId = _serviceId);
                diagnosticRepository.SelectMany(p => p.Restrictions).ToList().ForEach(f => f.v_ServiceId = _serviceId);

                if (_tmpExamDiagnosticComponentList != null)
                {
                    // Se agrega el DX obtenido a la lista de DX general.
                    _tmpExamDiagnosticComponentList.AddRange(diagnosticRepository);
                }
                else
                {
                    _tmpExamDiagnosticComponentList = new List<DiagnosticRepositoryList>();
                    _tmpExamDiagnosticComponentList.AddRange(diagnosticRepository);
                }

            }

            if (_tmpExamDiagnosticComponentList != null)
            {
                // Filtar para Mostrar en la grilla solo registros que no están eliminados
                var dataList = _tmpExamDiagnosticComponentList.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);

                // Refrescar grilla                        
                grdDiagnosticoPorExamenComponente.DataSource = dataList;
                lblRecordCountDiagnosticoPorExamenCom.Text = string.Format("Se encontraron {0} registros.", dataList.Count());
            }

        }

        private void ucSomnolencia_AfterValueChange(object sender, AudiometriaAfterValueChangeEventArgs e)
        {
            //var diagnosticRepository = e.PackageSynchronization as List<DiagnosticRepositoryList>;

            //#region Delete Dx

            //List<DiagnosticRepositoryList> findControlResult = null;

            //if (_tmpExamDiagnosticComponentList != null)
            //{
            //    // Buscar control que haya generado algun DX automático
            //    findControlResult = _tmpExamDiagnosticComponentList.FindAll(p => e.ListcomponentFieldsId.Contains(p.v_ComponentFieldsId) && p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);
            //}

            //// Remover DX (automático) encontrado.
            //if (findControlResult != null)
            //{
            //    foreach (var item in findControlResult)
            //    {
            //        if (item.i_RecordType == (int)RecordType.Temporal)
            //            _tmpExamDiagnosticComponentList.Remove(item);
            //        else
            //            item.i_RecordStatus = (int)RecordStatus.EliminadoLogico;
            //    }

            //}

            //#endregion

            //// Si se generó un DX (automático).
            //if (diagnosticRepository != null)
            //{
            //    // Set id servicio
            //    diagnosticRepository.ForEach(p => p.v_ServiceId = _serviceId);
            //    diagnosticRepository.SelectMany(p => p.Recomendations).ToList().ForEach(f => f.v_ServiceId = _serviceId);
            //    diagnosticRepository.SelectMany(p => p.Restrictions).ToList().ForEach(f => f.v_ServiceId = _serviceId);

            //    if (_tmpExamDiagnosticComponentList != null)
            //    {
            //        // Se agrega el DX obtenido a la lista de DX general.
            //        _tmpExamDiagnosticComponentList.AddRange(diagnosticRepository);
            //    }
            //    else
            //    {
            //        _tmpExamDiagnosticComponentList = new List<DiagnosticRepositoryList>();
            //        _tmpExamDiagnosticComponentList.AddRange(diagnosticRepository);
            //    }

            //}

            //if (_tmpExamDiagnosticComponentList != null)
            //{
            //    // Filtar para Mostrar en la grilla solo registros que no están eliminados
            //    var dataList = _tmpExamDiagnosticComponentList.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);

            //    // Refrescar grilla                        
            //    grdDiagnosticoPorExamenComponente.DataSource = dataList;
            //    lblRecordCountDiagnosticoPorExamenCom.Text = string.Format("Se encontraron {0} registros.", dataList.Count());
            //}

        }

        #endregion

        #region Antecedentes

        private void GetAntecedentConsolidateForService(string personId)
        {
            OperationResult objOperationResult = new OperationResult();
            var antecedent = _serviceBL.GetAntecedentConsolidateForService(ref objOperationResult, personId);

            if (antecedent == null)
                return;

            grdAntecedentes.DataSource = antecedent;

            // Analizar el resultado de la operación
            if (objOperationResult.Success != 1)
            {
                MessageBox.Show(Constants.GenericErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetServicesConsolidateForService(string personId)
        {
            OperationResult objOperationResult = new OperationResult();
            var services = _serviceBL.GetServicesConsolidateForService(ref objOperationResult, personId, _serviceId);

            grdServiciosAnteriores.DataSource = services;

            // Analizar el resultado de la operación
            if (objOperationResult.Success != 1)
            {
                MessageBox.Show(Constants.GenericErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mnuGridAntecedent_Click(object sender, EventArgs e)
        {
            ViewEditAntecedent();
        }

        private void ViewEditAntecedent()
        {
            frmHistory frm = new frmHistory(_personId);
            frm.MdiParent = this.MdiParent;
            frm.Show();
            // refresca grilla de antecedentes
            GetAntecedentConsolidateForService(_personId);
        }

        private void mnuVerServicio_Click(object sender, EventArgs e)
        {
            var frm = new Operations.frmEso(_serviceIdByWiewServiceHistory, null, "View");
            frm.ShowDialog();
        }

        private void grdServiciosAnteriores_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point point = new System.Drawing.Point(e.X, e.Y);
                Infragistics.Win.UIElement uiElement = ((Infragistics.Win.UltraWinGrid.UltraGridBase)sender).DisplayLayout.UIElement.ElementFromPoint(point);

                if (uiElement == null || uiElement.Parent == null)
                    return;

                Infragistics.Win.UltraWinGrid.UltraGridRow row = (Infragistics.Win.UltraWinGrid.UltraGridRow)uiElement.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridRow));

                if (row != null)
                {
                    grdServiciosAnteriores.Rows[row.Index].Selected = true;
                    _serviceIdByWiewServiceHistory = grdServiciosAnteriores.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();
                    cmVerServicioAnterior.Items["mnuVerServicio"].Enabled = true;
                }
                else
                {
                    cmVerServicioAnterior.Items["mnuVerServicio"].Enabled = false;

                }

            }
        }

        private void btnVerEditarAntecedentes_Click(object sender, EventArgs e)
        {
            ViewEditAntecedent();
        }

        private void btnVerServicioAnterior_Click(object sender, EventArgs e)
        {
            var frm = new Operations.frmEso(_serviceIdByWiewServiceHistory, null, "View");
            frm.MdiParent = this.MdiParent;
            frm.Show();
            /////
        }

        private void grdServiciosAnteriores_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            btnVerServicioAnterior.Enabled = (grdServiciosAnteriores.Selected.Rows.Count > 0);

            if (grdServiciosAnteriores.Selected.Rows.Count == 0)
                return;

            _serviceIdByWiewServiceHistory = grdServiciosAnteriores.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();
        }

        #endregion

        private void tcSubMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SaveExamWherePendingChange(); 
        }

        private void tcSubMain_TabIndexChanged(object sender, EventArgs e)
        {
            // mo funciona
            // SaveExamWherePendingChange();
        }

        private void tcSubMain_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            SaveExamWherePendingChange();
        }

        private void grdRecomendaciones_AnalisisDiagnostico_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            btnRemoverRecomendacion_AnalisisDx.Enabled = (grdRecomendaciones_AnalisisDiagnostico.Selected.Rows.Count > 0 && _removerRecomendacion_AnalisisDx);
        }

        private void grdRestricciones_AnalisisDiagnostico_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            btnRemoverRestriccion_Analisis.Enabled = (grdRestricciones_AnalisisDiagnostico.Selected.Rows.Count > 0 && _removerRestriccion_Analisis);
        }

        private void cbSueño_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Flag
            if (_cancelEventSelectedIndexChange)
                return;

            if (int.Parse(cbSueño.SelectedValue.ToString()) == (int)AumentadoDisminuidoConservado.Conservado)
            {
                var frm = new Operations.Popups.frmRegisterFinding("Funciones Biológicas", "", "Sueño");

                frm.ShowDialog();

                if (frm.DialogResult == DialogResult.Cancel)
                    return;

                //txtHallazgos.Text = null;
                #region Obtener campo Hallazgo


                #endregion

                if (txtHallazgos.Text != null)
                {
                    #region Escribir en el campo hallazgo

                    StringBuilder sb = new StringBuilder();

                    if (txtHallazgos.Text == string.Empty)
                    {
                        sb.Append(frm.FindingText);
                    }
                    else
                    {
                        sb.Append(txtHallazgos.Text);
                        sb.Append("\r\n");
                        sb.Append(frm.FindingText);
                    }

                    txtHallazgos.Text = sb.ToString();

                    #endregion
                }
            }
        }

        private void cbOrina_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Flag
            if (_cancelEventSelectedIndexChange)
                return;

            if (int.Parse(cbOrina.SelectedValue.ToString()) == (int)AumentadoDisminuidoConservado.Conservado)
            {
                var frm = new Operations.Popups.frmRegisterFinding("Funciones Biológicas", "", "Orina");

                frm.ShowDialog();

                if (frm.DialogResult == DialogResult.Cancel)
                    return;

                //txtHallazgos.Text = null;
                #region Obtener campo Hallazgo


                #endregion

                if (txtHallazgos.Text != null)
                {
                    #region Escribir en el campo hallazgo

                    StringBuilder sb = new StringBuilder();

                    if (txtHallazgos.Text == string.Empty)
                    {
                        sb.Append(frm.FindingText);
                    }
                    else
                    {
                        sb.Append(txtHallazgos.Text);
                        sb.Append("\r\n");
                        sb.Append(frm.FindingText);
                    }

                    txtHallazgos.Text = sb.ToString();

                    #endregion

                }
            }
        }

        private void cbDeposiciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Flag
            if (_cancelEventSelectedIndexChange)
                return;
            if (int.Parse(cbDeposiciones.SelectedValue.ToString()) == (int)AumentadoDisminuidoConservado.Conservado)
            {
                var frm = new Operations.Popups.frmRegisterFinding("Funciones Biológicas", "", "Deposiciones");

                frm.ShowDialog();

                if (frm.DialogResult == DialogResult.Cancel)
                    return;

                //txtHallazgos.Text = null;
                #region Obtener campo Hallazgo


                #endregion

                if (txtHallazgos.Text != null)
                {
                    #region Escribir en el campo hallazgo

                    StringBuilder sb = new StringBuilder();

                    if (txtHallazgos.Text == string.Empty)
                    {
                        sb.Append(frm.FindingText);
                    }
                    else
                    {
                        sb.Append(txtHallazgos.Text);
                        sb.Append("\r\n");
                        sb.Append(frm.FindingText);
                    }

                    txtHallazgos.Text = sb.ToString();

                    #endregion
                }
            }
        }

        private void cbApetito_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Flag
            if (_cancelEventSelectedIndexChange)
                return;
            if (int.Parse(cbApetito.SelectedValue.ToString()) == (int)AumentadoDisminuidoConservado.Conservado)
            {
                var frm = new Operations.Popups.frmRegisterFinding("Funciones Biológicas", "", "Apetito");

                frm.ShowDialog();

                if (frm.DialogResult == DialogResult.Cancel)
                    return;

                //txtHallazgos.Text = null;
                #region Obtener campo Hallazgo


                #endregion

                if (txtHallazgos.Text != null)
                {
                    #region Escribir en el campo hallazgo

                    StringBuilder sb = new StringBuilder();

                    if (txtHallazgos.Text == string.Empty)
                    {
                        sb.Append(frm.FindingText);
                    }
                    else
                    {
                        sb.Append(txtHallazgos.Text);
                        sb.Append("\r\n");
                        sb.Append(frm.FindingText);
                    }

                    txtHallazgos.Text = sb.ToString();

                    #endregion
                }
            }
        }

        private void cbSed_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Flag
            if (_cancelEventSelectedIndexChange)
                return;
            if (int.Parse(cbSed.SelectedValue.ToString()) == (int)AumentadoDisminuidoConservado.Conservado)
            {
                var frm = new Operations.Popups.frmRegisterFinding("Funciones Biológicas", "", "Sed");

                frm.ShowDialog();

                if (frm.DialogResult == DialogResult.Cancel)
                    return;

                //txtHallazgos.Text = null;
                #region Obtener campo Hallazgo


                #endregion

                if (txtHallazgos.Text != null)
                {
                    #region Escribir en el campo hallazgo

                    StringBuilder sb = new StringBuilder();

                    if (txtHallazgos.Text == string.Empty)
                    {
                        sb.Append(frm.FindingText);
                    }
                    else
                    {
                        sb.Append(txtHallazgos.Text);
                        sb.Append("\r\n");
                        sb.Append(frm.FindingText);
                    }

                    txtHallazgos.Text = sb.ToString();

                    #endregion
                }
            }
        }

        private void frmEso_FormClosing(object sender, FormClosingEventArgs e)
        {
            // volver el usuario que se logueo
            Globals.ClientSession.i_SystemUserId = Globals.ClientSession.i_SystemUserCopyId;

            if (_isChangeValue)
            {
                var result = MessageBox.Show("Se han realizado cambios, ¿Desea salir de todos modos?", "CONFIRMACIÓN", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                    e.Cancel = true;
            }

        }

        private void btnAddListaNerga_Click(object sender, EventArgs e)
        {
            string pstrPersonId = _personId;
            string pstrPaciente = _personName;

            frmBlackList frm = new frmBlackList(pstrPaciente, pstrPersonId);
            frm.ShowDialog();
        }

        private void btnVisorReporteExamen_Click(object sender, EventArgs e)
        {
            Form frm = null;

            var arrComponentId = _componentId.Split('|');

            if (arrComponentId.Contains(Constants.AUDIOMETRIA_ID))
            {
                frm = new Reports.frmAudiometria(_serviceId, Constants.AUDIOMETRIA_ID);               
            }
            else if (arrComponentId.Contains(Constants.OFTALMOLOGIA_ID))
            {
                frm = new Reports.frmOftalmologia(_serviceId, Constants.OFTALMOLOGIA_ID);              
            }
            else if (arrComponentId.Contains(Constants.PSICOLOGIA_ID))
            {
                frm = new Reports.frmInformePsicologicoOcupacional(_serviceId);             
            }

            frm.ShowDialog();
        }

        private void btnVerPaciente_Click(object sender, EventArgs e)
        {
            var frm = new frmPacient(_personId);
            frm.ShowDialog();
        }

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
        //        //Constants.GINECOLOGIA_ID,
        //        //Constants.EVALUACION_PSICOLABORAL,
        //        Constants.EXAMEN_PSICOLOGICO,
        //        Constants.PSICOLOGIA_ID,
        //        Constants.OIT_ID,
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



        //    serviceComponents = _serviceBL.GetServiceComponentsForManagementReport(_serviceId);

        //    foreach (var item in serviceComponents)
        //    {
        //        if (item.v_ComponentId == Constants.ALTURA_7D_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 10;
        //        }
        //        else if (item.v_ComponentId == Constants.OFTALMOLOGIA_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 9;
        //        }
        //        else if (item.v_ComponentId == Constants.ALTURA_ESTRUCTURAL_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 11;
        //        }
        //        else if (item.v_ComponentId == Constants.AUDIOMETRIA_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 7;
        //        }
        //        else if (item.v_ComponentId == Constants.ESPIROMETRIA_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 8;
        //        }
        //        else if (item.v_ComponentId == Constants.OIT_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 12;
        //        }
        //        else if (item.v_ComponentId == Constants.ELECTROCARDIOGRAMA_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 13;
        //        }
        //        else if (item.v_ComponentId == Constants.EXAMEN_PSICOLOGICO)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 14;
        //        }
        //        else if (item.v_ComponentId == Constants.ODONTOGRAMA_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 15;
        //        }
        //        else if (item.v_ComponentId == Constants.RX_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 16;
        //        }
        //        else if (item.v_ComponentId == Constants.PRUEBA_ESFUERZO_ID)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 17;
        //        }
        //        else if (item.v_ComponentId == Constants.ALTURA_FISICA_M_18)
        //        {
        //            var ent = serviceComponents.FirstOrDefault(o => o.v_ComponentId == item.v_ComponentId);
        //            ent.Orden = 10;
        //        }

        //    }

        //    List<ServiceComponentList> ConsolidadoReportes = new List<ServiceComponentList>();
        //    ConsolidadoReportes.Add(new ServiceComponentList { Orden = 1, v_ComponentName = "CERTIFICADO APTITUD", v_ComponentId = Constants.INFORME_CERTIFICADO_APTITUD });
        //    ConsolidadoReportes.Add(new ServiceComponentList { Orden = 3, v_ComponentName = "HISTORIA OCUPACIONAL", v_ComponentId = Constants.INFORME_HISTORIA_OCUPACIONAL });
        //    ConsolidadoReportes.Add(new ServiceComponentList { Orden = 2, v_ComponentName = "ANEXO 312", v_ComponentId = Constants.INFORME_ANEXO_312 });
        //    ConsolidadoReportes.Add(new ServiceComponentList { Orden = 4, v_ComponentName = "ANEXO 16", v_ComponentId = Constants.INFORME_ANEXO_7C });
        //    ConsolidadoReportes.Add(new ServiceComponentList { Orden = 5, v_ComponentName = "FICHA MEDICA DEL TRABAJADOR", v_ComponentId = Constants.INFORME_FICHA_MEDICA_TRABAJADOR });
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

        private void btnGuardarComentario_Click(object sender, EventArgs e)
        {



            if (chkUtilizarFirma.Checked)
            {
                var frm = new Popups.frmSelectSignature();
                frm.ShowDialog();

                if (frm.DialogResult != System.Windows.Forms.DialogResult.Cancel)
                {
                    if (frm.i_SystemUserSuplantadorId != null)
                    {
                        Globals.ClientSession.i_SystemUserId = (int)frm.i_SystemUserSuplantadorId;
                    }
                    else
                    {
                        Globals.ClientSession.i_SystemUserId = Globals.ClientSession.i_SystemUserCopyId;
                    }
                }
            }
            else
            {
                DialogResult Result = MessageBox.Show("¿Desea Guardar el Comentario cambiando de Firma en la Evaluación?:", "CONFIRMACIÓN!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (Result == DialogResult.No)
                {
                    return;
                }
            }

            _chkApprovedEnabled = chkApproved.Enabled;

            #region Capturar [Comentarios, estado, procedencia de un exmanen componente]

            var serviceComponentDtox = new servicecomponentDto();
            serviceComponentDtox.v_ServiceComponentId = _serviceComponentId;
            serviceComponentDtox.v_Comment = txtComentario.Text;
            serviceComponentDtox.i_ServiceComponentStatusId = int.Parse(cbEstadoComponente.SelectedValue.ToString());
            serviceComponentDtox.i_ExternalInternalId = int.Parse(cbTipoProcedenciaExamen.SelectedValue.ToString());
            serviceComponentDtox.i_IsApprovedId = Convert.ToInt32(chkApproved.Checked);

            serviceComponentDtox.v_ComponentId = _componentId;
            serviceComponentDtox.v_ServiceId = _serviceId;

            #endregion

            if (serviceComponentDtox != null)
            {
                // Actualizar algunos valores de ServiceComponent
                OperationResult objOperationResult = new OperationResult();
                _serviceBL.UpdateServiceComponentFromEso(ref objOperationResult, serviceComponentDtox, Globals.ClientSession.GetAsList(), _chkApprovedEnabled);
            }


        }

        private void EnviarCorreoCovid(string examen)
        {
            OperationResult objOperationResult = new OperationResult();
            try
            {
                // Obtener los Parametros necesarios para el envio de notificación
                var configEmail = BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 161, "i_ParameterId");
                var empresaData = new OrganizationBL().GetOrganization(ref objOperationResult, _OrganizationId);
                if (string.IsNullOrEmpty(empresaData.v_Mail)) return;

                var correoDoctor = empresaData.v_Mail;
                string smtp = configEmail[0].Value1.ToLower();
                int port = int.Parse(configEmail[1].Value1);
                string from = configEmail[2].Value1.ToLower();
                string fromPassword = configEmail[4].Value1;
                string subject = "Resultados de prueba Covid-19";
                bool enableSsl = Convert.ToBoolean(int.Parse(configEmail[3].Value1));
                string message = string.Format("{0}, resultados: {1}", _personName, examen);

                // Enviar notificación de usuario y clave via email
                Common.Utils.SendMessage(smtp, port, enableSsl, true, from, fromPassword, correoDoctor, "francisco.collantes@saluslaboris.com.pe", subject, message, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Verifique su conexion de internet y/o cable de red,\n es posible que este desconectado.", "Error al enviar notificación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CloseErrorfrmWaiting();
            }
        }

        private void EnviarPdfTrabajador()
        {
            OperationResult objOperationResult = new OperationResult();
            try
            {
                var configEmail = BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 161, "i_ParameterId");
                var trabajadorData = new ServiceBL().GetWorkerData(_serviceId);
                if (string.IsNullOrEmpty(trabajadorData.Email)) return;

                var correoTrabajador = trabajadorData.Email;
                string smtp = configEmail[0].Value1.ToLower();
                int port = int.Parse(configEmail[1].Value1);
                string from = configEmail[2].Value1.ToLower();
                string fromPassword = configEmail[4].Value1;
                string subject = "Resultado Covid-19";
                bool enableSsl = Convert.ToBoolean(int.Parse(configEmail[3].Value1));
                string message = string.Format("Buenos días, el resultado del trabajador {0} ", _personName);

                string ruta = Common.Utils.GetApplicationConfigValue("rutaReportes").ToString();
                var atachFiles = new List<string>();
                string atachFile = ruta + _serviceId + ".pdf";
                atachFiles.Add(atachFile);
                // Enviar notificación de usuario y clave via email
                Common.Utils.SendMessage(smtp, port, enableSsl, true, from, fromPassword, correoTrabajador, "francisco.collantes@saluslaboris.com.pe", subject, message, atachFiles);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Verifique su conexion de internet y/o cable de red,\n es posible que este desconectado.", "Error al enviar notificación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CloseErrorfrmWaiting();
            }
        }
    }

}

