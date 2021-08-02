using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class ReportEstudioElectrocardiografico
    {

        public DateTime FechaNacimiento { get; set; }

        public string NroFicha{get;set;}

        public DateTime? Fecha{get;set;}

        public string Empresa{get;set;}

        public string NroHistoria{get;set;}

        public string DatosPaciente{get;set;}

        public string Puesto{get;set;}

        public int Edad{get;set;}

        public string Genero{get;set;}

        public string SoploSiNo{get;set;}

        public string CansancioSiNo{get;set;}

        public string MareosSiNo{get;set;}

        public string PresionAltaSiNo{get;set;}

        public string DolorPrecordialSiNo{get;set;}

        public string PalpitacionesSiNo{get;set;}

        public string AtaquesCorazonSiNo{get;set;}

        public string PerdidaConcienciaSiNo{get;set;}

        public string ObesidadSiNo{get;set;}

        public string TabaquismoSiNo{get;set;}

        public string DisplidemiaSiNo{get;set;}

        public string DiabetesSiNo{get;set;}

        public string SedentarismoSiNo{get;set;}

        public string Otros1{get;set;}

        public string DolorPrecordial2SiNo{get;set;}

        public string DesmayosSiNo{get;set;}

        public string Palpitaciones2SiNo{get;set;}

        public string DisneaSiNo{get;set;}

        public string Otros2{get;set;}

        public string Mareos2SiNo{get;set;}

        public string VaricesSiNo{get;set;}

        public string ClaudicacSiNo{get;set;}

        public string Ritmo{get;set;}

        public string IntervaloPR{get;set;}

        public string IntervaloQRS{get;set;}

        public string IntervaloQT{get;set;}

        public string OndaP{get;set;}

        public string OndaPAnormal { get; set; }

        public string OndaT{get;set;}

        public string OndaTAnormal { get; set; }

        public string ComplejoQRS{get;set;}

        public string ComplejoQRSAnormal { get; set; }

        public string SegmentoST{get;set;}

        public string SegementoSTAnormal { get; set; }

        public string TrasntornoRitmo{get;set;}

        public string TranstornoConduccion{get;set;}

        public string Conclusiones{get;set;}

        public string Hallazgos{get;set;}

        public string Recomendaciones{get;set;}

        public byte[] FirmaTecnico{get;set;}

        public byte[] FirmaMedico { get; set; }

        public string NombreDoctor { get; set; }

        public string NombreTecnologo { get; set; }

        public byte[] b_Logo { get; set; }
        public string EmpresaPropietaria { get; set; }
        public string EmpresaPropietariaDireccion { get; set; }
        public string EmpresaPropietariaTelefono { get; set; }
        public string EmpresaPropietariaEmail { get; set; }

        public string ELECTROCARDIOGRAMA_ANTECEDENTES_IMAS_ID { get; set; }
        public string ELECTROCARDIOGRAMA_SINTOMAS_ASINTOMATICO_ID { get; set; }
        public string ELECTROCARDIOGRAMA_SINTOMAS_DISNEA_PAROXISTICA_ID { get; set; }

        public string ELECTROCARDIOGRAMA_EXAMEN_FISICO_PREF_PAS_ID { get; set; }
        public string ELECTROCARDIOGRAMA_EXAMEN_FISICO_PREF_PAD_ID { get; set; }
        public string ELECTROCARDIOGRAMA_EXAMEN_FISICO_PREF_EXA_CORAZON_ID { get; set; }
        public string ELECTROCARDIOGRAMA_EXAMEN_FISICO_PREF_OTROS_HALLAZGOS_ID { get; set; }

        public string ELECTROCARDIOGRAMA_ANTECEDENTES_OTROS_ID { get; set; }
        public string ELECTROCARDIOGRAMA_SINTOMAS_OTROS_ID { get; set; }

        public string Dx { get; set; }
        public string Restriction { get; set; }
        public string Recomendation { get; set; }

        // Signos
        public string ELECTROCARDIOGRAMA_SIGNOS_INTER_ECG_RITMO_ID { get; set; }
        public string ELECTROCARDIOGRAMA_SIGNOS_INTER_ECG_INTERVALO_PR_ID { get; set; }
        public string ELECTROCARDIOGRAMA_SIGNOS_INTER_ECG_INTERVALO_QRS_ID { get; set; }
        public string ELECTROCARDIOGRAMA_SIGNOS_INTER_ECG_INTERVALO_QT_ID { get; set; }
        public string ELECTROCARDIOGRAMA_SIGNOS_INTER_ECG_ONDA_P_ID { get; set; }
        public string ELECTROCARDIOGRAMA_SIGNOS_INTER_ECG_ONDA_Q_ID { get; set; }
        public string ELECTROCARDIOGRAMA_SIGNOS_INTER_ECG_ONDA_R_ID { get; set; }
        public string ELECTROCARDIOGRAMA_SIGNOS_INTER_ECG_ONDA_S_ID { get; set; }
        public string ELECTROCARDIOGRAMA_SIGNOS_INTER_ECG_ONDA_T_ID { get; set; }
        public string ELECTROCARDIOGRAMA_SIGNOS_INTER_ECG_ONDA_U_ID { get; set; }
        public string ELECTROCARDIOGRAMA_SIGNOS_INTER_ECG_SEGMENTO_ST_ID { get; set; }
        public string ELECTROCARDIOGRAMA_SIGNOS_INTER_ECG_EJE_QRS_ID { get; set; }
        public string ELECTROCARDIOGRAMA_SIGNOS_INTER_ECG_FC_ID { get; set; }

        public string ELECTROCARDIOGRAMA_DESCRIPCION_INTERPRETACION_ID { get; set; }
        public string ELECTROCARDIOGRAMA_PACIENTE_ENCUENTRA_APTO_TRAB_FORZADO_ID { get; set; }
        public string ELECTROCARDIOGRAMA_PACIENTE_ENCUENTRA_APTO_TRAB_ALTURA_MAY_2500_ID { get; set; }

        public string ELECTROCARDIOGRAMA_SINTOMAS_LIPOTIMIAS_ID { get; set; }
        public byte[] LogoEmpresaCliente { get; set; }
        public string RazonSocialEmpresaCliente { get; set; }
    }
}
