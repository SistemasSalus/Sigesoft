﻿using FineUI;
using Sigesoft.Common;
using Sigesoft.Server.WebClientAdmin.BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sigesoft.Server.WebClientAdmin.UI.ExternalUser
{
    public partial class FRM031C : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSaveRefresh.OnClientClick = winEdit1.GetSaveStateReference(hfRefresh.ClientID) + winEdit1.GetShowReference("../ExternalUser/FRM031D.aspx");
                //btnClose.OnClientClick = PageContext.Re
                List<Sigesoft.Node.WinClient.BE.ServiceComponentList> fichasMedicas = new List<Sigesoft.Node.WinClient.BE.ServiceComponentList>() 
            {
                new Sigesoft.Node.WinClient.BE.ServiceComponentList { v_ComponentName = "Anexo 312", v_ComponentId = Constants.INFORME_ANEXO_312 },
                new Sigesoft.Node.WinClient.BE.ServiceComponentList { v_ComponentName = "Anexo 16", v_ComponentId = Constants.INFORME_ANEXO_7C },
                //new Sigesoft.Node.WinClient.BE.ServiceComponentList { v_ComponentName = "Informe Medico", v_ComponentId = Constants.INFORME_FICHA_MEDICA_TRABAJADOR }, 
                //new Sigesoft.Node.WinClient.BE.ServiceComponentList { v_ComponentName = "Ficha Examen Clínico", v_ComponentId = Constants.INFORME_CLINICO },   
                //new Sigesoft.Node.WinClient.BE.ServiceComponentList { v_ComponentName = "Laboratorio Clínico", v_ComponentId = Constants.INFORME_LABORATORIO_CLINICO },  
            };

                informesSeleccionados.DataTextField = "v_ComponentName";
                informesSeleccionados.DataValueField = "v_ComponentId";
                informesSeleccionados.DataSource = fichasMedicas;
                informesSeleccionados.DataBind();
            }
        }

        protected void informesSeleccionados_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> lista = new List<string>();

            int selectedCount = informesSeleccionados.SelectedItemArray.Length;

            //var lista = List<ServiceComponentList>()
            foreach (var item in informesSeleccionados.SelectedItemArray)
            {
                lista.Add(item.Value);
            }

            Session["objListaExamenes"] = lista;

            if (selectedCount > 0)
            {
                btnSaveRefresh.Enabled = true;
            }
            else
            {
                btnSaveRefresh.Enabled = false;
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());

        }

    }
}