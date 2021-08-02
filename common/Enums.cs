using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Sigesoft.Common
{
    public enum OdoContext
    {
        None = 0,
        FillInternalData = 1,
        FillFromESO = 2,
        ClearControls = 3,
        SearchControlAndFill = 4
    }

    public enum SystemUserTypeId
    {
        Internal = 1,
        External = 2
    }

    public enum TipoProfesional
    {
        Evaluador = 30,
        Auditor = 6
    }
    public enum TypePrinter
    {
        Image = 1,
        Printer = 2
    }

    public enum StatusProtocol
    {
        Aprobado = 1,
        Pendiente = 2
    }

    public enum StatusBlackListPerson
    {
        Registrado = 1,
        Detectado = 2,
        Eliminado = 3
    }

    public enum LogEventType
    {
        ACCESOSSISTEMA = 1,
        CREACION = 2,
        ACTUALIZACION = 3,
        ELIMINACION = 4,
        EXPORTACION = 5,
        GENERACIONREPORTE = 6,
        PROCESO = 7
    }

  
    public enum Success
    {
        Failed = 0,
        Ok = 1
    }

    public enum TypeForm
    {
        Windows = 1,
        Web = 2
    }  
    
    public enum Document
    {
        DNI = 1,
        PASAPORTE = 2,
        LICENCIACONDUCIR = 3,
        CARNETEXTRANJ = 4
    }

    public enum TypeFamily
    {
        PADRE = 4,
        MADRE = 7,
        HERMANOS = 20,
        ABUELOS = 15,

        PADRE_OK = 1,
        MADRE_OK = 2,
        HERMANOS_OK = 3,
        ABUELOS_OK = 13
    }


    public enum ModeOperation
    {
        Total =1,
        Parcial = 2
    }


    public enum DropDownListAction
    {
        All,
        Select
    }

    public enum Scope
    {
        Global = 1,
        Contextual = 2     
    }

    public enum MasterService
    {
        Eso = 2,    
        ConsultaInformatica = 4,
        ConsultaMedica = 3,
        ConsultaNutricional = 6,
        ConsultaPsicológica = 7,
        ProcEnfermería = 8,
        AtxMedicaParticular = 10
    }

    public enum MotiveType
    {
        IngresoCompra = 1,
        IngresoCargaInicial = 2,
        EgresoAtencion = 11
    }

    public enum MovementType
    {
        INGRESO = 1,
        EGRESO = 2
    }

    public enum AptitudeStatus
    {
        Apto = 2,
        NoApto = 3,
        AptoObs = 4,
        AptRestriccion = 5,
        SinAptitud = 1,
        Pendiente =6
    }
    public enum RecordType
    {
        /// <summary>
        /// El registro tiene un ID [GUID]
        /// </summary>
        Temporal = 1,
        /// <summary>
        /// El registro Tiene in ID de Base de Datos
        /// </summary>
        NoTemporal = 2
    }
    public enum RecordStatus
    {
        Grabado = 0,
        Modificado = 1,
        Agregado = 2,
        EliminadoLogico = 3       
    }

    public enum CalendarStatus
    {        
        Agendado = 1,
        Atendido = 2,
        Vencido = 3,
        Cancelado = 4,
        Ingreso = 5
    }

    public enum ServiceStatus
    {
        PorIniciar = 1,
        Iniciado = 2,
        Culminado = 3,
        Incompleto = 4,
        Cancelado = 5,
        EsperandoAptitud = 6
    }

    public enum Flag_Call
    {
        NoseLlamo =0,
        Sellamo =1
    }

    public enum ServiceComponentStatus
    {
        PorIniciar = 1,
        Iniciado = 2,
        Culminado = 3,
        PorReevaluar = 4,
        NoRealizado = 5,
        PorAprobacion = 6,
    }


    public enum ServiceOrderStatus
    {
        Iniciado = 1,
        Concluido = 2,
        Observado = 3,
        Conforme = 4,
    }

    public enum LineStatus
    {
        EnCircuito = 1,
        FueraCircuito = 2,
    }


    public enum QueueStatusId
    {
        LIBRE = 1,
        LLAMANDO = 2,
        OCUPADO = 3
    }

    public enum TypeHabit
    {
        Tabaco = 1,
        Alcohol=2,
        Drogas = 3,
        ActividadFisica = 4
    }

    public enum modality
    {
        NuevoServicio = 1,
        ContinuacionServicio = 2,
    }

    public enum SiNo
    {
        NO = 0,
        SI = 1,
        NONE = 2
    }

    public enum TypeOfInsurance
    {
        ESSALUD =1,
        EPS =2,
        OTRO =3
    }

    public enum ProcessType
    {
        LOCAL =1,
        REMOTO = 2
    }

 
    public enum Gender
    {
        MASCULINO = 1,
        FEMENINO = 2
    }


    public enum GenderConditional
    {
        MASCULINO = 1,
        FEMENINO = 2,
        AMBOS = 3
    }



    public enum ControlType
    {
        CadenaTextual = 1,
        CadenaMultilinea = 2,
        NumeroEntero = 3,
        NumeroDecimal = 4,
        SiNoCheck = 5,
        SiNoRadioButton = 6,
        SiNoCombo = 7,
        UcFileUpload = 8,
        Lista = 9,
        UcAudiometria = 10,
        UcOdontograma = 11,
        ucEspirometria = 12,
        ucOftalmologia = 13,
        ucPsicologia = 14,
        ucTestFobias = 15,
        ucTestAcrofobia = 16,
        uc_EPWORTH_Testsomnolencia = 17,
        ucMiniTestPsiquiatrico = 18,
        ucTestFatigaLaboral = 19,
        uc_BECK_TestRasgosDepresivos = 20,
        uc_SCL_90_EstresAnsiedadDepresion = 21,
        uc_ZUNG_EscalaAutoEvaluacionDepresion = 22,
        ucRadiografiaOIT = 23,

        UcSomnolencia = 33,
        UcAcumetria = 34,
        UcSintomaticoRespi = 35,
        UcRxLumboSacra = 36,
        UcOtoscopia = 37,
        UcEvaluacionErgonomica = 38,
        UcOjoSeco = 39,
        UcOsteoMuscular = 40,
        UcBoton = 41
    }

    public enum ComponentType
    {
        Examen = 1,
        NoExamen = 2       
    }

    public enum ServiceType
    {
        Empresarial = 1,
        Particular = 9,
        Preventivo = 11
    }

    public enum AutoManual
    {
        Automático = 1,
        Manual = 2
    }

    public enum TypeESO
    {
        PreOcupacional = 1,
        PeriodicoAnual = 2,
        Retiro= 3,
        Preventivo = 4,
        Reubicacion = 5,
        Chequeo = 6
    }

    public enum PreQualification
    {
        SinPreCalificar = 1,
        Aceptado = 2,
        Rechazado = 3

    }

    public enum FinalQualification
    {
        SinCalificar = 1,
        Definitivo = 2,
        Presuntivo = 3,
        Descartado = 4

    }

    public enum LugarTrabajo
    {
        Superfice = 1,
        Concentradora = 2,
        Subsuelo = 3

    }

    public enum OperatorValue1
    {
        IGUAL = 1,
        DIFERENTE = 2,
        MENOR = 3,
        MENORIGUAL = 4,
        MAYOR = 5,
        MAYORIGUAL = 6,
        D18_1 = 4
    }

    public enum Operator2Values
    {
        X_esIgualque_A = 1,
        X_noesIgualque_A = 2,
        X_esMenorque_A = 3,
        X_esMenorIgualque_A = 4,
        X_esMayorque_A = 5,
        X_esMayorIgualque_A = 6,
        X_esMayorque_A_yMenorque_B = 7,
        X_esMayorque_A_yMenorIgualque_B = 8,
        X_esMayorIgualque_A_yMenorque_B = 9,
        X_esMayorIgualque_A_yMenorIgualque_B = 12,
    }

    public enum ComponenteProcedencia
    {
        Interno = 1,
        Externo = 2
    }

    public enum Typifying
    {
        Recomendaciones = 1,
        Restricciones = 2
    }

    public enum ExternalUserFunctionalityType
    {
        PermisosOpcionesUsuarioExternoWeb = 1,
        NotificacionesUsuarioExternoWeb = 2
    }

    public enum FileExtension
    {
        JPG,
        GIF,
        JPEG,
        PNG,
        BMP,
        XLS,
        XLSX,
        DOC,
        DOCX,
        PDF,
        PPT,
        PPTX,
        TXT,
        AVI,
        MPG,
        MPEG,
        MOV,
        WMV,
        FLV,
        MP3,
        MP4,
        WMA,
        WAV
    }

    /// <summary>
    ///  Especifica una acción.
    /// </summary>
    public enum ActionForm
    {
        None = 0,
        Add = 1,
        Edit = 2,
        Delete = 3,
        Upload = 4,
        Browse = 5,
        Cancel = 6
    }


    public enum NormalAlterado
    { 
        Normal = 1,
        Alterado = 2,
        NoSeRealizo = 3
    }

    public enum Altitud
    {
        Debajo2500 = 1,
        Entre2501a3000 = 2,
        Entre3001a3500 = 3,
        Entre3501a4000 = 4,
        Entre4001a4500 = 5,
        Mas4501 = 6
    }

    public enum EstadoCivil
    {
        Soltero = 1,
        Casado = 2,
        Viudo = 3,
        Divorciado = 4,
        Conviviente = 5
    }

    public enum NivelEducacion
    {
        Analfabeto = 1,
        PIncompleta = 2,
        PCompleta = 3,
        SIncompleta = 4,
        SCompleta = 5,
        Tecnico = 6,
        Universitario = 7
    }

    public enum TipoOcurrencia
    {
        Accidente = 1,
        Enfermedad = 2,
        NoDefine = 3
    }

    public enum TipoDx
    {
        Enfermedad_crónica_degenerativa = 1,
        Enfermedad_Ocupacional = 2,
        Accidente_Común = 3,
        Accidente_Ocupacional = 4,
        Enfermedad_crónica = 5,
        Enfermedad_Aguda = 6,
        SinDx = 7
    }


    public enum OrigenOcurrencia
    {
        Comun = 1,
        Laboral = 2,
        NoDefine = 3
    }

    public enum NormalAlteradoHallazgo
    {
        SinHallazgos =1,
        ConHallazgos=2,
        NoseRealizo = 3
    }

    public enum AumentadoDisminuidoConservado
    {
        Aumentado = 3,
        Disminuido = 2,
        Conservado = 1
    }

    public enum SystemParameterGroups
    {
        ConHallazgoSinHallazgosNoSeRealizo = 135,
        AlturaLabor = 176,
        TiempoExpsosicionRuidoSalus = 234,
        TiempoExpsosicionRuido = 235,
        NivelRuido = 236,
        OftalmologiaMedidas = 237,
        OftalmologiaMedidasCerca = 262,
        NormalAlterado = 238,
        ColoresBasicos = 239,
        EscalaDolor = 240,
        SiNoNoDefine = 241,
        HabitosNocivos = 148,
        GrupoOcupacional_EMPO_PSICOLOGIA = 242,
        GrupoOcupacional_EMOA_PSICOLOGIA = 243,
        GrupoOcupacional_EMOR_PSICOLOGIA = 244,
        DISC_Combinaciones_PSICOLOGIA = 245,
        ResultadoEvaluacion_EstabilidadEmocional = 255,
        ResultadoEvaluacion_NivelEstres = 256,
        ResultadoEvaluacion_Personalidad = 257,
        ResultadoEvaluacion_Afectividad = 258,
        ResultadoEvaluacion_Motivacion = 259,
        ResultadoEvaluacion_NIVEL_EMPO = 260,
    }

    public enum SiNoNoDefine
    {
        Si = 1,
        No = 2,
        NoDefine = 3     
    }

    public enum AppHierarchyType
    {
        AgrupadorDeMenu = 1, 
        PantallaOpcionDeMenu = 2,
        AccionDePantalla = 3,  
        PantallaOpcionIndependiente = 4
    }

    public enum PeligrosEnElPuesto
    {
        Fisicos = 1,
        Quimicos = 2,
        Ergonomicos = 3,
        Biologicos = 4,
        Ruido = 5,

    }

    public enum TipoEPP
    {
        TaponesAuditivosEspuma = 1,
        TaponesAuditivosSilicona = 2,
        Orejeras = 3,     

    }

    public enum CategoryTypeExam
    {
        Laboratorio = 1,
        odontologia = 2,
        Neonatal = 3,
        Ginecologia = 4,
        Cardiologia = 5,
        Rx = 6,
        Psicologia = 7,
        Triaje = 10,
        ExamenFisico = 11,
    }

    public enum Consultorio
    {
        Laboratorio = 1,
        Odontologia = 2,
        Neonatal = 3,
        Ginecología = 4,
        Cardiología = 5,
        RayosX = 6,
        Psicología = 7,
        Triaje = 10,
        Medicina = 11,
        GinecologíaExAuxiliares  = 12,
        Inmunizaciones = 13,

    }

    public enum VisionLejos
    {
        _20_20 = 1,
        _20_25 = 2,
        _20_30 = 3,
        _20_40 = 4,
        _20_50 = 5,
        _20_60 = 6,
        _20_80 = 7,
        _20_100 = 8,
        _20_140 = 9,
        _20_200 = 10,
        _20_400 = 11,
        _CD3M = 12,
        _CD1M = 13,
        _CD03M = 14,
        _MM=15,
        _PL=16,
        _NPL=17,
        _raya=18,
    }
    public enum VisionCerca
    {
        _20_20 = 1,
        _20_30 = 2,
        _20_40 = 3,
        _20_50 = 4,
        _20_60 = 5,
        _20_70 = 6,
        _20_80 = 7,
        _20_100 = 8,
        _20_160 = 9,
        _20_200 = 10,
    }

    public enum GrupoOcupacional_EMPO_Psicologia
    {
        ConductoresOperadoresEquipoPesado = 1,
        Operarios = 2,
        Tecnicos = 3,
        Universitarios = 4
    }

    public enum GrupoOcupacional_EMOA_Psicologia
    {
        OperariosYTecnicos = 1,
        Conductores = 2,
        Universitarios = 3
    }

    public enum GrupoOcupacional_EMOR_Psicologia
    {
        Operarios = 1,
        Tecnicos = 2,
        Universitarios = 3
    }

    public enum DISC_Combinaciones_Psicologia
    {
        DI = 1,     
        DC = 2,
        ID = 3,
        IS = 4,
        IC = 5,
        SD = 6,
        SI = 7,
        SC = 8,
        CD = 9,
        CI = 10,
        CS = 11,
        D_IGUAL_C = 12,
        DS = 13,
    }

    public enum Result_Eval_Nivel_Estres
    {
        Bajo  = 1,
	    Intermedio = 2,
	    Estres = 3,
        Alto = 4,
    }

    public enum PositivoNegativoNoSeRealizo
    {
        NoSeRealizo = 1,
        Positivo = 2,
        Negativo = 3,
    }

    //
    //

    public enum SedeBackus
    {
        ConoNorte = 100,
        Rimac = 101,
        Ate = 102,
        ConoSur = 103,
        Callao = 104,
        Vegueta = 105,
        TingoMaria = 106,
        Nazca = 107,

        Trujillo = 110,
        Chimbote = 111,
        Piura = 112,
        Tumbes = 113,
        Talara = 114,
        Huanuco = 116,
        Chanchamayo = 117,
        Ica = 118,
        Canete = 120,
        satipo = 121,
        OtrasClinicas = 151,
        Motupe = 200,
        Rom = 201,
        Ilo = 202,
        Administrativa = 203,

        PLANTA_PUCALLPA = 160,
        PLANTA_CUSCO = 161,
        PLANTA_ATE = 162,
        PLANTA_AREQUIPA = 163,
        PLANTA_MOTUPE = 164,
        PLANTA_SAN_MATEO = 165,
        PLANTA_MALTERIA = 166,
        PLANTA_HUACHIPA = 167,
        CD_HUARAZ = 168,
        TERCEROS  = 169,
        CD_JULIACA = 170,
        CD_SUR_CHICO = 171,
        CD_TARAPOTO = 172,
    }

    public enum ClinicasProvinciasExternas
    {
        SIGSO_Arequipa = 130,
        SIGSO_Camana = 131,
        OXIMEDIC_Juliaca = 132,
        SANPEDROAPOSTOL_Tacna = 133,
        SANPEDROAPOSTOL_Ilo = 134,
        CAYETANOHEREDIA_Huancayo = 135,
        SANTAANITA_Iquitos = 136,
        PARDO_Cuzco = 137,
        JUANPABLOII_Pucalpa = 138,
        INVBIOMEDIC_Cajamarca = 139,
        SALUSLABORIS_Chiclayo = 140,
        SALUSLABORIS_Motupe = 141,
        MEDICALCENTER_Huaraz = 142,
        ELNAZARENO_Ayacucho = 143,
        SANMARTIN_Tarapoto = 144,
        MIRAFLORES_Piura = 145,
        VIDAHUANCAVELICA_Huancavelica = 146,
        DANIELALCIDESCARRION_Moyobamba = 147,
        NORTHLAB_Tumbes = 148,
        DANIELALCIDESCARRION_Yurimaguas = 149,
        HUANBUCO_Huanuco = 152,
        EUROCLINIC_Trujillo = 150,
        
    }

    public enum SedeSalusLaboris
    {
        Chiclayo = 108,
    }

    public enum TipoFormatoCovid19
    {
        Detallado =1,
        Corto =2
    }

    #region Estados de conexion a Internet

    //[Flags]
    //public enum ConnectionState : int
    //{
    //    INTERNET_CONNECTION_MODEM = 0x1,
    //    INTERNET_CONNECTION_LAN = 0x2,
    //    INTERNET_CONNECTION_PROXY = 0x4,
    //    INTERNET_RAS_INSTALLED = 0x10,
    //    INTERNET_CONNECTION_OFFLINE = 0x20,
    //    INTERNET_CONNECTION_CONFIGURED = 0x40
    //}

    public enum ColorDiente
    {
        White = 1,
        Red = 2,
        Blue = 3
    }

    public enum LeyendaOdontograma
    {
        Ausente =1,
        Exodoncia = 2,
        //Obturacion = 3,
        Corona = 4,
        PuenteIni = 5,
        PPR = 6,
        ProtesisTotal =7,
        Implante = 8,
        AparatoOrtodontico = 9,
        MaterialProvisional = 10,
        CarieSevera=11,
        CoronaProvisional=12,
        CoronaMalAdaptada=13,
        PeriodontitisM1=14,
        PeriodontitisM2 = 15,
        PeriodontitisM3 = 16,
        PuenteFin=17,
        ImplanteCorona=18,
        AusenteCorona=19
    }

    public enum ResultKlockloff
    {
        MIXTA = 1,
        NS = 2,
        COND = 3,
        ND = 4,
        NORM = 5
    }

    public enum PreLiquidationStatus
    {
        Pendiente = 1,
        Generada = 2
      
    }

    public enum ModoCargaImagen
    {
        DesdeArchivo = 1,
        DesdeDispositivo = 2

    }

    #endregion    
    
    
}
