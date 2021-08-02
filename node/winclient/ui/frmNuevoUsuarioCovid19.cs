using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sigesoft.Node.WinClient.UI
{
    public partial class frmNuevoUsuarioCovid19 : Form
    {
        SystemParameterBL oSystemParameterBL = new SystemParameterBL();
        public frmNuevoUsuarioCovid19()
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            systemparameterDto objEntity = new systemparameterDto();

            if (validar())
            {
                // Populate the entity
                objEntity.i_GroupId = 407;
                objEntity.i_ParameterId = ObtenerIdParameter(407);
                objEntity.v_Value1 = armarValue1();
                objEntity.v_Value2 = "true";
                objEntity.i_ParentParameterId = -1;
                // Save the data                  
                oSystemParameterBL.AddSystemParameter(ref objOperationResult, objEntity, Globals.ClientSession.GetAsList());

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Por favor registrar todos los campos");
            }           
                    
        }

        private bool validar()
        {
            if (string.IsNullOrEmpty(txtUsuario.Text))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(txtPassword.Text))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(txtPassword.Text))
            {
                return false;
            }
            else {
                return true;
            }
        }

        private string armarValue1()
        {
            return string.Format("{0}|{1}|{2}",txtUsuario.Text.Trim(),txtPassword.Text.Trim(),txtTecnico.Text.Trim());
        }

        private int ObtenerIdParameter(int gruopId)
        {
            return oSystemParameterBL.ObtenerUltimoIdParamenter(gruopId);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {

        }
    }
}
