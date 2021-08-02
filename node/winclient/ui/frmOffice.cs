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
using Infragistics.Win.UltraWinGrid;
using System.Runtime.InteropServices;


namespace Sigesoft.Node.WinClient.UI
{
    public partial class frmOffice : Form
    {
        #region Declarations
            
        string _serviceId;
        string _componentId;
        string _componentName;
        string _IsCall;
        int _Flag = 0;
        int _TserviceId;
        string _CalendarId;
        List<string> _ServiceComponentId = new List<string>();
        byte[] _personImage;
        string _personName;
        int _categoryId;
        private Sigesoft.Node.WinClient.UI.Utils.CustomizedToolTip _customizedToolTip = null;
        private string _serviceStatusId;
        ServiceBL _serviceBL = new ServiceBL();
        PacientBL _pacientBL = new PacientBL();
        private string _personId;
       
        #endregion
        List<CalendarList> objCalendarList = new List<CalendarList>();
        
        public List<string> _componentIds { get; set; }

        public frmOffice(string pstrComponentName)
        {
            InitializeComponent();
            //timer1.Interval = 1000;
            //timer1.Start();
            _componentId = "";
            _componentName = pstrComponentName;

        }

        private void frmOffice_Load(object sender, EventArgs e)
        {
            //using (new LoadingClass.PleaseWait(this.Location, "Cargando..."))
            //{
                
            //}
            _customizedToolTip = new Sigesoft.Node.WinClient.UI.Utils.CustomizedToolTip(grdDataServiceComponent);
            btnRefresh_Click(sender, e);

            if (lblNameComponent.Text == "LABORATORIO" || lblNameComponent.Text == "RAYOS X" || lblNameComponent.Text == "CARDIOLOGIA")
            {
                btnImportar.Enabled = true;
                btnImprimir.Enabled = true;
            }
            else 
            {
                btnImportar.Enabled = false;
                btnImprimir.Enabled = false;            
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //label10.Visible = !label10.Visible;
            //btnRefresh_Click(sender, e);
        }

        private void grdListaLlamando_ClickCell(object sender, ClickCellEventArgs e)
        {
            //timer1.Stop();
        }

        private void Llamar()
        {
            OperationResult objOperationResult = new OperationResult();
            List<ServiceComponentList> ListServiceComponent = new List<ServiceComponentList>();
            CalendarBL objCalendarBL = new CalendarBL();
            CalendarList objCalendar = new CalendarList();
           
            ServiceBL objServiceBL = new ServiceBL();
            servicecomponentDto objservicecomponentDto = new servicecomponentDto();

            _ServiceComponentId = new List<string>();

            if (int.Parse(_serviceStatusId) == (int)ServiceStatus.EsperandoAptitud)
            {
                MessageBox.Show("Este paciente ya tiene el servicio en espera de Aptitud, no puede ser llamado.", "INFORMACIÓN!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_IsCall == "OcupadoLlamado")
            {
                DialogResult Result = MessageBox.Show("¿Está seguro de LLAMAR a este paciente que está ocupado?", "ADVERTENCIA!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (Result == DialogResult.No)
                    return;

                //DialogResult Result = MessageBox.Show("Este paciente está ocupado", "ADVERTENCIA!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //return;
            }
          
            objCalendar.v_Pacient = grdListaLlamando.Selected.Rows[0].Cells["v_Pacient"].Value.ToString();
            objCalendar.v_OrganizationName = grdListaLlamando.Selected.Rows[0].Cells["v_WorkingOrganizationName"].Value.ToString();
            objCalendar.v_ServiceComponentId = grdListaLlamando.Selected.Rows[0].Cells["v_ServiceComponentId"].Value.ToString();
            objCalendar.v_ServiceId = grdListaLlamando.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();  
            

            //busqueda de la grilla con el nombre del trab

            if (objCalendarList.Count > 0)
            {
                foreach (CalendarList item in objCalendarList)
                {
                    if (item.v_Pacient == objCalendar.v_Pacient)
                    {
                        MessageBox.Show("Ya está llamando a este paciente.", "Notificación!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }

            if (_categoryId == -1)
            {
                _ServiceComponentId.Add(grdListaLlamando.Selected.Rows[0].Cells["v_ServiceComponentId"].Value.ToString());
            }
            else
            {
                foreach (var item in objServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, _categoryId, _serviceId))
                {
                    _ServiceComponentId.Add(item.v_ServiceComponentId);
                }
            }

            // Cargar grilla de llamando al paciente  ************
            objCalendarList.Add(objCalendar);
            grdLlamandoPaciente.DataSource = new List<CalendarList>();
            grdLlamandoPaciente.DataSource = objCalendarList;

            if (grdLlamandoPaciente.Rows.Count > 0)         
                grdLlamandoPaciente.Rows[0].Selected = true;

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

            //Actualizar grdDataServiceComponent      
            ListServiceComponent = objServiceBL.GetServiceComponents(ref objOperationResult, _serviceId);
            grdDataServiceComponent.DataSource = ListServiceComponent;

            //grdListaLlamando.Enabled = false;
            grdLlamandoPaciente.Enabled = true;
            btnRefresh.Enabled = true; //se cambio para no bloquear el boton DAVID

            chkHability.Enabled = true;

            //btnLlamar.Enabled = false;
        }

        private void Atender()
        {
            OperationResult objOperationResult = new OperationResult();
            ServiceBL objServiceBL = new ServiceBL();
            servicecomponentDto objservicecomponentDto = new servicecomponentDto();
            List<ServiceComponentList> ListServiceComponent = new List<ServiceComponentList>();
            _ServiceComponentId = new List<string>();

            if (chkVerificarHuellaDigital.Checked)
            {
                var checkingFinger = new frmCheckingFinger();
                checkingFinger._PacientId = _personId;
                checkingFinger.ShowDialog();

                if (checkingFinger.DialogResult == DialogResult.Cancel)
                    return;
            }
          
            DialogResult Result = MessageBox.Show("¿Está seguro de INICIAR ATENCIÓN este registro?", "ADVERTENCIA!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            
            _serviceId = grdLlamandoPaciente.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString(); 
            
            if (_categoryId == -1)
            {
                _ServiceComponentId.Add(grdListaLlamando.Selected.Rows[0].Cells["v_ServiceComponentId"].Value.ToString());
            }
            else
            {
                foreach (var item in objServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, _categoryId, _serviceId))
                {
                    _ServiceComponentId.Add(item.v_ServiceComponentId);
                }
            }
      
            if (Result == System.Windows.Forms.DialogResult.Yes)
            {
                for (int i = 0; i < _ServiceComponentId.Count(); i++)
                {
                    objservicecomponentDto = objServiceBL.GetServiceComponent(ref objOperationResult, _ServiceComponentId[i]);
                    objservicecomponentDto.i_QueueStatusId = (int)Common.QueueStatusId.OCUPADO;
                    objServiceBL.UpdateServiceComponent(ref objOperationResult, objservicecomponentDto, Globals.ClientSession.GetAsList());
                }
                //Actualizar grdDataServiceComponent
                string strServicelId = _serviceId; //grdListaLlamando.Selected.Rows[0].Cells[5].Value.ToString();
                ListServiceComponent = objServiceBL.GetServiceComponents(ref objOperationResult, strServicelId);
                grdDataServiceComponent.DataSource = ListServiceComponent;

                _Flag = 1;

                Form frm;
                if (_TserviceId == (int)MasterService.AtxMedicaParticular)
                {
                    frm = new Operations.frmMedicalConsult(_serviceId, string.Join("|", _componentIds.Select(p => p)), null);
                    frm.Show();
                }
                else
                {
                    //this.Enabled = false;
                    //frm = new Operations.frmEso(_serviceId, string.Join("|", _componentIds.Select(p => p)), null);
                    ////frm.MdiParent = this.MdiParent;
                    //frm.Show();
                    //controlar David
                    //this.WindowState = FormWindowState.Minimized;

                    frm = new Operations.frmEsoNew(_serviceId, string.Join("|", _componentIds.Select(p => p)), null);
                    frm.Show();
                    this.Enabled = true;

                    //frm = new Operations.FrmEsoV2(_serviceId, lblNameComponent.Text, "Service", Globals.ClientSession.i_RoleId.Value, Globals.ClientSession.i_CurrentExecutionNodeId, Globals.ClientSession.i_SystemUserId);
                    //frm.ShowDialog();
                    //this.Enabled = true;

                    // Aviso automático de que se culminaron todos los examanes, se tendria que proceder
                    // a establecer el estado del servicio a (Culminado Esperando Aptitud)               

                    var alert = objServiceBL.GetServiceComponentsCulminados(ref objOperationResult, _serviceId);

                    if (alert != null && alert.Count > 0)
                    {

                    }
                    else
                    {
                        MessageBox.Show("Todos los Exámenes se encuentran concluidos.\nEl estado de la Atención es: En espera de Aptitud .", "INFORMACIÓN!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        serviceDto objserviceDto = new serviceDto();
                        objserviceDto = objServiceBL.GetService(ref objOperationResult, objservicecomponentDto.v_ServiceId);
                        objserviceDto.i_ServiceStatusId = (int)ServiceStatus.EsperandoAptitud;
                        objserviceDto.v_Motive = "Esperando Aptitud";

                        objServiceBL.UpdateService(ref objOperationResult, objserviceDto, Globals.ClientSession.GetAsList());
                    }
                }

                // refrecar la grilla
                ListServiceComponent = objServiceBL.GetServiceComponents(ref objOperationResult, _serviceId);
                grdDataServiceComponent.DataSource = ListServiceComponent;
            }
        }

        private void Liberar()
        {
            try
            {
                OperationResult objOperationResult = new OperationResult();
                ServiceBL objServiceBL = new ServiceBL();
                _ServiceComponentId = new List<string>();
                string pServiceId = grdLlamandoPaciente.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();
                servicecomponentDto objservicecomponentDto = null;
                List<ServiceComponentList> ListServiceComponent = new List<ServiceComponentList>();

                if (_categoryId == -1)
                {
                    _ServiceComponentId.Add(grdLlamandoPaciente.Selected.Rows[0].Cells["v_ServiceComponentId"].Value.ToString());
                }
                else
                {
                    
                    var servCompCat = objServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, _categoryId, pServiceId);

                    foreach (var item in servCompCat)
                    {
                        _ServiceComponentId.Add(item.v_ServiceComponentId);
                    }
                }

                List<servicecomponentDto> list = new List<servicecomponentDto>();

                for (int i = 0; i < _ServiceComponentId.Count; i++)
                {
                    objservicecomponentDto = new servicecomponentDto();
                    objservicecomponentDto.v_ServiceComponentId = _ServiceComponentId[i];
                    objservicecomponentDto.i_QueueStatusId = (int)Common.QueueStatusId.LIBRE;
                    objservicecomponentDto.i_Iscalling = (int)SiNo.NO;
                    list.Add(objservicecomponentDto);
                }

                // update
                objServiceBL.UpdateServiceComponentOffice(list);

                #region Check de salir de circuito
                              
                if (chkHability.Checked == true) // finaliza el servicio y actualiza el estado del servicio
                {
                    if (ddlServiceStatusId.SelectedValue.ToString() == ((int)ServiceStatus.Iniciado).ToString())
                    {
                        MessageBox.Show("Debe elegir cualquier otro estado que no sea (Iniciado)\nSi desea Liberar y/o Finalizar Circuito.", "ADVERTENCIA!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    serviceDto objserviceDto = new serviceDto();
                  
                    objserviceDto.v_ServiceId = _serviceId;
                    objserviceDto.i_ServiceStatusId = int.Parse(ddlServiceStatusId.SelectedValue.ToString());
                    objserviceDto.v_Motive = txtReason.Text;

                    objServiceBL.UpdateServiceOffice(ref objOperationResult, objserviceDto, Globals.ClientSession.GetAsList());

                    //Actualizamos el estado de la linea de la agenda como fuera de circuito
                    CalendarBL objCalendarBL = new CalendarBL();
                    calendarDto objcalendarDto = new calendarDto();
                    objcalendarDto = objCalendarBL.GetCalendar(ref objOperationResult, _CalendarId);
                    objcalendarDto.i_LineStatusId = 2;// int.Parse(Common.LineStatus.FueraCircuito.ToString());
                    objCalendarBL.UpdateCalendar(ref objOperationResult, objcalendarDto, Globals.ClientSession.GetAsList());

                }

                #endregion

                //Actualizar grdDataServiceComponent
              
                ListServiceComponent = objServiceBL.GetServiceComponents(ref objOperationResult, _serviceId);
                grdDataServiceComponent.DataSource = ListServiceComponent;
             
                btnRefresh_Click(null, null);

                txtReason.Text = "";
                //grdListaLlamando.Enabled = true;
                //grdLlamandoPaciente.Enabled = false;
                btnRefresh.Enabled = true;
                //chkHability.Enabled = false;
                //chkHability.Checked = false;
                //groupBox3.Enabled = false;

                var x = objCalendarList.Find(p => p.v_ServiceId == pServiceId);
                var res = objCalendarList.Remove(x);
                grdLlamandoPaciente.DataSource = new List<CalendarList>();
                grdLlamandoPaciente.DataSource = objCalendarList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Common.Utils.ExceptionFormatter(ex), "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          
        }

        private void grdListaLlamando_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();

            var serviceId = e.Row.Cells["v_ServiceId"].Value.ToString();

            var serviceComponents = _serviceBL.GetServiceComponents(ref objOperationResult, serviceId);

            var atendido = serviceComponents.Find(p => _componentIds.Contains(p.v_ComponentId) &&
                                                (p.i_ServiceComponentStatusId == (int)Common.ServiceComponentStatus.Culminado
                                                || p.i_ServiceComponentStatusId == (int)Common.ServiceComponentStatus.PorAprobacion));
            if (atendido != null)
            {
                e.Row.Cells["b_IsAttended"].Value = Resources.bullet_tick;
                e.Row.Cells["b_IsAttended"].ToolTipText = atendido.v_ServiceComponentStatusName;
            }
            else
            {
                var noAtendido = serviceComponents.Find(p => _componentIds.Contains(p.v_ComponentId));
                                           
                e.Row.Cells["b_IsAttended"].Value = Resources.bullet_red;
                e.Row.Cells["b_IsAttended"].ToolTipText = noAtendido.v_ServiceComponentStatusName;
            }

            //Si el contenido de la columna Vip es igual a SI
            if (e.Row.Cells["v_IsVipName"].Value.ToString().Trim() == Common.SiNo.SI.ToString())
            {
                //Escojo 2 colores
                e.Row.Appearance.BackColor = Color.White;
                e.Row.Appearance.BackColor2 = Color.Pink;
                //Y doy el efecto degradado vertical
                e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
              
            }
        }

        private void grdDataServiceComponent_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            //Si el contenido de la columna Vip es igual a SI
            if (e.Row.Cells["v_QueueStatusName"].Value.ToString().Trim() == Common.QueueStatusId.OCUPADO.ToString())
            {
                //Escojo 2 colores
                e.Row.Appearance.BackColor = Color.White;
                e.Row.Appearance.BackColor2 = Color.Pink;
                //Y doy el efecto degradado vertical
                e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
                //timer1.Start();
            }
            if (e.Row.Cells["v_QueueStatusName"].Value.ToString().Trim() == Common.QueueStatusId.LLAMANDO.ToString())
            {
                //Escojo 2 colores
                e.Row.Appearance.BackColor = Color.Orange;
                //e.Row.Appearance.BackColor2 = Color.Pink;
                //Y doy el efecto degradado vertical
                //e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
                //timer1.Start();
            }

            //Si el contenido de la columna Vip es igual a SI
            if (e.Row.Cells["i_ServiceComponentStatusId"].Value.ToString() == ((int)Common.ServiceComponentStatus.Culminado).ToString()
                || e.Row.Cells["i_ServiceComponentStatusId"].Value.ToString() == ((int)Common.ServiceComponentStatus.PorAprobacion).ToString())
            {
                //Escojo 2 colores
                e.Row.Appearance.BackColor = Color.White;
                e.Row.Appearance.BackColor2 = Color.Cyan;
                //Y doy el efecto degradado vertical
                e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;

            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

            try
            {
                OperationResult objOperationResult = new OperationResult();
                CalendarBL objCalendarBL = new CalendarBL();
                List<CalendarList> objCalendarList = new List<CalendarList>();
                ServiceComponentList objServiceComponent = new ServiceComponentList();
                List<ServiceComponentList> ListServiceComponent = new List<ServiceComponentList>();

                objCalendarList = objCalendarBL.GetPacientInLineByComponentId1(ref objOperationResult, 0, null, "d_ServiceDate ASC", _componentId, DateTime.Now.Date, DateTime.Now.Date.AddDays(1), _componentIds.ToArray());
                grdListaLlamando.DataSource = objCalendarList;
            
                lblNameComponent.Text = _componentName;

                //var dataList = BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 125, null).FindAll(p => p.Id != "1" && p.Id != "3");

                //Utils.LoadDropDownList(ddlServiceStatusId, "Value1", "Id", dataList, DropDownListAction.Select);

                grdDataServiceComponent.DataSource = ListServiceComponent;       
            }
            catch (Exception ex)
            {                
                MessageBox.Show(Common.Utils.ExceptionFormatter(ex), "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          
        }

        private void chkHability_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHability.Checked == true)
            {
                groupBox3.Enabled = true;
            }
            else
            {
                groupBox3.Enabled = false;
            }
        }

        private void grdListaLlamando_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
        {
            btnLlamar.Enabled = (grdListaLlamando.Selected.Rows.Count > 0);

            if (grdListaLlamando.Selected.Rows.Count == 0)
                return;

            txtPacient.Text = grdListaLlamando.Selected.Rows[0].Cells["v_Pacient"].Value.ToString();
            _personName = txtPacient.Text;
            //DZERVER *****
            WorkingOrganization.Text = grdListaLlamando.Selected.Rows[0].Cells["v_WorkingOrganizationName"].Value.ToString();
            txtPuestoTrab.Text = grdListaLlamando.Selected.Rows[0].Cells["v_CurrentOccupation"].Value.ToString();
            OperationResult objOperationResult = new OperationResult();
          
            List<ServiceComponentList> ListServiceComponent = new List<ServiceComponentList>();

            _serviceId = grdListaLlamando.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();
            _TserviceId = int.Parse(grdListaLlamando.Selected.Rows[0].Cells["i_ServiceId"].Value.ToString());
            _CalendarId = grdListaLlamando.Selected.Rows[0].Cells["v_CalendarId"].Value.ToString();
            _categoryId = (int)grdListaLlamando.Selected.Rows[0].Cells["i_CategoryId"].Value;
            _serviceStatusId = grdListaLlamando.Selected.Rows[0].Cells["i_ServiceStatusId"].Value.ToString();
            _personId = grdListaLlamando.Selected.Rows[0].Cells["v_PersonId"].Value.ToString();

            ListServiceComponent = _serviceBL.GetServiceComponents(ref objOperationResult, _serviceId);
            grdDataServiceComponent.DataSource = ListServiceComponent;

            DateTime FechaNacimiento = (DateTime)grdListaLlamando.Selected.Rows[0].Cells["d_Birthdate"].Value;
            int PacientAge = DateTime.Today.AddTicks(-FechaNacimiento.Ticks).Year - 1;
            txtAge.Text = PacientAge.ToString();

            txtDni.Text = grdListaLlamando.Selected.Rows[0].Cells["v_DocNumber"].Value.ToString();
            txtProtocol.Text = grdListaLlamando.Selected.Rows[0].Cells["v_ProtocolName"].Value.ToString();
            if (grdListaLlamando.Selected.Rows[0].Cells["v_ProtocolId"].Value.ToString() == Constants.CONSULTAMEDICA)
            {
                txtTypeESO.Text = "";
            }
            else
            {
                txtTypeESO.Text = grdListaLlamando.Selected.Rows[0].Cells["v_EsoTypeName"].Value.ToString();
            }

            // Mostrar la foto del paciente
            var personImage = _pacientBL.GetPersonImage(_personId);

            if (personImage != null)
            {
                pbImage.Image = Common.Utils.BytesArrayToImageOficce(personImage.b_PersonImage, pbImage);
                _personImage = personImage.b_PersonImage;
            }
       
            // Verificar el estado de la cola
            var ocupation = ListServiceComponent.Find(p => p.i_QueueStatusId == (int)Common.QueueStatusId.LLAMANDO
                                                        || p.i_QueueStatusId == (int)Common.QueueStatusId.OCUPADO);

            if (ocupation != null)        
                _IsCall = "OcupadoLlamado";
            else
                _IsCall = "Libre";

            ddlServiceStatusId.SelectedValue = ListServiceComponent[0].ServiceStatusId.ToString();
            txtReason.Text = ListServiceComponent[0].v_Motive.ToString();
        }

        private void Rellamar()
        {
            OperationResult objOperationResult = new OperationResult();
           
            ServiceBL objServiceBL = new ServiceBL();
            _ServiceComponentId = new List<string>();
            _serviceId = grdLlamandoPaciente.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString(); 
            var x = objServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, _categoryId, _serviceId);
            foreach (var item in x)
            {
                _ServiceComponentId.Add(item.v_ServiceComponentId);
            }

            foreach (var scid in _ServiceComponentId)
            {
                _serviceBL.UpdateServiceComponentVisor(ref objOperationResult, scid, (int)SiNo.SI);
            }
                
        }

        private void pbImage_Click(object sender, EventArgs e)
        {
            if (_personImage != null)
            {
                var frm = new Sigesoft.Node.WinClient.UI.Operations.Popups.frmPreviewImagePerson(_personImage, _personName);
                frm.ShowDialog();
            }

        }

        private void grdDataServiceComponent_MouseLeaveElement(object sender, Infragistics.Win.UIElementEventArgs e)
        {
            // if we are not leaving a cell, then don't anything
            if (!(e.Element is CellUIElement))
            {
                return;
            }

            // prevent the timer from ticking again
            _customizedToolTip.StopTimerToolTip();

            // destroy the tooltip
            _customizedToolTip.DestroyToolTip(this);
        }

        private void grdDataServiceComponent_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            ServiceBL oServiceBL = new ServiceBL();
            List<ServiceComponentList> oServiceComponentList = new List<ServiceComponentList>();
            StringBuilder Cadena = new StringBuilder();


            // if we are not entering a cell, then don't anything
            if (!(e.Element is CellUIElement))
            {
                return;
            }

            // find the cell that the cursor is over, if any
            UltraGridCell cell = e.Element.GetContext(typeof(UltraGridCell)) as UltraGridCell;

            if (cell != null)
            {
                int categoryId = int.Parse(cell.Row.Cells["i_CategoryId"].Value.ToString());
                oServiceComponentList = oServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, categoryId, _serviceId);


                if (categoryId != -1)
                {

                    foreach (var item in oServiceComponentList)
                    {
                        Cadena.Append(item.v_ComponentName);
                        Cadena.Append("\n");
                    }

                    _customizedToolTip.AutomaticDelay = 1;
                    _customizedToolTip.AutoPopDelay = 20000;
                    _customizedToolTip.ToolTipMessage = Cadena.ToString();
                    _customizedToolTip.StopTimerToolTip();
                    _customizedToolTip.StartTimerToolTip();
                }

            }
        }

        private void btnLlamar_Click(object sender, EventArgs e)
        {
            // AGVR 12/11/2016

            OperationResult objOperationResult = new OperationResult();
            ServiceBL objServiceBL = new ServiceBL();

            string[] compToxi = new string[]
            {    
                Constants.TOXICOLOGICO_COMPLETO_ID,
                Constants.TOXICOLOGICO_COCAINA_MARIHUANA_ID,
                Constants.TOXICOLOGICO_DOSAJE_10_DROGAS_ID,
                Constants.TOXICOLOGICO_DOSAJE_DROGAS_OPIACEOS_ID,
                Constants.TOXICOLOGICO_DOSAJE_METALES_ARSENICO_ID,               
            };

            var exams = objServiceBL.GetServiceComponents(ref objOperationResult, _serviceId);

            if (_componentName == "LABORATORIO")
            {

            }
            else
            {
                // buscar el toxi
                var findToxi = exams.Find(p => compToxi.Contains(p.v_ComponentId));

                if (findToxi != null)
                {
                    // Validar que se pase primero el toxi
                    if (findToxi.i_ServiceComponentStatusId == (int)ServiceComponentStatus.PorIniciar)
                    {
                        DialogResult Result = MessageBox.Show("Este paciente requiere resultados de toxicologico. Desea Esperar?", "INFORMACIÓN!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                        if (Result == System.Windows.Forms.DialogResult.Yes)
                        { return; }
                        //MessageBox.Show("Este paciente requiere resultados de toxicologico.", "INFORMACIÓN!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }

            }

            Llamar();

            //timer1.Start();
        }

        private void btnRellamar_Click(object sender, EventArgs e)
        {
            Rellamar();
        }

        private void btnAtenderVerServicio_Click(object sender, EventArgs e)
        {
            Atender();
        }

        private void btnLiberarFinalizarCircuito_Click(object sender, EventArgs e)
        {
            Liberar();
        }

        private void grdLlamandoPaciente_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            btnRellamar.Enabled = 
            btnAtenderVerServicio.Enabled = 
            btnLiberarFinalizarCircuito.Enabled = (grdLlamandoPaciente.Selected.Rows.Count > 0);
        }

        private void frmOffice_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (grdLlamandoPaciente.Rows.Count > 0)
            {
                grdLlamandoPaciente.Rows[0].Selected = true;
                var message = string.Format("Debe liberar al trabajador(a): {0} antes de cerrar.", grdListaLlamando.Selected.Rows[0].Cells["v_Pacient"].Value.ToString());
                MessageBox.Show(message, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

        private void grdListaLlamando_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            //timer1.Stop();

            frmNroTickes frm = new frmNroTickes();
            frm.ShowDialog();
            var y = frm.NroTickets;
            var o = grdListaLlamando.Selected.Rows[0].Cells["i_NroTickets"];
            o.Value = y;
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

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            ImprimirTicket oImprimirTicket = null;
            List<ImprimirTicket> ListImprimirTicket = new List<ImprimirTicket>();
            int j = 0;

            foreach (var item in grdListaLlamando.Rows)
            {
                oImprimirTicket = new ImprimirTicket();
                string x = item.Cells["i_NroTickets"].Value == null ? "0" : item.Cells["i_NroTickets"].Value.ToString();

                if (x != "0" || x != "")
                {
                    for (int i = 0; i < int.Parse(x); i++)
                    {
                        oImprimirTicket.v_ServicioId = item.Cells["v_ServiceId"].Value.ToString().Remove(0, 7);

                        if (item.Cells["v_Pacient"].Value.ToString().Length > 27)
                        { oImprimirTicket.NombreTrabajador = item.Cells["v_Pacient"].Value.ToString().Substring(0, 27); }
                        else
                        { oImprimirTicket.NombreTrabajador = item.Cells["v_Pacient"].Value.ToString(); }

                        var f = DateTime.Parse(item.Cells["d_ServiceDate"].Value.ToString());
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
                ////IMPRESORA DE CAMPAÑA
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

        #region MenuContextual Llamando Paciente

        private void liberarPacienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            ServiceBL objServiceBL = new ServiceBL();
            servicecomponentDto objservicecomponentDto = new servicecomponentDto();

            _serviceId = grdLlamandoPaciente.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();
            _categoryId = (int)grdLlamandoPaciente.Selected.Rows[0].Cells["i_CategoryId"].Value;

            _ServiceComponentId = new List<string>();

            foreach (var item in objServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, _categoryId, _serviceId))
            {
                _ServiceComponentId.Add(item.v_ServiceComponentId);
            }
            for (int i = 0; i < _ServiceComponentId.Count(); i++)
            {
                objservicecomponentDto = objServiceBL.GetServiceComponent(ref objOperationResult, _ServiceComponentId[i]);
                objservicecomponentDto.i_QueueStatusId = (int)Common.QueueStatusId.LIBRE;
                objServiceBL.UpdateServiceComponent(ref objOperationResult, objservicecomponentDto, Globals.ClientSession.GetAsList());
            }
        }
        
        private void porAprobaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            ServiceBL objServiceBL = new ServiceBL();
            servicecomponentDto objservicecomponentDto = new servicecomponentDto();

            _serviceId = grdLlamandoPaciente.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();
            _categoryId = (int)grdLlamandoPaciente.Selected.Rows[0].Cells["i_CategoryId"].Value;

            _ServiceComponentId = new List<string>();

            foreach (var item in objServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, _categoryId, _serviceId))
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

            _serviceId = grdLlamandoPaciente.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();
            _categoryId = (int)grdLlamandoPaciente.Selected.Rows[0].Cells["i_CategoryId"].Value;

            _ServiceComponentId = new List<string>();

            foreach (var item in objServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, _categoryId, _serviceId))
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

            _serviceId = grdLlamandoPaciente.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();
            _categoryId = (int)grdLlamandoPaciente.Selected.Rows[0].Cells["i_CategoryId"].Value;

            _ServiceComponentId = new List<string>();

            foreach (var item in objServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, _categoryId, _serviceId))
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

        #endregion

        #region MenuContextual ListaLlamado

        private void liberarPacienteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            ServiceBL objServiceBL = new ServiceBL();
            servicecomponentDto objservicecomponentDto = new servicecomponentDto();

            _serviceId = grdListaLlamando.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();
            _categoryId = (int)grdListaLlamando.Selected.Rows[0].Cells["i_CategoryId"].Value;

            _ServiceComponentId = new List<string>();

            foreach (var item in objServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, _categoryId, _serviceId))
            {
                _ServiceComponentId.Add(item.v_ServiceComponentId);
            }

            for (int i = 0; i < _ServiceComponentId.Count(); i++)
            {
                objservicecomponentDto = objServiceBL.GetServiceComponent(ref objOperationResult, _ServiceComponentId[i]);
                objservicecomponentDto.i_QueueStatusId = (int)Common.QueueStatusId.LIBRE;
                objServiceBL.UpdateServiceComponent(ref objOperationResult, objservicecomponentDto, Globals.ClientSession.GetAsList());
            }
        }

        private void porAprobaciónToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            ServiceBL objServiceBL = new ServiceBL();
            servicecomponentDto objservicecomponentDto = new servicecomponentDto();

            _serviceId = grdListaLlamando.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();
            _categoryId = (int)grdListaLlamando.Selected.Rows[0].Cells["i_CategoryId"].Value;

            _ServiceComponentId = new List<string>();

            foreach (var item in objServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, _categoryId, _serviceId))
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

        private void porReevaluarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            ServiceBL objServiceBL = new ServiceBL();
            servicecomponentDto objservicecomponentDto = new servicecomponentDto();

            _serviceId = grdListaLlamando.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();
            _categoryId = (int)grdListaLlamando.Selected.Rows[0].Cells["i_CategoryId"].Value;

            _ServiceComponentId = new List<string>();

            foreach (var item in objServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, _categoryId, _serviceId))
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

        private void culminadoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            ServiceBL objServiceBL = new ServiceBL();
            servicecomponentDto objservicecomponentDto = new servicecomponentDto();

            _serviceId = grdListaLlamando.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();
            _categoryId = (int)grdListaLlamando.Selected.Rows[0].Cells["i_CategoryId"].Value;

            _ServiceComponentId = new List<string>();

            foreach (var item in objServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, _categoryId, _serviceId))
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

        #endregion
    }
}
