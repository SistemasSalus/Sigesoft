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
using System.Drawing.Imaging;
using Infragistics.Win.UltraWinGrid;

namespace Sigesoft.Node.WinClient.UI
{
    public partial class frmNroTickes : Form
    {
        public int? NroTickets = 0;
        public frmNroTickes()
        {
            InitializeComponent();
        }

        private void frmNroTickes_Load(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            NroTickets = txtNro.Text == "" ? 0 : int.Parse(txtNro.Text);

            this.DialogResult = DialogResult.OK;
        }
    }
}
