using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sigesoft.Node.WinClientFast.DA;
using Sigesoft.Node.WinClientFast.BE;
using Sigesoft.Node.WinClient.UI;

namespace Sigesoft.Node.WinClientFast.UI
{
    public partial class frmConsultorio : Form
    {
        private ConsultorioBLDA ConsultorioBLDA = new ConsultorioBLDA();

        string strConn;
        string _serviceId;
        string _componentId;
        string _componentName;
        string _IsCall;
        int categoria;

        private List<PacienteAgendado> lsPacienteAgendados = new List<PacienteAgendado>();

        public frmConsultorio(string pstrConn, string pstrComponentName)
        {
            InitializeComponent();

            _componentName = pstrComponentName;
            strConn = pstrConn;

            timer1.Interval = 1000;
            timer1.Start();

        }

        private void frmConsultorio_Load(object sender, EventArgs e)
        {
            lblNameComponent.Text = _componentName;

            #region SWITCH Categoria
            switch (_componentName)
            { 
                case "LABORATORIO":
                    categoria = 1;
                    break;
                case "ODONTOLOGÍA":
                    categoria = 2;
                    break;
                case "CARDIOLOGÍA":
                    categoria = 5;
                    break;
                case "RAYOS X":
                    categoria = 6;
                    break;
                case "PSICOLOGÍA":
                    categoria = 7;
                    break;
                case "TRIAJE":
                    categoria = 10;
                    break;
                case "MEDICINA":
                    categoria = 11;
                    break;
                case "NEUMOLOGÍA":
                    categoria = 12;
                    break;
                case "INMUNIZACIONES":
                    categoria = 13;
                    break;
                case "OFTALMOLOGÍA":
                    categoria = 14;
                    break;
                case "AUDIOMETRÍA":
                    categoria = 19;
                    break;
                case "PSICOPRUEBAS":
                    categoria = 20;
                    break;
                case "ADMINISTRACIÓN":
                    categoria = 21;
                    break;
                case "ALTURA 1.8M":
                    categoria = 22;
                    break;
                case "PSICOSENSOMÉTRICO":
                    categoria = 23;
                    break;
            }
            #endregion

            LoadGridData();

            lsPacienteAgendados = new List<PacienteAgendado>();
        }

        private void LoadGridData()
        {
            var ListaAgendados = ConsultorioBLDA.GetPacienteAgendado(strConn, categoria);
            ugListaCola.DataSource = ListaAgendados;

        }

        private void btnLlamar_Click(object sender, EventArgs e)
        {
            if (ugListaCola.Selected.Rows.Count == 0)
                return;

            PacienteAgendado pacienteAgendado = new PacienteAgendado();

            pacienteAgendado.Paciente = ugListaCola.Selected.Rows[0].Cells["Paciente"].Value.ToString();
            pacienteAgendado.NroDocum = ugListaCola.Selected.Rows[0].Cells["NroDocum"].Value.ToString();
            pacienteAgendado.Edad = (int)ugListaCola.Selected.Rows[0].Cells["Edad"].Value;
            pacienteAgendado.Empleador = ugListaCola.Selected.Rows[0].Cells["Empleador"].Value.ToString();
            pacienteAgendado.Cliente = ugListaCola.Selected.Rows[0].Cells["Cliente"].Value.ToString();
            pacienteAgendado.Protocolo = ugListaCola.Selected.Rows[0].Cells["Protocolo"].Value.ToString();
            pacienteAgendado.v_Value1 = ugListaCola.Selected.Rows[0].Cells["v_Value1"].Value.ToString();
            pacienteAgendado.v_CurrentOccupation = ugListaCola.Selected.Rows[0].Cells["v_CurrentOccupation"].Value.ToString();
            pacienteAgendado.v_ServiceId = ugListaCola.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();
            pacienteAgendado.v_PersonId = ugListaCola.Selected.Rows[0].Cells["v_PersonId"].Value.ToString();

            bool pac = lsPacienteAgendados.Any(x => x.v_ServiceId == pacienteAgendado.v_ServiceId);

            if (pac == true)
            {
                MessageBox.Show("El Pacienta ya está siendo llamado", "Atención!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                lsPacienteAgendados.Add(pacienteAgendado);
            }

            ugListaLlamados.DataSource = null;
            ugListaLlamados.DataSource = lsPacienteAgendados;

            timer1.Start();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadGridData();
        }

        private void btnRellamar_Click(object sender, EventArgs e)
        {
            if (ugListaLlamados.Selected.Rows.Count == 0)
                return;

        }

        private void btnAtender_Click(object sender, EventArgs e)
        {
            if (ugListaLlamados.Selected.Rows.Count == 0)
                return;

            PacienteAgendado pacienteLlamado = new PacienteAgendado();
            pacienteLlamado.Paciente = ugListaLlamados.Selected.Rows[0].Cells["Paciente"].Value.ToString();
            pacienteLlamado.NroDocum = ugListaLlamados.Selected.Rows[0].Cells["NroDocum"].Value.ToString();
            pacienteLlamado.Edad = (int)ugListaLlamados.Selected.Rows[0].Cells["Edad"].Value;
            pacienteLlamado.Empleador = ugListaLlamados.Selected.Rows[0].Cells["Empleador"].Value.ToString();
            pacienteLlamado.Cliente = ugListaLlamados.Selected.Rows[0].Cells["Cliente"].Value.ToString();
            pacienteLlamado.Protocolo = ugListaLlamados.Selected.Rows[0].Cells["Protocolo"].Value.ToString();
            pacienteLlamado.v_Value1 = ugListaLlamados.Selected.Rows[0].Cells["v_Value1"].Value.ToString();
            pacienteLlamado.v_CurrentOccupation = ugListaLlamados.Selected.Rows[0].Cells["v_CurrentOccupation"].Value.ToString();
            pacienteLlamado.v_ServiceId = ugListaLlamados.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();
            pacienteLlamado.v_PersonId = ugListaLlamados.Selected.Rows[0].Cells["v_PersonId"].Value.ToString();

            ////haber si funca
            //Form childForm = new Sigesoft.Node.WinClient.UI.Operations.frmEsoStatic(_serviceId);
            ////childForm.Text = "Ventana " + childFormNumber++;
            //childForm.Show();
            //this.Close(); // si quiere serrar el formulario actual
        }

        private void btnLiberar_Click(object sender, EventArgs e)
        {
            if (ugListaLlamados.Selected.Rows.Count == 0)
                return;

            ugListaLlamados.Selected.Rows[0].Delete(false);
        }

        private void ugListaCola_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
        {
           
        }

        private void ugListaLlamados_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            btnRefresh_Click(sender, e);
        }

