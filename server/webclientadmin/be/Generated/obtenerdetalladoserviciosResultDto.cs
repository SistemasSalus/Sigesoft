//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2020/07/24 - 14:14:30
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Sigesoft.Server.WebClientAdmin.BE
{
    [DataContract()]
    public partial class obtenerdetalladoserviciosResultDto
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
        public String RUCFACT { get; set; }

        [DataMember()]
        public String EMPFACT { get; set; }

        [DataMember()]
        public String PROTOCOLO { get; set; }

        [DataMember()]
        public String EXAMEN { get; set; }

        [DataMember()]
        public Nullable<Single> PRECIO { get; set; }

        [DataMember()]
        public String v_UserName { get; set; }

        [DataMember()]
        public String Estado { get; set; }

        [DataMember()]
        public String CompEstado { get; set; }

        [DataMember()]
        public String cdoc_Serie { get; set; }

        [DataMember()]
        public String cdoc_nro { get; set; }

        public obtenerdetalladoserviciosResultDto()
        {
        }

        public obtenerdetalladoserviciosResultDto(String v_ServiceId, Nullable<DateTime> d_ServiceDate, String v_DocNumber, String pACIENTE, String v_CurrentOccupation, Nullable<Int32> eDAD, String rUCCLI, String cLIENTE, String rUCFACT, String eMPFACT, String pROTOCOLO, String eXAMEN, Nullable<Single> pRECIO, String v_UserName, String estado, String compEstado, String cdoc_Serie, String cdoc_nro)
        {
			this.v_ServiceId = v_ServiceId;
			this.d_ServiceDate = d_ServiceDate;
			this.v_DocNumber = v_DocNumber;
			this.PACIENTE = pACIENTE;
			this.v_CurrentOccupation = v_CurrentOccupation;
			this.EDAD = eDAD;
			this.RUCCLI = rUCCLI;
			this.CLIENTE = cLIENTE;
			this.RUCFACT = rUCFACT;
			this.EMPFACT = eMPFACT;
			this.PROTOCOLO = pROTOCOLO;
			this.EXAMEN = eXAMEN;
			this.PRECIO = pRECIO;
			this.v_UserName = v_UserName;
			this.Estado = estado;
			this.CompEstado = compEstado;
			this.cdoc_Serie = cdoc_Serie;
			this.cdoc_nro = cdoc_nro;
        }
    }
}
