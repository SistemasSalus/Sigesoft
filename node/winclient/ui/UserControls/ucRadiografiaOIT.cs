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
    public partial class ucRadiografiaOIT : UserControl
    {
       
        bool _isChangeValueControl = false;
        string _oldValue = String.Empty;
        ServiceComponentFieldValuesList _radiografiaOIT = null;
        List<ServiceComponentFieldValuesList> _listRadiografiaOIT = new List<ServiceComponentFieldValuesList>();


        public ucRadiografiaOIT()
        {
            InitializeComponent();

            #region Llenado de combos
            OperationResult objOperationResult = new OperationResult();
            Utils.LoadDropDownList(cb1, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 165, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cb2, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 111, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbPrimario, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 169, null), DropDownListAction.Select);

            Utils.LoadDropDownList(cbSecundario, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 266, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbMedia, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 169, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbInferior, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 169, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbProfusion, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 267, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbOpacGrandes, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 169, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cb3, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 111, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbLocalizPerfil1, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 268, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbLocalizFrente1, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 268, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbLocalizDiafragma, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 268, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbLocalizOtros, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 268, null), DropDownListAction.Select);

            Utils.LoadDropDownList(cbCalciPerfil1, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 268, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbCalciFrente1, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 268, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbCalciDiafragma1, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 268, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbCalciOtros1, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 268, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbExtenDerecha1, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 240, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbExtenIzquierda1, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 240, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbAnchuDerecha1, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 170, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbAnchuIzquierda1, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 170, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbObliteraDerecha, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 169, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbObliteraIzquierda, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 169, null), DropDownListAction.Select);

            Utils.LoadDropDownList(cbLocalizPerfil2, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 268, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbLocalizFrente2, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 268, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbCalciPerfil2, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 268, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbCalciFrente2, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 268, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbExtenDerecha2, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 240, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbCalciFrente2, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 240, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbExtenIzquierda2, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 240, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbAnchuDerecha2, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 240, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cbAnchuIzquierda2, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 240, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cb4, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 111, null), DropDownListAction.Select);
           
            
            
            
            #endregion
        }

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

        #region "----------------------- Custom Events -----------------------"

        private void Controls_Leave(object sender, System.EventArgs e)
        {
            Control senderCtrl = (Control)sender;

            if (!_isChangeValueControl)
            {
                var currentValue = GetValueControl(senderCtrl);

                if (_oldValue != currentValue)
                    _isChangeValueControl = true;
            }
        }

        private void Capture_OldValue(object sender, EventArgs e)
        {
            Control senderCtrl = (Control)sender;
            // Capturar valor inicial
            _oldValue = GetValueControl(senderCtrl);

        }

        private void Controls_ValueChanged(object sender, EventArgs e)
        {
            var senderCtrl = (Control)sender;        
            var value = GetValueControl(senderCtrl);
            SaveValueControlForInterfacingESO(senderCtrl.Name, value);
               
        }

        #endregion       

        #region "--------------- Properties --------------------"

        public List<ServiceComponentFieldValuesList> DataSource
        {
            get
            {
                return _listRadiografiaOIT;
            }
            set
            {
                if (value != _listRadiografiaOIT)
                {
                    ClearValueControl();
                    _listRadiografiaOIT = value;
                    SearchControlAndFill(value);
                }
            }
        }

        public void ClearValueControl()
        {
            _isChangeValueControl = false;

        }

        public bool IsChangeValueControl
        {
            get
            {
                return _isChangeValueControl;
            }
            set
            {
                _isChangeValueControl = value;
            }
        }

        #endregion

        public int CaliH = 145, PareH = 169, PleuH = 305, HallaH = 112, Comen = 77;

        private void ucRadiografiaOIT_Load(object sender, EventArgs e)
        {
            /* Tamaños completos de GroupBoxes
             * gboxCalidad 815x145 - 815x60
             * gboxParenquima 815x169 - 815x60
             * gboxPleural 815x305 - 815x60
             * gboxHallazgo 815x112 - 815x60
             * gboxComentarios 815x77 - 815x77
             */

            groupBoxCalidad.Width = 815; groupBoxCalidad.Height = 60;
            groupBoxCalidad.Location = new Point(8, 87);

            groupBoxParenquima.Width = 815; groupBoxParenquima.Height = 60;
            //groupBoxParenquima.Location = new Point(8, 238);
            groupBoxParenquima.Location = new Point(8, 87 + 6 + 60);

            groupBoxPleural.Width = 815; groupBoxPleural.Height = 60;
            //groupBoxPleural.Location = new Point(8, 413);
            groupBoxPleural.Location = new Point(8, 87 + 6 + 60 + 6 + 60);

            groupBoxHallazgos.Width = 815; groupBoxHallazgos.Height = 60;
            //groupBoxHallazgos.Location = new Point(8, 724);
            groupBoxHallazgos.Location = new Point(8, 87 + 6 + 60 + 6 + 60 + 6 + 60);

            groupBoxComentarios.Width = 815; groupBoxComentarios.Height = 77;
            //groupBoxComentarios.Location = new Point(8, 842);
            
            groupBoxComentarios.Location = new Point(8, 87 + 6 + 60 + 6 + 60 + 6 + 60 + 6 + 60);

            //ACA CARGAR TODOS LOS COMBOS
           // Utils.LoadDropDownList(ddlVipId, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, 111, null), DropDownListAction.All);


            SetControlName();
            SearchControlAndSetEvents(this);
        }

        private void cb1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cb1.SelectedValue == null) return;
         
            if (cb1.SelectedValue.ToString() == ((int) SiNo.SI).ToString())
            {
                CaliH = 60;

                groupBoxCalidad.Width = 815; groupBoxCalidad.Height = CaliH;
                groupBoxCalidad.Location = new Point(8, 87);

                groupBoxParenquima.Width = 815; groupBoxParenquima.Height = 60;
                //groupBoxParenquima.Location = new Point(8, 238);
                groupBoxParenquima.Location = new Point(8, 87 + 6 + CaliH);

                groupBoxPleural.Width = 815; groupBoxPleural.Height = 60;
                //groupBoxPleural.Location = new Point(8, 413);
                groupBoxPleural.Location = new Point(8, 87 + 6 + CaliH + 6 + 60);

                groupBoxHallazgos.Width = 815; groupBoxHallazgos.Height = 60;
                //groupBoxHallazgos.Location = new Point(8, 724);
                groupBoxHallazgos.Location = new Point(8, 87 + 6 + CaliH + 6 + 60 + 6 + 60);

                groupBoxComentarios.Width = 815; groupBoxComentarios.Height = 77;
                //groupBoxComentarios.Location = new Point(8, 842);
                groupBoxComentarios.Location = new Point(8, 87 + 6 + CaliH + 6 + 60 + 6 + 60 + 6 + 60);
            }
            else
            {
                CaliH = 145;

                groupBoxCalidad.Width = 815; groupBoxCalidad.Height = CaliH;
                groupBoxCalidad.Location = new Point(8, 87);

                groupBoxParenquima.Width = 815; groupBoxParenquima.Height = 60;
                //groupBoxParenquima.Location = new Point(8, 238);
                groupBoxParenquima.Location = new Point(8, 87 + 6 + CaliH);

                groupBoxPleural.Width = 815; groupBoxPleural.Height = 60;
                //groupBoxPleural.Location = new Point(8, 413);
                groupBoxPleural.Location = new Point(8, 87 + 6 + CaliH + 6 + 60);

                groupBoxHallazgos.Width = 815; groupBoxHallazgos.Height = 60;
                //groupBoxHallazgos.Location = new Point(8, 724);
                groupBoxHallazgos.Location = new Point(8, 87 + 6 + CaliH + 6 + 60 + 6 + 60);

                groupBoxComentarios.Width = 815; groupBoxComentarios.Height = 77;
                //groupBoxComentarios.Location = new Point(8, 842);
                groupBoxComentarios.Location = new Point(8, 87 + 6 + CaliH + 6 + 60 + 6 + 60 + 6 + 60);
            }
        }

        private void cb2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cb2.SelectedValue == null) return;
            if (cb2.SelectedValue.ToString() == ((int)SiNo.SI).ToString())
            {
                PareH = 60;

                groupBoxCalidad.Width = 815; groupBoxCalidad.Height = CaliH;
                groupBoxCalidad.Location = new Point(8, 87);

                groupBoxParenquima.Width = 815; groupBoxParenquima.Height = PareH;
                //groupBoxParenquima.Location = new Point(8, 238);
                groupBoxParenquima.Location = new Point(8, 87 + 6 + CaliH);

                groupBoxPleural.Width = 815; groupBoxPleural.Height = 60;
                //groupBoxPleural.Location = new Point(8, 413);
                groupBoxPleural.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH);

                groupBoxHallazgos.Width = 815; groupBoxHallazgos.Height = 60;
                //groupBoxHallazgos.Location = new Point(8, 724);
                groupBoxHallazgos.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + 60);

                groupBoxComentarios.Width = 815; groupBoxComentarios.Height = 77;
                //groupBoxComentarios.Location = new Point(8, 842);
                groupBoxComentarios.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + 60 + 6 + 60);
            }
            else
            {
                PareH = 169;

                groupBoxCalidad.Width = 815; groupBoxCalidad.Height = CaliH;
                groupBoxCalidad.Location = new Point(8, 87);

                groupBoxParenquima.Width = 815; groupBoxParenquima.Height = PareH;
                //groupBoxParenquima.Location = new Point(8, 238);
                groupBoxParenquima.Location = new Point(8, 87 + 6 + CaliH);

                groupBoxPleural.Width = 815; groupBoxPleural.Height = 60;
                //groupBoxPleural.Location = new Point(8, 413);
                groupBoxPleural.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH);

                groupBoxHallazgos.Width = 815; groupBoxHallazgos.Height = 60;
                //groupBoxHallazgos.Location = new Point(8, 724);
                groupBoxHallazgos.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + 60);

                groupBoxComentarios.Width = 815; groupBoxComentarios.Height = 77;
                //groupBoxComentarios.Location = new Point(8, 842);
                groupBoxComentarios.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + 60 + 6 + 60);
            }

        }

        private void cb3_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cb3.SelectedValue == null) return;
            if (cb3.SelectedValue.ToString() == ((int)SiNo.SI).ToString())
            {
                PleuH = 60;

                groupBoxCalidad.Width = 815; groupBoxCalidad.Height = CaliH;
                groupBoxCalidad.Location = new Point(8, 87);

                groupBoxParenquima.Width = 815; groupBoxParenquima.Height = PareH;
                //groupBoxParenquima.Location = new Point(8, 238);
                groupBoxParenquima.Location = new Point(8, 87 + 6 + CaliH);

                groupBoxPleural.Width = 815; groupBoxPleural.Height = PleuH;
                //groupBoxPleural.Location = new Point(8, 413);
                groupBoxPleural.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH);

                groupBoxHallazgos.Width = 815; groupBoxHallazgos.Height = 60;
                //groupBoxHallazgos.Location = new Point(8, 724);
                groupBoxHallazgos.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + PleuH);

                groupBoxComentarios.Width = 815; groupBoxComentarios.Height = 77;
                //groupBoxComentarios.Location = new Point(8, 842);
                groupBoxComentarios.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + PleuH + 6 + 60);
            }
            else
            {
                PleuH = 305;

                groupBoxCalidad.Width = 815; groupBoxCalidad.Height = CaliH;
                groupBoxCalidad.Location = new Point(8, 87);

                groupBoxParenquima.Width = 815; groupBoxParenquima.Height = PareH;
                //groupBoxParenquima.Location = new Point(8, 238);
                groupBoxParenquima.Location = new Point(8, 87 + 6 + CaliH);

                groupBoxPleural.Width = 815; groupBoxPleural.Height = PleuH;
                //groupBoxPleural.Location = new Point(8, 413);
                groupBoxPleural.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH);

                groupBoxHallazgos.Width = 815; groupBoxHallazgos.Height = 60;
                //groupBoxHallazgos.Location = new Point(8, 724);
                groupBoxHallazgos.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + PleuH);

                groupBoxComentarios.Width = 815; groupBoxComentarios.Height = 77;
                //groupBoxComentarios.Location = new Point(8, 842);
                groupBoxComentarios.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + PleuH + 6 + 60);
            }
        }

        private void cb4_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cb4.SelectedValue == null) return;
            if (cb4.SelectedValue.ToString() == ((int)SiNo.SI).ToString())
            {
                HallaH = 60;

                groupBoxCalidad.Width = 815; groupBoxCalidad.Height = CaliH;
                groupBoxCalidad.Location = new Point(8, 87);

                groupBoxParenquima.Width = 815; groupBoxParenquima.Height = PareH;
                //groupBoxParenquima.Location = new Point(8, 238);
                groupBoxParenquima.Location = new Point(8, 87 + 6 + CaliH);

                groupBoxPleural.Width = 815; groupBoxPleural.Height = PleuH;
                //groupBoxPleural.Location = new Point(8, 413);
                groupBoxPleural.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH);

                groupBoxHallazgos.Width = 815; groupBoxHallazgos.Height = HallaH;
                //groupBoxHallazgos.Location = new Point(8, 724);
                groupBoxHallazgos.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + PleuH);

                groupBoxComentarios.Width = 815; groupBoxComentarios.Height = 77;
                //groupBoxComentarios.Location = new Point(8, 842);
                groupBoxComentarios.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + PleuH + 6 + HallaH);
            }
            else
            {
                HallaH = 112;

                groupBoxCalidad.Width = 815; groupBoxCalidad.Height = CaliH;
                groupBoxCalidad.Location = new Point(8, 87);

                groupBoxParenquima.Width = 815; groupBoxParenquima.Height = PareH;
                //groupBoxParenquima.Location = new Point(8, 238);
                groupBoxParenquima.Location = new Point(8, 87 + 6 + CaliH);

                groupBoxPleural.Width = 815; groupBoxPleural.Height = PleuH;
                //groupBoxPleural.Location = new Point(8, 413);
                groupBoxPleural.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH);

                groupBoxHallazgos.Width = 815; groupBoxHallazgos.Height = HallaH;
                //groupBoxHallazgos.Location = new Point(8, 724);
                groupBoxHallazgos.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + PleuH);

                groupBoxComentarios.Width = 815; groupBoxComentarios.Height = 77;
                //groupBoxComentarios.Location = new Point(8, 842);
                groupBoxComentarios.Location = new Point(8, 87 + 6 + CaliH + 6 + PareH + 6 + PleuH + 6 + HallaH);
            }
        }

        private string GetValueControl(Control ctrl)
        {
            string value = string.Empty;

            if (ctrl is TextBox)
            {
                value = ctrl.Text;
            }
            else if (ctrl is Label)
            {
                value = ctrl.Text;
            }
            else if (ctrl is ComboBox)
            {
                value = ((ComboBox)ctrl).SelectedValue == null ? null : ((ComboBox)ctrl).SelectedValue.ToString();
            }
            else if (ctrl is CheckBox)
            {
                value = Convert.ToInt32(((CheckBox)ctrl).Checked).ToString();
            }

            return value;
        }

        private void SetControlName()
        {
            txtPlaca.Name = Constants.txt_Placa;
            txtHCL.Name = Constants.txt_HCL;
            txtLector.Name = Constants.txt_Lector;
            dtpFLectura.Name = Constants.dtp_FLectura;
            dtpFRadiografia.Name =  Constants.dtp_FRadiografia;
            cb1.Name =  Constants.cb_Calidad_Radiografica;
            chkSobreexposicion.Name = Constants.chk_Sobreexposicion;
            chkSubexposicion.Name = Constants.chk_Subexposicion;
            chkPosicionCentrado.Name = Constants.chk_PosicionCentrado;
            chkInspiracion.Name = Constants.chk_Inspiracion;
            chkEscapula.Name =  Constants.chk_Escapula;

            chkArtefacto.Name = Constants.chk_Artefacto;
            chkOtros.Name =  Constants.chk_Otros;
            txtComentario1.Name = Constants.txt_Comentario1;
            cb2.Name =  Constants.cb_Hay_anormalidades_Parenquimatosas;
            cbPrimario.Name = Constants.cb_Primario;
            cbSuperior.Name = Constants.cb_Superior;
            cbProfusion.Name = Constants.cb_Profusion;
            cbOpacGrandes.Name = Constants.cb_OpacGrandes;
            cbSecundario.Name =  Constants.cb_Secundario;
            cbMedia.Name = Constants.cb_Media;
            cbInferior.Name = Constants.cb_Inferior;

            cb3.Name = Constants.cb_Existe_anomalia_Pleural;
            cbLocalizPerfil1.Name = Constants.cb_LocalizPerfil1;
            cbCalciPerfil1.Name =  Constants.cb_CalciPerfil1;
            cbLocalizFrente1.Name = Constants.cb_LocalizFrente1;
            cbCalciFrente1.Name =  Constants.cb_CalciFrente1;
            cbLocalizDiafragma.Name = Constants.cb_LocalizDiafragma;
            cbCalciDiafragma1.Name = Constants.cb_CalciDiafragma1;
            cbExtenDerecha1.Name = Constants.cb_ExtenDerecha1;
            cbAnchuDerecha1.Name = Constants.cb_AnchuDerecha1;
            cbObliteraDerecha.Name = Constants.cb_ObliteraDerecha;
            cbLocalizOtros.Name = Constants.cb_LocalizOtros;
            cbCalciOtros1.Name = Constants.cbCalciOtros1;
            cbExtenIzquierda1.Name = Constants.cb_ExtenIzquierda1;
            cbAnchuIzquierda1.Name = Constants.cb_AnchuIzquierda1;
            cbObliteraIzquierda.Name = Constants.cb_ObliteraIzquierda;

            cbLocalizPerfil2.Name = Constants.cb_LocalizPerfil2;
            cbCalciPerfil2.Name = Constants.cb_CalciPerfil2;
            cbExtenDerecha2.Name = Constants.cb_ExtenDerecha2;
            cbAnchuDerecha2.Name = Constants.cb_AnchuDerecha2;

            cbLocalizFrente2.Name = Constants.cb_LocalizFrente2;
            cbCalciFrente2.Name = Constants.cb_CalciFrente2;
            cbExtenIzquierda2.Name = Constants.cb_ExtenIzquierda2;
            cbAnchuIzquierda2.Name = Constants.cb_AnchuIzquierda2;
            cb4.Name = Constants.cb_Otros_Hallazgos_radio_anormales;

            txtHallazgos.Name = Constants.txt_Hallazgos;
            txtComentarioTotal.Name = Constants.txt_ComentarioTotal;

        }

        private void SearchControlAndSetEvents(Control ctrlContainer)
        {
            foreach (Control ctrl in ctrlContainer.Controls)
            {
                if (ctrl is TextBox)
                {
                    var field = (TextBox)ctrl;
                    var tag = field.Tag;
                    
                    ctrl.Enter += new EventHandler(Capture_OldValue);                 
                    ((TextBox)ctrl).Leave += new EventHandler(Controls_Leave);
                    ctrl.TextChanged += new EventHandler(Controls_ValueChanged);
                }
                else if (ctrl is ComboBox)
                {
                    var cb = (ComboBox)ctrl;
                    cb.Click += new EventHandler(Capture_OldValue);
                    cb.Leave += new EventHandler(Controls_Leave);
                    cb.SelectedValueChanged += new EventHandler(Controls_ValueChanged);
                }
                else if (ctrl is CheckBox)
                {
                    var chk = (CheckBox)ctrl;
                    chk.Enter += new EventHandler(Capture_OldValue);
                    chk.Leave += new EventHandler(Controls_Leave);

                }

                if (ctrl.HasChildren)
                    SearchControlAndSetEvents(ctrl);
            }

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
                    else if (field is Label)
                    {
                        if (field.Name == item.v_ComponentFieldId)
                        {
                            ((Label)field).Text = item.v_Value1;
                        }
                    }
                    else if (field is ComboBox)
                    {
                        if (field.Name == item.v_ComponentFieldId)
                        {                         
                            ((ComboBox)field).SelectedValue = item.v_Value1;
                        }
                    }
                    else if (field is CheckBox)
                    {
                        if (field.Name == item.v_ComponentFieldId)
                        {
                            ((CheckBox)field).Checked = Convert.ToBoolean(int.Parse(item.v_Value1));
                        }
                    }
                }

            }
        }

        private void SaveValueControlForInterfacingESO(string name, string value)
        {
            #region Capturar Valor del campo

            _listRadiografiaOIT.RemoveAll(p => p.v_ComponentFieldId == name);

            _radiografiaOIT = new ServiceComponentFieldValuesList();

            _radiografiaOIT.v_ComponentFieldId = name;
            _radiografiaOIT.v_Value1 = value;
            _radiografiaOIT.v_ComponentId = Constants.OIT_ID;
            _listRadiografiaOIT.Add(_radiografiaOIT);

            DataSource = _listRadiografiaOIT;

            #endregion
        }

        private void cb1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbPrimario_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBoxCalidad_Enter(object sender, EventArgs e)
        {

        }

    }
}
