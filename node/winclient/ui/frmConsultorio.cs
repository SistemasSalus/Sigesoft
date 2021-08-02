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
using Sigesoft.Node.WinClient.BE.Custom;
using Infragistics.Win.UltraWinGrid;
using System.Runtime.InteropServices;

namespace Sigesoft.Node.WinClient.UI
{
    public partial class frmConsultorio : Form
    {
        private ConsultorioBLDA ConsultorioBLDA = new ConsultorioBLDA();

        PacientBL _pacientBL = new PacientBL();
        private string _personId;
        byte[] _personImage;

        List<string> _ServiceComponentId = new List<string>();

        ServiceBL _serviceBL = new ServiceBL();

        string strConn;
        string _serviceId;
        string _componentId;
        string _componentName;
        string _IsCall;
        int categoria;
        string nodo;
        
        private List<PacienteAgendado> lsPacienteAgendados = new List<PacienteAgendado>();

        public List<string> _componentIds { get; set; }

        public frmConsultorio(string pstrConn, string pstrComponentName)
        {
            InitializeComponent();

            _componentName = pstrComponentName;
            strConn = pstrConn;

            //timer1.Interval = 10000;
            //timer1.Start();

        }

        private void frmConsultorio_Load(object sender, EventArgs e)
        {
            lblNameComponent.Text = _componentName;

            if (lblNameComponent.Text == "LABORATORIO") //|| lblNameComponent.Text == "RAYOS X" || lblNameComponent.Text == "CARDIOLOGIA")
            {
                btnImportar.Enabled = true;
                btnImprimir.Enabled = true;
            }
            else
            {
                btnImportar.Enabled = false;
                btnImprimir.Enabled = false;
            }

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

            nodo = "N" + Globals.ClientSession.i_CurrentExecutionNodeId.ToString("000");

            LoadGridData();

            lsPacienteAgendados = new List<PacienteAgendado>();
        }

        private void LoadGridData()
        {
            var ListaAgendados = ConsultorioBLDA.GetPacienteAgendado(strConn, categoria, nodo);
            ugListaCola.DataSource = ListaAgendados;

        }

