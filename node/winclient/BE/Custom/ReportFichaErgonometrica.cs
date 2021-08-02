using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class ReportFichaErgonometrica
    {
        public DateTime? FechaNacimiento { get; set; }
        public string Ficha { get; set; }
        public string HistoriaClinica { get; set; }
        public string DatoPaciente { get; set; }
        public int Edad { get; set; }
        public string Genero { get; set; }
        public string v_FullPersonName { get; set; }
        public DateTime? d_BirthDate { get; set; }
        public DateTime? d_ServiceDate { get; set; }
        public string v_ServiceDate { get; set; }
        public byte[] FirmaTecnologo { get; set; }
        public byte[] FirmaMedico { get; set; }
        public string Dx { get; set; }
        public string NombreTecnico { get; set; }
        public string NombreDoctor { get; set; }   
        public byte[] b_Logo { get; set; }
        public string EmpresaPropietaria { get; set; }
        public string EmpresaPropietariaDireccion { get; set; }
        public string EmpresaPropietariaTelefono { get; set; }
        public string EmpresaPropietariaEmail { get; set; }


        // Campos
        public string DiagnosticoClinico { get; set; }
        public string Medicacion { get; set; }
        public string EKGBasal { get; set; }
        public string Metodo { get; set; }
        public string Protocolo { get; set; }
        public string FCMax { get; set; }
        public string FCSubMax { get; set; }
        public string PAInicial { get; set; }
        public string FCInicial { get; set; }

        public string Etapa1MIN { get; set; }
        public string Etapa1PA { get; set; }
        public string Etapa1FC { get; set; }
        public string Etapa1ST { get; set; }
        public string Etapa1Arritmia { get; set; }
        public string Etapa1Sintomas { get; set; }

        public string Etapa2MIN { get; set; }
        public string Etapa2PA { get; set; }
        public string Etapa2FC { get; set; }
        public string Etapa2ST { get; set; }
        public string Etapa2Arritmia { get; set; }
        public string Etapa2Sintomas { get; set; }

        public string Etapa3MIN { get; set; }
        public string Etapa3PA { get; set; }
        public string Etapa3FC { get; set; }
        public string Etapa3ST { get; set; }
        public string Etapa3Arritmia { get; set; }
        public string Etapa3Sintomas { get; set; }

        public string Etapa4MIN { get; set; }
        public string Etapa4PA { get; set; }
        public string Etapa4FC { get; set; }
        public string Etapa4ST { get; set; }
        public string Etapa4Arritmia { get; set; }
        public string Etapa4Sintomas { get; set; }

        public string Etapa5MIN { get; set; }
        public string Etapa5PA { get; set; }
        public string Etapa5FC { get; set; }
        public string Etapa5ST { get; set; }
        public string Etapa5Arritmia { get; set; }
        public string Etapa5Sintomas { get; set; }


        public string EtapaRecuperacion1MIN { get; set; }
        public string EtapaRecuperacion1PA { get; set; }
        public string EtapaRecuperacion1FC { get; set; }
        public string EtapaRecuperacion1ST { get; set; }
        public string EtapaRecuperacion1Arritmia { get; set; }
        public string EtapaRecuperacion1Sintomas { get; set; }

        public string EtapaRecuperacion2MIN { get; set; }
        public string EtapaRecuperacion2PA { get; set; }
        public string EtapaRecuperacion2FC { get; set; }
        public string EtapaRecuperacion2ST { get; set; }
        public string EtapaRecuperacion2Arritmia { get; set; }
        public string EtapaRecuperacion2Sintomas { get; set; }

        public string EtapaRecuperacion3MIN { get; set; }
        public string EtapaRecuperacion3PA { get; set; }
        public string EtapaRecuperacion3FC { get; set; }
        public string EtapaRecuperacion3ST { get; set; }
        public string EtapaRecuperacion3Arritmia { get; set; }
        public string EtapaRecuperacion3Sintomas { get; set; }

        public string Observaciones { get; set; }
        public string CapacidadFuncional { get; set; }
        public string RespuestaPresora { get; set; }
        public string RespuestaCronotropica { get; set; }
        public string RespuestaIsquemica { get; set; }
        public string Sugerencias { get; set; }




     
    }
}
