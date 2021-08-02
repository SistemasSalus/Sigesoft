using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class ReportMusculoEsqueletico1
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

        // Campos

        public string NucaDolorUltimo12Meses { get; set; }
        public string NucaIncapacitadoUltimo12Meses { get; set; }
        public string NucaDolorUltimo7Dias { get; set; }
        public string HombroDerechoDolorUltimo12Meses { get; set; }
        public string HombroDerechoIncapacitadoUltimo12Meses { get; set; }
        public string HombroDerechoDolorUltimo7Dias { get; set; }
        public string HombroIzquierdoDolorUltimo12Meses { get; set; }
        public string HombroIzquierdoIncapacitadoUltimo12Meses { get; set; }
        public string HombroIzquierdoDolorUltimo7Dias { get; set; }

        public string HombroAmbosUltimo12Meses { get; set; }
        public string HombroAmbosIncapacitadoUltimo12Meses { get; set; }
        public string HombroAmbosDolorUltimo7Dias { get; set; }

        public string CodoDerechoDolorUltimo12Meses { get; set; }
        public string CodoDerechoIncapacitadoUltimo12Meses { get; set; }
        public string CodoDerechoDolorUltimo7Dias { get; set; }

        public string CodoIzquierdoDolorUltimo12Meses { get; set; }
        public string CodoIzquierdoIncapacitadoUltimo12Meses { get; set; }
        public string CodoIzquierdoDolorUltimo7Dias { get; set; }

        public string CodoAmbosUltimo12Meses { get; set; }
        public string CodoAmbosIncapacitadoUltimo12Meses { get; set; }
        public string CodoAmbosDolorUltimo7Dias { get; set; }

        public string PuñosManosDerechoDolorUltimo12Meses { get; set; }
        public string PuñosManosDerechoIncapacitadoUltimo12Meses { get; set; }
        public string PuñosManosDerechoDolorUltimo7Dias { get; set; }

        public string PuñosManosIzquierdoDolorUltimo12Meses { get; set; }
        public string PuñosManosIzquierdoIncapacitadoUltimo12Meses { get; set; }
        public string PuñosManosIzquierdoDolorUltimo7Dias { get; set; }

        public string PuñosManosAmbosDolorUltimo12Meses { get; set; }
        public string PuñosManosAmbosIncapacitadoUltimo12Meses { get; set; }
        public string PuñosManosAmbosDolorUltimo7Dias { get; set; }

        public string ColumnaAltaDolorUltimo12Meses { get; set; }
        public string ColumnaAltaIncapacitadoUltimo12Meses { get; set; }
        public string ColumnaAltaDolorUltimo7Dias { get; set; }

        public string ColumnaBajaDolorUltimo12Meses { get; set; }
        public string ColumnaBajaIncapacitadoUltimo12Meses { get; set; }
        public string ColumnaBajaDolorUltimo7Dias { get; set; }

        public string CaderasDerechoDolorUltimo12Meses { get; set; }
        public string CaderasDerechoIncapacitadoUltimo12Meses { get; set; }
        public string CaderasDerechoDolorUltimo7Dias { get; set; }

        public string CaderasIzquierdoDolorUltimo12Meses { get; set; }
        public string CaderasIzquierdoIncapacitadoUltimo12Meses { get; set; }
        public string CaderasIzquierdoDolorUltimo7Dias { get; set; }

        public string RodillasDerechoDolorUltimo12Meses { get; set; }
        public string RodillasDerechoIncapacitadoUltimo12Meses { get; set; }
        public string RodillasDerechoDolorUltimo7Dias { get; set; }

        public string RodillasIzquierdoDolorUltimo12Meses { get; set; }
        public string RodillasIzquierdoIncapacitadoUltimo12Meses { get; set; }
        public string RodillasIzquierdoDolorUltimo7Dias { get; set; }

        public string TobillosPiesDerechoDolorUltimo12Meses { get; set; }
        public string TobillosPiesDerechoIncapacitadoUltimo12Meses { get; set; }
        public string TobillosPiesDerechoDolorUltimo7Dias { get; set; }

        public string TobillosPiesIzquierdoDolorUltimo12Meses { get; set; }
        public string TobillosPiesIzquierdoIncapacitadoUltimo12Meses { get; set; }
        public string TobillosPiesIzquierdoDolorUltimo7Dias { get; set; }

        public string ColumnaVertebralExploracionLasengueDerecho { get; set; }
        public string ColumnaVertebralExploracionLasengueIzquierdo { get; set; }
        public string ColumnaVertebralExploracionTestSchoberDerecho { get; set; }
        public string ColumnaVertebralExploracionTestSchoberIzquierdo { get; set; }

        public string ExtremidadesSuperioresInferioresTestPhalenDerecho { get; set; }
        public string ExtremidadesSuperioresInferioresTestPhalenIzquierdo { get; set; }
        public string ExtremidadesSuperioresInferioresTestTinelDerecho { get; set; }
        public string ExtremidadesSuperioresInferioresTestTinelIzquierdo { get; set; }

        public string ExtremidadesSuperioresInferioresCodoDerecho { get; set; }
        public string ExtremidadesSuperioresInferioresCodoIzquierdo { get; set; }

        public string ExtremidadesSuperioresInferioresPieDerecho { get; set; }
        public string ExtremidadesSuperioresInferioresPieIzquierdo { get; set; }

        public string CervicalReposoDerecho { get; set; }
        public string CervicalReposoDolorPalpacion { get; set; }
        public string CervicalReposoIzquierdo { get; set; }
        public string CervicalReposoDolorMovimiento { get; set; }

        public string CervicalFlexion40Derecho { get; set; }
        public string CervicalFlexion40DolorPalpacion { get; set; }
        public string CervicalFlexion40Izquierdo { get; set; }
        public string CervicalFlexion40DolorMovimiento { get; set; }


        public string Protocolo { get; set; }
        public string v_EsoTypeName { get; set; }
        public string v_ServiceComponentId { get; set; }
        public string v_ServiceId { get; set; }



        public string EmpresaPropietaria { get; set; }

        public string EmpresaPropietariaDireccion { get; set; }

        public string EmpresaPropietariaTelefono { get; set; }

        public string EmpresaPropietariaEmail { get; set; }

        public string DetalleAntecEncontrados { get; set; }
    }
}
