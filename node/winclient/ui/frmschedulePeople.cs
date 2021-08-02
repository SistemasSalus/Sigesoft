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
using System.IO;
using System.Drawing.Imaging;
using Microsoft.Office.Interop.Excel;
using Infragistics.Documents.Excel;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using System.Globalization;

namespace Sigesoft.Node.WinClient.UI
{
    public partial class frmschedulePeople : Form
    {
        List<PacientList> _TempPacientList;
        string _ProtocolId;
        int _IndexgrdDataPeople;

        public frmschedulePeople()
        {
            InitializeComponent();
        }

        private void frmschedulePeople_Load(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            //Llenado de combos
            Utils.LoadDropDownList(ddlServiceTypeId, "Value1", "Id", BLL.Utils.GetServiceType(ref objOperationResult, Globals.ClientSession.i_CurrentExecutionNodeId), DropDownListAction.Select);
            ddlServiceTypeId.SelectedValue = "1";
            //Modificado para CELIMA
            //Utils.LoadDropDownList(ddlMasterServiceId, "Value1", "Id", BLL.Utils.GetMasterService(ref objOperationResult,, Globals.ClientSession.i_CurrentExecutionNodeId), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlMasterServiceId, "Value1", "Id", BLL.Utils.GetServiceType(ref objOperationResult, Globals.ClientSession.i_CurrentExecutionNodeId), DropDownListAction.Select);
            ddlMasterServiceId.SelectedValue = "1";
            Utils.LoadDropDownList(ddlVipId, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 111, null), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlNewContinuationId, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 121, null), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlLineStatusId, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 120, null), DropDownListAction.Select);
            Utils.LoadDropDownList(ddlCalendarStatusId, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 122, null), DropDownListAction.Select);

            Utils.LoadDropDownList(ddlOrganization, "Value1", "Id", BLL.Utils.GetJoinOrganizationAndLocation(ref objOperationResult, Globals.ClientSession.i_CurrentExecutionNodeId), DropDownListAction.Select);

            dtpDateTimeCalendar.CustomFormat = "dd/MM/yyyy hh:mm:ss tt";
            ddlLineStatusId.SelectedValue =  ((int)LineStatus.FueraCircuito).ToString(); //2
            ddlNewContinuationId.SelectedValue = ((int)modality.NuevoServicio).ToString(); //"1";
            ddlCalendarStatusId.SelectedValue = ((int)CalendarStatus.Agendado).ToString(); //"1";
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            int Value = 0;
            bool Imported= true;
            int ErrorCounter = 0;
            DateTime ValueDateTime;
            StringBuilder sbMensaje = new StringBuilder();
            //if (_TempPacientList == null) return;
            if (_TempPacientList != null)
            {
                if (MessageBox.Show("Ya existe una lista de pacientes por agendar; ¿Desea reemplazarla?.", "Pregunta", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.Filter = "Image Files (*.xls;*.xlsx)|*.xls;*.xlsx";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _TempPacientList = new List<PacientList>();

                var Ext = Path.GetExtension(openFileDialog1.FileName).ToUpper();

                //try
                //{
                    if (Ext == ".XLSX" || Ext == ".XLS")
                    {
                  
                        Infragistics.Documents.Excel.Workbook workbook1 = Infragistics.Documents.Excel.Workbook.Load(openFileDialog1.FileName);
                 
                        Infragistics.Documents.Excel.Worksheet worksheet1 = workbook1.Worksheets["PLANTILLA"];

                        PacientList TempPacient;

                        int i = 4;
                        int ii = 4;
                        //Validar que el excel no esta vacio
                        while (worksheet1.Rows[ii].Cells[0].Value != null)
                        {
                            if (worksheet1.Rows[ii].Cells[0].Value == null || worksheet1.Rows[ii].Cells[1].Value == null || worksheet1.Rows[ii].Cells[2].Value == null || worksheet1.Rows[ii].Cells[3].Value == null || worksheet1.Rows[ii].Cells[4].Value == null || worksheet1.Rows[ii].Cells[5].Value == null || worksheet1.Rows[ii].Cells[6].Value == null || worksheet1.Rows[ii].Cells[7].Value == null || worksheet1.Rows[ii].Cells[8].Value == null || worksheet1.Rows[ii].Cells[9].Value == null)
                            {

                                for (int y = 0; y <= 9; y++)
                                {
                                    if (worksheet1.Rows[ii].Cells[y].Value == null)
                                    {
                                        Imported = false;
                                        sbMensaje.Append("Registro número : ");
                                        sbMensaje.Append(worksheet1.Rows[ii].Cells[0].Value);
                                        sbMensaje.Append(". El campo " + worksheet1.Rows[3].Cells[y].Value.ToString()  +" no puede estar vacio");
                                        sbMensaje.Append("\n");
                                    }
                                }
                            }
                            ii++;
                        }
                        if (Imported ==  false)
                        {
                            MessageBox.Show(sbMensaje.ToString(), "Corregir registros en blanco", MessageBoxButtons.OK, MessageBoxIcon.Warning);     
                            return;
                        }

                   
                        while (worksheet1.Rows[i].Cells[0].Value != null )
                        {
                            TempPacient = new PacientList();

                            if (worksheet1.Rows[i].Cells[0].Value != null)
                            {
                                TempPacient.i_Correlative = int.Parse(worksheet1.Rows[i].Cells[0].Value.ToString());
                                Imported = true;
                            }
                            //Nombres
                            if (worksheet1.Rows[i].Cells[1].Value != null)
                            {
                                TempPacient.v_FirstName = worksheet1.Rows[i].Cells[1].Value.ToString();
                                Imported = true;
                            }
                            else
                            {
                                ErrorCounter++;
                                Imported = false;
                                sbMensaje.Append("Registro número : ");
                                sbMensaje.Append(worksheet1.Rows[i].Cells[0].Value);
                                sbMensaje.Append(". El campo Nombres es inválido");
                                sbMensaje.Append("\n");
                                i++;
                                continue;
                            }
                            //Apellido Paterno
                            if (worksheet1.Rows[i].Cells[2].Value != null)
                            {
                                TempPacient.v_FirstLastName = worksheet1.Rows[i].Cells[2].Value.ToString();
                                Imported = true;
                            }
                            else
                            {
                                ErrorCounter++;
                                Imported = false;
                                sbMensaje.Append("Registro número : ");
                                sbMensaje.Append(worksheet1.Rows[i].Cells[0].Value);
                                sbMensaje.Append(". El campo Apellido Paterno es inválido");
                                sbMensaje.Append("\n");
                                i++;
                                continue;
                            }
                            //Apellido Materno
                            if (worksheet1.Rows[i].Cells[3].Value != null)
                            {
                                TempPacient.v_SecondLastName = worksheet1.Rows[i].Cells[3].Value.ToString();
                                Imported = true;
                            }
                            else
                            {
                                ErrorCounter++;
                                Imported = false;
                                sbMensaje.Append("Registro número : ");
                                sbMensaje.Append(worksheet1.Rows[i].Cells[0].Value);
                                sbMensaje.Append(". El campo Apellido Materno es inválido");
                                sbMensaje.Append("\n");
                                i++;
                                continue;
                            }
                            //ID Tipo Documento
                            if (worksheet1.Rows[i].Cells[4].Value == null)
                            {
                                ErrorCounter++;
                                Imported = false;
                                sbMensaje.Append("Registro número : ");
                                sbMensaje.Append(worksheet1.Rows[i].Cells[0].Value);
                                sbMensaje.Append(". El campo ID Tipo Documento es inválido");
                                sbMensaje.Append("\n");
                                i++;
                                continue;
                            }
                            if (int.TryParse(worksheet1.Rows[i].Cells[4].Value.ToString(), out  Value) == false)
                            {
                                ErrorCounter++;
                                Imported = false;
                                sbMensaje.Append("Registro número : ");
                                sbMensaje.Append(worksheet1.Rows[i].Cells[0].Value);
                                sbMensaje.Append(". El campo ID Tipo Documento es inválido");
                                sbMensaje.Append("\n");
                                i++;
                                continue;
                            }
                            else
                            {
                                Imported = true;
                                TempPacient.i_DocTypeId = int.Parse(worksheet1.Rows[i].Cells[4].Value.ToString());
                            }
                             //Nombre Tipo Documento
                             if (worksheet1.Rows[i].Cells[5].Value != null)
                             {
                                 TempPacient.v_DocTypeName = worksheet1.Rows[i].Cells[5].Value.ToString();
                                 Imported = true;
                             }
                             else
                             {
                                 ErrorCounter++;
                                 Imported = false;
                                 sbMensaje.Append("Registro número : ");
                                 sbMensaje.Append(worksheet1.Rows[i].Cells[0].Value);
                                 sbMensaje.Append(". El campo Nombre Tipo Documento es inválido");
                                 sbMensaje.Append("\n");
                                 i++;
                                 continue;
                             }
                            //Número Documento
                             if (worksheet1.Rows[i].Cells[6].Value != null)
                             {

                                 if (worksheet1.Rows[i].Cells[4].Value.ToString() == "1") // DNI
                                 {
                                     if (worksheet1.Rows[i].Cells[6].Value.ToString().Length != 8)
                                     {
                                         ErrorCounter++;
                                         Imported = false;
                                         sbMensaje.Append("Registro número : ");
                                         sbMensaje.Append(worksheet1.Rows[i].Cells[0].Value);
                                         sbMensaje.Append(". El campo Número de DNI debe tener 8 dígitos");
                                         sbMensaje.Append("\n");
                                         i++;
                                         continue;
                                     }
                                     else
                                     {
                                         Imported = true;
                                         TempPacient.v_DocNumber = worksheet1.Rows[i].Cells[6].Value.ToString();
                                     }

                                 }
                                 else if (worksheet1.Rows[i].Cells[4].Value.ToString() == "2") // PASAPORTE
                                 {
                                     if (worksheet1.Rows[i].Cells[6].Value.ToString().Length != 9)
                                     {
                                         ErrorCounter++;
                                         Imported = false;
                                         sbMensaje.Append("Registro número : ");
                                         sbMensaje.Append(worksheet1.Rows[i].Cells[0].Value);
                                         sbMensaje.Append(". El Número PASAPORTE debe tener 9 dígitos");
                                         sbMensaje.Append("\n");
                                         i++;
                                         continue;
                                     }
                                     else
                                     {
                                         Imported = true;
                                         TempPacient.v_DocNumber = worksheet1.Rows[i].Cells[6].Value.ToString();
                                     }
                                 }
                                 else if (worksheet1.Rows[i].Cells[4].Value.ToString() == "3") // LICENCIA DE CONDUCIR
                                 {
                                     if (worksheet1.Rows[i].Cells[6].Value.ToString().Length != 10)
                                     {
                                         ErrorCounter++;
                                         Imported = false;
                                         sbMensaje.Append("Registro número : ");
                                         sbMensaje.Append(worksheet1.Rows[i].Cells[0].Value);
                                         sbMensaje.Append(". El Número LICENCIA DE CONDUCIR debe tener 10 dígitos");
                                         sbMensaje.Append("\n");
                                         i++;
                                         continue;
                                     }
                                     else
                                     {
                                         Imported = true;
                                         TempPacient.v_DocNumber = worksheet1.Rows[i].Cells[6].Value.ToString();
                                     }
                                 }
                                 else if (worksheet1.Rows[i].Cells[4].Value.ToString() == "4")// CARNET DE EXTRANJERIA
                                 {
                                     if (worksheet1.Rows[i].Cells[6].Value.ToString().Length != 11)
                                     {
                                         ErrorCounter++;
                                         Imported = false;
                                         sbMensaje.Append("Registro número : ");
                                         sbMensaje.Append(worksheet1.Rows[i].Cells[0].Value);
                                         sbMensaje.Append(". El Número CARNET DE EXTRANJERIA debe tener 11 dígitos");
                                         sbMensaje.Append("\n");
                                         i++;
                                         continue;
                                     }
                                     else
                                     {
                                         Imported = true;
                                         TempPacient.v_DocNumber = worksheet1.Rows[i].Cells[6].Value.ToString();
                                     }
                                 }                             
                             }
                             else
                             {
                                 ErrorCounter++;
                                 Imported = false;
                                 sbMensaje.Append("Registro número : ");
                                 sbMensaje.Append(worksheet1.Rows[i].Cells[0].Value);
                                 sbMensaje.Append(". El campo Número Documento es inválido");
                                 sbMensaje.Append("\n");
                                 i++;
                                 continue;
                             }
                            //ID Género
                             if (int.TryParse(worksheet1.Rows[i].Cells[7].Value.ToString(), out  Value))
                             {
                                 Imported = true;
                                 TempPacient.i_SexTypeId = int.Parse(worksheet1.Rows[i].Cells[7].Value.ToString());
                             }
                             else
                             {
                                 ErrorCounter++;
                                 Imported = false;
                                 sbMensaje.Append("Registro número : ");
                                 sbMensaje.Append(worksheet1.Rows[i].Cells[0].Value);
                                 sbMensaje.Append(". El campo ID Género es inválido");
                                 sbMensaje.Append("\n");
                                 i++;
                                 continue;
                             }
                            // Nombre Género
                             if (worksheet1.Rows[i].Cells[8].Value != null)
                             {
                                 Imported = true;
                                 TempPacient.v_SexTypeName = worksheet1.Rows[i].Cells[8].Value.ToString();
                             }
                             else
                             {
                                 ErrorCounter++;
                                 Imported = false;
                                 sbMensaje.Append("Registro número : ");
                                 sbMensaje.Append(worksheet1.Rows[i].Cells[0].Value);
                                 sbMensaje.Append(". El campo Nombre Género es inválido");
                                 sbMensaje.Append("\n");
                                 i++;
                                 continue;
                             }
                            //Fecha Nacimiento
                             if (worksheet1.Rows[i].Cells[9].Value == null)
                             {
                                 ErrorCounter++;
                                 Imported = false;
                                 sbMensaje.Append("Registro número : ");
                                 sbMensaje.Append(worksheet1.Rows[i].Cells[0].Value);
                                 sbMensaje.Append(". El campo Fecha Nacimiento es inválido");
                                 sbMensaje.Append("\n");
                                 i++;
                                 continue;
                             }
                             if (DateTime.TryParseExact(worksheet1.Rows[i].Cells[9].Value.ToString(),"yyyyMMdd",CultureInfo.InvariantCulture, DateTimeStyles.None,out  ValueDateTime) == false)
                             {
                                 ErrorCounter++;
                                 Imported = false;
                                 sbMensaje.Append("Registro número : ");
                                 sbMensaje.Append(worksheet1.Rows[i].Cells[0].Value);
                                 sbMensaje.Append(". El campo Fecha Nacimiento es inválido");
                                 sbMensaje.Append("\n");
                                 i++;
                                 continue;
                             }
                             else
                             {
                                 Imported = true;
                                 TempPacient.d_Birthdate = DateTime.ParseExact(worksheet1.Rows[i].Cells[9].Value.ToString(),"yyyyMMdd",CultureInfo.InvariantCulture);
                             }

                            //ProtocoloId
                             if (worksheet1.Rows[i].Cells[10].Value != null)
                             {
                                 TempPacient.v_ProtocoloId = worksheet1.Rows[i].Cells[10].Value.ToString();
                                 Imported = true;
                             }
                             else
                             {
                                 TempPacient.v_ProtocoloId = string.Empty;
                             }

                           _TempPacientList.Add(TempPacient);                      

                            var Result = _TempPacientList.FindAll(p => p.v_DocNumber == TempPacient.v_DocNumber && p.i_DocTypeId == TempPacient.i_DocTypeId);
                            if (Result .Count > 1)
                            {
                                MessageBox.Show("El correlativo "  + Result[0].i_Correlative + " tiene el mismo Número Documento que el correlativo " + Result[1].i_Correlative + " .Revise el Excel y corriga la duplicidad", "Error al cargar Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;                            
                            }
                            i++;

                        }
                   
                        lblRecordCountPacients.Text = string.Format("Se encontraron {0} registros.", _TempPacientList.Count());

                        if (ErrorCounter >0)
                        {
                            _TempPacientList = new List<PacientList>();
                            grdDataPeople.DataSource = new List<PacientList>();
                            MessageBox.Show(sbMensaje.ToString(), "Registros no importados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            sbMensaje = null;
                        }
                        else if (ErrorCounter == 0 )
                        {
                            grdDataPeople.DataSource = _TempPacientList;
                            MessageBox.Show("Se importaron " + _TempPacientList.Count() + " registros.", "Importación correcta", MessageBoxButtons.OK, MessageBoxIcon.Information);                        
                        }
                        else
                        {
                            _TempPacientList = new List<PacientList>();
                            grdDataPeople.DataSource = new List<PacientList>();
                            MessageBox.Show(sbMensaje.ToString(), "Registros no importados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            sbMensaje = null;
                        }    
                    }
                    else
                    {
                        grdDataPeople.DataSource = new List<PacientList>();
                        MessageBox.Show("Seleccione un formato correcto (.xlsx)", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information);           
                        
                    }
                //}
                //catch (Exception )
                //{
                //    MessageBox.Show("El archivo está en uso. Por favor cierra el documento.", "INFORMACIÓN", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
            
                //}

                btnschedule.Enabled = (grdDataPeople.Rows.Count > 0);
               
            }
        }

        private void btnschedule_Click(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            CalendarBL objCalendarBL = new CalendarBL();
            calendarDto objCalendarDto = new calendarDto();
            PacientBL objPacientBL = new PacientBL();
            pacientDto objPacientDto = new pacientDto();
           PacientList PacientList = new PacientList();
           BlackListBL objBlackListBL = new BlackListBL();

           if (dtpDateTimeCalendar.Value < DateTime.Now.Date)
           {
               MessageBox.Show("No se permite agendar con una fecha anterior a la actual.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               return;
           }

            string CalendarId;
            string PacientId = "";
          
             StringBuilder sbDatos = new StringBuilder();

            if (uvschedule.Validate(true, false).IsValid)
            {
                    foreach (var item in _TempPacientList)
                    {
                        personDto objPersonDto = new personDto();
                        objPersonDto = objPacientBL.GetPersonByNroDocument(ref objOperationResult, item.v_DocNumber);
                        if (objPersonDto != null)
                        {
                            objPersonDto.v_FirstName = item.v_FirstName.Trim();
                            objPersonDto.v_FirstLastName = item.v_FirstLastName.Trim();
                            objPersonDto.v_SecondLastName = item.v_SecondLastName.Trim();
                            objPersonDto.i_DocTypeId = item.i_DocTypeId;
                            objPersonDto.v_DocNumber = item.v_DocNumber;
                            objPersonDto.i_SexTypeId = item.i_SexTypeId;
                            objPersonDto.d_Birthdate = item.d_Birthdate;
                            objPersonDto.i_LevelOfId = -1;
                            objPersonDto.i_MaritalStatusId = -1;

                            objPacientBL.UpdatePacient(ref objOperationResult, objPersonDto, Globals.ClientSession.GetAsList(), objPersonDto.v_DocNumber, "");
                            PacientId = objPersonDto.v_PersonId;
                        }
                        else
                        {
                            objPersonDto = new personDto();
                            objPersonDto.v_FirstName = item.v_FirstName.Trim();
                            objPersonDto.v_FirstLastName = item.v_FirstLastName.Trim();
                            objPersonDto.v_SecondLastName = item.v_SecondLastName.Trim();
                            objPersonDto.i_DocTypeId = item.i_DocTypeId;
                            objPersonDto.v_DocNumber = item.v_DocNumber;
                            objPersonDto.i_SexTypeId = item.i_SexTypeId;
                            objPersonDto.d_Birthdate = item.d_Birthdate;
                            objPersonDto.i_LevelOfId = -1;
                            objPersonDto.i_MaritalStatusId = -1;

                            PacientId = objPacientBL.AddPacient(ref objOperationResult, objPersonDto, Globals.ClientSession.GetAsList());
                        }

                        var Verificar = objBlackListBL.GetBlackList(ref objOperationResult, objPersonDto.v_PersonId);

                                               
                        if (PacientId != null && Verificar == null)  // Se grabo el paciente y se lo agenda
                        {
                            objCalendarDto.v_PersonId = PacientId;
                            objCalendarDto.d_DateTimeCalendar = dtpDateTimeCalendar.Value;
                            objCalendarDto.i_ServiceTypeId = Int32.Parse(ddlServiceTypeId.SelectedValue.ToString());
                            objCalendarDto.i_CalendarStatusId = Int32.Parse(ddlCalendarStatusId.SelectedValue.ToString());
                            objCalendarDto.i_ServiceId = Int32.Parse(ddlMasterServiceId.SelectedValue.ToString());
                            //objCalendarDto.v_ProtocolId = _ProtocolId;
                            objCalendarDto.v_ProtocolId = item.v_ProtocoloId;
                            objCalendarDto.i_NewContinuationId = Int32.Parse(ddlNewContinuationId.SelectedValue.ToString());
                            objCalendarDto.i_LineStatusId = Int32.Parse(ddlLineStatusId.SelectedValue.ToString());
                            objCalendarDto.i_IsVipId = Int32.Parse(ddlVipId.SelectedValue.ToString());

                            CalendarId = objCalendarBL.AddShedule(ref objOperationResult, objCalendarDto, Globals.ClientSession.GetAsList(), objCalendarDto.v_ProtocolId, PacientId, Int32.Parse(ddlMasterServiceId.SelectedValue.ToString()), "Nuevo", "","");

                        }
                        else  // no se grabro el paciente
                        {
                            sbDatos.Append("PACIENTE :  ");
                            sbDatos.Append(objPersonDto.v_FirstName + " " + objPersonDto.v_FirstLastName + " " + objPersonDto.v_SecondLastName );
                            sbDatos.Append("  DOCUMENTO :  ");
                            sbDatos.Append(objPersonDto.v_DocNumber);
                            sbDatos.Append("\n");
                        }
                    }
                    if (objOperationResult.Success == 1)  // Operación sin error
                    {
                        if (sbDatos != null && sbDatos.Length > 0)
                        {
                            MessageBox.Show(sbDatos.ToString(), "Estos pacientes no fueron agendados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Agenda creada correctamente.", "INFORMACIÓN!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                           
                        }
                      
                    }
                    else// Operación con error
                    {
                        if (objOperationResult.ErrorMessage != null)
                        {
                            MessageBox.Show(objOperationResult.ErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show(sbDatos.ToString(), "Estos pacientes no fueron agendados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            // Se queda en el formulario.
                        }
                        
                    }
            }
        }

        private void ddlServiceTypeId_TextChanged(object sender, EventArgs e)
        {
            OperationResult objOperationResult = new OperationResult();
            Utils.LoadDropDownList(ddlMasterServiceId, "Value1", "Id", BLL.Utils.GetMasterService(ref objOperationResult, int.Parse(ddlServiceTypeId.SelectedValue.ToString()), Globals.ClientSession.i_CurrentExecutionNodeId), DropDownListAction.Select);
            if ((string)ddlServiceTypeId.SelectedValue == "1")
            {
            }
            else
            {
                btnSearchProtocol.Enabled = false;
                ddlOrganization.Enabled = false;
                txtProtocolId.Text = "";
                txtViewProtocol.Text = "";
                txtViewLocation.Text = "";
                txtViewComponentType.Text = "";
                txtViewOccupation.Text = "";
                txtViewGes.Text = "";
                txtViewGroupOccupation.Text = "";
                txtViewOrganization.Text = "";
                txtIntermediaryOrganization.Text = "";
                txtViewIntermediaryOrganization.Text = "";
                ddlOrganization.SelectedValue = "-1";
            }           
        }

        private void ddlMasterServiceId_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((string)ddlMasterServiceId.SelectedValue == "2")
            {
                ddlOrganization.Enabled = false;
                btnSearchProtocol.Enabled = true;

            }
            else if ((string)ddlMasterServiceId.SelectedValue == null || (string)ddlMasterServiceId.SelectedValue == "-1")
            {

            }
            else
            {
                ddlOrganization.Enabled = true;
                txtProtocolId.Text = "";
                txtViewProtocol.Text = "";
                txtViewLocation.Text = "";
                txtViewComponentType.Text = "";
                txtViewOccupation.Text = "";
                txtViewGes.Text = "";
                txtViewGroupOccupation.Text = "";
                txtViewOrganization.Text = "";
                txtIntermediaryOrganization.Text = "";
                txtViewIntermediaryOrganization.Text = "";

            }
        }

        private void btnSearchProtocol_Click(object sender, EventArgs e)
        {
            Configuration.frmProtocolManagement frm = new Configuration.frmProtocolManagement("View", int.Parse(ddlServiceTypeId.SelectedValue.ToString()), int.Parse(ddlMasterServiceId.SelectedValue.ToString()),"");
            frm.ShowDialog();
            _ProtocolId = frm._pstrProtocolId;
            if (_ProtocolId == null) return;
            OperationResult objOperationResult = new OperationResult();
            ProtocolBL _objProtocoltBL = new ProtocolBL();
            protocolDto objProtocolDto = new protocolDto();
            ProtocolList objProtocol = new ProtocolList();
            objProtocol = _objProtocoltBL.GetProtocolById(ref objOperationResult, _ProtocolId);

            txtProtocolId.Text = objProtocol.v_ProtocolId;
            txtViewProtocol.Text = objProtocol.v_Protocol;
            txtViewOrganization.Text = objProtocol.v_Organization;
            txtViewLocation.Text = objProtocol.v_Location;
            txtViewGroupOccupation.Text = objProtocol.v_GroupOccupation;
            txtViewGes.Text = objProtocol.v_Ges;
            txtViewComponentType.Text = objProtocol.v_EsoType;
            txtViewOccupation.Text = objProtocol.v_Occupation;
            txtViewIntermediaryOrganization.Text = objProtocol.v_IntermediaryOrganization;
            txtIntermediaryOrganization.Text = objProtocol.v_OrganizationInvoice;
        }

        private void mnugrdDataPeopleDelete_Click(object sender, EventArgs e)
        {
            _TempPacientList.RemoveAt(_IndexgrdDataPeople);
        }

        private void grdDataPeople_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                System.Drawing.Point point = new System.Drawing.Point(e.X, e.Y);
                Infragistics.Win.UIElement uiElement = ((Infragistics.Win.UltraWinGrid.UltraGridBase)sender).DisplayLayout.UIElement.ElementFromPoint(point);

                if (uiElement == null || uiElement.Parent == null)
                    return;

                // Capturar valor de una celda especifica al hace click derecho sobre la celda k se quiere su valor
                Infragistics.Win.UltraWinGrid.UltraGridCell cell = (Infragistics.Win.UltraWinGrid.UltraGridCell)uiElement.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridCell));
                Infragistics.Win.UltraWinGrid.UltraGridRow row = (Infragistics.Win.UltraWinGrid.UltraGridRow)uiElement.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridRow));

                if (row != null)
                {
                    grdDataPeople.Rows[row.Index].Selected = true;       
                    _IndexgrdDataPeople = row.Index;
                }
            }
        }

        //David Interface con Laboris System SL
        private void btnInterfaceAgendado_Click(object sender, EventArgs e)
        {
            var _calendarInterfaceLS = new CalendarInterface().GetSystemUserAuthorization(7);
            OperationResult objOperationResult = new OperationResult();
            CalendarBL objCalendarBL = new CalendarBL();
            calendarDto objCalendarDto = new calendarDto();
            PacientBL objPacientBL = new PacientBL();
            pacientDto objPacientDto = new pacientDto();
            PacientList PacientList = new PacientList();
            BlackListBL objBlackListBL = new BlackListBL();
            string CalendarId;
            string PacientId = string.Empty;
            StringBuilder sbMensaje = new StringBuilder();
            StringBuilder sbDatos = new StringBuilder();

            //Obtener solo los protocolos
            var protocols = _calendarInterfaceLS.GroupBy(g => g.v_ProtocolId).Select(s => s.First()).ToList();

            foreach (var prot in protocols)
            {
                // obtener pacientes agrupados por protocolo
                var pacientByProtocol = _calendarInterfaceLS.FindAll(p => p.v_ProtocolId == prot.v_ProtocolId);

                // Grabar al paciente
                foreach (var item in pacientByProtocol)
                {
                    personDto objPersonDto = new personDto();
                    objPersonDto = objPacientBL.GetPersonByNroDocument(ref objOperationResult, item.v_NumeroDocumento);

                    if (objPersonDto != null)
                    {
                        objPersonDto.v_FirstName = item.v_Nombres.Trim();
                        objPersonDto.v_FirstLastName = item.v_ApellidoPaterno.Trim();
                        objPersonDto.v_SecondLastName = item.v_ApellidoMaterno.Trim();
                        objPersonDto.i_DocTypeId = item.i_IDTipoDocumento;
                        objPersonDto.v_DocNumber = item.v_NumeroDocumento;
                        objPersonDto.i_SexTypeId = Convert.ToInt32(item.i_IDGenero);
                        objPersonDto.d_Birthdate = DateTime.ParseExact(item.v_FechaNacimiento, "yyyyMMdd", CultureInfo.InvariantCulture);
                        objPersonDto.i_LevelOfId = -1;
                        objPersonDto.i_MaritalStatusId = -1;

                        objPacientBL.UpdatePacient(ref objOperationResult, objPersonDto, Globals.ClientSession.GetAsList(), objPersonDto.v_DocNumber, "");
                        PacientId = objPersonDto.v_PersonId;
                    }
                    else
                    {
                        objPersonDto = new personDto();
                        objPersonDto.v_FirstName = item.v_Nombres.Trim();
                        objPersonDto.v_FirstLastName = item.v_ApellidoPaterno.Trim();
                        objPersonDto.v_SecondLastName = item.v_ApellidoMaterno.Trim();
                        objPersonDto.i_DocTypeId = item.i_IDTipoDocumento;
                        objPersonDto.v_DocNumber = item.v_NumeroDocumento;
                        objPersonDto.i_SexTypeId = Convert.ToInt32(item.i_IDGenero);
                        objPersonDto.d_Birthdate = DateTime.ParseExact(item.v_FechaNacimiento, "yyyyMMdd", CultureInfo.InvariantCulture);
                        objPersonDto.i_LevelOfId = -1;
                        objPersonDto.i_MaritalStatusId = -1;

                        PacientId = objPacientBL.AddPacient(ref objOperationResult, objPersonDto, Globals.ClientSession.GetAsList());
                    }

                    if (PacientId != null || PacientId != string.Empty)  // Se grabo el paciente y se lo agenda
                    {
                       
                        objCalendarDto.v_PersonId = PacientId;
                        objCalendarDto.d_DateTimeCalendar = dtpDateTimeCalendar.Value;
                        objCalendarDto.i_ServiceTypeId = 1;
                        objCalendarDto.i_CalendarStatusId = 1;
                        objCalendarDto.i_ServiceId = 2;
                        objCalendarDto.v_ProtocolId = prot.v_ProtocolId;
                        objCalendarDto.i_NewContinuationId = 1;
                        objCalendarDto.i_LineStatusId = 2;
                        objCalendarDto.i_IsVipId = 0; // No

                        CalendarId = objCalendarBL.AddShedule(ref objOperationResult, objCalendarDto, Globals.ClientSession.GetAsList(), prot.v_ProtocolId, PacientId, objCalendarDto.i_ServiceId.Value, "Nuevo","","");                      
                    }
                    else  // no se grabro el paciente
                    {
                        sbDatos.Append("PACIENTE :  ");
                        sbDatos.Append(objPersonDto.v_FirstName + " " + objPersonDto.v_FirstLastName + " " + objPersonDto.v_SecondLastName);
                        sbDatos.Append("  DOCUMENTO :  ");
                        sbDatos.Append(objPersonDto.v_DocNumber);
                        sbDatos.Append("\n");
                    }
                }

            }

          

            if (objOperationResult.Success == 1)  // Operación sin error
            {
                if (sbDatos != null && sbDatos.Length > 0)
                {
                    MessageBox.Show(sbDatos.ToString(), "Estos pacientes no fueron agendados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Agenda creada correctamente.", "INFORMACIÓN!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;

                }

            }
            else// Operación con error
            {
                if (objOperationResult.ErrorMessage != null)
                {
                    MessageBox.Show(objOperationResult.ErrorMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(sbDatos.ToString(), "Estos pacientes no fueron agendados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // Se queda en el formulario.
                }

            }

          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
