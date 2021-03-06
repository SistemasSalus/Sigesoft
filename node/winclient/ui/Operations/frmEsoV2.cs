using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinMaskedEdit;
using Infragistics.Win.UltraWinTabControl;
using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;
using Sigesoft.Node.WinClient.UI.Operations.Popups;
using Sigesoft.Node.WinClient.UI.UserControls;
using System.ComponentModel;
using System.Text;
using Sigesoft.Node.WinClient.UI.Reports;
using System.Data;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

namespace Sigesoft.Node.WinClient.UI.Operations
{
    public partial class FrmEsoV2 : Form
    {
        #region Instancias
        public class RunWorkerAsyncPackage
        {
            public Infragistics.Win.UltraWinTabControl.UltraTabPageControl SelectedTab { get; set; }
            public List<DiagnosticRepositoryList> ExamDiagnosticComponentList { get; set; }
            public servicecomponentDto ServiceComponent { get; set; }
            public int? i_SystemUserSuplantadorId { get; set; }
        }
        public class ValidacionAMC
        {
            public string v_ServiceComponentFieldValuesId { get; set; }
            public string v_Value1 { get; set; }
            public string v_ComponentFieldsId { get; set; }
        }
        public class DatosModificados
        {
            public string v_ComponentFieldsId { get; set; }
        }
        private bool _cancelEventSelectedIndexChange;
        List<ValidacionAMC> _oListValidacionAMC = new List<ValidacionAMC>();
        //private readonly ServiceBL _serviceBl = new ServiceBL();
        private List<string> _filesNameToMerge = null;
        private readonly string _serviceId;
        private readonly string _componentIdDefault;
        private readonly string _action;
        private Dictionary<string, UltraValidator> _dicUltraValidators;
        private OperationResult _objOperationResult = new OperationResult();
        private string _personId;
        private static List<GroupParameter> _cboEso;
        private bool _isDisabledButtonsExamDx;
        private readonly int _nodeId;
        private readonly int _roleId;
        private readonly int _userId;
        private string _componentId;
        private string _serviceComponentId;
        TypeESO _esoTypeId;
        private int _age;
        private List<KeyValueDTO> _formActions;
        private List<DiagnosticRepositoryList> _tmpExamDiagnosticComponentList;
        private DiagnosticRepositoryList _tmpTotalDiagnostic;
        private List<RestrictionList> _tmpRestrictionList;
        private List<RecomendationList> _tmpRecomendationList;
        private string _componentIdConclusionesDX;
        private Keys _currentDownKey = Keys.None;
        private bool _removerTotalDiagnostico;
        private bool _removerRecomendacionAnalisisDx;
        private bool _removerRestriccionAnalisis;
        private List<DiagnosticRepositoryList> _tmpTotalConclusionesDxByServiceIdList;
        private readonly List<DiagnosticRepositoryList> _tmpTotalDiagnosticList = null;
        private List<DiagnosticRepositoryList> _tmpTotalDiagnosticByServiceIdList;
        public string _diagnosticId;
        private int? _rowIndexConclucionesDX;
        private List<RecomendationList> _tmpRecomendationConclusionesList;
        private List<RestrictionList> _tmpRestrictionListConclusiones = null;
        private List<ComponentList> _tmpServiceComponentsForBuildMenuList = null;

        private List<ServiceComponentFieldsList> _serviceComponentFieldsList = null;
        private List<ServiceComponentFieldValuesList> _serviceComponentFieldValuesList = null;
        private List<ServiceComponentFieldValuesList> _tmpListValuesOdontograma = null;

        private bool _isChangeValue = false;
        private Gender _sexType;
        private bool flagValueChange = false;
        private bool _chkApprovedEnabled;
        private string _oldValue;
        #endregion
      
        public FrmEsoV2(string serviceId, string componentIdDefault, string action, int roleId, int nodeId, int userId)
        {
            _serviceId = serviceId;
            _componentIdDefault = componentIdDefault;
            _action = action;
            _roleId = roleId;
            _nodeId = nodeId;
            _userId = userId;
            InitializeComponent();
        }

        private void FrmEsoV2_Load(object sender, EventArgs e)
        {
            InitializeForm();
            ViewMode(_action);
        }

        private void InitializeForm()
        {

            InitializeStaticControls();
            LoadData();
        }

        private void LoadTabExamenes(ServiceData dataService)
        {
           _cboEso = GetListCboEso();
            SumaryWorker(dataService);
            CreateMissingExamens();
        }

        private static List<GroupParameter> GetListCboEso()
        {
            return BLL.Utils.GetSystemParameterForComboFormEso();
        }

        private void CreateMissingExamens()
        {
            var listExamenes = new ServiceBL().ListMissingExamenesNames(ref _objOperationResult, _serviceId, _nodeId, _roleId).ToList();
            foreach (var examen in listExamenes)
            {
                //if (examen.v_ComponentId == "N002-ME000000145")
                //{
                    var componentsId = new ServiceBL().ConcatenateComponents(_serviceId, examen.v_ComponentId);
                    AsyncCreateNextExamen(componentsId, examen.v_CategoryName);
                //}
              
            }
        }

        private void SumaryWorker(ServiceData dataService)
        {
            lblTrabajador.Text = dataService.Trabajador;
            lblProtocolName.Text = dataService.ProtocolName;
            _esoTypeId = (TypeESO)dataService.EsoTypeId;
            _age = Common.Utils.GetAge(dataService.FechaNacimiento.Value);
        }

        private static List<rolenodecomponentprofileDto> UserPermission()
        {
            var readingPermission = new ServiceBL().GetRoleNodeComponentProfileByRoleNodeId_(int.Parse(Globals.ClientSession.i_CurrentExecutionNodeId.ToString()), int.Parse(Globals.ClientSession.i_RoleId.ToString())).FindAll(p => p.i_Read == 1);
            return readingPermission;
        }

        private void AsyncCreateNextExamen(string componentId, string threadName)
        {
            //ThreadPool.QueueUserWorkItem(CallBack,componentId);
            var t = new Thread(() => { DoWorkGetExamen(componentId); }) { Name = threadName };
            t.Priority = t.Name == _componentIdDefault ? ThreadPriority.Highest : ThreadPriority.Lowest;
            t.Start();
        }

        private void CallBack(object state)
        {
            var componentId = (string)state; 
            var nextExamen = new ServiceBL().ExamenByDefaultOrAssigned(ref _objOperationResult, _serviceId, componentId)[0];
            var serviceComponentsInfo = new ServiceBL().GetServiceComponentsInfo(ref _objOperationResult, nextExamen.v_ServiceComponentId, _serviceId);
            CreateExamen(nextExamen, serviceComponentsInfo);
        }

        public void DoWorkGetExamen(string componentId)
        {
            if (_tmpServiceComponentsForBuildMenuList == null)
            {
                _tmpServiceComponentsForBuildMenuList = new List<ComponentList>();
            }
            var nextExamen = new ServiceBL().ExamenByDefaultOrAssigned(ref _objOperationResult, _serviceId, componentId)[0];
            _tmpServiceComponentsForBuildMenuList.Add(nextExamen);
            var  serviceComponentsInfo  =  new ServiceBL().GetServiceComponentsInfo(ref _objOperationResult, nextExamen.v_ServiceComponentId, _serviceId);
            CreateExamen(nextExamen, serviceComponentsInfo);
        }

