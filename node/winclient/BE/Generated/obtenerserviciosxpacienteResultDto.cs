//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:33:08
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    [DataContract()]
    public partial class obtenerserviciosxpacienteResultDto
    {
        [DataMember()]
        public String v_ServiceId { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_ServiceDate { get; set; }

        [DataMember()]
        public String v_DocNumber { get; set; }

        [DataMember()]
        public String PACIENTE { get; set; }

        [DataMember()]
        public String v_CurrentOccupation { get; set; }

        [DataMember()]
        public Nullable<Int32> EDAD { get; set; }

        [DataMember()]
        public String RUCCLI { get; set; }

        [DataMember()]
        public String CLIENTE { get; set; }

        [DataMember()]
        public String Sede { get; set; }

        [DataMember()]
        public String RUCFACT { get; set; }

        [DataMember()]
        public String EMPFACT { get; set; }

        [DataMember()]
        public String PROTOCOLO { get; set; }

        [DataMember()]
        public Nullable<Double> PRECTOTAL { get; set; }

        [DataMember()]
        public String v_UserName { get; set; }

        [DataMember()]
        public String Estado { get; set; }

        [DataMember()]
        public String cdoc_Serie { get; set; }

        [DataMember()]
        public String cdoc_nro { get; set; }

        [DataMember()]
        public String cusu_crea { get; set; }

        [DataMember()]
        public String v_comment { get; set; }

        public obtenerserviciosxpacienteResultDto()
        {
        }

        public obtenerserviciosxpacienteResultDto(String v_ServiceId, Nullable<DateTime> d_ServiceDate, String v_DocNumber, String pACIENTE, String v_CurrentOccupation, Nullable<Int32> eDAD, String rUCCLI, String cLIENTE, String sede, String rUCFACT, String eMPFACT, String pROTOCOLO, Nullable<Double> pRECTOTAL, String v_UserName, String estado, String cdoc_Serie, String cdoc_nro, String cusu_crea, String v_comment)
        {
			this.v_ServiceId = v_ServiceId;
			this.d_ServiceDate = d_ServiceDate;
			this.v_DocNumber = v_DocNumber;
			this.PACIENTE = pACIENTE;
			this.v_CurrentOccupation = v_CurrentOccupation;
			this.EDAD = eDAD;
			this.RUCCLI = rUCCLI;
			this.CLIENTE = cLIENTE;
			this.Sede = sede;
			this.RUCFACT = rUCFACT;
			this.EMPFACT = eMPFACT;
			this.PROTOCOLO = pROTOCOLO;
			this.PRECTOTAL = pRECTOTAL;
			this.v_UserName = v_UserName;
			this.Estado = estado;
			this.cdoc_Serie = cdoc_Serie;
			this.cdoc_nro = cdoc_nro;
			this.cusu_crea = cusu_crea;
			this.v_comment = v_comment;
        }
    }
}
