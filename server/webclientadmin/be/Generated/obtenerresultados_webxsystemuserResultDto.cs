//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2020/07/24 - 14:14:32
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
    public partial class obtenerresultados_webxsystemuserResultDto
    {
        [DataMember()]
        public String v_ServiceId { get; set; }

        [DataMember()]
        public String v_PersonId { get; set; }

        [DataMember()]
        public String Paciente { get; set; }

        [DataMember()]
        public Nullable<DateTime> Fecha { get; set; }

        [DataMember()]
        public String v_Name { get; set; }

        [DataMember()]
        public String v_ProtocolId { get; set; }

        [DataMember()]
        public String Protocolo { get; set; }

        [DataMember()]
        public String ESTADO { get; set; }

        [DataMember()]
        public String PENDIENTE { get; set; }

        public obtenerresultados_webxsystemuserResultDto()
        {
        }

        public obtenerresultados_webxsystemuserResultDto(String v_ServiceId, String v_PersonId, String paciente, Nullable<DateTime> fecha, String v_Name, String v_ProtocolId, String protocolo, String eSTADO, String pENDIENTE)
        {
			this.v_ServiceId = v_ServiceId;
			this.v_PersonId = v_PersonId;
			this.Paciente = paciente;
			this.Fecha = fecha;
			this.v_Name = v_Name;
			this.v_ProtocolId = v_ProtocolId;
			this.Protocolo = protocolo;
			this.ESTADO = eSTADO;
			this.PENDIENTE = pENDIENTE;
        }
    }
}
