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

namespace Sigesoft.Node.WinClient.UI.UserControls
{
    public partial class ucPsychologicalExam : UserControl
    {
        #region "--------------------Declarations -------------------"

        List<PsychologicalExam> _psychologicalExam = null;
        List<PsychologicalExamDetail> _psychologicalExamDetail = null;
        List<PsychologicalInterpretation> _psychologicalInterpretation = null;
        bool _isChangeValueControl = false;
        string _oldValue = String.Empty;
        ServiceComponentFieldValuesList _serviceComponentFieldValues = null;
        List<ServiceComponentFieldValuesList> _serviceComponentFieldValuesList = new List<ServiceComponentFieldValuesList>();
        private string _oldGroupOcupationalId;

        #endregion

        #region "--------------- Properties --------------------"

        public List<ServiceComponentFieldValuesList> DataSource
        {
            get
            {
                return _serviceComponentFieldValuesList;
            }
            set
            {
                _isChangeValueControl = false;

                if (value != _serviceComponentFieldValuesList)
                {
                    cbGrupoOcupacional.TextChanged -= new EventHandler(cbGrupoOcupacional_TextChanged);
                    Common.Utils.ClearContentControlsByCondition(this);
                    cbGrupoOcupacional.TextChanged += new EventHandler(cbGrupoOcupacional_TextChanged);
                    _serviceComponentFieldValuesList = value;
                    SearchControlAndFill(value);
                }
            }
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

        public TypeESO EsoType { get; set; }

        #endregion

        public ucPsychologicalExam()
        {
            InitializeComponent();
        }

        public ucPsychologicalExam(TypeESO esoType)
        {
            EsoType = esoType;
            InitializeComponent();
        }

        #region "----------------------- Public Events ------------------------"

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

        #region "----------------------- Private Events -----------------------"

        private void ucPsychologicalExam_Load(object sender, EventArgs e)
        {
            LoadDataPsychologicalExams();
            SetControlName();
            SearchControlAndSetEvents(this);
            LoadComboGroupOcupationByEsoType();
            LoadCombo_DISC_Combinaciones();
            LoadCombo_Resultados_Evaluacion();
            
        }

        private void cbGrupoOcupacional_TextChanged(object sender, EventArgs e)
        {
            if (cbGrupoOcupacional.SelectedValue == null)
                return;

            // Limpiar todos los controles al desplazarte por el combo de grupo ocupacional
            if (cbGrupoOcupacional.SelectedValue.ToString() != _oldGroupOcupationalId)
            {
                _oldGroupOcupationalId = cbGrupoOcupacional.SelectedValue.ToString();
                Common.Utils.ClearContentControlsByGroupOccupational(this);
                ClearDataToSave();
            }

            EnabledDisabledExam();
            SaveValueControlForInterfacingESO(cbGrupoOcupacional.Name, cbGrupoOcupacional.SelectedValue.ToString());        

        }

        #endregion
    
        #region "----------------------- Custom Events -----------------------"

        private void Controls_Leave(object sender, System.EventArgs e)
        {
            Control senderCtrl = (Control)sender;

            if (!_isChangeValueControl)
            {
                var currentValue =  GetValueControl(senderCtrl);

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
            string value;

            if (senderCtrl.Tag != null && senderCtrl.Tag.ToString() != string.Empty)  // Contiene el Id de la prueba
            {              
                var testId = senderCtrl.Tag.ToString();
                var findResult = GetControlsByPsychologicalExamId(testId);

                if (findResult.HasLogic)
                {
                    Control puntaje = null;
                    TextBox nivel = null;
                    TextBox cat = null;
                    TextBox interpre = null;

                    puntaje = senderCtrl;
                    if (findResult.NivelCtrlId != null)
                        nivel = (TextBox)this.Controls.Find(findResult.NivelCtrlId, true)[0];
                    cat = (TextBox)this.Controls.Find(findResult.CategoriaCtrlId, true)[0];
                    interpre = (TextBox)this.Controls.Find(findResult.InterpretacionCtrlId, true)[0];

                    SetInfo(puntaje, nivel, cat, interpre);
                }
                else   // El control no tiene logica por lo tanto solo se guarda su valor
                {
                    value = GetValueControl(senderCtrl);
                    SaveValueControlForInterfacingESO(senderCtrl.Name, value);
                }

            }
            else  // La mayoria de interpretaciones
            {
                value = GetValueControl(senderCtrl);
                SaveValueControlForInterfacingESO(senderCtrl.Name, value);
            }
       
        }

        private void txt_Puntaje_KeyPress(object sender, KeyPressEventArgs e)
        {
            Common.Utils.OnlyValidateNumbers(e);
        }

        #endregion       

        #region "----------------------- Private Methods -----------------------"

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

        private void SearchControlAndSetEvents(Control ctrlContainer)
        {
            foreach (Control ctrl in ctrlContainer.Controls)
            {
                if (ctrl is TextBox)
                {
                    var field = (TextBox)ctrl;
                    var tag = field.Tag;

                    if (tag != null)
                    {
                        if (tag.ToString() != Constants.WAIS_LABERINTO && 
                            tag.ToString() != Constants.TEST_DIBUJO_HOMBRE_BAJO_LA_LLUVIA)
                        {
                            ctrl.KeyPress += new KeyPressEventHandler(txt_Puntaje_KeyPress);
                        }                                         
                    }

                    ctrl.Enter += new EventHandler(Capture_OldValue);
                    ctrl.TextChanged += new EventHandler(Controls_ValueChanged);
                    ((TextBox)ctrl).Leave += new EventHandler(Controls_Leave);
                }
                else if (ctrl is ComboBox)
                {                 
                    var cb = (ComboBox)ctrl;
                    cb.Click += new EventHandler(Capture_OldValue);
                    cb.Leave += new EventHandler(Controls_Leave);

                    // Este combo tiene su propio evento creado en tiempo de diseño x eso se omite
                    if (cb.Name != Constants.cb_GrupoOcupacional)
                        cb.SelectedValueChanged += new EventHandler(Controls_ValueChanged);
                    
                }
                else if (ctrl is CheckBox)
                {
                    var chk = (CheckBox)ctrl;
                    chk.Enter += new EventHandler(Capture_OldValue);
                    chk.Leave += new EventHandler(Controls_Leave);
                                
                    chk.CheckedChanged += new EventHandler(Controls_ValueChanged);
                    
                }

                if (ctrl.HasChildren)
                    SearchControlAndSetEvents(ctrl);
            }
        }

        private void LoadComboGroupOcupationByEsoType()
        {
            OperationResult objOperationResult = new OperationResult();

            switch (EsoType)
            {
                case TypeESO.PreOcupacional:
                    Utils.LoadDropDownList(cbGrupoOcupacional, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.GrupoOcupacional_EMPO_PSICOLOGIA, null), DropDownListAction.Select);
                    break;
                case TypeESO.PeriodicoAnual:
                    Utils.LoadDropDownList(cbGrupoOcupacional, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.GrupoOcupacional_EMOA_PSICOLOGIA, null), DropDownListAction.Select);
                    break;
                case TypeESO.Retiro:
                    Utils.LoadDropDownList(cbGrupoOcupacional, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.GrupoOcupacional_EMOR_PSICOLOGIA, null), DropDownListAction.Select);
                    break;
                default:
                    break;
            }

        }