        private void btnLlamar_Click(object sender, EventArgs e)
        {
            if (ugListaCola.Selected.Rows.Count == 0)
            { 
                MessageBox.Show("Seleccione un Paciente de la Cola", "Alerta!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
                

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
                
                //Pasarle al ROBOT el mensaje de Llamado del paciente
                Llamar();
            }

            ugListaLlamados.DataSource = null;
            ugListaLlamados.DataSource = lsPacienteAgendados;

            //timer1.Start();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadGridData();
        }

        private void btnRellamar_Click(object sender, EventArgs e)
        {
            if (ugListaLlamados.Selected.Rows.Count == 0)
            {
                MessageBox.Show("Seleccione un Paciente de la Lista de Llamados", "Alerta!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Rellamar();
        }

        private void btnAtender_Click(object sender, EventArgs e)
        {
            if (ugListaLlamados.Selected.Rows.Count == 0)
            {
                MessageBox.Show("Seleccione un Paciente de la Lista de Llamados", "Alerta!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


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

            _serviceId = pacienteLlamado.v_ServiceId;

            Atender();

            //DAVID: Enlazar con el ESO
            Form frm = new Operations.frmEsoNew(_serviceId, string.Join("|", _componentIds.Select(p => p)), null);
            frm.Show();
            this.Enabled = true;


            //Form frm = new Operations.FrmEsoV2(_serviceId, lblNameComponent.Text, "Service", Globals.ClientSession.i_RoleId.Value, Globals.ClientSession.i_CurrentExecutionNodeId, Globals.ClientSession.i_SystemUserId);
            //frm.ShowDialog();

        }

        private void btnLiberar_Click(object sender, EventArgs e)
        {
            if (ugListaLlamados.Selected.Rows.Count == 0)
            {
                MessageBox.Show("Seleccione un Paciente de la Lista de Llamados", "Alerta!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            Liberar();
            LoadGridData();

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
            //timer1.Stop();

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
            _personId = ugListaCola.Selected.Rows[0].Cells["v_PersonId"].Value.ToString();

            var EstadoComponentes = ConsultorioBLDA.GetEstadoComponentes(strConn, _serviceId);
            ugExamenes.DataSource = EstadoComponentes;

            // Mostrar la foto del paciente
            var personImage = _pacientBL.GetPersonImage(_personId);

            if (personImage != null)
            {
                pbFoto.Image = Common.Utils.BytesArrayToImageOficce(personImage.b_PersonImage, pbFoto);
                _personImage = personImage.b_PersonImage;
            }

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
            _personId = ugListaLlamados.Selected.Rows[0].Cells["v_PersonId"].Value.ToString();

            var EstadoComponentes = ConsultorioBLDA.GetEstadoComponentes(strConn, _serviceId);
            ugExamenes.DataSource = EstadoComponentes;

            // Mostrar la foto del paciente
            var personImage = _pacientBL.GetPersonImage(_personId);

            if (personImage != null)
            {
                pbFoto.Image = Common.Utils.BytesArrayToImageOficce(personImage.b_PersonImage, pbFoto);
                _personImage = personImage.b_PersonImage;
            }
        }

        private void ugListaCola_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (e.Row.Cells["i_IsVipId"].Value.ToString().Trim() == Common.SiNo.SI.ToString())
            {
                //Escojo 2 colores
                e.Row.Appearance.BackColor = Color.White;
                e.Row.Appearance.BackColor2 = Color.Pink;
                //Y doy el efecto degradado vertical
                e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;

            }
        }

        #region Metodos Propios

        private void Atender()
        {
            OperationResult objOperationResult = new OperationResult();
            ServiceBL objServiceBL = new ServiceBL();
            servicecomponentDto objservicecomponentDto = new servicecomponentDto();

            _ServiceComponentId = new List<string>();

            foreach (var item in objServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, categoria, _serviceId))
            {
                _ServiceComponentId.Add(item.v_ServiceComponentId);
            }

            for (int i = 0; i < _ServiceComponentId.Count; i++)
            {
                objservicecomponentDto = new servicecomponentDto();
                objservicecomponentDto.v_ServiceComponentId = _ServiceComponentId[i];
                objservicecomponentDto.i_QueueStatusId = (int)Common.QueueStatusId.OCUPADO;
                objServiceBL.UpdateServiceComponentOfficeLlamando(objservicecomponentDto, Globals.ClientSession.GetAsList());
            }
        }

        private void Llamar()
        {
            OperationResult objOperationResult = new OperationResult();
            ServiceBL objServiceBL = new ServiceBL();
            servicecomponentDto objservicecomponentDto = new servicecomponentDto();

            _ServiceComponentId = new List<string>();

            foreach (var item in objServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, categoria, _serviceId))
            {
                _ServiceComponentId.Add(item.v_ServiceComponentId);
            }

            for (int i = 0; i < _ServiceComponentId.Count; i++)
            {
                objservicecomponentDto = new servicecomponentDto();
                objservicecomponentDto.v_ServiceComponentId = _ServiceComponentId[i];
                objservicecomponentDto.i_QueueStatusId = (int)Common.QueueStatusId.LLAMANDO;
                objServiceBL.UpdateServiceComponentOfficeLlamando(objservicecomponentDto, Globals.ClientSession.GetAsList());
            }

            foreach (var scid in _ServiceComponentId)
            {
                _serviceBL.UpdateServiceComponentVisor(ref objOperationResult, scid, (int)SiNo.SI);
            }
        }

        private void Rellamar()
        {
            OperationResult objOperationResult = new OperationResult();

            ServiceBL objServiceBL = new ServiceBL();
            _ServiceComponentId = new List<string>();
            _serviceId = ugListaLlamados.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();
            var x = objServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, categoria, _serviceId);
            foreach (var item in x)
            {
                _ServiceComponentId.Add(item.v_ServiceComponentId);
            }

            foreach (var scid in _ServiceComponentId)
            {
                _serviceBL.UpdateServiceComponentVisor(ref objOperationResult, scid, (int)SiNo.SI);
            }

        }

        private void Liberar()
        {
            OperationResult objOperationResult = new OperationResult();
            ServiceBL objServiceBL = new ServiceBL();
            servicecomponentDto objservicecomponentDto = new servicecomponentDto();

            _ServiceComponentId = new List<string>();

            foreach (var item in objServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, categoria, _serviceId))
            {
                _ServiceComponentId.Add(item.v_ServiceComponentId);
            }

            for (int i = 0; i < _ServiceComponentId.Count; i++)
            {
                objservicecomponentDto = new servicecomponentDto();
                objservicecomponentDto.v_ServiceComponentId = _ServiceComponentId[i];
                objservicecomponentDto.i_QueueStatusId = (int)Common.QueueStatusId.LIBRE;
                objServiceBL.UpdateServiceComponentOfficeLlamando(objservicecomponentDto, Globals.ClientSession.GetAsList());
            }
        }

        #endregion

        private void porAprobarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            ServiceBL objServiceBL = new ServiceBL();
            servicecomponentDto objservicecomponentDto = new servicecomponentDto();

            _serviceId = ugListaCola.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();

            _ServiceComponentId = new List<string>();

            foreach (var item in objServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, categoria, _serviceId))
            {
                _ServiceComponentId.Add(item.v_ServiceComponentId);
            }
            for (int i = 0; i < _ServiceComponentId.Count(); i++)
            {
                objservicecomponentDto = objServiceBL.GetServiceComponent(ref objOperationResult, _ServiceComponentId[i]);
                objservicecomponentDto.i_ServiceComponentStatusId = (int)Common.ServiceComponentStatus.PorAprobacion;
                objServiceBL.UpdateServiceComponent(ref objOperationResult, objservicecomponentDto, Globals.ClientSession.GetAsList());
            }
        }