        private void CreateExamen(ComponentList component, ServiceComponentList serviceComponentsInfo)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<ComponentList, ServiceComponentList>(CreateExamen), component, serviceComponentsInfo);
            }
            else
            {
                var row = 1;
                const int column = 1;
                var fieldsByGroupBoxCount = 1;

                var tab = CrearTab(component);
                var tlpParent = CreateTableLayoutPanelParent(tab);

                var uv = CreateValidators(component);
                foreach (var groupComponentName in component.GroupedComponentsName)
                {
                    var gbComponent = CreateGroupBoxComponent(groupComponentName);
                    row++;
                    var tlpChildren = CreateTableLayoutPanelChildren(groupComponentName);
                    var groupBoxes = GetGroupBoxes(component, groupComponentName.v_ComponentId);
                    foreach (var groupbox in groupBoxes)
                    {
                        var groupBox = CreateGroupBoxForGroup(groupbox);
                        fieldsByGroupBoxCount++;
                        var tableLayoutPanel = CreateTableLayoutForControls(groupbox, component);
                        var fieldsByGroupBox = GetFieldsEachGroups(component, groupbox, groupComponentName);
                        fieldsByGroupBox.Aggregate(1, (current, field) => CreateCampoComponente(component, field, groupbox.i_Column, current, tableLayoutPanel, uv));
                        groupBox.Controls.Add(tableLayoutPanel);
                        tlpChildren.Controls.Add(groupBox, column, fieldsByGroupBoxCount);
                    }
                    gbComponent.Controls.Add(tlpChildren);
                    tlpParent.Controls.Add(gbComponent, column, row);
                }
                tlpParent.AutoScroll = true;
                tlpParent.Dock = DockStyle.Fill;
                tlpParent.BackColor = Color.Gray;
                _cancelEventSelectedIndexChange = true;
                if (component.i_ServiceComponentStatusId == (int) ServiceComponentStatus.Iniciado ||
                    component.i_ServiceComponentStatusId == (int) ServiceComponentStatus.PorIniciar)
                    SetDefaultValueAfterBuildMenu(component);
                else
                {
                    SetDefaultValueAfterBuildMenu(component);
                    SearchControlAndSetValue(tab.TabPage, serviceComponentsInfo);
                }
                _cancelEventSelectedIndexChange = false; 
            }
        }
        
        private void SearchControlAndSetValue(Control tlpParent, ServiceComponentList serviceComponentsInfo)
        {
            KeyTagControl keyTagControl = null;
            var breakHazChildrenUc = false;
            ValidacionAMC oValidacionAMC = null;
            var x = serviceComponentsInfo.ServiceComponentFields;
            var y = x.Select(p => p.ServiceComponentFieldValues).ToList();
            _oListValidacionAMC = new List<ValidacionAMC>();
            foreach (var item in y)
            {
                oValidacionAMC = new ValidacionAMC();
                oValidacionAMC.v_ServiceComponentFieldValuesId = item[0].v_ServiceComponentFieldValuesId;
                oValidacionAMC.v_Value1 = item[0].v_Value1;
                oValidacionAMC.v_ComponentFieldsId = item[0].v_ComponentFieldId;
                _oListValidacionAMC.Add(oValidacionAMC);
            }
            foreach (Control ctrl in tlpParent.Controls)
            {
                if (ctrl.Tag != null)
                {
                    var t = ctrl.Tag.GetType();
                    if (t == typeof(KeyTagControl))
                    {
                        // Capturar objeto tag
                        keyTagControl = (KeyTagControl)ctrl.Tag;

                        List<ServiceComponentFieldValuesList> dataSourceUserControls;
                        switch (keyTagControl.i_ControlId)
                        {
                            case (int)ControlType.UcOdontograma:
                                
                                //dataSourceUserControls = serviceComponentsInfo.ServiceComponentFields.SelectMany(p => p.ServiceComponentFieldValues).ToList();
                                //dataSourceUserControls = dataSourceUserControls.FindAll(p => p.v_ComponentFieldId.Contains("ODO"));
                                //((ucOdontograma)ctrl).DataSource = new List<ServiceComponentFieldValuesList>();
                                //((ucOdontograma)ctrl).DataSource = dataSourceUserControls;
                                //breakHazChildrenUc = true;
                                dataSourceUserControls = serviceComponentsInfo.ServiceComponentFields.SelectMany(p => p.ServiceComponentFieldValues).ToList();
                                dataSourceUserControls = dataSourceUserControls.FindAll(p => p.v_ComponentFieldId.Contains("ODO"));
                                ((UserControls.ucOdontograma)ctrl).OdoContext = OdoContext.FillFromESO;
                                ((UserControls.ucOdontograma)ctrl)._ListaOdontograma = new List<ServiceComponentFieldValuesList>();
                                //((UserControls.ucOdontograma)ctrl).DataSource = new List<ServiceComponentFieldValuesList>();
                                ((UserControls.ucOdontograma)ctrl).DataSource = dataSourceUserControls;
                                breakHazChildrenUc = true;
                                break;
                            case (int)ControlType.UcAudiometria:
                                //SEGUIR-  SE REPITE 4 VECES Y POR ESO NO PINTA EL USER CONTROL
                                dataSourceUserControls = serviceComponentsInfo.ServiceComponentFields.SelectMany(p => p.ServiceComponentFieldValues).ToList();
                                dataSourceUserControls = dataSourceUserControls.FindAll(p => p.v_ComponentFieldId.Contains("AUD"));
                                ((ucAudiometria)ctrl).DataSource = new List<ServiceComponentFieldValuesList>();
                                ((ucAudiometria)ctrl).DataSource = dataSourceUserControls;
                                breakHazChildrenUc = true;
                                break;
                            case (int)ControlType.UcSomnolencia:
                                dataSourceUserControls = serviceComponentsInfo.ServiceComponentFields.SelectMany(p => p.ServiceComponentFieldValues).ToList();
                                dataSourceUserControls = dataSourceUserControls.FindAll(p => p.v_ComponentFieldId.Contains("SOM"));
                                ((ucSomnolencia)ctrl).DataSource = new List<ServiceComponentFieldValuesList>();
                                ((ucSomnolencia)ctrl).DataSource = dataSourceUserControls;
                                breakHazChildrenUc = true;
                                break;
                            case (int)ControlType.UcAcumetria:
                                dataSourceUserControls = serviceComponentsInfo.ServiceComponentFields.SelectMany(p => p.ServiceComponentFieldValues).ToList();
                                dataSourceUserControls = dataSourceUserControls.FindAll(p => p.v_ComponentFieldId.Contains("ACU"));
                                ((ucAcumetria)ctrl).DataSource = new List<ServiceComponentFieldValuesList>();
                                ((ucAcumetria)ctrl).DataSource = dataSourceUserControls;
                                breakHazChildrenUc = true;
                                break;
                            case (int)ControlType.UcSintomaticoRespi:
                                dataSourceUserControls = serviceComponentsInfo.ServiceComponentFields.SelectMany(p => p.ServiceComponentFieldValues).ToList();
                                dataSourceUserControls = dataSourceUserControls.FindAll(p => p.v_ComponentFieldId.Contains("RES"));
                                ((ucSintomaticoResp)ctrl).DataSource = new List<ServiceComponentFieldValuesList>();
                                ((ucSintomaticoResp)ctrl).DataSource = dataSourceUserControls;
                                breakHazChildrenUc = true;
                                break;
                            case (int)ControlType.UcRxLumboSacra:
                                dataSourceUserControls = serviceComponentsInfo.ServiceComponentFields.SelectMany(p => p.ServiceComponentFieldValues).ToList();
                                dataSourceUserControls = dataSourceUserControls.FindAll(p => p.v_ComponentFieldId.Contains("RXL"));
                                ((ucRXLumboSacra)ctrl).DataSource = new List<ServiceComponentFieldValuesList>();
                                ((ucRXLumboSacra)ctrl).DataSource = dataSourceUserControls;
                                breakHazChildrenUc = true;
                                break;
                            case (int)ControlType.UcOjoSeco:
                                dataSourceUserControls = serviceComponentsInfo.ServiceComponentFields.SelectMany(p => p.ServiceComponentFieldValues).ToList();
                                dataSourceUserControls = dataSourceUserControls.FindAll(p => p.v_ComponentFieldId.Contains("OJS"));
                                ((ucOjoSeco)ctrl).DataSource = new List<ServiceComponentFieldValuesList>();
                                ((ucOjoSeco)ctrl).DataSource = dataSourceUserControls;
                                breakHazChildrenUc = true;
                                break;
                            case (int)ControlType.UcOtoscopia:
                                 dataSourceUserControls = serviceComponentsInfo.ServiceComponentFields.SelectMany(p => p.ServiceComponentFieldValues).ToList();
                                dataSourceUserControls = dataSourceUserControls.FindAll(p => p.v_ComponentFieldId.Contains("OTO"));
                                ((ucOtoscopia)ctrl).DataSource = new List<ServiceComponentFieldValuesList>();
                                ((ucOtoscopia)ctrl).DataSource = dataSourceUserControls;
                                breakHazChildrenUc = true;
                                break;
                            case (int)ControlType.UcEvaluacionErgonomica:
                                dataSourceUserControls = serviceComponentsInfo.ServiceComponentFields.SelectMany(p => p.ServiceComponentFieldValues).ToList();
                                dataSourceUserControls = dataSourceUserControls.FindAll(p => p.v_ComponentFieldId.Contains("EVA"));
                                ((ucEvaluacionErgonomica)ctrl).DataSource = new List<ServiceComponentFieldValuesList>();
                                ((ucEvaluacionErgonomica)ctrl).DataSource = dataSourceUserControls;
                                breakHazChildrenUc = true;
                                break;
                            case (int)ControlType.UcOsteoMuscular:
                                dataSourceUserControls = serviceComponentsInfo.ServiceComponentFields.SelectMany(p => p.ServiceComponentFieldValues).ToList();
                                dataSourceUserControls = dataSourceUserControls.FindAll(p => p.v_ComponentFieldId.Contains("OTS"));
                                ((ucOsteoMuscular)ctrl).DataSource = new List<ServiceComponentFieldValuesList>();
                                ((ucOsteoMuscular)ctrl).DataSource = dataSourceUserControls;
                                breakHazChildrenUc = true;
                                break;
                            case (int)ControlType.ucEspirometria:
                                dataSourceUserControls = serviceComponentsInfo.ServiceComponentFields.SelectMany(p => p.ServiceComponentFieldValues).ToList();
                                dataSourceUserControls = dataSourceUserControls.FindAll(p => p.v_ComponentFieldId.Contains("ESP"));
                                ((ucEspirometria)ctrl).DataSource = new List<ServiceComponentFieldValuesList>();
                                ((ucEspirometria)ctrl).DataSource = dataSourceUserControls;
                                breakHazChildrenUc = true;
                                break;
                            case (int)ControlType.ucOftalmologia:
                                dataSourceUserControls = serviceComponentsInfo.ServiceComponentFields.SelectMany(p => p.ServiceComponentFieldValues).ToList();
                                dataSourceUserControls = dataSourceUserControls.FindAll(p => p.v_ComponentFieldId.Contains("OFT"));
                                ((ucOftalmologia)ctrl).DataSource = new List<ServiceComponentFieldValuesList>();
                                ((ucOftalmologia)ctrl).DataSource = dataSourceUserControls;
                                breakHazChildrenUc = true;
                                break;
                            case (int)ControlType.ucPsicologia:
                                dataSourceUserControls = serviceComponentsInfo.ServiceComponentFields.SelectMany(p => p.ServiceComponentFieldValues).ToList();
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
                                breakHazChildrenUc = true;
                                break;
                            default:
                                foreach (var item in serviceComponentsInfo.ServiceComponentFields)
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
                                break;
                        }
                    }
                }

                if (!ctrl.HasChildren) continue;
                if (!breakHazChildrenUc && keyTagControl == null)
                {
                    SearchControlAndSetValue(ctrl, serviceComponentsInfo);
                }
            }
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

        private static void SetValueControl(int controlId, Control ctrl, string componentFieldsId, string tagComponentFieldsId, string value1, SiNo hasAutomaticDx)
        {
            switch ((ControlType)controlId)
            {
                //case ControlType.Fecha:
                //    if (componentFieldsId == tagComponentFieldsId)
                //    {
                //        ((DateTimePicker)ctrl).Value = Convert.ToDateTime(value1);
                //        if (hasAutomaticDx == SiNo.SI)
                //            ctrl.BackColor = Color.Pink;
                //        else
                //            ctrl.BackColor = Color.White;
                //    }
                //    break;
                case ControlType.CadenaTextual:
                    if (componentFieldsId == tagComponentFieldsId)
                    {
                        ((TextBox)ctrl).Text = value1;
                        if (hasAutomaticDx == SiNo.SI)
                            ctrl.BackColor = Color.Pink;
                        else
                            ctrl.BackColor = Color.White;

                    }
                    break;
                case ControlType.CadenaMultilinea:
                    if (componentFieldsId == tagComponentFieldsId)
                    {
                        ((TextBox)ctrl).Text = value1;
                        if (hasAutomaticDx == SiNo.SI)
                            ctrl.BackColor = Color.Pink;
                        else
                            ctrl.BackColor = Color.White;

                    }
                    break;
                case ControlType.NumeroEntero:
                    if (componentFieldsId == tagComponentFieldsId)
                    {
                        if (string.IsNullOrEmpty(value1))
                            value1 = "0";

                        ((UltraNumericEditor)ctrl).Value = value1;
                        if (hasAutomaticDx == SiNo.SI)
                            ctrl.BackColor = Color.Pink;
                        else
                            ctrl.BackColor = Color.White;
                    }
                    break;
                case ControlType.NumeroDecimal:
                    if (componentFieldsId == tagComponentFieldsId)
                    {
                        if (string.IsNullOrEmpty(value1))
                            value1 = "0";

                        ((UltraNumericEditor)ctrl).Value = value1;
                        if (hasAutomaticDx == SiNo.SI)
                            ctrl.BackColor = Color.Pink;
                        else
                            ctrl.BackColor = Color.White;
                    }
                    break;
                case ControlType.SiNoCheck:
                    if (componentFieldsId == tagComponentFieldsId)
                    {
                        ((CheckBox)ctrl).Checked = Convert.ToBoolean(int.Parse(value1));
                    }
                    break;
                case ControlType.SiNoRadioButton:
                    if (componentFieldsId == tagComponentFieldsId)
                    {
                        ((RadioButton)ctrl).Checked = Convert.ToBoolean(int.Parse(value1));
                    }
                    break;
                case ControlType.SiNoCombo:
                    if (componentFieldsId == tagComponentFieldsId)
                    {
                        ((ComboBox)ctrl).SelectedValue = value1;
                    }
                    break;
                case ControlType.Lista:
                    if (componentFieldsId == tagComponentFieldsId)
                    {
                        var cb = (ComboBox)ctrl;
                        cb.SelectedValue = value1;
                    }
                    break;
            }
        }
        
        private void SetDefaultValueAfterBuildMenu(ComponentList examen)
        {
            try
            {
                var findTab = tcExamList.Tabs[examen.v_ComponentId];

                foreach (ComponentFieldsList cf in examen.Fields)
                {
                    var ctrl = findTab.TabPage.Controls.Find(cf.v_ComponentFieldId, true);

                    if (ctrl.Length != 0)
                    {
                        #region Setear valor x defecto del control

                        switch ((ControlType)cf.i_ControlId)
                        {
                            //case ControlType.Fecha:
                            //    DateTimePicker dtp = (DateTimePicker)ctrl[0];
                            //    dtp.CreateControl();
                            //    dtp.Value = string.IsNullOrEmpty(cf.v_DefaultText) ? DateTime.Now : DateTime.Parse(cf.v_DefaultText);
                            //    break;
                            case ControlType.CadenaTextual:
                                TextBox txtt = (TextBox)ctrl[0];
                                txtt.CreateControl();
                                txtt.Text = cf.v_DefaultText;
                                txtt.BackColor = Color.White;
                                break;
                            case ControlType.CadenaMultilinea:
                                TextBox txtm = (TextBox)ctrl[0];
                                txtm.CreateControl();
                                txtm.Text = cf.v_DefaultText;
                                txtm.BackColor = Color.White;
                                break;
                            case ControlType.NumeroEntero:
                                UltraNumericEditor uni = (UltraNumericEditor)ctrl[0];
                                uni.CreateControl();
                                uni.Value = string.IsNullOrEmpty(cf.v_DefaultText) ? 0 : int.Parse(cf.v_DefaultText);
                                uni.BackColor = Color.White;
                                break;
                            case ControlType.NumeroDecimal:
                                UltraNumericEditor und = (UltraNumericEditor)ctrl[0];
                                und.CreateControl();
                                und.Value = string.IsNullOrEmpty(cf.v_DefaultText) ? 0 : double.Parse(cf.v_DefaultText);
                                und.BackColor = Color.White;
                                break;
                            case ControlType.SiNoCheck:
                                CheckBox chkSiNo = (CheckBox)ctrl[0];
                                chkSiNo.CreateControl();
                                chkSiNo.Checked = !string.IsNullOrEmpty(cf.v_DefaultText) && Convert.ToBoolean(int.Parse(cf.v_DefaultText));
                                break;
                            case ControlType.SiNoRadioButton:
                                RadioButton rbSiNo = (RadioButton)ctrl[0];
                                rbSiNo.CreateControl();
                                rbSiNo.Checked = !string.IsNullOrEmpty(cf.v_DefaultText) && Convert.ToBoolean(int.Parse(cf.v_DefaultText));
                                break;
                            case ControlType.SiNoCombo:
                                ComboBox cbSiNo = (ComboBox)ctrl[0];
                                cbSiNo.CreateControl();
                                cbSiNo.SelectedValue = string.IsNullOrEmpty(cf.v_DefaultText) ? "-1" : cf.v_DefaultText;
                                break;
                            case ControlType.UcFileUpload:
                                break;
                            case ControlType.Lista:
                                ComboBox cbList = (ComboBox)ctrl[0];
                                cbList.CreateControl();
                                cbList.SelectedValue = string.IsNullOrEmpty(cf.v_DefaultText) ? "-1" : cf.v_DefaultText;
                                break;
                            case ControlType.UcOdontograma:
                                //(UserControls.ucOdontograma).ClearValueControl();;
                                break;
                            case ControlType.UcAudiometria:
                                ((ucAudiometria)ctrl[0]).ClearValueControl();
                                break;
                            case ControlType.UcSomnolencia:
                                ((ucSomnolencia)ctrl[0]).ClearValueControl();
                                break;
                            case ControlType.UcAcumetria:
                                ((ucAcumetria)ctrl[0]).ClearValueControl();
                                break;
                            case ControlType.UcSintomaticoRespi:
                                ((ucSintomaticoResp)ctrl[0]).ClearValueControl();
                                break;
                            case ControlType.UcRxLumboSacra:
                                ((ucRXLumboSacra)ctrl[0]).ClearValueControl();
                                break;
                            case ControlType.UcOtoscopia:
                                ((ucOtoscopia)ctrl[0]).ClearValueControl();
                                break;
                            case ControlType.UcEvaluacionErgonomica:
                                ((ucEvaluacionErgonomica)ctrl[0]).ClearValueControl();
                                break;
                            case ControlType.UcOsteoMuscular:
                                ((ucOsteoMuscular)ctrl[0]).ClearValueControl();
                                break;
                          
                            case ControlType.UcOjoSeco:
                                ((ucOjoSeco)ctrl[0]).ClearValueControl();
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

        private int CreateCampoComponente(ComponentList component, ComponentFieldsList field, int groupboxIColumn, int nroControlNet, TableLayoutPanel tableLayoutPanel, UltraValidator ultraValidator)
        {
            CreateLabelField(field, groupboxIColumn, nroControlNet, tableLayoutPanel);
            nroControlNet++;

            CreateControl(component, field, groupboxIColumn, nroControlNet, tableLayoutPanel, ultraValidator);
            nroControlNet++;

            CreateUnidadMedida(field, groupboxIColumn, nroControlNet, tableLayoutPanel);
            nroControlNet++;

            return nroControlNet;
        }

        private static void CreateUnidadMedida(ComponentFieldsList field, int groupboxIColumn, int nroControlNet, TableLayoutPanel tableLayoutPanel)
        {
            var labelUnidadMedida = LabelUnidadMedida(field);
            var fila = RedondeoMayor(nroControlNet, groupboxIColumn * Constants.COLUMNAS_POR_CONTROL);
            var columna = nroControlNet - (fila - 1) * (groupboxIColumn * Constants.COLUMNAS_POR_CONTROL);
            tableLayoutPanel.Controls.Add(labelUnidadMedida, columna - 1, fila - 1);
        }

        private void CreateControl(ComponentList component, ComponentFieldsList field, int column, int nroControlNet, TableLayoutPanel tableLayoutPanel, UltraValidator ultraValidator)
        {
            if (component.v_ComponentId == "N009-ME000000086")
            {
                
            }
            var control = CreateControlInEso(field, component, ultraValidator);
            var fila = RedondeoMayor(nroControlNet, column * Constants.COLUMNAS_POR_CONTROL);
            var columna = nroControlNet - (fila - 1) * (column * Constants.COLUMNAS_POR_CONTROL);
            tableLayoutPanel.Controls.Add(control, columna - 1, fila - 1);
        }

        private static Label LabelUnidadMedida(ComponentFieldsList field)
        {
            var lbl1 = new Label
            {
                AutoSize = false,
                Width = 50,
                Text = field.v_MeasurementUnitName
            };
            lbl1.Font = new Font(lbl1.Font, FontStyle.Bold | FontStyle.Italic);
            lbl1.TextAlign = ContentAlignment.BottomLeft;

            return lbl1;
        }

        private Control CreateControlInEso(ComponentFieldsList field, ComponentList component, UltraValidator validator)
        {
            var esMayuscula = int.Parse(Common.Utils.GetApplicationConfigValue("EsMayuscula"));
            UltraNumericEditor une;
            TextBox txt;
            Control ctl = null;

            switch ((ControlType)field.i_ControlId)
            {
                #region Creacion del control
                //case ControlType.Fecha:
                //    var dtp = new DateTimePicker();
                //    dtp.Width = field.i_ControlWidth;
                //    dtp.Height = field.i_HeightControl;
                //    dtp.Name = field.v_ComponentFieldId;
                //    dtp.Format = DateTimePickerFormat.Custom;
                //    dtp.CustomFormat = @"dd/MM/yyyy";
                //    ctl = dtp;
                //    break;

                case ControlType.CadenaTextual:
                    txt = new TextBox();
                    txt.Width = field.i_ControlWidth;
                    txt.Height = field.i_HeightControl;
                    txt.MaxLength = field.i_MaxLenght;
                    txt.Name = field.v_ComponentFieldId;
                    txt.CharacterCasing = esMayuscula == 1 ? CharacterCasing.Upper : CharacterCasing.Normal;

                    if (field.i_IsCalculate == (int)SiNo.SI)
                    {
                        txt.Enabled = false;
                    }
                    else
                    {
                        txt.Leave += txt_Leave;
                    }

                    if (field.i_IsRequired == (int)SiNo.SI)
                        SetControlValidate(field.i_ControlId, txt, null, null, validator);

                    txt.Enter += Capture_Value;

                    //AMC
                    if (field.i_Enabled == (int)SiNo.NO)
                    {
                        txt.Enabled = true;
                    }
                    else
                    {
                        txt.Enabled = false;
                    }


                    if (field.i_ReadOnly == (int)SiNo.SI)
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
                        Width = field.i_ControlWidth,
                        Height = field.i_HeightControl,
                        Multiline = true,
                        MaxLength = field.i_MaxLenght,
                        ScrollBars = ScrollBars.Vertical,
                        Name = field.v_ComponentFieldId
                    };

                    //AMC
                    if (field.i_Enabled == (int)SiNo.NO)
                    {
                        txt.Enabled = true;
                    }
                    else
                    {
                        txt.Enabled = false;
                    }


                    if (field.i_ReadOnly == (int)SiNo.SI)
                    {
                        txt.ReadOnly = true;
                    }
                    else
                    {
                        txt.ReadOnly = false;
                    }

                    txt.CharacterCasing = esMayuscula == 1 ? CharacterCasing.Upper : CharacterCasing.Normal;

                    txt.Enter += Capture_Value;
                    txt.Leave += txt_Leave;

                    if (field.i_IsRequired == (int)SiNo.SI)
                        SetControlValidate(field.i_ControlId, txt, null, null, validator);

                    if (_action == "View")
                    {
                        txt.ReadOnly = true;
                    }

                    ctl = txt;
                    break;
                case ControlType.NumeroEntero:
                    une = new UltraNumericEditor()
                    {
                        Width = field.i_ControlWidth,
                        Height = field.i_HeightControl,
                        NumericType = NumericType.Integer,
                        PromptChar = ' ',
                        Name = field.v_ComponentFieldId,
                        MaskDisplayMode = MaskMode.Raw

                    };

                    // Asociar el control a un evento
                    une.Enter += Capture_Value;

                    if (field.i_IsCalculate == (int)SiNo.SI)
                    {
                        une.ReadOnly = true;
                        une.ValueChanged += txt_ValueChanged;
                    }
                    else
                    {
                        une.Leave += txt_Leave;
                    }

                    if (field.i_IsRequired == (int)SiNo.SI)
                    {
                        // Establecer condición por rangos
                        SetControlValidate(field.i_ControlId, une, field.r_ValidateValue1, field.r_ValidateValue2, validator);
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
                        Width = field.i_ControlWidth,
                        Height = field.i_HeightControl,
                        PromptChar = ' ',
                        Name = field.v_ComponentFieldId,
                        NumericType = NumericType.Double,
                        MaskDisplayMode = MaskMode.Raw

                    };

                    if (field.i_NroDecimales == 1)
                    {
                        une.MaskInput = "nnnnn.n";
                    }
                    else if (field.i_NroDecimales == 2)
                    {
                        une.MaskInput = "nnnnn.nn";
                    }
                    else if (field.i_NroDecimales == 3)
                    {
                        une.MaskInput = "nnnnn.nnn";
                    }
                    else if (field.i_NroDecimales == 4)
                    {
                        une.MaskInput = "nnnnn.nnnn";
                    }




                    // Asociar el control a un evento
                    une.Enter += Capture_Value;

                    if (field.i_IsCalculate == (int)SiNo.SI)
                    {
                        une.ValueChanged += txt_ValueChanged;
                        une.ReadOnly = true;
                    }
                    else
                    {
                        une.Leave += txt_Leave;
                    }

                    if (field.i_IsRequired == (int)SiNo.SI)
                    {
                        // Establecer condición por rangos                                                              
                        SetControlValidate(field.i_ControlId, une, field.r_ValidateValue1, field.r_ValidateValue2, validator);
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
                        Width = field.i_ControlWidth,
                        Height = field.i_HeightControl,
                        Text = @"Si/No",
                        Name = field.v_ComponentFieldId,
                    };

                    ctl.Enter += Capture_Value;
                    ctl.Leave += txt_Leave;

                    if (_action == "View")
                    {
                        ctl.Enabled = false;
                    }

                    break;
                case ControlType.SiNoRadioButton:
                    ctl = new RadioButton()
                    {
                        Width = field.i_ControlWidth,
                        Height = field.i_HeightControl,
                        Text = @"Si/No",
                        Name = field.v_ComponentFieldId
                    };

                    ctl.Enter += Capture_Value;
                    ctl.Leave += txt_Leave;

                    if (_action == "View")
                    {
                        ctl.Enabled = false;
                    }

                    break;
                case ControlType.SiNoCombo:
                    ctl = new ComboBox()
                    {
                        Width = field.i_ControlWidth,
                        Height = field.i_HeightControl,
                        DropDownStyle = ComboBoxStyle.DropDownList,
                        Name = field.v_ComponentFieldId
                    };
                    var data = _cboEso.Find(p => p.Id == field.i_GroupId.ToString()).Items;
                    var list = ConvertClassToStruct(data);
                    Utils.LoadDropDownList((ComboBox)ctl, "Value1", "Id", list, DropDownListAction.Select);

                    if (field.i_IsRequired == (int)SiNo.SI)
                    {
                        SetControlValidate(field.i_ControlId, ctl, null, null, validator);
                    }

                    ctl.Enter += Capture_Value;
                    ctl.Leave += txt_Leave;

                    if (_action == "View")
                    {
                        ctl.Enabled = false;
                    }

                    break;
                #region ...
                case ControlType.UcFileUpload:
                    var ucFileUpload = new ucFileUpload
                    {
                        PersonId = _personId,
                        Name = field.v_ComponentFieldId
                    };
                    //ucFileUpload.Dni = _Dni;
                    //ucFileUpload.Fecha = lblFecInicio.Text;
                    //ucFileUpload.Consultorio = com.v_CategoryName;
                    //ucFileUpload.ServiceComponentId = com.v_ServiceComponentId;

                    //ctl = new Sigesoft.Node.WinClient.UI.UserControls.ucFileUpload();
                    ctl = ucFileUpload;
                    break;
                case ControlType.UcOdontograma:
                    var ucOdontograma = new ucOdontograma {Name = field.v_ComponentFieldId};
                    ctl = ucOdontograma;
                    break;
                case ControlType.UcAudiometria:
                    var ucAudiometria = new ucAudiometria
                    {
                        Name = field.v_ComponentFieldId,
                        PersonId = _personId,
                        ServiceComponentId = component.v_ServiceComponentId
                    };
                    ctl = ucAudiometria;
                    break;
                case ControlType.UcSomnolencia:
                    var ucSomnolencia = new ucSomnolencia {Name = field.v_ComponentFieldId};
                    ctl = ucSomnolencia;
                    break;

                case ControlType.UcBoton:
                    var ucBoton = new ucBoton
                    {
                        Name = field.v_ComponentFieldId,
                        Examen = component.v_Name
                    };
                    //ucBoton.Dni = _Dni;
                    //ucBoton.FechaServicio = _FechaServico.Value;
                    ctl = ucBoton;
                    break;

                case ControlType.UcAcumetria:
                    var ucAcumetria = new ucAcumetria {Name = field.v_ComponentFieldId};
                    ctl = ucAcumetria;
                    break;
                case ControlType.UcSintomaticoRespi:
                    var ucSintomaticoRespi = new ucSintomaticoResp {Name = field.v_ComponentFieldId};
                    ctl = ucSintomaticoRespi;
                    break;
                case ControlType.UcRxLumboSacra:
                    var ucRxLumboSacra = new ucRXLumboSacra {Name = field.v_ComponentFieldId};
                    ctl = ucRxLumboSacra;
                    break;               
                case ControlType.UcOjoSeco:
                    var ucOjoSeco = new ucOjoSeco {Name = field.v_ComponentFieldId};
                    ctl = ucOjoSeco;
                    break;

                case ControlType.UcOtoscopia:
                    var ucOtoscopia = new ucOtoscopia {Name = field.v_ComponentFieldId};
                    ctl = ucOtoscopia;
                    break;

                case ControlType.UcEvaluacionErgonomica:
                    var ucEvaluacionErgonomica = new ucEvaluacionErgonomica {Name = field.v_ComponentFieldId};
                    ctl = ucEvaluacionErgonomica;
                    break;

                case ControlType.UcOsteoMuscular:
                    var ucOsteo = new ucOsteoMuscular {Name = field.v_ComponentFieldId};
                    ctl = ucOsteo;
                    break;


                #endregion
                case ControlType.ucEspirometria:
                    var ucEspirometria = new Sigesoft.Node.WinClient.UI.UserControls.ucEspirometria();
                    ucEspirometria.Name = field.v_ComponentFieldId;
                    // Establecer evento
                    ucEspirometria.AfterValueChange += new EventHandler<AudiometriaAfterValueChangeEventArgs>(ucAudiometria_AfterValueChange);
                    ctl = ucEspirometria;
                    break;
                case ControlType.ucOftalmologia:
                    var ucOftalmologia = new Sigesoft.Node.WinClient.UI.UserControls.ucOftalmologia();
                    ucOftalmologia.Name = field.v_ComponentFieldId;
                    ucOftalmologia.Age = _age;
                    // Establecer evento
                    ucOftalmologia.AfterValueChange += new EventHandler<AudiometriaAfterValueChangeEventArgs>(ucAudiometria_AfterValueChange);
                    ctl = ucOftalmologia;
                    break;
                case ControlType.ucPsicologia:
                    if (_esoTypeId == TypeESO.PreOcupacional)
                    {
                        var ucPsicologiaEMPO = new Sigesoft.Node.WinClient.UI.UserControls.ucPsychologicalExam(_esoTypeId);
                        ucPsicologiaEMPO.Name = field.v_ComponentFieldId;

                        // Establecer evento
                        ucPsicologiaEMPO.AfterValueChange += new EventHandler<AudiometriaAfterValueChangeEventArgs>(ucAudiometria_AfterValueChange);
                        ctl = ucPsicologiaEMPO;
                    }
                    else if (_esoTypeId == TypeESO.PeriodicoAnual)
                    {
                        var ucPsicologiaEMOA = new Sigesoft.Node.WinClient.UI.UserControls.ucPsychologicalExamAnual(_esoTypeId);
                        ucPsicologiaEMOA.Name = field.v_ComponentFieldId;

                        // Establecer evento
                        ucPsicologiaEMOA.AfterValueChange += new EventHandler<AudiometriaAfterValueChangeEventArgs>(ucAudiometria_AfterValueChange);
                        ctl = ucPsicologiaEMOA;
                    }
                    break;
                case ControlType.Lista:
                    var cb = new ComboBox()
                    {
                        Width = field.i_ControlWidth,
                        Height = field.i_HeightControl,
                        DropDownStyle = ComboBoxStyle.DropDownList,
                        Name = field.v_ComponentFieldId
                    };
                    var dataLista = _cboEso.Find(p => p.Id == field.i_GroupId.ToString()).Items;
                    var Listas = ConvertClassToStruct(dataLista);
                    Utils.LoadDropDownList(cb, "Value1", "Id", Listas, DropDownListAction.Select);

                    if (field.i_IsRequired == (int)SiNo.SI)
                    {
                        SetControlValidate(field.i_ControlId, cb, null, null, validator);
                    }

                    // Setear levantamiento de popup para el ingreso de los hallazgos solo cuando 
                    // se seleccione un valor alterado

                    if ((field.v_ComponentId == Constants.EXAMEN_FISICO_ID
                                                || field.v_ComponentId == Constants.RX_ID
                                                || field.v_ComponentId == Constants.OFTALMOLOGIA_ID
                                                || field.v_ComponentId == Constants.ALTURA_ESTRUCTURAL_ID
                                                || field.v_ComponentId == Constants.TACTO_RECTAL_ID
                                                || field.v_ComponentId == Constants.EVAL_NEUROLOGICA_ID
                                                || field.v_ComponentId == Constants.TEST_ROMBERG_ID
                                                || field.v_ComponentId == Constants.TAMIZAJE_DERMATOLOGIO_ID
                                                || field.v_ComponentId == Constants.GINECOLOGIA_ID
                                                || field.v_ComponentId == Constants.EXAMEN_MAMA_ID
                                                || field.v_ComponentId == Constants.AUDIOMETRIA_ID
                                                || field.v_ComponentId == Constants.ELECTROCARDIOGRAMA_ID
                                                || field.v_ComponentId == Constants.ESPIROMETRIA_ID
                                                || field.v_ComponentId == Constants.OSTEO_MUSCULAR_ID
                                                || field.v_ComponentId == Constants.PRUEBA_ESFUERZO_ID
                                                || field.v_ComponentId == Constants.TAMIZAJE_DERMATOLOGIO_ID
                                                || field.v_ComponentId == Constants.ODONTOGRAMA_ID
                                                || field.v_ComponentId == Constants.EXAMEN_FISICO_7C_ID)
                                                && (field.i_GroupId == (int)SystemParameterGroups.ConHallazgoSinHallazgosNoSeRealizo))
                    {
                        cb.SelectedIndexChanged += new EventHandler(cb_SelectedIndexChanged);
                    }

                    cb.Enter += Capture_Value;
                    cb.Leave += txt_Leave;

                    if (_action == "View")
                    {
                        cb.Enabled = false;
                    }

                    ctl = cb;
                    break;

                #endregion
            }

            if (ctl != null)
                ctl.Tag = new KeyTagControl
                {
                    i_ControlId = field.i_ControlId,
                    v_ComponentId = field.v_ComponentId,
                    v_ComponentFieldsId = field.v_ComponentFieldId,
                    i_IsSourceFieldToCalculate = field.i_IsSourceFieldToCalculate,
                    v_Formula = field.v_Formula,
                    v_TargetFieldOfCalculateId = field.v_TargetFieldOfCalculateId,
                    v_SourceFieldToCalculateJoin = field.v_SourceFieldToCalculateJoin,
                    v_FormulaChild = field.v_FormulaChild,
                    Formula = field.Formula,
                    TargetFieldOfCalculateId = field.TargetFieldOfCalculateId,
                    v_TextLabel = field.v_TextLabel,
                    v_ComponentName = component.v_Name
                };

            return ctl;
        }

        private static List<StructKeyDto> ConvertClassToStruct(IEnumerable<KeyValueDTO> data)
        {
            return data.Select(item => new StructKeyDto
            {
                Value1 = item.Value1, Id = item.Id
            }).ToList();
        }
        
        public  struct StructKeyDto
        {
            public string Id { get; set; }
            public string Value1 { get; set; }
            public string Value2 { get; set; }
            public string Value3 { get; set; }
            public float Value4 { get; set; }
            public int GrupoId { get; set; }
            public int ParameterId { get; set; }
            public string Field { get; set; }
            public int ParentId { get; set; }
        }

        private static void CreateLabelField(ComponentFieldsList fields, int column, int nroControlNet, TableLayoutPanel tableLayoutPanel)
        {
            var lbl = CreateLabel(fields);
            var fila = RedondeoMayor(nroControlNet, column * Constants.COLUMNAS_POR_CONTROL);
            var columna = nroControlNet - (fila - 1) * (column * Constants.COLUMNAS_POR_CONTROL);
            tableLayoutPanel.Controls.Add(lbl, columna - 1, fila - 1);
        }

        private static Label CreateLabel(ComponentFieldsList fields)
        {
            var lbl = new Label
            {
                Text = fields.v_TextLabel,
                Width = fields.i_LabelWidth,
                TextAlign = ContentAlignment.BottomRight,
                AutoSize = false
            };
            lbl.Font = new Font(lbl.Font.FontFamily.Name, 7.25F);

            return lbl;
        }

        private static IEnumerable<ComponentFieldsList> GetFieldsEachGroups(ComponentList component, ComponentFieldsList groupbox, ComponentList groupComponentName)
        {
            return component.Fields.FindAll(p => p.v_Group == groupbox.v_Group && p.v_ComponentId == groupComponentName.v_ComponentId);

        }

        private UltraTab CrearTab(ComponentList component)
        {
            var ultraTab = new UltraTab
            {
                Text = component.v_Name,
                Key = component.v_ComponentId,
                Tag = component.v_ServiceComponentId,
                ToolTipText = component.v_Name + @" / " + component.v_ServiceComponentId + @" / " + component.v_ComponentId
            };
            if (component.i_ServiceComponentStatusId == (int)ServiceComponentStatus.Culminado) ultraTab.Appearance.BackColor = Color.Pink;
            tcExamList.Tabs.Add(ultraTab);

            return ultraTab;
        }

        private static bool VisibleTab(ComponentList component)
        {
            var readingPermission = UserPermission();
            return readingPermission.Find(p => component.v_ComponentId.Contains(p.v_ComponentId)) != null;
        }

        private static TableLayoutPanel CreateTableLayoutPanelParent(UltraTab tab)
        {
            var tblpParent = new TableLayoutPanel
            {
                Name = "tblpParent",
                ColumnCount = 1,
                RowCount = new List<ComponentFieldsList>().Count
            };
            tab.TabPage.Controls.Add(tblpParent);
            return tblpParent;
        }

        private UltraValidator CreateValidators(ComponentList component)
        {
            var uv = CreateUltraValidatorByComponentId(component.v_ComponentId);
            return uv;
        }

        private static GroupBox CreateGroupBoxComponent(ComponentList groupBox)
        {
            var gbGroupedComponent = new GroupBox
            {
                Text = groupBox.v_GroupedComponentName,
                Name = "gb_" + groupBox.v_GroupedComponentName,
                BackColor = Color.LightCyan,
                AutoSize = true,
                Dock = DockStyle.Top
            };
            return gbGroupedComponent;
        }

        private static TableLayoutPanel CreateTableLayoutForControls(ComponentFieldsList groupbox, ComponentList component)
        {
            var tblpGroup = new TableLayoutPanel
            {
                Name = "tblpGroup_" + groupbox.v_Group,
                ColumnCount = groupbox.i_Column * Constants.COLUMNAS_POR_CONTROL,
                RowCount = RedondeoMayor(component.Fields.Count, groupbox.i_Column),
                Dock = DockStyle.Fill,
                AutoSize = true
            };
            return tblpGroup;
        }

        private static int RedondeoMayor(int a, int b)
        {
            return (int)Math.Ceiling(a / (double)b);
        }

        private static GroupBox CreateGroupBoxForGroup(ComponentFieldsList groupbox)
        {
            var groupBox = new GroupBox
            {
                Text = groupbox.v_Group,
                Name = "gb_" + groupbox.v_Group,
                BackColor = Color.Azure,
                AutoSize = true,
                Dock = DockStyle.Top
            };
            return groupBox;
        }

        private static IEnumerable<ComponentFieldsList> GetGroupBoxes(ComponentList component, string componentId)
        {
            try
            {
                var fieldsByComponent = component.Fields
                                      .ToList()
                                      .FindAll(p => p.v_ComponentId == componentId);

                var groupBoxes = fieldsByComponent.GroupBy(e => new { e.v_Group }).Select(g => g.First()).OrderBy(o => o.v_Group).ToList();

                return groupBoxes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private static TableLayoutPanel CreateTableLayoutPanelChildren(ComponentList groupComponentName)
        {
            TableLayoutPanel tblpGroupedComponent = new TableLayoutPanel
            {
                Name = "tblpGroup_" + groupComponentName.v_GroupedComponentName,
                ColumnCount = 1,
                RowCount = 1,
                Dock = DockStyle.Fill,
                AutoSize = true
            };
            return tblpGroupedComponent;
        }

        private UltraValidator CreateUltraValidatorByComponentId(string componentId)
        {
            var uv = new UltraValidator(components);

            if (_dicUltraValidators == null)
                _dicUltraValidators = new Dictionary<string, UltraValidator>();

            _dicUltraValidators.Add(componentId, uv);

            return uv;
        }

        private void InitializeStaticControls()
        {
            LoadComboBox();
            FormActions();
        }

        private void FormActions()
        {
            _formActions = BLL.Utils.SetFormActionsInSession("frmEso", _nodeId, _roleId, _userId);
            btnGuardarExamen.Enabled = BLL.Utils.IsActionEnabled("frmEso_EXAMENES_SAVE", _formActions);
            btnAgregarTotalDiagnostico.Enabled = BLL.Utils.IsActionEnabled("frmEso_ANADX_ADDDX", _formActions);
            _removerTotalDiagnostico = BLL.Utils.IsActionEnabled("frmEso_ANADX_REMOVEDX", _formActions);
            btnAgregarRecomendaciones_AnalisisDx.Enabled = BLL.Utils.IsActionEnabled("frmEso_ANADX_ADDRECOME", _formActions);
            _removerRecomendacionAnalisisDx = BLL.Utils.IsActionEnabled("frmEso_ANADX_REMOVERECOME", _formActions);
            btnAgregarRestriccion_Analisis.Enabled = BLL.Utils.IsActionEnabled("frmEso_ANADX_ADDRESTRIC", _formActions);
            _removerRestriccionAnalisis = BLL.Utils.IsActionEnabled("frmEso_ANADX_REMOVERESTRIC", _formActions);
            btnAceptarDX.Enabled = BLL.Utils.IsActionEnabled("frmEso_ANADX_SAVE", _formActions);

            if (btnAceptarDX.Enabled) return;
            cbCalificacionFinal.Enabled = false;
            cbTipoDx.Enabled = false;
            cbEnviarAntecedentes.Enabled = false;
            dtpFechaVcto.Enabled = false;
        }

        private void LoadData()
        {
            var dataService = GetServiceData(_serviceId);
            _cancelEventSelectedIndexChange = true;
            LoadTabAnamnesis(dataService);
            _cancelEventSelectedIndexChange = false;
            LoadTabExamenes(dataService);
            LoadTabControlCalidad();
            LoadTabAptitud();
        }

        private static void LoadTabAptitud()
        {
            //throw new NotImplementedException();
        }

        private void LoadTabControlCalidad()
        {
            List<DiagnosticRepositoryList> tmpTotalDiagnosticByServiceIdList = null;
            Task.Factory.StartNew(() =>
            {
                tmpTotalDiagnosticByServiceIdList = new ServiceBL().GetServiceComponentDisgnosticsByServiceId(ref _objOperationResult, _serviceId);
            }).ContinueWith(t =>
            {
                grdTotalDiagnosticos.DataSource = tmpTotalDiagnosticByServiceIdList;
                lblRecordCountTotalDiagnosticos.Text = string.Format("Se encontraron {0} registros.", tmpTotalDiagnosticByServiceIdList.Count());
                SetCurrentRow();

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private int? _rowIndex;

        private void SetCurrentRow()
        {
            if (_rowIndex == null || grdTotalDiagnosticos.Rows.Count == 0) return;
            var rowCount = grdTotalDiagnosticos.Rows.Count;
            var rows = rowCount - _rowIndex.Value;
            int i;
            if (rows != 0)
                i = _rowIndex.Value;
            else
                i = _rowIndex.Value - 1;

            grdTotalDiagnosticos.Rows[i].Selected = true;
            grdTotalDiagnosticos.ActiveRowScrollRegion.ScrollRowIntoView(grdTotalDiagnosticos.Rows[i]);
        }

        private ServiceData GetServiceData(string serviceId)
        {
            return new ServiceBL().GetServiceData(ref _objOperationResult, serviceId);
        }

        private void LoadTabAnamnesis(ServiceData dataAnamnesis)
        {
            _personId = dataAnamnesis.PersonId;
            chkPresentaSisntomas.Checked = Convert.ToBoolean(dataAnamnesis.HasSymptomId);
            txtSintomaPrincipal.Text = string.IsNullOrEmpty(dataAnamnesis.MainSymptom) ? "No Refiere" : dataAnamnesis.MainSymptom;
            txtValorTiempoEnfermedad.Enabled = chkPresentaSisntomas.Checked;
            txtValorTiempoEnfermedad.Text = dataAnamnesis.TimeOfDisease == null ? string.Empty : dataAnamnesis.TimeOfDisease.ToString();
            cbCalendario.Enabled = chkPresentaSisntomas.Checked;
            cbCalendario.SelectedValue = dataAnamnesis.TimeOfDiseaseTypeId == null ? "1" : dataAnamnesis.TimeOfDiseaseTypeId.ToString();
            txtRelato.Text = string.IsNullOrEmpty(dataAnamnesis.Story) ? "Paciente Asintomático" : dataAnamnesis.Story;
            cbSueño.SelectedValue = dataAnamnesis.DreamId == null ? "1" : dataAnamnesis.DreamId.ToString();
            cbApetito.SelectedValue = dataAnamnesis.AppetiteId == null ? "1" : dataAnamnesis.AppetiteId.ToString();
            cbDeposiciones.SelectedValue = dataAnamnesis.DepositionId == null ? "1" : dataAnamnesis.DepositionId.ToString();
            cbOrina.SelectedValue = dataAnamnesis.UrineId == null ? "1" : dataAnamnesis.UrineId.ToString();
            cbSed.SelectedValue = dataAnamnesis.ThirstId == null ? "1" : dataAnamnesis.ThirstId.ToString();
            txtHallazgos.Text = string.IsNullOrEmpty(dataAnamnesis.Findings) ? "Sin Alteración" : dataAnamnesis.Findings;
            txtMenarquia.Text = dataAnamnesis.Menarquia;
            txtGestapara.Text = string.IsNullOrEmpty(dataAnamnesis.Gestapara) ? "G ( )  P ( ) ( ) ( ) ( ) " : dataAnamnesis.Gestapara;
            cbMac.SelectedValue = dataAnamnesis.MacId == null ? "1" : dataAnamnesis.MacId.ToString();
            txtRegimenCatamenial.Text = dataAnamnesis.CatemenialRegime;
            txtCiruGine.Text = dataAnamnesis.CiruGine;
            txtFecVctoGlobal.Text = dataAnamnesis.GlobalExpirationDate == null ? "NO REQUIERE" : dataAnamnesis.GlobalExpirationDate.Value.ToShortDateString();
            cbAptitudEso.SelectedValue = dataAnamnesis.AptitudeStatusId.ToString();
              
            if (dataAnamnesis.Pap != null) dtpPAP.Value = dataAnamnesis.Pap.Value;
            if (dataAnamnesis.Pap != null) dtpPAP.Checked = true;
            if (dataAnamnesis.Fur != null) dtpFur.Value = dataAnamnesis.Fur.Value;
            if (dataAnamnesis.Fur != null) dtpFur.Checked = true;
            if (dataAnamnesis.Mamografia != null) dtpMamografia.Value = dataAnamnesis.Mamografia.Value;
            if (dataAnamnesis.Mamografia != null) dtpMamografia.Checked = true;
            _sexType = (Gender)dataAnamnesis.SexTypeId;
            BlockControlsByGender(dataAnamnesis.SexTypeId);
            ConsolidadoAntecedentes(dataAnamnesis.PersonId);
            HistorialServiciosAnteriores(dataAnamnesis.PersonId, dataAnamnesis.ServiceId);
        }

        private void HistorialServiciosAnteriores(string personId, string serviceId)
        {
            var services = new ServiceBL().GetServicesConsolidateForService(ref _objOperationResult, personId, serviceId);
            grdServiciosAnteriores.DataSource = services;
            GetValueResult(_objOperationResult);
        }

        private void ConsolidadoAntecedentes(string personId)
        {
            GetAntecedentConsolidateForService(personId);
        }

        private void GetAntecedentConsolidateForService(string personId)
        {
            var antecedent = new ServiceBL().GetAntecedentConsolidateForService(ref _objOperationResult, personId);
            if (antecedent == null) return;
            grdAntecedentes.DataSource = antecedent;
            GetValueResult(_objOperationResult);
        }

        private static void GetValueResult(OperationResult objOperationResult)
        {
            if (objOperationResult.Success != 1)
            {
                MessageBox.Show(Constants.GenericErrorMessage, "¡SE OBTUVO UN ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BlockControlsByGender(int? sexTypeId)
        {
            if (sexTypeId != null)
                switch ((Gender)sexTypeId)
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
                }
        }

        private void LoadComboBox()
        {
            var listDataForCombo = BLL.Utils.GetSystemParameterForComboForm(ref _objOperationResult, "frmEso");
            var data124 = listDataForCombo.Find(p => p.Id == "124").Items;
            Utils.LoadDropDownList(cbAptitudEso, "Value1", "Id", ConvertClassToStruct(data124), DropDownListAction.Select);
            var data133 = listDataForCombo.Find(p => p.Id == "133").Items;
            Utils.LoadDropDownList(cbCalendario, "Value1", "Id", ConvertClassToStruct(data133), DropDownListAction.Select);
            var data183 = listDataForCombo.Find(p => p.Id == "183").Items;
            Utils.LoadDropDownList(cbSueño, "Value1", "Id", ConvertClassToStruct(data183), DropDownListAction.Select);
            Utils.LoadDropDownList(cbOrina, "Value1", "Id", ConvertClassToStruct(data183), DropDownListAction.Select);
            Utils.LoadDropDownList(cbDeposiciones, "Value1", "Id", ConvertClassToStruct(data183), DropDownListAction.Select);
            Utils.LoadDropDownList(cbApetito, "Value1", "Id", ConvertClassToStruct(data183), DropDownListAction.Select);
            Utils.LoadDropDownList(cbSed, "Value1", "Id", ConvertClassToStruct(data183), DropDownListAction.Select);
            var data134 = listDataForCombo.Find(p => p.Id == "134").Items;
            Utils.LoadDropDownList(cbMac, "Value1", "Id", ConvertClassToStruct(data134), DropDownListAction.Select);
            var serviceComponent = listDataForCombo.Find(p => p.Id == "127").Items;
            Utils.LoadDropDownList(cbEstadoComponente, "Value1", "Id", serviceComponent.FindAll(p => p.Id != ((int)ServiceComponentStatus.PorIniciar).ToString()));
            Utils.LoadDropDownList(cbTipoProcedenciaExamen, "Value1", "Id", listDataForCombo.Find(p => p.Id == "132").Items);
            cbTipoProcedenciaExamen.SelectedValue = Convert.ToInt32(ComponenteProcedencia.Interno).ToString();
            var data138 = listDataForCombo.Find(p => p.Id == "138").Items;
            Utils.LoadDropDownList(cbCalificacionFinal, "Value1", "Id", ConvertClassToStruct(data138), DropDownListAction.Select);
            var data139 = listDataForCombo.Find(p => p.Id == "139").Items;
            Utils.LoadDropDownList(cbTipoDx, "Value1", "Id", ConvertClassToStruct(data139), DropDownListAction.Select);
            var data111 = listDataForCombo.Find(p => p.Id == "111").Items;
            Utils.LoadDropDownList(cbEnviarAntecedentes, "Value1", "Id", ConvertClassToStruct(data111), DropDownListAction.Select);

        }

        private void ViewMode(string mode)
        {
            if ("View" == mode)
            {
                OnlyRead();
            }
            else if ("Service" == mode)
            {
                 tcSubMain.SelectedIndex = 3;
            }
                
        }

        private void OnlyRead()
        {
            //Anamnesis
            gbSintomasySignos.Enabled = false;
            gbFuncionesBiologicas.Enabled = false;
            btnGuardarAnamnesis.Enabled = false;
            //Examenes
            gbDiagnosticoExamen.Enabled = false;
            txtComentario.Enabled = false;
            cbEstadoComponente.Enabled = false;
            cbTipoProcedenciaExamen.Enabled = false;
            //Diagnósticos
            gbTotalDiagnostico.Enabled = false;
            gbEdicionDiagnosticoTotal.Enabled = false;
            btnAceptarDX.Enabled = false;
        }

        private void btnViewWorker_Click(object sender, EventArgs e)
        {
            var frmWorkerData = new FrmWorkerData(_serviceId);
            frmWorkerData.ShowDialog();
        }

        private void lblView_Click(object sender, EventArgs e)
        {
            var lbl = sender as Label;
            if (lbl != null && lbl.Text == "Ver Diagnósticos ...")
            {
                splitContainer2.SplitterDistance = splitContainer2.Height - 190;
                lbl.Text = string.Format("{0}", "Ver menos");
                lbl.ForeColor = Color.Blue;
            }
            else
            {
                if (lbl == null) return;
                splitContainer2.SplitterDistance = splitContainer2.Height;
                lbl.Text = "Ver Diagnósticos ...";
                lbl.ForeColor = Color.Red;
            }

        }

        private static void SetControlValidate(int controlId, Control ctrl, float? validateValue1, float? validateValue2, UltraValidator uv)
        {
            // Objetos para validar
            RangeCondition rc;
            ValidationSettings vs;

            uv.ErrorAppearance.BackColor = Color.FromArgb(255, 255, 192);
            uv.ErrorAppearance.BackGradientStyle = GradientStyle.Vertical;
            uv.ErrorAppearance.BorderColor = Color.Pink;
            uv.NotificationSettings.Action = NotificationAction.MessageBox;

            switch ((ControlType)controlId)
            {
                case ControlType.CadenaTextual:
                    uv.GetValidationSettings((TextBox)ctrl).IsRequired = true;
                    break;
                case ControlType.CadenaMultilinea:
                    uv.GetValidationSettings((TextBox)ctrl).IsRequired = true;
                    break;
                case ControlType.NumeroEntero:
                    // Establecer condición por rangos
                    rc = new RangeCondition(validateValue1,
                                                             validateValue2,
                                                             typeof(int));
                    vs = uv.GetValidationSettings((UltraNumericEditor)ctrl);
                    vs.Condition = rc;
                    break;
                case ControlType.NumeroDecimal:
                    // Establecer condición por rangos
                    rc = new RangeCondition(validateValue1,
                                                             validateValue2,
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
            }
        }

        #region Events


        private void cb_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (_cancelEventSelectedIndexChange)
                return;

            var tagCtrl = (KeyTagControl)((ComboBox)sender).Tag;
            var componentId = tagCtrl.v_ComponentId;
            var value1 = int.Parse(((ComboBox)sender).SelectedValue.ToString());

            TextBox field = null;

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
                { }
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

                    } 

                    bool isFound = false;
                    
                    var isContain = formu.v_Formula.Contains("/");
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

        }

        private void Capture_Value(object sender, EventArgs e)
        {
            Control senderCtrl = (Control)sender;
            // Obtener información contenida en la propiedad Tag del control invocante
            var tagCtrl = (KeyTagControl)senderCtrl.Tag;
            // Capturar valor inicial
            //_oldValue = GetValueControl(tagCtrl.i_ControlId, senderCtrl);

        }

        private void txt_ValueChanged(object sender, EventArgs e)
        {
            //if (flagValueChange)
            //{
            //    // Capturar el control invocador
            //    Control senderCtrl = (Control)sender;
            //    // Obtener información contenida en la propiedad Tag del control invocante
            //    var tagCtrl = (KeyTagControl)senderCtrl.Tag;
            //    string valueToAnalyze = GetValueControl(tagCtrl.i_ControlId, senderCtrl);
            //    int isSourceField = tagCtrl.i_IsSourceFieldToCalculate;
            //    Dictionary<string, object> Params = null;
            //    List<double> evalExpResultList = new List<double>();

            //    ////MessageBox.Show(senderCtrl.Text);
            //    if (isSourceField == (int)SiNo.SI)
            //    {
            //        #region Nueva logica de calculo de formula soporta n parametros

            //        // Recorrer las formulas en las cuales el campo esta referenciado
            //        foreach (var formu in tagCtrl.Formula)
            //        {
            //            // Obtener Campos fuente participantes en el calculo
            //            var sourceFields = Common.Utils.GetTextFromExpressionInCorchete(formu.v_Formula);
            //            Params = new Dictionary<string, object>();

            //            foreach (string sf in sourceFields)
            //            {
            //                // Buscar controles fuentes
            //                var findCtrlResult = FindDynamicControl(sf);
            //                var length = findCtrlResult.Length;
            //                // La busqueda si tuvo exito
            //                if (length != 0)
            //                {
            //                    // Obtener información del control encontrado 
            //                    var tagSourceField = (KeyTagControl)findCtrlResult[0].Tag;
            //                    // Obtener el tipo de dato al cual se va castear un control encontrado
            //                    string dtc = GetDataTypeControl(tagSourceField.i_ControlId);
            //                    // Obtener value del control encontrado
            //                    var value = GetValueControl(tagSourceField.i_ControlId, findCtrlResult[0]);

            //                    if (dtc == "int")
            //                    {
            //                        //var ival = int.Parse(value);
            //                        Params[sf] = int.Parse(value);
            //                    }
            //                    else if (dtc == "double")
            //                    {
            //                        Params[sf] = double.Parse(value);
            //                    }
            //                }
            //                else
            //                {
            //                    if (sf.ToUpper() == "EDAD")
            //                    {
            //                        Params[sf] = _age;
            //                    }
            //                    else if (sf.ToUpper() == "GENERO_2")
            //                    {
            //                        Params[sf] = _sexType == Gender.FEMENINO ? 0 : 1;
            //                    }
            //                    else if (sf.ToUpper() == "GENERO_1")
            //                    {
            //                        Params[sf] = _sexType == Gender.MASCULINO ? 0 : 1;
            //                    }
            //                }

            //            } // fin foreach sourceFields

            //            bool isFound = false;

            //            // Buscar algun cero
            //            foreach (var item in Params)
            //            {
            //                if (item.Value.ToString() == "0" &&
            //                    item.Key != "EDAD" &&
            //                    item.Key != "GENERO_1" &&
            //                    item.Key != "GENERO_2")
            //                {
            //                    isFound = true;
            //                    break;
            //                }
            //            }

            //            if (!isFound)
            //            {
            //                var evalExpResult = Common.Utils.EvaluateExpression(formu.v_Formula, Params);
            //                evalExpResultList.Add(evalExpResult);
            //            }

            //        } // fin foreach Formula

            //        // Mostrar el resultado en el control indicado
            //        if (evalExpResultList.Count != 0)
            //        {
            //            for (int i = 0; i < tagCtrl.TargetFieldOfCalculateId.Count; i++)
            //            {
            //                var targetFieldOfCalculate1 = FindDynamicControl(tagCtrl.TargetFieldOfCalculateId[i].v_TargetFieldOfCalculateId);

            //                for (int j = 0; j < evalExpResultList.Count; j++)
            //                {
            //                    if (i == j)
            //                    {
            //                        targetFieldOfCalculate1[0].Text = evalExpResultList[j].ToString();
            //                    }
            //                }
            //            }
            //        }

            //        #endregion

            //    }

            //    GeneratedAutoDX(valueToAnalyze, senderCtrl, tagCtrl);

            //}

        }

        #endregion

        private Control[] FindControlInCurrentTab(string key)
        {
            // Obtener TabPage actual
            var currentTabPage = tcExamList.SelectedTab.TabPage;
            var findControl = currentTabPage.Controls.Find(key, true);
            return findControl;
        }
        
        private void EditTotalDiagnosticos()
        {
            if (grdTotalDiagnosticos.Selected.Rows.Count == 0)return;
            _rowIndex = grdTotalDiagnosticos.Selected.Rows[0].Cells["v_DiagnosticRepositoryId"].Row.Index;
            var diagnosticRepositoryId = grdTotalDiagnosticos.Selected.Rows[0].Cells["v_DiagnosticRepositoryId"].Value.ToString();
            var objOperationResult = new OperationResult();
            _tmpTotalDiagnostic = new ServiceBL().GetServiceComponentTotalDiagnostics(ref objOperationResult, diagnosticRepositoryId);
            _componentIdConclusionesDX = _tmpTotalDiagnostic.v_ComponentId;
            if (_tmpTotalDiagnostic.i_FinalQualificationId != null)
            {
                var califiFinalId = (FinalQualification)_tmpTotalDiagnostic.i_FinalQualificationId;
                lblDiagnostico.Text = _tmpTotalDiagnostic.v_DiseasesName;
                cbCalificacionFinal.SelectedValue = califiFinalId == FinalQualification.SinCalificar ? ((int)FinalQualification.Presuntivo).ToString() : ((int)califiFinalId).ToString();
                //cbTipoDx.SelectedValue = _tmpTotalDiagnostic.i_DiagnosticTypeId == null ? ((int)TipoDx.Otros).ToString() : _tmpTotalDiagnostic.i_DiagnosticTypeId.ToString();
                cbTipoDx.SelectedValue = _tmpTotalDiagnostic.i_DiagnosticTypeId == null ? "-1" : _tmpTotalDiagnostic.i_DiagnosticTypeId.ToString();
                cbEnviarAntecedentes.SelectedValue = _tmpTotalDiagnostic.i_IsSentToAntecedent == null ? ((int)SiNo.NO).ToString() : _tmpTotalDiagnostic.i_IsSentToAntecedent.ToString();
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

                    #endregion

                }
            }

            if (_tmpTotalDiagnostic.d_ExpirationDateDiagnostic != null)
                    dtpFechaVcto.Value = _tmpTotalDiagnostic.d_ExpirationDateDiagnostic.Value;

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

        private void grdTotalDiagnosticos_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            EditTotalDiagnosticos();
            var flag = false;
            if (btnAceptarDX.Enabled)
            {
                if (((UltraGrid)sender).Selected.Rows.Count != 0)
                {
                    var componentId = ((UltraGrid)sender).Selected.Rows[0].Cells["v_ComponentId"].Value;
                    flag = componentId == null;
                }
            }
            btnRemoverTotalDiagnostico.Enabled = (grdTotalDiagnosticos.Selected.Rows.Count > 0 && _removerTotalDiagnostico && flag);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnGuardarAnamnesis_Click(object sender, EventArgs e)
        {
            if (uvAnamnesis.Validate(true, false).IsValid)
            {
                var result = MessageBox.Show(@"¿Está seguro de grabar este registro?:", @"CONFIRMACIÓN!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes) return;

                #region Cargar Entidad
                var serviceDto = new serviceDto
                {
                    v_ServiceId = _serviceId,
                    v_MainSymptom = chkPresentaSisntomas.Checked ? txtSintomaPrincipal.Text : null,
                    i_TimeOfDisease = chkPresentaSisntomas.Checked ? int.Parse(txtValorTiempoEnfermedad.Text) : (int?)null,
                    i_TimeOfDiseaseTypeId = chkPresentaSisntomas.Checked ? int.Parse(cbCalendario.SelectedValue.ToString()) : -1,
                    v_Story = txtRelato.Text,
                    i_DreamId = int.Parse(cbSueño.SelectedValue.ToString()),
                    i_UrineId = int.Parse(cbOrina.SelectedValue.ToString()),
                    i_DepositionId = int.Parse(cbDeposiciones.SelectedValue.ToString()),
                    v_Findings = txtHallazgos.Text,
                    i_AppetiteId = int.Parse(cbApetito.SelectedValue.ToString()),
                    i_ThirstId = int.Parse(cbSed.SelectedValue.ToString()),
                    d_Fur = dtpFur.Checked ? dtpFur.Value : (DateTime?)null,
                    v_CatemenialRegime = txtRegimenCatamenial.Text,
                    i_MacId = int.Parse(cbMac.SelectedValue.ToString()),
                    i_HasSymptomId = Convert.ToInt32(chkPresentaSisntomas.Checked),
                    d_PAP = dtpPAP.Checked ? dtpPAP.Value : (DateTime?)null,
                    d_Mamografia = dtpMamografia.Checked ? dtpMamografia.Value : (DateTime?)null,
                    v_Gestapara = txtGestapara.Text,
                    v_Menarquia = txtMenarquia.Text,
                    v_CiruGine = txtCiruGine.Text,
                    i_AptitudeStatusId = int.Parse(cbAptitudEso.SelectedValue.ToString())
                };
                #endregion

                Task.Factory.StartNew(() =>
                {
                    new ServiceBL().UpdateAnamnesis(ref _objOperationResult, serviceDto, Globals.ClientSession.GetAsList());
                },TaskCreationOptions.LongRunning);
            }
            else
            {
                MessageBox.Show(@"Por favor corrija la información ingresada. Vea los indicadores de error.", @"Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tcExamList_SelectedTabChanged(object sender, SelectedTabChangedEventArgs e)
        {
            EXAMENES_lblComentarios.Text = string.Format("Comentarios de {0}", e.Tab.Text);
            EXAMENES_lblEstadoComponente.Text = string.Format("Estado del exámen ({0})", e.Tab.Text);
            btnGuardarExamen.Text = string.Format("&Guardar ({0})", e.Tab.Text);
            _componentId = e.Tab.Key;
            _serviceComponentId = e.Tab.Tag.ToString();
            LoadDataBySelectedComponent(_componentId, _serviceComponentId);
        }

        private void LoadDataBySelectedComponent(string componentId, string serviceComponentId)
        {
            SetSecurityByComponent(componentId);
            PopulateGridDiagnostic(componentId);
            PaintGrdDiagnosticoPorExamenComponente();
            PopulateDataExam(serviceComponentId);
        }
        
        private void PopulateGridDiagnostic(string componentId)
        {
            ClearGridDiagnostic();
            List<DiagnosticRepositoryList> diagnosticList = null;
            Task.Factory.StartNew(() =>
            {
                diagnosticList = new ServiceBL().GetServiceComponentDisgnosticsForGridView(ref _objOperationResult, _serviceId, componentId);
            }).ContinueWith(t =>
            {
                if (!diagnosticList.Any()) return;
                _tmpExamDiagnosticComponentList = diagnosticList;
                grdDiagnosticoPorExamenComponente.DataSource = diagnosticList;
                lblRecordCountDiagnosticoPorExamenCom.Text = string.Format("Se encontraron {0} registros.", diagnosticList.Count);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void ClearGridDiagnostic()
        {
            _tmpExamDiagnosticComponentList = null;
            grdDiagnosticoPorExamenComponente.DataSource = new List<DiagnosticRepositoryList>();
            lblRecordCountDiagnosticoPorExamenCom.Text = @"Se encontraron 0 registros.";
        }

        private void PopulateDataExam(string serviceComponentId)
        {
            ServiceComponentList serviceComponentsInfo = null;
            Task.Factory.StartNew(() =>
            {
                serviceComponentsInfo =  new ServiceBL().GetServiceComponentsInfo(ref _objOperationResult, serviceComponentId, _serviceId);

            }).ContinueWith(t =>
            {
                if (serviceComponentsInfo == null) return;
                txtComentario.Text = serviceComponentsInfo.v_Comment;
                cbEstadoComponente.SelectedValue = serviceComponentsInfo.i_ServiceComponentStatusId == (int)ServiceComponentStatus.PorIniciar ? ((int)ServiceComponentStatus.Iniciado).ToString() : serviceComponentsInfo.i_ServiceComponentStatusId.ToString();
                cbTipoProcedenciaExamen.SelectedValue = serviceComponentsInfo.i_ExternalInternalId == null ? "1" : serviceComponentsInfo.i_ExternalInternalId.ToString();
                chkApproved.Checked = Convert.ToBoolean(serviceComponentsInfo.i_IsApprovedId);
                if (serviceComponentsInfo.d_UpdateDate != null)
                   lblUsuGraba.Text = serviceComponentsInfo.v_UpdateUser.ToUpper();
                if (serviceComponentsInfo.d_UpdateDate != null)
                    lblFechaGraba.Text = serviceComponentsInfo.d_UpdateDate.Value.ToString("dd-MMMM-yyyy (hh:mm) ");
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void TsmverMas_Click(object sender, EventArgs e)
        {
            lblView_Click(sender, e);
        }

        private void lblTrabajador_Click(object sender, EventArgs e)
        {
            var frmWorkerData = new FrmWorkerData(_serviceId);
            frmWorkerData.ShowDialog();
        }

        private void lblProtocolName_Click(object sender, EventArgs e)
        {
            var frmWorkerData = new FrmWorkerData(_serviceId);
            frmWorkerData.ShowDialog();
        }

        private void grdDiagnosticoPorExamenComponente_ClickCell(object sender, ClickCellEventArgs e)
        {
            ValidateRemoveDxAutomatic();
        }

        private void grdDiagnosticoPorExamenComponente_AfterPerformAction(object sender, AfterUltraGridPerformActionEventArgs e)
        {
            ValidateRemoveDxAutomatic();
        }

        private void ValidateRemoveDxAutomatic()
        {
            if (grdDiagnosticoPorExamenComponente.Selected.Rows.Count <= 0) return;
            if (!_isDisabledButtonsExamDx) return;
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
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetSecurityByComponent(string componentId)
        {
            ExamConfiguration(componentId);
        }

        private void ExamConfiguration(string componentId)
        {
            var objOperationResult = new OperationResult();
            var componentProfile = new ServiceBL().GetRoleNodeComponentProfile(ref objOperationResult, _nodeId, _roleId, componentId);

            IsExamDiagnosable(componentProfile.i_Dx);
            IsExamApproved(componentProfile.i_Approved);
        }
        
        private void IsExamApproved(int? idApproved)
        {
            switch (idApproved)
            {
                case (int)SiNo.NO:
                    // el check de APROBADO está desactivado. No importando el permiso del rol
                    chkApproved.Enabled = false;
                    break;
                case (int)SiNo.SI:
                    // el check se activa o desactiva dependiendo del rol
                    chkApproved.Enabled = BLL.Utils.IsActionEnabled("frmEso_EXAMENES_APPROVED", _formActions);
                    break;
            }
        }

        private void IsExamDiagnosable(int? idDx)
        {
            switch (idDx)
            {
                case (int)SiNo.SI:
                    //la sección se activa o desactiva dependiendo del PERMISO. Los diagnósticos automáticos deben seguir funcionando y reportándose.
                    btnAgregarDxExamen.Enabled = BLL.Utils.IsActionEnabled("frmEso_EXAMENES_ADDDX", _formActions);
                    btnEditarDxExamen.Enabled = BLL.Utils.IsActionEnabled("frmEso_EXAMENES_EDITDX", _formActions);
                    btnRemoverDxExamen.Enabled = BLL.Utils.IsActionEnabled("frmEso_EXAMENES_REMOVEDX", _formActions);
                    break;
                case (int)SiNo.NO:
                    // toda la sección esta desactivada, pero los diagnósticos automáticos deben seguir funcionando y reportándose.
                    btnAgregarDxExamen.Enabled = false;
                    btnEditarDxExamen.Enabled = false;
                    btnRemoverDxExamen.Enabled = false;
                    _isDisabledButtonsExamDx = true;
                    break;
            }
        }

        private void btnRemoverDxExamen_Click(object sender, EventArgs e)
        {
            if (!ValidateSelected()) return;
            var result = MessageBox.Show(@"¿Está seguro de eliminar este registro?:", @"ADVERTENCIA!",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;
            var diagnosticRepositoryId = grdDiagnosticoPorExamenComponente.Selected.Rows[0].Cells["v_DiagnosticRepositoryId"].Value.ToString();
            var recordType = int.Parse(grdDiagnosticoPorExamenComponente.Selected.Rows[0].Cells["i_RecordType"].Value.ToString());
            var findResult = _tmpExamDiagnosticComponentList.Find(p => p.v_DiagnosticRepositoryId == diagnosticRepositoryId);
            switch (recordType)
            {
                case (int) RecordType.Temporal:
                    _tmpExamDiagnosticComponentList.Remove(findResult);
                    break;
                case (int) RecordType.NoTemporal:
                    findResult.i_RecordStatus = (int) RecordStatus.EliminadoLogico;
                    foreach (var item in findResult.Recomendations)
                    {
                        item.i_RecordStatus = (int) RecordStatus.EliminadoLogico;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            BindGridDiagnosticTemporal(_tmpExamDiagnosticComponentList);
            //var dataList = _tmpExamDiagnosticComponentList.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);
            //grdDiagnosticoPorExamenComponente.DataSource = dataList;
            //lblRecordCountDiagnosticoPorExamenCom.Text = string.Format("Se encontraron {0} registros.", _tmpExamDiagnosticComponentList.Count());
        }

        private bool ValidateSelected()
        {
            if (grdDiagnosticoPorExamenComponente.Selected.Rows.Count != 0)
                return true;

            MessageBox.Show(@"Por favor seleccione un registro.", @"Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private void btnAgregarDxExamen_Click(object sender, EventArgs e)
        {
            var frm = new frmAddExamDiagnosticComponent("New"){_componentId = _componentId,_serviceId = _serviceId};

            if (_tmpExamDiagnosticComponentList != null)
                frm._tmpExamDiagnosticComponentList = _tmpExamDiagnosticComponentList;

            frm.ShowDialog();

            if (frm.DialogResult == DialogResult.Cancel)
                return;
            BindGridDiagnosticTemporal(frm._tmpExamDiagnosticComponentList);
        }

        private void BindGridDiagnosticTemporal(List<DiagnosticRepositoryList> listDx)
        {
            if (listDx == null) return;
            _tmpExamDiagnosticComponentList = listDx;

            var dataList = _tmpExamDiagnosticComponentList.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);
            grdDiagnosticoPorExamenComponente.DataSource = new DiagnosticRepositoryList();
            grdDiagnosticoPorExamenComponente.DataSource = dataList;
            lblRecordCountDiagnosticoPorExamenCom.Text = string.Format("Se encontraron {0} registros.", dataList.Count());
        }

        private void btnEditarDxExamen_Click(object sender, EventArgs e)
        {
            if (!ValidateSelected()) return;
            var frm = new frmAddExamDiagnosticComponent("Edit");

            var diagnosticRepositoryId = grdDiagnosticoPorExamenComponente.Selected.Rows[0].Cells["v_DiagnosticRepositoryId"].Value.ToString();
            frm._diagnosticRepositoryId = diagnosticRepositoryId;
            frm._componentId = _componentId;
            frm._serviceId = _serviceId;

            if (_tmpExamDiagnosticComponentList != null)
                frm._tmpExamDiagnosticComponentList = _tmpExamDiagnosticComponentList;

            frm.ShowDialog();

            if (frm.DialogResult == DialogResult.Cancel)
                return;
            BindGridDiagnosticTemporal(frm._tmpExamDiagnosticComponentList);
            PaintGrdDiagnosticoPorExamenComponente();

            //if (frm._tmpExamDiagnosticComponentList != null)
            //{
            //    _tmpExamDiagnosticComponentList = frm._tmpExamDiagnosticComponentList;

            //    var dataList = _tmpExamDiagnosticComponentList.FindAll(p => p.i_RecordStatus != (int)RecordStatus.EliminadoLogico);
            //    grdDiagnosticoPorExamenComponente.DataSource = new DiagnosticRepositoryList();
            //    grdDiagnosticoPorExamenComponente.DataSource = dataList;
            //    grdDiagnosticoPorExamenComponente.Refresh();
            //    lblRecordCountDiagnosticoPorExamenCom.Text = string.Format("Se encontraron {0} registros.", dataList.Count());

            //    
            //}
            //if (grdDiagnosticoPorExamenComponente.Selected.Rows.Count != 0) return;
            //MessageBox.Show(@"Por favor seleccione un registro.", @"Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    throw new ArgumentOutOfRangeException();
            }

            e.Row.Appearance.BackGradientStyle = GradientStyle.VerticalBump;
        }

        private void PaintGrdDiagnosticoPorExamenComponente()
        {
            foreach (var t in grdDiagnosticoPorExamenComponente.Rows)
            {
                var caliFinal = (PreQualification)t.Cells["i_PreQualificationId"].Value;
                switch (caliFinal)
                {
                    case PreQualification.SinPreCalificar:
                        t.Appearance.BackColor = Color.Pink;
                        t.Appearance.BackColor2 = Color.Pink;
                        break;
                    case PreQualification.Aceptado:
                        t.Appearance.BackColor = Color.LawnGreen;
                        t.Appearance.BackColor2 = Color.LawnGreen;
                        break;
                    case PreQualification.Rechazado:
                        t.Appearance.BackColor = Color.DarkGray;
                        t.Appearance.BackColor2 = Color.DarkGray;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                t.Appearance.BackGradientStyle = GradientStyle.VerticalBump;
            }
        }

        private void grdTotalDiagnosticos_InitializeRow(object sender, InitializeRowEventArgs e)
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
                    throw new ArgumentOutOfRangeException();
            }
            e.Row.Appearance.BackGradientStyle = GradientStyle.VerticalBump;

        }

        private void grdTotalDiagnosticos_BeforePerformAction(object sender, BeforeUltraGridPerformActionEventArgs e)
        {
            if ((e.UltraGridAction == UltraGridAction.NextRow && _currentDownKey == Keys.Right)
                   || (e.UltraGridAction == UltraGridAction.PrevRow && _currentDownKey == Keys.Left))
            {
                e.Cancel = true;
            }
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

        private void grdRecomendaciones_AnalisisDiagnostico_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            btnRemoverRecomendacion_AnalisisDx.Enabled = (grdRecomendaciones_AnalisisDiagnostico.Selected.Rows.Count > 0 && _removerRecomendacionAnalisisDx);
        }

        private void grdRestricciones_AnalisisDiagnostico_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            btnRemoverRestriccion_Analisis.Enabled = (grdRestricciones_AnalisisDiagnostico.Selected.Rows.Count > 0 && _removerRestriccionAnalisis);
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

        private void btnAceptarDX_Click(object sender, EventArgs e)
        {
            var califiFinalId = (FinalQualification)int.Parse(cbCalificacionFinal.SelectedValue.ToString());
            if (califiFinalId == FinalQualification.Descartado || califiFinalId == FinalQualification.SinCalificar)
            {
                GuardarDx();
            }
            else
            {
                if (uvAnalisisDx.Validate(true, false).IsValid)
                {
                    GuardarDx();
                }
                else
                {
                    MessageBox.Show("Por favor corrija la información ingresada. Vea los indicadores de error.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void GuardarDx()
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

                #endregion

                new ServiceBL().UpdateTotalDiagnostic(ref objOperationResult,
                                                 _tmpTotalDiagnostic,
                                                 _serviceId,
                                                 Globals.ClientSession.GetAsList());


                //InitializeData();
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

        private void GetTotalDiagnosticsForGridView()
        {
                OperationResult objOperationResult = new OperationResult();
                _tmpTotalDiagnosticByServiceIdList = new ServiceBL().GetServiceComponentDisgnosticsByServiceId(ref objOperationResult, _serviceId);

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

        private void ConclusionesyTratamiento_LoadAllGrid()
        {
            OperationResult objOperationResult = new OperationResult();

            #region Conclusiones Diagnósticas

            GetConclusionesDiagnosticasForGridView();

            #endregion

            #region Recomendación

            _tmpRecomendationConclusionesList = new ServiceBL().GetServiceRecommendationByServiceId(ref objOperationResult, _serviceId);

            if (_tmpRecomendationConclusionesList == null)
                return;

            grdRecomendaciones_Conclusiones.DataSource = _tmpRecomendationConclusionesList;
            lblRecordCountRecomendaciones_Conclusiones.Text = string.Format("Se encontraron {0} registros.",
                                                            _tmpRecomendationConclusionesList.Count());

            #endregion

            #region Restricciones

            _tmpRestrictionListConclusiones = new ServiceBL().GetServiceRestrictionsForGridView(ref objOperationResult, _serviceId);

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

        private void GetConclusionesDiagnosticasForGridView()
        {
            var objOperationResult = new OperationResult();
            _tmpTotalConclusionesDxByServiceIdList = new ServiceBL().GetServiceComponentConclusionesDxServiceId(ref objOperationResult, _serviceId);

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

        private void btnAgregarDX_Click(object sender, EventArgs e)
        {
            var returnDiseasesList = new DiseasesList();
            var frm = new frmDiseases();

            frm.ShowDialog();
            returnDiseasesList = frm._objDiseasesList;

            if (returnDiseasesList.v_DiseasesId == null) return;
            lblDiagnostico.Text = returnDiseasesList.v_Name + " / " + returnDiseasesList.v_CIE10Id;
            _diagnosticId = returnDiseasesList.v_DiseasesId;
        }
        
        private void grdConclusionesDiagnosticas_ClickCell(object sender, ClickCellEventArgs e)
        {
            _rowIndexConclucionesDX = e.Cell.Row.Index;
            GetConclusionesDiagnosticasForGridView();
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
                var diagnosticRepositoryId = grdTotalDiagnosticos.Selected.Rows[0].Cells["v_DiagnosticRepositoryId"].Value.ToString();
                new ServiceBL().DeleteTotalDiagnostic(ref objOperationResult, diagnosticRepositoryId, Globals.ClientSession.GetAsList());
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

        private void btnRefrescarTotalDiagnostico_Click(object sender, EventArgs e)
        {
            GetTotalDiagnosticsForGridView();
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

        private void btnGuardarExamen_Click(object sender, EventArgs e)
        {
            if (_isChangeValue && cbEstadoComponente.SelectedValue.ToString() == ((int)ServiceComponentStatus.Iniciado).ToString())
            {
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
        }

        private void SaveExamBySelectedTab(Infragistics.Win.UltraWinTabControl.UltraTabPageControl selectedTab)
        {
            _isChangeValue = false;
            UltraValidator uv = null;
             OperationResult objOperationResult = new OperationResult();
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
                        this.Enabled = false;
                        #region Capturar [Comentarios, estado, procedencia de un exmanen componente]

                        var serviceComponentDto = new servicecomponentDto();
                        serviceComponentDto.v_ServiceComponentId = _serviceComponentId;
                        serviceComponentDto.v_Comment = txtComentario.Text;
                        serviceComponentDto.i_ServiceComponentStatusId = int.Parse(cbEstadoComponente.SelectedValue.ToString());
                        serviceComponentDto.i_ExternalInternalId = int.Parse(cbTipoProcedenciaExamen.SelectedValue.ToString());
                        serviceComponentDto.i_IsApprovedId = Convert.ToInt32(chkApproved.Checked);

                        serviceComponentDto.v_ComponentId = _componentId;
                        serviceComponentDto.v_ServiceId = _serviceId;
                        var FechaUpdate = new ServiceBL().GetServiceComponent(ref objOperationResult, _serviceComponentId).d_UpdateDate;
                        serviceComponentDto.d_UpdateDate = FechaUpdate;

                        #endregion


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

        private Control[] FindDynamicControl(string key)
        {
            // Obtener TabPage actual
            var currentTabPage = tcExamList.SelectedTab.TabPage;
            //var findControl = currentTabPage.Controls.Find(key, true);

            var findControl = tcExamList.Tabs.TabControl.Controls.Find(key, true);

            return findControl;
        }

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

        private void bgwSaveExamen_DoWork(object sender, DoWorkEventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();

            using (new LoadingClass.PleaseWait(this.Location, "Grabando..."))
            {
                RunWorkerAsyncPackage packageForSave = (RunWorkerAsyncPackage)e.Argument;
                bool result = false;
                #region GRABAR CONTROLES DINAMICOS

                var selectedTab = (Infragistics.Win.UltraWinTabControl.UltraTabPageControl)packageForSave.SelectedTab;

                var serviceComponentId = selectedTab.Tab.Tag.ToString();

                ProcessControlBySelectedTab(selectedTab);

                if (packageForSave.ServiceComponent.d_UpdateDate == null)
                {
                    result = new ServiceBL().AddServiceComponentValues(ref objOperationResult,
                                                           _serviceComponentFieldsList,
                                                           Globals.ClientSession.GetAsList(),
                                                           _personId,
                                                           serviceComponentId);
                }
                else
                {
                    var DatosAntiguos = _oListValidacionAMC.AsEnumerable()
                  .GroupBy(x => x.v_ServiceComponentFieldValuesId)
                  .Select(group => group.First()).ToList();

                    List<DatosModificados> ListaDatoModificado = new List<DatosModificados>();
                    DatosModificados oDatosModificados = null;

                    foreach (var itemAntiguo in DatosAntiguos)
                    {
                        var Coincidencia = _serviceComponentFieldsList.FindAll(p => p.v_ComponentFieldsId == itemAntiguo.v_ComponentFieldsId);

                        if (Coincidencia.Count != 0)
                        {
                            if (itemAntiguo.v_Value1 != Coincidencia[0].ServiceComponentFieldValues[0].v_Value1)
                            {
                                oDatosModificados = new DatosModificados();
                                oDatosModificados.v_ComponentFieldsId = itemAntiguo.v_ComponentFieldsId;
                                ListaDatoModificado.Add(oDatosModificados);
                            }
                        }
                    }

                    //Agregar los datos nuevos

                    foreach (var item in _serviceComponentFieldsList)
                    {
                        var Coincidencia = DatosAntiguos.FindAll(p => p.v_ComponentFieldsId == item.v_ComponentFieldsId);
                        if (Coincidencia.Count == 0)
                        {
                            oDatosModificados = new DatosModificados();
                            oDatosModificados.v_ComponentFieldsId = item.v_ComponentFieldsId;
                            ListaDatoModificado.Add(oDatosModificados);
                        }
                    }

                    string[] ListaDatoModificado_ = new string[ListaDatoModificado.Count()];
                    for (int i = 0; i < ListaDatoModificado.Count(); i++)
                    {
                        ListaDatoModificado_[i] = ListaDatoModificado[i].v_ComponentFieldsId;
                    }

                    //var DatosGrabar = _serviceComponentFieldsList.FindAll(p => p.v_ComponentFieldsId.Contains(ListaDatoModificado.));
                    var DatosGrabar = _serviceComponentFieldsList.FindAll(p => ListaDatoModificado_.Contains(p.v_ComponentFieldsId));


                    result = new ServiceBL().AddServiceComponentValues(ref objOperationResult,
                                                                DatosGrabar,
                                                                Globals.ClientSession.GetAsList(),
                                                                _personId,
                                                                _serviceComponentId);
                }
               

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

                new ServiceBL().AddDiagnosticRepository(ref objOperationResult,
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
                    //InitializeData();
                    //LoadDataBySelectedComponent(_componentId);
                    GetTotalDiagnosticsForGridView();
                    ConclusionesyTratamiento_LoadAllGrid();

                    #endregion

                }));

            }
        }

        private void bgwSaveExamen_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Enabled = true;
        }

        private void tcExamList_SelectedTabChanging(object sender, SelectedTabChangingEventArgs e)
        {
            var obj = e.Tab.TabControl.ActiveTab;

            SaveExamWherePendingChange();
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

                new ServiceBL().AddConclusiones(ref objOperationResult,
                                           _tmpRestrictionListConclusiones,
                                           _tmpRecomendationConclusionesList,
                                           serviceDTO,
                                           null,
                                           Globals.ClientSession.GetAsList());


                // Refrescar todas las grillas
                //InitializeData();
                ConclusionesyTratamiento_LoadAllGrid();

                // Analizar el resultado de la operación
                if (objOperationResult.Success != 1)
                {
                    MessageBox.Show(Constants.GenericErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    #region Mensaje de información de guardado
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
        }

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

            //var ListaPdf = _serviceBL.GetFilePdfsByServiceId(ref objOperationResult, _serviceId);


            //var o = _serviceBL.GetServiceShort(serviceId);
            //string Fecha = o.FechaServicio.Value.Day.ToString().PadLeft(2, '0') + o.FechaServicio.Value.Month.ToString().PadLeft(2, '0') + o.FechaServicio.Value.Year.ToString();
            //DirectoryInfo rutaOrigen = null;

            ////ELECTRO
            //rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgEKGOrigen").ToString());
            //FileInfo[] files1 = rutaOrigen.GetFiles();

            //foreach (FileInfo file in files1)
            //{
            //    if (file.ToString().Count() > 16)
            //    {
            //        if (file.ToString().Substring(0, 17) == o.DNI + "-" + Fecha)
            //        {
            //            _filesNameToMerge.Add(rutaOrigen + file.ToString());
            //        };
            //    }
            //}

            ////ESPIRO
            //rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgESPIROOrigen").ToString());
            //FileInfo[] files2 = rutaOrigen.GetFiles();

            //foreach (FileInfo file in files2)
            //{
            //    if (file.ToString().Count() > 16)
            //    {
            //        if (file.ToString().Substring(0, 17) == o.DNI + "-" + Fecha)
            //        {
            //            _filesNameToMerge.Add(rutaOrigen + file.ToString());
            //        };
            //    }
            //}

            ////RX
            //rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgRxOrigen").ToString());
            //FileInfo[] files3 = rutaOrigen.GetFiles();

            //foreach (FileInfo file in files3)
            //{
            //    if (file.ToString().Count() > 16)
            //    {
            //        if (file.ToString().Substring(0, 17) == o.DNI + "-" + Fecha)
            //        {
            //            _filesNameToMerge.Add(rutaOrigen + file.ToString());
            //        };
            //    }
            //}

            ////LAB
            //rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgLABOrigen").ToString());
            //FileInfo[] files4 = rutaOrigen.GetFiles();

            //foreach (FileInfo file in files4)
            //{
            //    if (file.ToString().Count() > 16)
            //    {
            //        if (file.ToString().Substring(0, 17) == o.DNI + "-" + Fecha)
            //        {
            //            _filesNameToMerge.Add(rutaOrigen + file.ToString());
            //        };
            //    }
            //}

            ////MED
            //rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgMedicinaOrigen").ToString());
            //FileInfo[] files5 = rutaOrigen.GetFiles();

            //foreach (FileInfo file in files5)
            //{
            //    if (file.ToString().Count() > 16)
            //    {
            //        if (file.ToString().Substring(0, 17) == o.DNI + "-" + Fecha)
            //        {
            //            _filesNameToMerge.Add(rutaOrigen + file.ToString());
            //        };
            //    }
            //}

            ////PSICO
            //rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgPsicoOrigen").ToString());
            //FileInfo[] files6 = rutaOrigen.GetFiles();

            //foreach (FileInfo file in files6)
            //{
            //    if (file.ToString().Count() > 16)
            //    {
            //        if (file.ToString().Substring(0, 17) == o.DNI + "-" + Fecha)
            //        {
            //            _filesNameToMerge.Add(rutaOrigen + file.ToString());
            //        };
            //    }
            //}

            ////ADMIN
            //rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgAdminOrigen").ToString());
            //FileInfo[] files7 = rutaOrigen.GetFiles();

            //foreach (FileInfo file in files7)
            //{
            //    if (file.ToString().Count() > 16)
            //    {
            //        if (file.ToString().Substring(0, 17) == o.DNI + "-" + Fecha)
            //        {
            //            _filesNameToMerge.Add(rutaOrigen + file.ToString());
            //        };
            //    }
            //}

            ////OFTALMO
            //rutaOrigen = new DirectoryInfo(Common.Utils.GetApplicationConfigValue("ImgOftalmoOrigen").ToString());
            //FileInfo[] files8 = rutaOrigen.GetFiles();

            //foreach (FileInfo file in files8)
            //{
            //    if (file.ToString().Count() > 16)
            //    {
            //        if (file.ToString().Substring(0, 17) == o.DNI + "-" + Fecha)
            //        {
            //            _filesNameToMerge.Add(rutaOrigen + file.ToString());
            //        };
            //    }
            //}


            //var x = _filesNameToMerge.ToList();
            //_mergeExPDF.FilesName = x;
            //_mergeExPDF.DestinationFile = Application.StartupPath + @"\TempMerge\" + _serviceId + ".pdf"; ;
            //_mergeExPDF.DestinationFile = ruta + _serviceId + ".pdf"; ;
            //_mergeExPDF.Execute();

        }

        private void ChooseReport(string componentId, string psrtServiceId, string pstrRutaReportes, string pstrPersonId)
        {
            DataSet dsGetRepo = null;
            DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
            OperationResult objOperationResult = new OperationResult();
            ReportDocument rp;

            switch (componentId)
            {
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

        private void button1_Click(object sender, EventArgs e)
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

        private void btnCerrarESO_Click(object sender, EventArgs e)
        {
            Close();
        }
             
    }
}
