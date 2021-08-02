using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sigesoft.Common;
using Sigesoft.Node.WinClient.BE;
using Sigesoft.Node.WinClient.BLL;

namespace Sigesoft.Node.WinClient.UI.UserControls
{
    public partial class ucOftalmologia : UserControl
    {
        List<RecomendationList> _recomendations = null;
        List<RestrictionList> _restrictions = null;
        List<DiagnosticRepositoryList> _dx = null;

        bool _isChangueValueControl = false;

        ServiceComponentFieldValuesList _oftalmologia = null;
        List<ServiceComponentFieldValuesList> _listOftalmologia = new List<ServiceComponentFieldValuesList>();

        #region "------------- Public Events -------------"

        /// <summary>
        /// Se desencadena cada vez que se cambia un valor del examen de Audiometria.
        /// </summary>
        public event EventHandler<AudiometriaAfterValueChangeEventArgs> AfterValueChange;
        protected void OnAfterValueChange(AudiometriaAfterValueChangeEventArgs e)
        {
            if (AfterValueChange != null)
                AfterValueChange(this, e);
        }

        #endregion

        #region "--------------- Properties --------------------"

        public List<ServiceComponentFieldValuesList> DataSource
        {
            get
            {
                return _listOftalmologia;
            }
            set
            {
                if (value != _listOftalmologia)
                {
                    ClearValueControl();
                    _listOftalmologia = value;
                    SearchControlAndFill(value);
                }
            }
        }

        public void ClearValueControl()
        {
            _isChangueValueControl = false;
            cbDLSC.SelectedValue = "-1";
            cbDLCC.SelectedValue = "-1";
            cbILSC.SelectedValue = "-1";
            cbILCC.SelectedValue = "-1";
        }

        public bool IsChangeValueControl { get { return _isChangueValueControl; } }

        public int Age { get; set; }

        #endregion
    
        public ucOftalmologia()
        {
            InitializeComponent();
        }

