using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class AudiometriaList
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

        // Requisitos para la audiometria

        public string CambiosAltitud { get; set; }
        public string ExpuestoRuido { get; set; }
        public string ProcesoInfeccioso { get; set; }
        public string DurmioNochePrevia { get; set; }
        public string ConsumioAlcoholDiaPrevio { get; set; }
      

        // Antecedentes Medicos de importancia

        public string RinitisSinusitis { get; set; }
        public string UsoMedicamentos { get; set; }
        public string Sarampion { get; set; }
        public string Tec { get; set; }
        public string OtitisMediaCronica { get; set; }
        public string DiabetesMellitus { get; set; }
        public string SorderaAntecedente { get; set; }
        public string SorderaFamiliar { get; set; }
        public string Meningitis { get; set; }
        public string Dislipidemia { get; set; }
        public string EnfTiroidea { get; set; }
        public string SustQuimicas { get; set; }
      

        // Hobbies

        public string UsoMP3 { get; set; }
        public string PracticaTiro { get; set; }
        public string Otros { get; set; }

        // Sintomas actuales

        public string Sordera { get; set; }
        public string Otalgia { get; set; }
        public string Acufenos { get; set; }
        public string SecrecionOtica { get; set; }
        public string Vertigos { get; set; }

        // Otoscopia

        public string OidoIzquierdo { get; set; }
        public string OidoDerecho { get; set; }

        // Dx automaticos
        public string DX_OIDO_DERECHO { get; set; }
        public string DX_OIDO_IZQUIERDO { get; set; }
        public string v_RecomendationName { get; set; }


        public byte[] b_Logo { get; set; }
        public string EmpresaPropietaria { get; set; }
        public string EmpresaPropietariaDireccion { get; set; }
        public string EmpresaPropietariaTelefono { get; set; }
        public string EmpresaPropietariaEmail { get; set; }

        public string v_ServiceComponentId { get; set; }
        public int intTypeEso { get; set; }
        public string i_TypeEso { get; set; }
        public string v_EsoTypeName { get; set; }
        public string v_WorkingOrganizationName { get; set; }
        public string v_FullWorkingOrganizationName { get; set; }

        public string MarcaAudiometria { get; set; }
        public string ModeloAudiometria { get; set; }
        public string CalibracionAudiometria { get; set; }

        public string TiempoTrabajo { get; set; }

        public string AUDIOMETRIA_MINERA_USA_PROT { get; set; }
        public string AUDIOMETRIA_MINERA_TIPO_PROT { get; set; }
        public string AUDIOMETRIA_MINERA_TIEMPO_PUESTO { get; set; }
        public string AUDIOMETRIA_MINERA_EXP_DIARIA { get; set; }
        public string AUDIOMETRIA_MINERA_EXPO_RUI { get; set; }
        public string AUDIOMETRIA_MINERA_FRE_EPP { get; set; }
        public string AUDIOMETRIA_MINERA_FUENTE { get; set; }
        public string AUDIOMETRIA_MINERA_NIVEL { get; set; }
        public string AUDIOMETRIA_MINERA_APRE_RUI { get; set; }



        public string AUDIOMETRIA_ANTECEDENTES_TEC { get; set; } // "N002-MF000000297";
        public string AUDIOMETRIA_ANTECEDENTES_RINITIS { get; set; } // "N002-MF000000288";
        public string AUDIOMETRIA_ANTECEDENTES_SINOSITIS { get; set; } // "N009-MF000001559";
        public string AUDIOMETRIA_ANTECEDENTES_SORDERA { get; set; } // "N002-MF000000295";
        public string AUDIOMETRIA_ANTECEDENTES_PAROTIDITIS { get; set; } // "N009-MF000001557";

        public string AUDIOMETRIA_ANTECEDENTES_MENINGITIS { get; set; } // "N002-MF000000290";
        public string AUDIOMETRIA_ANTECEDENTES_SARAMPION { get; set; } // "N002-MF000000294";
        public string AUDIOMETRIA_ANTECEDENTES_OTOTOXICO { get; set; } // "N002-MF000000291";
        public string AUDIOMETRIA_ANTECEDENTES_SORDERA_FAMILIAR { get; set; } // "N002-MF000000298";
        public string AUDIOMETRIA_ANTECEDENTES_DISLIPIDEMIA { get; set; } // "N002-MF000000293";

        public string AUDIOMETRIA_ANTECEDENTES_TRAUMA_ACUSTICO { get; set; } // "N009-MF000001583";
        public string AUDIOMETRIA_ANTECEDENTES_DIABETES_MELLITUS { get; set; } // "N002-MF000000292";
        public string AUDIOMETRIA_ANTECEDENTES_ENF_TIROIDEA { get; set; } // "N002-MF000000296";
        public string AUDIOMETRIA_ANTECEDENTES_OTITIS_MEDIA_CRONICA { get; set; } // "N002-MF000000289";

        public string AUDIOMETRIA_ANTECEDENTES_OTROS { get; set; } // "N009-MF000001560";

        public string AUDIOMETRIA_ANTECEDENTES_SUST_QUIMICAS { get; set; } // "N002-MF000000299";

        // Hobbies
        public string AUDIOMETRIA_HOBBIES_USO_MP3 { get; set; } // "N002-MF000000300";
        public string AUDIOMETRIA_HOBBIES_PRACTICA_TIRO { get; set; } // "N002-MF000000301";
        public string AUDIOMETRIA_HOBBIES_DISCOTECAS { get; set; } // "N002-MF000000302";
        public string AUDIOMETRIA_HOBBIES_SERVICIO_MILITAR { get; set; } // "N009-MF000001553";
        public string AUDIOMETRIA_HOBBIES_CONSUMO_TABACO { get; set; } // "N009-MF000001558";

        // Sintomas actuales
        public string AUDIOMETRIA_SINTOMAS_ACTUALES_SORDERA { get; set; } // "N002-MF000000303";
        public string AUDIOMETRIA_SINTOMAS_ACTUALES_ACUFENOS { get; set; } // "N002-MF000000304";
        public string AUDIOMETRIA_SINTOMAS_ACTUALES_VERTIGOS { get; set; } // "N002-MF000000305";
        public string AUDIOMETRIA_SINTOMAS_ACTUALES_OTALGIA { get; set; } // "N002-MF000000306";
        public string AUDIOMETRIA_SINTOMAS_ACTUALES_SECRECION_OTICA { get; set; } // "N002-MF000000307";
        public string AUDIOMETRIA_SINTOMAS_ACTUALES_OTROS { get; set; } // "N009-MF000001556";

        public byte[] LogoEmpresaCliente { get; set; }
        public string RazonSocialEmpresaCliente { get; set; }
        public string DxConcatenados { get; set; }

    }
}
