using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class ReporteExcelCovid19
    {
        public DateTime FechaExamen { get; set; }
        public string CentroMedico { get; set; }
        public string Unidad { get; set; }
        public string EmpresaEmpleadora { get; set; }
        public string NombresApellidos { get; set; }
        public string Dni { get; set; }
        public string Edad { get; set; }
        public string Sintomas { get; set; }
        public string ResultadoIgM { get; set; }
        public string ResultadoIgG { get; set; }
        public string Resultado { get; set; }
        public string Aptitud { get; set; }
        public string Motivo { get; set; }
        public string Tecnico { get; set; }
        public string Celular { get; set; }
        public string ValorExamen { get; set; }

        public DateTime FechaNacimiento { get; set; }
    }

    public class ReporteExcelCovid19JessicaOblitas
    {
        public string ServicioId { get; set; }
        public string Nodo { get; set; }
        public DateTime FechaExamen { get; set; }
        public string CentroMedico { get; set; }
        public string EmpresaPrincipal { get; set; }
        public string EmpresaEmpleadora { get; set; }
        public string EmpresaContratista { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string TipoEmpresa{ get; set; }
        public string Area { get; set; }
        public string Puesto { get; set; }
        public string Dni { get; set; }
        public string Sede { get; set; }
        public string Edad { get; set; }
        public string Sexo { get; set; }
        public string Sintomas { get; set; }
        public string ResultadoIgM { get; set; }
        public string ResultadoIgG { get; set; }
        public string Resultado { get; set; }     
        public string Tecnico { get; set; }
        public string Celular { get; set; }     
        public DateTime? FechaNacimiento { get; set; }
        public string AntecedenteResultado { get; set; }
        public DateTime? AntecedenteFechaResultado { get; set; }
        
        public string TipoExamen { get; set; }
    }

    public class ReporteExcelAutomaticoCovid19
    {
        public string ServicioId { get; set; }
        public string Nodo { get; set; }
        public DateTime FechaExamen { get; set; }
        public string CentroMedico { get; set; }
        public string EmpresaPrincipal { get; set; }
        public string EmpresaEmpleadora { get; set; }
        public string EmpresaContratista { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string TipoEmpresa { get; set; }
        public string Area { get; set; }
        public string Puesto { get; set; }
        public string Dni { get; set; }
        public string Sede { get; set; }
        public string Edad { get; set; }
        public string Sexo { get; set; }
        public string Sintomas { get; set; }
        public string ResultadoIgM { get; set; }
        public string ResultadoIgG { get; set; }
        public string Resultado { get; set; }
        public string Tecnico { get; set; }
        public string Celular { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string AntecedenteResultado { get; set; }
        public DateTime? AntecedenteFechaResultado { get; set; }

        public string TipoExamen { get; set; }
        public string RazonExamen { get; set; }
        public string LugarExamen { get; set; }

    }
    public class ReporteViewModel
    {
        public string Tipo { get; set; }
        public string Centro_medico { get; set; }
        public string Ciudad { get; set; }
        public string Fecha { get; set; }
        public string Empleador { get; set; }
        public string Empresa_principal { get; set; }
        public string Sede { get; set; }
        public string Tipo_documento { get; set; }
        public string Nro_documento { get; set; }
        public string Apellido_paterno { get; set; }

        public string ApellidoMaterno { get; set; }
        public string Nombres { get; set; }
        public string Edad { get; set; }
        public string Sexo { get; set; }
        public string Celular { get; set; }
        public string Otro_telefono { get; set; }
        public string Domicilio_residencia { get; set; }
        public string Direccion_donde_reside { get; set; }
        public string Departamento_Provincia_Distrito { get; set; }
        public string Ha_viajado_fuera_del_pais { get; set; }

        public string Ha_tenido_usted_un_contacto_directo_con_algun_caso_CONFIRMADO_COVID19 { get; set; }
        public string Ha_visitado_algun_establecimiento_de_salud { get; set; }
        public string Esta_tomando_alguna_medicacion { get; set; }
        public string Si_la_respuesta_es_SI_especifique { get; set; }
        public string Mayor_65 { get; set; }
        public string Diabetes { get; set; }
        public string Enfermedad_pulmonar_cronica { get; set; }
        public string Cancer { get; set; }
        public string Hipertension_arterial { get; set; }
        public string Obesidad_IMC_40 { get; set; }

        public string Insuficiencia_renal_cronica { get; set; }
        public string Embarazo_o_puerperio { get; set; }
        public string Enf_cardiovascular { get; set; }
        public string Asma { get; set; }
        public string Enf_o_tratamiento_inmunosupresor { get; set; }
        public string Condicion_Personal_de_salud { get; set; }
        public string Pregunta_Es_personal_de_salud { get; set; }
        public string Cual_es_su_profesion { get; set; }
        public string Tiene_sintomas { get; set; }
        public string Fecha_inicio_de_sintomas { get; set; }

        public string Tos { get; set; }
        public string Fiebre_Escalofrio { get; set; }
        public string Cefalea { get; set; }
        public string Dolor_de_garganta { get; set; }
        public string Malestar_general { get; set; }
        public string Irritacion_Confusion { get; set; }
        public string Congestion_nasal { get; set; }
        public string Diarrea { get; set; }
        public string Dolor { get; set; }
        public string Dificultad_respiratoria { get; set; }

        public string Nauseas_Vomitos { get; set; }
        public string Expectoracion { get; set; }
        public string Dolor_muscular { get; set; }
        public string Dolor_abdominal { get; set; }
        public string Dolor_pecho { get; set; }
        public string Dolor_articulaciones { get; set; }
        public string Otros_sintomas_especificar { get; set; }
        public string Clasificacion_clinica_de_severidad { get; set; }
        public string Temperatura { get; set; }
        public string Peso { get; set; }

        public string Talla { get; set; }
        public string IMC { get; set; }
        public string Procedencia_Solicitud { get; set; }
        public string Resultado_prueba_rapida { get; set; }

        public string Resultado_de_segunda_prueba_rapida { get; set; }
        public string Profesional_quien_realiza_la_prueba { get; set; }
        public string Aptitud { get; set; }

    }
}
