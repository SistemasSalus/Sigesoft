﻿using System;
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
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid.DocumentExport;

namespace Sigesoft.Node.WinClient.UI
{
    public partial class frmCalendar : Form
    {
        string strFilterExpression;
        List<string> _ListaCalendar;
        string _PacientId;
        byte[] _personImage;
        string _personName;
        string _serviceId;
        int _RowIndexgrdDataCalendar;
        string _strServicelId;    
        CalendarBL _objCalendarBL = new CalendarBL();
        string _calendarId;
        List<CalendarList> _objData = new List<CalendarList>();
        private Sigesoft.Node.WinClient.UI.Utils.CustomizedToolTip _customizedToolTip = null;
        private List<KeyValueDTO> _formActions = null;

        public ServiceBL oServiceBL = new ServiceBL();
        int? _rowIndexOld;

        private bool _sendEmailEnabled;

        public frmCalendar()
        {
            InitializeComponent();
        }

        private void frmCalendar_Load(object sender, EventArgs e)
        {

            #region FormActions

            _formActions = Sigesoft.Node.WinClient.BLL.Utils.SetFormActionsInSession("frmCalendar",
                                                                               Globals.ClientSession.i_CurrentExecutionNodeId,
                                                                               Globals.ClientSession.i_RoleId.Value,
                                                                               Globals.ClientSession.i_SystemUserId);

            _sendEmailEnabled = Sigesoft.Node.WinClient.BLL.Utils.IsActionEnabled("frmCalendar_SENDEMAIL", _formActions);

            #endregion  

            UltraGridColumn c = grdDataCalendar.DisplayLayout.Bands[0].Columns["b_Seleccionar"];
            c.CellActivation = Activation.AllowEdit;
            c.CellClickAction = CellClickAction.Edit;

            _customizedToolTip = new Sigesoft.Node.WinClient.UI.Utils.CustomizedToolTip(grdDataServiceComponent);

            OperationResult objOperationResult = new OperationResult();

            Utils.LoadDropDownList(ddlServiceTypeId, "Value1", "Id", BLL.Utils.GetServiceType(ref objOperationResult,Globals.ClientSession.i_CurrentExecutionNodeId), DropDownListAction.All);
            Utils.LoadDropDownList(ddlMasterServiceId, "Value1", "Id", BLL.Utils.GetMasterService(ref objOperationResult, -1, Globals.ClientSession.i_CurrentExecutionNodeId), DropDownListAction.All);
            Utils.LoadDropDownList(ddlVipId, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 111, null), DropDownListAction.All);
            Utils.LoadDropDownList(ddlNewContinuationId, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 121, null), DropDownListAction.All);
            Utils.LoadDropDownList(ddlLineStatusId, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 120, null), DropDownListAction.All);
            Utils.LoadDropDownList(ddlCalendarStatusId, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 122, null), DropDownListAction.All);
            int nodeId = int.Parse(Common.Utils.GetApplicationConfigValue("NodeId"));
            var dataListOrganization1 = BLL.Utils.GetJoinOrganizationAndLocation(ref objOperationResult, nodeId);
            Utils.LoadDropDownList(cbCustomerOrganization,
                "Value1",
                "Id",
                dataListOrganization1,
                DropDownListAction.All, true);
            dtpDateTimeStar.CustomFormat = "dd/MM/yyyy";
            dptDateTimeEnd.CustomFormat = "dd/MM/yyyy";
            // Establecer el filtro inicial para los datos
            strFilterExpression = null;
            //btnFilter_Click(sender, e);
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            btnFilter.Enabled = false;
            this.Enabled = false;
           
            // Get the filters from the UI
            List<string> Filters = new List<string>();
            if (ddlServiceTypeId.SelectedValue.ToString() != "-1") Filters.Add("i_ServiceTypeId==" + ddlServiceTypeId.SelectedValue);
            if (ddlMasterServiceId.SelectedValue.ToString() != "-1") Filters.Add("i_ServiceId==" + ddlMasterServiceId.SelectedValue);
            if (!string.IsNullOrEmpty(txtPacient.Text)) Filters.Add("v_Pacient.Contains(\"" + txtPacient.Text.Trim() + "\")");
            if (!string.IsNullOrEmpty(txtNroDocument.Text)) Filters.Add("v_NumberDocument==" + "\"" + txtNroDocument.Text.Trim() + "\"");
            if (ddlVipId.SelectedValue.ToString() != "-1") Filters.Add("i_IsVipId==" + ddlVipId.SelectedValue);
            if (ddlCalendarStatusId.SelectedValue.ToString() != "-1") Filters.Add("i_CalendarStatusId==" + ddlCalendarStatusId.SelectedValue);
            if (ddlNewContinuationId.SelectedValue.ToString() != "-1") Filters.Add("i_NewContinuationId==" + ddlNewContinuationId.SelectedValue);
            if (ddlLineStatusId.SelectedValue.ToString() != "-1") Filters.Add("i_LineStatusId==" + ddlLineStatusId.SelectedValue);
            if (cbCustomerOrganization.SelectedValue.ToString() != "-1")
            {
                var id2 = cbCustomerOrganization.SelectedValue.ToString().Split('|');
                Filters.Add("v_CustomerOrganizationId==" + "\"" + id2[0] + "\"&&v_CustomerLocationId==" + "\"" + id2[1] + "\"");

            }          

            // Create the Filter Expression
            strFilterExpression = null;
            if (Filters.Count > 0)
            {
                foreach (string item in Filters)
                {
                    strFilterExpression = strFilterExpression + item + " && ";
                }
                strFilterExpression = strFilterExpression.Substring(0, strFilterExpression.Length - 4);
            }

            this.BindGrid();
            btnFilter.Enabled = true;
            this.Enabled = true;
            
        }

        private void BindGrid()
        {
            var objData = GetData(0, null, "d_EntryTimeCM ASC", strFilterExpression);

            grdDataCalendar.DataSource = objData;
            this.grdDataServiceComponent.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;

            lblRecordCountCalendar.Text = string.Format("Se encontraron {0} registros.", objData.Count());

            txtTrabajador.Text = "";
            WorkingOrganization.Text = "";
            txtProtocol.Text = "";
            txtService.Text = "";
            txtTypeESO.Text = "";
            pbImage.Image = null;
            txtExisteHuella.Text = "";
            grdDataServiceComponent.DataSource = new List<ServiceComponentList>();

            if (grdDataCalendar.Rows.Count > 0)
            {
                if (_rowIndexOld == null)
                    grdDataCalendar.Rows[0].Selected = true;
                else
                    grdDataCalendar.Rows[_rowIndexOld.Value].Selected = true;
            }
            else
            {
                _rowIndexOld = null;
            }
        }

        private List<CalendarList> GetData(int pintPageIndex, int? pintPageSize, string pstrSortExpression, string pstrFilterExpression)
        {
            OperationResult objOperationResult = new OperationResult();
            DateTime? pdatBeginDate = dtpDateTimeStar.Value.Date;
            DateTime? pdatEndDate = dptDateTimeEnd.Value.Date.AddDays(1);
            
             _objData = _objCalendarBL.GetCalendarsPagedAndFiltered(ref objOperationResult, pintPageIndex, pintPageSize, pstrSortExpression, pstrFilterExpression, pdatBeginDate, pdatEndDate);

            if (objOperationResult.Success != 1)
            {
                MessageBox.Show("Error en operación:" + System.Environment.NewLine + objOperationResult.ExceptionMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return _objData;
        }
        
        private void btnPerson_Click(object sender, EventArgs e)
        {
            frmSchedulePerson frm = new frmSchedulePerson("","New","");
            MdiParent = ParentForm;
            frm.Show();
            if (frm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                // Refrescar grilla
                btnFilter_Click(sender, e);
            }
        }

        private void btnMassive_Click(object sender, EventArgs e)
        {
            frmschedulePeople frm = new frmschedulePeople();
            MdiParent = ParentForm;
            frm.Show();
            if (frm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                // Refrescar grilla
                btnFilter_Click(sender, e);
            }
        }

        private void mnuCancelCalendar_Click(object sender, EventArgs e)
        {
            CalendarBL objCalendarBL = new CalendarBL();
            calendarDto objCalendarDto = new calendarDto();
            //DAVID
            ServiceBL objServiceBL = new ServiceBL();
            serviceDto objServiceDto = new serviceDto();
            OperationResult objOperationResult = new OperationResult();

               DialogResult Result = MessageBox.Show("¿Está seguro de CANCELAR este registro?", "ADVERTENCIA!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

               if (Result == System.Windows.Forms.DialogResult.Yes)
               {
                   string strCalendarId = grdDataCalendar.Selected.Rows[0].Cells[0].Value.ToString();

                   objCalendarDto = objCalendarBL.GetCalendar(ref objOperationResult, strCalendarId);
                   objCalendarDto.v_CalendarId = strCalendarId;
                   objCalendarDto.i_CalendarStatusId = (int)Common.CalendarStatus.Cancelado;
                   objCalendarDto.i_LineStatusId = (int)Common.LineStatus.FueraCircuito;

                   objCalendarBL.UpdateCalendar(ref objOperationResult, objCalendarDto, Globals.ClientSession.GetAsList());

                   //DAVID para que puedan volver a agendar
                   objServiceDto = objServiceBL.GetService(ref objOperationResult, objCalendarDto.v_ServiceId);
                   objServiceDto.v_ServiceId = objCalendarDto.v_ServiceId;
                   objServiceDto.i_ServiceStatusId = (int)Common.ServiceStatus.Cancelado;

                   objServiceBL.UpdateService(ref objOperationResult, objServiceDto, Globals.ClientSession.GetAsList());

                   btnFilter_Click(sender, e);
               }
           
        }

        private void mnuFinCircuito_Click(object sender, EventArgs e)
        {
            CalendarBL objCalendarBL = new CalendarBL();
            calendarDto objCalendarDto = new calendarDto();
            OperationResult objOperationResult = new OperationResult();

                DialogResult Result = MessageBox.Show("¿Está seguro de TERMINAR CIRCUITO este registro?", "ADVERTENCIA!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (Result == System.Windows.Forms.DialogResult.Yes)
                {
                    string strCalendarId = grdDataCalendar.Selected.Rows[0].Cells[0].Value.ToString();

                    objCalendarDto = objCalendarBL.GetCalendar(ref objOperationResult, strCalendarId);
                    objCalendarDto.v_CalendarId = strCalendarId;
                    objCalendarDto.i_LineStatusId = (int)Common.LineStatus.FueraCircuito;

                    objCalendarBL.UpdateCalendar(ref objOperationResult, objCalendarDto, Globals.ClientSession.GetAsList());

                    btnFilter_Click(sender, e);
                }
           
        }

        private void mnuComenzarCircuito_Click(object sender, EventArgs e)
        {
            CalendarBL objCalendarBL = new CalendarBL();            
            calendarDto objCalendarDto = new calendarDto();
            ServiceBL objServiceBL = new ServiceBL();
            serviceDto objServiceDto = new serviceDto();
            OperationResult objOperationResult = new OperationResult();
            List<ServiceComponentList> ListServiceComponent = new List<ServiceComponentList>();

            DateTime FechaAgenda = DateTime.Parse(grdDataCalendar.Selected.Rows[0].Cells[4].Value.ToString());
            if (FechaAgenda.Date != DateTime.Now.Date)
            {
                MessageBox.Show("No se permite Iniciar Circuito con una fecha que no sea la actual.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
              DialogResult Result = MessageBox.Show("¿Está seguro de INICIAR CIRCUITO este registro?", "ADVERTENCIA!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

              if (Result == System.Windows.Forms.DialogResult.Yes)
              {
                  string strCalendarId = grdDataCalendar.Selected.Rows[0].Cells[0].Value.ToString();

                  objCalendarBL.CircuitStart(ref objOperationResult, strCalendarId, DateTime.Now, Globals.ClientSession.GetAsList());

                  objServiceDto = objServiceBL.GetService(ref objOperationResult, grdDataCalendar.Selected.Rows[0].Cells[5].Value.ToString());

                  var NewCont =grdDataCalendar.Selected.Rows[0].Cells["i_NewContinuationId"].Value;
                  if ((int)NewCont  == (int)Common.modality.NuevoServicio)
                  {
                      objServiceDto.i_ServiceStatusId = (int)Common.ServiceStatus.Iniciado;
                  }
                  else if ((int)NewCont  == (int)Common.modality.ContinuacionServicio)
                  {
                      objServiceDto.i_ServiceStatusId =int.Parse(grdDataCalendar.Selected.Rows[0].Cells["i_ServiceStatusId"].Value.ToString());
                  }
                  
                  objServiceBL.UpdateService(ref objOperationResult, objServiceDto, Globals.ClientSession.GetAsList());

                   _strServicelId = grdDataCalendar.Selected.Rows[0].Cells[5].Value.ToString();

                  btnFilter_Click(sender, e);                              
                                  
                  ListServiceComponent = objServiceBL.GetServiceComponents(ref objOperationResult, _strServicelId);
                  grdDataServiceComponent.DataSource = ListServiceComponent;
                  
                  grdDataCalendar.Rows[_RowIndexgrdDataCalendar].Selected = true;

                  MessageBox.Show("Circuito iniciado, paciente disponible para su atención", " ¡ INFORMACIÓN !", MessageBoxButtons.OK, MessageBoxIcon.Information);

              }
           
        }

        private void mnuRefrescar_Click(object sender, EventArgs e)
        {
            btnFilter_Click(sender, e);
        }

        private void mnuReagendarCita_Click(object sender, EventArgs e)
        {
            string strCalendarId = grdDataCalendar.Selected.Rows[0].Cells[0].Value.ToString();
            string strProtocolId = grdDataCalendar.Selected.Rows[0].Cells[12].Value.ToString();

              DialogResult Result = MessageBox.Show("¿Está seguro de REAGENDAR este registro?", "ADVERTENCIA!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

              if (Result == System.Windows.Forms.DialogResult.Yes)
              {
                  frmSchedulePerson frm = new frmSchedulePerson(strCalendarId, "Reschedule", strProtocolId);
                  frm.ShowDialog();
                  if (frm.DialogResult == System.Windows.Forms.DialogResult.OK)
                  {
                      // Refrescar grilla
                      btnFilter_Click(sender, e);
                  }
              }
        }
        
        private void ddlServiceTypeId_TextChanged(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            Utils.LoadDropDownList(ddlMasterServiceId, "Value1", "Id", BLL.Utils.GetMasterService(ref objOperationResult, int.Parse(ddlServiceTypeId.SelectedValue.ToString()), Globals.ClientSession.i_CurrentExecutionNodeId), DropDownListAction.All);
            
        }

        private void grdDataCalendar_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
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

        private void grdDataCalendar_MouseDown(object sender, MouseEventArgs e)
        {
            Point point = new System.Drawing.Point(e.X, e.Y);
            Infragistics.Win.UIElement uiElement = ((Infragistics.Win.UltraWinGrid.UltraGridBase)sender).DisplayLayout.UIElement.ElementFromPoint(point);
           

            if (uiElement == null || uiElement.Parent == null)
                return;

            Infragistics.Win.UltraWinGrid.UltraGridRow row = (Infragistics.Win.UltraWinGrid.UltraGridRow)uiElement.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridRow));
            
            if (e.Button == MouseButtons.Right)
            {

                if (row != null)
                {
                    _RowIndexgrdDataCalendar = row.Index;
                    grdDataCalendar.Rows[row.Index].Selected = true;
                    int CalendarStatusId = int.Parse(grdDataCalendar.Selected.Rows[0].Cells["i_CalendarStatusId"].Value.ToString());
                    int LineStatusId = int.Parse(grdDataCalendar.Selected.Rows[0].Cells["i_LineStatusId"].Value.ToString());
                    int ServiceStatusId = int.Parse(grdDataCalendar.Selected.Rows[0].Cells["i_ServiceStatusId"].Value.ToString());
                    _PacientId = grdDataCalendar.Selected.Rows[0].Cells["v_PersonId"].Value.ToString();

                    if (CalendarStatusId == (int)Common.CalendarStatus.Agendado)
                    {
                        contextMenuStrip1.Items["mnuCancelCalendar"].Enabled = true;
                        contextMenuStrip1.Items["mnuFinCircuito"].Enabled = false;
                        contextMenuStrip1.Items["mnuComenzarCircuito"].Enabled = true;
                        contextMenuStrip1.Items["mnuReagendarCita"].Enabled = true;  
                    }
                    else if (CalendarStatusId == (int)Common.CalendarStatus.Atendido)
                    {
                        contextMenuStrip1.Items["mnuCancelCalendar"].Enabled = false;
                        contextMenuStrip1.Items["mnuFinCircuito"].Enabled = true;
                        contextMenuStrip1.Items["mnuComenzarCircuito"].Enabled = false;
                        contextMenuStrip1.Items["mnuReagendarCita"].Enabled = false;   
                    }
                    else if (CalendarStatusId == (int)Common.CalendarStatus.Cancelado)
                    {
                        contextMenuStrip1.Items["mnuCancelCalendar"].Enabled = false;
                        contextMenuStrip1.Items["mnuFinCircuito"].Enabled = false;
                        contextMenuStrip1.Items["mnuComenzarCircuito"].Enabled = false;
                        contextMenuStrip1.Items["mnuReagendarCita"].Enabled = true;
                    }

                    if (LineStatusId == (int)Common.LineStatus.FueraCircuito && CalendarStatusId == (int)Common.CalendarStatus.Atendido )
                    {
                        contextMenuStrip1.Items["mnuCancelCalendar"].Enabled = false;
                        contextMenuStrip1.Items["mnuFinCircuito"].Enabled = false;
                        contextMenuStrip1.Items["mnuComenzarCircuito"].Enabled = false;
                        contextMenuStrip1.Items["mnuReagendarCita"].Enabled = false;
                    }

                 

                } 
                else
                {
                    contextMenuStrip1.Items["mnuCancelCalendar"].Enabled = false;
                    contextMenuStrip1.Items["mnuFinCircuito"].Enabled = false;
                    contextMenuStrip1.Items["mnuComenzarCircuito"].Enabled = false;
                    contextMenuStrip1.Items["mnuReagendarCita"].Enabled = false;                    
                }               
            }

         
        }

        private void mnuVerPaciente_Click(object sender, EventArgs e)
        {
            this.Enabled = false;

            var frm = new frmPacient(_PacientId);
            frm.ShowDialog();

            if (!frm.IsSaved)
            {
                this.Enabled = true;
                return;
            }

            btnFilter_Click(sender, e);

            this.Enabled = true;

        }


        private void grdDataCalendarAfterSelectChange()
        {
            btnAgregarAdicionales.Enabled = btnConsentimiento.Enabled = btnExportExcel.Enabled = btnExportPdf.Enabled = btnImprimirHojaRuta.Enabled = (grdDataCalendar.Selected.Rows.Count > 0);
            btnSendEmail.Enabled = (grdDataCalendar.Selected.Rows.Count > 0 && _sendEmailEnabled);


            if (grdDataCalendar.Selected.Rows.Count != 0)
            {
                // Guardar el indice fila seleccionada
                _rowIndexOld = grdDataCalendar.Selected.Rows[0].Index;

                OperationResult objOperationResult = new OperationResult();
                ServiceBL objServiceBL = new ServiceBL();
                PacientBL objPacientBL = new PacientBL();
                personDto objpersonDto = new personDto();
                List<ServiceComponentList> ListServiceComponent = new List<ServiceComponentList>();
                //string strServicelId = grdDataCalendar.Selected.Rows[0].Cells[5].Value.ToString();
                _strServicelId = grdDataCalendar.Selected.Rows[0].Cells[5].Value.ToString();

                ListServiceComponent = objServiceBL.GetServiceComponents(ref objOperationResult, _strServicelId);
                grdDataServiceComponent.DataSource = ListServiceComponent;

                _PacientId = grdDataCalendar.Selected.Rows[0].Cells["v_PersonId"].Value.ToString();

                _serviceId = grdDataCalendar.Selected.Rows[0].Cells["v_ServiceId"].Value.ToString();

                _calendarId = grdDataCalendar.Selected.Rows[0].Cells["v_CalendarId"].Value.ToString();

                if (grdDataCalendar.Selected.Rows[0].Cells["v_WorkingOrganizationName"].Value != null)
                    WorkingOrganization.Text = grdDataCalendar.Selected.Rows[0].Cells["v_WorkingOrganizationName"].Value.ToString();


                if (grdDataCalendar.Selected.Rows[0].Cells["v_ProtocolId"].Value != null)
                {
                    if (grdDataCalendar.Selected.Rows[0].Cells["v_ProtocolId"].Value.ToString() == Constants.CONSULTAMEDICA)
                    {
                        txtTypeESO.Text = "";
                    }
                    else
                    {
                        if (grdDataCalendar.Selected.Rows[0].Cells["v_EsoTypeName"].Value != null)
                            txtTypeESO.Text = grdDataCalendar.Selected.Rows[0].Cells["v_EsoTypeName"].Value.ToString();
                    }
                }

                objpersonDto = objPacientBL.GetPerson(ref objOperationResult, _PacientId);

                // Setear datos personales del paciente  al lado derecho
                txtTrabajador.Text = objpersonDto.v_FirstLastName + " " + objpersonDto.v_SecondLastName + " " + objpersonDto.v_FirstName;
                txtProtocol.Text = grdDataCalendar.Selected.Rows[0].Cells["v_ProtocolName"].Value == null ? "" : grdDataCalendar.Selected.Rows[0].Cells["v_ProtocolName"].Value.ToString();
                txtService.Text = grdDataCalendar.Selected.Rows[0].Cells["v_ServiceName"].Value.ToString();

                //Byte[] ooo = (byte[])grdDataCalendar.Selected.Rows[0].Cells["b_PersonImage"].Value;
                Byte[] ooo = objpersonDto.b_PersonImage;
                if (ooo == null)
                {
                    //pbImage.SizeMode = PictureBoxSizeMode.CenterImage;
                    pbImage.Image = Resources.nofoto;
                    _personImage = null;

                }
                else
                {
                    //pbImage.SizeMode = PictureBoxSizeMode.Zoom;
                    pbImage.Image = Common.Utils.BytesArrayToImageOficce(ooo, pbImage);
                    _personImage = ooo;
                }

                // Huella y Firma
                if (objpersonDto.b_FingerPrintImage == null)
                {
                    txtExisteHuella.Text = "NO REGISTRADO";
                    txtExisteHuella.ForeColor = Color.Red;
                }
                else
                {
                    txtExisteHuella.Text = "REGISTRADO";
                    txtExisteHuella.ForeColor = Color.DarkBlue;
                }

                // Firma
                if (objpersonDto.b_RubricImage == null)
                {
                    txtExisteFirma.Text = "NO REGISTRADO";
                    txtExisteFirma.ForeColor = Color.Red;
                }
                else
                {
                    txtExisteFirma.Text = "REGISTRADO";
                    txtExisteFirma.ForeColor = Color.DarkBlue;
                }

            }

            //btnImprimirHojaRuta.Enabled = (grdDataServiceComponent.Rows.Count > 0);
        }

        private void grdDataCalendar_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
        {
            grdDataCalendarAfterSelectChange();
        }

        private void grdDataCalendar_ClickCell(object sender, ClickCellEventArgs e)
        {
            if ((e.Cell.Column.Key == "b_Seleccionar"))
            {
                if ((e.Cell.Value.ToString() == "False"))
                {
                    e.Cell.Value = true;                  
                }
                else
                {
                    e.Cell.Value = false;                    
                }
            }
            grdDataCalendarAfterSelectChange();
        }

        private void pbImage_Click(object sender, EventArgs e)
        {
            if (_personImage != null)
            {
                var frm = new Sigesoft.Node.WinClient.UI.Operations.Popups.frmPreviewImagePerson(_personImage, _personName);
                frm.ShowDialog();
            }
        }
      
        private void grdDataServiceComponent_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            
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
                oServiceComponentList = oServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, categoryId, _strServicelId);


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

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = string.Empty;
            saveFileDialog1.Filter = "Files (*.xls;*.xlsx;*)|*.xls;*.xlsx;*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.ultraGridExcelExporter1.Export(this.grdDataCalendar, saveFileDialog1.FileName);
                MessageBox.Show("Se exportaron correctamente los datos.", " ¡ INFORMACIÓN !", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }       
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = string.Empty;
            saveFileDialog1.Filter = "Files (*.pdf;)|*.pdf;";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.ultraGridDocumentExporter1.Export(this.grdDataCalendar, saveFileDialog1.FileName,GridExportFileFormat.PDF);
                MessageBox.Show("Se exportaron correctamente los datos.", " ¡ INFORMACIÓN !", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }    
        }

        private void VerAntecedentes_Click(object sender, EventArgs e)
        {
            string pstrPacientId = grdDataCalendar.Selected.Rows[0].Cells["v_PersonId"].Value.ToString();        
            frmHistory frm = new frmHistory(pstrPacientId);
            frm.ShowDialog();
        }

        private void btnImprimirHojaRuta_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmRoadMap(_strServicelId, _calendarId);
            frm.MdiParent = this.MdiParent;
            frm.Show();
            
        }

        private void grdDataServiceComponent_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            btnRemoverExamen.Enabled = (grdDataServiceComponent.Selected.Rows.Count > 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            CalendarListEmail oCalendarListEmail = new CalendarListEmail();
            List<CalendarListEmail> oList = new List<CalendarListEmail>();

            try
            {
                foreach (var item in _objData)
                {
                    oCalendarListEmail = new CalendarListEmail();
                    oCalendarListEmail.v_EntryTimeCM = item.d_EntryTimeCM.ToString();
                    oCalendarListEmail.v_Pacient = item.v_Pacient;
                    oCalendarListEmail.v_NumberDocument = item.v_NumberDocument;
                    oCalendarListEmail.v_ProtocolName = item.v_ProtocolName;
                    oCalendarListEmail.v_ServiceTypeName = item.v_ServiceTypeName;
                    oCalendarListEmail.v_EsoTypeName = item.v_EsoTypeName;

                    oList.Add(oCalendarListEmail);
                }
                //LogList xxx = new LogList();
                oCalendarListEmail = new CalendarListEmail();
                oCalendarListEmail.v_EntryTimeCM = "INGRESO CENTRO MÉDICO";
                oCalendarListEmail.v_Pacient = "PACIENTE";
                oCalendarListEmail.v_NumberDocument = "NRO. DOCUMENTO";
                oCalendarListEmail.v_ProtocolName = "PROTOCOLO";
                oCalendarListEmail.v_ServiceTypeName = "TIPO SERVICIO";
                oCalendarListEmail.v_EsoTypeName = "TIPO ESO";

                oList.Insert(0, oCalendarListEmail);

                frmEmail frm = new frmEmail(oList, cbCustomerOrganization.Text.ToString(), dtpDateTimeStar.Value.ToShortDateString(), dptDateTimeEnd.Value.ToShortDateString());
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void grdDataServiceComponent_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            //Si el contenido de la columna Vip es igual a SI
            if (e.Row.Cells["i_ServiceComponentStatusId"].Value.ToString() == ((int)Common.ServiceComponentStatus.Culminado).ToString())
            {
                //Escojo 2 colores
                e.Row.Appearance.BackColor = Color.White;
                e.Row.Appearance.BackColor2 = Color.Cyan;
                //Y doy el efecto degradado vertical
                e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;

            }
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtNroDocument_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnFilter_Click(null, null);
            }
        }

        private void txtPacient_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnFilter_Click(null, null);
            }          
        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void btnConsentimiento_Click(object sender, EventArgs e)
        {
            var frm = new Reports.frmConsentimiento(_serviceId);
            frm.MdiParent = this.MdiParent;
            frm.Show();
        }

        private void mnuListaNegra_Click(object sender, EventArgs e)
        {
            string pstrPersonId = grdDataCalendar.Selected.Rows[0].Cells["v_PersonId"].Value.ToString();
            string pstrPaciente = grdDataCalendar.Selected.Rows[0].Cells["v_Pacient"].Value.ToString();

            frmBlackList frm = new frmBlackList(pstrPaciente,pstrPersonId);
            frm.ShowDialog();
        }

        private void btnAgregarAdicionales_Click(object sender, EventArgs e)
        {
            var frm = new frmAddAdditionalExam();
            frm._serviceId = _serviceId;
            frm.ShowDialog();

            if (frm.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                return;

            OperationResult objOperationResult = new OperationResult();
            var ListServiceComponent = oServiceBL.GetServiceComponents(ref objOperationResult, _strServicelId);
            grdDataServiceComponent.DataSource = ListServiceComponent;
        }

        private void btnRemoverExamen_Click(object sender, EventArgs e)
        {
             DialogResult Result = MessageBox.Show("¿Está seguro de eliminar este registro?", "ADVERTENCIA!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

             if (Result == System.Windows.Forms.DialogResult.OK)
             {

                 var _auxiliaryExams = new List<ServiceComponentList>();
                 OperationResult objOperationResult = new OperationResult();

                 //int categoryId = int.Parse(grdDataServiceComponent.Selected.Rows[0].Cells["i_CategoryId"].Value.ToString());
                 int categoryId = -1;
                 var serviceComponentId = grdDataServiceComponent.Selected.Rows[0].Cells["v_ServiceComponentId"].Value.ToString();

                 if (categoryId == -1)
                 {
                     ServiceComponentList auxiliaryExam = new ServiceComponentList();
                     auxiliaryExam.v_ServiceComponentId = serviceComponentId;
                     _auxiliaryExams.Add(auxiliaryExam);
                 }
                 else
                 {
                     var oServiceComponentList = oServiceBL.GetServiceComponentByCategoryId(ref objOperationResult, categoryId, _strServicelId);

                     foreach (var scid in oServiceComponentList)
                     {
                         ServiceComponentList auxiliaryExam = new ServiceComponentList();
                         auxiliaryExam.v_ServiceComponentId = scid.v_ServiceComponentId;
                         _auxiliaryExams.Add(auxiliaryExam);
                     }

                 }

                 _objCalendarBL.UpdateAdditionalExam(_auxiliaryExams, _serviceId, (int?)SiNo.NO, Globals.ClientSession.GetAsList());
                 var ListServiceComponent = oServiceBL.GetServiceComponents(ref objOperationResult, _strServicelId);
                 grdDataServiceComponent.DataSource = ListServiceComponent;
                 //MessageBox.Show("Se grabo correctamente", " ¡ INFORMACIÓN !", MessageBoxButtons.OK, MessageBoxIcon.Information);
             }

        }

        private void btnIniciarCircuitoMasivo_Click(object sender, EventArgs e)
        {
            CalendarBL objCalendarBL = new CalendarBL();
            _ListaCalendar = new List<string>();
            foreach (var item in grdDataCalendar.Rows)
            {                
                if ((bool)item.Cells["b_Seleccionar"].Value)
                {
                    string x = item.Cells["v_CalendarId"].Value.ToString();
                    _ListaCalendar.Add(x);
                }
            }

            if (_ListaCalendar.Count == 0)
            {
                MessageBox.Show("No hay ningún servicio con check, por favor seleccionar uno.", "VALIDACIÓN!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                OperationResult objOperationResult = new OperationResult();
                foreach (var item in _ListaCalendar)
                {                   
                    objCalendarBL.CircuitStart(ref objOperationResult, item.ToString(), DateTime.Now, Globals.ClientSession.GetAsList());

                }
            }
            MessageBox.Show("Se inició correctamente el inicio de circuito", "SISTEMAS!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnFilter_Click(sender, e);
        }

      
       
    
    }
}