        private void ucOftalmologia_Load(object sender, EventArgs e)
        {

            cbDLSC.Name = Constants.TXT_OFT_SC_OD;
            cbDLCC.Name = Constants.TXT_OFT_CC_OD;
            cbILSC.Name = Constants.TXT_OFT_SC_OI;
            cbILCC.Name = Constants.TXT_OFT_CC_OI;
            cbDLAE.Name = Constants.TXT_OFT_AE_OD;
            cbILAE.Name = Constants.TXT_OFT_AE_OI;

            cbDCSC.Name = Constants.TXT_OFT_SC_ODC;
            cbDCCC.Name = Constants.TXT_OFT_CC_ODC;
            cbDCCC2.Name = Constants.TXT_OFT_CC_ODC2;
            cbICSC.Name = Constants.TXT_OFT_SC_OIC;
            cbICCC.Name = Constants.TXT_OFT_CC_OIC;
            cbICCC2.Name = Constants.TXT_OFT_CC_OIC2;

            #region Cargar Combos

            LoadDxAndRecoAndRes();

            OperationResult objOperationResult = new OperationResult();
            Utils.LoadDropDownList(cbDLSC, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.OftalmologiaMedidas, "i_ParameterId"), DropDownListAction.Select);//237
            Utils.LoadDropDownList(cbDLCC, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.OftalmologiaMedidas, "i_ParameterId"), DropDownListAction.Select);
            Utils.LoadDropDownList(cbILSC, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.OftalmologiaMedidas, "i_ParameterId"), DropDownListAction.Select);
            Utils.LoadDropDownList(cbILCC, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.OftalmologiaMedidas, "i_ParameterId"), DropDownListAction.Select);
            Utils.LoadDropDownList(cbDLAE, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.OftalmologiaMedidas, "i_ParameterId"), DropDownListAction.Select);
            Utils.LoadDropDownList(cbILAE, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.OftalmologiaMedidas, "i_ParameterId"), DropDownListAction.Select);
            Utils.LoadDropDownList(cbDCSC, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.OftalmologiaMedidasCerca, "i_ParameterId"), DropDownListAction.Select);//262
            Utils.LoadDropDownList(cbDCCC, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.OftalmologiaMedidasCerca, "i_ParameterId"), DropDownListAction.Select);
            Utils.LoadDropDownList(cbDCCC2, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.OftalmologiaMedidasCerca, "i_ParameterId"), DropDownListAction.Select);
            Utils.LoadDropDownList(cbICSC, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.OftalmologiaMedidasCerca, "i_ParameterId"), DropDownListAction.Select);
            Utils.LoadDropDownList(cbICCC, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.OftalmologiaMedidasCerca, "i_ParameterId"), DropDownListAction.Select);
            Utils.LoadDropDownList(cbICCC2, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.OftalmologiaMedidasCerca, "i_ParameterId"), DropDownListAction.Select);

            #endregion

            txtEdad.Text = Age.ToString();
            chkFiguras.Name = Constants.CHK_OFT_FIGURAS;
        }

        private void cbDSC_SelectedValueChanged(object sender, EventArgs e)
        {
            GenerarDx();

            if (cbDLSC.SelectedValue != null)              
                SaveValueControlForInterfacingESO(cbDLSC.Name, cbDLSC.SelectedValue.ToString());
        }

        private void cbDCC_SelectedValueChanged(object sender, EventArgs e)
        {
            GenerarDx();
            if (cbDLCC.SelectedValue != null)
                SaveValueControlForInterfacingESO(cbDLCC.Name, cbDLCC.SelectedValue.ToString());
        }

        private void cbISC_SelectedValueChanged(object sender, EventArgs e)
        {
            GenerarDx();
            if (cbILSC.SelectedValue != null)
                SaveValueControlForInterfacingESO(cbILSC.Name, cbILSC.SelectedValue.ToString());
        }

        private void cbICC_SelectedValueChanged(object sender, EventArgs e)
        {
            GenerarDx();
            if (cbILCC.SelectedValue != null)
                SaveValueControlForInterfacingESO(cbILCC.Name, cbILCC.SelectedValue.ToString());
        }

        private void cbDLAE_SelectedValueChanged(object sender, EventArgs e)
        {
            GenerarDx();
            if (cbDLAE.SelectedValue != null)
                SaveValueControlForInterfacingESO(cbDLAE.Name, cbDLAE.SelectedValue.ToString());
        }

        private void cbILAE_SelectedValueChanged(object sender, EventArgs e)
        {
            GenerarDx();
            if (cbILAE.SelectedValue != null)
                SaveValueControlForInterfacingESO(cbILAE.Name, cbILAE.SelectedValue.ToString());
        }

        private void cbDCSC_SelectedValueChanged(object sender, EventArgs e)
        {
            GenerarDx();
            if (cbDCSC.SelectedValue != null)
                SaveValueControlForInterfacingESO(cbDCSC.Name, cbDCSC.SelectedValue.ToString());
        }

        private void cbDCCC_SelectedValueChanged(object sender, EventArgs e)
        {
            GenerarDx();
            if (cbDCCC.SelectedValue != null)
                SaveValueControlForInterfacingESO(cbDCCC.Name, cbDCCC.SelectedValue.ToString());
        }

        private void cbICSC_SelectedValueChanged(object sender, EventArgs e)
        {
            GenerarDx();
            if (cbICSC.SelectedValue != null)
                SaveValueControlForInterfacingESO(cbICSC.Name, cbICSC.SelectedValue.ToString());
        }

        private void cbICCC_SelectedValueChanged(object sender, EventArgs e)
        {
            GenerarDx();
            if (cbICCC.SelectedValue != null)
                SaveValueControlForInterfacingESO(cbICCC.Name, cbICCC.SelectedValue.ToString());
        }

        private void cbDCCC2_SelectedValueChanged(object sender, EventArgs e)
        {
            GenerarDx();
            if (cbDCCC2.SelectedValue != null)
                SaveValueControlForInterfacingESO(cbDCCC2.Name, cbDCCC2.SelectedValue.ToString());
        }

        private void cbICCC2_SelectedValueChanged(object sender, EventArgs e)
        {
            GenerarDx();
            if (cbICCC2.SelectedValue != null)
                SaveValueControlForInterfacingESO(cbICCC2.Name, cbICCC2.SelectedValue.ToString());
        }

        private void txtEdad_TextChanged(object sender, EventArgs e)
        {
            GenerarDx();
        }

        private void chkFiguras_CheckedChanged(object sender, EventArgs e)
        {
            SaveValueControlForInterfacingESO(chkFiguras.Name, Convert.ToInt32(chkFiguras.Checked).ToString());
        }

        void GenerarDx()
        {
            int DLSC=-1,DLCC=-1,ILSC=-1,ILCC=-1,DLAE=-1,ILAE=-1,DCSC=-1,DCCC=-1,DCCC2=-1,ICSC=-1,ICCC=-1,ICCC2=-1,edad=0;

            if (cbDLSC.SelectedValue == null 
                && cbDLCC.SelectedValue == null 
                && cbILSC.SelectedValue == null
                && cbILCC.SelectedValue == null
                && cbDCSC.SelectedValue == null
                && cbDCCC.SelectedValue == null
                && cbDCCC2.SelectedValue == null
                && cbICSC.SelectedValue == null
                && cbICCC.SelectedValue == null
                && cbICCC2.SelectedValue == null)
            {
                return;
            }

            DLSC = Convert.ToInt32(cbDLSC.SelectedValue);
            DLCC = Convert.ToInt32(cbDLCC.SelectedValue);
            ILSC = Convert.ToInt32(cbILSC.SelectedValue);
            ILCC = Convert.ToInt32(cbILCC.SelectedValue);
            DLAE = Convert.ToInt32(cbDLAE.SelectedValue);
            ILAE = Convert.ToInt32(cbILAE.SelectedValue);
            DCSC = Convert.ToInt32(cbDCSC.SelectedValue);
            DCCC = Convert.ToInt32(cbDCCC.SelectedValue);
            DCCC2 = Convert.ToInt32(cbDCCC2.SelectedValue);
            ICSC = Convert.ToInt32(cbICSC.SelectedValue);
            ICCC = Convert.ToInt32(cbICCC.SelectedValue);
            ICCC2 = Convert.ToInt32(cbICCC2.SelectedValue);

            string strDiag_DL = "", strDiag_IL = "", strDiag_DC = "", strDiag_IC = "", strDiag_X = "",  strDiag_P = "", strDiag_P2 = "", strAMEPRE = "";
            int DiagDL = 0, DiagIL = 0, DiagDC = 0, DiagIC = 0, DiagX = 0, DiagP = 0, DiagP2 = 0;

            //Nueva Lógica 01-12-2015 David
            //edad = Convert.ToInt32(txtEdad.Text);
            edad = Age;
            if (edad > 40) { strAMEPRE = "Presbicia"; } else { strAMEPRE = "Ametropía"; }
            // Sin datos en los L y C
            if (DLSC == -1 && ILSC == -1 && DCSC == -1 && ICSC == -1 && DLCC == -1 && ILCC == -1 && DCCC == -1 && ICCC == -1)
            { txtDiag.Text = "No hay Datos para Procesar"; }
            else
            {
                
                //Completo X 03-12-2015
                #region X
                #region DL
                if (DLSC != -1)
                {
                    if (DLSC == (int)VisionLejos._20_20)
                    { strDiag_DL = "OD: Emétrope"; DiagDL = 10; }
                    else
                    {
                        if (DLSC < (int)VisionLejos._20_40)
                        {
                            if (DLCC != -1)
                            {
                                if (DLCC < (int)VisionLejos._20_40)
                                { strDiag_DL = "OD: Ametropía Leve Corregida"; DiagDL = 20; }
                                else
                                {
                                    if (DLAE != -1)
                                    {
                                        if (DLAE < DLCC)
                                        { strDiag_DL = "OD: Ametropía Leve Corregida - Actualizar Refracción"; DiagDL = 30; }
                                        else
                                        {
                                            if (DLAE == DLCC)
                                            { strDiag_DL = "OD: Ametropía Leve Corregida"; DiagDL = 20; }
                                            else
                                            { strDiag_DL = "OD: Ametropía Leve Parcialmente Corregida: Completar Ev. Oftalmológica con Pupila Dilatada"; DiagDL = 40; }
                                        }
                                    }
                                    else
                                    { strDiag_DL = "OD: Completar Agujero Estenopeico"; DiagDL = 0; }

                                }
                            }
                            else
                            { strDiag_DL = "OD: Ametropía Leve No Corregida"; DiagDL = 50; }
                        }
                        else
                        {
                            if (DLCC != -1)
                            {
                                if (DLCC < (int)VisionLejos._20_40)
                                { strDiag_DL = "OD: Ametropía Corregida"; DiagDL = 60; }
                                else
                                {
                                    if (DLAE != -1)
                                    {
                                        if (DLAE < DLCC)
                                        { strDiag_DL = "OD: Ametropía Parcialmente Corregida - Actualizar Refracción"; DiagDL = 70; }
                                        else
                                        {
                                            if (DLAE == DLCC)
                                            { strDiag_DL = "OD: Ametropía Corregida"; DiagDL = 60; }
                                            else
                                            { strDiag_DL = "OD: Ametropía Parcialmente Corregida: Completar Ev. Oftalmológica con Pupila Dilatada"; DiagDL = 90; }
                                        }
                                    }
                                    else
                                    { strDiag_DL = "OD: Completar Agujero Estenopeico"; DiagDL = 0; }
                                }
                            }
                            else
                            { strDiag_DL = "OD: Ametropía No Corregida"; DiagDL = 100; }
                        }
                    }
                }
                else
                {
                    if (DLCC != -1)
                    {
                        if (DLCC < (int)VisionLejos._20_40)
                        { strDiag_DL = "OD: Ametropía Corregida"; DiagDL = 60; }
                        else
                        { strDiag_DL = "OD: Ametropía Parcialmente Corregida: Completar Ev. Oftalmológica con Pupila Dilatada, sin Lentes de Contacto"; DiagDL = 90; }
                    }
                    else
                    { strDiag_DL = "OD: No hay Datos"; DiagDL = 0; }
                }
                #endregion
                #region IL
                if (ILSC != -1)
                {
                    if (ILSC == (int)VisionLejos._20_20)
                    { strDiag_IL = "OI: Emétrope"; DiagIL = 10; }
                    else
                    {
                        if (ILSC < (int)VisionLejos._20_40)
                        {
                            if (ILCC != -1)
                            {
                                if (ILCC < (int)VisionLejos._20_40)
                                { strDiag_IL = "OI: Ametropía Leve Corregida"; DiagIL = 20; }
                                else
                                {
                                    if (ILAE != -1)
                                    {
                                        if (ILAE < ILCC)
                                        { strDiag_DL = "OI: Ametropía Leve Corregida - Actualizar Refracción"; DiagIL = 30; }
                                        else
                                        {
                                            if (ILAE == ILCC)
                                            { strDiag_IL = "OI: Ametropía Leve Corregida"; DiagIL = 20; }
                                            else
                                            { strDiag_IL = "OI: Ametropía Leve Parcialmente Corregida: Completar Ev. Oftalmológica con Pupila Dilatada"; DiagIL = 40; }
                                        }
                                    }
                                    else
                                    { strDiag_IL = "OI: Completar Agujero Estenopeico"; DiagIL = 0; }
                                }
                            }
                            else
                            { strDiag_IL = "OI: Ametropía Leve No Corregida"; DiagIL = 50; }
                        }
                        else
                        {
                            if (ILCC != -1)
                            {
                                if (ILCC < (int)VisionLejos._20_40)
                                { strDiag_IL = "OI: Ametropía Corregida"; DiagIL = 60; }
                                else
                                {
                                    if (ILAE != -1)
                                    {
                                        if (ILAE < ILCC)
                                        { strDiag_IL = "OI: Ametropía Parcialmente Corregida - Actualizar Refracción"; DiagIL = 70; }
                                        else
                                        {
                                            if (ILAE == ILCC)
                                            { strDiag_IL = "OI: Ametropía Corregida"; DiagIL = 60; }
                                            else
                                            { strDiag_IL = "OI: Ametropía Parcialmente Corregida: Completar Ev. Oftalmológica con Pupila Dilatada"; DiagIL = 90; }
                                        }
                                    }
                                    else
                                    { strDiag_IL = "OI: Completar Agujero Estenopeico"; DiagIL = 0; }
                                }
                            }
                            else
                            { strDiag_IL = "OI: Ametropía No Corregida"; DiagIL = 100; }
                        }
                    }
                }
                else
                {
                    if (ILCC != -1)
                    {
                        if (ILCC < (int)VisionLejos._20_40)
                        { strDiag_IL = "OI: Ametropía Corregida"; DiagIL = 60; }
                        else
                        { strDiag_IL = "OI: Ametropía Parcialmente Corregida: Completar Ev. Oftalmológica con Pupila Dilatada, sin Lentes de Contacto"; DiagIL = 90; }
                    }
                    else
                    { strDiag_IL = "OI: No hay Datos"; DiagIL = 0; }
                }
                #endregion
                #region DC
                if (DCSC != -1)
                {
                    if (DCSC == (int)VisionCerca._20_20)
                    { strDiag_DC = "OD: Emétrope"; DiagDC = 10; }
                    else
                    {
                        if (DCSC < (int)VisionCerca._20_40)
                        {
                            if (DCCC != -1)
                            {
                                if (DCCC < (int)VisionCerca._20_40)
                                { strDiag_DC = "OD: " + strAMEPRE + " Leve Corregida"; DiagDC = 20; }
                                else
                                { strDiag_DC = "OD: " + strAMEPRE + " Parcialmente Corregida: Completar Ev. Oftalmológica con Pupila Dilatada"; DiagDC = 90; }
                            }
                            else
                            { strDiag_DC = "OD: " + strAMEPRE + " Leve No Corregida"; DiagDC = 50; }
                        }
                        else
                        {
                            if (DCCC != -1)
                            {
                                if (DCCC < (int)VisionCerca._20_40)
                                { strDiag_DC = "OD: " + strAMEPRE + " Corregida"; DiagDC = 60; }
                                else
                                { strDiag_DC = "OD: " + strAMEPRE + " Parcialmente Corregida: Completar Ev. Oftalmológica con Pupila Dilatada"; DiagDC = 90; }
                            }
                            else
                            { strDiag_DC = "OD: " + strAMEPRE + " No Corregida"; DiagDC = 100; }
                        }
                    }
                }
                else
                {
                    if (DCCC != -1)
                    {
                        if (DCCC < (int)VisionCerca._20_40)
                        {
                            if (DCCC2 != -1)
                            {
                                if (DCCC2 < (int)VisionCerca._20_40)
                                { strDiag_DC = "OD: " + strAMEPRE + " Corregida"; DiagDC = 50; }
                                else
                                { strDiag_DC = "OD: " + strAMEPRE + " Parcialmente Corregida: Completar Ev. Oftalmológica con Pupila Dilatada"; DiagDC = 90; }
                            }
                            else
                            { strDiag_DC = "OD: " + strAMEPRE + " No Corregida"; DiagDC = 100; }
                        }
                        else
                        {
                            if (DCCC2 != -1)
                            {
                                if (DCCC2 < (int)VisionCerca._20_40)
                                { strDiag_DC = "OD: " + strAMEPRE + " Corregida"; DiagDC = 50; }
                                else
                                { strDiag_DC = "OD: " + strAMEPRE + " Parcialmente Corregida: Completar Ev. Oftalmológica con Pupila Dilatada"; DiagDC = 90; }
                            }
                            else
                            { strDiag_DC = "OD: " + strAMEPRE + " No Corregida"; DiagDC = 100; }
                        }
                    }
                    else
                    { strDiag_DC = "OD: No hay Datos"; DiagDC = 0; }
                }
                #endregion

                #region IC
                if (ICSC != -1)
                {
                    if (ICSC == (int)VisionCerca._20_20)
                    { strDiag_IC = "OI: Emétrope"; DiagIC = 10; }
                    else
                    {
                        if (ICSC < (int)VisionCerca._20_40)
                        {
                            if (ICCC != -1)
                            {
                                if (ICCC < (int)VisionCerca._20_40)
                                { strDiag_IC = "OI: " + strAMEPRE + " Leve Corregida"; DiagIC = 20; }
                                else
                                { strDiag_IC = "OI: " + strAMEPRE + " Parcialmente Corregida: Completar Ev. Oftalmológica con Pupila Dilatada"; DiagIC = 90; }
                            }
                            else
                            { strDiag_IC = "OI: " + strAMEPRE + " Leve No Corregida"; DiagIC = 50; }
                        }
                        else
                        {
                            if (ICCC != -1)
                            {
                                if (ICCC < (int)VisionCerca._20_40)
                                { strDiag_IC = "OI: " + strAMEPRE + " Corregida"; DiagIC = 60; }
                                else
                                { strDiag_IC = "OI: " + strAMEPRE + " Parcialmente Corregida: Completar Ev. Oftalmológica con Pupila Dilatada"; DiagIC = 90; }
                            }
                            else
                            { strDiag_IC = "OI: " + strAMEPRE + " No Corregida"; DiagIC = 100; }
                        }
                    }
                }
                else
                {
                    if (ICCC != -1)
                    {
                        if (ICCC < (int)VisionCerca._20_40)
                        {
                            if (ICCC2 != -1)
                            {
                                if (ICCC2 < (int)VisionCerca._20_40)
                                { strDiag_IC = "OI: " + strAMEPRE + " Corregida"; DiagDC = 50; }
                                else
                                { strDiag_IC = "OI: " + strAMEPRE + " Parcialmente Corregida: Completar Ev. Oftalmológica con Pupila Dilatada"; DiagDC = 90; }
                            }
                            else
                            { strDiag_IC = "OI: " + strAMEPRE + " No Corregida"; DiagDC = 100; }
                        }
                        else
                        {
                            if (ICCC2 != -1)
                            {
                                if (ICCC2 < (int)VisionCerca._20_40)
                                { strDiag_IC = "OI: " + strAMEPRE + " Corregida"; DiagDC = 50; }
                                else
                                { strDiag_IC = "OI: " + strAMEPRE + " Parcialmente Corregida: Completar Ev. Oftalmológica con Pupila Dilatada"; DiagDC = 90; }
                            }
                            else
                            { strDiag_IC = "OI: " + strAMEPRE + " No Corregida"; DiagDC = 100; }
                        }
                    }
                    else
                    { strDiag_IC = "OI: No hay Datos"; DiagIC = 0; }
                }
                #endregion

                #endregion

                #region Fusion Dx
                if (DiagDL >= DiagIL)
                {
                    strDiag_P = strDiag_DL; DiagP = DiagDL; 
                    if (DiagDL >= DiagDC)
                    {
                        if (DiagDL >= DiagIC)
                        { strDiag_X = strDiag_DL; DiagX = DiagDL; }
                        else
                        { strDiag_X = strDiag_IC; DiagX = DiagIC; }
                    }
                    else
                    {
                        if (DiagDC >= DiagIC)
                        { strDiag_X = strDiag_DC; DiagX = DiagDC; }
                        else
                        { strDiag_X = strDiag_IC; DiagX = DiagIC; }
                    }
                }
                else
                {
                    strDiag_P = strDiag_IL; DiagP = DiagIL; 
                    if (DiagIL >= DiagDC)
                    {
                        if (DiagIL >= DiagIC)
                        { strDiag_X = strDiag_IL; DiagX = DiagIL; }
                        else
                        { strDiag_X = strDiag_IC; DiagX = DiagIC; }
                    }
                    else
                    {
                        if (DiagDC >= DiagIC)
                        { strDiag_X = strDiag_DC; DiagX = DiagDC; }
                        else
                        { strDiag_X = strDiag_IC; DiagX = DiagIC; }
                    }
                }

                if (DiagDC >= DiagIC)
                { strDiag_P2 = strDiag_DC; DiagP2 = DiagDC; }
                else
                { strDiag_P2 = strDiag_IC; DiagP2 = DiagIC; }

                switch (DiagX)
                {
                    case 10:
                        { strDiag_X = "Emétrope"; }
                        break;
                    case 20:
                        if (edad > 40)
                        {
                            if (DiagP > 10)
                            {
                                if (DiagP2 > 10)
                                { strDiag_X = strDiag_P + "\r\n" + strAMEPRE + " Corregida"; }
                                else
                                { strDiag_X = strDiag_P; }
                            }
                            else
                            { strDiag_X = strAMEPRE + " Corregida"; }
                        }
                        else
                        { strDiag_X = strAMEPRE + " Leve Corregida"; }
                        break;
                    case 50:
                        if (edad > 40)
                        {
                            if (DiagP > 10)
                            {
                                if (DiagP2 > 10)
                                { strDiag_X = strDiag_P + "\r\n" + strAMEPRE + " No Corregida"; }
                                else
                                { strDiag_X = strDiag_P; }
                            }
                            else
                            { strDiag_X = strAMEPRE + " No Corregida"; }
                        }
                        else
                        { strDiag_X = strAMEPRE + " Leve No Corregida"; }
                        break;
                    case 60:
                        if (edad > 40)
                        {
                            if (DiagP > 10)
                            {
                                if (DiagP2 > 10)
                                { strDiag_X = strDiag_P + "\r\n" + strAMEPRE + " Corregida"; }
                                else
                                { strDiag_X = strDiag_P; }
                            }
                            else
                            { strDiag_X = strAMEPRE + " Corregida"; }
                        }
                        else
                        { strDiag_X = strAMEPRE + " Corregida"; }
                        break;
                    case 100:
                        if (edad > 40)
                        { 
                            if (DiagP > 10)
                            {
                                if (DiagP2 > 10)
                                { strDiag_X = strDiag_P + "\r\n" + strAMEPRE + " No Corregida";  }
                                else
                                { strDiag_X = strDiag_P; }
                            }
                            else
                            { strDiag_X = strAMEPRE + " No Corregida"; }
                        }
                        else
                        { strDiag_X = strAMEPRE + " No Corregida"; }
                        break;
                    default:
                        if (edad > 40)
                        { 
                            if (DiagP > 10)
                            {
                                if (DiagP2 > 10)
                                { strDiag_P = strDiag_P + "\r\n" + strDiag_P2; strDiag_X = strDiag_P; }
                                else
                                { strDiag_X = strDiag_P; }
                            }
                            else
                            { strDiag_X = strAMEPRE + " No Corregida"; }
                        }
                        break;
                }
                #endregion

                //Detalle de Donde Sale strDiag_X
                //txtDiag.Text = strDiag_DL + "\r\n" + strDiag_IL + "\r\n" + strDiag_DC + "\r\n" + strDiag_IC + "\r\n" + "\r\n" + strDiag_X;
                txtDiag.Text = strDiag_X;
            }
            
            // capturar dx automatico + recomendaciones y restricciones   

            List<DiagnosticRepositoryList> dxList = null;

            var dx = SearchDxSugeridoOfSystem(txtDiag.Text, Constants.txt_OFT_DX_AUTO);
           
            if (dx != null)
            {
                dxList = new List<DiagnosticRepositoryList>();
            }

            if (dx != null)
            {
                dxList.Add(dx);
            }

            List<string> listCf = new List<string>() { Constants.txt_OFT_DX_AUTO };

            // Disparar evento
            OnAfterValueChange(new AudiometriaAfterValueChangeEventArgs { PackageSynchronization = dxList, ListcomponentFieldsId = listCf });
        }

        // Alejandro
        private DiagnosticRepositoryList SearchDxSugeridoOfSystem(string dx, string componentFieldsId)
        {
            DiagnosticRepositoryList diagnosticRepository = null;
            // Buscar reco / res asociadas a un dx

          
            var diagnostic = GetDxByName(dx);

            if (diagnostic != null)
            {
                // Insertar DX sugerido (automático) a la bolsa de DX 
                diagnosticRepository = new DiagnosticRepositoryList();

                diagnosticRepository.v_DiagnosticRepositoryId = Guid.NewGuid().ToString();
                diagnosticRepository.v_DiseasesId = diagnostic.v_DiseasesId;
                diagnosticRepository.i_AutoManualId = (int)AutoManual.Automático;
                diagnosticRepository.i_PreQualificationId = (int)PreQualification.SinPreCalificar;
                diagnosticRepository.i_FinalQualificationId = (int)FinalQualification.SinCalificar;
                diagnosticRepository.v_ServiceId = "";
                diagnosticRepository.v_ComponentId = Constants.AGUDEZA_VISUAL;
                diagnosticRepository.v_DiseasesName = diagnostic.v_DiseasesName;
                diagnosticRepository.v_AutoManualName = "AUTOMÁTICO";
                diagnosticRepository.v_RestrictionsName = string.Join(", ", diagnostic.Restrictions.Select(p => p.v_RestrictionName));
                diagnosticRepository.v_RecomendationsName = string.Join(", ", diagnostic.Recomendations.Select(p => p.v_RecommendationName));
                diagnosticRepository.v_PreQualificationName = "SIN PRE-CALIFICAR";

                // ID enlace DX automatico para grabar valores dinamicos
                //diagnosticRepository.v_ComponentFieldValuesId = val.v_ComponentFieldValuesId;
                diagnosticRepository.v_ComponentFieldsId = componentFieldsId;
                diagnosticRepository.Recomendations = diagnostic.Recomendations;
                diagnosticRepository.Restrictions = diagnostic.Restrictions;
                diagnosticRepository.i_RecordStatus = (int)RecordStatus.Agregado;
                diagnosticRepository.i_RecordType = (int)RecordType.Temporal;

                //diagnosticRepository.d_ExpirationDateDiagnostic = DateTime.Now.AddMonths(vm);
            }
            else
            {

            }

            return diagnosticRepository;
        }


        // Alejandro
        private void LoadDxAndRecoAndRes()
        {
            _recomendations = new List<RecomendationList>()
            {
                new RecomendationList { v_MasterRecommendationId = "N002-MR000000071", v_DiseasesId = Constants.VISIÓN_MONOCULAR_EMÉTROPE_OD, v_RecommendationName = "Control por Oftalmología" },
                new RecomendationList { v_MasterRecommendationId = "N002-MR000000071", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_LEVE_OD, v_RecommendationName = "Control por Oftalmología" },
                new RecomendationList { v_MasterRecommendationId = "N002-MR000000071", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_NO_CORREGIDA_OD, v_RecommendationName = "Control por Oftalmología" },
                new RecomendationList { v_MasterRecommendationId = "N002-MR000000071", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_CORREGIDA_OD, v_RecommendationName = "Control por Oftalmología" },
                new RecomendationList { v_MasterRecommendationId = "N002-MR000000071", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_PARCIALMENTE_CORREGIDA_OD, v_RecommendationName = "Control por Oftalmología" },

                new RecomendationList { v_MasterRecommendationId = "N002-MR000000071", v_DiseasesId = Constants.VISIÓN_MONOCULAR_EMÉTROPE_OI, v_RecommendationName = "Control por Oftalmología" },
                new RecomendationList { v_MasterRecommendationId = "N002-MR000000071", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_LEVE_OI, v_RecommendationName = "Control por Oftalmología" },
                new RecomendationList { v_MasterRecommendationId = "N002-MR000000071", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_NO_CORREGIDA_OI, v_RecommendationName = "Control por Oftalmología" },
                new RecomendationList { v_MasterRecommendationId = "N002-MR000000071", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_CORREGIDA_OI, v_RecommendationName = "Control por Oftalmología" },
                new RecomendationList { v_MasterRecommendationId = "N002-MR000000071", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_PARCIALMENTE_CORREGIDA_OI, v_RecommendationName = "Control por Oftalmología" },

                new RecomendationList { v_MasterRecommendationId = "N002-MR000000071", v_DiseasesId = Constants.AMETROPÍA_PARCIALMENTE_CORREGIDA, v_RecommendationName = "Control por Oftalmología" },
                new RecomendationList { v_MasterRecommendationId = "N002-MR000000071", v_DiseasesId = Constants.AMETROPÍA_NO_CORREGIDA, v_RecommendationName = "Control por Oftalmología" },
                new RecomendationList { v_MasterRecommendationId = "N002-MR000000071", v_DiseasesId = Constants.AMETROPÍA_LEVE, v_RecommendationName = "Control por Oftalmología" },
            };

            _restrictions = new List<RestrictionList>()
            {             
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000072", v_DiseasesId = Constants.VISIÓN_MONOCULAR_EMÉTROPE_OD, v_RestrictionName = "No conducir vehículos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000019", v_DiseasesId = Constants.VISIÓN_MONOCULAR_EMÉTROPE_OD, v_RestrictionName = "No realizar trabajos en altura mayor a 1.8 mts" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000073", v_DiseasesId = Constants.VISIÓN_MONOCULAR_EMÉTROPE_OD, v_RestrictionName = "No realizar trabajos con cables eléctricos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000072", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_LEVE_OD, v_RestrictionName = "No conducir vehículos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000019", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_LEVE_OD, v_RestrictionName = "No realizar trabajos en altura mayor a 1.8 mts" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000073", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_LEVE_OD, v_RestrictionName = "No realizar trabajos con cables eléctricos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000072", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_NO_CORREGIDA_OD, v_RestrictionName = "No conducir vehículos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000019", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_NO_CORREGIDA_OD, v_RestrictionName = "No realizar trabajos en altura mayor a 1.8 mts" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000073", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_NO_CORREGIDA_OD, v_RestrictionName = "No realizar trabajos con cables eléctricos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000072", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_CORREGIDA_OD, v_RestrictionName = "No conducir vehículos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000019", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_CORREGIDA_OD, v_RestrictionName = "No realizar trabajos en altura mayor a 1.8 mts" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000073", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_CORREGIDA_OD, v_RestrictionName = "No realizar trabajos con cables eléctricos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000072", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_PARCIALMENTE_CORREGIDA_OD, v_RestrictionName = "No conducir vehículos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000019", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_PARCIALMENTE_CORREGIDA_OD, v_RestrictionName = "No realizar trabajos en altura mayor a 1.8 mts" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000073", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_PARCIALMENTE_CORREGIDA_OD, v_RestrictionName = "No realizar trabajos con cables eléctricos" },

                new RestrictionList { v_MasterRestrictionId = "N002-MR000000072", v_DiseasesId = Constants.VISIÓN_MONOCULAR_EMÉTROPE_OI, v_RestrictionName = "No conducir vehículos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000019", v_DiseasesId = Constants.VISIÓN_MONOCULAR_EMÉTROPE_OI, v_RestrictionName = "No realizar trabajos en altura mayor a 1.8 mts" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000073", v_DiseasesId = Constants.VISIÓN_MONOCULAR_EMÉTROPE_OI, v_RestrictionName = "No realizar trabajos con cables eléctricos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000072", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_LEVE_OI, v_RestrictionName = "No conducir vehículos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000019", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_LEVE_OI, v_RestrictionName = "No realizar trabajos en altura mayor a 1.8 mts" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000073", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_LEVE_OI, v_RestrictionName = "No realizar trabajos con cables eléctricos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000072", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_NO_CORREGIDA_OI, v_RestrictionName = "No conducir vehículos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000019", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_NO_CORREGIDA_OI, v_RestrictionName = "No realizar trabajos en altura mayor a 1.8 mts" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000073", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_NO_CORREGIDA_OI, v_RestrictionName = "No realizar trabajos con cables eléctricos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000072", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_CORREGIDA_OI, v_RestrictionName = "No conducir vehículos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000019", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_CORREGIDA_OI, v_RestrictionName = "No realizar trabajos en altura mayor a 1.8 mts" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000073", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_CORREGIDA_OI, v_RestrictionName = "No realizar trabajos con cables eléctricos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000072", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_PARCIALMENTE_CORREGIDA_OI, v_RestrictionName = "No conducir vehículos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000019", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_PARCIALMENTE_CORREGIDA_OI, v_RestrictionName = "No realizar trabajos en altura mayor a 1.8 mts" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000073", v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_PARCIALMENTE_CORREGIDA_OI, v_RestrictionName = "No realizar trabajos con cables eléctricos" },

                new RestrictionList { v_MasterRestrictionId = "N002-MR000000072", v_DiseasesId = Constants.AMETROPÍA_LEVE, v_RestrictionName = "No conducir vehículos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000019", v_DiseasesId = Constants.AMETROPÍA_LEVE, v_RestrictionName = "No realizar trabajos en altura mayor a 1.8 mts" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000073", v_DiseasesId = Constants.AMETROPÍA_LEVE, v_RestrictionName = "No realizar trabajos con cables eléctricos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000072", v_DiseasesId = Constants.AMETROPÍA_NO_CORREGIDA, v_RestrictionName = "No conducir vehículos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000019", v_DiseasesId = Constants.AMETROPÍA_NO_CORREGIDA, v_RestrictionName = "No realizar trabajos en altura mayor a 1.8 mts" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000073", v_DiseasesId = Constants.AMETROPÍA_NO_CORREGIDA, v_RestrictionName = "No realizar trabajos con cables eléctricos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000074", v_DiseasesId = Constants.AMETROPÍA_CORREGIDA, v_RestrictionName = "Uso de correctores visuales al conducir vehículos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000072", v_DiseasesId = Constants.AMETROPÍA_PARCIALMENTE_CORREGIDA, v_RestrictionName = "No conducir vehículos" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000019", v_DiseasesId = Constants.AMETROPÍA_PARCIALMENTE_CORREGIDA, v_RestrictionName = "No realizar trabajos en altura mayor a 1.8 mts" },
                new RestrictionList { v_MasterRestrictionId = "N002-MR000000073", v_DiseasesId = Constants.AMETROPÍA_PARCIALMENTE_CORREGIDA, v_RestrictionName = "No realizar trabajos con cables eléctricos" },
            };

            _dx = new List<DiagnosticRepositoryList>()
            {
                new DiagnosticRepositoryList { v_DiseasesId = Constants.VISIÓN_MONOCULAR_EMÉTROPE_OD, v_DiseasesName = "Visión Monocular OD Emétrope" },
                new DiagnosticRepositoryList { v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_LEVE_OD, v_DiseasesName = "Visión Monocular OD Ametropía Leve" },
                new DiagnosticRepositoryList { v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_NO_CORREGIDA_OD, v_DiseasesName = "Visión Monocular OD Ametropía No Corregida" },
                new DiagnosticRepositoryList { v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_CORREGIDA_OD, v_DiseasesName = "Visión Monocular OD Ametropía Corregida" },
                new DiagnosticRepositoryList { v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_PARCIALMENTE_CORREGIDA_OD, v_DiseasesName = "Visión Monocular OD Ametropía Parcialmente Corregida" },

                new DiagnosticRepositoryList { v_DiseasesId = Constants.VISIÓN_MONOCULAR_EMÉTROPE_OI, v_DiseasesName = "Visión Monocular OI Emétrope" },
                new DiagnosticRepositoryList { v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_LEVE_OI, v_DiseasesName = "Visión Monocular OI Ametropía Leve" },
                new DiagnosticRepositoryList { v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_NO_CORREGIDA_OI, v_DiseasesName = "Visión Monocular OI Ametropía No Corregida" },
                new DiagnosticRepositoryList { v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_CORREGIDA_OI, v_DiseasesName = "Visión Monocular OI Ametropía Corregida" },
                new DiagnosticRepositoryList { v_DiseasesId = Constants.VISIÓN_MONOCULAR_AMETROPÍA_PARCIALMENTE_CORREGIDA_OI, v_DiseasesName = "Visión Monocular OI Ametropía Parcialmente Corregida" },
              
                new DiagnosticRepositoryList { v_DiseasesId = Constants.AMETROPÍA_LEVE, v_DiseasesName = "Ametropía Leve" },
                new DiagnosticRepositoryList { v_DiseasesId = Constants.AMETROPÍA_NO_CORREGIDA, v_DiseasesName = "Ametropía No Corregida" },
                new DiagnosticRepositoryList { v_DiseasesId = Constants.AMETROPÍA_CORREGIDA, v_DiseasesName = "Ametropía Corregida" },
                
                new DiagnosticRepositoryList { v_DiseasesId = Constants.AMETROPÍA_PARCIALMENTE_CORREGIDA, v_DiseasesName = "Ametropía Parcialmente Corregida" },
                //Agregar casos DAVID 20170517
                new DiagnosticRepositoryList { v_DiseasesId = Constants.EMÉTROPE, v_DiseasesName = "Emétrope" }
            };

        }

        // Alejandro
        private DiagnosticRepositoryList GetDxByName(string dx)
        {
            var sql = (from d in _dx
                       join rec in _recomendations on d.v_DiseasesId equals rec.v_DiseasesId into l_rec
                       join rec in _restrictions on d.v_DiseasesId equals rec.v_DiseasesId into l_res
                       where d.v_DiseasesName == dx
                       select new DiagnosticRepositoryList
                       {
                           v_DiseasesId = d.v_DiseasesId,
                           v_DiseasesName = d.v_DiseasesName,
                           Recomendations = l_rec.ToList(),
                           Restrictions = l_res.ToList()
                       }).FirstOrDefault();

            if (sql != null)
            {
                sql.Recomendations.ForEach(p => { p.v_ComponentId = Constants.AGUDEZA_VISUAL; p.i_RecordStatus = (int)RecordStatus.Agregado; p.i_RecordType = (int)RecordType.Temporal; });
                sql.Restrictions.ForEach(p => { p.v_ComponentId = Constants.AGUDEZA_VISUAL; p.i_RecordStatus = (int)RecordStatus.Agregado; p.i_RecordType = (int)RecordType.Temporal; });           
            }
           
            return sql;
        }

        private void SearchControlAndFill(List<ServiceComponentFieldValuesList> DataSource)
        {
            if (DataSource == null || DataSource.Count == 0) return;
            // Ordenar Lista Datasource
            var DataSourceOrdenado = DataSource.OrderBy(p => p.v_ComponentFieldId).ToList();

            // recorrer la lista que viene de la BD
            foreach (var item in DataSourceOrdenado)
            {
                var matchedFields = this.Controls.Find(item.v_ComponentFieldId, true);

                if (matchedFields.Length > 0)
                {
                    var field = matchedFields[0];

                    if (field is TextBox)
                    {
                        if (field.Name == item.v_ComponentFieldId)
                        {
                            ((TextBox)field).Text = item.v_Value1;
                        }
                    }
                    else if (field is ComboBox)
                    {
                        if (field.Name == item.v_ComponentFieldId)
                        {
                            ((ComboBox)field).SelectedValue = item.v_Value1;
                        }
                    }
                }
            }
        }

        private void SaveValueControlForInterfacingESO(string name, string value)
        {
            #region Capturar Valor del campo

            _listOftalmologia.RemoveAll(p => p.v_ComponentFieldId == name);

            _oftalmologia = new ServiceComponentFieldValuesList();

            _oftalmologia.v_ComponentFieldId = name;
            _oftalmologia.v_Value1 = value;
            _oftalmologia.v_ComponentId = Constants.AGUDEZA_VISUAL;

            _listOftalmologia.Add(_oftalmologia);

            DataSource = _listOftalmologia;

            #endregion
        }

    }
}
