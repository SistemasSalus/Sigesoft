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
    public partial class ucPsychologicalExamAnual : UserControl
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

        public ucPsychologicalExamAnual()
        {
            InitializeComponent();
        }

        public ucPsychologicalExamAnual(TypeESO esoType)
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

            //cbGrupoOcupacional.SelectedValueChanged -= new EventHandler(Controls_ValueChanged);

            LoadComboGroupOcupationByEsoType();
            LoadCombo_DISC_Combinaciones();
          
            // Cargar combo de estabilidad emocional
            OperationResult objOperationResult = new OperationResult();
            Utils.LoadDropDownList(cb_RESULTADO_EVAL_EstabilidadEmocional, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.ResultadoEvaluacion_EstabilidadEmocional, null), DropDownListAction.Select);

            // Cargar combo de Nivel de stress        
            Utils.LoadDropDownList(cb_RESULTADO_EVAL_NivelStres, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.ResultadoEvaluacion_NivelEstres, null), DropDownListAction.Select);

            // Cargar combo de personalidad        
            Utils.LoadDropDownList(cb_RESULTADO_EVAL_Personalidad, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.ResultadoEvaluacion_Personalidad, null), DropDownListAction.Select);

            // Cargar combo de afectividad        
            Utils.LoadDropDownList(cb_RESULTADO_EVAL_Afectividad, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.ResultadoEvaluacion_Afectividad, null), DropDownListAction.Select);

            // Cargar combo de Motivacion      
            Utils.LoadDropDownList(cb_RESULTADO_EVAL_Motivacion, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.ResultadoEvaluacion_Motivacion, null), DropDownListAction.Select);

            
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
                    if (findResult.CategoriaCtrlId != null)
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
            cbGrupoOcupacional.TextChanged -= new EventHandler(cbGrupoOcupacional_TextChanged);

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

            cbGrupoOcupacional.TextChanged += new EventHandler(cbGrupoOcupacional_TextChanged);

        }

        private void LoadCombo_DISC_Combinaciones()
        {
            OperationResult objOperationResult = new OperationResult();
            Utils.LoadDropDownList(cb_DISC_Combinacion, "Value1", "Id", BLL.Utils.GetSystemParameterForCombo(ref objOperationResult, (int)SystemParameterGroups.DISC_Combinaciones_PSICOLOGIA, null), DropDownListAction.Select);
        }

        private void EnabledDisabledExam()
        {
            var grupoId = (GrupoOcupacional_EMOA_Psicologia)Convert.ToInt32(cbGrupoOcupacional.SelectedValue);
         
            if (grupoId == GrupoOcupacional_EMOA_Psicologia.OperariosYTecnicos)
            {
                gbEscalaSintomaticaEstres_Operativos_Tecnicos.Enabled = true;                   
                gbTestDibujo_HombreBajoLaLluvia.Enabled = true;

                gbCuestionarioEstresOrganizacional_GrupoUniversitario.Enabled = false;
                gbDISC_GrupoUniversitario.Enabled = false;
                gbTestIndicativoFatigaLaboral.Enabled = false;

                pnlResultdosEvaluacion.Enabled = true;

                // Deshabilitar
                cb_RESULTADO_EVAL_EstabilidadEmocional.Enabled = false;
                lblEstabilidadEmocional_NO_APLICA.Text = "No Aplica";
                txt_RESULTADO_EVAL_IndicadoresFobia.Text = "No Aplica";
                txt_RESULTADO_EVAL_IndicadoresFatigaySomnolencia.Text = "No Aplica";

                // habilitar
                cb_RESULTADO_EVAL_Personalidad.Enabled = true;
                cb_RESULTADO_EVAL_Afectividad.Enabled = true;
                cb_RESULTADO_EVAL_Motivacion.Enabled = true;

                // Deshabilitar
                cb_RESULTADO_EVAL_NivelStres.Enabled = false;
                lblNievelEstres_NO_APLICA.Text = "No Aplica";

                if (txt_RESULTADO_EVAL_IndicadoresPsicosomaticosStres.Text == "No Aplica")
                {
                    txt_RESULTADO_EVAL_IndicadoresPsicosomaticosStres.Text = "";
                }

                txt_RESULTADO_EVAL_IndicadoresFatigaLaboral.Text = "No Aplica";

            }
            else if (grupoId == GrupoOcupacional_EMOA_Psicologia.Conductores)
            {
                gbEscalaSintomaticaEstres_Operativos_Tecnicos.Enabled = true;
                gbTestIndicativoFatigaLaboral.Enabled = true;
                gbTestDibujo_HombreBajoLaLluvia.Enabled = true;

                gbCuestionarioEstresOrganizacional_GrupoUniversitario.Enabled = false;
                gbDISC_GrupoUniversitario.Enabled = false;

                pnlResultdosEvaluacion.Enabled = true;

                // Deshabilitar
                cb_RESULTADO_EVAL_EstabilidadEmocional.Enabled = false;
                lblEstabilidadEmocional_NO_APLICA.Text = "No Aplica";
                txt_RESULTADO_EVAL_IndicadoresFobia.Text = "No Aplica";
                txt_RESULTADO_EVAL_IndicadoresFatigaySomnolencia.Text = "No Aplica";

                // habilitar
                cb_RESULTADO_EVAL_Personalidad.Enabled = true;
                cb_RESULTADO_EVAL_Afectividad.Enabled = true;
                cb_RESULTADO_EVAL_Motivacion.Enabled = true;

                // Deshabilitar
                cb_RESULTADO_EVAL_NivelStres.Enabled = false;
                lblNievelEstres_NO_APLICA.Text = "No Aplica";
                txt_RESULTADO_EVAL_IndicadoresPsicosomaticosStres.Text = "No Aplica";
                   
            }
            else if (grupoId == GrupoOcupacional_EMOA_Psicologia.Universitarios)
            {
              
                gbDISC_GrupoUniversitario.Enabled = true;
                gbCuestionarioEstresOrganizacional_GrupoUniversitario.Enabled = true;

                gbEscalaSintomaticaEstres_Operativos_Tecnicos.Enabled = false;
                gbTestDibujo_HombreBajoLaLluvia.Enabled = false;
                gbTestIndicativoFatigaLaboral.Enabled = false;

                pnlResultdosEvaluacion.Enabled = true;

                // Deshabilitar
                cb_RESULTADO_EVAL_EstabilidadEmocional.Enabled = false;
                lblEstabilidadEmocional_NO_APLICA.Text = "No Aplica";
                txt_RESULTADO_EVAL_IndicadoresFobia.Text = "No Aplica";
                txt_RESULTADO_EVAL_IndicadoresFatigaySomnolencia.Text = "No Aplica";

                // habilitar
                cb_RESULTADO_EVAL_Personalidad.Enabled = true;
                cb_RESULTADO_EVAL_Afectividad.Enabled = true;
                cb_RESULTADO_EVAL_Motivacion.Enabled = true;

               
                cb_RESULTADO_EVAL_NivelStres.Enabled = false;
                lblNievelEstres_NO_APLICA.Text = "";
                txt_RESULTADO_EVAL_IndicadoresPsicosomaticosStres.Text = "No Aplica";
                txt_RESULTADO_EVAL_IndicadoresFatigaLaboral.Text = "No Aplica";

            }
            else
            {
                pnlResultdosEvaluacion.Enabled = false;

                gbEscalaSintomaticaEstres_Operativos_Tecnicos.Enabled = false;
                gbTestDibujo_HombreBajoLaLluvia.Enabled = false;

                gbCuestionarioEstresOrganizacional_GrupoUniversitario.Enabled = false;
                gbDISC_GrupoUniversitario.Enabled = false;
                gbTestIndicativoFatigaLaboral.Enabled = false;

            }

            
        }

        private void LoadDataPsychologicalExams()
        {
            _psychologicalExam = new List<PsychologicalExam>()
            {
                #region Prueba de Psicologia
		 	             
                new PsychologicalExam { PsychologicalExamId = Constants.DISC, Name = "DISC (Grupo Universitarios)", HasLogic = true, CategoriaCtrlId = Constants.txt_DISC_Categoria, InterpretacionCtrlId = Constants.txt_DISC_Interpretacion },              
                new PsychologicalExam { PsychologicalExamId = Constants.TEST_DIBUJO_HOMBRE_BAJO_LA_LLUVIA, Name = "TEST DE DIBUJO: HOMBRE BAJO LA LLUVIA", HasLogic = false },             
                new PsychologicalExam { PsychologicalExamId = Constants.CUESTIONARIO_ESTRÉS_ORGANIZACIONAL, Name = "CUESTIONARIO DE ESTRÉS ORGANIZACIONAL - GRUPO UNIVERSITARIOS", HasLogic = true, CategoriaCtrlId = Constants.txt_CuestionarioEstresOrganizacional_Categoria, InterpretacionCtrlId = Constants.txt_CuestionarioEstresOrganizacional_interpretacion },
                new PsychologicalExam { PsychologicalExamId = Constants.ESCALA_SINTOMATICA_ESTRES, Name = "ESCALA SINTOMATICA DE ESTRÉS (SEPPO ARO) - GRUPO OCUPACIONAL OPERATIVO Y TECNICOS", HasLogic = true, CategoriaCtrlId = Constants.txt_EscalaSintomaticaEstres_Categoria, InterpretacionCtrlId = Constants.txt_EscalaSintomaticaEstres_Interpretacion },
                new PsychologicalExam { PsychologicalExamId = Constants.TEST_INDICATIVO_FATIGA_LABORAL, Name = "TEST INDICATIVO PARA LA DETECCION DE FATIGA LABORAL PARA CONDUCTORES", HasLogic = true, InterpretacionCtrlId = Constants.txt_TestIndicativoFatigaLaboral_Interpretacion },

                #endregion
            };

            _psychologicalInterpretation = new List<PsychologicalInterpretation>() 
            { 
                #region Interpretaciones
		 	                      
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
               
                // CUESTIONARIO DE ESTRÉS ORGANIZACIONAL - GRUPO UNIVERSITARIOS
                new PsychologicalInterpretation { PsychologicalInterpretationId = 100, Name = "No presenta indicadores significativos de estrés laboral"},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 101, Name = "Presenta indicadores de estrés laboral"},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 102, Name = "Presenta indicadores significativos de estrés laboral"},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 103, Name = "Presenta indicadores significativos de estrés laboral"},
                
                // ESCALA SINTOMATICA DE ESTRÉS (SEPPO ARO) - GRUPO OCUPACIONAL OPERATIVO Y TECNICOS
                new PsychologicalInterpretation { PsychologicalInterpretationId = 104, Name = "No presenta indicadores"},
                new PsychologicalInterpretation { PsychologicalInterpretationId = 105, Name = "psicosomaticos de estrés."},
                             
                #endregion
            };

            _psychologicalExamDetail = new List<PsychologicalExamDetail>()
            {
                #region Pruebas de Psicologia con sus interpretaciones
		 	                            
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
                // TEST INDICATIVO PARA LA DETECCION DE FATIGA LABORAL PARA CONDUCTORES
                new PsychologicalExamDetail { PsychologicalExamId = Constants.TEST_INDICATIVO_FATIGA_LABORAL, PsychologicalInterpretationId = 77, AnalyzingValue1 = 1, AnalyzingValue2 = 2, },
                new PsychologicalExamDetail { PsychologicalExamId = Constants.TEST_INDICATIVO_FATIGA_LABORAL, PsychologicalInterpretationId = 78, AnalyzingValue1 = 3, AnalyzingValue2 = 10 },
                new PsychologicalExamDetail { PsychologicalExamId = Constants.TEST_INDICATIVO_FATIGA_LABORAL, PsychologicalInterpretationId = 79, AnalyzingValue1 = 11, AnalyzingValue2 = 14 },
                new PsychologicalExamDetail { PsychologicalExamId = Constants.TEST_INDICATIVO_FATIGA_LABORAL, PsychologicalInterpretationId = 80, AnalyzingValue1 = 15, AnalyzingValue2 = 100 },
                
                // CUESTIONARIO DE ESTRÉS ORGANIZACIONAL - GRUPO UNIVERSITARIOS
                new PsychologicalExamDetail { PsychologicalExamId = Constants.CUESTIONARIO_ESTRÉS_ORGANIZACIONAL, PsychologicalInterpretationId = 100, AnalyzingValue1 = 0, AnalyzingValue2 = 89, Level = 1, Category = "Bajo nivel de estrés (4)"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.CUESTIONARIO_ESTRÉS_ORGANIZACIONAL, PsychologicalInterpretationId = 101, AnalyzingValue1 = 90, AnalyzingValue2 = 117, Level = 2, Category = "Nivel intermedio(3)"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.CUESTIONARIO_ESTRÉS_ORGANIZACIONAL, PsychologicalInterpretationId = 102, AnalyzingValue1 = 118, AnalyzingValue2 = 153, Level = 3, Category = "Estrés(2)"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.CUESTIONARIO_ESTRÉS_ORGANIZACIONAL, PsychologicalInterpretationId = 103, AnalyzingValue1 = 154, AnalyzingValue2 = 1000, Level = 4, Category = "Alto nivel de estrés(1)"},
               
                // ESCALA SINTOMATICA DE ESTRÉS (SEPPO ARO) - GRUPO OCUPACIONAL OPERATIVO Y TECNICOS
                new PsychologicalExamDetail { PsychologicalExamId = Constants.ESCALA_SINTOMATICA_ESTRES, PsychologicalInterpretationId = 104, AnalyzingValue1 = 18, AnalyzingValue2 = 1000, Level = 1, Category = "AUSENTE"},
                new PsychologicalExamDetail { PsychologicalExamId = Constants.ESCALA_SINTOMATICA_ESTRES, PsychologicalInterpretationId = 105, AnalyzingValue1 = 0, AnalyzingValue2 = 17, Level = 2, Category = "PRESENTA"},
               
                #endregion
            };

        }

        private void SetControlName()
        {
            cbGrupoOcupacional.Name = Constants.cb_GrupoOcupacional;

            txt_CuestionarioEstresOrganizacional_Puntaje.Name = Constants.txt_CuestionarioEstresOrganizacional_Puntaje;
            txt_CuestionarioEstresOrganizacional_Categoria.Name = Constants.txt_CuestionarioEstresOrganizacional_Categoria;
            txt_CuestionarioEstresOrganizacional_interpretacion.Name = Constants.txt_CuestionarioEstresOrganizacional_interpretacion;

            txt_EscalaSintomaticaEstres_Puntaje.Name = Constants.txt_EscalaSintomaticaEstres_Puntaje;
            txt_EscalaSintomaticaEstres_Categoria.Name = Constants.txt_EscalaSintomaticaEstres_Categoria;
            txt_EscalaSintomaticaEstres_Interpretacion.Name = Constants.txt_EscalaSintomaticaEstres_Interpretacion;

            // Fatiga laboral
            txt_TestIndicativoFatigaLaboral_Puntaje.Name = Constants.txt_TestIndicativoFatigaLaboral_Puntaje;
            txt_TestIndicativoFatigaLaboral_Interpretacion.Name = Constants.txt_TestIndicativoFatigaLaboral_Interpretacion;

            cb_DISC_Combinacion.Name = Constants.cb_DISC_Combinacion;
            txt_DISC_Categoria.Name = Constants.txt_DISC_Categoria;
            txt_DISC_Interpretacion.Name = Constants.txt_DISC_Interpretacion;

            txt_TEST_DIBUJO_HOMBRE_BAJO_LLUVIA_Interpretacion.Name = Constants.txt_TEST_DIBUJO_HOMBRE_BAJO_LLUVIA_Interpretacion;        
          
            // TAG
            txt_CuestionarioEstresOrganizacional_Puntaje.Tag = Constants.CUESTIONARIO_ESTRÉS_ORGANIZACIONAL;
            txt_EscalaSintomaticaEstres_Puntaje.Tag = Constants.ESCALA_SINTOMATICA_ESTRES;
            txt_TestIndicativoFatigaLaboral_Puntaje.Tag = Constants.TEST_INDICATIVO_FATIGA_LABORAL;  
            cb_DISC_Combinacion.Tag = Constants.DISC;
            txt_TEST_DIBUJO_HOMBRE_BAJO_LLUVIA_Interpretacion.Tag = Constants.TEST_DIBUJO_HOMBRE_BAJO_LA_LLUVIA;

            // Resultado de Evaluacion EMOA
            cb_RESULTADO_EVAL_EstabilidadEmocional.Name = Constants.cb_RESULTADO_EVAL_EstabilidadEmocional;
            txt_RESULTADO_EVAL_IndicadoresFobia.Name = Constants.txt_RESULTADO_EVAL_IndicadoresFobia;
            txt_RESULTADO_EVAL_IndicadoresFatigaySomnolencia.Name = Constants.txt_RESULTADO_EVAL_IndicadoresFatigaySomnolencia;
            cb_RESULTADO_EVAL_Personalidad.Name = Constants.cb_RESULTADO_EVAL_Personalidad;
            cb_RESULTADO_EVAL_Afectividad.Name = Constants.cb_RESULTADO_EVAL_Afectividad;
            cb_RESULTADO_EVAL_Motivacion.Name = Constants.cb_RESULTADO_EVAL_Motivacion;
            cb_RESULTADO_EVAL_NivelStres.Name = Constants.cb_RESULTADO_EVAL_NivelStres;
            txt_RESULTADO_EVAL_IndicadoresPsicosomaticosStres.Name = Constants.txt_RESULTADO_EVAL_IndicadoresPsicosomaticosStres;
            txt_RESULTADO_EVAL_IndicadoresFatigaLaboral.Name = Constants.txt_RESULTADO_EVAL_IndicadoresFatigaLaboral;
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
                if (categoria != null)
                    categoria.Text = string.Empty;
                interpretacion.Text = string.Empty;

                // Limpiar caja de resultado de evaluacion
                if (puntaje.Name == Constants.txt_CuestionarioEstresOrganizacional_Puntaje)
                {
                    cb_RESULTADO_EVAL_NivelStres.SelectedValue = "-1";
                }
                else if (puntaje.Name == Constants.txt_EscalaSintomaticaEstres_Puntaje)
                {
                    txt_RESULTADO_EVAL_IndicadoresPsicosomaticosStres.Text = string.Empty;
                }
                else if (puntaje.Name == Constants.txt_TestIndicativoFatigaLaboral_Puntaje)
                {
                    txt_RESULTADO_EVAL_IndicadoresFatigaLaboral.Text = string.Empty;
                }                                  

                return;
            }
           
            var data = GetInterpretation(puntaje.Tag.ToString(), Convert.ToInt32(value));

            if (data == null)
            {             
                if (nivel != null)
                    nivel.Text = string.Empty;
                if (categoria != null)
                    categoria.Text = string.Empty;
                interpretacion.Text = string.Empty;
                return;             
            }

            SaveValueControlForInterfacingESO(puntaje.Name, value);

            if (nivel != null)
                nivel.Text = data.Level.ToString();
            if (categoria != null)
                categoria.Text = data.Category;
            interpretacion.Text = data.PsychologicalInterpretationName;     
            
            // Setear automaticamente los resultados de la evaluacion en sus cajas correspondientes
            if (data.InterpretacionCtrlId == Constants.txt_EscalaSintomaticaEstres_Interpretacion)
            {              
                txt_RESULTADO_EVAL_IndicadoresPsicosomaticosStres.Text = interpretacion.Text;
            }
            else if (data.InterpretacionCtrlId == Constants.txt_TestIndicativoFatigaLaboral_Interpretacion)
            {
                txt_RESULTADO_EVAL_IndicadoresFatigaLaboral.Text = interpretacion.Text;
            }
            else if (data.InterpretacionCtrlId == Constants.txt_CuestionarioEstresOrganizacional_interpretacion)
            {
                if (data.Category == "Bajo nivel de estrés (4)")
                {
                    cb_RESULTADO_EVAL_NivelStres.SelectedValue = ((int)Result_Eval_Nivel_Estres.Bajo).ToString();
                }
                else if (data.Category == "Nivel intermedio(3)")
                {
                    cb_RESULTADO_EVAL_NivelStres.SelectedValue = ((int)Result_Eval_Nivel_Estres.Intermedio).ToString();
                }
                else if (data.Category == "Estrés(2)")
                {
                    cb_RESULTADO_EVAL_NivelStres.SelectedValue = ((int)Result_Eval_Nivel_Estres.Estres).ToString();
                }
                else if (data.Category == "Alto nivel de estrés(1)")
                {
                    cb_RESULTADO_EVAL_NivelStres.SelectedValue = ((int)Result_Eval_Nivel_Estres.Alto).ToString();
                }
            }
           
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
                        //if (item.v_ComponentFieldId == Constants.txt_RESULTADO_EVAL_IndicadoresPsicosomaticosStres)
                        //{
                        //    var ss = item.v_Value1;
                        //}
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
                    || item.v_ComponentFieldId == Constants.cb_RESULTADO_EVAL_EstabilidadEmocional
                    || item.v_ComponentFieldId == Constants.cb_RESULTADO_EVAL_Personalidad
                    || item.v_ComponentFieldId == Constants.cb_RESULTADO_EVAL_Afectividad
                    || item.v_ComponentFieldId == Constants.cb_RESULTADO_EVAL_Motivacion
                    || item.v_ComponentFieldId == Constants.cb_RESULTADO_EVAL_NivelStres)
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
