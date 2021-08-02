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
using System.IO;

namespace Sigesoft.Server.WebClientAdmin.UI.ExternalUser
{
    public partial class FRM031 : System.Web.UI.Page
    {
        SystemParameterBL _objSystemParameterBL = new SystemParameterBL();
        ProtocolBL _objProtocolBL = new ProtocolBL();
        ServiceBL _ServiceBL = new ServiceBL();
        ResultsWebxSystemUserBLDA _ResultsWeb = new ResultsWebxSystemUserBLDA();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OperationResult objOperationResult = new OperationResult();
                dpFechaInicio.SelectedDate = DateTime.Now.AddDays(-90); //DateTime.Parse("25/07/2014");
                dpFechaFin.SelectedDate = DateTime.Now;// DateTime.Parse("25/07/2014");
                LoadComboBox();
                btnNewCertificado.OnClientClick = winEdit1.GetSaveStateReference(hfRefresh.ClientID) + winEdit1.GetShowReference("../ExternalUser/FRM031H.aspx");
                btnNewFichaOcupacional.OnClientClick = winEdit2.GetSaveStateReference(hfRefresh.ClientID) + winEdit2.GetShowReference("../ExternalUser/FRM031H2.aspx");
                btnNewExamenes.OnClientClick = winEdit3.GetSaveStateReference(hfRefresh.ClientID) + winEdit3.GetShowReference("../ExternalUser/FRM031H3.aspx");

                btnPlacaRx.OnClientClick = winEdit4.GetSaveStateReference(hfRefresh.ClientID) + winEdit4.GetShowReference("../ExternalUser/frmPlacasRx.aspx");
                btnFichaCovid19.OnClientClick = winEdit5.GetSaveStateReference(hfRefresh.ClientID) + winEdit5.GetShowReference("../ExternalUser/frmFichaCoviv19.aspx");

                var ObtenerEmpresasCliente = new ProtocolBL().GetOrganizationCustumerByProtocolSystemUser_(ref objOperationResult, ((ClientSession)Session["objClientSession"]).i_SystemUserId);
                //Session["EmpresaClienteId"] = ObtenerEmpresasCliente[0].IdEmpresaCliente;

                Utils.LoadDropDownList(ddlEmpresa, "CustomerOrganizationName", "IdEmpresaCliente", ObtenerEmpresasCliente, DropDownListAction.All);

