//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:33:14
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
    public partial class salus_comprobarresultadoslaboratorioResultDto
    {
        [DataMember()]
        public String v_ServiceId { get; set; }

        [DataMember()]
        public String TecnicoCovid { get; set; }

        [DataMember()]
        public String ComponentFieldId { get; set; }

        [DataMember()]
        public String ServiceComponentFieldsId { get; set; }

        [DataMember()]
        public String Value1 { get; set; }

        public salus_comprobarresultadoslaboratorioResultDto()
        {
        }

        public salus_comprobarresultadoslaboratorioResultDto(String v_ServiceId, String tecnicoCovid, String componentFieldId, String serviceComponentFieldsId, String value1)
        {
			this.v_ServiceId = v_ServiceId;
			this.TecnicoCovid = tecnicoCovid;
			this.ComponentFieldId = componentFieldId;
			this.ServiceComponentFieldsId = serviceComponentFieldsId;
			this.Value1 = value1;
        }
    }
}
