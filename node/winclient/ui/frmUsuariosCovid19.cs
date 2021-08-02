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
    public partial class frmUsuariosCovid19 : Form
    {
        private string _id = "";
        SystemParameterBL oSystemParameterBL = new SystemParameterBL();

        public frmUsuariosCovid19()
        {
            InitializeComponent();
        }

        private void btnNuevoUsuario_Click(object sender, EventArgs e)
        {
            var frm = new frmNuevoUsuarioCovid19();
            frm.ShowDialog();
            if (frm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                filtrar();
            }
        }

        private void frmUsuariosCovid19_Load(object sender, EventArgs e)
        {
            filtrar();
        }

        private void filtrar()
        {
            OperationResult objOperationResult = new OperationResult();
            var listaUsuarios = new List<UsuarioCovid19BE>();
            var oUsuarios = BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 407, null);
            foreach (var item in oUsuarios)
            {
                var arr = item.Value1.Split('|');
                var oUsuarioCovid19BE = new UsuarioCovid19BE();
                oUsuarioCovid19BE.id = int.Parse(item.Id);
                oUsuarioCovid19BE.usuario = arr[0];
                oUsuarioCovid19BE.password = arr[1];
                oUsuarioCovid19BE.tecnico = arr[2];
                oUsuarioCovid19BE.estado = bool.Parse(item.Value2);
                listaUsuarios.Add(oUsuarioCovid19BE);
            }

            grdData.DataSource = listaUsuarios;
        }

        private void grdData_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
        {
            btnActivar.Enabled=
            btnDesactivar.Enabled =
            (grdData.Selected.Rows.Count > 0);

            if (grdData.Selected.Rows.Count == 0)
                return;

            _id = grdData.Selected.Rows[0].Cells["id"].Value.ToString();

        }

        private void btnActivar_Click(object sender, EventArgs e)
        {
            oSystemParameterBL.ActualizarFlagUsuarioCovid19(int.Parse(_id), "true");
            filtrar();
        }

        private void btnDesactivar_Click(object sender, EventArgs e)
        {
            oSystemParameterBL.ActualizarFlagUsuarioCovid19(int.Parse(_id), "false");
            filtrar();
        }

    }
}