                if (ObtenerEmpresasCliente.ToList().Count >0)
                {
                    ddlEmpresa.SelectedValue = ObtenerEmpresasCliente[0].IdEmpresaCliente;
                    Utils.LoadDropDownList(ddlProtocolo, "Value1", "Id", _objProtocolBL.GetProtocolBySystemUserByEmpresaId(ref objOperationResult, ObtenerEmpresasCliente[0].IdEmpresaCliente), DropDownListAction.All);
                }
            }
        }

        private void LoadComboBox()
        {
            OperationResult objOperationResult = new OperationResult();
            Utils.LoadDropDownList(ddlAptitud, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 124), DropDownListAction.All);
            Utils.LoadDropDownList(ddlTipoESO, "Value1", "Id", _objSystemParameterBL.GetSystemParameterForCombo(ref objOperationResult, 118), DropDownListAction.All);
            //Utils.LoadDropDownList(ddlProtocolo, "Value1", "Id", _objProtocolBL.GetProtocolBySystemUser(ref objOperationResult, ((ClientSession)Session["objClientSession"]).i_SystemUserId), DropDownListAction.All);

        }

        protected void grdData_RowCommand(object sender, GridCommandEventArgs e)
        {

        }

        protected void grdData_PreRowDataBound(object sender, GridPreRowEventArgs e)
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
            if (ddlTipoESO.SelectedValue.ToString() != "-1") Filters.Add("i_TypeEsoId==" + ddlTipoESO.SelectedValue);
            if (ddlAptitud.SelectedValue.ToString() != "-1") Filters.Add("i_AptitudeId==" + ddlAptitud.SelectedValue);
            if (!string.IsNullOrEmpty(txtTrabajador.Text)) Filters.Add("v_Trabajador.Contains(\"" + txtTrabajador.Text.ToUpper().Trim() + "\")");
            if (ddlProtocolo.SelectedValue.ToString() != "-1") Filters.Add("v_ProtocolId==" + "\"" + ddlProtocolo.SelectedValue + "\"");
            if (!string.IsNullOrEmpty(txtHCL.Text)) Filters.Add("v_HCL==" + "\"" + txtHCL.Text + "\"");
            Filters.Add("v_CustomerOrganizationId==" + "\"" + ddlEmpresa.SelectedValue.ToString() + "\"");
            //Filters.Add("v_CustomerOrganizationId==" + "\"" + Session["EmpresaClienteId"].ToString() + "\"");

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
            grdData.PageIndex = 0;
            this.BindGrid();

        }

        private List<MyListWeb> LlenarLista()
        {
            List<MyListWeb> lista = new List<MyListWeb>();
            int selectedCount = grdData.SelectedRowIndexArray.Length;
            if (selectedCount > 0)
            {
                btnNewCertificado.Enabled = (bool)Session["CertificadoAptitud"];
                btnNewFichaOcupacional.Enabled = (bool)Session["FichaOcupacional"];
                if (selectedCount > 1)
                {
                    btnPlacaRx.Enabled = false;
                    btnFichaCovid19.Enabled = false;
                    btnNewExamenes.Enabled = false;
                }
                else
                {
                    btnNewExamenes.Enabled = (bool)Session["Examenes"];
                    btnPlacaRx.Enabled = true;
                    btnFichaCovid19.Enabled = true;
                }

            }
            else
            {
                btnNewCertificado.Enabled = false;
                btnNewFichaOcupacional.Enabled = false;
                btnNewExamenes.Enabled = false;

                btnPlacaRx.Enabled = false;
                btnFichaCovid19.Enabled = false;
            }

            for (int i = 0; i < selectedCount; i++)
            {
                int rowIndex = grdData.SelectedRowIndexArray[i];

                var dataKeys = grdData.DataKeys[rowIndex];
                //for (int j = 0; j < dataKeys.Length; j++)
                //{
                //lista.Add( new MyListWeb< [0].ToString());
                lista.Add(new MyListWeb
                {
                    IdServicio = dataKeys[0].ToString(),
                    IdPaciente = dataKeys[1].ToString(),
                    EmpresaCliente = dataKeys[2].ToString(),
                    Paciente = dataKeys[3].ToString(),
                });

                //}

            }

            Session["objLista"] = lista;

            return lista;
        }

        private void BindGrid()
        {
            string strFilterExpression = Convert.ToString(Session["strFilterExpression"]);
            grdData.DataSource = GetData(grdData.PageIndex, grdData.PageSize, "v_ServiceId", strFilterExpression);
            if (((ClientSession)Session["objClientSession"]).i_SystemUserId == 7 || ((ClientSession)Session["objClientSession"]).i_SystemUserId == 8)
            {
                grdData.DataSource = GetData(grdData.PageIndex, grdData.PageSize, "v_ServiceId", strFilterExpression);
            }
            else
            {
                string ppaciente, pempresa, pprotocolo;
                if (txtTrabajador.Text.ToString() == "") { ppaciente = "-1"; } else { ppaciente = txtTrabajador.Text.ToString(); }
                if (ddlEmpresa.SelectedText.ToString() == "--Todos--") { pempresa = "-1"; } else { pempresa = ddlEmpresa.SelectedText.ToString(); }
                if (ddlProtocolo.SelectedText.ToString() == "--Todos--") { pprotocolo = "-1"; } else { pprotocolo = ddlProtocolo.SelectedText.ToString(); }

                grdData.DataSource = _ResultsWeb.GetResultsWebxSystemUser("SIGESOftConnStr", dpFechaInicio.SelectedDate.Value, dpFechaFin.SelectedDate.Value.AddDays(1),
                                                                            ((ClientSession)Session["objClientSession"]).i_SystemUserId, ppaciente, pempresa, pprotocolo);

                lblContador.Text = "Se encontraron " + grdData.Items.Count().ToString() + " registros";
            }
            grdData.DataBind();
        }

        private List<ServiceList> GetData(int pintPageIndex, int pintPageSize, string pstrSortExpression, string pstrFilterExpression)
        {
            OperationResult objOperationResult = new OperationResult();
            var _objData = _ServiceBL.GetService(ref objOperationResult, pintPageIndex, pintPageSize, pstrSortExpression, pstrFilterExpression, dpFechaInicio.SelectedDate.Value, dpFechaFin.SelectedDate.Value.AddDays(1));
            lblContador.Text = "Se encontraron " + _objData.Count().ToString() + " registros";

            if (objOperationResult.Success != 1)
            {
                Alert.ShowInTop("Error en operación:" + System.Environment.NewLine + objOperationResult.ExceptionMessage);
            }

            return _objData;
        }

        protected void grdData_RowClick(object sender, GridRowClickEventArgs e)
        {
            LlenarLista();
        }

        protected void winEdit1_Close(object sender, EventArgs e)
        {

        }

        protected void winEdit2_Close(object sender, EventArgs e)
        {
            if (Session["EliminarArchivo"] != null)
            {
                File.Delete(Session["EliminarArchivo"].ToString());
            }
        }

        protected void winEdit3_Close(object sender, EventArgs e)
        {
            if (Session["EliminarArchivo"] != null)
            {
                File.Delete(Session["EliminarArchivo"].ToString());
            }

        }

        protected void winEdit4_Close(object sender, EventArgs e)
        {
          

        }

        protected void winEdit5_Close(object sender, EventArgs e)
        {


        }

        protected void grdData_PageIndexChange(object sender, FineUI.GridPageEventArgs e)
        {
            grdData.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void ddlEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEmpresa.SelectedValue.ToString()!="-1")
            {
                OperationResult objOperationResult = new OperationResult();
                Utils.LoadDropDownList(ddlProtocolo, "Value1", "Id", _objProtocolBL.GetProtocolBySystemUserByEmpresaId(ref objOperationResult, ddlEmpresa.SelectedValue.ToString()), DropDownListAction.All);
            }
        }
    }
}