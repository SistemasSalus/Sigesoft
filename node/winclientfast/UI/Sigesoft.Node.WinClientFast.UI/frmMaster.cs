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
    public partial class frmMaster : Form
    {
        public frmMaster()
        {
            InitializeComponent();
        }

        private void frmMaster_Load(object sender, EventArgs e)
        {

        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro que desea cerrar sesión?", "Pregunta", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (DialogResult.Yes == result)
            {
                this.Hide();
                frmLogin frm = new frmLogin();
                frm.ShowDialog();
                return;
            }
            else
            {
                return;
            }
        }

        private void salirDelSistemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
            return;
        }

        private void consultorioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmSelecConsultorio("SIGESOftConnStr");
            frm.MdiParent = this;
            frm.Show();
        }
    }
}
