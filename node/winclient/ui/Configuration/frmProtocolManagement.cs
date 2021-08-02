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

namespace Sigesoft.Node.WinClient.UI.Configuration
{
    public partial class frmProtocolManagement : Form
    {
        private string _Mode;
        private int _pintServiceId;
        private int _pintServiceTypeId;
        public  string _pstrProtocolId;
        private string _EmpresaPadreId;
       

        #region Declarations
        private ProtocolBL _protocolBL = new ProtocolBL();
        private string _protocolId;
     
        #endregion

        public frmProtocolManagement(string pstrMode, int pintServiceTypeId,int pintServiceId, string pstrEmpresaPadreId)
        {
            InitializeComponent();
            _Mode = pstrMode;
            if (_Mode == "View")
            {
                _pintServiceId = pintServiceId;
                _pintServiceTypeId = pintServiceTypeId;
                 //cmProtocol.Enabled = false;
                 chkIsActive.Enabled = false;

                 //btnNuevo.Enabled = false;
                 //btnEditar.Enabled = false;
                 //btnClon.Enabled = false;
                 btnGenerarOS.Enabled = false;
                 _EmpresaPadreId = pstrEmpresaPadreId;
              
                //cbMasterService.SelectedValue = pintServiceId.ToString();
                ////cbMasterService.Enabled = false;
            }
        }

