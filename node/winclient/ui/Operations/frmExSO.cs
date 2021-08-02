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
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win;

namespace Sigesoft.Node.WinClient.UI
{
    public partial class frmExSO : Form
    {
        HistoryBL _objHistoryBL = new HistoryBL();
        List<PersonMedicalHistoryList> _TempPersonMedicalHistoryList = new List<PersonMedicalHistoryList>();
        List<PersonMedicalHistoryList> _TempPersonMedicalHistoryListOld = new List<PersonMedicalHistoryList>();  

        string _PacientId;


        public frmExSO()
        {
            InitializeComponent();
        }

        private void frmExSO_Load(object sender, EventArgs e)
        {

        }

        private void CargarTabs()
        {
            #region Antecedentes

            //Resumen de Antecendentes


            #region Ocupaciones
            
            #endregion
            #region MédicosPersonales

            #endregion
            #region Hábitos/Hobbies

            #endregion
            #region MédicosFamiliares

            #endregion

            #endregion

            #region Anamnesis


            #endregion

            #region Exámenes


            #endregion

            #region Análisis de Dx


            #endregion

            #region Conclusiones


            #endregion
        }

        private List<HistoryList> GetData(int pintPageIndex, int? pintPageSize, string pstrSortExpression, string pstrFilterExpression)
        {
            OperationResult objOperationResult = new OperationResult();
            var _objData = _objHistoryBL.GetHistoryPagedAndFiltered(ref objOperationResult, pintPageIndex, pintPageSize, pstrSortExpression, pstrFilterExpression, _PacientId);

            if (objOperationResult.Success != 1)
            {
                MessageBox.Show("Error en operación:" + System.Environment.NewLine + objOperationResult.ExceptionMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return _objData;
        }

        private void BindGridOccupational()
        {
            var objData = GetData(0, null, "d_EndDate DESC", null);

            ugAntecedentesOcupacionales.DataSource = objData;
            var Contador = string.Format("Se encontraron {0} registros.", objData.Count());
        }

        private List<PersonMedicalHistoryList> GetDataPersonMedical(int pintPageIndex, int? pintPageSize, string pstrSortExpression, string pstrFilterExpression)
        {
            OperationResult objOperationResult = new OperationResult();
            var _objData = _objHistoryBL.GetPersonMedicalHistoryPagedAndFilteredByPersonId(ref objOperationResult, pintPageIndex, pintPageSize, pstrSortExpression, pstrFilterExpression, _PacientId);

            if (objOperationResult.Success != 1)
            {
                MessageBox.Show("Error en operación:" + System.Environment.NewLine + objOperationResult.ExceptionMessage, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return _objData;
        }

        private void BindGridPersonMedical()
        {
            //List<PersonMedicalHistoryList> objData = GetDataPersonMedical(0, null, "d_StartDate DESC", null);
            _TempPersonMedicalHistoryList = GetDataPersonMedical(0, null, "d_StartDate DESC", null);
            _TempPersonMedicalHistoryListOld = GetDataPersonMedical(0, null, "d_StartDate DESC", null);

            var objData = GetDataPersonMedical(0, null, "d_StartDate DESC", null);
            grdDataPersonMedical.DataSource = objData;

            var Contador = string.Format("Se encontraron {0} registros.", objData.Count());
        }

        private void tabAntecedentes_Selecting(object sender, TabControlCancelEventArgs e)
        {

        }


    }
}
