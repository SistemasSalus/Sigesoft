using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class OftalmologiaList
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
   
        public bool ExploracionClinicaEstaEnProtolo { get; set; }
        public string ExploracionClinicaCorrectoresOcularesVisionLejos { get; set; }
        public string ExploracionClinicaCorrectoresOcularesVisionCerca { get; set; }
        public string ExploracionClinicaMovimientosOculares_OD { get; set; }
        public string ExploracionClinicaMovimientosOculares_OI { get; set; }
        public string ExploracionClinicaHallazgosEstrabismo { get; set; }
        public string ExploracionClinicaHallazgosChalazion { get; set; }
        public string ExploracionClinicaHallazgosOrzuelo { get; set; }
        public string ExploracionClinicaHallazgosPinguecula { get; set; }
        public string ExploracionClinicaHallazgosPterigion { get; set; }
        public string ExploracionClinicaHallazgosSecuelaTrauma { get; set; }
        public string ExploracionClinicaHallazgosCatarata { get; set; }
        public string ExploracionClinicaHallazgosPseudofaquia { get; set; }
        public string ExploracionClinicaHallazgosOtros { get; set; }
        public string ExploracionClinicaDescripcionHallazgos { get; set; }

        public bool VisionColoresEstaEnProtolo { get; set; }
        public string VisionColoresTestIshiharaSeleccionar { get; set; }
        public string VisionColoresTestIshiharaDescripcion { get; set; }
        public string VisionColoresPercepcionColoresBasicosRojo_OD { get; set; }
        public string VisionColoresPercepcionColoresBasicosRojo_OI { get; set; }
        public string VisionColoresPercepcionColoresBasicosVerde_OD { get; set; }
        public string VisionColoresPercepcionColoresBasicosVerde_OI { get; set; }
        public string VisionColoresPercepcionColoresBasicosAzul_OD { get; set; }
        public string VisionColoresPercepcionColoresBasicosAzul_OI { get; set; }
        public string VisionColoresPercepcionColoresBasicosAmarillo_OD { get; set; }
        public string VisionColoresPercepcionColoresBasicosAmarillo_OI { get; set; }
        public string VisionColoresPercepcionColoresBasicosBlanco_OD { get; set; }
        public string VisionColoresPercepcionColoresBasicosBlanco_OI { get; set; }
        public string VisionColoresPercepcionColoresBasicosNegro_OD { get; set; }
        public string VisionColoresPercepcionColoresBasicosNegro_OI { get; set; }

        public bool VisionEstereoscopicaEstaEnProtolo { get; set; }
        public string VisionEstereoscopicaTestAnillosSeleccionar { get; set; }
        public string VisionEstereoscopicaTestAnillosDescripcion { get; set; }

        public bool CampoVisualEstaEnProtolo { get; set; }
        public string CampoVisualCampimetriaSeleccionar_OD { get; set; }
        public string CampoVisualCampimetriaSeleccionar_OI { get; set; }
        public string CampoVisualCampimetriaDescripcion_OD { get; set; }
        public string CampoVisualCampimetriaDescripcion_OI { get; set; }

        public bool PresionIntraocularEstaEnProtolo { get; set; }
        public string PresionIntraocularTonometria_OD { get; set; }
        public string PresionIntraocularTonometria_OI { get; set; }

        public bool FondoOjoEstaEnProtolo { get; set; }
        public string FondoOjoOftalmoscopiaVitreo_OD { get; set; }
        public string FondoOjoOftalmoscopiaVitreo_OI { get; set; }
        public string FondoOjoOftalmoscopiaMacula_OD { get; set; }
        public string FondoOjoOftalmoscopiaMacula_OI { get; set; }
        public string FondoOjoOftalmoscopiaRetina_OD { get; set; }
        public string FondoOjoOftalmoscopiaRetina_OI { get; set; }
        public string FondoOjoOftalmoscopiaNervioOptico_OD { get; set; }
        public string FondoOjoOftalmoscopiaNervioOptico_OI { get; set; }
        public string FondoOjoDescripcion { get; set; }

        public bool RefraccionEstaEnProtolo { get; set; }
        public string RefraccionLejosEsfera_OD { get; set; }
        public string RefraccionLejosEsfera_OI { get; set; }
        public string RefraccionLejosCilindro_OD { get; set; }
        public string RefraccionLejosCilindro_OI { get; set; }
        public string RefraccionLejosEje_OD { get; set; }
        public string RefraccionLejosEje_OI { get; set; }
        public string RefraccionCercaAddMas { get; set; }

        public bool AgudezaVisualEstaEnProtolo { get; set; }
        public string AgudezaVisualLejos_SC_OD { get; set; }
        public string AgudezaVisualLejos_SC_OI { get; set; }
        public string AgudezaVisualLejos_CC_OD { get; set; }
        public string AgudezaVisualLejos_CC_OI { get; set; }

        public string AgudezaVisualLejos_AE_OD { get; set; }
        public string AgudezaVisualLejos_AE_OI { get; set; }

        public string AgudezaVisualCerca_SC_OD { get; set; }
        public string AgudezaVisualCerca_SC_OI { get; set; }
        public string AgudezaVisualCerca_CC_OD { get; set; }
        public string AgudezaVisualCerca_CC_OI { get; set; }

        public byte[] b_Logo { get; set; }
        public string EmpresaPropietaria { get; set; }
        public string EmpresaPropietariaDireccion { get; set; }
        public string EmpresaPropietariaTelefono { get; set; }
        public string EmpresaPropietariaEmail { get; set; }

        public string Diagnosticos { get; set; }
        public string Recomendaciones { get; set; }
        
        //
        public string CC2CercaOD { get; set; }
        public string CC2CercaOI { get; set; }
        public string AgudezaVisualFIGURAS { get; set; }

    }
}