        private void ugListaCola_ClickCell(object sender, Infragistics.Win.UltraWinGrid.ClickCellEventArgs e)
        {
            timer1.Stop();

            if (ugListaCola.Selected.Rows.Count == 0)
                return;

            txtTrabajador.Text = ugListaCola.Selected.Rows[0].Cells["Paciente"].Value.ToString();
            txtEdad.Text = ugListaCola.Selected.Rows[0].Cells["Edad"].Value.ToString();
            txtDocumento.Text = ugListaCola.Selected.Rows[0].Cells["NroDocum"].Value.ToString();
            txtPuesto.Text = ugListaCola.Selected.Rows[0].Cells["v_CurrentOccupation"].Value.ToString();
            txtEmpTrabajo.Text = ugListaCola.Selected.Rows[0].Cells["Empleador"].Value.ToString();
            txtEmpCliente.Text = ugListaCola.Selected.Rows[0].Cells["Cliente"].Value.ToString();
            txtProtocolo.Text = ugListaCola.Selected.Rows[0].Cells["Protocolo"].Value.ToString();
            txtTipoESO.Text = ugListaCola.Selected.Rows[0].Cells["v_Value1"].Value.ToString();

            _serviceId = ugListaCola.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();

            var EstadoComponentes = ConsultorioBLDA.GetEstadoComponentes(strConn, _serviceId);
            ugExamenes.DataSource = EstadoComponentes;

        }

        private void ugListaLlamados_ClickCell(object sender, Infragistics.Win.UltraWinGrid.ClickCellEventArgs e)
        {
            if (ugListaLlamados.Selected.Rows.Count == 0)
                return;

            txtTrabajador.Text = ugListaLlamados.Selected.Rows[0].Cells["Paciente"].Value.ToString();
            txtEdad.Text = ugListaLlamados.Selected.Rows[0].Cells["Edad"].Value.ToString();
            txtDocumento.Text = ugListaLlamados.Selected.Rows[0].Cells["NroDocum"].Value.ToString();
            txtPuesto.Text = ugListaLlamados.Selected.Rows[0].Cells["v_CurrentOccupation"].Value.ToString();
            txtEmpTrabajo.Text = ugListaLlamados.Selected.Rows[0].Cells["Empleador"].Value.ToString();
            txtEmpCliente.Text = ugListaLlamados.Selected.Rows[0].Cells["Cliente"].Value.ToString();
            txtProtocolo.Text = ugListaLlamados.Selected.Rows[0].Cells["Protocolo"].Value.ToString();
            txtTipoESO.Text = ugListaLlamados.Selected.Rows[0].Cells["v_Value1"].Value.ToString();

            _serviceId = ugListaLlamados.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();

            var EstadoComponentes = ConsultorioBLDA.GetEstadoComponentes(strConn, _serviceId);
            ugExamenes.DataSource = EstadoComponentes;
        }

    }
}
