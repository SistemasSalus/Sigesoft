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
    public partial class frmReporteCovid19 : Form
    {
        string _empresa = "";
        List<string> _servicios = new List<string>();
        public frmReporteCovid19(string empresa, List<string>Servicios)
        {
            InitializeComponent();

            _empresa = empresa;
            _servicios = Servicios;
            txtEmpresa.Text = _empresa;
        }

        private void btnReporte_Click(object sender, EventArgs e)
        {
            try
            {
                string NombreArchivo = "";

                NombreArchivo = "Reporte Covid-19 Empresa: " + _empresa;

                NombreArchivo = NombreArchivo.Replace("/", "_");
                NombreArchivo = NombreArchivo.Replace(":", "_");

                saveFileDialog1.FileName = NombreArchivo;
                saveFileDialog1.Filter = "Files (*.xls;*.xlsx;*)|*.xls;*.xlsx;*";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.ultraGridExcelExporter1.Export(this.grdData, saveFileDialog1.FileName);
                    MessageBox.Show("Se exportaron correctamente los datos.", " ¡ INFORMACIÓN !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
         
        }

        private void frmReporteCovid19_Load(object sender, EventArgs e)
        {
            ServiceBL bl = new ServiceBL();
            var lista = new List<ReporteExcelCovid19>(); 
            foreach (var item in _servicios)
            {
                lista.Add(bl.ReporteExcelCovid19(item));
                
            }
            grdData.DataSource = lista;
        }
    }
}