        private void porReevaluarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            ServiceBL objServiceBL = new ServiceBL();
            servicecomponentDto objservicecomponentDto = new servicecomponentDto();

            _serviceId = ugListaCola.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();

            _ServiceComponentId = new List<string>();

            foreach (var item in objServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, categoria, _serviceId))
            {
                _ServiceComponentId.Add(item.v_ServiceComponentId);
            }
            for (int i = 0; i < _ServiceComponentId.Count(); i++)
            {
                objservicecomponentDto = objServiceBL.GetServiceComponent(ref objOperationResult, _ServiceComponentId[i]);
                objservicecomponentDto.i_ServiceComponentStatusId = (int)Common.ServiceComponentStatus.PorReevaluar;
                objServiceBL.UpdateServiceComponent(ref objOperationResult, objservicecomponentDto, Globals.ClientSession.GetAsList());
            }
        }

        private void culminadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            ServiceBL objServiceBL = new ServiceBL();
            servicecomponentDto objservicecomponentDto = new servicecomponentDto();

            _serviceId = ugListaCola.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();

            _ServiceComponentId = new List<string>();

            foreach (var item in objServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, categoria, _serviceId))
            {
                _ServiceComponentId.Add(item.v_ServiceComponentId);
            }
            for (int i = 0; i < _ServiceComponentId.Count(); i++)
            {
                objservicecomponentDto = objServiceBL.GetServiceComponent(ref objOperationResult, _ServiceComponentId[i]);
                objservicecomponentDto.i_ServiceComponentStatusId = (int)Common.ServiceComponentStatus.Culminado;
                objServiceBL.UpdateServiceComponent(ref objOperationResult, objservicecomponentDto, Globals.ClientSession.GetAsList());
            }
        }

        public class TSCLIB_DLL
        {
            [DllImport("TSCLIB.dll", EntryPoint = "about")]
            public static extern int about();

            [DllImport("TSCLIB.dll", EntryPoint = "openport")]
            public static extern int openport(string printername);

            [DllImport("TSCLIB.dll", EntryPoint = "barcode")]
            public static extern int barcode(string x, string y, string type,
                        string height, string readable, string rotation,
                        string narrow, string wide, string code);

            [DllImport("TSCLIB.dll", EntryPoint = "clearbuffer")]
            public static extern int clearbuffer();

            [DllImport("TSCLIB.dll", EntryPoint = "closeport")]
            public static extern int closeport();

            [DllImport("TSCLIB.dll", EntryPoint = "downloadpcx")]
            public static extern int downloadpcx(string filename, string image_name);

            [DllImport("TSCLIB.dll", EntryPoint = "formfeed")]
            public static extern int formfeed();

            [DllImport("TSCLIB.dll", EntryPoint = "nobackfeed")]
            public static extern int nobackfeed();

            [DllImport("TSCLIB.dll", EntryPoint = "printerfont")]
            public static extern int printerfont(string x, string y, string fonttype,
                            string rotation, string xmul, string ymul,
                            string text);

            [DllImport("TSCLIB.dll", EntryPoint = "printlabel")]
            public static extern int printlabel(string set, string copy);

            [DllImport("TSCLIB.dll", EntryPoint = "sendcommand")]
            public static extern int sendcommand(string printercommand);

            [DllImport("TSCLIB.dll", EntryPoint = "setup")]
            public static extern int setup(string width, string height,
                      string speed, string density,
                      string sensor, string vertical,
                      string offset);

            [DllImport("TSCLIB.dll", EntryPoint = "windowsfont")]
            public static extern int windowsfont(int x, int y, int fontheight,
                            int rotation, int fontstyle, int fontunderline,
                            string szFaceName, string content);
        }

        private void ugListaCola_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            frmNroTickes frm = new frmNroTickes();
            frm.ShowDialog();
            var y = frm.NroTickets;
            var o = ugListaCola.Selected.Rows[0].Cells["i_NroTickets"];
            o.Value = y;
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            ImprimirTicket oImprimirTicket = null;
            List<ImprimirTicket> ListImprimirTicket = new List<ImprimirTicket>();
            int j = 0;

            foreach (var item in ugListaCola.Rows)
            {
                oImprimirTicket = new ImprimirTicket();
                string x = item.Cells["i_NroTickets"].Value == null ? "0" : item.Cells["i_NroTickets"].Value.ToString();

                if (x != "0" || x != "")
                {
                    for (int i = 0; i < int.Parse(x); i++)
                    {
                        oImprimirTicket.v_ServicioId = item.Cells["v_ServiceId"].Value.ToString().Remove(0, 7);

                        if (item.Cells["Paciente"].Value.ToString().Length > 27)
                        { oImprimirTicket.NombreTrabajador = item.Cells["Paciente"].Value.ToString().Substring(0, 27); }
                        else
                        { oImprimirTicket.NombreTrabajador = item.Cells["Paciente"].Value.ToString(); }

                        var f = DateTime.Parse(item.Cells["d_CircuitStartDate"].Value.ToString());
                        oImprimirTicket.Fecha = f.Date;

                        ListImprimirTicket.Add(oImprimirTicket);
                    }
                }
                else
                {
                    MessageBox.Show("No hay Tickets para imprimir", "ADVERTENCIA!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            foreach (ImprimirTicket xTicket in ListImprimirTicket)
            {
                ////IMPRESORA CAMPAÑA
                //if (j == 0)
                //{
                //    TSCLIB_DLL.openport("TSC TTP-244 Pro");                                             //Open specified printer driver
                //    TSCLIB_DLL.setup("108", "24", "4", "8", "0", "0", "0");                           //Setup the media size and sensor type info
                //    TSCLIB_DLL.clearbuffer();                                                           //Clear image buffer
                //    TSCLIB_DLL.sendcommand("DIRECTION 1,0");
                //    TSCLIB_DLL.sendcommand("GAP 3 mm,0 mm");

                //    TSCLIB_DLL.printerfont("30", "10", "2", "0", "1", "1", xTicket.NombreTrabajador);        //Drawing printer font
                //    TSCLIB_DLL.printerfont("410", "30", "2", "90", "1", "1", xTicket.Fecha.ToString());      //Drawing printer font
                //    TSCLIB_DLL.barcode("140", "40", "128", "130", "2", "0", "1", "2", xTicket.v_ServicioId); //Drawing barcode

                //    j = j + 1;
                //}
                //else
                //{
                //    TSCLIB_DLL.printerfont("450", "10", "2", "0", "1", "1", xTicket.NombreTrabajador);       //Drawing printer font
                //    TSCLIB_DLL.printerfont("820", "30", "2", "90", "1", "1", xTicket.Fecha.ToString());      //Drawing printer font
                //    TSCLIB_DLL.barcode("560", "40", "128", "130", "2", "0", "1", "2", xTicket.v_ServicioId); //Drawing barcode

                //    TSCLIB_DLL.printlabel("1", "1");
                //    TSCLIB_DLL.closeport();

                //    j = j - 1;
                //}

                //IMPRESORA SURCO
                if (j == 0)
                {
                    TSCLIB_DLL.openport("TSC TTP-244 Pro");                                             //Open specified printer driver
                    TSCLIB_DLL.setup("108", "25.4", "4", "8", "0", "0", "0");                           //Setup the media size and sensor type info
                    TSCLIB_DLL.clearbuffer();                                                           //Clear image buffer
                    TSCLIB_DLL.sendcommand("DIRECTION 1,0");
                    TSCLIB_DLL.sendcommand("GAP 3 mm,0 mm");

                    TSCLIB_DLL.printerfont("30", "10", "2", "0", "1", "1", xTicket.NombreTrabajador);        //Drawing printer font
                    TSCLIB_DLL.printerfont("410", "30", "2", "90", "1", "1", xTicket.Fecha.ToString());      //Drawing printer font
                    TSCLIB_DLL.barcode("150", "40", "128", "130", "2", "0", "1", "2", xTicket.v_ServicioId); //Drawing barcode

                    j = j + 1;
                }
                else
                {
                    TSCLIB_DLL.printerfont("470", "10", "2", "0", "1", "1", xTicket.NombreTrabajador);       //Drawing printer font
                    TSCLIB_DLL.printerfont("840", "30", "2", "90", "1", "1", xTicket.Fecha.ToString());      //Drawing printer font
                    TSCLIB_DLL.barcode("580", "40", "128", "130", "2", "0", "1", "2", xTicket.v_ServicioId); //Drawing barcode

                    TSCLIB_DLL.printlabel("1", "1");
                    TSCLIB_DLL.closeport();

                    j = j - 1;
                }
            }

            if (ListImprimirTicket.Count() % 2 == 0)
            {
                MessageBox.Show("Se terminó de imprimir los Tickets", "AVISO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                TSCLIB_DLL.printlabel("1", "1");
                TSCLIB_DLL.closeport();

                MessageBox.Show("Se terminó de imprimir los Tickets", "AVISO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //timer1.Start();
        }

        private void btnImportar_Click(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            List<ServiceComponentFieldsList> _serviceComponentFieldsList = null;
            ServiceComponentFieldsList serviceComponentFields = null;
            ServiceComponentFieldValuesList serviceComponentFieldValues = null;
            List<ServiceComponentFieldValuesList> _serviceComponentFieldValuesList = null;

            //Obtener Servicios
            var ListaServicios = _serviceBL.ObtenerServicios();
            int contador = 0;

            foreach (var item in ListaServicios)
            {
                string ServiceId = item.v_ServiceId;
                string PersonId = item.v_PersonId;

                //Obtener Datos de la tabla interface
                List<InterfaceBS> _datainterfaceBS = null;
                _datainterfaceBS = _serviceBL.ObtenerListaDatos(ServiceId);


                if (_datainterfaceBS.Count() > 0)
                {
                    _serviceComponentFieldsList = new List<ServiceComponentFieldsList>();
                    _serviceComponentFieldValuesList = new List<ServiceComponentFieldValuesList>();

                    //Obtener Servicecomponent

                    List<ServiceComponentShort> o = new List<ServiceComponentShort>();

                    foreach (var item1 in _datainterfaceBS)
                    {
                        serviceComponentFields = new ServiceComponentFieldsList();
                        serviceComponentFieldValues = new ServiceComponentFieldValuesList();

                        serviceComponentFields.v_ComponentFieldsId = item1.v_ComponentId;
                        o = _serviceBL.ObtenerServiceComponent(ServiceId, item1.i_CategoryId);
                        serviceComponentFields.v_ServiceComponentId = o[0].v_ServiceComponentId;
                        serviceComponentFields.v_ComponentId = o[0].v_ComponentId;
                        _serviceComponentFieldValuesList = new List<Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList>();
                        serviceComponentFieldValues = new Sigesoft.Node.WinClient.BE.ServiceComponentFieldValuesList();
                        serviceComponentFieldValues.v_ComponentFieldValuesId = null;
                        serviceComponentFieldValues.v_Value1 = item1.v_ResultComponent;
                        _serviceComponentFieldValuesList.Add(serviceComponentFieldValues);
                        serviceComponentFields.ServiceComponentFieldValues = _serviceComponentFieldValuesList;
                        // Agregar a mi lista
                        _serviceComponentFieldsList.Add(serviceComponentFields);

                        contador = contador + 1;
                    }
                    _serviceBL.AddServiceComponentValues(ref objOperationResult, _serviceComponentFieldsList, Globals.ClientSession.GetAsList(), PersonId, o[0].v_ServiceComponentId);

                }
            }
            MessageBox.Show("Se ingresaron " + contador.ToString() + " Registros", "INFORMACIÓN!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ImprimirTicket oImprimirTicket = null;
            List<ImprimirTicket> ListImprimirTicket = new List<ImprimirTicket>();
            int j = 0;

            foreach (var item in ugListaCola.Rows)
            {
                oImprimirTicket = new ImprimirTicket();
                //string x = item.Cells["i_NroTickets"].Value == null ? "0" : item.Cells["i_NroTickets"].Value.ToString();
                string x = "2";

                if (item.Cells["Cliente"].Value.ToString() == "CERAMICA LIMA S.A.")
                {
                    for (int i = 0; i < int.Parse(x); i++)
                    {
                        oImprimirTicket.v_ServicioId = item.Cells["v_ServiceId"].Value.ToString().Remove(0, 7);

                        if (item.Cells["Paciente"].Value.ToString().Length > 27)
                        { oImprimirTicket.NombreTrabajador = item.Cells["Paciente"].Value.ToString().Substring(0, 27); }
                        else
                        { oImprimirTicket.NombreTrabajador = item.Cells["Paciente"].Value.ToString(); }

                        var f = DateTime.Parse(item.Cells["d_CircuitStartDate"].Value.ToString());
                        oImprimirTicket.Fecha = f.Date;

                        ListImprimirTicket.Add(oImprimirTicket);
                    }
                }
            }

            foreach (ImprimirTicket xTicket in ListImprimirTicket)
            {
                ////IMPRESORA CAMPAÑA
                //if (j == 0)
                //{
                //    TSCLIB_DLL.openport("TSC TTP-244 Pro");                                             //Open specified printer driver
                //    TSCLIB_DLL.setup("108", "24", "4", "8", "0", "0", "0");                           //Setup the media size and sensor type info
                //    TSCLIB_DLL.clearbuffer();                                                           //Clear image buffer
                //    TSCLIB_DLL.sendcommand("DIRECTION 1,0");
                //    TSCLIB_DLL.sendcommand("GAP 3 mm,0 mm");

                //    TSCLIB_DLL.printerfont("30", "10", "2", "0", "1", "1", xTicket.NombreTrabajador);        //Drawing printer font
                //    TSCLIB_DLL.printerfont("410", "30", "2", "90", "1", "1", xTicket.Fecha.ToString());      //Drawing printer font
                //    TSCLIB_DLL.barcode("140", "40", "128", "130", "2", "0", "1", "2", xTicket.v_ServicioId); //Drawing barcode

                //    j = j + 1;
                //}
                //else
                //{
                //    TSCLIB_DLL.printerfont("450", "10", "2", "0", "1", "1", xTicket.NombreTrabajador);       //Drawing printer font
                //    TSCLIB_DLL.printerfont("820", "30", "2", "90", "1", "1", xTicket.Fecha.ToString());      //Drawing printer font
                //    TSCLIB_DLL.barcode("560", "40", "128", "130", "2", "0", "1", "2", xTicket.v_ServicioId); //Drawing barcode

                //    TSCLIB_DLL.printlabel("1", "1");
                //    TSCLIB_DLL.closeport();

                //    j = j - 1;
                //}

                //IMPRESORA SURCO
                if (j == 0)
                {
                    TSCLIB_DLL.openport("TSC TTP-244 Pro");                                             //Open specified printer driver
                    TSCLIB_DLL.setup("108", "25.4", "4", "8", "0", "0", "0");                           //Setup the media size and sensor type info
                    TSCLIB_DLL.clearbuffer();                                                           //Clear image buffer
                    TSCLIB_DLL.sendcommand("DIRECTION 1,0");
                    TSCLIB_DLL.sendcommand("GAP 3 mm,0 mm");

                    TSCLIB_DLL.printerfont("30", "10", "2", "0", "1", "1", xTicket.NombreTrabajador);        //Drawing printer font
                    TSCLIB_DLL.printerfont("410", "30", "2", "90", "1", "1", xTicket.Fecha.ToString());      //Drawing printer font
                    TSCLIB_DLL.barcode("150", "40", "128", "130", "2", "0", "1", "2", xTicket.v_ServicioId); //Drawing barcode

                    j = j + 1;
                }
                else
                {
                    TSCLIB_DLL.printerfont("470", "10", "2", "0", "1", "1", xTicket.NombreTrabajador);       //Drawing printer font
                    TSCLIB_DLL.printerfont("840", "30", "2", "90", "1", "1", xTicket.Fecha.ToString());      //Drawing printer font
                    TSCLIB_DLL.barcode("580", "40", "128", "130", "2", "0", "1", "2", xTicket.v_ServicioId); //Drawing barcode

                    TSCLIB_DLL.printlabel("1", "1");
                    TSCLIB_DLL.closeport();

                    j = j - 1;
                }
            }

            if (ListImprimirTicket.Count() % 2 == 0)
            {
                MessageBox.Show("Se terminó de imprimir los Tickets", "AVISO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                TSCLIB_DLL.printlabel("1", "1");
                TSCLIB_DLL.closeport();

                MessageBox.Show("Se terminó de imprimir los Tickets", "AVISO!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
