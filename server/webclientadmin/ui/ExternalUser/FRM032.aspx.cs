using FineUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sigesoft.Server.WebClientAdmin.BLL;
using Sigesoft.Server.WebClientAdmin.BE;
using Sigesoft.Common;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;

namespace Sigesoft.Server.WebClientAdmin.UI.ExternalUser
{
    public partial class FRM032 : System.Web.UI.Page
    {
        SystemParameterBL _objSystemParameterBL = new SystemParameterBL();
        ProtocolBL _objProtocolBL = new ProtocolBL();
        ServiceBL _ServiceBL = new ServiceBL();
        CalendarBL _CalendarBL = new CalendarBL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {              
                LoadComboBox();
            }
        }
        private void LoadComboBox()
        {
            OperationResult objOperationResult = new OperationResult();
            Utils.LoadDropDownList(ddltipoServicioBus, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 119), DropDownListAction.All);
            Utils.LoadDropDownList(ddlColaBus, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 120), DropDownListAction.All);
            Utils.LoadDropDownList(ddlEstadoCitaBus, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 122), DropDownListAction.All);
            var ObtenerEmpresaCliente = new ProtocolBL().GetOrganizationCustumerByProtocolSystemUser(ref objOperationResult, ((ClientSession)Session["objClientSession"]).i_SystemUserId);
            txtEmpresa.Text = ObtenerEmpresaCliente.CustomerOrganizationName;
            Session["EmpresaClienteId"] = ObtenerEmpresaCliente.IdEmpresaCliente;

            dpFechaInicioBus.SelectedDate = DateTime.Now;
        }

        protected void winEdit_Close(object sender, EventArgs e)
        {
        }

        protected void grdData_RowCommand(object sender, GridCommandEventArgs e)
        {

        }

        protected void grdData_PreRowDataBound(object sender, GridPreRowEventArgs e)
        {
        }

        protected void grdData_PageIndexChange(object sender, FineUI.GridPageEventArgs e)
        {

        }

        protected void ddlProtocolo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtEmpresa.Text = _objProtocolBL.GetNameOrganizationCustomer(ddlProtocolo.SelectedValue.ToString());
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {

            // Get the filters from the UI
            List<string> Filters = new List<string>();
            if (ddltipoServicioBus.SelectedValue.ToString() != "-1") Filters.Add("i_ServiceTypeId==" + ddltipoServicioBus.SelectedValue);
            if (ddlColaBus.SelectedValue.ToString() != "-1") Filters.Add("i_LineStatusId==" + ddlColaBus.SelectedValue);
            if (ddlEstadoCitaBus.SelectedValue.ToString() != "-1") Filters.Add("i_CalendarStatusId==" + ddlEstadoCitaBus.SelectedValue);
            if (!string.IsNullOrEmpty(txtTrabajadorBus.Text)) Filters.Add("v_Pacient.Contains(\"" + txtTrabajadorBus.Text.Trim() + "\")");
            Filters.Add("v_CustomerOrganizationId==" + "\"" + Session["EmpresaClienteId"].ToString() + "\"");
            string strFilterExpression = null;
            if (Filters.Count > 0)
            {
                foreach (string item in Filters)
                {
                    strFilterExpression = strFilterExpression + item + " && ";
                }
                strFilterExpression = strFilterExpression.Substring(0, strFilterExpression.Length - 4);
            }

            // Save the Filter expression in the Session
            Session["strFilterExpression"] = strFilterExpression;

            // Refresh the grid
            //grdData.PageIndex = 0;
            this.BindGrid();

        }

        protected void btnNewCertificado_Click(object sender, EventArgs e)
        {

          
            Response.Redirect("../ExternalUser/FRM031A.aspx");

        }

        private void BindGrid()
        {
            string strFilterExpression = Convert.ToString(Session["strFilterExpression"]);

            var Lista = GetData(grdData.PageIndex, grdData.PageSize, "", strFilterExpression);
            if (Lista != null)
            {
                txtNroAtenciones.Text = Lista.FindAll(p => p.i_LineStatusId == (int)Sigesoft.Common.LineStatus.EnCircuito && p.i_ServiceStatusId == (int)Sigesoft.Common.ServiceStatus.Iniciado).Count().ToString();
                txtNroAtender.Text = Lista.FindAll(p => p.i_LineStatusId == (int)Sigesoft.Common.LineStatus.FueraCircuito && p.i_ServiceStatusId == (int)Sigesoft.Common.ServiceStatus.PorIniciar).Count().ToString();
                txtNroAtendidos.Text = Lista.FindAll(p => p.i_LineStatusId == (int)Sigesoft.Common.LineStatus.EnCircuito && (p.i_ServiceStatusId == (int)Sigesoft.Common.ServiceStatus.EsperandoAptitud || p.i_ServiceStatusId == (int)Sigesoft.Common.ServiceStatus.Culminado)).Count().ToString();
            }
            else
            {
                txtNroAtenciones.Text = "0";
                txtNroAtender.Text = "0";
                txtNroAtendidos.Text = "0";
            }

            grdData.DataSource = Lista;
            grdData.DataBind();
        }

        private List<CalendarList> GetData(int pintPageIndex, int pintPageSize, string pstrSortExpression, string pstrFilterExpression)
        {
            OperationResult objOperationResult = new OperationResult();
            var _objData = _CalendarBL.GetCalendarsPagedAndFiltered(ref objOperationResult, pintPageIndex, pintPageSize, pstrSortExpression, pstrFilterExpression, dpFechaInicioBus.SelectedDate.Value, dpFechaInicioBus.SelectedDate.Value.Date.AddDays(1));

            if (objOperationResult.Success != 1)
            {
                Alert.ShowInTop("Error en operación:" + System.Environment.NewLine + objOperationResult.ExceptionMessage);
            }

            return _objData;
        }

        protected void grdData_RowClick(object sender, GridRowClickEventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            ServiceBL objServiceBL = new ServiceBL();
            List<ServiceComponentList> ListServiceComponent = new List<ServiceComponentList>();

            int index = e.RowIndex;
            var dataKeys = grdData.DataKeys[index];

            ListServiceComponent = objServiceBL.GetServiceComponents(ref objOperationResult, dataKeys[0].ToString());

            txtTrabajador.Text = dataKeys[1].ToString();
            txtEmpresaTrabajo.Text = dataKeys[2].ToString();
            txtServicio.Text = dataKeys[3].ToString();
            txttipoEso.Text = dataKeys[4].ToString();
            grdData2.DataSource = ListServiceComponent;
            grdData2.DataBind();

        }

        protected void grdData2_RowDataBound(object sender, GridRowEventArgs e)
        {
            
            DataRowView row = e.DataItem as DataRowView;
            if (row != null)
            {
                //e.Values[1] = String.Format("Bound - {0}", row["MyValue"]);
                int entranceYear = Convert.ToInt32(row["v_CalendarStatusName"]);

                if (entranceYear >= 2006)
                {
                    //highlightRows.Text += e.RowIndex.ToString() + ",";
                }
            }
        }

        protected void grdData_RowSelect(object sender, GridRowSelectEventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            ServiceBL objServiceBL = new ServiceBL();
            List<ServiceComponentList> ListServiceComponent = new List<ServiceComponentList>();

            int index = e.RowIndex;
            var dataKeys = grdData.DataKeys[index];

            ListServiceComponent = objServiceBL.GetServiceComponents(ref objOperationResult, dataKeys[0].ToString());

            txtTrabajador.Text = dataKeys[1].ToString();
            txtEmpresaTrabajo.Text = dataKeys[2].ToString();
            txtServicio.Text = dataKeys[3].ToString();
            txttipoEso.Text = dataKeys[4].ToString();
            grdData2.DataSource = ListServiceComponent;
            grdData2.DataBind();
        }
    }
}