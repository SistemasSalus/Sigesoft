//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:33:13
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
    public partial class regcovid_servicios_vs_resultadosResultDto
    {
        [DataMember()]
        public String column0 { get; set; }

        [DataMember()]
        public String v_DocNumber { get; set; }

        [DataMember()]
        public String Value1 { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_ServiceDate { get; set; }

        [DataMember()]
        public Nullable<Int32> ValueSede { get; set; }

        [DataMember()]
        public String v_Sede { get; set; }

        [DataMember()]
        public String v_ServiceId { get; set; }

        [DataMember()]
        public Nullable<DateTime> d_InsertDate { get; set; }

        [DataMember()]
        public String TecnicoCovid { get; set; }

        [DataMember()]
        public String ComponentFieldId { get; set; }

        [DataMember()]
        public String ServiceComponentFieldsId { get; set; }

        [DataMember()]
        public Nullable<Int32> CorreoEnviado { get; set; }

        public regcovid_servicios_vs_resultadosResultDto()
        {
        }

        public regcovid_servicios_vs_resultadosResultDto(String column0, String v_DocNumber, String value1, Nullable<DateTime> d_ServiceDate, Nullable<Int32> valueSede, String v_Sede, String v_ServiceId, Nullable<DateTime> d_InsertDate, String tecnicoCovid, String componentFieldId, String serviceComponentFieldsId, Nullable<Int32> correoEnviado)
        {
			this.column0 = column0;
			this.v_DocNumber = v_DocNumber;
			this.Value1 = value1;
			this.d_ServiceDate = d_ServiceDate;
			this.ValueSede = valueSede;
			this.v_Sede = v_Sede;
			this.v_ServiceId = v_ServiceId;
			this.d_InsertDate = d_InsertDate;
			this.TecnicoCovid = tecnicoCovid;
			this.ComponentFieldId = componentFieldId;
			this.ServiceComponentFieldsId = serviceComponentFieldsId;
			this.CorreoEnviado = correoEnviado;
        }
    }
}