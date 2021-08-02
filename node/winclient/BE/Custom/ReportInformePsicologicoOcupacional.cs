using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class ReportInformePsicologicoOcupacional
    {
        public string v_PersonId { get; set; }
        public string v_FullPersonName { get; set; }
        public string v_DocNumber { get; set; }
        public int? i_SexTypeId { get; set; }
        public string v_SexType { get; set; }
        public DateTime? d_BirthDate { get; set; }
        public DateTime? d_ServiceDate { get; set; }
        public string Puesto { get; set; }
        

        public byte[] FirmaTrabajador { get; set; }
        public byte[] HuellaTrabajador { get; set; }
        public byte[] FirmaMedico { get; set; }
        public byte[] FirmaTecnologo { get; set; }
        public int i_AgePacient { get; set; }
        public byte[] b_Logo { get; set; }
        public string v_WorkingOrganizationName { get; set; }
        public string Protocolo { get; set; }
        public int? i_EsoTypeId { get; set; }
        public string v_EsoTypeName { get; set; }
        public string v_ServiceComponentId { get; set; }
        public string v_ServiceId { get; set; }
        public string EmpresaPropietaria { get; set; }
        public string EmpresaPropietariaDireccion { get; set; }
        public string EmpresaPropietariaTelefono { get; set; }
        public string EmpresaPropietariaEmail { get; set; }
        public string Hallazgos { get; set; }

        // (campos dinamicos)
        public string EXAMEN_PSICOLOGICO_Obs_conductas_Articulación { get; set; }
        public string EXAMEN_PSICOLOGICO_Obs_conductas_Espacio { get; set; }
        public string EXAMEN_PSICOLOGICO_Obs_conductas_Persona { get; set; }
        public string EXAMEN_PSICOLOGICO_Obs_conductas_Postura { get; set; }
        public string EXAMEN_PSICOLOGICO_Obs_conductas_Presentacion { get; set; }
        public string EXAMEN_PSICOLOGICO_Obs_conductas_Ritmo { get; set; }
        public string EXAMEN_PSICOLOGICO_Obs_conductas_Tiempo { get; set; }
        public string EXAMEN_PSICOLOGICO_Obs_conductas_Tono { get; set; }
        public string EXAMEN_PSICOLOGICO_Enfe_psicoso_Apetito_Bulimia_anorexia_otros { get; set; }
        public string EXAMEN_PSICOLOGICO_Enfe_psicoso_Cardiovasculares_palpitaciones_cefale_otros { get; set; }
        public string EXAMEN_PSICOLOGICO_Enfe_psicoso_Gastrointestinales_Sequedad_de_boca_gastritis_ulcera_otros { get; set; }
        public string EXAMEN_PSICOLOGICO_Enfe_psicoso_Observaciones { get; set; }
        public string EXAMEN_PSICOLOGICO_Enfe_psicoso_Respiratorios_Asma_hiperventilación_suspiros_otros { get; set; }
        public string EXAMEN_PSICOLOGICO_Enfe_psicoso_Sudoracion { get; set; }
        public string EXAMEN_PSICOLOGICO_Enfe_psicoso_Sueño_insomnio { get; set; }
        public string EXAMEN_PSICOLOGICO_Enfe_psicoso_Tics_nerviosos { get; set; }
        public string EXAMEN_PSICOLOGICO_His_fami_Esposo { get; set; }
        public string EXAMEN_PSICOLOGICO_His_fami_Hermanos { get; set; }
        public string EXAMEN_PSICOLOGICO_His_fami_Hijo { get; set; }
        public string EXAMEN_PSICOLOGICO_His_fami_Otros { get; set; }
        public string EXAMEN_PSICOLOGICO_His_fami_Padres { get; set; }
        public string EXAMEN_PSICOLOGICO_Habitos_Alcohol { get; set; }
        public string EXAMEN_PSICOLOGICO_Habitos_Pasatiempo { get; set; }
        public string EXAMEN_PSICOLOGICO_Habitos_Tabaco { get; set; }
        public string EXAMEN_PSICOLOGICO_Conclu_final_Aptitud { get; set; }
        public string EXAMEN_PSICOLOGICO_Conclu_final_Riesgo { get; set; }



        // Campos UC
        public string cb_GrupoOcupacional { get; set; }

        // EMOA
        public string cb_RESULTADO_EVAL_EstabilidadEmocional_EMOA { get; set; }
        public string cb_RESULTADO_EVAL_Personalidad { get; set; }
        public string cb_RESULTADO_EVAL_Afectividad { get; set; }
        public string cb_RESULTADO_EVAL_Motivacion { get; set; }
        public string cb_RESULTADO_EVAL_NivelStres { get; set; }

        public string txt_RESULTADO_EVAL_IndicadoresPsicosomaticosStres { get; set; }
        public string txt_RESULTADO_EVAL_IndicadoresFatigaLaboral { get; set; }      
        public string txt_RESULTADO_EVAL_IndicadoresFobia { get; set; }
        public string txt_RESULTADO_EVAL_IndicadoresFatigaySomnolencia { get; set; }

        // EMPO
        public string cb_RESULTADO_EVAL_EstabilidadEmocional_EMPO { get; set; }
        public string cb_RESULTADO_EVAL_Capacidad { get; set; }
        public string cb_RESULTADO_EVAL_JuicioSentidoComun { get; set; }
        public string cb_RESULTADO_EVAL_CoordinacionVisoMotriz { get; set; }
        public string cb_RESULTADO_EVAL_PlanificacionyOrganizacion { get; set; }
        public string cb_RESULTADO_EVAL_PercepcionFrenteSeguridad { get; set; }
        public string cb_RESULTADO_EVAL_MotivacionHaciaTrabajo { get; set; }
        public string cb_RESULTADO_EVAL_ControlImpulsos { get; set; }
        public string cb_RESULTADO_EVAL_RelacionesInterpersonales { get; set; }
        public string cb_RESULTADO_EVAL_ManejoPresionyEstres { get; set; }

        public string cb_GrupoOcupacionalName { get; set; }

        public bool EMOA_OperativosyTecnicos { get; set; }
      
       

    }
}