        private void LoadCombo_DISC_Combinaciones()
        {
            OperationResult objOperationResult = new OperationResult();
            Utils.LoadDropDownList(cb_DISC_Combinacion, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.DISC_Combinaciones_PSICOLOGIA, null), DropDownListAction.Select);
        }

        private void LoadCombo_Resultados_Evaluacion()
        {
            OperationResult objOperationResult = new OperationResult();
            Utils.LoadDropDownList(cb_RESULTADO_EVAL_Capacidad, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.ResultadoEvaluacion_NIVEL_EMPO, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cb_RESULTADO_EVAL_JuicioSentidoComun, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.ResultadoEvaluacion_NIVEL_EMPO, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cb_RESULTADO_EVAL_CoordinacionVisoMotriz, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.ResultadoEvaluacion_NIVEL_EMPO, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cb_RESULTADO_EVAL_PlanificacionyOrganizacion, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.ResultadoEvaluacion_NIVEL_EMPO, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cb_RESULTADO_EVAL_PercepcionFrenteSeguridad, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.ResultadoEvaluacion_NIVEL_EMPO, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cb_RESULTADO_EVAL_MotivacionHaciaTrabajo, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.ResultadoEvaluacion_NIVEL_EMPO, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cb_RESULTADO_EVAL_EstabildadEmocional, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.ResultadoEvaluacion_NIVEL_EMPO, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cb_RESULTADO_EVAL_ControlImpulsos, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.ResultadoEvaluacion_NIVEL_EMPO, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cb_RESULTADO_EVAL_RelacionesInterpersonales, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.ResultadoEvaluacion_NIVEL_EMPO, null), DropDownListAction.Select);
            Utils.LoadDropDownList(cb_RESULTADO_EVAL_ManejoPresionyEstres, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.ResultadoEvaluacion_NIVEL_EMPO, null), DropDownListAction.Select);
        }

        private void EnabledDisabledExam()
        {
            var grupoId = (GrupoOcupacional_EMPO_Psicologia)Convert.ToInt32(cbGrupoOcupacional.SelectedValue);

            if (EsoType == TypeESO.PreOcupacional)
            {
                if (grupoId == GrupoOcupacional_EMPO_Psicologia.ConductoresOperadoresEquipoPesado)
                {
                    pnlResultadosEvaluacion.Enabled = true;

                    gbBC_discriminacion_GrupoConductoresYOperadoresEquipo.Enabled = true;
                    gbTestBenton_GrupoConductoresYOperadoresEquipo.Enabled = true;
                    gbTestDibujo_HombreBajoLaLluvia.Enabled = true;

                    // deshailitar    (Operarios)              
                    gbBetaIII_RClaves_GrupoTecnicosYOperarios.Enabled = false;
                    gbBetaIII_RRazonamientoNoVerbal_GrupoOperarios.Enabled = false;

                    //                (Tecnicos)
                    gbFactorE_ConceptualizacionEspacial_GrupoTecnicos.Enabled = false;
                    gbFactorV_ComprensionVerbal_GrupoTecnicos.Enabled = false;
                    gbFactorN_CalculoNumerico_GrupoTecnicos.Enabled = false;
                    //                (Universitarios)
                    gbRazonamiento_GrupoUniversitario.Enabled = false;
                    gbWais_Laberintos.Enabled = false;
                    gbDISC_GrupoUniversitario.Enabled = false;

                }
                else if (grupoId == GrupoOcupacional_EMPO_Psicologia.Operarios)
                {
                    pnlResultadosEvaluacion.Enabled = true;

                    gbBetaIII_RClaves_GrupoTecnicosYOperarios.Enabled = true;
                    gbBetaIII_RRazonamientoNoVerbal_GrupoOperarios.Enabled = true;
                    gbTestDibujo_HombreBajoLaLluvia.Enabled = true;

                    // deshailitar    (Conductores y Operadores de Equipo Pesado)              
                    gbBC_discriminacion_GrupoConductoresYOperadoresEquipo.Enabled = false;
                    gbTestBenton_GrupoConductoresYOperadoresEquipo.Enabled = false;

                    //                (Tecnicos)
                    gbFactorE_ConceptualizacionEspacial_GrupoTecnicos.Enabled = false;
                    gbFactorV_ComprensionVerbal_GrupoTecnicos.Enabled = false;
                    gbFactorN_CalculoNumerico_GrupoTecnicos.Enabled = false;
                    //                (Universitarios)
                    gbRazonamiento_GrupoUniversitario.Enabled = false;
                    gbWais_Laberintos.Enabled = false;
                    gbDISC_GrupoUniversitario.Enabled = false;
                }
                else if (grupoId == GrupoOcupacional_EMPO_Psicologia.Tecnicos)
                {
                    pnlResultadosEvaluacion.Enabled = true;

                    gbBetaIII_RClaves_GrupoTecnicosYOperarios.Enabled = true;
                    gbFactorE_ConceptualizacionEspacial_GrupoTecnicos.Enabled = true;
                    gbFactorV_ComprensionVerbal_GrupoTecnicos.Enabled = true;
                    gbFactorN_CalculoNumerico_GrupoTecnicos.Enabled = true;
                    gbTestDibujo_HombreBajoLaLluvia.Enabled = true;

                    // deshailitar    (Operarios)              
                    gbBetaIII_RRazonamientoNoVerbal_GrupoOperarios.Enabled = false;

                    //                (Conductores y Operadores de Equipo Pesado)              
                    gbBC_discriminacion_GrupoConductoresYOperadoresEquipo.Enabled = false;
                    gbTestBenton_GrupoConductoresYOperadoresEquipo.Enabled = false;

                    //                (Universitarios)
                    gbRazonamiento_GrupoUniversitario.Enabled = false;
                    gbWais_Laberintos.Enabled = false;
                    gbDISC_GrupoUniversitario.Enabled = false;
                }
                else if (grupoId == GrupoOcupacional_EMPO_Psicologia.Universitarios)
                {
                    pnlResultadosEvaluacion.Enabled = true;

                    gbRazonamiento_GrupoUniversitario.Enabled = true;
                    gbWais_Laberintos.Enabled = true;
                    gbTestDibujo_HombreBajoLaLluvia.Enabled = true;
                    gbDISC_GrupoUniversitario.Enabled = true;

                    // deshailitar    (Operarios)              
                    gbBetaIII_RClaves_GrupoTecnicosYOperarios.Enabled = false;
                    gbBetaIII_RRazonamientoNoVerbal_GrupoOperarios.Enabled = false;

                    //                (Tecnicos)
                    gbFactorE_ConceptualizacionEspacial_GrupoTecnicos.Enabled = false;
                    gbFactorV_ComprensionVerbal_GrupoTecnicos.Enabled = false;
                    gbFactorN_CalculoNumerico_GrupoTecnicos.Enabled = false;
                    //                (Conductores y Operadores de Equipo Pesado)              
                    gbBC_discriminacion_GrupoConductoresYOperadoresEquipo.Enabled = false;
                    gbTestBenton_GrupoConductoresYOperadoresEquipo.Enabled = false;

                }
                else
                {
                    pnlResultadosEvaluacion.Enabled = false;

                    // deshailitar    (Conductores y Operadores de Equipo Pesado)              
                    gbBC_discriminacion_GrupoConductoresYOperadoresEquipo.Enabled = false;
                    gbTestBenton_GrupoConductoresYOperadoresEquipo.Enabled = false;
                    gbTestDibujo_HombreBajoLaLluvia.Enabled = false;
                    //                (Operarios)              
                    gbBetaIII_RClaves_GrupoTecnicosYOperarios.Enabled = false;
                    gbBetaIII_RRazonamientoNoVerbal_GrupoOperarios.Enabled = false;
                    gbTestDibujo_HombreBajoLaLluvia.Enabled = false;
                    //                (Tecnicos)
                    gbFactorE_ConceptualizacionEspacial_GrupoTecnicos.Enabled = false;
                    gbFactorV_ComprensionVerbal_GrupoTecnicos.Enabled = false;
                    gbFactorN_CalculoNumerico_GrupoTecnicos.Enabled = false;
                    //                (Universitarios)
                    gbRazonamiento_GrupoUniversitario.Enabled = false;
                    gbWais_Laberintos.Enabled = false;
                    gbDISC_GrupoUniversitario.Enabled = false;
                }

            }
        }

        private void LoadDataPsychologicalExams()
        {
            _psychologicalExam = new List<PsychologicalExam>()
            {
                #region Prueba de Psicologia
		 	
                new PsychologicalExam { PsychologicalExamId = Constants.RAZONAMIENTO, Name = "Razonamiento (Grupo Universitarios)", HasLogic = true, NivelCtrlId = Constants.txt_Razonamiento_Nivel, CategoriaCtrlId = Constants.txt_Razonamiento_Categoria, InterpretacionCtrlId = Constants.txt_Razonamiento_Interpretacion },
                new PsychologicalExam { PsychologicalExamId = Constants.FACTOR_V, Name = "FACTOR V.- Comprensión Verbal (GRUPO TÉCNICOS)", HasLogic = true, NivelCtrlId = Constants.txt_FACTORV_ComprensionVerbal_Nivel, CategoriaCtrlId = Constants.txt_FACTORV_ComprensionVerbal_Categoria, InterpretacionCtrlId = Constants.txt_FACTORV_ComprensionVerbal_Interpretacion },
                new PsychologicalExam { PsychologicalExamId = Constants.FACTOR_N, Name = "FACTOR N.- Cálculo Numérico (GRUPO TÉCNICOS)", HasLogic = true, NivelCtrlId = Constants.txt_FACTORN_CalculoNumerico_Nivel, CategoriaCtrlId = Constants.txt_FACTORN_CalculoNumerico_Categoria, InterpretacionCtrlId = Constants.txt_FACTORN_CalculoNumerico_Interpretacion },
                new PsychologicalExam { PsychologicalExamId = Constants.FACTOR_E, Name = "FACTOR E.- Conceptualización Espacial  (GRUPO TÉCNICOS)", HasLogic = true, NivelCtrlId = Constants.txt_FACTORE_ConceptualizacionEspacial_Nivel, CategoriaCtrlId = Constants.txt_FACTORE_ConceptualizacionEspacial_Categoria, InterpretacionCtrlId = Constants.txt_FACTORE_ConceptualizacionEspacial_Interpretacion },
                new PsychologicalExam { PsychologicalExamId = Constants.BETA_III_R_CLAVES, Name = "BETA III - R: Claves (GRUPO TECNICOS Y OPERARIOS)", HasLogic = true, NivelCtrlId = Constants.txt_BETAIII_RClaves_Nivel, CategoriaCtrlId = Constants.txt_BETAIII_RClaves_Categoria, InterpretacionCtrlId = Constants.txt_BETAIII_RClaves_Interpretacion },
                new PsychologicalExam { PsychologicalExamId = Constants.BETA_III_R_RAZONAMIENTO, Name = "BETA III - R: Razonamiento No Verbal (GRUPO OPERARIOS)", HasLogic = true, NivelCtrlId = Constants.txt_BETTAIII_RRazonamientoNoVerbal_Nivel, CategoriaCtrlId = Constants.txt_BETTAIII_RRazonamientoNoVerbal_Categoria, InterpretacionCtrlId = Constants.txt_BETTAIII_RRazonamientoNoVerbal_Interpretacion },
                new PsychologicalExam { PsychologicalExamId = Constants.BC_DISCRIMINACION, Name = "BC: Discriminación (GRUPO CONDUCTORES Y OPERADORES DE EQUIPO)", HasLogic = true, NivelCtrlId = Constants.txt_BC_Discriminacion_Nivel, CategoriaCtrlId = Constants.txt_BC_Discriminacion_Categoria, InterpretacionCtrlId = Constants.txt_BC_Discriminacion_Interpretacion },
                new PsychologicalExam { PsychologicalExamId = Constants.TEST_BENTON, Name = "TEST DE BENTON (GRUPO CONDUCTORES Y OPERADORES DE EQUIPO)", HasLogic = false },
                new PsychologicalExam { PsychologicalExamId = Constants.WAIS_LABERINTO, Name = "WAIS - Laberintos", HasLogic = false },
                new PsychologicalExam { PsychologicalExamId = Constants.DISC, Name = "DISC (Grupo Universitarios)", HasLogic = true, CategoriaCtrlId = Constants.txt_DISC_Categoria, InterpretacionCtrlId = Constants.txt_DISC_Interpretacion },              
                new PsychologicalExam { PsychologicalExamId = Constants.TEST_DIBUJO_HOMBRE_BAJO_LA_LLUVIA, Name = "TEST DE DIBUJO: HOMBRE BAJO LA LLUVIA", HasLogic = false },
                new PsychologicalExam { PsychologicalExamId = Constants.NOCIONES_SEGURIDAD, Name = "NOCIONES DE SEGURIDAD", HasLogic = false },
                new PsychologicalExam { PsychologicalExamId = "", Name = "" },

                #endregion
            };

            _psychologicalInterpretation = new List<PsychologicalInterpretation>() 
            { 
                #region Interpretaciones
		 	          
                // Razonamiento (Grupo Universitarios)
                new PsychologicalInterpretation { PsychologicalInterpretationId = 1, Name = "Escasa capacidad para el desarrollo de tareas complejas, orientado para trabajos que no requieran de ningún tipo de análisis y  pensamiento lógico."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 2, Name = "Pobre capacidad para percibir situaciones complejas, mas orientado a trabajos que no requieran de análisis, muy poca facilidad para desarrollar situaciones lógicas.facilidad para desarrollar situaciones lógicas."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 3, Name = "Desarrollo limitado para cumplir con tareas que requieran de compleja resolución, con orientación hacia los trabajos que tengan movimientos motores gruesos."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 4, Name = "Tiene la posibilidad de desarrollar el pensamiento lógico, requiere de una constante evaluación, con capacidades para la realización de trabajos complejos pero con apoyo."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 5, Name = "Orientado hacia la ejecución de tareas complejas con capacidad de análisis y toma de decisiones, percepción para investigar y reconocer problemas de carácter lógico, nuevos sistemas y da ideas que requieran de pensamiento abstracto."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 6, Name = "Con gran disponibilidad para asumir tareas de carácter lógico buena disponibilidad para resolver problemas y de adaptación"},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 7, Name = "Facilidad para integrar conceptos y asociarlos, generalmente es crítico, analítico y muy racional en la toma de deciones."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 8, Name = "Totalmente capaz de asumir responsabilidades de riesgo y de nivel lógico, con capacidad para instruir y dirigir situaciones complejas, facilidad para el aprendizaje y percepción muy desarrollada de situaciones difíciles."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 9, Name = "Brillante en desarrollo de tareas que requieran de análisis y de actividad lógica, totalmente comprometido con el aprendizaje y con el crecimiento creativo, extremadamente analítico."},
                //FACTOR V.- Comprensión Verbal (GRUPO TÉCNICOS)
                new PsychologicalInterpretation { PsychologicalInterpretationId = 10, Name = "Escasa capacidad para comprender ideas expresadas en palabras; así como, para captar los problemas por medio de la palabra hablada o escrita."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 11, Name = "Pobre capacidad para comprender ideas expresadas en palabras; así como, para captar los problemas por medio de la palabra hablada o escrita."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 12, Name = "Baja capacidad para comprender ideas expresadas en palabras; así como, para captar los problemas por medio de la palabra hablada o escrita."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 13, Name = "Con indicativos de capacidad para comprender ideas expresadas en palabras.  Capta los problemas por medio de la palabra hablada o escrita."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 14, Name = "Adecuada capacidad para comprender ideas expresadas en palabras. Facilidad para captar los problemas por medio de la palabra hablada o escrita."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 15, Name = "Buena capacidad para comprender ideas expresadas en palabras. Facilidad para captar los problemas por medio de la palabra hablada o escrita."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 16, Name = "Alta capacidad para comprender ideas expresadas en palabras. Facilidad para captar los problemas por medio de la palabra hablada o escrita."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 17, Name = "Destacada capacidad para comprender ideas expresadas en palabras. Facilidad para captar los problemas por medio de la palabra hablada o escrita."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 18, Name = "Brillante capacidad para comprender ideas expresadas en palabras. Facilidad para captar los problemas por medio de la palabra hablada o escrita."},
                // FACTOR N.- Cálculo Numérico (GRUPO TÉCNICOS)
                new PsychologicalInterpretation { PsychologicalInterpretationId = 19, Name = "Escasa capacidad para manejar números y resolver ejercicios o labores de carácter cuantitativo, no recomendable para trabajos donde se apliquen labores exactas.  "},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 20, Name = "Pobre capacidad para manejar números y resolver ejercicios o labores de carácter cuantitativo, no recomendable para trabajos donde se apliquen labores exactas."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 21, Name = "Baja capacidad para manejar números y resolver ejercicios o labores de carácter cuantitativo, no recomendable para trabajos donde se apliquen labores exactas."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 22, Name = "Con indicativos de capacidad para manejar números y resolver ejercicios o labores de carácter cuantitativo, orientado para trabajos donde se apliquen labores exactas."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 23, Name = "Adecuada capacidad para manejar números y resolver ejercicios o labores de carácter cuantitativo, orientado para trabajos donde se apliquen labores exactas."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 24, Name = "Buena capacidad para manejar números y resolver ejercicios o labores de carácter cuantitativo, orientado para trabajos donde se apliquen labores exactas."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 25, Name = "Alta capacidad para manejar números y resolver ejercicios o labores de carácter cuantitativo, orientado para trabajos donde se apliquen labores exactas."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 26, Name = "Destacada capacidad para manejar números y resolver ejercicios o labores de carácter cuantitativo, orientado para trabajos donde se apliquen labores exactas."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 27, Name = "Brillante capacidad para manejar números y resolver ejercicios o labores de carácter cuantitativo, orientado para trabajos donde se apliquen labores exactas."},
                // FACTOR E.- Conceptualización Espacial  (GRUPO TÉCNICOS)
                new PsychologicalInterpretation { PsychologicalInterpretationId = 28, Name = "Escasa capacidad para imaginar y concebir objetos en dos o tres dimensiones; así como, para enfocar y localizar permanentemente el objeto en el espacio, ubicando la figura y forma."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 29, Name = "Pobre capacidad para imaginar y concebir objetos en dos o tres dimensiones; así como, para enfocar y localizar permanentemente el objeto en el espacio, ubicando la figura y forma."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 30, Name = "Baja capacidad para imaginar y concebir objetos en dos o tres dimensiones; así como, para enfocar y localizar permanentemente el objeto en el espacio, ubicando la figura y forma."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 31, Name = "Aparente capacidad para imaginar y concebir objetos en dos o tres dimensiones; así como, para enfocar y localizar permanentemente el objeto en el espacio, ubicando la figura y forma, requiriendo el apoyo de otros."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 32, Name = "Adecuada capacidad para imaginar y concebir objetos en dos o tres dimensiones; así como, para enfocar y localizar permanentemente el objeto en el espacio, ubicando la figura y forma."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 33, Name = "Adecuada capacidad para imaginar y concebir objetos en dos o tres dimensiones; así como, para enfocar y localizar permanentemente el objeto en el espacio, ubicando la figura y forma."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 34, Name = "Alta capacidad para imaginar y concebir objetos en dos o tres dimensiones; así como, para enfocar y localizar permanentemente el objeto en el espacio, ubicando la figura y forma."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 35, Name = "Destacada capacidad para imaginar y concebir objetos en dos o tres dimensiones; así como, para enfocar y localizar permanentemente el objeto en el espacio, ubicando la figura y forma."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 36, Name = "Brillante capacidad para imaginar y concebir objetos en dos o tres dimensiones; así como, para enfocar y localizar permanentemente el objeto en el espacio, ubicando la figura y forma."},
                // BETA III - R: Claves (GRUPO TECNICOS Y OPERARIOS)
                new PsychologicalInterpretation { PsychologicalInterpretationId = 37, Name = "Pobre capacidad para asociar e integrar conceptos, así como para mantenerse atento en una actividad. Presenta serias dificultades en la realización de actividades que requieran destreza, precisión y exactitud."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 38, Name = "Escasa capacidad para asociar e integrar conceptos, así como para mantenerse atento en una actividad. Presenta dificultad en la realización de actividades que requieran destreza, precisión y exactitud."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 39, Name = "Desarrollo limitado para asociar e integrar conceptos, así como para mantenerse atento en una actividad. Denota dificultad en la realización de actividades que requieran destreza, precisión y exactitud."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 40, Name = "Presenta relativa capacidad para asociar e integrar conceptos, siendo capaz de mantenerse atento y concentrado en una actividad, así como, poseer habilidad para realizar actividades que requieran cierta destreza, precisión y exactitud."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 41, Name = "Posee adecuada capacidad para asociar e integrar conceptos, siendo capaz de mantenerse atento y concentrado en una actividad, así como, poseer habilidad para realizar actividades que requieran destreza, precisión y exactitud con rapidez."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 42, Name = "Presenta acertada capacidad para asociar e integrar conceptos, con buen nivel de atención concentración; así como, poseer habilidad para realizar actividades que requieran destreza, precisión y exactitud con rapidez."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 43, Name = "Presenta destacada capacidad para asociar e integrar conceptos, con buen nivel de atención concentración; así como, poseer habilidad para realizar actividades que requieran destreza, precisión y exactitud con rapidez."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 44, Name = "Sobresaliente capacidad para asociar e integrar conceptos, siendo capaz de mantenerse atento y concentrado en una actividad, así como, poseer habilidad para realizar actividades que requieran destreza, precisión y exactitud con rapidez."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 45, Name = "Brillante capacidad para asociar e integrar conceptos, siendo capaz de mantenerse atento y concentrado en una actividad, así como, poseer habilidad para realizar actividades que requieran destreza, precisión y exactitud con rapidez."},
                // BETA III - R: Razonamiento No Verbal (GRUPO OPERARIOS)
                new PsychologicalInterpretation { PsychologicalInterpretationId = 46, Name = "Pobre capacidad para entender la información dada y encontrar soluciones a los problemas por medio del razonamiento práctico o visual."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 47, Name = "Escasa capacidad para entender la información dada y encontrar soluciones a los problemas por medio del razonamiento práctico o visual."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 48, Name = "Desarrollo limitado para entender la información dada y encontrar soluciones a los problemas por medio del razonamiento práctico o visual."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 49, Name = "Posee relativa capacidad para entender la información dada y encontrar soluciones a los problemas por medio del razonamiento práctico o visual."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 50, Name = "Posee adecuada capacidad para entender la información dada y encontrar soluciones a los problemas por medio del razonamiento práctico o visual."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 51, Name = "Presenta buena capacidad para entender la información dada y encontrar soluciones a los problemas por medio del razonamiento práctico o visual."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 52, Name = "Presenta destacada capacidad para entender la información dada y encontrar soluciones a los problemas por medio del razonamiento práctico o visual."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 53, Name = "Sobresaliente capacidad para entender la información dada y encontrar soluciones a los problemas por medio del razonamiento práctico o visual."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 54, Name = "Brillante capacidad para entender la información dada y encontrar soluciones a los problemas por medio del razonamiento práctico o visual."},
                // BC: Discriminación (GRUPO CONDUCTORES Y OPERADORES DE EQUIPO)
                new PsychologicalInterpretation { PsychologicalInterpretationId = 55, Name = "Escasa capacidad de atención - concentración; con dificultad para mantenerse centrado en una tarea por un tiempo prolongado; así como, para discriminar elementos de manera rápida y precisa."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 56, Name = "Pobre capacidad de atención - concentración; con dificultad para mantenerse centrado en una tarea por un tiempo prolongado; así como, para discriminar elementos de manera rápida y precisa."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 57, Name = "Desarrollo limitado de su atención - concentración; con cierta dificultad para mantenerse centrado en una tarea por un tiempo prolongado; así como, para discriminar elementos de manera rápida y precisa."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 58, Name = "Aparente nivel de atención - concentración; es decir, con cierta disposición para mantenerse atento en una tarea; sin embargo, requiere de apoyo para discriminar elementos de manera rápida y precisa."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 59, Name = "Posee adecuado nivel de atención - concentración; es decir, es capaz de mantenerse centrado en una tarea por un tiempo considerable; así como, discriminar elementos de manera rápida y precisa."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 60, Name = "Posee buen nivel de atención - concentración; es decir, es capaz de mantenerse centrado en una tarea por un tiempo prolongado; así como, discriminar elementos de manera rápida y precisa."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 61, Name = "Presenta destacado nivel de atención - concentración; es decir, es capaz de mantenerse centrado en una tarea por un tiempo prolongado; así como, discriminar elementos de manera rápida y precisa."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 62, Name = "Sobresaliente nivel de atención - concentración; es decir, es capaz de mantenerse centrado en una tarea por un tiempo prolongado; así como, discriminar elementos de manera rápida y precisa."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 63, Name = "Brillante nivel de atención - concentración; es decir, es capaz de mantenerse centrado en una tarea por un tiempo prolongado; así como, discriminar elementos de manera rápida y precisa."},
                // DISC (Grupo Universitarios)
                new PsychologicalInterpretation { PsychologicalInterpretationId = 64, Name = "Tiende a ser directo y franco. Dice las cosas como son. Tiene curiosidad intelectual y se siente estimulado por problemas dificiles que requieren poder mental, asi como analisis y resultados logicos. No le interesa mucho complacer a la gente y a veces puede ser brusco, sarcastico y critico."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 65, Name = "Se impacienta por obtener resultados rapidos y tiene un impulso institivo por conseguir poder. Reacciona rapidamente, demuestra aburrimiento instantaneo y acepta desafios. Es versatil, flexible, emprendedor, imperioso e irritable. Soporta bien la presion y se la aplica a los demas. al ser critico e insatisfecho, se siente a gusto con metas cambiantes, proyectos y metodos nuevos. "},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 66, Name = "La oposicion lo estimula. Se define a favor de algo y lucha para defenderlo. Esta decidido en hacer las cosas a su manera. Al ser franco, honesto y directo, se hace cargo de las cosas, actua de un modo positivo, encaminado hacia la solucion y procede sin consultar. Por lo general, es muy respetado cuando todo marcha bien y tiene razon en la mayoria de las casos. Cuando ocurre un reves, en algunos casos alguien lo atribuira a su imprudencia."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 67, Name = "Desea ser aceptado y para lograrlo es amistoso, optimista, confiado y aplomado. Es cordial y encantador, sociable y conversador. Se siente mejor en un ambiente social favorable."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 68, Name = "Es impulsivo y necesita atraer la atencion. Se acerca a las personas y le es facil y grato el contacto inicial. Su optimismo contagioso, entusiasmo instantaneo y persuasiva espontaneidad se concentra para ganarse a las personas y con frecuencia lo logra."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 69, Name = "Por encima de todo, usted esta seguro de si mismo. Necesita y por lo general tiene, una confianza absoluta en su propia habilidad. Prefiere obrar con los demas para conseguir lo que quiere de ellos. Puede ser terco de una manera amistosa; puede dominar una situacion social y sonreir mientras sigue discutiendo el punto. Muestra una independencia efusiva y si la gente lo rechaza a usted o a sus ideas, usted los rechazara a ellos."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 70, Name = "Su anhelo por la sencillez y su aversion a la agresividad abierta se combinan para que sea controlado, firma y consecuente. Al ser modesto, tranquilo y deliberado, le gusta hacer una cosa a la vez y resiste el cambio que transtorna el status quo. Puede aminorar la marcha de las cosas cuando esta frustrado. Es posible que guarde rencores."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 71, Name = "Desea un ambiente pacifico pacifico en el cual puede proceder a su propio ritmo. Prefiere trabajar con cosas en vez de tenerlos que influir en los demas. Es paceinte, controlado, moderadi y deliberado. Alser ocasionalmente timido, no le resulta facil la conversacion y le disguta el antagonismo y los conflictos."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 72, Name = "Se inclina ser un tanto deliberado como terco. Prefiere marcar su propio paso y seguirlo. 'Cumple con la tarea' y resiente que lo presionen. Una vez que se decide, es dificil influirlo para que cambie de opinion. Persiste laboriosamente con la tarea que tiene por delante. Tiene una paciencia impresionante. Sin embargo, puede ser obstinado, reacio y dificil de persuadirlo."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 73, Name = "Es cauteloso y prefiere la cooperacion antes que la confrotacion. Es conservador, preciso y diplomatico. Con frecuencia es riguroso con los detalles, hace cumplir reglas y cuestiona lo diferente. Se dedica a evitar problemas, especialmente los que se pueden impedir con un pocomas de verificacion. Su desempeño parejo, metodo cabal dan la impresion de que es muy perspicaz."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 74, Name = "Es reservado y pensativo, se dedica a los procedentes, instituciones, normas exactas, sistemas, procedimientos definidos y metodos tradicionales. Quiere evitar el riesgo y problemas innecesarios. Es convencional, por lo general diplomatico y cooperativo, con frecuencia aprensivo."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 75, Name = "Tiende a ser diplomatico y preciso por un lado, inquieto y disconforme por el otro. Esta combinacion con frecuencia resulta en tension, nerviosismo y un desasociego generalizado. No se siente contenti hasta que se confirma el acierto de sus acciones o decisiones. Es sensible, a veces sagaz y muy despierto. Por lo general, no se le escapan los motivos ulteriores o practicas engañosas por parte de otros."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 76, Name = "Quiere alcanzar su contenido, pero tambien quiere estar en lo correcto. Con frecuencia no esta conforme con su busqueda de la mejor respuesta posible. Parece vacilar y ser indeciso. Su preocupacion simultanea por las metas y calidad lo convierte en un perfeccionista sin mucho realismo. Puede volverse tenso y deficil de predecir. confundiendo a sus compañeros."},
                //TEST INDICATIVO PARA LA DETECCION DE FATIGA LABORAL
                new PsychologicalInterpretation { PsychologicalInterpretationId = 77, Name = "Puede que se haya detectado algun problema, pero que tenga solucion."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 78, Name = "Se detectan indicios claros de que en el trabajo hay problemas importantes de fatigabilidad, causando efectos. Es necesario actuar abordando el problema."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 79, Name = "Se detectan indicios muy claros de que en el trabajo hay problemas importantes de fatigabilidad, causando efectos. Es necesario actuar abordando el problema inmediatamente."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 80, Name = "Es posible que haya problemas serios. La actuacion preventiva debe añadir las repercusiones de los daños a la salud."},
                // INVENTARIO DE DEPRESION DE BECK
                new PsychologicalInterpretation { PsychologicalInterpretationId = 81, Name = "Depresion ausente o minima."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 82, Name = "Depresion Leve"},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 83, Name = "Depresion Moderada"},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 84, Name = "Depresion Grave"},
                // ESCALA DE AUTOEVALUACION PARA LA DEPRESION DE ZUNG
                new PsychologicalInterpretation { PsychologicalInterpretationId = 85, Name = "Rango Normal"},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 86, Name = "Depresion Leve"},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 87, Name = "Depresion Moderada"},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 88, Name = "Depresion Severa"},
                // ESCALA DE SOMNOLENCIA DE EPWORTH
                new PsychologicalInterpretation { PsychologicalInterpretationId = 89, Name = "No presenta indicadores significativos de somnolencia"},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 90, Name = "Presenta indicadores leves de somnolencia"},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 91, Name = "Presenta indicadores que evidencian tener somnolencia"},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 92, Name = "Presenta indicadores significativos de somnolencia"},
                // ESCALA DE FOBIAS
                new PsychologicalInterpretation { PsychologicalInterpretationId = 93, Name = "Presenta niveles bajos de ansiedad, temor, miedo frente a situaciones que le generen amenazas de asfixia y restricción de movimientos."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 94, Name = "Presenta indicadores que le pueden producir poca ansiedad, temor, miedo frente a situaciones que le generen amenazas de asfixia y restricción de movimientos."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 95, Name = "Presenta indicadores que le pueden producir ansiedad, temor, miedo frente a situaciones que le generen amenazas de asfixia y restricción de movimientos."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 96, Name = "Presenta indicadores que le pueden producir mucha ansiedad, temor, miedo frente a situaciones que le generen amenazas de asfixia y restricción de movimientos."},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 97, Name = "Presenta indicadores que le pueden producir excesiva ansiedad, temor, miedo frente a situaciones que le generen amenazas de asfixia y restricción de movimientos."},
                // TEST DE ACROFOBIA
                new PsychologicalInterpretation { PsychologicalInterpretationId = 98, Name = "Presenta indicadores significativos de acrofobia"},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 99, Name = "No presenta indicadores significativos de Acrofobia"},

                #endregion
            };

            _psychologicalExamDetail = new List<PsychologicalExamDetail>()
            {
                #region Pruebas de Psicologia con sus interpretaciones
		 	
                // Razonamiento (Grupo Universitarios)
                new PsychologicalExamDetail { PsychologicalExamId = Constants.RAZONAMIENTO, PsychologicalInterpretationId = 1, AnalyzingValue1 = 3, AnalyzingValue2 = 7, Level = 1, Category = "BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.RAZONAMIENTO, PsychologicalInterpretationId = 2, AnalyzingValue1 = 8, AnalyzingValue2 = 10, Level = 2, Category = "BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.RAZONAMIENTO, PsychologicalInterpretationId = 3, AnalyzingValue1 = 11, AnalyzingValue2 = 13, Level = 3, Category = "BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.RAZONAMIENTO, PsychologicalInterpretationId = 4, AnalyzingValue1 = 14, AnalyzingValue2 = 15, Level = 4, Category = "PROMEDIO BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.RAZONAMIENTO, PsychologicalInterpretationId = 5, AnalyzingValue1 = 16, AnalyzingValue2 = 18, Level = 5, Category = "PROMEDIO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.RAZONAMIENTO, PsychologicalInterpretationId = 6, AnalyzingValue1 = 19, AnalyzingValue2 = 21, Level = 6, Category = "PROMEDIO ALTO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.RAZONAMIENTO, PsychologicalInterpretationId = 7, AnalyzingValue1 = 22, AnalyzingValue2 = 24, Level = 7, Category = "SUPERIOR AL PROMEDIO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.RAZONAMIENTO, PsychologicalInterpretationId = 8, AnalyzingValue1 = 25, AnalyzingValue2 = 26, Level = 8, Category = "SUPERIOR"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.RAZONAMIENTO, PsychologicalInterpretationId = 9, AnalyzingValue1 = 27, AnalyzingValue2 = 54, Level = 9, Category = "EXCELENTE"},
                // FACTOR V.- Comprensión Verbal (GRUPO TÉCNICOS)
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_V, PsychologicalInterpretationId = 10, AnalyzingValue1 = 1, AnalyzingValue2 = 3, Level = 1, Category = "BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_V, PsychologicalInterpretationId = 11, AnalyzingValue1 = 4, AnalyzingValue2 = 6, Level = 2, Category = "BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_V, PsychologicalInterpretationId = 12, AnalyzingValue1 = 7, AnalyzingValue2 = 9, Level = 3, Category = "BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_V, PsychologicalInterpretationId = 13, AnalyzingValue1 = 10, AnalyzingValue2 = 15, Level = 4, Category = "PROMEDIO BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_V, PsychologicalInterpretationId = 14, AnalyzingValue1 = 16, AnalyzingValue2 = 21, Level = 5, Category = "PROMEDIO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_V, PsychologicalInterpretationId = 15, AnalyzingValue1 = 22, AnalyzingValue2 = 27, Level = 6, Category = "PROMEDIO ALTO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_V, PsychologicalInterpretationId = 16, AnalyzingValue1 = 28, AnalyzingValue2 = 33, Level = 7, Category = "SUPERIOR AL PROMEDIO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_V, PsychologicalInterpretationId = 17, AnalyzingValue1 = 34, AnalyzingValue2 = 43, Level = 8, Category = "SUPERIOR"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_V, PsychologicalInterpretationId = 18, AnalyzingValue1 = 44, AnalyzingValue2 = 48, Level = 9, Category = "EXCELENTE"},
                // FACTOR N.- Cálculo Numérico (GRUPO TÉCNICOS)
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_N, PsychologicalInterpretationId = 19, AnalyzingValue1 = 1, AnalyzingValue2 = 7, Level = 1, Category = "BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_N, PsychologicalInterpretationId = 20, AnalyzingValue1 = 8, AnalyzingValue2 = 12, Level = 2, Category = "BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_N, PsychologicalInterpretationId = 21, AnalyzingValue1 = 13, AnalyzingValue2 = 16, Level = 3, Category = "BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_N, PsychologicalInterpretationId = 22, AnalyzingValue1 = 17, AnalyzingValue2 = 21, Level = 4, Category = "PROMEDIO BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_N, PsychologicalInterpretationId = 23, AnalyzingValue1 = 22, AnalyzingValue2 = 26, Level = 5, Category = "PROMEDIO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_N, PsychologicalInterpretationId = 24, AnalyzingValue1 = 27, AnalyzingValue2 = 31, Level = 6, Category = "PROMEDIO ALTO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_N, PsychologicalInterpretationId = 25, AnalyzingValue1 = 32, AnalyzingValue2 = 37, Level = 7, Category = "SUPERIOR AL PROMEDIO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_N, PsychologicalInterpretationId = 26, AnalyzingValue1 = 38, AnalyzingValue2 = 49, Level = 8, Category = "SUPERIOR"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_N, PsychologicalInterpretationId = 27, AnalyzingValue1 = 50, AnalyzingValue2 = 70, Level = 9, Category = "EXCELENTE"},
                // FACTOR E.- Conceptualización Espacial  (GRUPO TÉCNICOS)
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_E, PsychologicalInterpretationId = 28, AnalyzingValue1 = 0, AnalyzingValue2 = 6, Level = 1, Category = "BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_E, PsychologicalInterpretationId = 29, AnalyzingValue1 = 7, AnalyzingValue2 = 12, Level = 2, Category = "BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_E, PsychologicalInterpretationId = 30, AnalyzingValue1 = 13, AnalyzingValue2 = 18, Level = 3, Category = "BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_E, PsychologicalInterpretationId = 31, AnalyzingValue1 = 19, AnalyzingValue2 = 23, Level = 4, Category = "PROMEDIO BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_E, PsychologicalInterpretationId = 32, AnalyzingValue1 = 24, AnalyzingValue2 = 28, Level = 5, Category = "PROMEDIO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_E, PsychologicalInterpretationId = 33, AnalyzingValue1 = 29, AnalyzingValue2 = 33, Level = 6, Category = "PROMEDIO ALTO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_E, PsychologicalInterpretationId = 34, AnalyzingValue1 = 34, AnalyzingValue2 = 42, Level = 7, Category = "SUPERIOR AL PROMEDIO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_E, PsychologicalInterpretationId = 35, AnalyzingValue1 = 43, AnalyzingValue2 = 48, Level = 8, Category = "SUPERIOR"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.FACTOR_E, PsychologicalInterpretationId = 36, AnalyzingValue1 = 49, AnalyzingValue2 = 54, Level = 9, Category = "EXCELENTE"},
                // BETA III - R: Claves (GRUPO TECNICOS Y OPERARIOS)
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BETA_III_R_CLAVES, PsychologicalInterpretationId = 37, AnalyzingValue1 = 0, AnalyzingValue2 = 14, Level = 1, Category = "BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BETA_III_R_CLAVES, PsychologicalInterpretationId = 38, AnalyzingValue1 = 15, AnalyzingValue2 = 25, Level = 2, Category = "BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BETA_III_R_CLAVES, PsychologicalInterpretationId = 39, AnalyzingValue1 = 26, AnalyzingValue2 = 35, Level = 3, Category = "BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BETA_III_R_CLAVES, PsychologicalInterpretationId = 40, AnalyzingValue1 = 36, AnalyzingValue2 = 42, Level = 4, Category = "PROMEDIO BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BETA_III_R_CLAVES, PsychologicalInterpretationId = 41, AnalyzingValue1 = 43, AnalyzingValue2 = 62, Level = 5, Category = "PROMEDIO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BETA_III_R_CLAVES, PsychologicalInterpretationId = 42, AnalyzingValue1 = 63, AnalyzingValue2 = 83, Level = 6, Category = "PROMEDIO ALTO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BETA_III_R_CLAVES, PsychologicalInterpretationId = 43, AnalyzingValue1 = 84, AnalyzingValue2 = 99, Level = 7, Category = "SUPERIOR AL PROMEDIO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BETA_III_R_CLAVES, PsychologicalInterpretationId = 44, AnalyzingValue1 = 100, AnalyzingValue2 = 123, Level = 8, Category = "SUPERIOR"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BETA_III_R_CLAVES, PsychologicalInterpretationId = 45, AnalyzingValue1 = 124, AnalyzingValue2 = 140, Level = 9, Category = "EXCELENTE"},
                // BETA III - R: Razonamiento No Verbal (GRUPO OPERARIOS)
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BETA_III_R_RAZONAMIENTO, PsychologicalInterpretationId = 46, AnalyzingValue1 = 0, AnalyzingValue2 = 1, Level = 1, Category = "BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BETA_III_R_RAZONAMIENTO, PsychologicalInterpretationId = 47, AnalyzingValue1 = 2, AnalyzingValue2 = 4, Level = 2, Category = "BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BETA_III_R_RAZONAMIENTO, PsychologicalInterpretationId = 48, AnalyzingValue1 = 5, AnalyzingValue2 = 6, Level = 3, Category = "BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BETA_III_R_RAZONAMIENTO, PsychologicalInterpretationId = 49, AnalyzingValue1 = 7, AnalyzingValue2 = 8, Level = 4, Category = "PROMEDIO BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BETA_III_R_RAZONAMIENTO, PsychologicalInterpretationId = 50, AnalyzingValue1 = 9, AnalyzingValue2 = 19, Level = 5, Category = "PROMEDIO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BETA_III_R_RAZONAMIENTO, PsychologicalInterpretationId = 51, AnalyzingValue1 = 20, AnalyzingValue2 = 21, Level = 6, Category = "PROMEDIO ALTO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BETA_III_R_RAZONAMIENTO, PsychologicalInterpretationId = 52, AnalyzingValue1 = 22, AnalyzingValue2 = 22, Level = 7, Category = "SUPERIOR AL PROMEDIO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BETA_III_R_RAZONAMIENTO, PsychologicalInterpretationId = 53, AnalyzingValue1 = 23, AnalyzingValue2 = 23, Level = 8, Category = "SUPERIOR"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BETA_III_R_RAZONAMIENTO, PsychologicalInterpretationId = 54, AnalyzingValue1 = 24, AnalyzingValue2 = 24, Level = 9, Category = "EXCELENTE"},
                // BC: Discriminación (GRUPO CONDUCTORES Y OPERADORES DE EQUIPO)
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BC_DISCRIMINACION, PsychologicalInterpretationId = 55, AnalyzingValue1 = 0, AnalyzingValue2 = 4, Level = 1, Category = "BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BC_DISCRIMINACION, PsychologicalInterpretationId = 56, AnalyzingValue1 = 5, AnalyzingValue2 = 8, Level = 2, Category = "BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BC_DISCRIMINACION, PsychologicalInterpretationId = 57, AnalyzingValue1 = 9, AnalyzingValue2 = 13, Level = 3, Category = "BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BC_DISCRIMINACION, PsychologicalInterpretationId = 58, AnalyzingValue1 = 14, AnalyzingValue2 = 16, Level = 4, Category = "PROMEDIO BAJO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BC_DISCRIMINACION, PsychologicalInterpretationId = 59, AnalyzingValue1 = 17, AnalyzingValue2 = 19, Level = 5, Category = "PROMEDIO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BC_DISCRIMINACION, PsychologicalInterpretationId = 60, AnalyzingValue1 = 20, AnalyzingValue2 = 21, Level = 6, Category = "PROMEDIO ALTO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BC_DISCRIMINACION, PsychologicalInterpretationId = 61, AnalyzingValue1 = 22, AnalyzingValue2 = 22, Level = 7, Category = "SUPERIOR AL PROMEDIO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BC_DISCRIMINACION, PsychologicalInterpretationId = 62, AnalyzingValue1 = 23, AnalyzingValue2 = 23, Level = 8, Category = "SUPERIOR"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.BC_DISCRIMINACION, PsychologicalInterpretationId = 63, AnalyzingValue1 = 24, AnalyzingValue2 = 24, Level = 9, Category = "EXCELENTE"},
                // DISC (Grupo Universitarios)
                new PsychologicalExamDetail { PsychologicalExamId = Constants.DISC, PsychologicalInterpretationId = 64, AnalyzingValue1 = (int?)DISC_Combinaciones_Psicologia.DI, AnalyzingValue2 = (int?)DISC_Combinaciones_Psicologia.DI, Level = 0, Category = "INCISIVO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.DISC, PsychologicalInterpretationId = 65, AnalyzingValue1 = (int?)DISC_Combinaciones_Psicologia.DS, AnalyzingValue2 = (int?)DISC_Combinaciones_Psicologia.DS, Level = 0, Category = "CONSIGUE RESULTADOS"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.DISC, PsychologicalInterpretationId = 66, AnalyzingValue1 = (int?)DISC_Combinaciones_Psicologia.DC, AnalyzingValue2 = (int?)DISC_Combinaciones_Psicologia.DC, Level = 0, Category = "DECISIVO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.DISC, PsychologicalInterpretationId = 67, AnalyzingValue1 = (int?)DISC_Combinaciones_Psicologia.ID, AnalyzingValue2 = (int?)DISC_Combinaciones_Psicologia.ID, Level = 0, Category = "DISCRETO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.DISC, PsychologicalInterpretationId = 68, AnalyzingValue1 = (int?)DISC_Combinaciones_Psicologia.IS, AnalyzingValue2 = (int?)DISC_Combinaciones_Psicologia.IS, Level = 0, Category = "AGRADABLE"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.DISC, PsychologicalInterpretationId = 69, AnalyzingValue1 = (int?)DISC_Combinaciones_Psicologia.IC, AnalyzingValue2 = (int?)DISC_Combinaciones_Psicologia.IC, Level = 0, Category = "CONFIADO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.DISC, PsychologicalInterpretationId = 70, AnalyzingValue1 = (int?)DISC_Combinaciones_Psicologia.SD, AnalyzingValue2 = (int?)DISC_Combinaciones_Psicologia.SD, Level = 0, Category = "CONTROLADO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.DISC, PsychologicalInterpretationId = 71, AnalyzingValue1 = (int?)DISC_Combinaciones_Psicologia.SI, AnalyzingValue2 = (int?)DISC_Combinaciones_Psicologia.SI, Level = 0, Category = "CONCENTRADO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.DISC, PsychologicalInterpretationId = 72, AnalyzingValue1 = (int?)DISC_Combinaciones_Psicologia.SC, AnalyzingValue2 = (int?)DISC_Combinaciones_Psicologia.SC, Level = 0, Category = "PERSISTENTE"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.DISC, PsychologicalInterpretationId = 73, AnalyzingValue1 = (int?)DISC_Combinaciones_Psicologia.CD, AnalyzingValue2 = (int?)DISC_Combinaciones_Psicologia.CD, Level = 0, Category = "COOPERATIVO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.DISC, PsychologicalInterpretationId = 74, AnalyzingValue1 = (int?)DISC_Combinaciones_Psicologia.CI, AnalyzingValue2 = (int?)DISC_Combinaciones_Psicologia.CI, Level = 0, Category = "DISCIPLINADO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.DISC, PsychologicalInterpretationId = 75, AnalyzingValue1 = (int?)DISC_Combinaciones_Psicologia.CS, AnalyzingValue2 = (int?)DISC_Combinaciones_Psicologia.CS, Level = 0, Category = "CONCIENZUDO"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.DISC, PsychologicalInterpretationId = 76, AnalyzingValue1 = (int?)DISC_Combinaciones_Psicologia.D_IGUAL_C, AnalyzingValue2 = (int?)DISC_Combinaciones_Psicologia.D_IGUAL_C, Level = 0, Category = "AMBIVALENTE"},

                #endregion
            };

        }

        private void SetControlName()
        {
            cbGrupoOcupacional.Name = Constants.cb_GrupoOcupacional;

            txt_Razonamiento_Puntaje.Name = Constants.txt_Razonamiento_Puntaje;
            txt_Razonamiento_Nivel.Name = Constants.txt_Razonamiento_Nivel;
            txt_Razonamiento_Categoria.Name = Constants.txt_Razonamiento_Categoria;
            txt_Razonamiento_Interpretacion.Name = Constants.txt_Razonamiento_Interpretacion;
          
            txt_BETAIII_RClaves_Puntaje.Name = Constants.txt_BETAIII_RClaves_Puntaje;
            txt_BETAIII_RClaves_Nivel.Name = Constants.txt_BETAIII_RClaves_Nivel;
            txt_BETAIII_RClaves_Categoria.Name = Constants.txt_BETAIII_RClaves_Categoria;
            txt_BETAIII_RClaves_Interpretacion.Name = Constants.txt_BETAIII_RClaves_Interpretacion;

            txt_BETTAIII_RRazonamientoNoVerbal_Puntaje.Name = Constants.txt_BETTAIII_RRazonamientoNoVerbal_Puntaje;
            txt_BETTAIII_RRazonamientoNoVerbal_Nivel.Name = Constants.txt_BETTAIII_RRazonamientoNoVerbal_Nivel;
            txt_BETTAIII_RRazonamientoNoVerbal_Categoria.Name = Constants.txt_BETTAIII_RRazonamientoNoVerbal_Categoria;
            txt_BETTAIII_RRazonamientoNoVerbal_Interpretacion.Name = Constants.txt_BETTAIII_RRazonamientoNoVerbal_Interpretacion;

            txt_FACTORV_ComprensionVerbal_Puntaje.Name = Constants.txt_FACTORV_ComprensionVerbal_Puntaje;
            txt_FACTORV_ComprensionVerbal_Nivel.Name = Constants.txt_FACTORV_ComprensionVerbal_Nivel;
            txt_FACTORV_ComprensionVerbal_Categoria.Name = Constants.txt_FACTORV_ComprensionVerbal_Categoria;
            txt_FACTORV_ComprensionVerbal_Interpretacion.Name = Constants.txt_FACTORV_ComprensionVerbal_Interpretacion;

            txt_FACTORN_CalculoNumerico_Puntaje.Name = Constants.txt_FACTORN_CalculoNumerico_Puntaje;
            txt_FACTORN_CalculoNumerico_Nivel.Name = Constants.txt_FACTORN_CalculoNumerico_Nivel;
            txt_FACTORN_CalculoNumerico_Categoria.Name = Constants.txt_FACTORN_CalculoNumerico_Categoria;
            txt_FACTORN_CalculoNumerico_Interpretacion.Name = Constants.txt_FACTORN_CalculoNumerico_Interpretacion;

            txt_FACTORE_ConceptualizacionEspacial_Puntaje.Name = Constants.txt_FACTORE_ConceptualizacionEspacial_Puntaje;
            txt_FACTORE_ConceptualizacionEspacial_Nivel.Name = Constants.txt_FACTORE_ConceptualizacionEspacial_Nivel;
            txt_FACTORE_ConceptualizacionEspacial_Categoria.Name = Constants.txt_FACTORE_ConceptualizacionEspacial_Categoria;
            txt_FACTORE_ConceptualizacionEspacial_Interpretacion.Name = Constants.txt_FACTORE_ConceptualizacionEspacial_Interpretacion;

            txt_BC_Discriminacion_Puntaje.Name = Constants.txt_BC_Discriminacion_Puntaje;
            txt_BC_Discriminacion_Nivel.Name = Constants.txt_BC_Discriminacion_Nivel;
            txt_BC_Discriminacion_Categoria.Name = Constants.txt_BC_Discriminacion_Categoria;
            txt_BC_Discriminacion_Interpretacion.Name = Constants.txt_BC_Discriminacion_Interpretacion;

            txt_WAIS_LABERINTOS_Interpretacion.Name = Constants.txt_WAIS_LABERINTOS_Interpretacion;
            cb_DISC_Combinacion.Name = Constants.cb_DISC_Combinacion;
            txt_DISC_Categoria.Name = Constants.txt_DISC_Categoria;
            txt_DISC_Interpretacion.Name = Constants.txt_DISC_Interpretacion;

            txt_TEST_DIBUJO_HOMBRE_BAJO_LLUVIA_Interpretacion.Name = Constants.txt_TEST_DIBUJO_HOMBRE_BAJO_LLUVIA_Interpretacion;
            chk_TieneNocionesSeguridad.Name = Constants.chk_TieneNocionesSeguridad;

            // TAG
            txt_Razonamiento_Puntaje.Tag = Constants.RAZONAMIENTO;
            txt_FACTORV_ComprensionVerbal_Puntaje.Tag = Constants.FACTOR_V;
            txt_FACTORN_CalculoNumerico_Puntaje.Tag = Constants.FACTOR_N;
            txt_FACTORE_ConceptualizacionEspacial_Puntaje.Tag = Constants.FACTOR_E;
            txt_BETAIII_RClaves_Puntaje.Tag = Constants.BETA_III_R_CLAVES;
            txt_BETTAIII_RRazonamientoNoVerbal_Puntaje.Tag = Constants.BETA_III_R_RAZONAMIENTO;
            txt_BC_Discriminacion_Puntaje.Tag = Constants.BC_DISCRIMINACION;
            txt_TEST_BETON_Interpretacion.Tag = Constants.TEST_BENTON;
            txt_WAIS_LABERINTOS_Interpretacion.Tag = Constants.WAIS_LABERINTO;
            cb_DISC_Combinacion.Tag = Constants.DISC;
            txt_TEST_DIBUJO_HOMBRE_BAJO_LLUVIA_Interpretacion.Tag = Constants.TEST_DIBUJO_HOMBRE_BAJO_LA_LLUVIA;
            chk_TieneNocionesSeguridad.Tag = Constants.NOCIONES_SEGURIDAD;

            // Resultado de la evaluacion EMPO
            cb_RESULTADO_EVAL_Capacidad.Name = Constants.cb_RESULTADO_EVAL_Capacidad;
            cb_RESULTADO_EVAL_JuicioSentidoComun.Name = Constants.cb_RESULTADO_EVAL_JuicioSentidoComun;
            cb_RESULTADO_EVAL_CoordinacionVisoMotriz.Name = Constants.cb_RESULTADO_EVAL_CoordinacionVisoMotriz;
            cb_RESULTADO_EVAL_PlanificacionyOrganizacion.Name = Constants.cb_RESULTADO_EVAL_PlanificacionyOrganizacion;
            cb_RESULTADO_EVAL_PercepcionFrenteSeguridad.Name = Constants.cb_RESULTADO_EVAL_PercepcionFrenteSeguridad;
            cb_RESULTADO_EVAL_MotivacionHaciaTrabajo.Name = Constants.cb_RESULTADO_EVAL_MotivacionHaciaTrabajo;
            cb_RESULTADO_EVAL_EstabildadEmocional.Name = Constants.cb_RESULTADO_EVAL_EstabildadEmocional;
            cb_RESULTADO_EVAL_ControlImpulsos.Name = Constants.cb_RESULTADO_EVAL_ControlImpulsos;
            cb_RESULTADO_EVAL_RelacionesInterpersonales.Name = Constants.cb_RESULTADO_EVAL_RelacionesInterpersonales;
            cb_RESULTADO_EVAL_ManejoPresionyEstres.Name = Constants.cb_RESULTADO_EVAL_ManejoPresionyEstres;

        }

        private PsychologicalExam GetControlsByPsychologicalExamId(string PsychologicalExamId)
        {
            var sql = (from b in _psychologicalExam       
                                   
                       where (b.PsychologicalExamId == PsychologicalExamId) 

                       select new PsychologicalExam
                       {
                           PsychologicalExamId = b.PsychologicalExamId,
                           Name = b.Name,                         
                           HasLogic = b.HasLogic,
                           NivelCtrlId = b.NivelCtrlId,
                           CategoriaCtrlId = b.CategoriaCtrlId,
                           InterpretacionCtrlId = b.InterpretacionCtrlId,
                       }).FirstOrDefault();

            return sql;
        }

        private PsychologicalExamDetail GetInterpretation(string PsychologicalExamId, int valueToAnalyze)
        {
            var sql = (from a in _psychologicalExamDetail
                       join b in _psychologicalExam on a.PsychologicalExamId equals b.PsychologicalExamId
                       join c in _psychologicalInterpretation on a.PsychologicalInterpretationId equals c.PsychologicalInterpretationId

                       where (a.PsychologicalExamId == PsychologicalExamId) &&
                             (valueToAnalyze >= a.AnalyzingValue1 && valueToAnalyze <= a.AnalyzingValue2)

                       select new PsychologicalExamDetail
                       {
                           PsychologicalExamName = b.Name,
                           AnalyzingValue1 = a.AnalyzingValue1,
                           AnalyzingValue2 = a.AnalyzingValue2,
                           Level = a.Level,
                           Category = a.Category,
                           PsychologicalInterpretationName = c.Name,
                           HasLogic = b.HasLogic,
                           NivelCtrlId = b.NivelCtrlId,
                           InterpretacionCtrlId = b.InterpretacionCtrlId,
                       }).FirstOrDefault();

            return sql;
        }

        private void SetInfo(Control puntaje, TextBox nivel, TextBox categoria, TextBox interpretacion)
        {
            var value = GetValueControl(puntaje);
          
            if (string.IsNullOrEmpty(value))
            {
                if (nivel != null)
                    nivel.Text = string.Empty;
                categoria.Text = string.Empty;
                interpretacion.Text = string.Empty;
                return;
            }
           
            var data = GetInterpretation(puntaje.Tag.ToString(), Convert.ToInt32(value));

            if (data == null)
            {             
                if (nivel != null)
                    nivel.Text = string.Empty;
                categoria.Text = string.Empty;
                interpretacion.Text = string.Empty;
                return;             
            }

            SaveValueControlForInterfacingESO(puntaje.Name, value);

            if (nivel != null)
                nivel.Text = data.Level.ToString();
            categoria.Text = data.Category;
            interpretacion.Text = data.PsychologicalInterpretationName;                  
           
        }

        private void SearchControlAndFill(List<ServiceComponentFieldValuesList> DataSource)
        {
            if (DataSource == null || DataSource.Count == 0)
                return;

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
                            if (item.v_ComponentFieldId == Constants.cb_GrupoOcupacional)
                            {
                                _oldGroupOcupationalId = item.v_Value1;
                            }
                         
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

            _serviceComponentFieldValuesList.RemoveAll(p => p.v_ComponentFieldId == name);

            _serviceComponentFieldValues = new ServiceComponentFieldValuesList();

            _serviceComponentFieldValues.v_ComponentFieldId = name;
            _serviceComponentFieldValues.v_Value1 = value;

            _serviceComponentFieldValuesList.Add(_serviceComponentFieldValues);

            DataSource = _serviceComponentFieldValuesList;

            #endregion
        }

        private void ClearDataToSave()
        {         
            foreach (var item in _serviceComponentFieldValuesList)
            {
                if (item.v_ComponentFieldId == Constants.cb_DISC_Combinacion
                   || item.v_ComponentFieldId == Constants.cb_RESULTADO_EVAL_Capacidad
                   || item.v_ComponentFieldId == Constants.cb_RESULTADO_EVAL_JuicioSentidoComun
                   || item.v_ComponentFieldId == Constants.cb_RESULTADO_EVAL_CoordinacionVisoMotriz
                   || item.v_ComponentFieldId == Constants.cb_RESULTADO_EVAL_PlanificacionyOrganizacion
                   || item.v_ComponentFieldId == Constants.cb_RESULTADO_EVAL_PercepcionFrenteSeguridad
                   || item.v_ComponentFieldId == Constants.cb_RESULTADO_EVAL_MotivacionHaciaTrabajo
                   || item.v_ComponentFieldId == Constants.cb_RESULTADO_EVAL_EstabildadEmocional
                   || item.v_ComponentFieldId == Constants.cb_RESULTADO_EVAL_ControlImpulsos
                   || item.v_ComponentFieldId == Constants.cb_RESULTADO_EVAL_RelacionesInterpersonales
                   || item.v_ComponentFieldId == Constants.cb_RESULTADO_EVAL_ManejoPresionyEstres)
                {
                    item.v_Value1 = "-1";
                }
                else if (item.v_ComponentFieldId == Constants.chk_TieneNocionesSeguridad)
                {
                     item.v_Value1 = "0";
                }
                else
                {
                    item.v_Value1 = string.Empty;
                }
            }
        }

        #endregion

       

      
    }
}