        public frmProtocolManagement()
        {
            InitializeComponent();           
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private string BuildFilterExpression()
        {
            OperationResult objOperationResult = new OperationResult();
            // Get the filters from the UI

          
            string filterExpression = string.Empty;

            List<string> Filters = new List<string>();

            if (!string.IsNullOrEmpty(txtProtocolName.Text)) Filters.Add("v_Protocol.Contains(\"" + txtProtocolName.Text.Trim() + "\")");
            //if (cbOrganization.SelectedValue.ToString() != "-1")
            //{
            //    var id1 = cbOrganization.SelectedValue.ToString().Split('|');
            //    Filters.Add("v_OrganizationId==" + "\"" + id1[0] + "\"&&v_LocationId==" + "\"" + id1[1] + "\"");
            //}
            if (cbEsoType.SelectedValue.ToString() != "-1") Filters.Add("i_EsoTypeId==" + int.Parse(cbEsoType.SelectedValue.ToString()));
            //if (cbGeso.SelectedValue.ToString() != "-1") Filters.Add("v_GroupOccupationId==" + "\"" + cbGeso.SelectedValue + "\"");
            //if (cbIntermediaryOrganization.SelectedValue.ToString() != "-1")
            //{
            //    var id2 = cbIntermediaryOrganization.SelectedValue.ToString().Split('|');
            //    Filters.Add("v_WorkingOrganizationId==" + "\"" + id2[0] + "\"&&v_WorkingLocationId==" + "\"" + id2[1] + "\"");
            //}
            //if (cbOrganizationInvoice.SelectedValue.ToString() != "-1")
            //{
            //    var id3 = cbOrganizationInvoice.SelectedValue.ToString().Split('|');
            //    Filters.Add("v_OrganizationInvoiceId==" + "\"" + id3[0] + "\"&&v_CustomerLocationId==" + "\"" + id3[1] + "\"");
            //}

            //if (cbOrganizationInvoice.SelectedValue.ToString() != "-1") Filters.Add("v_OrganizationInvoiceId==" + "\"" + cbOrganizationInvoice.SelectedValue + "\"");
          

            if (cbServiceType.SelectedValue.ToString() != "-1")
            {
                Filters.Add("i_ServiceTypeId==" + int.Parse(cbServiceType.SelectedValue.ToString()));
            }

            if (cbService.SelectedValue.ToString() != "-1")
            {           
                Filters.Add("i_MasterServiceId==" + int.Parse(cbService.SelectedValue.ToString()));
            }
            
            Filters.Add("i_IsActive ==" + Convert.ToInt32(chkIsActive.Checked).ToString());
           
            //Filters.Add("i_IsDeleted==0");
            // Create the Filter Expression
            filterExpression = null;
            if (Filters.Count > 0)
            {
                foreach (string item in Filters)
                {
                    filterExpression = filterExpression + item + " && ";
                }
                filterExpression = filterExpression.Substring(0, filterExpression.Length - 4);
            }

            return filterExpression;
        }

        private void New_Click(object sender, EventArgs e)
        {
            var frm = new frmProtocolEdit(string.Empty, "New");
            frm.MdiParent = this.MdiParent;
            frm.Show();
          
            // Refrescar grilla
            btnFilter_Click(sender, e);
            
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            var frm = new frmProtocolEdit(_protocolId, "Edit");
            frm.MdiParent = this.MdiParent;
            frm.Show();
          
            // Refrescar grilla
            btnFilter_Click(sender, e);
            
        }

        private void Clonar_Click(object sender, EventArgs e)
        {
            var frm = new frmProtocolEdit(_protocolId, "Clon");
            frm.MdiParent = this.MdiParent;
            frm.Show();

            if (frm.DialogResult == DialogResult.OK)
            {
                // Refrescar grilla
                btnFilter_Click(sender, e);
            }
        }     

        private void BindGrid()
        {
            var dataList = GetData(0, null, "v_Protocol ASC", BuildFilterExpression());

            if (dataList != null)
            {
                if (dataList.Count != 0)
                {
                    grd.DataSource = dataList;
                    lblRecordCount.Text = string.Format("Se encontraron {0} registros.", dataList.Count());
                }
                else
                {
                    grd.DataSource = dataList;                  
                    lblRecordCount.Text = string.Format("Se encontraron {0} registros.", 0);

                }

                grdProtocolComponent.DataSource = new List<ProtocolComponentList>();
                lblCostoTotal.Text = "";
            }
          
        }

        private List<ProtocolList> GetData(int pintPageIndex, int? pintPageSize, string pstrSortExpression, string pstrFilterExpression)
        {
            OperationResult objOperationResult = new OperationResult();

            List<string> ListaEmpresasId = new List<string>();
            //Obtener todas las empresas relacionadas a la empresa Padre
            if (cbOrganizationInvoice.SelectedValue == null)
            {
                MessageBox.Show("Seleccionar un protocolo", "VALIDACIÓN!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            if (cbOrganizationInvoice.SelectedValue.ToString() == "-1")
            {
                var Lista = BLL.Utils.GetAllEmpresaPadre(ref objOperationResult);
                foreach (var item in Lista)
                {
                    ListaEmpresasId.Add(item.Id.ToString());
                }
            }
            else
            {
                ListaEmpresasId = _protocolBL.DevolverEmpresasHijo(cbOrganizationInvoice.SelectedValue.ToString());

            }

            var dataList = _protocolBL.GetProtocolPagedAndFiltered(ref objOperationResult, pintPageIndex, pintPageSize, pstrSortExpression, pstrFilterExpression, ListaEmpresasId, txtComponente.Text);

            if (objOperationResult.Success != 1)
            {
                MessageBox.Show("Error en operación:" + System.Environment.NewLine + objOperationResult.ExceptionMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dataList;
        }
      
        private void frmProtocolManagement_Load(object sender, EventArgs e)
        {                   
            OperationResult objOperationResult = new OperationResult();
            //Utils.LoadDropDownList(cbGeso, "Value1", "Id", BLL.Utils.GetGESO(ref objOperationResult, null), DropDownListAction.All);
            LoadComboBox();
            if (_Mode=="View")
            {
                   cbOrganizationInvoice.SelectedValue = _EmpresaPadreId;
            }
            BindGrid();

            if (grd.Rows.Count != 0)          
                grd.Rows[0].Selected = true;
           
        }

        private void LoadComboBox()
        {
            // Llenado de combos
            // Tipos de eso
            OperationResult objOperationResult = new OperationResult();
            Utils.LoadDropDownList(cbEsoType, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 118, null), DropDownListAction.All);
            Utils.LoadDropDownList(cboStatusProtocolId, "Value1", "Id", BLL.Utils.GetDataHierarchyForCombo(ref objOperationResult, 116, null), DropDownListAction.All);
            // Lista de empresas por nodo
            int nodeId = int.Parse(Common.Utils.GetApplicationConfigValue("NodeId"));
            OperationResult objOperationResult1 = new OperationResult();
            var dataListOrganization = BLL.Utils.GetAllEmpresaPadre(ref objOperationResult1);
            //var dataListOrganization1 = BLL.Utils.GetJoinOrganizationAndLocation(ref objOperationResult1, nodeId);
            //var dataListOrganization2 = BLL.Utils.GetJoinOrganizationAndLocation(ref objOperationResult1, nodeId);

            //Utils.LoadDropDownList(cbOrganization,
            //    "Value1",
            //    "Id",
            //    dataListOrganization,
            //    DropDownListAction.All,
            //    true);
            //Obtener Empresas Padres
            //Utils.LoadDropDownList(cbOrganizationInvoice, "Value1", "Id", BLL.Utils.GetJoinOrganizationPadreAndLocation(ref objOperationResult, Globals.ClientSession.i_CurrentExecutionNodeId), DropDownListAction.Select);

            Utils.LoadDropDownList(cbOrganizationInvoice,
               "Value1",
               "Id",
               dataListOrganization,
               DropDownListAction.All,
               true);

            //Utils.LoadDropDownList(cbOrganizationInvoice,
            //  "Value1",
            //  "Id",
            //  dataListOrganization2,
            //  DropDownListAction.All,
            //  true);

            //Llenado de los tipos de servicios [Emp/Part]
            Utils.LoadDropDownList(cbServiceType, "Value1", "Id", BLL.Utils.GetSystemParameterByParentIdForCombo(ref objOperationResult, 119, -1, null), DropDownListAction.All);
            // combo servicio
            Utils.LoadDropDownList(cbService, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, -1, null), DropDownListAction.All);
           
            if (_Mode == "View")
            {          
                Utils.LoadDropDownList(cbServiceType, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 119, null), DropDownListAction.All);
                cbServiceType.SelectedValue = _pintServiceTypeId.ToString();

                Utils.LoadDropDownList(cbService, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 119, null), DropDownListAction.All);
                cbService.SelectedValue = _pintServiceId.ToString();
               
                cbServiceType.Enabled = false;
                cbService.Enabled = false;
            }
            else
            {
                //Utils.LoadDropDownList(cbMasterService, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 119, null), DropDownListAction.All);

            }

           
        }

        private void cbOrganization_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadcbGESO();
           
        }

