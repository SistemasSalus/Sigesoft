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
    public partial class frmSoporteCovid19 : Form
    {
        public frmSoporteCovid19()
        {
            InitializeComponent();
        }

        private void btnActualizarReportesCovid19_Click(object sender, EventArgs e)
        {
            var sedesId = new List<string>();

            string[] miList = chkSedes.CheckedItems.OfType<object>().Select(li => li.ToString()).ToArray();
                var lst = new List<X>(chkSedes.CheckedItems.Cast<X>());
          
        }

        private void frmSoporteCovid19_Load(object sender, EventArgs e)
        {
            chkSedes.DisplayMember = "Text";
            chkSedes.ValueMember = "Value";
            chkSedes.Items.Insert(0, new { Text = "CONO NORTE", Value = "100" });
            chkSedes.Items.Insert(1, new { Text = "RIMAC", Value = "101" });
            chkSedes.Items.Insert(2, new { Text = "ATE", Value = "102" });
            chkSedes.Items.Insert(3, new { Text = "CONO SUR", Value = "103" });
            chkSedes.Items.Insert(4, new { Text = "CALLAO", Value = "104" });
            chkSedes.Items.Insert(5, new { Text = "VEGUETA", Value = "105" });
            chkSedes.Items.Insert(6, new { Text = "RUTA2", Value = "106" });
            chkSedes.Items.Insert(7, new { Text = "RUTA3", Value = "107" });
        }

        public class X {
            public string Text { get; set; }
            public string Value { get; set; }
        }
    }
}
