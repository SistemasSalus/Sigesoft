using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sigesoft.Node.WinClientFast.UI
{
    public partial class frmSelecConsultorio : Form
    {
        string strConn;

        public frmSelecConsultorio(string pstrConn)
        {
            InitializeComponent();
            strConn = pstrConn;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            //if (ddlComponentId.Select() == null)
            //{
            //    return;
            //}
            string o = ddlComponentId.Text;

            frmConsultorio frm = new frmConsultorio(strConn, o);

            frm.MdiParent = this.MdiParent;
            frm.Show();
            this.Hide();
        }

        private void frmSelecConsultorio_Load(object sender, EventArgs e)
        {
            ddlComponentId.Items.Add("--Seleccione--");
            ddlComponentId.Items.Add("MEDICINA");
            ddlComponentId.Items.Add("ALTURA 1.8M");
            ddlComponentId.Items.Add("TRIAJE");

            ddlComponentId.SelectedIndex = 0;
        }
    }
}
