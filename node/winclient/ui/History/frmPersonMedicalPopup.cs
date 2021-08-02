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

namespace Sigesoft.Node.WinClient.UI.History
{
    public partial class frmPersonMedicalPopup : Form
    {
        public int _TypeDiagnosticId;
        public string _TypeDiagnosticName;
        public DateTime? _StartDate = null;
        public string _DiagnosticDetail;
        public DateTime _Date;
        public string _TreatmentSite;

        public string PersonId { get; set; }
        public string DiseasesId { get; set; }
        public string Invoked { get; set; }
        public bool ExistPersonMedicalHistory { get; set; }

        public frmPersonMedicalPopup(string DiagnosticName, int TypeDiagnosticId, DateTime StartDate, string DiagnosticDetail, DateTime? Date, string TreatmentSite)
        {
            InitializeComponent();

            OperationResult objOperationResult = new OperationResult();
            Utils.LoadDropDownList(ddlTypeDiagnosticId, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 139, null), DropDownListAction.Select);
        
            this.Text = this.Text + DiagnosticName;
            ddlTypeDiagnosticId.SelectedValue = TypeDiagnosticId.ToString();
            dtpDateTimeStar.Value = StartDate;
            txtDxDetail.Text = DiagnosticDetail;
            txtTreatmentSite.Text = TreatmentSite;
        }

        public frmPersonMedicalPopup(string DiagnosticName, int TypeDiagnosticId, DateTime StartDate, string DiagnosticDetail, DateTime? Date, string TreatmentSite, string personId, string diseasesId, string invoked)
        {
            InitializeComponent();

            OperationResult objOperationResult = new OperationResult();
            Utils.LoadDropDownList(ddlTypeDiagnosticId, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 139, null), DropDownListAction.Select);

            this.Text = this.Text + DiagnosticName;
            ddlTypeDiagnosticId.SelectedValue = TypeDiagnosticId.ToString();
            dtpDateTimeStar.Value = StartDate;
            txtDxDetail.Text = DiagnosticDetail;
            txtTreatmentSite.Text = TreatmentSite;
            
            PersonId = personId;
            DiseasesId = diseasesId;
            Invoked = invoked;

             // si el form invocador es el ESO
            if (Invoked == "frmEso")
            {
                //Verificar si existe el antecedente medico personal
                var found = new ServiceBL().GetPersonMedicalHistoryForESO(PersonId, DiseasesId);

                if (found != null)
                {
                    ddlTypeDiagnosticId.SelectedValue = found.i_TypeDiagnosticId.ToString();
                    if (found.d_StartDate != null)
                        dtpDateTimeStar.Value = found.d_StartDate.Value;
                    txtDxDetail.Text = found.v_DiagnosticDetail;
                    txtTreatmentSite.Text = found.v_TreatmentSite;

                    ExistPersonMedicalHistory = true;
                }
                else
                {
                    ExistPersonMedicalHistory = false;
                }
            }

        }

        private void frmPersonMedicalPopup_Load(object sender, EventArgs e)
        {
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (uvPersonMedicalPopup.Validate(true, false).IsValid)
            {
                _TypeDiagnosticId = int.Parse(ddlTypeDiagnosticId.SelectedValue.ToString());
                _TypeDiagnosticName = ddlTypeDiagnosticId.Text;
                _StartDate = dtpDateTimeStar.Value.Date;
                _DiagnosticDetail = txtDxDetail.Text;
                _TreatmentSite = txtTreatmentSite.Text;

                // si el form invocador es el ESO
                if (Invoked == "frmEso")
                {
                    OperationResult objOperationResult = new OperationResult();
                  
                    var personmedicalhistoryDto = new List<personmedicalhistoryDto>();

                    personmedicalhistoryDto personmedicalhistoryDtoDto = new personmedicalhistoryDto();

                    personmedicalhistoryDtoDto.i_TypeDiagnosticId = _TypeDiagnosticId;

                    personmedicalhistoryDtoDto.v_PersonId = PersonId;
                    personmedicalhistoryDtoDto.v_DiseasesId = DiseasesId;
                    personmedicalhistoryDtoDto.d_StartDate = _StartDate;
                    personmedicalhistoryDtoDto.v_DiagnosticDetail = _DiagnosticDetail;
                    personmedicalhistoryDtoDto.v_TreatmentSite = _TreatmentSite;

                    if (ExistPersonMedicalHistory) // Actualizar
                    {                       
                        personmedicalhistoryDto.Add(personmedicalhistoryDtoDto);

                        new HistoryBL().UpdatePersonMedicalHistoryFromESO(ref objOperationResult,
                                                    personmedicalhistoryDto,
                                                    Globals.ClientSession.GetAsList());
                    }
                    else     // Nuevo
                    {                        
                        personmedicalhistoryDtoDto.i_AnswerId = (int)SiNoNoDefine.Si;
                        personmedicalhistoryDto.Add(personmedicalhistoryDtoDto);

                        new HistoryBL().AddPersonMedicalHistoryFromESO(ref objOperationResult,
                                                    personmedicalhistoryDto,
                                                    Globals.ClientSession.GetAsList());
                    }

                    
                }

                DialogResult = System.Windows.Forms.DialogResult.OK;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