        private void LoadcbGESO()
        {
            var index = cbOrganization.SelectedIndex;
            if (index == 0)
                return;

            var dataList = cbOrganization.SelectedValue.ToString().Split('|');
            string idOrg = dataList[0];

            OperationResult objOperationResult = new OperationResult();
            Utils.LoadDropDownList(cbGeso, "Value1", "Id", BLL.Utils.GetGESO(ref objOperationResult, idOrg), DropDownListAction.All);
        }

        private void grd_MouseDown(object sender, MouseEventArgs e)
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
                    grd.Rows[row.Index].Selected = true;
                    _protocolId = row.Cells["v_ProtocolId"].Value.ToString();
                    cmProtocol.Items["Edit"].Enabled = true;
                    cmProtocol.Items["Clonar"].Enabled = true;                                  
                }
                else
                {
                    cmProtocol.Items["Edit"].Enabled = false;
                    cmProtocol.Items["Clonar"].Enabled = false;
                }

            } 
        }

        private void grd_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_Mode == "View")
            {
                var x =int.Parse(grd.Selected.Rows[0].Cells["i_StatusProtocolId"].Value.ToString());

                if (x == (int)StatusProtocol.Pendiente)
                {
                    if (MessageBox.Show("El protocolo seleccionado no está aprobado por el CLIENTE. ¿Está seguro que desea utilizarlo?", "¡Advertencia!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) 
                    {
                        return;
                    }
                }

                if (grd.Selected.Rows.Count == 0) 
                    return;

                _pstrProtocolId = grd.Selected.Rows[0].Cells[0].Value.ToString();
                this.DialogResult = DialogResult.OK;
                
            }

        }

        private void cbMasterService_TextChanged(object sender, EventArgs e)
        {
            if (cbServiceType.SelectedIndex == 0 || cbServiceType.SelectedIndex == -1)
                return;

            OperationResult objOperationResult = new OperationResult();
            var id = int.Parse(cbServiceType.SelectedValue.ToString());
            Utils.LoadDropDownList(cbService, "Value1", "Id", BLL.Utils.GetSystemParameterByParentIdForCombo(ref objOperationResult, 119, id, null), DropDownListAction.All);

        }

        private void grd_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
        {
            if (grd.Selected.Rows.Count != 0)
            {
                string protocolName = grd.Selected.Rows[0].Cells["v_Protocol"].Value.ToString();
                float Total = 0;
                _protocolId = grd.Selected.Rows[0].Cells["v_ProtocolId"].Value.ToString();

                gbProtocolComponents.Text = string.Format("Comp. del Prot. < {0} >", protocolName);

                // Cargar componentes de un protocolo seleccionado
                OperationResult objOperationResult = new OperationResult();
                var dataListPc = _protocolBL.GetProtocolComponents(ref objOperationResult, _protocolId);

                grdProtocolComponent.DataSource = dataListPc;

                lblRecordCountProtocolComponents.Text = string.Format("Se encontraron {0} registros.", dataListPc.Count());

                if (dataListPc != null && dataListPc.Count > 0)
                {
                    // Costos totales
                    lblCostoProtocoloBasico.Text = dataListPc.FindAll(p => p.i_IsConditionalId == (int)SiNo.NO).Select(s => s.r_Price.Value).Sum().ToString("###,###.00");
                    lblCostoExamenesAdicionales.Text = dataListPc.FindAll(p => p.i_IsConditionalId == (int)SiNo.SI).Select(s => s.r_Price.Value).Sum().ToString("###,###.00");

                    if (lblCostoProtocoloBasico.Text != string.Empty && lblCostoExamenesAdicionales.Text != string.Empty)
                    {
                        var total = Convert.ToDouble(lblCostoProtocoloBasico.Text) + Convert.ToDouble(lblCostoExamenesAdicionales.Text);
                        lblCostoTotal.Text = total.ToString("###,###.00");
                        //lblCostoTotal.Text = string.Format("{0:0.00}", total);
                    }
                }


                if (objOperationResult.Success != 1)
                {
                    MessageBox.Show("Error en operación:" + System.Environment.NewLine + objOperationResult.ExceptionMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void grd_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            var frm = new frmProtocolEdit(string.Empty, "New");
            frm.MdiParent = this.MdiParent;
            frm.Show();

            // Refrescar grilla
            btnFilter_Click(sender, e);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            var frm = new frmProtocolEdit(_protocolId, "Edit");
            frm.MdiParent = this.MdiParent;
            frm.Show();

            // Refrescar grilla
            btnFilter_Click(sender, e);
            grdProtocolComponent.DataSource = new List<ProtocolComponentList>();
            lblCostoTotal.Text = "";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var frm = new frmProtocolEdit(_protocolId, "Clon");
            frm.MdiParent = this.MdiParent;
            frm.Show();

            if (frm.DialogResult == DialogResult.OK)
            {
                // Refrescar grilla
                btnFilter_Click(sender, e);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerarOS_Click(object sender, EventArgs e)
        {
            frmServiceOrderEdit frm = new frmServiceOrderEdit("", _protocolId, "New");
            frm.MdiParent = this.MdiParent;
            frm.Show();
        }

        private void txtProtocolName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BindGrid();
            }
        }

        private void btnMigrarComponentes_Click(object sender, EventArgs e)
        {
            try
            {
                var protocolList = GetData(0, null, "v_Protocol ASC", BuildFilterExpression());

                Infragistics.Documents.Excel.Workbook workbook1 = Infragistics.Documents.Excel.Workbook.Load(@"C:\Users\Administrator.DEV02\Desktop\Book1.xlsx");

                Infragistics.Documents.Excel.Worksheet worksheet1 = workbook1.Worksheets["COMPONENTES"];

                int rowCount = 2;
                //Validar que el excel no esta vacio

                var componentsList = new List<ComponentsForMigration>();
                ComponentsForMigration component = null;

                while (worksheet1.Rows[rowCount].Cells[0].Value != null)
                {
                    component = new ComponentsForMigration();

                    component.IDE_PERFIL = Convert.ToInt32(worksheet1.Rows[rowCount].Cells[0].Value);
                    component.NOMBRE_CLIENTE = worksheet1.Rows[rowCount].Cells[1].Value.ToString();
                    component.NOMBRE_PROTOCOLO = worksheet1.Rows[rowCount].Cells[2].Value.ToString();
                    component.NOMBRE_PERFIL = worksheet1.Rows[rowCount].Cells[3].Value.ToString();
                    component.IDE_SERVICIO = Convert.ToInt32(worksheet1.Rows[rowCount].Cells[4].Value);
                    component.NOMB_SERVICIO = worksheet1.Rows[rowCount].Cells[5].Value == null ? string.Empty : worksheet1.Rows[rowCount].Cells[5].Value.ToString();
                    component.PREC_VENTA = Convert.ToSingle(worksheet1.Rows[rowCount].Cells[6].Value);
                    component.ADICIONAL = Convert.ToInt32(worksheet1.Rows[rowCount].Cells[7].Value);
                    component.SEXO = Convert.ToInt32(worksheet1.Rows[rowCount].Cells[8].Value);
                    component.DES_SEXO = worksheet1.Rows[rowCount].Cells[9].Value.ToString();
                    component.EDAD_DESDE = Convert.ToInt32(worksheet1.Rows[rowCount].Cells[10].Value);
                    component.EDAD_HASTA = Convert.ToInt32(worksheet1.Rows[rowCount].Cells[11].Value);
                    component.v_ComponentId = worksheet1.Rows[rowCount].Cells[12].Value.ToString();

                    componentsList.Add(component);

                    rowCount++;
                }

                // Ordenar listas                
                protocolList = protocolList.FindAll(p => p.i_ValidInDays != null).OrderBy(o => o.i_ValidInDays).ToList();
                componentsList = componentsList.FindAll(p => p.v_ComponentId != "-1");  // discriminar los componentes k no existen para evitar problemas de integridad referencial
                componentsList.Sort((x, y) => x.IDE_PERFIL.CompareTo(y.IDE_PERFIL));

                List<ComponentsForMigration> componentsByProtocol = null;
                string[] componentId;
                int testFatigaSomnolensiaStres = 112;
                int testFobiaStres = 129;
                int pruebaEsfuerzo = 42;
                int triaje = 68;
                int ekg = 12;
                float price;
                int componentCount;
                int genderId;
                int operatorId;
                int isConditionalId;
                int age;

                ComponentsForMigration findTriaje = null;
                List<protocolcomponentDto> protocolcomponentListDTO = new List<protocolcomponentDto>();
                protocolcomponentDto protocolComponent = null;

                foreach (var p in protocolList)
                {
                    componentsByProtocol = componentsList.FindAll(w => w.IDE_PERFIL == p.i_ValidInDays);

                    // Buscar triaje
                    findTriaje = componentsByProtocol.Find(w => w.IDE_SERVICIO == triaje);
                    // si no existe crearlo obligatoriamente
                    if (findTriaje == null)
                    {
                        // FUNCIONES VITALES
                        protocolComponent = new protocolcomponentDto();
                        protocolComponent.v_ComponentId = Constants.FUNCIONES_VITALES_ID;
                        protocolComponent.v_ProtocolId = p.v_ProtocolId;
                        protocolComponent.i_Age = 0;
                        protocolComponent.i_OperatorId = -1;
                        protocolComponent.i_GenderId = 3;
                        protocolComponent.i_IsConditionalId = 0;
                        protocolComponent.r_Price = 0;

                        protocolcomponentListDTO.Add(protocolComponent);

                        // ANTROPOMETRIA
                        protocolComponent = new protocolcomponentDto();
                        protocolComponent.v_ComponentId = Constants.ANTROPOMETRIA_ID;
                        protocolComponent.v_ProtocolId = p.v_ProtocolId;
                        protocolComponent.i_Age = 0;
                        protocolComponent.i_OperatorId = -1;
                        protocolComponent.i_GenderId = 3;
                        protocolComponent.i_IsConditionalId = 0;
                        protocolComponent.r_Price = 0;

                        protocolcomponentListDTO.Add(protocolComponent);
                    }

                    foreach (var cp in componentsByProtocol)
                    {
                        componentId = cp.v_ComponentId.Split('|');
                        genderId = cp.SEXO;
                        age = cp.EDAD_DESDE;
                        isConditionalId = cp.ADICIONAL;
                        price = cp.PREC_VENTA;
                        operatorId = -1;
               
                        componentCount = componentId.Length;

                        if (componentCount > 1)  // HAY MAS DE 1 componentId
                        {
                            foreach (var cc in componentId)
                            {
                                // CONSIDERACIONES PARA EL SETEO DEL PRECIO
                                if (cp.IDE_SERVICIO == testFatigaSomnolensiaStres)
                                {
                                    price = cp.PREC_VENTA / 2;
                                }
                                else if (cp.IDE_SERVICIO == testFobiaStres)
                                {
                                    price = cp.PREC_VENTA / 1;
                                }
                                else
                                {
                                    if (price != 0)
                                        price = cp.PREC_VENTA / componentCount;
                                }

                                protocolComponent = new protocolcomponentDto();
                                protocolComponent.v_ComponentId = cc;
                                protocolComponent.v_ProtocolId = p.v_ProtocolId;
                                protocolComponent.i_Age = age;
                                protocolComponent.i_OperatorId = -1;
                                protocolComponent.i_GenderId = genderId;

                                if (isConditionalId == 1)
                                { protocolComponent.i_IsConditionalId = 1; }
                                else { protocolComponent.i_IsConditionalId = 0; }
                                
                                protocolComponent.r_Price = price;

                                protocolcomponentListDTO.Add(protocolComponent);
                            }
                        }
                        else
                        {
                            // CONSIDERACIONES PARA EL SETEO DEL (IS CONITIONAL)                       
                            if (isConditionalId == 1)
                            {
                                if (cp.SEXO == 0 || cp.DES_SEXO == "AMBOS")
                                { genderId = 3; }
                                else if (cp.SEXO == 1 || cp.DES_SEXO == "MASCULINO")
                                { genderId = 1; }
                                else if (cp.SEXO == 2 || cp.DES_SEXO == "FEMENINO")
                                { genderId = 2; }
                                    

                                if (cp.IDE_SERVICIO == pruebaEsfuerzo || cp.IDE_SERVICIO == ekg)
                                {
                                    age = cp.EDAD_DESDE;
                                    operatorId = 6;
                                    isConditionalId = 1;
                                }
                            }

                            protocolComponent = new protocolcomponentDto();
                            protocolComponent.v_ComponentId = componentId[0];
                            protocolComponent.v_ProtocolId = p.v_ProtocolId;
                            protocolComponent.i_Age = age;
                            protocolComponent.i_OperatorId = operatorId;
                            protocolComponent.i_GenderId = genderId;
                            protocolComponent.i_IsConditionalId = isConditionalId;
                            protocolComponent.r_Price = price;

                            protocolcomponentListDTO.Add(protocolComponent);

                        }
                    }

                }

                // Insertar
                Globals.ClientSession.i_CurrentExecutionNodeId = 1;
                _protocolBL.AddProtocolComponentForMigration(protocolcomponentListDTO, Globals.ClientSession.GetAsList());

                MessageBox.Show("Se crearon " + protocolcomponentListDTO.Count + " campos correctamente.", "!INFORMACIÓN", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "!ERROR", MessageBoxButtons.OK);
            }
          
        }

        private void btnReporte_Click(object sender, EventArgs e)
        {
            //"Matriz de datos <Empresa Cliente> de <fecha inicio> a <fecha fin>"
            string NombreArchivo = "";
            var NombreProtocolo = grd.Selected.Rows[0].Cells["v_Protocol"].Value.ToString();

            NombreArchivo = "Protocolo: " +NombreProtocolo;
         
            NombreArchivo = NombreArchivo.Replace("/", "_");
            NombreArchivo = NombreArchivo.Replace(":", "_");

            saveFileDialog1.FileName = NombreArchivo;
            saveFileDialog1.Filter = "Files (*.xls;*.xlsx;*)|*.xls;*.xlsx;*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.ultraGridExcelExporter1.Export(this.grdProtocolComponent, saveFileDialog1.FileName);
                MessageBox.Show("Se exportaron correctamente los datos.", " ¡ INFORMACIÓN !", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
          
    }
}
